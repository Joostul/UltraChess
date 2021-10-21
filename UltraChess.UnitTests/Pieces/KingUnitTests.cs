using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using UltraChess.Blazor.Models;

namespace UltraChess.UnitTests.Pieces
{
    [TestClass]
    public class KingUnitTests
    {
        [TestMethod]
        public void GetKingMovesTest()
        {
            // Arrange
            var sut = new ChessBoard("k6K/8/8/8/8/8/8/8 w KQkq - 0 1");
            sut.CurrentBoardInfo.IsWhiteTurn = false;

            // Act
            var moves = sut.GetMovesFromSquare(0, sut.CurrentBoardInfo.IsWhiteTurn);

            // Assert
            var expectedMoves = new List<Move> { new Move(0, 1), new Move(0, 8), new Move(0, 9)};
            foreach (var move in expectedMoves)
            {
                moves.ShouldContain(m => m.ToSquareId == move.ToSquareId && m.FromSquareId == move.FromSquareId && m.CapturedPieceId == move.CapturedPieceId,
                    $"Move from: {move.FromSquareId} to: {move.ToSquareId}, capturing: {move.CapturedPieceId} not found.");
            }
            moves.Count.ShouldBe(3);
        }
    }
}
