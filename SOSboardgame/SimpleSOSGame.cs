using System;
using System.Windows;

namespace SOSboardgame
{
    public class SimpleSOSGame : SOSGame
    {
        public SimpleSOSGame(int size) : base(size) { }

        protected override bool CheckForSOS(int row, int col)
        {
            return DetectSOSPattern(row, col);
        }

        protected override void HandleSOS()
        {
            Winner = CurrentTurn;
            IsGameOver = true;
            MessageBox.Show($"{Winner} wins!", "Simple Mode");
        }

        protected override void CheckGameOver()
        {
            // In Simple Mode, game ends instantly after SOS.
            if (IsGameOver) return;

            if (IsBoardFull())
            {
                IsGameOver = true;
                Winner = null;
                MessageBox.Show("Draw! No more moves left.", "Simple Mode");
            }
        }
    }
}
