using System;
using System.Collections.Generic;

namespace UltraChess.Blazor.Models
{
    public class Knight : Piece
    {
        public override List<int> GetSquaresToCapture(int fromSquare)
        {
            throw new NotImplementedException();
        }

        public override List<int> GetSquaresToMoveTo(int fromSquare)
        {
            List<int> squaresToMoveTo = new()
            {
                // Forward
                fromSquare - 7,
                fromSquare - 8,
                fromSquare - 9,
                // Sideways
                fromSquare - 1,
                fromSquare + 1,
                // Backwards
                fromSquare + 7,
                fromSquare + 8,
                fromSquare + 9
            };

            return squaresToMoveTo;
        }
    }
}
