using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace SpaceInvader
{

    partial class GameEngine
    {
        MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;

        double x;
        double y;
        private DispatcherTimer spawnTimer;
        private DispatcherTimer checkAufprall;
        int leben = 100;
        int score = 0;
        int geschwindigkeit = 20;
        bool spielVorbei = false;
        int spawnRate = 5;
        List<Rectangle> loeschen = new List<Rectangle>();


        public GameEngine()
        {
            
            y = Canvas.GetTop(mainWindow.spielfigur);
            x = Canvas.GetLeft(mainWindow.spielfigur);

            spawnTimer = new DispatcherTimer();
            spawnTimer.Interval = new TimeSpan(0, 0, 0, 5, 0);
            spawnTimer.Tick += sT_Tick;
            spawnTimer.Start();

            checkAufprall = new DispatcherTimer();
            checkAufprall.Interval = new TimeSpan(0, 0, 0, 0, 5);
            checkAufprall.Tick += cA_Tick;
            checkAufprall.Start();
        }

        void sT_Tick(object? sender, EventArgs e)
        {
            Gegner gegner = new Gegner(geschwindigkeit);
            gegner.GegnerErstellen();
            gegner.LebenVerlieren += LebenAbziehen;
        }

        public void ScoreErhoen()
        {
            score++;
            mainWindow.Score.Content = "Score: " + score;

            if (score % 2 == 0 && geschwindigkeit > 1)
            {
                geschwindigkeit--;
            }

            if (score % 5 == 0 && spawnRate > 1)
            {
                spawnRate--;
                spawnTimer.Interval = new TimeSpan(0, 0, 0, spawnRate, 0);
            }
        }

        public void LebenAbziehen()
        {
            if (leben > 0)
            {
                leben -= 10;
                mainWindow.leben.Content = "Leben: " + leben;
            }

            if (leben <= 0 && !spielVorbei)
            {
                checkAufprall.Stop();
                spawnTimer.Stop();
                spielVorbei = true;

                var neustart = MessageBox.Show("Beenden \n Dein Score: " + score);

                if (neustart == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
        }

        void cA_Tick(object? sender, EventArgs e)
        {
            foreach (var x in mainWindow.canvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "schuss")
                {
                    double ySchuss = Canvas.GetTop(x);
                    double xSchuss = Canvas.GetLeft(x);
                    double xSchussRechts = xSchuss + x.ActualWidth;

                    foreach (var y in mainWindow.canvas.Children.OfType<Rectangle>())
                    {
                        if ((string)y.Tag == "gegner")
                        {
                            double yGegner = Canvas.GetTop(y) + y.ActualHeight;
                            double xGegner = Canvas.GetLeft(y);
                            double xGegnerRechts = xGegner + y.ActualWidth;

                            if (ySchuss < yGegner && xSchuss <= xGegnerRechts && xSchussRechts >= xGegner)
                            {
                                loeschen.Add(x);
                                loeschen.Add(y);
                                ScoreErhoen();
                            }
                        }
                    }
                }
            }
            foreach (Rectangle x in loeschen)
            {
                mainWindow.canvas.Children.Remove(x);
            }
        }

        [RelayCommand]
        public void BewegeUnten()
        {
            if (y + 2*mainWindow.spielfigur.ActualHeight <= mainWindow.canvas.ActualHeight)
            {
                y += 10;
                Canvas.SetTop(mainWindow.spielfigur, y);
            }
        }

        [RelayCommand]
        public void BewegeOben()
        {
            if (y > mainWindow.canvas.ActualHeight*0.35)
            {
                y -= 10;
                Canvas.SetTop(mainWindow.spielfigur, y);
            }
        }

        [RelayCommand]
        public void BewegeRechts()
        {
            if (x + mainWindow.spielfigur.ActualWidth <= mainWindow.canvas.ActualWidth)
            {
                x += 10;
                Canvas.SetLeft(mainWindow.spielfigur, x);
            }
        }

        [RelayCommand]
        public void BewegeLinks()
        {
            if (x > 0)
            {
                x -= 10;
                Canvas.SetLeft(mainWindow.spielfigur, x);
            }
        }

        [RelayCommand]
        public void Schiessen()
        {
            Schuss schuss = new Schuss();
            schuss.Erstellen(x, y);
        }
    }
}
