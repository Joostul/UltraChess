﻿using System;
using System.Collections.Generic;
using System.Linq;
using UltraChess.Blazor.Models.Enums;

namespace UltraChess.Blazor.Models
{
    public class ChessBoard
    {
        readonly char[] characters = "abcdefgh".ToCharArray();
        readonly char[] numbers = "87654321".ToCharArray();
        public readonly int[] DirectionOffsets = { -8, 1, 8, -1, -7, 9, 7, -9 };
        // Get number of squares to edge based on square index and direction: 0: North, 1: East, 2: South, 3: West, 4: NorthEast, 5: SouthEast, 6: SouthWest, 7: NorthWest
        public readonly int[][] NumberOfSquaresToEdge = new int[64][];
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
                int skipped = 0;
                for (int rank = 0; rank < 8; rank++)
                {
                    // Pre compute move data
                    int squareIndex = file * 8 + rank;

                    int numberNorth = file;
                    int numberSouth = 7 - file;
                    int numberWest = rank;
                    int numberEast = 7 - rank;

                    NumberOfSquaresToEdge[squareIndex] = new int[]
                    {
                        numberNorth,
                        numberEast,
                        numberSouth,
                        numberWest,
                        Math.Min(numberNorth, numberEast),
                        Math.Min(numberSouth, numberEast),
                        Math.Min(numberSouth, numberWest),
                        Math.Min(numberNorth, numberWest),
                    };

                    var square = new Square
                    {
                        Id = squareIndex,
                        File = numbers[file],
                        Rank = characters[rank],
                        IsLight = (file + rank) % 2 == 0
                    };
                    if (skip == 0)
                    {
                        var fenCharacter = FENranks[file][Math.Max(rank - skipped, 0)];
                        if (char.IsDigit(fenCharacter))
                        {
                            skipped = int.Parse(fenCharacter.ToString());
                            skip = skipped - 1;
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

        public void Move(int fromSquareId, int toSquareId)
        {
            // TODO: Add pawn promotions
            var fromPiece = pieces[squares[fromSquareId].PieceId];
            var toPiece = pieces[squares[toSquareId].PieceId];
            var legalSquaresToMoveTo = GetMovementSquares(fromSquareId);
            var legalSquaresToCapture = GetCaptureSquares(fromSquareId, fromPiece);
            if (legalSquaresToCapture.Count < 1 && legalSquaresToMoveTo.Count < 1)
            {
                return;
            }

            // Remove highlighting
            foreach (var square in legalSquaresToMoveTo)
            {
                squares[square].IsHighlighted = false;
            }
            foreach (var square in legalSquaresToCapture)
            {
                squares[square].IsHighlighted = false;
            }

            if (fromSquareId == toSquareId)
            {
                return;
            }

            // If a capture is taking place
            if (legalSquaresToCapture.Contains(toSquareId))
            {
                if (toPiece != null && toPiece.IsWhite != fromPiece.IsWhite)
                {
                    squares[toSquareId].PieceId = squares[fromSquareId].PieceId;
                    squares[fromSquareId].PieceId = 0;
                    IsWhiteTurn = !IsWhiteTurn;
                }
            }
            if (legalSquaresToMoveTo.Contains(toSquareId))
            {
                if (squares[toSquareId].PieceId == 0)
                {
                    squares[toSquareId].PieceId = squares[fromSquareId].PieceId;
                    squares[fromSquareId].PieceId = 0;
                    IsWhiteTurn = !IsWhiteTurn;
                }
            }
        }

        public List<int> GetMovementSquares(int fromSquareId)
        {
            var piece = pieces[squares[fromSquareId].PieceId];
            var moves = new List<int>();

            if (piece is Pawn)
            {
                if (piece.IsWhite)
                {
                    moves.Add(fromSquareId - 8);
                    if (fromSquareId > 47 && fromSquareId < 56)
                    {
                        moves.Add(fromSquareId - 16);
                    }
                }
                else
                {
                    moves.Add(fromSquareId + 8);
                    if (fromSquareId > 7 && fromSquareId < 16)
                    {
                        moves.Add(fromSquareId + 16);
                    }
                }
            }
            else if (piece is King)
            {
                moves.AddRange(GenerateSlidingMoves(fromSquareId, PieceType.King));
            } else if(piece is Rook)
            {
                moves.AddRange(GenerateSlidingMoves(fromSquareId, PieceType.Rook));
            } else if(piece is Bishop)
            {
                moves.AddRange(GenerateSlidingMoves(fromSquareId, PieceType.Bishop));
            } else if(piece is Queen)
            {
                moves.AddRange(GenerateSlidingMoves(fromSquareId, PieceType.Queen));
            }

            return GetValidSquares(moves);
        }

        List<int> GenerateSlidingMoves(int fromSquareId, PieceType pieceType)
        {
            var moves = new List<int>();

            var maxSquaresToMove = pieceType == PieceType.King ? 1 : 7;

            int startDirectionIndex = pieceType == PieceType.Bishop ? 4 : 0;
            int endDirectionIndex = pieceType == PieceType.Rook ? 4 : 8;

            // For each direction
            for(int directionIndex = startDirectionIndex; directionIndex < endDirectionIndex; directionIndex ++)
            {
                // For the amount of squares that we can move
                for(int i = 0; i < maxSquaresToMove; i++)
                {
                    // If we are not on the edge of the board
                    if(NumberOfSquaresToEdge[fromSquareId][directionIndex] > i)
                    {
                        var toSquareId = fromSquareId + DirectionOffsets[directionIndex] * (i + 1);

                        // If a piece is not blocking
                        if(squares[toSquareId].PieceId == 0)
                        {
                            moves.Add(toSquareId);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return moves;
        }

        public List<int> GetCaptureSquares(int fromSquareId, Piece piece)
        {
            var moves = new List<int>();

            if (piece is Pawn)
            {
                if (piece.IsWhite)
                {
                    moves.Add(fromSquareId - 7);
                    moves.Add(fromSquareId - 9);
                }
                else
                {
                    moves.Add(fromSquareId + 7);
                    moves.Add(fromSquareId + 9);
                }
            }

            return GetValidSquares(moves);
        }

        public void HighlightLegalMoves(int fromSquareId)
        {
            var piece = pieces[squares[fromSquareId].PieceId];
            var legalSquaresToMoveTo = GetMovementSquares(fromSquareId);
            var legalSquaresToCapture = GetCaptureSquares(fromSquareId, piece);
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
            return squares.Where(s => s < 64 && s > 0).Distinct().ToList();
        }
    }
}
