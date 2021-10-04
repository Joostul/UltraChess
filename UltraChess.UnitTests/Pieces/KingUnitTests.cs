//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Shouldly;
//using System.Collections.Generic;
//using System.Linq;
//using UltraChess.Blazor.Models;

//namespace UltraChess.UnitTests.Pieces
//{
//    [TestClass]
//    public class KingUnitTests
//    {
//        [TestMethod]
//        public void GetKingMovesTest()
//        {
//            // Arrange
//            var sut = new ChessBoard("k7/8/8/8/8/8/8/8 w KQkq - 0 1")
//            {
//                IsWhiteTurn = false
//            };

//            // Act
//            var moves = sut.GetMovementSquares(0, sut.IsWhiteTurn);
//            //var moves = new List<int> { 9, 18, 27, 36, 45, 54, 63 };

//            // Assert
//            moves.OrderBy(m => m).ShouldBe(new List<int> { 1, 8, 9 });
//        }
//    }
//}
