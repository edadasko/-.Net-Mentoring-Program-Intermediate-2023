using System;
using System.Windows;
using System.Windows.Threading;

namespace GameOfLife
{
    public partial class MainWindow : Window
    {
        private Grid mainGrid;
        DispatcherTimer timer;   //  Generation timer
        private int genCounter;
        private AdWindow[] adWindows;


        public MainWindow()
        {
            InitializeComponent();
            mainGrid = new Grid(MainCanvas);

            timer = new DispatcherTimer();
            timer.Tick += OnTimer;
            timer.Interval = TimeSpan.FromMilliseconds(200);
        }


        private void StartAd()
        {
            const int adsCount = 2;
            if (adWindows == null)
            {
                adWindows = new AdWindow[adsCount];
            }
            else
            {
                foreach (var ad in adWindows)
                {
                    ad.Close();
                }
            }

            for (int i = 0; i < adsCount; i++)
            {
                adWindows[i] = new AdWindow(this);
                adWindows[i].Top = this.Top + (330 * i) + 70;
                adWindows[i].Left = this.Left + 240;
                adWindows[i].Show();
            }
        }


        private void Button_OnClick(object sender, EventArgs e)
        {
            if (!timer.IsEnabled)
            {
                timer.Start();
                ButtonStart.Content = "Stop";
                StartAd();
            }
            else
            {
                timer.Stop();
                ButtonStart.Content = "Start";
            }
        }

        private void OnTimer(object sender, EventArgs e)
        {
            mainGrid.Update();
            genCounter++;
            lblGenCount.Content = "Generations: " + genCounter;
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            mainGrid.Clear();
        }
    }
}
