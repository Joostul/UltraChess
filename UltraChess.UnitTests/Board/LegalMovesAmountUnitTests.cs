using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using UltraChess.UnitTests.UnitTestTools;

namespace UltraChess.UnitTests.Board
{
    [TestClass]
    public class LegalMovesAmountUnitTests
    {
        [TestMethod]
        public void DefaultBoard_WhiteToMove_LegalMoves_ShouldBeCorrect()
        {
            // Arrange
            var moveGeneratorTestTool = new MoveGenerator("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1 ");

            // Act
            var numberOfLegalMoves0Ply = moveGeneratorTestTool.GenerateMoves(0);
            var numberOfLegalMoves1Ply = moveGeneratorTestTool.GenerateMoves(1);
            var numberOfLegalMoves2Ply = moveGeneratorTestTool.GenerateMoves(2);
            var numberOfLegalMoves3Ply = moveGeneratorTestTool.GenerateMoves(3);
            var numberOfLegalMoves4Ply = moveGeneratorTestTool.GenerateMoves(4);
            //var numberOfLegalMoves5Ply = moveGeneratorTestTool.GenerateMoves(5);
            //var numberOfLegalMoves6Ply = moveGeneratorTestTool.GenerateMoves(6);

            // Assert
            numberOfLegalMoves0Ply.ShouldBe(1);
            numberOfLegalMoves1Ply.ShouldBe(20);
            numberOfLegalMoves2Ply.ShouldBe(400);
            numberOfLegalMoves3Ply.ShouldBe(8902);
            numberOfLegalMoves4Ply.ShouldBe(197281);
            //numberOfLegalMoves5Ply.ShouldBe(4865609);
            //numberOfLegalMoves6Ply.ShouldBe(119060324);
        }

        [TestMethod]
        public void DefaultBoard_BlackToMove_LegalMoves_ShouldBeCorrect()
        {
            // Arrange
            var moveGeneratorTestTool = new MoveGenerator("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq - 0 1 ");

            // Act
            var numberOfLegalMoves0Ply = moveGeneratorTestTool.GenerateMoves(0);
            var numberOfLegalMoves1Ply = moveGeneratorTestTool.GenerateMoves(1);
            var numberOfLegalMoves2Ply = moveGeneratorTestTool.GenerateMoves(2);
            var numberOfLegalMoves3Ply = moveGeneratorTestTool.GenerateMoves(3);
            var numberOfLegalMoves4Ply = moveGeneratorTestTool.GenerateMoves(4);
            //var numberOfLegalMoves5Ply = moveGeneratorTestTool.GenerateMoves(5);
            //var numberOfLegalMoves6Ply = moveGeneratorTestTool.GenerateMoves(6);

            // Assert
            numberOfLegalMoves0Ply.ShouldBe(1);
            numberOfLegalMoves1Ply.ShouldBe(20);
            numberOfLegalMoves2Ply.ShouldBe(400);
            numberOfLegalMoves3Ply.ShouldBe(8902);
            numberOfLegalMoves4Ply.ShouldBe(197281);
            //numberOfLegalMoves5Ply.ShouldBe(4865609);
            //numberOfLegalMoves6Ply.ShouldBe(119060324);
        }

        [TestMethod]
        public void Board1_WhiteToMove_LegalMoves_ShouldBeCorrect()
        {
            // Arrange
            var moveGeneratorTestTool = new MoveGenerator("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1");

            // Act
            var numberOfLegalMoves0Ply = moveGeneratorTestTool.GenerateMoves(0);
            numberOfLegalMoves0Ply.ShouldBe(1);
            var numberOfLegalMoves1Ply = moveGeneratorTestTool.GenerateMoves(1);
            numberOfLegalMoves1Ply.ShouldBe(48);
            var numberOfLegalMoves2Ply = moveGeneratorTestTool.GenerateMoves(2);
            numberOfLegalMoves2Ply.ShouldBe(2039);
            var numberOfLegalMoves3Ply = moveGeneratorTestTool.GenerateMoves(3);
            numberOfLegalMoves3Ply.ShouldBe(97862);
            var numberOfLegalMoves4Ply = moveGeneratorTestTool.GenerateMoves(4);
            numberOfLegalMoves4Ply.ShouldBe(4085603);
            //var numberOfLegalMoves5Ply = moveGeneratorTestTool.GenerateMoves(5);
            //numberOfLegalMoves5Ply.ShouldBe(193690690);
            //var numberOfLegalMoves6Ply = moveGeneratorTestTool.GenerateMoves(6);
            //numberOfLegalMoves6Ply.ShouldBe(8031647685);

            // Assert
        }

        [TestMethod]
        public void GetNumberOfLegalMoves()
        {
            // Arrange
            var moveGeneratorTestTool = new MoveGenerator("r3k2r/p1p1qpb1/bn1ppnp1/1B1PN3/1p2P3/2N2Q1p/PPPB1PPP/1R2K2R b Kkq - 1 2");

            // Act
            var numberOfLegalMoves = moveGeneratorTestTool.GenerateMoves(1, true);

            // Assert
            numberOfLegalMoves.ShouldBe(7);
        }
    }
}
