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
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		//---------------------------- Fields ---------------------------------
		static private Lazy<GameWindow> gameWindow = new Lazy<GameWindow>();
		static private Lazy<HightScoresWindow> hightScoresWindow = new Lazy<HightScoresWindow>();

		//---------------------------- Properties ---------------------------------
		static public HightScoresWindow HightScoresWindow => hightScoresWindow.Value;
		static public GameWindow GameWindow => gameWindow.Value;
		static public MainWindow MenuWindow { get; private set;}

		//---------------------------- Methods ---------------------------------

		public MainWindow(){
            InitializeComponent();
			WindowManager.AddWindow(this);
			MenuWindow = this;
		}

		private void WindowClosed(object sender, EventArgs e) {
			WindowManager.CloseAll();
		}

		//---------------------------- Methods - Button.Click ---------------------------------
		private void ButtonSingleplayer(object sender, RoutedEventArgs e) {
			this.ReopenWindow(GameWindow);
			GameWindow.StartGame(Game.Enemy.Bot);
		}

		private void ButtonMultiplayer(object sender, RoutedEventArgs e) {
			this.ReopenWindow(GameWindow);
			GameWindow.StartGame(Game.Enemy.Player);
		}

		private void ButtonHightscores(object sender, RoutedEventArgs e) {
			this.ReopenWindow(HightScoresWindow);
		}

		private void ButtonExit(object sender, RoutedEventArgs e) {
			this.Close();
		}
	}
}
