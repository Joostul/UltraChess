using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraChess.Blazor.Models;

namespace UltraChess.UnitTests.Models
{
    [TestClass]
    public class KingUnitTests
    {
        [TestMethod]
        public void GetKingMovesTest()
        {
            // Arrange
            var sut = new ChessBoard("k7/8/8/8/8/8/8/8 w KQkq - 0 1");

            // Act
            var moves = sut.GetMovementSquares(0);
            //var moves = new List<int> { 9, 18, 27, 36, 45, 54, 63 };

            // Assert
            moves.ShouldBe(new List<int> { 1, 8, 9 });
        }
    }
}
