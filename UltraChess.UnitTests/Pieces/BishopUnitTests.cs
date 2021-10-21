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
            var moves = sut.GetMovesFromSquare(2, sut.CurrentBoardInfo.IsWhiteTurn);

            // Assert
            moves.ShouldBe(new List<Move> { });
        }

        [TestMethod]
        public void GetSlidingMovesTest_DefaultBoard_MoveBishopOut()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/pppp1ppp/8/4p3/2B1P3/8/PPPP1PPP/RNBQK1NR b KQkq - 1 2");

            // Act
            var moves = sut.GetMovesFromSquare(5, sut.CurrentBoardInfo.IsWhiteTurn);

            // Assert
            var expectedMoves = new List<Move> { new Move(5, 12), new Move(5, 19), new Move(5, 26), new Move(5, 33), new Move(5, 40) };
            foreach (var move in expectedMoves)
            {
                moves.ShouldContain(m => m.ToSquareId == move.ToSquareId && m.FromSquareId == move.FromSquareId && m.CapturedPieceId == move.CapturedPieceId,
                    $"Move from: {move.FromSquareId} to: {move.ToSquareId}, capturing: {move.CapturedPieceId} not found.");
            }
            moves.Count.ShouldBe(5);
        }

        [TestMethod]
        public void GetSlidingMovesTest_OnlyBishopOnBoard_SquareIndex0()
        {
            // Arrange
            var sut = new ChessBoard("b1K1k4/8/8/8/8/8/8/8 w KQkq - 0 1");
            sut.CurrentBoardInfo.IsWhiteTurn = false;

            // Act
            var moves = sut.GetMovesFromSquare(0, sut.CurrentBoardInfo.IsWhiteTurn);

            // Assert
            var expectedMoves = new List<Move> { new Move(0, 9), new Move(0, 18), new Move(0, 27), new Move(0, 36), new Move(0, 45), new Move(0, 54), new Move(0, 63) };
            foreach (var move in expectedMoves)
            {
                moves.ShouldContain(m => m.ToSquareId == move.ToSquareId && m.FromSquareId == move.FromSquareId && m.CapturedPieceId == move.CapturedPieceId,
                    $"Move from: {move.FromSquareId} to: {move.ToSquareId}, capturing: {move.CapturedPieceId} not found.");
            }
            moves.Count.ShouldBe(7);
        }
    }
}
