using System.Windows;
using System.Windows.Controls;

namespace SOSboardgame
{
    public partial class MainWindow : Window
    {
        private GameLogic? game;

        public MainWindow()
        {
            InitializeComponent();  // ✅ ensures XAML and code-behind connect
            BuildBoard(8);
            UpdateTurnLabel();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            int size = int.Parse(((ComboBoxItem)BoardSizeBox.SelectedItem!).Content!.ToString()!);
            var modeStr = ((ComboBoxItem)GameModeBox.SelectedItem!).Content!.ToString()!;
            var mode = modeStr == "General" ? GameMode.General : GameMode.Simple;

            game = new GameLogic(size, mode);
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
                        FontSize = 16,
                        MinHeight = 30,
                        MinWidth = 30,
                        Margin = new Thickness(1)
                    };
                    btn.Click += (s, _) =>
                    {
                        if (game == null) return;
                        char letter = (LetterS.IsChecked == true) ? 'S' : 'O';
                        if (game.PlaceLetter(x, y, letter))
                        {
                            ((Button)s).Content = letter.ToString();
                            UpdateTurnLabel();
                        }
                    };
                    BoardGrid.Children.Add(btn);
                }
            }
        }

        private void UpdateTurnLabel()
        {
            if (game == null)
                TurnLabel.Text = "Click 'Start New Game' to begin.";
            else
                TurnLabel.Text = $"Current turn: {game.CurrentTurn} • Mode: {game.Mode} • Size: {game.BoardSize}x{game.BoardSize}";
        }
    }
}
