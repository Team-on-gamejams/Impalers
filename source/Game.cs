using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Impalers {
	public class Game {
		public enum GameOverResult : byte { WinPlayer, WinEnemy, Draw, None }
		public enum Enemy : byte { Player, Bot }
		public Enemy enemy;

		public GameCell[,] map = new GameCell[Settings.sizeY, Settings.sizeX];
		public bool isPlayerTurn;
		public ushort scorePlayer, scoreEnemy;
		public ushort counterMeatPlayer, counterMeatEnemy, counterImpalePlayer, counterImpaleEnemy;

		public Grid TopGrid { get; set; }
		public Grid BottomGrid { get; set; }
		public Grid LeftGrid { get; set; }
		public Grid RightGrid { get; set; }

		List<Tuple<byte, byte, byte, byte>> avaliableImpales = new List<Tuple<byte, byte, byte, byte>>(10);

		public Game() {
			for(byte i = 0; i < Settings.sizeY; ++i)
				for(byte j = 0; j < Settings.sizeX; ++j)
					map[i, j] = new GameCell();
		}

		public void Start(Enemy enemy) {
			this.enemy = enemy;
			for(byte i = 0; i < Settings.sizeY; ++i)
				for(byte j = 0; j < Settings.sizeX; ++j)
					map[i, j].Init();
			isPlayerTurn = true;
			scorePlayer = scoreEnemy = 0;
		}

		public void PlaceMeat(byte x, byte y) {
			map[y, x].isMeat = true;

			byte randomImageNumber;
			do {
				randomImageNumber = (byte) Singletons.rand.Next(1, 11);
			} while(randomImageNumber == (y + 1 == Settings.sizeY ? 255 : map[y + 1, x].imageNumber) ||
				randomImageNumber == (y - 1 == -1 ? 255 : map[y - 1, x].imageNumber) ||
				randomImageNumber == (x + 1 == Settings.sizeX ? 255 : map[y, x + 1].imageNumber) ||
				randomImageNumber == (x - 1 == -1 ? 255 : map[y, x - 1].imageNumber) ||

				randomImageNumber == (x - 1 == -1 || y - 1 == -1 ? 255 : map[y - 1, x - 1].imageNumber) ||
				randomImageNumber == (x + 1 == Settings.sizeX || y - 1 == -1 ? 255 : map[y - 1, x + 1].imageNumber) ||
				randomImageNumber == (x - 1 == -1 || y + 1 == Settings.sizeY ? 255 : map[y + 1, x - 1].imageNumber) ||
				randomImageNumber == (x + 1 == Settings.sizeX || y + 1 == Settings.sizeY ? 255 : map[y + 1, x + 1].imageNumber)
			);

			map[y, x].imageNumber = randomImageNumber;
			map[y, x].imageMeat.Source = new BitmapImage(new Uri("Resources/Meat/" + randomImageNumber.ToString() + ".png", UriKind.Relative));

			if(Settings.useAnimation) {
				map[y, x].imageMeat.BeginAnimation(Image.HeightProperty,
					new DoubleAnimation {
						From = 25,
						To = 100,
						Duration = new Duration(TimeSpan.FromSeconds(0.5))
					}
				);
			}

			//System.Timers.Timer t = new System.Timers.Timer() {Interval = 100, AutoReset=true};
			//t.Elapsed += (a, b)=> {
			//	System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
			//		map[y, x].imageMeat.Opacity += 0.2;
			//		if (map[y, x].imageMeat.Opacity >= 1) {
			//			map[y, x].imageMeat.Opacity = 1;
			//			t.Stop();
			//		}
			//	});
			//};
			//t.Start();

			if(isPlayerTurn) {
				++scorePlayer;
				++counterMeatPlayer;
			}
			else {
				++scoreEnemy;
				++counterMeatEnemy;
			}
			isPlayerTurn = !isPlayerTurn;
		}

		public void PlaceStick(byte startX, byte startY, byte endX, byte endY) {
			short dx = (short) (startX - endX), dy = (short) (startY - endY);
			if(dx == 0 || dy == 0) {
				for(byte i = Math.Min(startX, endX); i <= Math.Max(startX, endX); ++i)
					if(!map[startY, i].isMeat || map[startY, i].isStickStart || map[startY, i].isStickBody || (i != endX && map[startY, i].isStickEnd))
						return;
				for(byte i = Math.Min(startY, endY); i <= Math.Max(startY, endY); ++i)
					if(!map[i, startX].isMeat || map[i, startX].isStickStart || map[i, startX].isStickBody || (i != endY && map[i, startX].isStickEnd))
						return;

				//MessageBox.Show($"{dx} {dy}");

				ushort lengthImpale = (ushort) (Math.Max(Math.Abs(dy), Math.Abs(dx)) + 1);
				if(lengthImpale >= 3) {


					if(isPlayerTurn) {
						scorePlayer += (ushort) ((lengthImpale - 2) * 2);
						++counterImpalePlayer;
					}
					else {
						scoreEnemy += (ushort) ((lengthImpale - 2) * 2);
						++counterImpaleEnemy;
					}

					string styleNum = (Singletons.rand.Next(0, 2) == 1 ? "1" : "2");

					//Down impale
					if(dy < 0 && (startY == 0 || map[startY - 1, startX].IsEmpty())) {
						if(startY != 0) {
							map[startY - 1, startX].imageStick.Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
								'/' + styleNum + "Start.png", Rotation.Rotate90);
							map[startY - 1, startX].isStickStart = true;
							map[startY - 1, startX].imageStick.VerticalAlignment = VerticalAlignment.Stretch;
						}
						else {
							(TopGrid.Children[startX] as Image).Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
								'/' + styleNum + "Start.png", Rotation.Rotate90);
						}

						while(dy++ != 0) {
							map[startY - dy, startX].imageStick.Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
								'/' + styleNum + "Body.png", Rotation.Rotate90);
							map[startY - dy, startX].isStickBody = true;
							map[startY - dy, startX].imageStick.VerticalAlignment = VerticalAlignment.Stretch;
						}

						Image img = map[endY, endX].imageStick;
						if(img.Source != null)
							for(byte i = 0; i < 3; ++i)
								if(map[endY, endX].imageStickEnd[i].Source == null) {
									img = map[endY, endX].imageStickEnd[i];
									break;
								}
						map[endY, endX].isStickEnd = true;
						img.Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
									'/' + styleNum + "End.png", Rotation.Rotate90);
						img.VerticalAlignment = VerticalAlignment.Stretch;
					}

					//Up impale
					else if(dy > 0 && (startY == Settings.sizeY - 1 || map[startY + 1, startX].IsEmpty())) {
						if(startY != Settings.sizeY - 1) {
							map[startY + 1, startX].imageStick.Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
								'/' + styleNum + "Start.png", Rotation.Rotate90);
							map[startY + 1, startX].imageStick.RenderTransform = new ScaleTransform(1, -1);
							map[startY + 1, startX].isStickStart = true;
							map[startY + 1, startX].imageStick.VerticalAlignment = VerticalAlignment.Stretch;
						}
						else {
							(BottomGrid.Children[startX] as Image).Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
								'/' + styleNum + "Start.png", Rotation.Rotate90);
						}

						while(dy-- != 0) {
							map[startY - dy, startX].imageStick.Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
								'/' + styleNum + "Body.png", Rotation.Rotate90);
							map[startY - dy, startX].isStickBody = true;
							map[startY - dy, startX].imageStick.VerticalAlignment = VerticalAlignment.Stretch;
							map[startY - dy, startX].imageStick.RenderTransform = new ScaleTransform(1, -1);
						}

						Image img = map[endY, endX].imageStick;
						if(img.Source != null)
							for(byte i = 0; i < 3; ++i)
								if(map[endY, endX].imageStickEnd[i].Source == null) {
									img = map[endY, endX].imageStickEnd[i];
									break;
								}
						map[endY, endX].isStickEnd = true;
						img.Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
									'/' + styleNum + "End.png", Rotation.Rotate90);
						img.VerticalAlignment = VerticalAlignment.Stretch;
						img.RenderTransform = new ScaleTransform(1, -1);
					}

					//Left impale
					else if(dx > 0 && (startX == Settings.sizeX - 1 || map[startY, startX + 1].IsEmpty())) {
						if(startX != Settings.sizeX - 1) {
							map[startY, startX + 1].imageStick.Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
								'/' + styleNum + "Start.png", Rotation.Rotate0);
							map[startY, startX + 1].imageStick.RenderTransform = new ScaleTransform(-1, 1);
							map[startY, startX + 1].isStickStart = true;
							map[startY, startX + 1].imageStick.HorizontalAlignment = HorizontalAlignment.Stretch;
						}
						else {
							(RightGrid.Children[startY] as Image).Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
								'/' + styleNum + "Start.png", Rotation.Rotate0);
						}

						while(dx-- != 0) {
							map[startY, startX - dx].imageStick.Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
								'/' + styleNum + "Body.png", Rotation.Rotate0);
							map[startY, startX - dx].isStickBody = true;
							map[startY, startX - dx].imageStick.HorizontalAlignment = HorizontalAlignment.Stretch;
							map[startY, startX - dx].imageStick.RenderTransform = new ScaleTransform(-1, 1);
						}

						Image img = map[endY, endX].imageStick;
						if(img.Source != null)
							for(byte i = 0; i < 3; ++i)
								if(map[endY, endX].imageStickEnd[i].Source == null) {
									img = map[endY, endX].imageStickEnd[i];
									break;
								}
						map[endY, endX].isStickEnd = true;
						img.Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
									'/' + styleNum + "End.png", Rotation.Rotate0);
						img.HorizontalAlignment = HorizontalAlignment.Stretch;
						img.RenderTransform = new ScaleTransform(-1, 1);
					}

					//Right impale
					else if(dx < 0 && (startX == 0 || map[startY, startX - 1].IsEmpty())) {
						if(startX != 0) {
							map[startY, startX - 1].imageStick.Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
								'/' + styleNum + "Start.png", Rotation.Rotate0);
							map[startY, startX - 1].isStickStart = true;
							map[startY, startX - 1].imageStick.HorizontalAlignment = HorizontalAlignment.Stretch;
						}
						else {
							(LeftGrid.Children[startY] as Image).Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
								'/' + styleNum + "Start.png", Rotation.Rotate0);
						}

						while(dx++ != 0) {
							map[startY, startX - dx].imageStick.Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
								'/' + styleNum + "Body.png", Rotation.Rotate0);
							map[startY, startX - dx].isStickBody = true;
							map[startY, startX - dx].imageStick.HorizontalAlignment = HorizontalAlignment.Stretch;
						}

						Image img = map[endY, endX].imageStick;
						if(img.Source != null)
							for(byte i = 0; i < 3; ++i)
								if(map[endY, endX].imageStickEnd[i].Source == null) {
									img = map[endY, endX].imageStickEnd[i];
									break;
								}
						map[endY, endX].isStickEnd = true;
						img.Source = CreateBitmapImage("Resources/Pl" + (isPlayerTurn ? '1' : '2') +
									'/' + styleNum + "End.png", Rotation.Rotate0);
						img.HorizontalAlignment = HorizontalAlignment.Stretch;
					}

					isPlayerTurn = !isPlayerTurn;
				}

			}

			BitmapImage CreateBitmapImage(string uri, Rotation rotation) {
				BitmapImage bi = new BitmapImage();
				bi.BeginInit();
				bi.UriSource = new Uri(uri, UriKind.Relative);
				bi.Rotation = rotation;
				bi.EndInit();
				return bi;
			}
		}

		public void BotTurn() {
			if(!(Impale() || PlaceInChessOrder(false) || PlaceInChessOrder(true) || PlaceInEmptyNeightborns() || PlaceOnAnyPos()))
				MessageBox.Show("Error in BotTurn(). Impossible if worked", "Never reached code");

			bool Impale() {
				avaliableImpales.Clear();
				byte lastImpLength = 0;

				for(byte x = 0; x < Settings.sizeX; ++x) {
					for(byte y = 0; y < Settings.sizeY; ++y) {
						if(map[y, x].isMeat) {
							if(x + 1 == Settings.sizeX || map[y, x + 1].IsEmpty()) {
								lastImpLength = CheckImpaleLeft(x, y);
								if(lastImpLength != 0)
									avaliableImpales.Add(new Tuple<byte, byte, byte, byte>(x, y, 0, lastImpLength));
							}

							if(y + 1 == Settings.sizeY || map[y + 1, x].IsEmpty()) {
								lastImpLength = CheckImpaleUp(x, y);
								if(lastImpLength != 0)
									avaliableImpales.Add(new Tuple<byte, byte, byte, byte>(x, y, 1, lastImpLength));
							}

							if(x == 0 || map[y, x - 1].IsEmpty()) {
								lastImpLength = CheckImpaleRight(x, y);
								if(lastImpLength != 0)
									avaliableImpales.Add(new Tuple<byte, byte, byte, byte>(x, y, 2, lastImpLength));
							}

							if(y == 0 || map[y - 1, x].IsEmpty()) {
								lastImpLength = CheckImpaleDown(x, y);
								if(lastImpLength != 0)
									avaliableImpales.Add(new Tuple<byte, byte, byte, byte>(x, y, 3, lastImpLength));
							}
						}
					}
				}

				if(avaliableImpales.Count != 0) {
					avaliableImpales.Sort((a, b) => b.Item4 - a.Item4);
					var i = avaliableImpales[0];

					if(i.Item3 == 0) {
						MessageBox.Show(string.Format("{0} {1} {2} {3}", i.Item1, i.Item2, (byte) (i.Item1 - i.Item4), i.Item2));
						PlaceStick(i.Item1, i.Item2, (byte) (i.Item1 - i.Item4), i.Item2);
					}
					else if(i.Item3 == 2)
						PlaceStick(i.Item1, i.Item2, (byte) (i.Item1 + i.Item4), i.Item2);
					else if(i.Item3 == 1)
						PlaceStick(i.Item1, i.Item2, i.Item1, (byte) (i.Item2 - i.Item4));
					else if(i.Item3 == 3)
						PlaceStick(i.Item1, i.Item2, i.Item1, (byte) (i.Item2 + i.Item4));
				}

				return isPlayerTurn;

				byte CheckImpaleLeft(byte x, byte y) {
					int X = x;

					byte len = 0;
					while(--X != -1) {
						--x;
						if(!map[y, x].isMeat || map[y, x].isStickBody || map[y, x].isStickStart)
							break;
						++len;
						if(map[y, x].isStickEnd)
							break;
					}

					if(len < 2)
						len = 0;
					return len;
				}

				byte CheckImpaleRight(byte x, byte y) {
					return 0;
				}

				byte CheckImpaleUp(byte x, byte y) {
					return 0;
				}

				byte CheckImpaleDown(byte x, byte y) {
					return 0;
				}
			}

			bool PlaceInChessOrder(bool useBorder) {
				List<Tuple<byte, byte>> pos = new List<Tuple<byte, byte>>();
				for(byte x = 0; x < Settings.sizeX; ++x) {
					for(byte y = 0; y < Settings.sizeY; ++y) {
						if(
							map[y, x].IsEmpty() &&
							(y - 1 != -1 ? map[y - 1, x].IsEmpty() : true) &&
							(y + 1 != Settings.sizeY ? map[y + 1, x].IsEmpty() : true) &&
							(x - 1 != -1 ? map[y, x - 1].IsEmpty() : true) &&
							(x + 1 != Settings.sizeX ? map[y, x + 1].IsEmpty() : true)
						) {
							byte diagCnt = 0;
							if((y - 1 != -1 && x - 1 != -1 ? !map[y - 1, x - 1].IsEmpty() : useBorder))
								++diagCnt;
							if((y + 1 != Settings.sizeY && x - 1 != -1 ? !map[y + 1, x - 1].IsEmpty() : useBorder))
								++diagCnt;
							if((y - 1 != -1 && x + 1 != Settings.sizeX ? !map[y - 1, x + 1].IsEmpty() : useBorder))
								++diagCnt;
							if((y + 1 != Settings.sizeY && x + 1 != Settings.sizeX ? !map[y + 1, x + 1].IsEmpty() : useBorder))
								++diagCnt;
							if(diagCnt != 0)
								pos.Add(new Tuple<byte, byte>(x, y));
						}
					}
				}

				if(pos.Count == 0)
					return false;

				ushort randId = (ushort) Singletons.rand.Next(0, pos.Count);
				PlaceMeat(pos[randId].Item1, pos[randId].Item2);

				return true;
			}

			bool PlaceInEmptyNeightborns() {
				List<Tuple<byte, byte>> pos = new List<Tuple<byte, byte>>();
				for(byte x = 0; x < Settings.sizeX; ++x) {
					for(byte y = 0; y < Settings.sizeY; ++y) {
						if(
							map[y, x].IsEmpty() &&
							(y - 1 != -1 ? map[y - 1, x].IsEmpty() : true) &&
							(y + 1 != Settings.sizeY ? map[y + 1, x].IsEmpty() : true) &&
							(x - 1 != -1 ? map[y, x - 1].IsEmpty() : true) &&
							(x + 1 != Settings.sizeX ? map[y, x + 1].IsEmpty() : true)
						) {
							pos.Add(new Tuple<byte, byte>(x, y));
						}
					}
				}

				if(pos.Count == 0)
					return false;

				ushort randId = (ushort) Singletons.rand.Next(0, pos.Count);
				PlaceMeat(pos[randId].Item1, pos[randId].Item2);

				return true;
			}

			bool PlaceOnAnyPos() {
				List<Tuple<byte, byte>> pos = new List<Tuple<byte, byte>>();
				for(byte x = 0; x < Settings.sizeX; ++x)
					for(byte y = 0; y < Settings.sizeY; ++y)
						if(map[y, x].IsEmpty())
							pos.Add(new Tuple<byte, byte>(x, y));

				if(pos.Count == 0)
					return false;

				ushort randId = (ushort) Singletons.rand.Next(0, pos.Count);
				PlaceMeat(pos[randId].Item1, pos[randId].Item2);

				return true;
			}
		}

		public GameOverResult IsGameOver() {
			for(byte i = 0; i < Settings.sizeY; ++i)
				for(byte j = 0; j < Settings.sizeX; ++j)
					if(map[i, j].IsEmpty())
						return GameOverResult.None;
			return scoreEnemy == scorePlayer ? GameOverResult.Draw :
					(scoreEnemy < scorePlayer ? GameOverResult.WinPlayer :
					GameOverResult.WinEnemy);
		}
	}
}
