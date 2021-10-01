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
        public readonly int[] DirectionOffsets = { -8, 8, 1, -1, -9, -7, 9, 7 };
        // Get number of squares to edge based on square index and direction: 0: North, 1: South, 2: East,  3: West, 4: NorthWest 5: NorthEast, 6: SouthEast, 7: SouthWest
        public readonly int[][] NumberOfSquaresToEdge = new int[64][];
        public readonly int[] AllKnightJumps = { 15, 17, -17, -15, 10, -6, 6, -10 };
        public readonly int[][] KnightMoves = new int[64][];
        public int[] PromotionPiece = new int[2] { 11, 5 };
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
            new Bishop { IsWhite = false, Image = "img/B_B.png" },
            new Rook { IsWhite = false, Image = "img/R_B.png" },
            new Queen { IsWhite = false, Image = "img/Q_B.png" },
            new King { IsWhite = false, Image = "img/K_B.png" },
        };
        public bool IsWhiteTurn;
        public int EnPassantSquare = 64;
        public bool PromotionModalIsOpen;
        public bool AutoPromoteQueen = true;

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
                        numberSouth,
                        numberEast,
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
                        Rank = numbers[file],
                        File = characters[rank],
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
                    else if (fenCharacter == 'b')
                    {
                        square.PieceId = 9;
                    }
                    else if (fenCharacter == 'r')
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
            var oldEnPassantSquare = EnPassantSquare;
            var legalSquaresToMoveTo = GetMovementSquares(fromSquareId, true);
            var pieceToMove = GetPiece(fromSquareId);
            if (legalSquaresToMoveTo.Count < 1)
            {
                return;
            }

            foreach (var square in legalSquaresToMoveTo)
            {
                Squares[square].IsHighlighted = false;
            }

            // If we are moving to the same square
            if (fromSquareId == toSquareId)
            {
                return;
            }

            // Check for en passant
            if (EnPassantSquare == toSquareId)
            {
                if (pieceToMove is Pawn)
                {
                    // If it's not a normal pawn step
                    if (toSquareId != fromSquareId + DirectionOffsets[0] || toSquareId != fromSquareId + DirectionOffsets[1])
                    {
                        // Move the pawn to the en passant square
                        Squares[toSquareId].PieceId = Squares[fromSquareId].PieceId;
                        Squares[fromSquareId].PieceId = 0;
                        // Remove the piece that was en passanted
                        Squares[toSquareId + DirectionOffsets[Convert.ToInt32(IsWhiteTurn)]].PieceId = 0;
                        IsWhiteTurn = !IsWhiteTurn;
                        return;
                    }
                }
            }

            if (legalSquaresToMoveTo.Contains(toSquareId))
            {
                var piece = GetPiece(fromSquareId);
                var promotionRank = piece.IsWhite ? '8' : '1';
                // Check for pawn promotion
                if (piece is Pawn && Squares[toSquareId].Rank == promotionRank)
                {
                    if (!AutoPromoteQueen)
                    {
                        // Open modal
                        PromotionModalIsOpen = true;
                    }
                    else
                    {
                        MovePiece(fromSquareId, toSquareId, PromotionPiece[Convert.ToInt32(piece.IsWhite)]);
                    }
                }
                else
                {
                    MovePiece(fromSquareId, toSquareId);
                }
            }
            if (EnPassantSquare == oldEnPassantSquare)
            {
                EnPassantSquare = 64;
            }
        }

        public void MovePiece(int fromSquareId, int toSquareId, int promotionPiece = 0)
        {
            if (Squares[toSquareId].PieceId == 0 || SquareContainsEnemyPiece(GetPiece(fromSquareId).IsWhite, toSquareId))
            {
                Squares[toSquareId].PieceId = promotionPiece == 0 ? Squares[fromSquareId].PieceId : promotionPiece;
                Squares[fromSquareId].PieceId = 0;
                IsWhiteTurn = !IsWhiteTurn;
            }
        }

        public List<int> GetMovementSquares(int fromSquareId, bool setEnPassantSquare = false)
        {
            var moves = new List<int>();
            var piece = GetPiece(fromSquareId);
            if (piece.IsWhite != IsWhiteTurn)
            {
                return moves;
            }

            if (piece is Pawn)
            {
                if (piece.IsWhite)
                {
                    moves.AddRange(GeneratePawnMoves(fromSquareId, 0, 4, 6, true, setEnPassantSquare));
                }
                else
                {
                    moves.AddRange(GeneratePawnMoves(fromSquareId, 1, 6, 8, false, setEnPassantSquare));
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

        List<int> GeneratePawnMoves(int fromSquareId, int direction, int startDirectionIndex, int endDirectionIndex, bool isWhite, bool setEnPassantSquare)
        {
            var piece = GetPiece(fromSquareId);
            var moves = new List<int>();
            int rankStartSquareIndex;
            int rankEndSquareIndex;

            if (isWhite)
            {
                rankStartSquareIndex = 47;
                rankEndSquareIndex = 56;
            }
            else
            {
                rankStartSquareIndex = 7;
                rankEndSquareIndex = 16;
            }

            // Pawns can move up one
            var toSquareId = fromSquareId + DirectionOffsets[direction];
            if (!SquareContainsPiece(toSquareId))
            {
                moves.Add(toSquareId);
            }

            // Or two from the starting rank
            toSquareId = fromSquareId + (DirectionOffsets[direction] * 2);
            if (fromSquareId > rankStartSquareIndex && fromSquareId < rankEndSquareIndex)
            {
                if (!SquareContainsPiece(toSquareId))
                {
                    moves.Add(toSquareId);
                    if (setEnPassantSquare)
                    {
                        EnPassantSquare = fromSquareId + DirectionOffsets[direction];
                    }
                }
            }

            // And can capture diagonally
            var diagonalCaptures = GenerateSlidingMoves(fromSquareId, startDirectionIndex, endDirectionIndex, 1);
            foreach (var diagonalCapture in diagonalCaptures)
            {
                if (SquareContainsEnemyPiece(piece.IsWhite, diagonalCapture) || EnPassantSquare == diagonalCapture)
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
                if (SquareContainsPiece(squareId))
                {
                    // If it's not your own piece
                    if (GetPiece(squareId).IsWhite != GetPiece(fromSquareId).IsWhite)
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
                            if (GetPiece(toSquareId).IsWhite != GetPiece(fromSquareId).IsWhite)
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

        private static List<int> GetValidSquares(List<int> squares)
        {
            return squares.Where(s => s < 64 && s >= 0).Distinct().ToList();
        }

        private bool SquareContainsPiece(int toSquareId)
        {
            return Squares[toSquareId].PieceId != 0;
        }

        private bool SquareContainsEnemyPiece(bool pieceIsWhite, int toSquareId)
        {
            var toSquarePieceId = Squares[toSquareId].PieceId;
            return toSquarePieceId != 0 && GetPiece(toSquareId).IsWhite != pieceIsWhite;
        }

        private Piece GetPiece(int squareId)
        {
            return Pieces[Squares[squareId].PieceId];
        }
    }
}
