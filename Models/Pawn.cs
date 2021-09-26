using System.Collections.Generic;

namespace UltraChess.Blazor.Models
{
    public class Pawn : Piece
    {
        public List<int> GetSquaresToMoveTo(int fromSquare)
        {
            List<int> squaresToMoveTo = new();
            if (IsWhite)
            {
                squaresToMoveTo.Add(fromSquare - 8);
                if(fromSquare > 47 && fromSquare < 56)
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
    }
}
