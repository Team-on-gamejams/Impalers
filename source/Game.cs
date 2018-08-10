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

		public Game() {
			for (byte i = 0; i < Settings.sizeY; ++i)
				for (byte j = 0; j < Settings.sizeX; ++j)
					map[i, j] = new GameCell();
		}

		public void Start(Enemy enemy) {
			this.enemy = enemy;
			for (byte i = 0; i < Settings.sizeY; ++i)
				for (byte j = 0; j < Settings.sizeX; ++j)
					map[i, j].Init();
			isPlayerTurn = true;
			scorePlayer = scoreEnemy = 0;
		}

		public void PlaceMeat(byte x, byte y) {
			map[y, x].isMeat = true;

			byte randomImageNumber;
			do {
				randomImageNumber = (byte)Singletons.rand.Next(1, 11);
			} while (randomImageNumber == (y + 1 == Settings.sizeY ? 255: map[y + 1, x].imageNumber) ||
				randomImageNumber == (y - 1 == -1 ? 255 : map[y - 1, x].imageNumber) ||
				randomImageNumber == (x + 1 == Settings.sizeX ? 255 : map[y, x + 1].imageNumber) ||
				randomImageNumber == (x - 1 == -1 ? 255 : map[y, x - 1].imageNumber) || 

				randomImageNumber == (x - 1 == -1				|| y - 1 == -1				? 255 : map[y - 1, x - 1].imageNumber) ||
				randomImageNumber == (x + 1 == Settings.sizeX	|| y - 1 == -1				? 255 : map[y - 1, x + 1].imageNumber) ||
				randomImageNumber == (x - 1 == -1				|| y + 1 == Settings.sizeY	? 255 : map[y + 1, x - 1].imageNumber) ||
				randomImageNumber == (x + 1 == Settings.sizeX	|| y + 1 == Settings.sizeY	? 255 : map[y + 1, x + 1].imageNumber) 
			);

			map[y, x].imageNumber = randomImageNumber;
			map[y, x].imageMeat.Source = new BitmapImage(new Uri("Resources/Meat/" + randomImageNumber.ToString() + ".png", UriKind.Relative));

			map[y, x].imageMeat.BeginAnimation(Image.HeightProperty, 
				new DoubleAnimation {
					From = 25,
					To = 100,
					Duration = new Duration(TimeSpan.FromSeconds(0.5))
				}
			);

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

			if (isPlayerTurn) {
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
			short dx = (short)(startX - endX), dy = (short)(startY - endY);
			MessageBox.Show(dx.ToString() + ' ' + dy.ToString());
			if(dx == 0 || dy == 0) {
				if (dy < 0 && (startY == 0 || map[startY - 1, startX].IsEmpty())) {
					map[startY - 1, startX].isStickEnd = true;
					map[startY - 1, startX].stickDirection = Direction.Down;
					map[startY - 1, startX].imageStick.Source = new BitmapImage(new Uri("Resources/Pl1/1Start.png", UriKind.Relative));
					map[startY - 1, startX].imageStick.RenderTransform = new RotateTransform(90);
				}
				isPlayerTurn = !isPlayerTurn;
			}
		}

		public void BotTurn() {
			byte randX, randY;
			do {
				randX = (byte)Singletons.rand.Next(0, Settings.sizeX);
				randY = (byte)Singletons.rand.Next(0, Settings.sizeY);
			} while (!map[randY, randX].IsEmpty());
			PlaceMeat(randX, randY);
		}

		public GameOverResult IsGameOver() {
			for (byte i = 0; i < Settings.sizeY; ++i)
				for (byte j = 0; j < Settings.sizeX; ++j)
					if (map[i, j].IsEmpty())
						return GameOverResult.None;
			return	scoreEnemy == scorePlayer ? GameOverResult.Draw :
					(scoreEnemy > scorePlayer ? GameOverResult.WinPlayer : 
					GameOverResult.WinEnemy);
		}
	}

	public enum Direction : byte { Left, Up, Right, Down, None}
}
