namespace UltraChess.Blazor.Models
{
    public class Square
    {
        public int Id { get; set; }
        public char File { get; set; }
        public char Rank { get; set; }
        public bool IsLight { get; set; }
        public bool IsHighlighted { get; set; }
        public Piece Piece { get; set; }
    }
}
