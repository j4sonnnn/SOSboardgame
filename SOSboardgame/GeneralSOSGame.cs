using System;
using System.Windows;

namespace SOSboardgame
{
    public class GeneralSOSGame : SOSGame
    {
        public GeneralSOSGame(int size) : base(size) { }

        /// <summary>
        /// Checks if an SOS pattern was formed by the latest move.
        /// </summary>
        protected override bool CheckForSOS(int row, int col)
        {
            return DetectSOSPattern(row, col);
        }

        /// <summary>
        /// Handles scoring when an SOS is detected.
        /// </summary>
        protected override void HandleSOS()
        {
            if (CurrentTurn == Player.Blue)
                BlueScore++;
            else
                RedScore++;

            // ✅ In General Mode, players take turns even after scoring.
            // This ensures fairness between both players.
            SwitchTurn();
        }

        /// <summary>
        /// Checks if the game is over and determines the winner or draw.
        /// </summary>
        protected override void CheckGameOver()
        {
            if (!IsBoardFull()) return;

            IsGameOver = true;

            if (BlueScore > RedScore)
                Winner = Player.Blue;
            else if (RedScore > BlueScore)
                Winner = Player.Red;
            else
                Winner = null;

            string result = Winner == null ? "Draw!" : $"{Winner} wins!";
            MessageBox.Show($"{result}\nBlue: {BlueScore} | Red: {RedScore}", "General Mode");
        }
    }
}
