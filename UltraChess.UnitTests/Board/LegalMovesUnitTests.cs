using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using UltraChess.Blazor.Models;

namespace UltraChess.UnitTests.Board
{
    [TestClass]
    public class LegalMovesUnitTests
    {
        [TestMethod]
        public void FirstMove_LegalMoves_ShouldBeCorrect()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            // Act

            // Assert
            var expectedMoves = new List<Move>
            {
                new Move(48, 40), new Move(48, 32), // A file pawn
                new Move(49, 41), new Move(49, 33), // B file pawn
                new Move(50, 42), new Move(50, 34), // C file pawn
                new Move(51, 43), new Move(51, 35), // D file pawn
                new Move(52, 44), new Move(52, 36), // E file pawn
                new Move(53, 45), new Move(53, 37), // F file pawn
                new Move(54, 46), new Move(54, 38), // G file pawn
                new Move(55, 47), new Move(55, 39), // H file pawn
                new Move(57, 40), new Move(57, 42), // B1 horse
                new Move(62, 45), new Move(62, 47), // G1 horse
            };

            foreach (var move in expectedMoves)
            {
                sut.LegalMoves.ShouldContain(m => m.ToSquareId == move.ToSquareId && m.FromSquareId == move.FromSquareId && m.CapturedPieceId == move.CapturedPieceId,
                    $"Move from: {move.FromSquareId} to: {move.ToSquareId}, capturing: {move.CapturedPieceId} not found.");
            }

            sut.LegalMoves.Count.ShouldBe(20);
        }

        [TestMethod]
        public void E4_LegalMoves_ShouldBeCorrect()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            // Act
            sut.MakeMove(new Move(52, 36));
            sut.LegalMoves = sut.GenerateLegalMoves(sut.IsWhiteTurn);

            // Assert
            var expectedMoves = new List<Move>
            {
                new Move(8, 24), new Move(8, 16), // A file pawn
                new Move(9, 25), new Move(9, 17), // B file pawn
                new Move(10, 26), new Move(10, 18), // C file pawn
                new Move(11, 27), new Move(11, 19), // D file pawn
                new Move(12, 28), new Move(12, 20), // E file pawn
                new Move(13, 29), new Move(13, 21), // F file pawn
                new Move(14, 30), new Move(14, 22), // G file pawn
                new Move(15, 31), new Move(15, 23), // H file pawn
                new Move(1, 16), new Move(1, 18), // B8 horse
                new Move(6, 23), new Move(6, 21), // G8 horse
            };

            foreach (var move in expectedMoves)
            {
                sut.LegalMoves.ShouldContain(m => m.ToSquareId == move.ToSquareId && m.FromSquareId == move.FromSquareId && m.CapturedPieceId == move.CapturedPieceId, 
                    $"Move from: {move.FromSquareId} to: {move.ToSquareId}, capturing: {move.CapturedPieceId} not found.");
            }

            sut.LegalMoves.Count.ShouldBe(20);
        }

        [TestMethod]
        public void E4F5_LegalMoves_ShouldBeCorrect()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            // Act
            sut.MakeMove(new Move(52, 36));
            sut.LegalMoves = sut.GenerateLegalMoves(sut.IsWhiteTurn);
            sut.MakeMove(new Move(13, 29));
            sut.LegalMoves = sut.GenerateLegalMoves(sut.IsWhiteTurn);

            // Assert
            var expectedMoves = new List<Move>
            {
                new Move(48, 40), new Move(48, 32), // A file pawn
                new Move(49, 41), new Move(49, 33), // B file pawn
                new Move(50, 42), new Move(50, 34), // C file pawn
                new Move(51, 43), new Move(51, 35), // D file pawn
                new Move(36, 28), new Move(36, 29, 7), // E file pawn
                new Move(53, 45), new Move(53, 37), // F file pawn
                new Move(54, 46), new Move(54, 38), // G file pawn
                new Move(55, 47), new Move(55, 39), // H file pawn
                new Move(57, 40), new Move(57, 42), // B1 horse
                new Move(62, 45), new Move(62, 47), new Move(62, 52), // G1 horse
                new Move(59, 52), new Move(59, 45), new Move(59, 38), new Move(59, 31), // D1 queen
                new Move(61, 52), new Move(61, 43), new Move(61, 34), new Move(61, 25), new Move(61, 16), // F1 bishop
                new Move(60, 52) // E1 king
            };

            foreach (var move in expectedMoves)
            {
                sut.LegalMoves.ShouldContain(m => m.ToSquareId == move.ToSquareId && m.FromSquareId == move.FromSquareId && m.CapturedPieceId == move.CapturedPieceId,
                    $"Move from: {move.FromSquareId} to: {move.ToSquareId}, capturing: {move.CapturedPieceId} not found.");
            }

            sut.LegalMoves.Count.ShouldBe(31);
        }

        [TestMethod]
        public void E4F5F5E5_LegalMoves_ShouldBeCorrect()
        {
            // Arrange
            var sut = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            // Act
            sut.MakeMove(new Move(52, 36));
            sut.LegalMoves = sut.GenerateLegalMoves(sut.IsWhiteTurn);
            sut.MakeMove(new Move(13, 29));
            sut.LegalMoves = sut.GenerateLegalMoves(sut.IsWhiteTurn);
            sut.MakeMove(new Move(36, 29));
            sut.LegalMoves = sut.GenerateLegalMoves(sut.IsWhiteTurn);
            sut.MakeMove(new Move(12, 28) { Flag = MoveFlag.PawnTwoForward });
            sut.LegalMoves = sut.GenerateLegalMoves(sut.IsWhiteTurn);

            // Assert
            var expectedMoves = new List<Move>
            {
                new Move(48, 40), new Move(48, 32), // A file pawn
                new Move(49, 41), new Move(49, 33), // B file pawn
                new Move(50, 42), new Move(50, 34), // C file pawn
                new Move(51, 43), new Move(51, 35), // D file pawn
                new Move(53, 45), new Move(53, 37), // F file pawn
                new Move(29, 21), new Move(29, 20, 7), // F file pawn2
                new Move(54, 46), new Move(54, 38), // G file pawn
                new Move(55, 47), new Move(55, 39), // H file pawn
                new Move(57, 40), new Move(57, 42), // B1 horse
                new Move(62, 45), new Move(62, 47), new Move(62, 52), // G1 horse
                new Move(59, 52), new Move(59, 45), new Move(59, 38), new Move(59, 31), // D1 queen
                new Move(61, 52), new Move(61, 43), new Move(61, 34), new Move(61, 25), new Move(61, 16), // F1 bishop
                new Move(60, 52) // E1 king
            };

            foreach (var move in expectedMoves)
            {
                sut.LegalMoves.ShouldContain(m => m.ToSquareId == move.ToSquareId && m.FromSquareId == move.FromSquareId && m.CapturedPieceId == move.CapturedPieceId,
                    $"Move from: {move.FromSquareId} to: {move.ToSquareId}, capturing: {move.CapturedPieceId} not found.");
            }

            sut.LegalMoves.Count.ShouldBe(31);
        }
    }
}
