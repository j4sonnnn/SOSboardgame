using System;

namespace SOSboardgame
{
    public abstract class SOSGame
    {
        public int BoardSize { get; protected set; }
        public char[,] Board { get; protected set; }
        public Player CurrentTurn { get; protected set; } = Player.Blue;

        public bool IsGameOver { get; protected set; } = false;
        public Player? Winner { get; protected set; } = null;

        public int BlueScore { get; protected set; } = 0;
        public int RedScore { get; protected set; } = 0;


        protected SOSGame(int size)
        {
            BoardSize = size;
            Board = new char[size, size];
        }

        /// <summary>
        /// Places a letter on the board for the current player.
        /// Returns false if invalid (occupied or game over).
        /// </summary>
        public virtual bool PlaceLetter(int row, int col, char letter)
        {
            if (IsGameOver || Board[row, col] != '\0')
                return false;

            Board[row, col] = letter;
            bool madeSOS = CheckForSOS(row, col);

            if (madeSOS)
            {
                HandleSOS(); // scoring or winning handled by subclass
            }
            else
            {
                SwitchTurn();
            }

            CheckGameOver();
            return true;
        }

        /// <summary>
        /// Switches turns between players.
        /// </summary>
        protected void SwitchTurn()
        {
            if (IsGameOver) return;
            CurrentTurn = (CurrentTurn == Player.Blue) ? Player.Red : Player.Blue;
        }

        /// <summary>
        /// Checks if the board is full.
        /// </summary>
        protected bool IsBoardFull()
        {
            for (int i = 0; i < BoardSize; i++)
                for (int j = 0; j < BoardSize; j++)
                    if (Board[i, j] == '\0')
                        return false;
            return true;
        }

        /// <summary>
        /// Scans 8 directions from the placed letter to find SOS patterns.
        /// </summary>
        protected bool DetectSOSPattern(int x, int y)
        {
            int[][] directions = new int[][]
            {
                new[]{ 0, 1 }, new[]{ 1, 0 }, new[]{ 1, 1 }, new[]{ 1, -1 },
                new[]{ 0, -1 }, new[]{ -1, 0 }, new[]{ -1, 1 }, new[]{ -1, -1 }
            };

            foreach (var dir in directions)
            {
                int dx = dir[0], dy = dir[1];

                try
                {
                    if (Board[x, y] == 'O')
                    {
                        if (Board[x - dx, y - dy] == 'S' && Board[x + dx, y + dy] == 'S')
                            return true;
                    }
                    else if (Board[x, y] == 'S')
                    {
                        if (Board[x + dx, y + dy] == 'O' && Board[x + 2 * dx, y + 2 * dy] == 'S')
                            return true;
                        if (Board[x - dx, y - dy] == 'O' && Board[x - 2 * dx, y - 2 * dy] == 'S')
                            return true;
                    }
                }
                catch { /* ignore out of range */ }
            }

            return false;
        }

        /// <summary>
        /// Must be implemented by each game mode (Simple vs General).
        /// </summary>
        protected abstract void HandleSOS();

        /// <summary>
        /// Each subclass must use DetectSOSPattern(x, y) inside this.
        /// </summary>
        protected abstract bool CheckForSOS(int row, int col);

        /// <summary>
        /// Determines if game is over and who won (if applicable).
        /// </summary>
        protected abstract void CheckGameOver();
    }
}
