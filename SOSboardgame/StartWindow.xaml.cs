using System.Windows;

namespace SOSboardgame
{
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainGame = new MainWindow();
            mainGame.Show();
            this.Close(); // close the start screen
        }
    }
}
