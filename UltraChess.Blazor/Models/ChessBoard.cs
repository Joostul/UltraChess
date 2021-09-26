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
        public Piece[] pieces = new Piece[16];
        public bool IsWhiteTurn;

        public ChessBoard(string FEN)
        {
            string[] FENpieces = FEN.Split(' ');
            IsWhiteTurn = FENpieces[1][0] == 'w';
            string[] FENranks = FENpieces[0].Split('/');
            int skip = 0;
            for (int rank = 7; rank >= 0; rank--)
            {
                for (int file = 0; file < 8; file++)
                {
                    Piece piece = null;
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
                            var lowerChar = char.ToLower(fenCharacter);
                            if (lowerChar == 'p')
                            {
                                piece = new Pawn();
                            } else if (lowerChar == 'n')
                            {
                                piece = new Knight();
                            } else if (lowerChar == 'b')
                            {
                                piece = new Bishop();
                            }
                            else if (lowerChar == 'r')
                            {
                                piece = new Rook();
                            }
                            else if (lowerChar == 'k')
                            {
                                piece = new King();
                            }
                            else if (lowerChar == 'q')
                            {
                                piece = new Queen();
                            }
                            else
                            {
                                throw new System.Exception();
                            }
                            piece.Fen = fenCharacter;
                            piece.Image = images[fenCharacter];
                            piece.IsWhite = char.IsUpper(fenCharacter);
                        }
                    }
                    else
                    {
                        skip--;
                    }

                    square.Piece = piece;

                    squares[square.Id] = square;
                }
            }
        }

        public bool Move(int fromSquareId, int toSquareId)
        {
            // TODO: Add pawn promotions
            var legalSquaresToMoveTo = squares[fromSquareId].Piece.GetSquaresToMoveTo(fromSquareId);
            var legalSquaresToCapture = squares[fromSquareId].Piece.GetSquaresToCapture(fromSquareId);
            if(legalSquaresToCapture.Count < 1 && legalSquaresToMoveTo.Count < 1)
            {
                return false;
            }

            foreach (var square in legalSquaresToMoveTo)
            {
                squares[square].IsHighlighted = false;
            }
            if (fromSquareId == toSquareId)
            {
                return false;
            }
            if (legalSquaresToMoveTo.Contains(toSquareId))
            {
                if (squares[toSquareId].Piece == null)
                {
                    squares[toSquareId].Piece = squares[fromSquareId].Piece;
                    squares[fromSquareId].Piece = null;
                    return true;
                }
            }
            if (legalSquaresToCapture.Contains(toSquareId))
            {
                if (squares[toSquareId].Piece != null && squares[toSquareId].Piece.IsWhite != squares[fromSquareId].Piece.IsWhite)
                {
                    squares[toSquareId].Piece = squares[fromSquareId].Piece;
                    squares[fromSquareId].Piece = null;
                    return true;
                }
            }
            return false;
        }

        public void HighlightLegalMoves(int fromSquareId)
        {
            if (squares[fromSquareId].Piece is Pawn pawn)
            {
                var legalSquaresToMoveTo = pawn.GetSquaresToMoveTo(fromSquareId);
                foreach (var legalSquareToMoveTo in legalSquaresToMoveTo)
                {
                    squares[legalSquareToMoveTo].IsHighlighted = true;
                }
            }
        }
    }
}
