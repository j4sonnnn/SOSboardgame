using Xunit;

namespace SOSboardgame.Tests
{
    public class SimpleModeTests
    {
        [Fact]
        public void BlueWinsOnFirstSOS()
        {
            // Arrange
            var g = new SimpleSOSGame(3);

            // Act
            g.PlaceLetter(0, 0, 'S');
            g.PlaceLetter(0, 1, 'O');
            g.PlaceLetter(0, 2, 'S');

            // Assert
            Assert.True(g.IsGameOver);
            Assert.Equal(Player.Blue, g.Winner);
        }

        [Fact]
        public void DrawIfBoardFullNoSOS()
        {
            // Arrange
            var g = new SimpleSOSGame(3);

            // Act - fill board with no SOS
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    g.PlaceLetter(i, j, 'S');

            // Assert
            Assert.True(g.IsGameOver);
            Assert.Null(g.Winner); // draw
        }

        [Fact]
        public void NoWinnerUntilSOSMade()
        {
            var g = new SimpleSOSGame(3);
            g.PlaceLetter(0, 0, 'S');
            g.PlaceLetter(1, 1, 'O');
            Assert.False(g.IsGameOver);
            Assert.Null(g.Winner);
        }
    }
}
