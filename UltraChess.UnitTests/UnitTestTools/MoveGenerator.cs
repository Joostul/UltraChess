using System.Collections.Generic;
using UltraChess.Blazor.Models;

namespace UltraChess.UnitTests.UnitTestTools
{
    public class MoveGenerator
    {
        private readonly ChessBoard ChessBoard;

        public MoveGenerator()
        {
            ChessBoard = new ChessBoard("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }

        public int GenerateMoves(int depth)
        {
            if(depth == 0)
            {
                return 1;
            }

            List<Move> moves = ChessBoard.GenerateLegalMoves(ChessBoard.IsWhiteTurn);
            int numberOfPosiitions = 0;

            foreach (var move in moves)
            {
                var moveMade = ChessBoard.MakeMove(move.FromSquareId, move.ToSquareId);
                numberOfPosiitions += GenerateMoves(depth - 1);
                ChessBoard.UnMakeMove(moveMade);
            }

            return numberOfPosiitions;
        }
    }
}
