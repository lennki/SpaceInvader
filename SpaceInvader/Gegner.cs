using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;




namespace SpaceInvader
{
    public class Gegner
    {
        private int geschwindigkeit;

        MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
        Random rand = new Random();
        private DispatcherTimer dispatcherTimer;
        private Rectangle gegner;
        private double y = 5;
        List<Rectangle> loeschen = new List<Rectangle>();
        public event Action? LebenVerlieren;

        ImageBrush gegnerBild = new ImageBrush();

        public Gegner(int geschwindigkeit)
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, geschwindigkeit);
            dispatcherTimer.Tick += Dp_Tick;
            dispatcherTimer.Start();

            gegnerBild.ImageSource = new BitmapImage(new Uri("Images/gegner.png", UriKind.Relative));
        }

        public void GegnerErstellen()
        {

            gegner = new Rectangle();
            {
                gegner.Tag = "gegner";
                gegner.Width = 50;
                gegner.Height = 50;
                gegner.Fill = gegnerBild;
                Canvas.SetTop(gegner, y);
                Canvas.SetLeft(gegner, (double)rand.Next(50,350));
            }

            mainWindow.canvas.Children.Add(gegner);
        }
        void Dp_Tick(object? sender, EventArgs e)
        {
            y += 5;

            Canvas.SetTop(gegner, y);


            bool lebenVerloren = false;

            for (int i = mainWindow.canvas.Children.Count - 1; i >= 0; i--)
            {
                if (mainWindow.canvas.Children[i] is Rectangle rect && (string)rect.Tag == "gegner")
                {
                    if (Canvas.GetTop(rect) + 50 > mainWindow.canvas.ActualHeight)
                    {
                        if (!lebenVerloren)
                        {
                            LebenVerlieren?.Invoke();
                            lebenVerloren = true;
                        }

                        mainWindow.canvas.Children.Remove(rect);
                    }
                }
            }

        }
    }
}
