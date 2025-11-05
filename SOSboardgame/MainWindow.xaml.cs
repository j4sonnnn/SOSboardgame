using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SOSboardgame
{
    public partial class MainWindow : Window
    {
        private SOSGame? game;   // ✅ now uses the base class

        public MainWindow()
        {
            InitializeComponent();
            BuildBoard(8);
            UpdateTurnLabel();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            int size = int.Parse(((ComboBoxItem)BoardSizeBox.SelectedItem!).Content!.ToString()!);
            var modeStr = ((ComboBoxItem)GameModeBox.SelectedItem!).Content!.ToString()!;

            // ✅ create subclass based on mode
            game = modeStr == "General"
                ? new GeneralSOSGame(size)
                : new SimpleSOSGame(size);

            BuildBoard(size);
            UpdateTurnLabel();
        }

        private void BuildBoard(int size)
        {
            BoardGrid.Children.Clear();
            BoardGrid.Rows = size;
            BoardGrid.Columns = size;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int x = i, y = j;
                    var btn = new Button
                    {
                        Content = "",
                        FontSize = 18,
                        MinHeight = 35,
                        MinWidth = 35,
                        Margin = new Thickness(1),
                        Background = Brushes.WhiteSmoke
                    };

                    btn.Click += (s, _) =>
                    {
                        if (game == null || game.IsGameOver) return;

                        char letter = (LetterS.IsChecked == true) ? 'S' : 'O';
                        if (game.PlaceLetter(x, y, letter))
                        {
                            var btnClicked = (Button)s;
                            btnClicked.Content = letter.ToString();

                            // 🎨 Color based on player who just moved
                            if (game.CurrentTurn == Player.Red)
                            {
                                // Blue just played
                                btnClicked.Foreground = Brushes.Blue;
                                btnClicked.Background = Brushes.LightBlue;
                            }
                            else
                            {
                                // Red just played
                                btnClicked.Foreground = Brushes.Red;
                                btnClicked.Background = Brushes.LightCoral;
                            }

                            UpdateTurnLabel();

                            // ✅ Show winner/draw popup if finished
                            if (game.IsGameOver)
                            {
                                string result = game.Winner == null
                                    ? "Draw!"
                                    : $"{game.Winner} wins!";
                                MessageBox.Show(result, "Game Over",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    };

                    BoardGrid.Children.Add(btn);
                }
            }
        }

        private void UpdateTurnLabel()
        {
            if (game == null)
            {
                TurnLabel.Text = "Click 'Start New Game' to begin.";
            }
            else
            {
                TurnLabel.Text =
                    $"Current turn: {game.CurrentTurn}  •  " +
                    $"Mode: {(game is GeneralSOSGame ? "General" : "Simple")}  •  " +
                    $"Size: {game.BoardSize}x{game.BoardSize}";
            }
        }

        private void GameModeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optional: handle UI feedback when switching modes
        }
    }
}
