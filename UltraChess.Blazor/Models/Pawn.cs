using System.Collections.Generic;

namespace UltraChess.Blazor.Models
{
    public class Pawn : Piece
    {
        public override List<int> GetSquaresToMoveTo(int fromSquare)
        {
            // TODO: put this on board since we need to know if something is in the way
            List<int> squaresToMoveTo = new();
            if (IsWhite)
            {
                squaresToMoveTo.Add(fromSquare - 8);
                if (fromSquare > 47 && fromSquare < 56)
                {
                    squaresToMoveTo.Add(fromSquare - 16);
                }
            }
            else
            {
                squaresToMoveTo.Add(fromSquare + 8 );
                if (fromSquare > 7 && fromSquare < 16)
                {
                    squaresToMoveTo.Add(fromSquare + 16);
                }
            }
            return squaresToMoveTo;
        }

        public override List<int> GetSquaresToCapture(int fromSquare)
        {
            List<int> squaresToCapture = new();
            if (IsWhite)
            {
                squaresToCapture.Add(fromSquare - 7);
                squaresToCapture.Add(fromSquare - 9);
            }
            else
            {
                squaresToCapture.Add(fromSquare + 7);
                squaresToCapture.Add(fromSquare + 9);
            }
            return squaresToCapture;
        }
    }
}
