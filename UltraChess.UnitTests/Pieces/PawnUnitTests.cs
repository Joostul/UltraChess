using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using UltraChess.Blazor.Models;

namespace UltraChess.UnitTests.Pieces
{
    [TestClass]
    public class PawnUnitTests
    {
        [TestMethod]
        public void GetPawnMoves_DefaultBoard_SquareIndex8()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            // Act
            var moves = sut.GetMovementSquares(8);

            // Assert
            moves.ShouldBe(new List<int> { 16, 24 });
        }

        [TestMethod]
        public void GetPawnMoves_e4e5_SquareIndex36()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq e6 0 2");

            // Act
            var moves = sut.GetMovementSquares(36);

            // Assert
            moves.ShouldBe(new List<int> { });
        }

        [TestMethod]
        public void GetPawnMoves_e4d5_SquareIndex36()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/ppp1pppp/8/3p4/4P3/8/PPPP1PPP/RNBQKBNR w KQkq d6 0 2");

            // Act
            var moves = sut.GetMovementSquares(36);

            // Assert
            moves.ShouldBe(new List<int> { 28, 27 });
        }
    }
}
