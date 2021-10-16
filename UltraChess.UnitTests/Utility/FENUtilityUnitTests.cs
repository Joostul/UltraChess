using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using UltraChess.Blazor.Utility;

namespace UltraChess.UnitTests.Utility
{
    [TestClass]
    public class FENUtilityUnitTests
    {
        [TestMethod]
        public void DefaultFENString_GetFENBoard_IsCorrect()
        {
            // Act
            var result = FENUtility.GetBoardPositionInfo("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            // Assert
            result.Board.ShouldBe(new char[64] { 
                'r','n','b','q','k','b','n','r',
                'p','p','p','p','p','p','p','p',
                '0','0','0','0','0','0','0','0',
                '0','0','0','0','0','0','0','0',
                '0','0','0','0','0','0','0','0',
                '0','0','0','0','0','0','0','0',
                'P','P','P','P','P','P','P','P',
                'R','N','B','Q','K','B','N','R'
            });
            result.BlackCanCastleKingSide.ShouldBeTrue();
            result.BlackCanCastleQueenSide.ShouldBeTrue();
            result.WhiteCanCastleKingSide.ShouldBeTrue();
            result.WhiteCanCastleQueenSide.ShouldBeTrue();
            result.HalfClockMove.ShouldBe(0);
            result.FullMoveNumber.ShouldBe(1);
            result.EnPassantSquareId.ShouldBe(64);
        }

        [TestMethod]
        public void OnlyTwoKingsFENString_GetFENBoard_IsCorrect()
        {
            // Act
            var result = FENUtility.GetBoardPositionInfo("k6K/8/8/8/8/8/8/8 w KQkq - 0 1");

            // Assert
            result.Board.ShouldBe(new char[64] {
                'k','0','0','0','0','0','0','K',
                '0','0','0','0','0','0','0','0',
                '0','0','0','0','0','0','0','0',
                '0','0','0','0','0','0','0','0',
                '0','0','0','0','0','0','0','0',
                '0','0','0','0','0','0','0','0',
                '0','0','0','0','0','0','0','0',
                '0','0','0','0','0','0','0','0'
            });
        }

        [TestMethod]
        public void DefaultBoard_GetFENString_IsCorrect()
        {
            // Arrange
            var boardInfo = FENUtility.GetBoardPositionInfo("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            // Act
            var result = FENUtility.GetFENString(boardInfo);

            // Assert
            result.ShouldBe("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }
    }
}
