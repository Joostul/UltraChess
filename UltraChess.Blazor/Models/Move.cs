namespace UltraChess.Blazor.Models
{
    public class Move
    {
        public Move(int fromSquareId, int toSquareId)
        {
            FromSquareId = fromSquareId;
            ToSquareId = toSquareId;
        }

        public Move(int fromSquareId, int toSquareId, int capturedPieceId)
        {
            FromSquareId = fromSquareId;
            ToSquareId = toSquareId;
            CapturedPieceId = capturedPieceId;
        }

        public int FromSquareId { get; set; }
        public int ToSquareId { get; set; }
        public int CapturedPieceId { get; set; }
        public MoveFlag Flag { get; set; }
    }
}
