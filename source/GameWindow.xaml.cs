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
						Singletons.startClickX = x;
						Singletons.startClickY = y;
					};
					game.map[i, j].grid.PreviewMouseLeftButtonUp += (a, b) => {
						byte x = (byte)Grid.GetColumn(a as UIElement);
						byte y = (byte)Grid.GetRow(a as UIElement);
						Singletons.endClickX = x;
						Singletons.endClickY = y;

						if (game.isPlayerTurn || (!game.isPlayerTurn && game.enemy == Game.Enemy.Player)) {
							if (Singletons.startClickX == Singletons.endClickX && Singletons.startClickY == Singletons.endClickY) {
								if (game.map[y, x].IsEmpty()) 
									game.PlaceMeat(x, y);
							}
						}
						else{

						}

						this.ScoreEnemy.Text = game.scoreEnemy.ToString();
						this.ScorePlayer.Text = game.scorePlayer.ToString();

						var gameResult = game.IsGameOver();
						if(gameResult != Game.GameOverResult.None) {
							string winnerStr;
							if (gameResult == Game.GameOverResult.Draw)
								winnerStr = "Draw!";
							else {
								if (game.enemy == Game.Enemy.Bot) {
									if(gameResult == Game.GameOverResult.WinPlayer)
										winnerStr = "Player win!";
									else
										winnerStr = "Bot win!";
								}
								else {
									if (gameResult == Game.GameOverResult.WinPlayer)
										winnerStr = "Player 1 win!";
									else
										winnerStr = "Player 2 win!";
								}
							}

							if (MessageBox.Show(winnerStr + "\nDo you want to play again?", "Game over!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
								this.StartGame(game.enemy);
							else
								WindowManager.ReopenWindow(this, MainWindow.MenuWindow);
						}
					};
				}
			}
		}

		private void WindowClosed(object sender, EventArgs e) {
			WindowManager.CloseAll();
		}
	}
}
