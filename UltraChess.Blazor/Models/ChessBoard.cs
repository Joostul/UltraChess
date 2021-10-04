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

        public Move MakeMove(int fromSquareId, int toSquareId)
        {
            Move moveMade = null;
            Squares = Squares.Select(s => { s.IsHighlighted = false; return s; }).ToArray();
            var pieceToMove = GetPiece(fromSquareId);

            // If we are moving to the same square
            if (fromSquareId == toSquareId)
            {
                return moveMade;
            }

            // Pawns are special
            if (pieceToMove is Pawn)
            {
                var promotionRank = pieceToMove.IsWhite ? '8' : '1';
                // Check for en passant
                if (EnPassantSquare == toSquareId)
                {
                    // If it's not a normal pawn step
                    if (toSquareId != fromSquareId + DirectionOffsets[0] || toSquareId != fromSquareId + DirectionOffsets[1])
                    {
                        var enemyPieceId = Squares[toSquareId + DirectionOffsets[Convert.ToInt32(IsWhiteTurn)]].PieceId;
                        // Move the pawn to the en passant square
                        Squares[toSquareId].PieceId = Squares[fromSquareId].PieceId;
                        Squares[fromSquareId].PieceId = 0;
                        // Remove the piece that was en passanted
                        Squares[toSquareId + DirectionOffsets[Convert.ToInt32(IsWhiteTurn)]].PieceId = 0;
                        IsWhiteTurn = !IsWhiteTurn;
                        moveMade = new Move(fromSquareId, toSquareId, enemyPieceId);
                    }
                }

                // Check for pawn promotion
                else if (Squares[toSquareId].Rank == promotionRank)
                {
                    if (!AutoPromoteQueen)
                    {
                        // Open modal
                        PromotionModalIsOpen = true;
                    }
                    else
                    {
                        moveMade = MovePiece(new Move(fromSquareId, toSquareId), PromotionPiece[Convert.ToInt32(pieceToMove.IsWhite)]);
                    }
                }
                else
                {
                    moveMade = MovePiece(new Move(fromSquareId, toSquareId));
                }

                // If the pawn moved two spaces
                var direction = Convert.ToInt32(pieceToMove.IsWhite);
                if (fromSquareId - (DirectionOffsets[direction] * 2) == toSquareId)
                {
                    // Set the new en passant square
                    OldEnPassantSquare = EnPassantSquare;
                    moveMade.Flag = MoveFlag.PawnTwoForward;
                    if (pieceToMove.IsWhite)
                    {
                        EnPassantSquare = moveMade.ToSquareId + 8;
                    }
                    else
                    {
                        EnPassantSquare = moveMade.ToSquareId - 8;
                    }
                }
            }
            else
            {
                moveMade = MovePiece(new Move(fromSquareId, toSquareId));
            }

            return moveMade;
        }

        public void UnMakeMove(Move move)
        {
            // Move the piece back
            MovePiece(new Move(move.ToSquareId, move.FromSquareId));

            // Uncapture the piece
            if (move.CapturedPieceId != 0)
            {
                Squares[move.ToSquareId].PieceId = move.CapturedPieceId;
            }

            // Set en passant back if moved forward two squares
            if (move.Flag == MoveFlag.PawnTwoForward)
            {
                EnPassantSquare = OldEnPassantSquare;
                OldEnPassantSquare = 64;
            }

            // Set pawns back correctly if en passant was done

            // TODO: BUG pawns that can be captured by en passant move only one square instead of two
        }

        public Move MovePiece(Move move, int promotionPiece = 0)
        {
            var myPiece = GetPiece(move.FromSquareId);

            var squareContainsEnemyPiece = SquareContainsEnemyPiece(myPiece.IsWhite, move.ToSquareId);
            if (Squares[move.ToSquareId].PieceId == 0 || squareContainsEnemyPiece)
            {
                var enemyPieceId = squareContainsEnemyPiece ? Squares[move.ToSquareId].PieceId : 0;
                Squares[move.ToSquareId].PieceId = promotionPiece == 0 ? Squares[move.FromSquareId].PieceId : promotionPiece;
                Squares[move.FromSquareId].PieceId = 0;
                IsWhiteTurn = !IsWhiteTurn;
                return new Move(move.FromSquareId, move.ToSquareId, enemyPieceId);
            }
            return null;
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
            var yourPieceKingId = IsWhiteTurn ? 6 : 11;
            var yourKingSquare = Squares.Single(s => s.PieceId == yourPieceKingId);
            var pseudoLegalMoves = GenerateMoves(isWhite);
            var legalMoves = new List<Move>();

            foreach (var move in pseudoLegalMoves)
            {
                var moveMade = MakeMove(move.FromSquareId, move.ToSquareId);
                if (moveMade != null)
                {
                    var opponentResponses = GenerateMoves(!isWhite);
                    if (!opponentResponses.Any(r => r.ToSquareId == yourKingSquare.Id))
                    {
                        legalMoves.Add(move);
                    }
                    UnMakeMove(moveMade);
                }
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
                    moves.AddRange(GeneratePawnMoves(fromSquareId, 0, 4, 6, true));
                }
                else
                {
                    moves.AddRange(GeneratePawnMoves(fromSquareId, 1, 6, 8, false));
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

        List<Move> GeneratePawnMoves(int fromSquareId, int direction, int startDirectionIndex, int endDirectionIndex, bool isWhite)
        {
            var piece = GetPiece(fromSquareId);
            var moves = new List<Move>();
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
                moves.Add(new Move(fromSquareId, toSquareId));
            }

            // Or two from the starting rank
            toSquareId = fromSquareId + (DirectionOffsets[direction] * 2);
            if (fromSquareId > rankStartSquareIndex && fromSquareId < rankEndSquareIndex)
            {
                if (!SquareContainsPiece(toSquareId))
                {
                    moves.Add(new Move(fromSquareId, toSquareId) { Flag = MoveFlag.PawnTwoForward });
                }
            }

            // And can capture diagonally
            var diagonalCaptures = GenerateSlidingMoves(fromSquareId, startDirectionIndex, endDirectionIndex, 1);
            foreach (var diagonalCapture in diagonalCaptures)
            {
                if (SquareContainsEnemyPiece(piece.IsWhite, diagonalCapture.ToSquareId) || EnPassantSquare == diagonalCapture.ToSquareId)
                {
                    if(EnPassantSquare == diagonalCapture.ToSquareId)
                    {
                        var enemyPieceId = Squares[diagonalCapture.ToSquareId + DirectionOffsets[Convert.ToInt32(IsWhiteTurn)]].PieceId;
                        diagonalCapture.CapturedPieceId = enemyPieceId;
                    }
                    else
                    {
                        diagonalCapture.CapturedPieceId = Squares[diagonalCapture.ToSquareId].PieceId;
                    }
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

        public void HighlightMoves(IEnumerable<Move> moves)
        {
            foreach (var move in moves)
            {
                Squares[move.ToSquareId].IsHighlighted = true;
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
