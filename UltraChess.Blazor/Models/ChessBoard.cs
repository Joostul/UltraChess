using System;
using System.Collections.Generic;
using System.Linq;
using UltraChess.Blazor.Constants;
using UltraChess.Blazor.Utility;

namespace UltraChess.Blazor.Models
{
    public class ChessBoard
    {
        // Get number of squares to edge based on square index and direction: 0: North, 1: South, 2: East,  3: West, 4: NorthWest 5: NorthEast, 6: SouthEast, 7: SouthWest
        public readonly int[][] NumberOfSquaresToEdge = new int[64][];
        public readonly int[][] KnightMoves = new int[64][];
        public Square[] Squares = new Square[64];
        public List<Move> LegalMoves;
        public Stack<Move> MoveHistory = new();

        public BoardInfo CurrentBoardInfo = new();
        public Stack<BoardInfo> BoardInfoHistory = new();
        public Dictionary<int, Piece>[] Pieces;
        public int[] AttackedSquares;

        public ChessBoard(string FEN)
        {
            CurrentBoardInfo = FENUtility.GetBoardPositionInfo(FEN);

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
                    foreach (var knightJumpDelta in ChessConstants.AllKnightJumps)
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
                        Rank = ChessConstants.BoardNumbers[file],
                        File = ChessConstants.BoardCharacters[rank],
                        IsLight = (file + rank) % 2 == 0
                    };
                    var fenCharacter = CurrentBoardInfo.Board[squareIndex];
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

            AttackedSquares = GenerateMoves(!CurrentBoardInfo.IsWhiteTurn, true).Select(m => m.ToSquareId).ToArray();
            LegalMoves = GenerateLegalMoves(CurrentBoardInfo.IsWhiteTurn);
        }

        public bool MakeMove(Move move)
        {
            int fromSquareId = move.FromSquareId;
            int toSquareId = move.ToSquareId;

            // If we are moving to the same square
            if (fromSquareId == toSquareId)
            {
                return false;
            }
            else
            {
                MoveHistory.Push(move);
                BoardInfoHistory.Push(new BoardInfo(CurrentBoardInfo));
                SetCastlingRights(move);
                CurrentBoardInfo.EnPassantSquareId = 64;
                switch (move.Flag)
                {
                    case MoveFlag.EnPassantCapture:
                        // Move the pawn to the en passant square
                        Squares[toSquareId].PieceId = Squares[fromSquareId].PieceId;
                        Squares[fromSquareId].PieceId = 0;
                        // Remove the piece that was en passanted
                        Squares[toSquareId + ChessConstants.DirectionOffsets[Convert.ToInt32(CurrentBoardInfo.IsWhiteTurn)]].PieceId = 0;
                        break;
                    case MoveFlag.PawnPromotion:
                        Squares[fromSquareId].PieceId = 0;
                        Squares[toSquareId].PieceId = move.PromotionPieceId;
                        break;
                    case MoveFlag.PawnTwoForward:
                        Squares[toSquareId].PieceId = Squares[fromSquareId].PieceId;
                        Squares[fromSquareId].PieceId = 0;
                        CurrentBoardInfo.EnPassantSquareId = toSquareId + ChessConstants.DirectionOffsets[Convert.ToInt32(CurrentBoardInfo.IsWhiteTurn)];
                        break;
                    case MoveFlag.Castling:
                        if (Squares[fromSquareId].PieceId == 6)
                        {
                            CurrentBoardInfo.WhiteCanCastleKingSide = false;
                            CurrentBoardInfo.WhiteCanCastleQueenSide = false;
                            Squares[toSquareId].PieceId = 6;
                            if (move.ToSquareId == 58)
                            {
                                Squares[56].PieceId = 0;
                                Squares[59].PieceId = 4;
                            }
                            else if (move.ToSquareId == 62)
                            {
                                Squares[63].PieceId = 0;
                                Squares[61].PieceId = 4;
                            }
                        }
                        else
                        {
                            CurrentBoardInfo.BlackCanCastleKingSide = false;
                            CurrentBoardInfo.BlackCanCastleQueenSide = false;
                            Squares[toSquareId].PieceId = 12;
                            if (move.ToSquareId == 2)
                            {
                                Squares[0].PieceId = 0;
                                Squares[3].PieceId = 10;
                            }
                            else if (move.ToSquareId == 6)
                            {
                                Squares[7].PieceId = 0;
                                Squares[5].PieceId = 10;
                            }
                        }
                        Squares[fromSquareId].PieceId = 0;
                        break;
                    case MoveFlag.None:
                    default:
                        MoveToSquare(fromSquareId, toSquareId);
                        break;
                }

                CurrentBoardInfo.IsWhiteTurn = !CurrentBoardInfo.IsWhiteTurn;
                return true;
            }
        }

        public void UnMakeMove(Move move)
        {
            var previousBoardInfo = BoardInfoHistory.Peek();
            switch (move.Flag)
            {
                case MoveFlag.EnPassantCapture:
                    // Move the piece back
                    Squares[move.FromSquareId].PieceId = Squares[move.ToSquareId].PieceId;
                    // Move the captured pawn back to original square
                    Squares[move.ToSquareId - ChessConstants.DirectionOffsets[Convert.ToInt32(CurrentBoardInfo.IsWhiteTurn)]].PieceId = move.CapturedPieceId;
                    // Set the captured square back to 0 piece
                    Squares[move.ToSquareId].PieceId = 0;
                    break;
                case MoveFlag.PawnPromotion:
                    Squares[move.FromSquareId].PieceId = GetPiece(move.ToSquareId).IsWhite ? 1 : 7;
                    Squares[move.ToSquareId].PieceId = move.CapturedPieceId;
                    break;
                case MoveFlag.Castling:
                    if (Squares[move.ToSquareId].PieceId == 6)
                    {
                        Squares[move.FromSquareId].PieceId = 6;
                        if (move.ToSquareId == 58)
                        {
                            Squares[59].PieceId = 0;
                            Squares[56].PieceId = 4;
                        }
                        else if (move.ToSquareId == 62)
                        {
                            Squares[61].PieceId = 0;
                            Squares[63].PieceId = 4;
                        }
                    }
                    else
                    {
                        Squares[move.FromSquareId].PieceId = 12;
                        if (move.ToSquareId == 2)
                        {
                            Squares[3].PieceId = 0;
                            Squares[0].PieceId = 10;
                        }
                        else if (move.ToSquareId == 6)
                        {
                            Squares[5].PieceId = 0;
                            Squares[7].PieceId = 10;
                        }
                    }
                    Squares[move.ToSquareId].PieceId = 0;
                    break;
                case MoveFlag.None:
                case MoveFlag.PawnTwoForward:
                default:
                    UnMoveToSquare(move.FromSquareId, move.ToSquareId, move.CapturedPieceId);
                    break;
            }
            // Set back to correct turn
            CurrentBoardInfo = previousBoardInfo;

            MoveHistory.Pop();
            BoardInfoHistory.Pop();
        }

        public void MoveToSquare(int fromSquareId, int toSquareId)
        {
            Squares[toSquareId].PieceId = Squares[fromSquareId].PieceId;
            Squares[fromSquareId].PieceId = 0;
        }

        private void SetCastlingRights(Move move)
        {
            // If rook on A8 is captured or moved, black can't castle there anymore
            if (move.FromSquareId == 0 || move.ToSquareId == 0)
            {
                CurrentBoardInfo.BlackCanCastleQueenSide = false;
            }
            // If rook on H8 is captured or moved, black can't castle there anymore
            if (move.FromSquareId == 7 || move.ToSquareId == 7)
            {
                CurrentBoardInfo.BlackCanCastleKingSide = false;
            }
            // If the black king moves, black can't castle anymore
            if (move.FromSquareId == 4)
            {
                CurrentBoardInfo.BlackCanCastleKingSide = false;
                CurrentBoardInfo.BlackCanCastleQueenSide = false;
            }

            // If rook on A1 is captured or moved, white can't castle there anymore
            if (move.FromSquareId == 56 || move.ToSquareId == 56)
            {
                CurrentBoardInfo.WhiteCanCastleQueenSide = false;
            }
            // If rook on H1 is captured or moved, white can't castle there anymore
            if (move.FromSquareId == 63 || move.ToSquareId == 63)
            {
                CurrentBoardInfo.WhiteCanCastleKingSide = false;
            }
            // If the white king moves, white can't castle anymore
            if (move.FromSquareId == 60)
            {
                CurrentBoardInfo.WhiteCanCastleKingSide = false;
                CurrentBoardInfo.WhiteCanCastleQueenSide = false;
            }

        }

        public void UnMoveToSquare(int fromSquareId, int toSquareId, int capturedPieceId = 0)
        {
            // Move the piece back
            Squares[fromSquareId].PieceId = Squares[toSquareId].PieceId;
            if (capturedPieceId == 0)
            {
                // If there was no capture, set the tosquare piece back to nothing
                Squares[toSquareId].PieceId = 0;
            }
            else
            {
                // Set the captured piece back
                Squares[toSquareId].PieceId = capturedPieceId;
            }
        }

        public List<Move> GenerateMoves(bool isWhite, bool attacksOnly = false)
        {
            var moves = new List<Move>();

            foreach (var square in Squares)
            {
                if (SquareContainsPiece(square.Id))
                {
                    moves.AddRange(GetMovesFromSquare(square.Id, isWhite, attacksOnly));
                }
            }

            return moves;
        }

        public List<Move> GenerateLegalMoves(bool isWhite)
        {
            var yourPieceKingId = isWhite ? 6 : 12;
            var pseudoLegalMoves = GenerateMoves(isWhite);
            var legalMoves = new List<Move>();

            foreach (var move in pseudoLegalMoves)
            {
                MakeMove(move);
                AttackedSquares = GenerateMoves(CurrentBoardInfo.IsWhiteTurn, true).Select(m => m.ToSquareId).ToArray();
                var yourKingSquareId = Squares.Single(s => s.PieceId == yourPieceKingId).Id;

                if (move.Flag == MoveFlag.Castling)
                {
                    int middleSquareId;
                    if (move.FromSquareId > move.ToSquareId)
                    {
                        middleSquareId = move.FromSquareId - 1;
                    }
                    else
                    {
                        middleSquareId = move.FromSquareId + 1;
                    }
                    if (!AttackedSquares.Contains(yourKingSquareId) && !AttackedSquares.Contains(move.ToSquareId) && !AttackedSquares.Contains(middleSquareId) && !AttackedSquares.Contains(move.FromSquareId))
                    {
                        legalMoves.Add(move);
                    }
                }
                // If your king is not in an attacked square
                else if (!AttackedSquares.Contains(yourKingSquareId))
                {
                    legalMoves.Add(move);
                }

                UnMakeMove(move);
            }

            return legalMoves;
        }

        public List<Move> GetMovesFromSquare(int fromSquareId, bool isWhite, bool attacksOnly = false)
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
                    moves.AddRange(GeneratePawnMoves(fromSquareId, 0, 4, 6, attacksOnly));
                }
                else
                {
                    moves.AddRange(GeneratePawnMoves(fromSquareId, 1, 6, 8, attacksOnly));
                }
            }
            else if (piece is King)
            {
                moves.AddRange(GenerateKingMoves(fromSquareId));
            }
            else if (piece is Rook)
            {
                var rookMoves = GenerateSlidingMoves(fromSquareId, 0, 4);
                moves.AddRange(rookMoves);
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

        List<Move> GeneratePawnMoves(int fromSquareId, int direction, int startDirectionIndex, int endDirectionIndex, bool attacksOnly = false)
        {
            var piece = GetPiece(fromSquareId);

            var moves = new List<Move>();
            var promotionRank = piece.IsWhite ? '8' : '1';
            var startRank = piece.IsWhite ? '2' : '7';

            if (!attacksOnly)
            {
                // Pawns can move up one
                var toSquareId = fromSquareId + ChessConstants.DirectionOffsets[direction];
                if (!SquareContainsPiece(toSquareId))
                {
                    moves.Add(new Move(fromSquareId, toSquareId));
                }

                // Or two from the starting rank
                var toSquareIdTwoForward = fromSquareId + (ChessConstants.DirectionOffsets[direction] * 2);
                if (Squares[fromSquareId].Rank == startRank)
                {
                    if (!SquareContainsPiece(toSquareIdTwoForward) && !SquareContainsPiece(toSquareId))
                    {
                        moves.Add(new Move(fromSquareId, toSquareIdTwoForward) { Flag = MoveFlag.PawnTwoForward });
                    }
                }
            }

            // And can capture diagonally
            var diagonalCaptures = GenerateSlidingMoves(fromSquareId, startDirectionIndex, endDirectionIndex, 1);
            if (attacksOnly)
            {
                moves.AddRange(diagonalCaptures);
            }
            else
            {
                foreach (var diagonalCapture in diagonalCaptures)
                {
                    if (SquareContainsEnemyPiece(piece.IsWhite, diagonalCapture.ToSquareId))
                    {
                        diagonalCapture.CapturedPieceId = Squares[diagonalCapture.ToSquareId].PieceId;
                        moves.Add(diagonalCapture);
                    }
                    else if (CurrentBoardInfo.EnPassantSquareId == diagonalCapture.ToSquareId && SquareContainsEnemyPiece(piece.IsWhite, Squares[diagonalCapture.ToSquareId + ChessConstants.DirectionOffsets[Convert.ToInt32(CurrentBoardInfo.IsWhiteTurn)]].Id))
                    {
                        var enemyPieceSquare = Squares[diagonalCapture.ToSquareId + ChessConstants.DirectionOffsets[Convert.ToInt32(CurrentBoardInfo.IsWhiteTurn)]].PieceId;
                        diagonalCapture.CapturedPieceId = enemyPieceSquare;
                        diagonalCapture.Flag = MoveFlag.EnPassantCapture;
                        moves.Add(diagonalCapture);
                    }
                }
            }

            var promotionMoves = new List<Move>();

            // Tag promotions and add piece variations
            foreach (var move in moves)
            {
                if (Squares[move.ToSquareId].Rank == promotionRank)
                {
                    var promotionPieceIsWhite = GetPiece(move.FromSquareId).IsWhite;
                    move.Flag = MoveFlag.PawnPromotion;
                    move.PromotionPieceId = promotionPieceIsWhite ? 2 : 8;

                    promotionMoves.Add(new Move(move.FromSquareId, move.ToSquareId)
                    {
                        CapturedPieceId = move.CapturedPieceId,
                        Flag = MoveFlag.PawnPromotion,
                        PromotionPieceId = promotionPieceIsWhite ? 3 : 9
                    });
                    promotionMoves.Add(new Move(move.FromSquareId, move.ToSquareId)
                    {
                        CapturedPieceId = move.CapturedPieceId,
                        Flag = MoveFlag.PawnPromotion,
                        PromotionPieceId = promotionPieceIsWhite ? 4 : 10
                    });
                    promotionMoves.Add(new Move(move.FromSquareId, move.ToSquareId)
                    {
                        CapturedPieceId = move.CapturedPieceId,
                        Flag = MoveFlag.PawnPromotion,
                        PromotionPieceId = promotionPieceIsWhite ? 5 : 11
                    });
                }
            }
            moves.AddRange(promotionMoves);
            return moves;
        }

        List<Move> GenerateKingMoves(int fromSquareId)
        {
            var piece = GetPiece(fromSquareId);
            var moves = GenerateSlidingMoves(fromSquareId, 0, 8, 1);

            if (piece.IsWhite && CurrentBoardInfo.WhiteCanCastleKingSide && !SquareContainsPiece(61) && !SquareContainsPiece(62) && fromSquareId == 60)
            {
                moves.Add(new Move(60, 62) { Flag = MoveFlag.Castling });
            }
            if (piece.IsWhite && CurrentBoardInfo.WhiteCanCastleQueenSide && !SquareContainsPiece(59) && !SquareContainsPiece(58) && !SquareContainsPiece(57) && fromSquareId == 60)
            {
                moves.Add(new Move(60, 58) { Flag = MoveFlag.Castling });
            }
            if (!piece.IsWhite && CurrentBoardInfo.BlackCanCastleKingSide && !SquareContainsPiece(5) && !SquareContainsPiece(6) && fromSquareId == 4)
            {
                moves.Add(new Move(4, 6) { Flag = MoveFlag.Castling });
            }
            if (!piece.IsWhite && CurrentBoardInfo.BlackCanCastleQueenSide && !SquareContainsPiece(3) && !SquareContainsPiece(2) && !SquareContainsPiece(1) && fromSquareId == 4)
            {
                moves.Add(new Move(4, 2) { Flag = MoveFlag.Castling });
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
                        moves.Add(new Move(fromSquareId, toSquareId, Squares[toSquareId].PieceId));
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
                        var toSquareId = fromSquareId + ChessConstants.DirectionOffsets[directionIndex] * (i + 1);

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
            return ChessConstants.Pieces[Squares[squareId].PieceId];
        }
    }
}
