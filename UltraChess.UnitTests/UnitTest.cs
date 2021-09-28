using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using UltraChess.Blazor.Models;

namespace UltraChess.UnitTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void PossibleMovesFirstMove()
        {
            var possibleMoves = 0;
            var chessBoard = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }
    }
}
