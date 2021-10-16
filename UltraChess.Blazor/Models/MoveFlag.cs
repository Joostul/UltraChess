namespace UltraChess.Blazor.Models
{
    public enum MoveFlag
    {
        None = 0,
        PawnTwoForward = 1,
        EnPassant = 2,
        PawnPromotion = 3,
        Castling = 4,
        BreakCastlingRights = 5,
        BreakKingsideCastlingRights = 6,
        BreakQueensideCastingRights = 7
    }
}
