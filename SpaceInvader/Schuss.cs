using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SpaceInvader
{
    public class Schuss
    {
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        private double X {  get; set; } 
        private double Y { get; set; }
        List<Rectangle> loeschen = new List<Rectangle>();

        private DispatcherTimer dispatcherTimer;

        private Rectangle schuss;

        ImageBrush schussBild = new ImageBrush();

        public Schuss()
        {
            schussBild.ImageSource = new BitmapImage(new Uri("images/schuss.png", UriKind.Relative));

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            dispatcherTimer.Tick += Dp_Tick;
            dispatcherTimer.Start();
        }

        public void Erstellen(double x,double y)
        {
            X = x;
            Y = y;

            schuss = new Rectangle();
            {
                schuss.Tag = "schuss";
                schuss.Width = 10;
                schuss.Height = 20;
                schuss.Fill = schussBild;
                Canvas.SetTop(schuss, Y);
                Canvas.SetLeft(schuss, X + mainWindow.spielfigur.ActualWidth/2);
            }
            mainWindow.canvas.Children.Add(schuss);
        }

        void Dp_Tick(object? sender, EventArgs e)
        {
            Y -= 5;
            Canvas.SetTop(schuss, Y);
            Canvas.SetLeft(schuss, X + mainWindow.spielfigur.ActualWidth / 2);

            foreach (var x in mainWindow.canvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "schuss")
                {

                    if (Canvas.GetTop(x) < 0)
                    {
                        loeschen.Add(x);
                    }
                }
            }

            foreach (Rectangle x in loeschen)
            {
                mainWindow.canvas.Children.Remove(x);
            }
        }
    }
}
