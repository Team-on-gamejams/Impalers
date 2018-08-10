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

namespace Impalers {
	/// <summary>
	/// Interaction logic for GameWindow.xaml
	/// </summary>
	public partial class GameWindow : Window {
		//---------------------------- Fields ---------------------------------
		Game game = new Game();
		byte startClickX, startClickY, endClickX, endClickY;
		bool isBotThinking = false;
		double widthMod;

		//---------------------------- Properties ---------------------------------


		//---------------------------- Methods ---------------------------------
		public void StartGame(Game.Enemy enemy) {
			this.ScoreEnemy.Text = this.ScorePlayer.Text = "0";
			game.Start(enemy);
		}

		public GameWindow() {
			InitializeComponent();
			WindowManager.AddWindow(this);
			for (byte i = 0; i < Settings.sizeY; ++i)
				MeatGrid.RowDefinitions.Add(new RowDefinition());
			for (byte j = 0; j < Settings.sizeX; ++j)
				MeatGrid.ColumnDefinitions.Add(new ColumnDefinition());

			for (byte i = 0; i < Settings.sizeY; ++i) {
				for (byte j = 0; j < Settings.sizeX; ++j) {
					this.MeatGrid.Children.Add(game.map[i, j].grid);
					Grid.SetRow(game.map[i, j].grid, i);
					Grid.SetColumn(game.map[i, j].grid, j);

					game.map[i, j].grid.PreviewMouseLeftButtonDown += (a, b) => {
						byte x = (byte)Grid.GetColumn(a as UIElement);
						byte y = (byte)Grid.GetRow(a as UIElement);
						startClickX = x;
						startClickY = y;
					};
					game.map[i, j].grid.PreviewMouseLeftButtonUp += (a, b) => {
						byte x = (byte)Grid.GetColumn(a as UIElement);
						byte y = (byte)Grid.GetRow(a as UIElement);
						endClickX = x;
						endClickY = y;

						if (game.isPlayerTurn || (!game.isPlayerTurn && game.enemy == Game.Enemy.Player)) {
							if (startClickX == endClickX && startClickY == endClickY) {
								if (game.map[y, x].IsEmpty()) 
									game.PlaceMeat(startClickX, startClickY);
							}
							else {
								game.PlaceStick(startClickX, startClickY, endClickX, endClickY);
							}

							WriteScore();
							CheckGameOver();
						}

						if(!game.isPlayerTurn && game.enemy == Game.Enemy.Bot && !isBotThinking) {
							isBotThinking = true;
							System.Timers.Timer t = new System.Timers.Timer() {
								AutoReset = false,
								Interval = 500,
							};
							t.Elapsed += (c, d) => {
								System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
									game.BotTurn();
									isBotThinking = false;
									WriteScore();
									CheckGameOver();
								});
							};
							t.Start();
						}
					};
				}
			}
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
			if (e.HeightChanged) 
				this.Width = Height * widthMod;
			else 
				Height = Width / widthMod;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			widthMod = this.Width / this.Height;
		}

		void WriteScore() {
			this.ScoreEnemy.Text = game.scoreEnemy.ToString();
			this.ScorePlayer.Text = game.scorePlayer.ToString();
		}

		void CheckGameOver() {
			var gameResult = game.IsGameOver();
			if (gameResult != Game.GameOverResult.None) {
				string winnerStr;
				if (gameResult == Game.GameOverResult.Draw)
					winnerStr = "Draw!";
				else {
					if (gameResult == Game.GameOverResult.WinPlayer)
						winnerStr = game.enemy == Game.Enemy.Bot ? "Player win!" : "Player 1 win!";
					else
						winnerStr = game.enemy == Game.Enemy.Bot ? "Bot win!" : "Player 2 win!" ;
				}

				if (MessageBox.Show(winnerStr + "\nDo you want to play again?", "Game over!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
					this.StartGame(game.enemy);
				else
					WindowManager.ReopenWindow(this, MainWindow.MenuWindow);
			}
		}

		private void WindowClosed(object sender, EventArgs e) {
			WindowManager.CloseAll();
		}
	}
}
