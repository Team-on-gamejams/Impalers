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
	public class GameCell {
		public Grid grid;
		public byte imageNumber;
		public Image imageMeat;
		public Image imageStick;

		public bool isMeat;
		public bool isStickStart, isStickEnd, isStickBody;

		public GameCell() {
			grid = new Grid();
			imageMeat = new Image() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
			};
			imageStick = new Image() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				RenderTransformOrigin = new Point(0.5, 0.5),
			};
			grid.Children.Add(imageStick);
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
			imageStick.Source = null;
			imageStick.VerticalAlignment = VerticalAlignment.Center;
			imageStick.HorizontalAlignment = HorizontalAlignment.Center;
			imageNumber = 255;
			isStickStart = isStickEnd = isStickBody = isMeat = false;
		}

		public bool IsEmpty() => !isMeat && !isStickEnd;
	}
}
