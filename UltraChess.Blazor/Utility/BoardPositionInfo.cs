namespace UltraChess.Blazor.Utility
{
    public class BoardPositionInfo
    {
        public char[] Board { get; set; }
        public bool IsWhiteTurn { get; set; }
        public bool WhiteCanCastleKingSide { get; set; }
        public bool WhiteCanCastleQueenSide { get; set; }
        public bool BlackCanCastleKingSide { get; set; }
        public bool BlackCanCastleQueenSide { get; set; }
        public string EnPassantSquare { get; set; }
    }
}
