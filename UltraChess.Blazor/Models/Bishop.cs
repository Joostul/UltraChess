using System;
using System.Collections.Generic;

namespace UltraChess.Blazor.Models
{
    public class Bishop : Piece
    {
        public override List<int> GetSquaresToCapture(int fromSquare)
        {
            throw new NotImplementedException();
        }

        public override List<int> GetSquaresToMoveTo(int fromSquare)
        {
            List<int> squaresToMoveTo = new();

            squaresToMoveTo.AddRange(GetSlidingMoves(fromSquare, -7));
            squaresToMoveTo.AddRange(GetSlidingMoves(fromSquare, -9));
            squaresToMoveTo.AddRange(GetSlidingMoves(fromSquare, 7));
            squaresToMoveTo.AddRange(GetSlidingMoves(fromSquare, 9));

            return squaresToMoveTo;
        }

        private List<int> GetSlidingMoves(int startSquare, int direction)
        {
            // TODO: Remove diagonal moves that are wrong color
            var moves = new List<int>();
            int endSquare = startSquare;
            while (endSquare >= 0 && endSquare < 64)
            {
                endSquare += direction;
                if (endSquare >= 0 && endSquare < 64)
                {
                    moves.Add(endSquare);
                }
            };
            moves.Remove(startSquare);
            return moves;
        }
    }
}
