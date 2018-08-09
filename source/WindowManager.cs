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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Impalers{
	static class WindowManager {
		static List<Window> allWindows = new List<Window>();

		public static void ReopenWindow(this Window opened, Window toOpen){
            toOpen.Width = opened.ActualWidth;
            toOpen.Height = opened.ActualHeight;
            toOpen.WindowState = opened.WindowState;
            toOpen.Top = opened.Top;
            toOpen.Left = opened.Left;

            toOpen.Show();
            opened.Hide();
		}

		public static void AddWindow(Window window) {
			allWindows.Add(window);
		}

		public static void CloseAll() {
			foreach (var window in allWindows)
				window.Close();
			allWindows.Clear();
		}
    }
}
