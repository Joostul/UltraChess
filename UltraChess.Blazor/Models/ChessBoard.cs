using System.Collections.Generic;
using System.Linq;

namespace UltraChess.Blazor.Models
{
    public class ChessBoard
    {
        readonly char[] characters = "abcdefgh".ToCharArray();
        readonly char[] numbers = "87654321".ToCharArray();
        public Square[] squares = new Square[64];
        public Piece[] pieces = new Piece[13]
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
            new Rook { IsWhite = false, Image = "img/R_B.png" },
            new Bishop { IsWhite = false, Image = "img/B_B.png" },
            new Queen { IsWhite = false, Image = "img/Q_B.png" },
            new King { IsWhite = false, Image = "img/K_B.png" },
        };
        public bool IsWhiteTurn;

        public ChessBoard(string FEN)
        {
            string[] FENpieces = FEN.Split(' ');
            IsWhiteTurn = FENpieces[1][0] == 'w';
            string[] FENranks = FENpieces[0].Split('/');
            int skip = 0;
            for (int file = 0; file < 8; file++)
            {
                for (int rank = 0; rank < 8; rank++)
                {
                    var square = new Square
                    {
                        Id = file * 8 + rank,
                        File = numbers[file],
                        Rank = characters[rank],
                        IsLight = (file + rank) % 2 == 0
                    };
                    if (skip == 0)
                    {
                        var fenCharacter = FENranks[file][rank];
                        if (char.IsDigit(fenCharacter))
                        {
                            skip = int.Parse(fenCharacter.ToString()) - 1;
                        }
                        else
                        {
                            if (fenCharacter == 'P')
                            {
                                square.PieceId = 1;
                            }
                            else if (fenCharacter == 'N')
                            {
                                square.PieceId = 2;
                            }
                            else if (fenCharacter == 'B')
                            {
                                square.PieceId = 3;
                            }
                            else if (fenCharacter == 'R')
                            {
                                square.PieceId = 4;
                            }
                            else if (fenCharacter == 'Q')
                            {
                                square.PieceId = 5;
                            }
                            else if (fenCharacter == 'K')
                            {
                                square.PieceId = 6;
                            }
                            if (fenCharacter == 'p')
                            {
                                square.PieceId = 7;
                            }
                            else if (fenCharacter == 'n')
                            {
                                square.PieceId = 8;
                            }
                            else if (fenCharacter == 'r')
                            {
                                square.PieceId = 9;
                            }
                            else if (fenCharacter == 'b')
                            {
                                square.PieceId = 10;
                            }
                            else if (fenCharacter == 'q')
                            {
                                square.PieceId = 11;
                            }
                            else if (fenCharacter == 'k')
                            {
                                square.PieceId = 12;
                            }
                        }
                    }
                    else
                    {
                        skip--;
                    }

                    squares[square.Id] = square;
                }
            }
        }

        public bool Move(int fromSquareId, int toSquareId)
        {
            // TODO: Add pawn promotions
            var fromPiece = pieces[squares[fromSquareId].PieceId];
            var toPiece = pieces[squares[toSquareId].PieceId];
            var legalSquaresToMoveTo = GetValidSquares(fromPiece.GetSquaresToMoveTo(fromSquareId));
            var legalSquaresToCapture = GetValidSquares(fromPiece.GetSquaresToCapture(fromSquareId));
            if (legalSquaresToCapture.Count < 1 && legalSquaresToMoveTo.Count < 1)
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
                if (squares[toSquareId].PieceId == 0)
                {
                    squares[toSquareId].PieceId = squares[fromSquareId].PieceId;
                    squares[fromSquareId].PieceId = 0;
                    return true;
                }
            }
            if (legalSquaresToCapture.Contains(toSquareId))
            {
                if (toPiece != null && toPiece.IsWhite != fromPiece.IsWhite)
                {
                    squares[toSquareId].PieceId = squares[fromSquareId].PieceId;
                    squares[fromSquareId].PieceId = 0;
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<int> GetMovementSquares(int fromSquare, Piece piece)
        {
            var moves = new List<int>();

            if(piece is Pawn)
            {
                if (piece.IsWhite)
                {
                    moves.Add(fromSquare - 8);
                    if (fromSquare > 47 && fromSquare < 56)
                    {
                        moves.Add(fromSquare - 16);
                    }
                }
                else
                {
                    moves.Add(fromSquare + 8);
                    if (fromSquare > 7 && fromSquare < 16)
                    {
                        moves.Add(fromSquare + 16);
                    }
                }
            }

            return GetValidSquares(moves);
        }

        public void HighlightLegalMoves(int fromSquareId)
        {
            var piece = pieces[squares[fromSquareId].PieceId];
            var legalSquaresToMoveTo = piece.GetSquaresToMoveTo(fromSquareId);
            var legalSquaresToCapture = piece.GetSquaresToCapture(fromSquareId);
            foreach (var legalSquareToCapture in legalSquaresToCapture)
            {
                // Only able to capture a square if there is a piece there that is not our own color
                if (squares[legalSquareToCapture].PieceId != 0 && pieces[squares[legalSquareToCapture].PieceId].IsWhite != piece.IsWhite)
                {
                    legalSquaresToMoveTo.Add(legalSquareToCapture);
                }
            }

            foreach (var legalSquareToMoveTo in legalSquaresToMoveTo)
            {
                squares[legalSquareToMoveTo].IsHighlighted = true;
            }
        }

        private List<int> GetValidSquares(List<int> squares)
        {
            return squares.Where(s => s < 64 && s > 0).ToList();
        }
    }
}
