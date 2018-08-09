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
	public class Game {
		public enum Enemy : byte { Player, Bot }
		public Enemy enemy;

		public GameCell[,] map = new GameCell[Settings.sizeY, Settings.sizeX];
		public bool playerTurn;

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
			playerTurn = true;
		}

	}

	public enum Direction : byte { Left, Up, Right, Down, None}

	public class GameCell {
		public Grid grid;
		public Image imageMeat;

		public bool isMeat;
		public Direction stickDirection;
		public bool isStickStart, isStickEnd, isStickBody;

		public GameCell() {
			grid = new Grid();
			imageMeat = new Image();
			grid.Children.Add(imageMeat);
			grid.Children.Add(new Image() {
				Source = new BitmapImage(new Uri(@"Resources\ClickMask.jpg", UriKind.Relative)),
				Opacity = 0.001,
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				Stretch = Stretch.Fill,
			});
		}

		public void Init() {
			imageMeat.Source = null;
			stickDirection = Direction.None;
			isStickStart = isStickEnd = isStickBody = isMeat = false;
		}
	}
}
