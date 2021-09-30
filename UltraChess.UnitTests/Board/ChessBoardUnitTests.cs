using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using UltraChess.Blazor.Models;

namespace UltraChess.UnitTests.Board
{
    [TestClass]
    public class ChessBoardUnitTests
    {
        [TestMethod]
        public void GetNumberOfSquaresToEdge_SquareIndex0()
        {
            // Arrange / Act
            var sut = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

            // Assert
            sut.NumberOfSquaresToEdge[0][0].ShouldBe(0); // North
            sut.NumberOfSquaresToEdge[0][1].ShouldBe(7); // East
            sut.NumberOfSquaresToEdge[0][2].ShouldBe(7); // South
            sut.NumberOfSquaresToEdge[0][3].ShouldBe(0); // West
            sut.NumberOfSquaresToEdge[0][4].ShouldBe(0); // NorthWest
            sut.NumberOfSquaresToEdge[0][5].ShouldBe(0); // NorthEast
            sut.NumberOfSquaresToEdge[0][6].ShouldBe(7); // SouthEast
            sut.NumberOfSquaresToEdge[0][7].ShouldBe(0); // SouthWest
        }

        [TestMethod]
        public void GetNumberOfSquaresToEdge_SquareIndex3()
        {
            // Arrange / Act
            var sut = new ChessBoard("8/8/8/8/8/8/8/8 w KQkq - 0 1");

            // Assert
            sut.NumberOfSquaresToEdge[3][0].ShouldBe(0); // North
            sut.NumberOfSquaresToEdge[3][1].ShouldBe(7); // East
            sut.NumberOfSquaresToEdge[3][2].ShouldBe(4); // South
            sut.NumberOfSquaresToEdge[3][3].ShouldBe(3); // West
            sut.NumberOfSquaresToEdge[3][4].ShouldBe(0); // NorthWest
            sut.NumberOfSquaresToEdge[3][5].ShouldBe(0); // NorthEast
            sut.NumberOfSquaresToEdge[3][6].ShouldBe(4); // SouthEast
            sut.NumberOfSquaresToEdge[3][7].ShouldBe(3); // SouthWest
        }

        [TestMethod]
        public void GetNumberOfSquaresToEdge_SquareIndex5()
        {
            // Arrange / Act
            var sut = new ChessBoard("8/8/8/8/8/8/8/8 w KQkq - 0 1");

            // Assert
            sut.NumberOfSquaresToEdge[5][0].ShouldBe(0); // North
            sut.NumberOfSquaresToEdge[5][1].ShouldBe(7); // East
            sut.NumberOfSquaresToEdge[5][2].ShouldBe(2); // South
            sut.NumberOfSquaresToEdge[5][3].ShouldBe(5); // West
            sut.NumberOfSquaresToEdge[5][4].ShouldBe(0); // NorthWest
            sut.NumberOfSquaresToEdge[5][5].ShouldBe(0); // NorthEast
            sut.NumberOfSquaresToEdge[5][6].ShouldBe(2); // SouthEast
            sut.NumberOfSquaresToEdge[5][7].ShouldBe(5); // SouthWest
        }

        [TestMethod]
        public void GetNumberOfSquaresToEdge_SquareIndex8()
        {
            // Arrange / Act
            var sut = new ChessBoard("8/8/8/8/8/8/8/8 w KQkq - 0 1");

            // Assert
            sut.NumberOfSquaresToEdge[8][0].ShouldBe(1); // North
            sut.NumberOfSquaresToEdge[8][1].ShouldBe(6); // South
            sut.NumberOfSquaresToEdge[8][2].ShouldBe(7); // East
            sut.NumberOfSquaresToEdge[8][3].ShouldBe(0); // West
            sut.NumberOfSquaresToEdge[8][4].ShouldBe(0); // NorthWest
            sut.NumberOfSquaresToEdge[8][5].ShouldBe(1); // NorthEast
            sut.NumberOfSquaresToEdge[8][6].ShouldBe(6); // SouthEast
            sut.NumberOfSquaresToEdge[8][7].ShouldBe(0); // SouthWest
        }

        [TestMethod]
        public void GetNumberOfSquaresToEdge_SquareIndex16()
        {
            // Arrange / Act
            var sut = new ChessBoard("8/8/8/8/8/8/8/8 w KQkq - 0 1");

            // Assert
            sut.NumberOfSquaresToEdge[16][0].ShouldBe(2); // North
            sut.NumberOfSquaresToEdge[16][1].ShouldBe(5); // South
            sut.NumberOfSquaresToEdge[16][2].ShouldBe(7); // East
            sut.NumberOfSquaresToEdge[16][3].ShouldBe(0); // West
            sut.NumberOfSquaresToEdge[16][4].ShouldBe(0); // NorthWest
            sut.NumberOfSquaresToEdge[16][5].ShouldBe(2); // NorthEast
            sut.NumberOfSquaresToEdge[16][6].ShouldBe(5); // SouthEast
            sut.NumberOfSquaresToEdge[16][7].ShouldBe(0); // SouthWest
        }

        [TestMethod]
        public void GetNumberOfSquaresToEdge_SquareIndex28()
        {
            // Arrange / Act
            var sut = new ChessBoard("8/8/8/8/8/8/8/8 w KQkq - 0 1");

            // Assert
            sut.NumberOfSquaresToEdge[28][0].ShouldBe(3); // North
            sut.NumberOfSquaresToEdge[28][1].ShouldBe(4); // South
            sut.NumberOfSquaresToEdge[28][2].ShouldBe(3); // East
            sut.NumberOfSquaresToEdge[28][3].ShouldBe(4); // West
            sut.NumberOfSquaresToEdge[28][4].ShouldBe(3); // NorthWest
            sut.NumberOfSquaresToEdge[28][5].ShouldBe(3); // NorthEast
            sut.NumberOfSquaresToEdge[28][6].ShouldBe(3); // SouthEast
            sut.NumberOfSquaresToEdge[28][7].ShouldBe(4); // SouthWest
        }

        [TestMethod]
        public void GetNumberOfSquaresToEdge_SquareIndex60()
        {
            // Arrange / Act
            var sut = new ChessBoard("8/8/8/8/8/8/8/8 w KQkq - 0 1");

            // Assert
            sut.NumberOfSquaresToEdge[60][0].ShouldBe(7); // North
            sut.NumberOfSquaresToEdge[60][1].ShouldBe(0); // South
            sut.NumberOfSquaresToEdge[60][2].ShouldBe(3); // East
            sut.NumberOfSquaresToEdge[60][3].ShouldBe(4); // West
            sut.NumberOfSquaresToEdge[60][4].ShouldBe(4); // NorthWest
            sut.NumberOfSquaresToEdge[60][5].ShouldBe(3); // NorthEast
            sut.NumberOfSquaresToEdge[60][6].ShouldBe(0); // SouthEast
            sut.NumberOfSquaresToEdge[60][7].ShouldBe(0); // SouthWest
        }

        [TestMethod]
        public void GetNumberOfSquaresToEdge_SquareIndex63()
        {
            // Arrange / Act
            var sut = new ChessBoard("8/8/8/8/8/8/8/8 w KQkq - 0 1");

            // Assert
            sut.NumberOfSquaresToEdge[63][0].ShouldBe(7); // North
            sut.NumberOfSquaresToEdge[63][1].ShouldBe(0); // South
            sut.NumberOfSquaresToEdge[63][2].ShouldBe(0); // East
            sut.NumberOfSquaresToEdge[63][3].ShouldBe(7); // West
            sut.NumberOfSquaresToEdge[63][4].ShouldBe(7); // NorthWest
            sut.NumberOfSquaresToEdge[63][5].ShouldBe(0); // NorthEast
            sut.NumberOfSquaresToEdge[63][6].ShouldBe(0); // SouthEast
            sut.NumberOfSquaresToEdge[63][7].ShouldBe(0); // SouthWest
        }

    }
}
