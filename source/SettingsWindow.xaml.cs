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
	/// Interaction logic for SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : System.Windows.Window {
		bool SmthChamged = false;

		public SettingsWindow() {
			InitializeComponent();
			WindowManager.AddWindow(this);

			ZLvl.IsChecked = Settings.meatImageZ < Settings.stickImageZ;
			Animation.IsChecked = Settings.useAnimation;
		}

		private void WindowClosed(object sender, EventArgs e) {
			WindowManager.CloseAll();
		}

		private void ButtonReturn(object sender, RoutedEventArgs e) {
			//WindowManager.ReopenWindow(this, MainWindow.MenuWindow);
			Settings.Save();
			System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
			System.Threading.Thread.Sleep(1000);
			Application.Current.Shutdown();
		}

		void ChangeMeatStickLevel() {
			var tmp = Settings.meatImageZ;
			Settings.meatImageZ = Settings.stickImageZ;
			Settings.stickImageZ = tmp;
			SmthChamged = true;
		}

		void ChangeAnimationMode() {
			Settings.useAnimation = !Settings.useAnimation;
			Settings.Save();
			SmthChamged = true;
		}

		private void ZLvl_Click(object sender, RoutedEventArgs e) {
			ChangeMeatStickLevel();
		}

		private void Animation_Click(object sender, RoutedEventArgs e) {
			ChangeAnimationMode();
		}
	}
}
