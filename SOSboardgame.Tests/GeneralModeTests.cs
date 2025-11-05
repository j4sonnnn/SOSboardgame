using Xunit;

namespace SOSboardgame.Tests
{
    public class GeneralModeTests
    {
        [Fact]
        public void CountsMultipleSOS()
        {
            // Arrange
            var g = new GeneralSOSGame(3);

            // Act
            g.PlaceLetter(0, 0, 'S');
            g.PlaceLetter(0, 1, 'O');
            g.PlaceLetter(0, 2, 'S'); // Blue gets 1 point
            g.PlaceLetter(1, 0, 'S');
            g.PlaceLetter(1, 1, 'O');
            g.PlaceLetter(1, 2, 'S'); // Red gets 1 point

            // Assert
            Assert.Equal(1, g.BlueScore);
            Assert.Equal(1, g.RedScore);
            Assert.False(g.IsGameOver);
        }

        [Fact]
        public void BlueWinsWhenHasHigherScore()
        {
            var g = new GeneralSOSGame(3);
            g.PlaceLetter(0, 0, 'S');
            g.PlaceLetter(0, 1, 'O');
            g.PlaceLetter(0, 2, 'S'); // Blue 1
            g.PlaceLetter(1, 0, 'S');
            g.PlaceLetter(1, 1, 'S');
            g.PlaceLetter(1, 2, 'S'); // filler moves
            g.PlaceLetter(2, 0, 'S');
            g.PlaceLetter(2, 1, 'O');
            g.PlaceLetter(2, 2, 'S'); // Red 1

            // Fill rest of board
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (g.Board[i, j] == '\0') g.PlaceLetter(i, j, 'S');

            // Act
            g.PlaceLetter(0, 0, 'S'); // trigger CheckGameOver

            // Assert
            Assert.True(g.IsGameOver);
            Assert.Equal(Player.Blue, g.Winner);
        }

        [Fact]
        public void DrawIfEqualScores()
        {
            var g = new GeneralSOSGame(3);
            g.PlaceLetter(0, 0, 'S');
            g.PlaceLetter(0, 1, 'O');
            g.PlaceLetter(0, 2, 'S'); // Blue 1
            g.PlaceLetter(1, 0, 'S');
            g.PlaceLetter(1, 1, 'O');
            g.PlaceLetter(1, 2, 'S'); // Red 1

            // Fill rest of board with no more SOS
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (g.Board[i, j] == '\0') g.PlaceLetter(i, j, 'S');

            Assert.True(g.IsGameOver);
            Assert.Null(g.Winner);
        }
    }
}
