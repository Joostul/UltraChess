using System;
using System.Collections.Generic;
using System.Linq;

namespace UltraChess.Blazor.Models
{
    public class King : Piece
    {
        public override List<int> GetSquaresToCapture(int fromSquare)
        {
            return GetSquaresToMoveTo(fromSquare);
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

            var validSquaresToMoveTo = squaresToMoveTo.Where(s => s < 64 && s > 0).ToList();
            return validSquaresToMoveTo;
        }
    }
}
