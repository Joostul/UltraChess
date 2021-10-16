namespace UltraChess.Blazor.Utility
{
    public class BoardInfo
    {
        public char[] Board { get; set; }
        public bool IsWhiteTurn { get; set; }
        public bool WhiteCanCastleKingSide { get; set; }
        public bool WhiteCanCastleQueenSide { get; set; }
        public bool BlackCanCastleKingSide { get; set; }
        public bool BlackCanCastleQueenSide { get; set; }
        public int EnPassantSquareId { get; set; }
        public int HalfClockMove { get; set; }
        public int FullMoveNumber { get; set; }
    }
}
