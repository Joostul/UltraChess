using System;
using System.Collections.Generic;
using System.Linq;
using UltraChess.Blazor.Utility;

namespace UltraChess.Blazor.Models
{
    public class ChessBoard
    {
        readonly char[] characters = "abcdefgh".ToCharArray();
        readonly char[] numbers = "87654321".ToCharArray();
        public readonly int[] DirectionOffsets = { -8, 1, 8, -1, -9, -7, 9, 7 };
        // Get number of squares to edge based on square index and direction: 0: North, 1: East, 2: South, 3: West, 4: NorthWest 5: NorthEast, 6: SouthEast, 7: SouthWest
        public readonly int[][] NumberOfSquaresToEdge = new int[64][];
        public readonly int[] AllKnightJumps = { 15, 17, -17, -15, 10, -6, 6, -10 };
        public readonly int[][] KnightMoves = new int[64][]; // TODO: Create knight moves in advance
        public Square[] Squares = new Square[64];
        public Piece[] Pieces = new Piece[13]
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
            IsWhiteTurn = FEN.Split(' ')[1][0] == 'w';
            var boardFENCharacters = FENUtility.ParseFENString(FEN);
            for (int file = 0; file < 8; file++)
            {
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
                        Math.Min(numberNorth, numberWest),
                        Math.Min(numberNorth, numberEast),
                        Math.Min(numberSouth, numberEast),
                        Math.Min(numberSouth, numberWest),
                    };

                    // Generate knight moves in advance
                    List<int> legalKnightJumps = new();
                    foreach (var knightJumpDelta in AllKnightJumps)
                    {
                        var squareToJumpTo = squareIndex + knightJumpDelta;

                        // Check if knight jump is inside board range
                        if (squareToJumpTo >= 0 && squareToJumpTo < 64)
                        {
                            int knightSquareY = squareToJumpTo / 8;
                            int knightSquareX = squareToJumpTo - knightSquareY * 8;

                            // Check if knight didn't move to other side of board
                            var maxMoveDistance = Math.Max(Math.Abs(rank - knightSquareX), Math.Abs(file - knightSquareY));

                            if (maxMoveDistance == 2)
                            {
                                legalKnightJumps.Add(squareToJumpTo);
                            }
                        }
                    }
                    KnightMoves[squareIndex] = legalKnightJumps.ToArray();

                    var square = new Square
                    {
                        Id = squareIndex,
                        File = numbers[file],
                        Rank = characters[rank],
                        IsLight = (file + rank) % 2 == 0
                    };
                    var fenCharacter = boardFENCharacters[squareIndex];
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

                    Squares[square.Id] = square;
                }
            }
        }

        public void Move(int fromSquareId, int toSquareId)
        {
            // TODO: Add pawn promotions
            var legalSquaresToMoveTo = GetMovementSquares(fromSquareId);
            if (legalSquaresToMoveTo.Count < 1)
            {
                return;
            }
            // Remove highlighting
            foreach (var square in legalSquaresToMoveTo)
            {
                Squares[square].IsHighlighted = false;
            }

            if (fromSquareId == toSquareId)
            {
                return;
            }

            if (legalSquaresToMoveTo.Contains(toSquareId))
            {
                if (Squares[toSquareId].PieceId == 0)
                {
                    Squares[toSquareId].PieceId = Squares[fromSquareId].PieceId;
                    Squares[fromSquareId].PieceId = 0;
                    IsWhiteTurn = !IsWhiteTurn;
                }
                else
                {
                    var fromPiece = Pieces[Squares[fromSquareId].PieceId];
                    var toPiece = Pieces[Squares[toSquareId].PieceId];

                    if (toPiece.IsWhite != fromPiece.IsWhite)
                    {
                        // Capture the piece
                        Squares[toSquareId].PieceId = Squares[fromSquareId].PieceId;
                        Squares[fromSquareId].PieceId = 0;
                        IsWhiteTurn = !IsWhiteTurn;
                    }
                }
            }
        }

        public List<int> GetMovementSquares(int fromSquareId)
        {
            var piece = Pieces[Squares[fromSquareId].PieceId];
            var moves = new List<int>();

            if (piece is Pawn)
            {
                if (piece.IsWhite)
                {
                    moves.AddRange(GeneratePawnMoves(fromSquareId, 0, 4, 6, 2));
                }
                else
                {
                    moves.AddRange(GeneratePawnMoves(fromSquareId, 2, 6, 8, 7));
                }
            }
            else if (piece is King)
            {
                moves.AddRange(GenerateSlidingMoves(fromSquareId, 0, 8, 1));
            }
            else if (piece is Rook)
            {
                moves.AddRange(GenerateSlidingMoves(fromSquareId, 0, 4));
            }
            else if (piece is Bishop)
            {
                moves.AddRange(GenerateSlidingMoves(fromSquareId, 4, 8));
            }
            else if (piece is Queen)
            {
                moves.AddRange(GenerateSlidingMoves(fromSquareId, 0, 8));
            }
            else if (piece is Knight)
            {
                moves.AddRange(GenerateKnightMoves(fromSquareId));
            }

            return GetValidSquares(moves);
        }

        List<int> GeneratePawnMoves(int fromSquareId, int direction, int startDirectionIndex, int endDirectionIndex, int startRank)
        {
            var piece = Pieces[Squares[fromSquareId].PieceId];
            var moves = new List<int>();
            int rankStartIndex;
            int rankEndIndex;

            if (startRank == 2)
            {
                rankStartIndex = 47;
                rankEndIndex = 56;
            }
            else if (startRank == 7)
            {
                rankStartIndex = 7;
                rankEndIndex = 16;
            }
            else
            {
                throw new Exception();
            }
            // Pawns can move up one
            var toSquareId = fromSquareId + DirectionOffsets[direction];
            if (!SquareContainsPiece(toSquareId))
            {
                moves.Add(toSquareId);
            }
            // Or two from the starting rank
            toSquareId = fromSquareId + (DirectionOffsets[direction] * 2);
            if (fromSquareId > rankStartIndex && fromSquareId < rankEndIndex)
            {
                if (!SquareContainsPiece(toSquareId))
                {
                    moves.Add(toSquareId);
                }
            }
            // And can capture diagonally
            var diagonalCaptures = GenerateSlidingMoves(fromSquareId, startDirectionIndex, endDirectionIndex, 1);
            foreach (var diagonalCapture in diagonalCaptures)
            {
                if (SquareContainsEnemyPiece(piece.IsWhite, diagonalCapture))
                {
                    moves.Add(diagonalCapture);
                }
            }

            return moves;
        }

        List<int> GenerateKnightMoves(int fromSquareId)
        {
            var moves = new List<int>();

            foreach (var squareId in KnightMoves[fromSquareId])
            {
                // If a piece is already there
                if (Squares[squareId].PieceId != 0)
                {
                    // If it's not your own piece
                    if (Pieces[Squares[squareId].PieceId].IsWhite != Pieces[Squares[fromSquareId].PieceId].IsWhite)
                    {
                        moves.Add(squareId);
                    }
                }
                else
                {
                    moves.Add(squareId);
                }
            }

            return moves;
        }

        List<int> GenerateSlidingMoves(int fromSquareId, int startDirectionIndex, int endDirectionIndex, int maxSquaresToMove = 7)
        {
            var moves = new List<int>();

            // For each direction
            for (int directionIndex = startDirectionIndex; directionIndex < endDirectionIndex; directionIndex++)
            {
                // For the amount of squares that we can move
                for (int i = 0; i < maxSquaresToMove; i++)
                {
                    // If we are not on the edge of the board
                    if (NumberOfSquaresToEdge[fromSquareId][directionIndex] > i)
                    {
                        var toSquareId = fromSquareId + DirectionOffsets[directionIndex] * (i + 1);

                        // If a piece is not blocking
                        if (Squares[toSquareId].PieceId == 0)
                        {
                            moves.Add(toSquareId);
                        }
                        else
                        {
                            // If that piece is not your own color
                            if (Pieces[Squares[toSquareId].PieceId].IsWhite != Pieces[Squares[fromSquareId].PieceId].IsWhite)
                            {
                                // Able to capture
                                moves.Add(toSquareId);
                            }
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

        public void HighlightLegalMoves(int fromSquareId)
        {
            var legalSquaresToMoveTo = GetMovementSquares(fromSquareId);

            foreach (var legalSquareToMoveTo in legalSquaresToMoveTo)
            {
                Squares[legalSquareToMoveTo].IsHighlighted = true;
            }
        }

        private List<int> GetValidSquares(List<int> squares)
        {
            return squares.Where(s => s < 64 && s > 0).Distinct().ToList();
        }

        private bool SquareContainsPiece(int toSquareId)
        {
            return Squares[toSquareId].PieceId != 0;
        }

        private bool SquareContainsEnemyPiece(bool pieceIsWhite, int toSquareId)
        {
            var toSquarePieceId = Squares[toSquareId].PieceId;
            return toSquarePieceId != 0 && Pieces[Squares[toSquareId].PieceId].IsWhite != pieceIsWhite;
        }
    }
}
