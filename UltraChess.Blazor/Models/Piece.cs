namespace UltraChess.Blazor.Models
{
    public abstract class Piece
    {
        public char Fen { get; set; }
        public string Image { get; set; }
        public bool IsWhite { get; set; }
    }
}
