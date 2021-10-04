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
            var sut = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
            {
                IsWhiteTurn = false
            };

            // Act
            var moves = sut.GetMovesFromSquare(1, sut.IsWhiteTurn);

            // Assert
            var expectedMoves = new List<Move> { new Move(1, 16), new Move(1, 18) };
            foreach (var move in expectedMoves)
            {
                moves.ShouldContain(m => m.ToSquareId == move.ToSquareId && m.FromSquareId == move.FromSquareId && m.CapturedPieceId == move.CapturedPieceId,
                    $"Move from: {move.FromSquareId} to: {move.ToSquareId}, capturing: {move.CapturedPieceId} not found.");
            }
            moves.Count.ShouldBe(2);
        }

        [TestMethod]
        public void GetKnightMoves_DefaultBoard_SquareIndex57()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            // Act
            var moves = sut.GetMovesFromSquare(57, sut.IsWhiteTurn);

            // Assert
            var expectedMoves = new List<Move> { new Move(57, 40), new Move(57, 42) };
            foreach (var move in expectedMoves)
            {
                moves.ShouldContain(m => m.ToSquareId == move.ToSquareId && m.FromSquareId == move.FromSquareId && m.CapturedPieceId == move.CapturedPieceId,
                    $"Move from: {move.FromSquareId} to: {move.ToSquareId}, capturing: {move.CapturedPieceId} not found.");
            }
            moves.Count.ShouldBe(2);
        }
    }
}
