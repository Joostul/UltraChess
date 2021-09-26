using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UltraChess.Blazor.Models
{
    public class Rook : Piece
    {
        public override List<int> GetSquaresToCapture(int fromSquare)
        {
            throw new NotImplementedException();
        }

        public override List<int> GetSquaresToMoveTo(int fromSquare)
        {
            throw new NotImplementedException();
        }
    }
}
