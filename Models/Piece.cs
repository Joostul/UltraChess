using System.Collections.Generic;

namespace UltraChess.Blazor.Models
{
    public class Piece
    {
        public char Fen { get; set; }
        public string Image { get; set; }
        public bool IsWhite { get; set; }
        public List<int> GetSquaresToMoveTo() 
        {
            return new List<int>();
        }
    }
}
