using System;
using System.Windows;

namespace SOSboardgame
{
    public class GeneralSOSGame : SOSGame
    {
        public GeneralSOSGame(int size) : base(size) { }

        protected override bool CheckForSOS(int row, int col)
        {
            return DetectSOSPattern(row, col);
        }

        protected override void HandleSOS()
        {
            if (CurrentTurn == Player.Blue)
                BlueScore++;
            else
                RedScore++;
            // In General mode, do not switch turns if player scores
        }

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
