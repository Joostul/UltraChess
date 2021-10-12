using System.Collections.Generic;

namespace UltraChess.Blazor.Utility
{
    public static class FENUtility
    {
        public static BoardPositionInfo GetBoardPositionInfo(string FENString)
        {
            string[] FENparts = FENString.Split(' ');
            string[] FENranks = FENparts[0].Split('/');
            string castlingRights = (FENparts.Length > 2) ? FENparts[2] : "KQkq";

            var boardPositionInfo = new BoardPositionInfo 
            { 
                IsWhiteTurn = FENparts[1][0] == 'w',
                WhiteCanCastleKingSide = castlingRights.Contains('K'),
                WhiteCanCastleQueenSide = castlingRights.Contains('Q'),
                BlackCanCastleKingSide = castlingRights.Contains('k'),
                BlackCanCastleQueenSide = castlingRights.Contains('q')
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
                boardPositionInfo.EnPassantSquare = FENparts[3];
            }

            return boardPositionInfo;
        }
    }
}
