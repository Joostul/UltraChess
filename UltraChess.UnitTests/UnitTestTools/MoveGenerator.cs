using System.Collections.Generic;
using UltraChess.Blazor.Models;

namespace UltraChess.UnitTests.UnitTestTools
{
    public class MoveGenerator
    {
        private readonly ChessBoard ChessBoard;

        public MoveGenerator(string FENString)
        {
            ChessBoard = new ChessBoard(FENString);
        }

        public int GenerateMoves(int depth)
        {
            if(depth == 0)
            {
                return 1;
            }

            List<Move> moves = ChessBoard.GenerateLegalMoves(ChessBoard.CurrentBoardInfo.IsWhiteTurn);
            int numberOfPosiitions = 0;

            foreach (var move in moves)
            {
                ChessBoard.MakeMove(move);
                numberOfPosiitions += GenerateMoves(depth - 1);
                ChessBoard.UnMakeMove(move);
            }

            return numberOfPosiitions;
        }
    }
}
