using System;

namespace SOSboardgame
{
    public abstract class SOSGame
    {
        public int BoardSize { get; }
        public Board Board { get; }
        public Player CurrentTurn { get; protected set; } = Player.Blue;

        public bool IsGameOver { get; protected set; } = false;
        public Player? Winner { get; protected set; } = null;

        public int BlueScore { get; protected set; } = 0;
        public int RedScore { get; protected set; } = 0;

        protected SOSGame(int size)
        {
            BoardSize = size;
            Board = new Board(size);
        }

        public virtual bool PlaceLetter(int row, int col, char letter)
        {
            if (IsGameOver) return false;
            if (Board[row, col] != '\0') return false;

            Board[row, col] = letter;
            bool madeSOS = CheckForSOS(row, col);

            if (madeSOS)
                HandleSOS();
            else
                SwitchTurn();

            CheckGameOver();
            return true;
        }

        protected void SwitchTurn()
        {
            if (!IsGameOver)
                CurrentTurn = CurrentTurn == Player.Blue ? Player.Red : Player.Blue;
        }

        protected bool DetectSOSPattern(int r, int c)
        {
            char[,] b = Board.Cells;
            int N = BoardSize;

            int[][] dirs = new int[][]
            {
                new[]{0,1}, new[]{1,0}, new[]{1,1}, new[]{1,-1},
                new[]{0,-1}, new[]{-1,0}, new[]{-1,1}, new[]{-1,-1}
            };

            foreach (var d in dirs)
            {
                int dx = d[0], dy = d[1];

                try
                {
                    if (b[r, c] == 'O')
                    {
                        if (b[r - dx, c - dy] == 'S' &&
                            b[r + dx, c + dy] == 'S')
                            return true;
                    }
                    else if (b[r, c] == 'S')
                    {
                        if (b[r + dx, c + dy] == 'O' && b[r + 2 * dx, c + 2 * dy] == 'S')
                            return true;
                        if (b[r - dx, c - dy] == 'O' && b[r - 2 * dx, c - 2 * dy] == 'S')
                            return true;
                    }
                }
                catch { }
            }

            return false;
        }

        protected abstract bool CheckForSOS(int row, int col);
        protected abstract void HandleSOS();
        protected abstract void CheckGameOver();
    }
}
