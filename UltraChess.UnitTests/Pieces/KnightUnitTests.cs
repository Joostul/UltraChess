using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using UltraChess.Blazor.Models;

namespace UltraChess.UnitTests.Pieces
{
    [TestClass]
    public class KnightUnitTests
    {
        [TestMethod]
        public void GetKnightMoves_DefaultBoard_SquareIndex1()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            // Act
            var moves = sut.GetMovementSquares(1);

            // Assert
            moves.ShouldBe(new List<int> { 16, 18 });
        }

        [TestMethod]
        public void GetKnightMoves_DefaultBoard_SquareIndex57()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            // Act
            var moves = sut.GetMovementSquares(57);

            // Assert
            moves.ShouldBe(new List<int> { 40, 42 });
        }
    }
}
