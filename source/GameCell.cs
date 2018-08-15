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

		public Image[] imageStickEnd;

		public bool isMeat;
		public bool isStickStart, isStickEnd, isStickBody;

		public GameCell() {
			grid = new Grid();
			imageStick = new Image() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				RenderTransformOrigin = new Point(0.5, 0.5),
			};
			imageStick.RenderTransformOrigin = new Point(0.5, 0.5);
			grid.Children.Add(imageStick);

			imageStickEnd = new Image[3];
			imageStickEnd[0] = new Image() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				RenderTransformOrigin = new Point(0.5, 0.5),
			};
			imageStickEnd[1] = new Image() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				RenderTransformOrigin = new Point(0.5, 0.5),
			};
			imageStickEnd[2] = new Image() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				RenderTransformOrigin = new Point(0.5, 0.5),
			};
			grid.Children.Add(imageStickEnd[0]);
			grid.Children.Add(imageStickEnd[1]);
			grid.Children.Add(imageStickEnd[2]);

			imageMeat = new Image() {
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
			};
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
			imageStick.RenderTransform = null;
			imageStickEnd[0].Source = imageStickEnd[1].Source = imageStickEnd[2].Source = null;
			imageStickEnd[0].VerticalAlignment = imageStickEnd[1].VerticalAlignment = imageStickEnd[2].VerticalAlignment = VerticalAlignment.Center;
			imageStickEnd[0].HorizontalAlignment = imageStickEnd[1].HorizontalAlignment = imageStickEnd[2].HorizontalAlignment = HorizontalAlignment.Center;
			imageStickEnd[0].RenderTransform = imageStickEnd[1].RenderTransform = imageStickEnd[2].RenderTransform = null;
			imageNumber = 255;
			isStickStart = isStickEnd = isStickBody = isMeat = false;
		}

		public bool IsEmpty() => !isMeat && !isStickStart;
	}
}
