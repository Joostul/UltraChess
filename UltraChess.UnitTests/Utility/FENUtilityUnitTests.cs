using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using UltraChess.Blazor.Utility;

namespace UltraChess.UnitTests.Utility
{
    [TestClass]
    public class FENUtilityUnitTests
    {
        [TestMethod]
        public void DefaultFENString_GetFENBoard_IsCorret()
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
        public void DefaultFENString_GetIsWhiteTurn_IsCorrect()
        {
            // Act
            var result = FENUtility.GetBoardPositionInfo("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            // Assert
            result.IsWhiteTurn.ShouldBeTrue();
        }

        [TestMethod]
        public void E4FENString_GetIsWhiteTurn_IsCorrect()
        {
            // Act
            var result = FENUtility.GetBoardPositionInfo("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1");

            // Assert
            result.IsWhiteTurn.ShouldBeFalse();
        }
    }
}
