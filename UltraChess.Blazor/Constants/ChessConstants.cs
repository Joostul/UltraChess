using UltraChess.Blazor.Models;

namespace UltraChess.Blazor.Constants
{
    public static class ChessConstants
    {
        public static char[] BoardCharacters = "abcdefgh".ToCharArray();
        public static char[] BoardNumbers = "87654321".ToCharArray();
        public static int[] DirectionOffsets = { -8, 8, 1, -1, -9, -7, 9, 7 };
        public static Piece[] Pieces = new Piece[13]
        {
            null,
            new Pawn { IsWhite = true, Image = "img/P_W.png" },
            new Knight { IsWhite = true, Image = "img/N_W.png" },
            new Bishop { IsWhite = true, Image = "img/B_W.png" },
            new Rook { IsWhite = true, Image = "img/R_W.png" },
            new Queen { IsWhite = true, Image = "img/Q_W.png" },
            new King { IsWhite = true, Image = "img/K_W.png" },
            new Pawn { IsWhite = false, Image = "img/P_B.png" },
            new Knight { IsWhite = false, Image = "img/N_B.png" },
            new Bishop { IsWhite = false, Image = "img/B_B.png" },
            new Rook { IsWhite = false, Image = "img/R_B.png" },
            new Queen { IsWhite = false, Image = "img/Q_B.png" },
            new King { IsWhite = false, Image = "img/K_B.png" },
        };

        public static int[] AllKnightJumps = { 15, 17, -17, -15, 10, -6, 6, -10 };
    }
}
