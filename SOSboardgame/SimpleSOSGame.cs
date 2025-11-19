using System.Windows;

namespace SOSboardgame
{
    public class SimpleSOSGame : SOSGame
    {
        public SimpleSOSGame(int size) : base(size) { }

        protected override bool CheckForSOS(int r, int c)
            => DetectSOSPattern(r, c);

        protected override void HandleSOS()
        {
            Winner = CurrentTurn;
            IsGameOver = true;
        }

        protected override void CheckGameOver()
        {
            if (IsGameOver) return;

            if (Board.IsFull())
            {
                Winner = null;
                IsGameOver = true;
            }
        }
    }
}
