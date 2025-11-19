namespace SOSboardgame
{
    public class GeneralSOSGame : SOSGame
    {
        public GeneralSOSGame(int size) : base(size) { }

        protected override bool CheckForSOS(int r, int c)
            => DetectSOSPattern(r, c);

        protected override void HandleSOS()
        {
            if (CurrentTurn == Player.Blue)
                BlueScore++;
            else
                RedScore++;

            SwitchTurn();
        }

        protected override void CheckGameOver()
        {
            if (!Board.IsFull()) return;

            IsGameOver = true;

            if (BlueScore > RedScore)
                Winner = Player.Blue;
            else if (RedScore > BlueScore)
                Winner = Player.Red;
            else
                Winner = null;
        }
    }
}
