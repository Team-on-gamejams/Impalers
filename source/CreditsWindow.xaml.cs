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
	/// Interaction logic for CreditsWindow.xaml
	/// </summary>
	public partial class CreditsWindow : Window {
		public CreditsWindow() {
			InitializeComponent();
			WindowManager.AddWindow(this);
		}

		private void WindowClosed(object sender, EventArgs e) {
			WindowManager.CloseAll();
		}

		private void ButtonReturn(object sender, RoutedEventArgs e) {
			WindowManager.ReopenWindow(this, MainWindow.MenuWindow);
		}
	}
}
