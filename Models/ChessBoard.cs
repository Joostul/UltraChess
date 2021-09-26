using System.Collections.Generic;

namespace UltraChess.Blazor.Models
{
    public class ChessBoard
    {
        readonly char[] characters = "abcdefgh".ToCharArray();
        readonly char[] numbers = "87654321".ToCharArray();
        readonly Dictionary<char, string> images = new()
        {
            { 'r', "img/R_B.png" },
            { 'n', "img/N_B.png" },
            { 'b', "img/B_B.png" },
            { 'q', "img/Q_B.png" },
            { 'k', "img/K_B.png" },
            { 'p', "img/P_B.png" },
            { 'R', "img/R_W.png" },
            { 'N', "img/N_W.png" },
            { 'B', "img/B_W.png" },
            { 'Q', "img/Q_W.png" },
            { 'K', "img/K_W.png" },
            { 'P', "img/P_W.png" }
        };
        public Square[] squares = new Square[64];

        public ChessBoard(string FEN)
        {
            string[] FENpieces = FEN.Split(' ');
            string[] FENranks = FENpieces[0].Split('/');
            int skip = 0;
            for (int rank = 7; rank >= 0; rank--)
            {
                for (int file = 0; file < 8; file++)
                {
                    var piece = new Piece();
                    var square = new Square
                    {
                        Id = rank * 8 + file,
                        File = characters[file],
                        Rank = numbers[rank],
                        IsLight = (file + rank) % 2 != 0
                    };
                    if (skip == 0)
                    {
                        var fenCharacter = FENranks[rank][file];
                        if (char.IsDigit(fenCharacter))
                        {
                            skip = int.Parse(fenCharacter.ToString()) - 1;
                        }
                        else
                        {
                            if(fenCharacter == 'p' || fenCharacter == 'P')
                            {
                                piece = new Pawn
                                {
                                    Fen = fenCharacter,
                                    Image = images[fenCharacter],
                                    IsWhite = char.IsUpper(fenCharacter)
                                };
                            }
                            else
                            {
                                piece.Fen = fenCharacter;
                                piece.Image = images[fenCharacter];
                                piece.IsWhite = char.IsUpper(fenCharacter);
                            }
                        }
                    }
                    else
                    {
                        skip--;
                    }

                    square.Piece = skip != 0 ? null : piece;

                    squares[square.Id] = square;
                }
            }
        }
    }
}
