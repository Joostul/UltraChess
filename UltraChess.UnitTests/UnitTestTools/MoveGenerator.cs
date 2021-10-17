using System;
using System.Collections.Generic;
using UltraChess.Blazor.Models;

namespace UltraChess.UnitTests.UnitTestTools
{
    public class MoveGenerator
    {
        private static readonly string[] _squares = new[]
        {
            "a8", "b8","c8","d8","e8","f8","g8","h8",
            "a7", "b7","c7","d7","e7","f7","g7","h7",
            "a6", "b6","c6","d6","e6","f6","g6","h6",
            "a5", "b5","c5","d5","e5","f5","g5","h5",
            "a4", "b4","c4","d4","e4","f4","g4","h4",
            "a3", "b3","c3","d3","e3","f3","g3","h3",
            "a2", "b2","c2","d2","e2","f2","g2","h2",
            "a1", "b1","c1","d1","e1","f1","g1","h1"
        };
        private readonly ChessBoard ChessBoard;

        public MoveGenerator(string FENString)
        {
            ChessBoard = new ChessBoard(FENString);
        }

        public int GenerateMoves(int depth, bool log = false)
        {
            if(depth == 0)
            {
                return 1;
            }

            List<Move> moves = ChessBoard.GenerateLegalMoves(ChessBoard.CurrentBoardInfo.IsWhiteTurn);
            int numberOfPositions = 0;

            foreach (var move in moves)
            {
                ChessBoard.MakeMove(move);
                var positionsToAdd = GenerateMoves(depth - 1);
                numberOfPositions += positionsToAdd;
                if(log)
                {
                    Console.WriteLine($"{_squares[move.FromSquareId]}{_squares[move.ToSquareId]}: {positionsToAdd}");
                }
                ChessBoard.UnMakeMove(move);
            }

            return numberOfPositions;
        }
    }
}
