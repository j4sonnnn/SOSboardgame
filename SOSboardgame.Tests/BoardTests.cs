using SOSboardgame;
using Xunit;

namespace SOSboardgame.Tests
{
    public class BoardTests
    {
        [Theory]
        [InlineData(8)]
        [InlineData(10)]
        [InlineData(16)]
        public void Allowed_Board_Sizes_Work(int size)
        {
            var g = new GameLogic(size);
            Assert.Equal(size, g.BoardSize);
            Assert.Equal('\0', g.Board[0, 0]);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(12)]
        public void Disallowed_Board_Sizes_Throw(int size)
        {
            Assert.Throws<ArgumentException>(() => new GameLogic(size));
        }

        [Fact]
        public void PlaceLetter_StoresValue_And_SwitchesTurn()
        {
            var g = new GameLogic(8);
            bool success = g.PlaceLetter(0, 0, 'S');
            Assert.True(success);
            Assert.Equal('S', g.Board[0, 0]);
            Assert.Equal(Player.Red, g.CurrentTurn);
        }
    }
}
