using SOSboardgame;
using Xunit;

namespace SOSboardgame.Tests
{
    public class GameControllerTests
    {
        [Fact]
        public void ResetGame_ClearsBoardAndSetsBlueTurn()
        {
            var g = new GameLogic(10);
            g.PlaceLetter(0, 0, 'S');
            g.ResetGame();
            Assert.Equal('\0', g.Board[0, 0]);
            Assert.Equal(Player.Blue, g.CurrentTurn);
        }

        [Fact]
        public void SetGameMode_UpdatesMode_ToGeneral()
        {
            var g = new GameLogic(16);
            g.SetGameMode(GameMode.General);
            Assert.Equal(GameMode.General, g.Mode);
        }

        [Fact]
        public void PlaceLetter_SwitchesTurnBetweenPlayers()
        {
            var g = new GameLogic(8);
            g.PlaceLetter(0, 0, 'S');
            var afterFirst = g.CurrentTurn;
            g.PlaceLetter(1, 1, 'O');
            var afterSecond = g.CurrentTurn;

            Assert.Equal(Player.Red, afterFirst);
            Assert.Equal(Player.Blue, afterSecond);
        }
    }
}
