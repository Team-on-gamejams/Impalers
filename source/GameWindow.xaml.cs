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
						var img = game.map[Grid.GetRow(a as UIElement), Grid.GetColumn(a as UIElement)].imageMeat.Source;
						if(img == null)
							game.map[Grid.GetRow(a as UIElement), Grid.GetColumn(a as UIElement)].imageMeat.Source = new BitmapImage(new Uri("Resources/Meat/" + Settings.rand.Next(1, 11) + ".png", UriKind.Relative));
					};
				}
			}
		}

		private void WindowClosed(object sender, EventArgs e) {
			WindowManager.CloseAll();
		}
	}
}
