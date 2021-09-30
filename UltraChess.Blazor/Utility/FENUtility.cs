using System.Collections.Generic;

namespace UltraChess.Blazor.Utility
{
    public static class FENUtility
    {
        public static char[] ParseFENString(string FENstring)
        {
            List<char> board = new();

            string[] FENparts = FENstring.Split(' ');
            string[] FENranks = FENparts[0].Split('/');
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


            return board.ToArray();
        }
    }
}
