using System;

namespace SOSboardgame
{
    public enum PlayerType
    {
        Human,
        Computer
    }

    public abstract class SOSPlayer
    {
        public Player Color { get; }
        public PlayerType Type { get; }

        protected SOSPlayer(Player color, PlayerType type)
        {
            Color = color;
            Type = type;
        }

        public abstract (int row, int col, char letter) ChooseMove(SOSGame game);
    }

    public sealed class HumanPlayer : SOSPlayer
    {
        public HumanPlayer(Player color)
            : base(color, PlayerType.Human) { }

        public override (int row, int col, char letter) ChooseMove(SOSGame game)
        {
            throw new InvalidOperationException("Human moves come from UI.");
        }
    }

    public sealed class ComputerPlayer : SOSPlayer
    {
        private readonly Random rand = new Random();

        public ComputerPlayer(Player color)
            : base(color, PlayerType.Computer) { }

        public override (int row, int col, char letter) ChooseMove(SOSGame game)
        {
            for (int r = 0; r < game.BoardSize; r++)
            {
                for (int c = 0; c < game.BoardSize; c++)
                {
                    if (game.Board[r, c] == '\0')
                    {
                        char letter = rand.Next(2) == 0 ? 'S' : 'O';
                        return (r, c, letter);
                    }
                }
            }

            return (-1, -1, ' ');
        }
    }
}
