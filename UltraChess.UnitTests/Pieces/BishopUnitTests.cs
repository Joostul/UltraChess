using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using UltraChess.Blazor.Models;

namespace UltraChess.UnitTests.Pieces
{
    [TestClass]
    public class BishopUnitTests
    {
        [TestMethod]
        public void GetSlidingMovesTest_DefaultBoard()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            // Act
            var moves = sut.GetMovementSquares(2);

            // Assert
            moves.ShouldBe(new List<int> {  });
        }

        [TestMethod]
        public void GetSlidingMovesTest_DefaultBoard_MoveBishopOut()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/pppp1ppp/8/4p3/2B1P3/8/PPPP1PPP/RNBQK1NR b KQkq - 1 2");

            // Act
            var moves = sut.GetMovementSquares(5);

            // Assert
            moves.ShouldBe(new List<int> { 12, 19, 26, 33, 40 });
        }

        [TestMethod]
        public void GetSlidingMovesTest_OnlyBishopOnBoard_SquareIndex0()
        {
            // Arrange
            var sut = new ChessBoard("b7/8/8/8/8/8/8/8 w KQkq - 0 1")
            {
                IsWhiteTurn = false
            };

            // Act
            var moves = sut.GetMovementSquares(0);

            // Assert
            moves.ShouldBe(new List<int> { 9, 18, 27, 36, 45, 54, 63 });
        }
    }
}
