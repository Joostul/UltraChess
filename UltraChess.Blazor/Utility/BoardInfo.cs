namespace UltraChess.Blazor.Utility
{
    public class BoardInfo
    {
        public BoardInfo()
        {

        }

        public BoardInfo(BoardInfo boardInfo)
        {
            Board = boardInfo.Board;
            IsWhiteTurn = boardInfo.IsWhiteTurn;
            WhiteCanCastleKingSide = boardInfo.WhiteCanCastleKingSide;
            WhiteCanCastleQueenSide = boardInfo.WhiteCanCastleQueenSide;
            BlackCanCastleKingSide = boardInfo.BlackCanCastleKingSide;
            BlackCanCastleQueenSide = boardInfo.BlackCanCastleQueenSide;
            EnPassantSquareId = boardInfo.EnPassantSquareId;
            HalfClockMove = boardInfo.HalfClockMove;
            FullMoveNumber = boardInfo.FullMoveNumber;
        }

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
