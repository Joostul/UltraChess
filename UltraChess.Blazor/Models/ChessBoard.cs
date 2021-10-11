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
        public int OldEnPassantSquare = 64;
        public bool PromotionModalIsOpen;
        public bool AutoPromoteQueen = true;
        public List<Move> LegalMoves;
        public Stack<Move> MovesMade = new();

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

            LegalMoves = GenerateLegalMoves(IsWhiteTurn);
        }

        public bool MakeMove(Move move)
        {
            OldEnPassantSquare = EnPassantSquare;
            Squares = Squares.Select(s => { s.IsHighlighted = false; return s; }).ToArray();
            var pieceToMove = GetPiece(move.FromSquareId);
            int fromSquareId = move.FromSquareId;
            int toSquareId = move.ToSquareId;

            // If we are moving to the same square
            if (move.FromSquareId == move.ToSquareId)
            {
                return false;
            }
            else
            {
                switch (move.Flag)
                {
                    case MoveFlag.EnPassant:
                        var enemyPieceId = Squares[toSquareId + DirectionOffsets[Convert.ToInt32(IsWhiteTurn)]].PieceId;
                        // Move the pawn to the en passant square
                        Squares[toSquareId].PieceId = Squares[fromSquareId].PieceId;
                        Squares[fromSquareId].PieceId = 0;
                        // Remove the piece that was en passanted
                        Squares[toSquareId + DirectionOffsets[Convert.ToInt32(IsWhiteTurn)]].PieceId = 0;
                        break;
                    case MoveFlag.PawnPromotion:
                        Squares[fromSquareId].PieceId = 0;
                        Squares[toSquareId].PieceId = PromotionPiece[Convert.ToInt32(pieceToMove.IsWhite)];
                        break;
                    case MoveFlag.PawnTwoForward:
                        Squares[toSquareId].PieceId = Squares[fromSquareId].PieceId;
                        Squares[fromSquareId].PieceId = 0;
                        EnPassantSquare = toSquareId + DirectionOffsets[Convert.ToInt32(IsWhiteTurn)];
                        break;
                    case MoveFlag.None:
                    default:
                        Squares[toSquareId].PieceId = Squares[fromSquareId].PieceId;
                        Squares[fromSquareId].PieceId = 0;
                        break;
                }
                EnPassantSquare = OldEnPassantSquare == EnPassantSquare ? 64 : EnPassantSquare;

                MovesMade.Push(move);
                IsWhiteTurn = !IsWhiteTurn;
                return true;
            }
        }

        public void UnMakeMove(Move move)
        {
            // Move the piece back
            Squares[move.FromSquareId].PieceId = Squares[move.ToSquareId].PieceId;

            // If there was no capture, set the tosquare piece back to nothing
            if (move.CapturedPieceId == 0)
            {
                Squares[move.ToSquareId].PieceId = 0;
            }
            else
            {
                // If there was a capture and it was en passant, set the pawn back
                if (move.Flag == MoveFlag.EnPassant)
                {
                    // Move the pawn back to original square
                    Squares[move.ToSquareId - DirectionOffsets[Convert.ToInt32(IsWhiteTurn)]].PieceId = move.CapturedPieceId;
                    // Set the captured square back to 0 piece
                    Squares[move.ToSquareId].PieceId = 0;
                }
                else
                {
                    // If it was a normal capture, set the captured piece back
                    Squares[move.ToSquareId].PieceId = move.CapturedPieceId;
                }
            }

            // Unpromote if a promotion took place
            if (move.Flag == MoveFlag.PawnPromotion)
            {
                Squares[move.FromSquareId].PieceId = GetPiece(Squares[move.FromSquareId].Id).IsWhite ? 1 : 7;
            }

            // Set en passant square back
            EnPassantSquare = OldEnPassantSquare;

            // Set back to correct turn
            IsWhiteTurn = !IsWhiteTurn;

            MovesMade.Pop();
        }

        public List<Move> GenerateMoves(bool isWhite)
        {
            var moves = new List<Move>();

            foreach (var square in Squares)
            {
                if (SquareContainsPiece(square.Id))
                {
                    moves.AddRange(GetMovesFromSquare(square.Id, isWhite));
                }
            }

            return moves;
        }

        public List<Move> GenerateLegalMoves(bool isWhite)
        {
            var yourPieceKingId = IsWhiteTurn ? 6 : 12;
            var pseudoLegalMoves = GenerateMoves(isWhite);
            var legalMoves = new List<Move>();

            foreach (var move in pseudoLegalMoves)
            {
                MakeMove(move);
                
                var opponentResponses = GenerateMoves(!isWhite);
                if (!opponentResponses.Exists(r => r.CapturedPieceId == yourPieceKingId))
                {
                    legalMoves.Add(move);
                }
                UnMakeMove(move);
            }

            return legalMoves;
        }

        public List<Move> GetMovesFromSquare(int fromSquareId, bool isWhite)
        {
            var moves = new List<Move>();
            var piece = GetPiece(fromSquareId);
            if (piece.IsWhite != isWhite)
            {
                return moves;
            }

            if (piece is Pawn)
            {
                if (piece.IsWhite)
                {
                    moves.AddRange(GeneratePawnMoves(fromSquareId, 0, 4, 6));
                }
                else
                {
                    moves.AddRange(GeneratePawnMoves(fromSquareId, 1, 6, 8));
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

            return RemoveSquaresOutsideBoard(moves);
        }

        List<Move> GeneratePawnMoves(int fromSquareId, int direction, int startDirectionIndex, int endDirectionIndex)
        {
            var piece = GetPiece(fromSquareId);

            var moves = new List<Move>();
            var promotionRank = piece.IsWhite ? '8' : '1';
            var startRank = piece.IsWhite ? '2' : '7';

            // Pawns can move up one
            var toSquareId = fromSquareId + DirectionOffsets[direction];
            if (!SquareContainsPiece(toSquareId))
            {
                if (Squares[toSquareId].Rank != promotionRank)
                {
                    moves.Add(new Move(fromSquareId, toSquareId));
                }
                else
                {
                    if (!AutoPromoteQueen)
                    {
                        // Open modal
                        PromotionModalIsOpen = true;
                    }
                    else
                    {
                        moves.Add(new Move(fromSquareId, toSquareId) { Flag = MoveFlag.PawnPromotion });
                    }
                }
            }

            // Or two from the starting rank
            var toSquareIdTwoForward = fromSquareId + (DirectionOffsets[direction] * 2);
            if (Squares[fromSquareId].Rank == startRank)
            {
                if (!SquareContainsPiece(toSquareIdTwoForward) && !SquareContainsPiece(toSquareId))
                {
                    moves.Add(new Move(fromSquareId, toSquareIdTwoForward) { Flag = MoveFlag.PawnTwoForward });
                }
            }

            // And can capture diagonally
            var diagonalCaptures = GenerateSlidingMoves(fromSquareId, startDirectionIndex, endDirectionIndex, 1);
            foreach (var diagonalCapture in diagonalCaptures)
            {
                if (SquareContainsEnemyPiece(piece.IsWhite, diagonalCapture.ToSquareId))
                {
                    diagonalCapture.CapturedPieceId = Squares[diagonalCapture.ToSquareId].PieceId;
                    if (Squares[diagonalCapture.ToSquareId].Rank != promotionRank)
                    {
                        moves.Add(diagonalCapture);
                    }
                    else
                    {
                        if (!AutoPromoteQueen)
                        {
                            // Open modal
                            PromotionModalIsOpen = true;
                        }
                        else
                        {
                            diagonalCapture.Flag = MoveFlag.PawnPromotion;
                            moves.Add(diagonalCapture);
                        }
                    }
                }
                else if (EnPassantSquare == diagonalCapture.ToSquareId && SquareContainsEnemyPiece(piece.IsWhite, Squares[diagonalCapture.ToSquareId + DirectionOffsets[Convert.ToInt32(IsWhiteTurn)]].Id))
                {
                    var enemyPieceSquare = Squares[diagonalCapture.ToSquareId + DirectionOffsets[Convert.ToInt32(IsWhiteTurn)]].PieceId;
                    diagonalCapture.CapturedPieceId = enemyPieceSquare;
                    diagonalCapture.Flag = MoveFlag.EnPassant;
                    moves.Add(diagonalCapture);
                }
            }

            return moves;
        }

        List<Move> GenerateKnightMoves(int fromSquareId)
        {
            var moves = new List<Move>();

            foreach (var toSquareId in KnightMoves[fromSquareId])
            {
                // If a piece is already there
                if (SquareContainsPiece(toSquareId))
                {
                    // If it's not your own piece
                    if (GetPiece(toSquareId).IsWhite != GetPiece(fromSquareId).IsWhite)
                    {
                        moves.Add(new Move(fromSquareId, toSquareId));
                    }
                }
                else
                {
                    moves.Add(new Move(fromSquareId, toSquareId));
                }
            }

            return moves;
        }

        List<Move> GenerateSlidingMoves(int fromSquareId, int startDirectionIndex, int endDirectionIndex, int maxSquaresToMove = 7)
        {
            var moves = new List<Move>();

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
                            moves.Add(new Move(fromSquareId, toSquareId));
                        }
                        else
                        {
                            // If that piece is not your own color
                            if (GetPiece(toSquareId).IsWhite != GetPiece(fromSquareId).IsWhite)
                            {
                                // Able to capture
                                moves.Add(new Move(fromSquareId, toSquareId, Squares[toSquareId].PieceId));
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

        public void HighlightMoves(IEnumerable<Move> moves, bool highlight)
        {
            foreach (var move in moves)
            {
                Squares[move.ToSquareId].IsHighlighted = highlight;
            }
        }

        private static List<Move> RemoveSquaresOutsideBoard(List<Move> squares)
        {
            return squares.Where(s => s.ToSquareId < 64 && s.ToSquareId >= 0).Distinct().ToList();
        }

        private bool SquareContainsPiece(int squareId)
        {
            return Squares[squareId].PieceId != 0;
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
