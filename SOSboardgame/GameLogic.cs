using System;
using System.Linq;

namespace SOSboardgame
{
    public enum GameMode { Simple, General }
    public enum Player { Blue, Red }

    public class GameLogic
    {
        private static readonly int[] AllowedSizes = new[] { 8, 10, 16 };
        public int BoardSize { get; private set; }
        public GameMode Mode { get; private set; }
        public Player CurrentTurn { get; private set; } = Player.Blue;
        public char[,] Board { get; private set; } = new char[1, 1]; // ✅ prevents null warning

        public GameLogic(int size = 8, GameMode mode = GameMode.Simple)
        {
            SetBoardSize(size);
            SetGameMode(mode);
        }

        public void SetBoardSize(int size)
        {
            if (!AllowedSizes.Contains(size))
                throw new ArgumentException("Board size must be one of: 8, 10, or 16.");
            BoardSize = size;
            Board = new char[size, size];
        }

        public void SetGameMode(GameMode mode) => Mode = mode;

        public bool PlaceLetter(int x, int y, char letter)
        {
            if (letter != 'S' && letter != 'O') return false;
            if (x < 0 || y < 0 || x >= BoardSize || y >= BoardSize) return false;
            if (Board[x, y] != '\0') return false;

            Board[x, y] = letter;
            CurrentTurn = (CurrentTurn == Player.Blue) ? Player.Red : Player.Blue;
            return true;
        }

        public void ResetGame()
        {
            Board = new char[BoardSize, BoardSize];
            CurrentTurn = Player.Blue;
        }
    }
}
