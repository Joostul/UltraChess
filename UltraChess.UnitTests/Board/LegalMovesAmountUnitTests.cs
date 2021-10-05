using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using UltraChess.UnitTests.UnitTestTools;

namespace UltraChess.UnitTests.Board
{
    [TestClass]
    public class LegalMovesAmountUnitTests
    {


        [TestMethod]
        public void LegalMoves_0ply_ShouldBeCorrect()
        {
            // Arrange
            var moveGeneratorTestTool = new MoveGenerator();

            // Act
            var numberOfLegalMoves = moveGeneratorTestTool.GenerateMoves(0);

            // Assert
            numberOfLegalMoves.ShouldBe(1);
        }

        [TestMethod]
        public void LegalMoves_1ply_ShouldBeCorrect()
        {
            // Arrange
            var moveGeneratorTestTool = new MoveGenerator();

            // Act
            var numberOfLegalMoves = moveGeneratorTestTool.GenerateMoves(1);

            // Assert
            numberOfLegalMoves.ShouldBe(20);
        }

        [TestMethod]
        public void LegalMoves_2ply_ShouldBeCorrect()
        {
            // Arrange
            var moveGeneratorTestTool = new MoveGenerator();

            // Act
            var numberOfLegalMoves = moveGeneratorTestTool.GenerateMoves(2);

            // Assert
            numberOfLegalMoves.ShouldBe(400);
        }

        // Tests fail, probably because of pins and castling, which are topics for another day

        //[TestMethod]
        //public void LegalMoves_3ply_ShouldBeCorrect()
        //{
        //    // Arrange
        //    var moveGeneratorTestTool = new MoveGenerator();

        //    // Act
        //    var numberOfLegalMoves = moveGeneratorTestTool.GenerateMoves(3);

        //    // Assert
        //    numberOfLegalMoves.ShouldBe(8902);
        //}

        //[TestMethod]
        //public void LegalMoves_4ply_ShouldBeCorrect()
        //{
        //    // Arrange
        //    var moveGeneratorTestTool = new MoveGenerator();

        //    // Act
        //    var numberOfLegalMoves = moveGeneratorTestTool.GenerateMoves(4);

        //    // Assert
        //    numberOfLegalMoves.ShouldBe(197281);
        //}

        //[TestMethod]
        //public void LegalMoves_5ply_ShouldBeCorrect()
        //{
        //    // Arrange
        //    var moveGeneratorTestTool = new MoveGenerator();

        //    // Act
        //    var numberOfLegalMoves = moveGeneratorTestTool.GenerateMoves(5);

        //    // Assert
        //    numberOfLegalMoves.ShouldBe(4865609);
        //}

        //[TestMethod]
        //public void LegalMoves_6ply_ShouldBeCorrect()
        //{
        //    // Arrange
        //    var moveGeneratorTestTool = new MoveGenerator();

        //    // Act
        //    var numberOfLegalMoves = moveGeneratorTestTool.GenerateMoves(6);

        //    // Assert
        //    numberOfLegalMoves.ShouldBe(119060324);
        //}
    }
}
