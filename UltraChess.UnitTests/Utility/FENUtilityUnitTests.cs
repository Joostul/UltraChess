using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using UltraChess.Blazor.Utility;

namespace UltraChess.UnitTests.Utility
{
    [TestClass]
    public class FENUtilityUnitTests
    {
        [TestMethod]
        public void DefaultFENString()
        {
            // Act
            var result = FENUtility.ParseFENString("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            // Assert
            result.ShouldBe(new char[64] { 
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
    }
}
