using System;
using System.Collections.Generic;
using System.Text;

namespace UltraChess.Blazor.Utility
{
    public static class FENUtility
    {
        private static readonly char[] _ranks = "87654321".ToCharArray();
        private static readonly char[] _files = "abcdefgh".ToCharArray();
        private static readonly string[] _squares = new[]
        {
            "a8", "b8","c8","d8","e8","f8","g8","h8",
            "a7", "b7","c7","d7","e7","f7","g7","h7",
            "a6", "b6","c6","d6","e6","f6","g6","h6",
            "a5", "b5","c5","d5","e5","f5","g5","h5",
            "a4", "b4","c4","d4","e4","f4","g4","h4",
            "a3", "b3","c3","d3","e3","f3","g3","h3",
            "a2", "b2","c2","d2","e2","f2","g2","h2",
            "a1", "b1","c1","d1","e1","f1","g1","h1"
        };

        public static BoardInfo GetBoardPositionInfo(string FENString)
        {
            string[] FENparts = FENString.Split(' ');
            string[] FENranks = FENparts[0].Split('/');
            string castlingRights = (FENparts.Length > 2) ? FENparts[2] : "KQkq";

            var boardPositionInfo = new BoardInfo
            {
                IsWhiteTurn = FENparts[1][0] == 'w',
                WhiteCanCastleKingSide = castlingRights.Contains('K'),
                WhiteCanCastleQueenSide = castlingRights.Contains('Q'),
                BlackCanCastleKingSide = castlingRights.Contains('k'),
                BlackCanCastleQueenSide = castlingRights.Contains('q'),
                HalfClockMove = FENparts.Length > 4 ? Convert.ToInt32(char.GetNumericValue(FENparts[4][0])) : 0,
                FullMoveNumber = FENparts.Length > 5 ? Convert.ToInt32(char.GetNumericValue(FENparts[5][0])) : 0
            };

            List<char> board = new();

            foreach (var rank in FENranks)
            {
                foreach (var character in rank)
                {
                    if (char.IsDigit(character))
                    {
                        for (int i = 0; i < int.Parse(character.ToString()); i++)
                        {
                            board.Add('0');
                        }
                    }
                    else
                    {
                        board.Add(character);
                    }
                }
            }
            boardPositionInfo.Board = board.ToArray();

            if (FENparts.Length > 3)
            {
                if (FENparts[3][0] != '-')
                {
                    var fileIndex = Array.IndexOf(_files, FENparts[3][0]);
                    var rankIndex = Array.IndexOf(_ranks, FENparts[3][1]);
                    boardPositionInfo.EnPassantSquareId = rankIndex * 8 + fileIndex;
                }
                else
                {
                    boardPositionInfo.EnPassantSquareId = 64;
                }
            }
            else
            {
                boardPositionInfo.EnPassantSquareId = 64;
            }

            return boardPositionInfo;
        }

        public static string GetFENString(BoardInfo boardInfo)
        {
            StringBuilder builder = new();
            for (int file = 0; file < 8; file++)
            {
                for (int rank = 0; rank < 8; rank++)
                {
                    var charPosition = file * 8 + rank;
                    var character = boardInfo.Board[charPosition];
                    if (character == '0')
                    {
                        var emptyCells = Math.Min(GetAmountOfNextCharactersContaining0(boardInfo.Board, charPosition), 8);
                        builder.Append(emptyCells);
                        rank += emptyCells;
                    }
                    else
                    {
                        builder.Append(character);
                    }
                }
                if(file < 7)
                {
                    builder.Append('/');
                }
            }

            // Turn
            char isWhiteTurn = boardInfo.IsWhiteTurn ? 'w' : 'b';
            builder.Append($" {isWhiteTurn} ");

            // Castling
            if (boardInfo.WhiteCanCastleKingSide)
            {
                builder.Append('K');
            }
            if (boardInfo.WhiteCanCastleQueenSide)
            {
                builder.Append('Q');
            }
            if (boardInfo.BlackCanCastleKingSide)
            {
                builder.Append('k');
            }
            if (boardInfo.BlackCanCastleQueenSide)
            {
                builder.Append('q');
            }
            if(!boardInfo.WhiteCanCastleKingSide && !boardInfo.WhiteCanCastleQueenSide && !boardInfo.BlackCanCastleKingSide && !boardInfo.BlackCanCastleQueenSide)
            {
                builder.Append('-');
            }

            // En passant
            if(boardInfo.EnPassantSquareId == 64)
            {
                builder.Append(" -");
            }
            else
            {
                builder.Append($" {_squares[boardInfo.EnPassantSquareId]}");
            }

            builder.Append($" {boardInfo.HalfClockMove}");
            builder.Append($" {boardInfo.FullMoveNumber}");

            return builder.ToString();
        }

        public static int GetAmountOfNextCharactersContaining0(char[] board, int position)
        {
            int amountZeros = 0;
            for (int i = position; i < board.Length; i++)
            {
                if(board[i] == '0')
                {
                    amountZeros += 1;
                }
                else
                {
                    return amountZeros;
                }                
            }

            return amountZeros;
        }
    }
}
