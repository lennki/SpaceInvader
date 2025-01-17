using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace SpaceInvader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new GameEngine();
            startKnopf.Visibility = Visibility.Collapsed;
            text1.Visibility = Visibility.Collapsed;
            text2.Visibility = Visibility.Collapsed;
        }
    }

}

