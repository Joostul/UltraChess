namespace UltraChess.Blazor.Models
{
    public class Move
    {
        public Move(int fromSquareId, int toSquareId)
        {
            FromSquareId = fromSquareId;
            ToSquareId = toSquareId;
        }

        public Move(int fromSquareId, int toSquareId, int capturedPiece)
        {
            FromSquareId = fromSquareId;
            ToSquareId = toSquareId;
            CapturedPiece = capturedPiece;
        }

        public int FromSquareId { get; set; }
        public int ToSquareId { get; set; }
        public int CapturedPiece { get; set; }
    }
}
