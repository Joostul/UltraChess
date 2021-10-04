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
            var moves = sut.GetMovesFromSquare(8, sut.IsWhiteTurn);

            // Assert
            moves.ShouldBe(new List<Move> { });
            moves.Count.ShouldBe(0);
        }

        [TestMethod]
        public void GetPawnMoves_e4e5_SquareIndex36()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq e6 0 2");

            // Act
            var moves = sut.GetMovesFromSquare(36, sut.IsWhiteTurn);

            // Assert
            moves.ShouldBe(new List<Move> { });
            moves.Count.ShouldBe(0);
        }

        [TestMethod]
        public void GetPawnMoves_e4d5_SquareIndex36()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            sut.MakeMove(52, 36);
            sut.MakeMove(13, 29);

            // Act
            var moves = sut.GetMovesFromSquare(36, sut.IsWhiteTurn);

            // Assert
            var expectedMoves = new List<Move> { new Move(36, 28), new Move(36, 29, 7) };
            foreach (var move in expectedMoves)
            {
                moves.ShouldContain(m => m.ToSquareId == move.ToSquareId && m.FromSquareId == move.FromSquareId && m.CapturedPieceId == move.CapturedPieceId,
                    $"Move from: {move.FromSquareId} to: {move.ToSquareId}, capturing: {move.CapturedPieceId} not found.");
            }
            moves.Count.ShouldBe(2);
        }

        [TestMethod]
        public void GetPawnMoves_EnPassant()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            sut.MakeMove(52, 36);
            sut.MakeMove(8, 16);
            sut.MakeMove(36, 28);
            sut.MakeMove(11, 27);

            // Act
            var moves = sut.GetMovesFromSquare(28, sut.IsWhiteTurn);

            // Assert
            var expectedMoves = new List<Move> { new Move(28, 20), new Move(28, 19, 7) };
            foreach (var move in expectedMoves)
            {
                moves.ShouldContain(m => m.ToSquareId == move.ToSquareId && m.FromSquareId == move.FromSquareId && m.CapturedPieceId == move.CapturedPieceId,
                    $"Move from: {move.FromSquareId} to: {move.ToSquareId}, capturing: {move.CapturedPieceId} not found.");
            }
            moves.Count.ShouldBe(2);
        }
    }
}
