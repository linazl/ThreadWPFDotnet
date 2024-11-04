using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Exercice_Thread_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Thread> threads;
        private bool isRunning;
        private object lockObject = new object();
        private List<ProgressBar> progressBars;

        public MainWindow()
        {
            InitializeComponent();
            InitializeThreadComponents();
        }

        private void InitializeThreadComponents()
        {
            // Initialisation des composants
            threads = new List<Thread>();
            isRunning = false;

            // Création de la liste des ProgressBars
            progressBars = new List<ProgressBar>
            {
                ProgressBar1, ProgressBar2, ProgressBar3, ProgressBar4, ProgressBar5
            };

            // Configuration initiale des ProgressBars
            foreach (var progressBar in progressBars)
            {
                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                progressBar.Value = 0;
            }
        }

        private void BtnDemarrer_Click(object sender, RoutedEventArgs e)
        {
            if (!isRunning)
            {
                // Réinitialisation des ProgressBars
                foreach (var progressBar in progressBars)
                {
                    progressBar.Value = 0;
                }

                isRunning = true;
                threads.Clear();

                // Création et démarrage des threads
                for (int i = 0; i < 5; i++)
                {
                    int threadIndex = i;
                    var thread = new Thread(() => IncrementCounter(threadIndex));
                    threads.Add(thread);
                    thread.Start();
                }

                BtnDemarrer.IsEnabled = false;
                BtnArreter.IsEnabled = true;
            }
        }

        private void BtnArreter_Click(object sender, RoutedEventArgs e)
        {
            isRunning = false;
            BtnDemarrer.IsEnabled = true;
            BtnArreter.IsEnabled = false;
        }

        private void IncrementCounter(int index)
        {
            Random rand = new Random();
            while (isRunning && progressBars[index].Dispatcher.Invoke(() => progressBars[index].Value) < 100)
            {
                // Mise à jour de la ProgressBar de manière thread-safe
                progressBars[index].Dispatcher.Invoke(() =>
                {
                    if (progressBars[index].Value < 100)
                    {
                        progressBars[index].Value += 1;
                    }
                });

                Thread.Sleep(rand.Next(50, 200));
            }
        }
    }
}