using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SOSboardgame
{
    public partial class MainWindow : Window
    {
        private SOSPlayer? bluePlayer;
        private SOSPlayer? redPlayer;
        private SOSPlayer? currentPlayer;

        private SOSGame? game;
        private Button[,]? boardButtons;

        // NEW FOR SPRINT 5
        private List<(int row, int col, char letter)> recordedMoves = new();
        private int replayIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            BuildBoard(8);
            UpdateTurnLabel();
        }

        // -------------------------------------------------------
        // START GAME
        // -------------------------------------------------------
        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            int size = int.Parse(((ComboBoxItem)BoardSizeBox.SelectedItem!).Content!.ToString()!);
            string modeStr = ((ComboBoxItem)GameModeBox.SelectedItem!).Content!.ToString()!;

            game = (modeStr == "General")
                ? new GeneralSOSGame(size)
                : new SimpleSOSGame(size);

            // Player selection
            bluePlayer = (BlueHuman.IsChecked == true)
                ? new HumanPlayer(Player.Blue)
                : new ComputerPlayer(Player.Blue);

            redPlayer = (RedHuman.IsChecked == true)
                ? new HumanPlayer(Player.Red)
                : new ComputerPlayer(Player.Red);

            currentPlayer = bluePlayer;

            // RESET BOARD + REPLAY DATA
            BuildBoard(size);
            recordedMoves.Clear();
            replayIndex = 0;
            MoveCounterLabel.Content = "Move 0 / 0";

            UpdateTurnLabel();

            if (currentPlayer.Type == PlayerType.Computer)
                PlayComputerTurn();
        }

        // -------------------------------------------------------
        // BUILD BOARD UI
        // -------------------------------------------------------
        private void BuildBoard(int size)
        {
            BoardGrid.Children.Clear();
            BoardGrid.Rows = size;
            BoardGrid.Columns = size;

            boardButtons = new Button[size, size];

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    int row = r, col = c;

                    var btn = new Button
                    {
                        FontSize = 20,
                        Background = Brushes.WhiteSmoke,
                        Margin = new Thickness(1),
                        Content = ""
                    };

                    btn.Click += (s, _) => HandleHumanMove((Button)s, row, col);

                    boardButtons[r, c] = btn;
                    BoardGrid.Children.Add(btn);
                }
            }
        }

        // -------------------------------------------------------
        // HUMAN MOVE
        // -------------------------------------------------------
        private void HandleHumanMove(Button btn, int row, int col)
        {
            if (game == null || game.IsGameOver) return;
            if (currentPlayer!.Type == PlayerType.Computer) return;

            char letter = (LetterS.IsChecked == true) ? 'S' : 'O';

            if (game.PlaceLetter(row, col, letter))
            {
                UpdateCellUI(btn, letter);

                // RECORD MOVE
                recordedMoves.Add((row, col, letter));
                MoveCounterLabel.Content = $"Move {recordedMoves.Count} / {recordedMoves.Count}";

                AfterMoveLogic();
            }
        }

        // -------------------------------------------------------
        // UPDATE CELL UI
        // -------------------------------------------------------
        private void UpdateCellUI(Button btn, char letter)
        {
            btn.Content = letter.ToString();

            if (currentPlayer!.Color == Player.Blue)
            {
                btn.Foreground = Brushes.Blue;
                btn.Background = Brushes.LightBlue;
            }
            else
            {
                btn.Foreground = Brushes.Red;
                btn.Background = Brushes.LightCoral;
            }
        }

        // -------------------------------------------------------
        // POST-MOVE LOGIC
        // -------------------------------------------------------
        private void AfterMoveLogic()
        {
            UpdateTurnLabel();

            if (game!.IsGameOver)
            {
                string msg = game.Winner == null
                    ? "Draw!"
                    : $"{game.Winner} wins!";

                MessageBox.Show(msg, "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            SwitchPlayer();
        }

        // -------------------------------------------------------
        // SWITCH PLAYER
        // -------------------------------------------------------
        private void SwitchPlayer()
        {
            currentPlayer = (currentPlayer == bluePlayer) ? redPlayer : bluePlayer;

            if (currentPlayer!.Type == PlayerType.Computer)
                PlayComputerTurn();
        }

        // -------------------------------------------------------
        // COMPUTER MOVE
        // -------------------------------------------------------
        private async void PlayComputerTurn()
        {
            await Task.Delay(250);

            if (game == null || game.IsGameOver || boardButtons == null)
                return;

            var (row, col, letter) = currentPlayer!.ChooseMove(game);
            if (row == -1) return;

            var btn = boardButtons[row, col];

            game.PlaceLetter(row, col, letter);
            UpdateCellUI(btn, letter);

            // RECORD COMPUTER MOVE
            recordedMoves.Add((row, col, letter));
            MoveCounterLabel.Content = $"Move {recordedMoves.Count} / {recordedMoves.Count}";

            AfterMoveLogic();
        }

        // -------------------------------------------------------
        // UPDATE TURN TEXT
        // -------------------------------------------------------
        private void UpdateTurnLabel()
        {
            if (game == null)
            {
                TurnLabel.Content = "Click 'Start New Game' to begin.";
            }
            else
            {
                TurnLabel.Content =
                    $"Current turn: {game.CurrentTurn}  •  " +
                    $"Mode: {(game is GeneralSOSGame ? "General" : "Simple")}  •  " +
                    $"Size: {game.BoardSize}x{game.BoardSize}";
            }
        }

        // -------------------------------------------------------
        // ⭐ REPLAY BUTTON CLICK
        // -------------------------------------------------------
        private async void ReplayButton_Click(object sender, RoutedEventArgs e)
        {
            if (recordedMoves.Count == 0)
            {
                MessageBox.Show("No moves recorded yet!", "Replay", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (game == null)
            {
                MessageBox.Show("Start a game first.", "Replay", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            BuildBoard(game.BoardSize);

            replayIndex = 0;
            MoveCounterLabel.Content = $"Move 0 / {recordedMoves.Count}";

            int delay = GetReplayDelay();

            foreach (var (row, col, letter) in recordedMoves)
            {
                var btn = boardButtons[row, col];

                btn.Content = letter.ToString();
                btn.Foreground = Brushes.Black;
                btn.Background = Brushes.LightYellow;

                await HighlightCell(btn);

                replayIndex++;
                MoveCounterLabel.Content = $"Move {replayIndex} / {recordedMoves.Count}";

                await Task.Delay(delay);
            }
        }

        // -------------------------------------------------------
        // ⭐ HIGHLIGHT CELL ANIMATION
        // -------------------------------------------------------
        private async Task HighlightCell(Button cell)
        {
            cell.BorderBrush = Brushes.Gold;
            cell.BorderThickness = new Thickness(3);

            await Task.Delay(300);

            cell.BorderBrush = Brushes.Black;
            cell.BorderThickness = new Thickness(1);
        }

        // -------------------------------------------------------
        // ⭐ REPLAY SPEED
        // -------------------------------------------------------
        private int GetReplayDelay()
        {
            string speed = (ReplaySpeedBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            return speed switch
            {
                "Slow" => 700,
                "Fast" => 150,
                _ => 400, // Normal
            };
        }
    }
}
