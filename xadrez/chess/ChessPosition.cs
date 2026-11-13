using System.Diagnostics.CodeAnalysis;
using board;

namespace chess
{
    internal class ChessPosition
    {
        public char column { get; set; }
        public int row { get; set; }

        public ChessPosition(char column, int row)
        {
            this.column = column;
            this.row = row;
        }

        public Position toPosition()
        {
            return new Position(8 - row, column - 'a');
        }

        // Parses text like "e2" (case-insensitive, surrounding spaces ignored).
        // Returns false for anything that is not a column a-h followed by a row 1-8.
        public static bool TryParse(string? text, [NotNullWhen(true)] out ChessPosition? result)
        {
            result = null;
            if (text == null)
            {
                return false;
            }
            text = text.Trim().ToLowerInvariant();
            if (text.Length != 2)
            {
                return false;
            }
            char column = text[0];
            char rowChar = text[1];
            if (column < 'a' || column > 'h' || rowChar < '1' || rowChar > '8')
            {
                return false;
            }
            result = new ChessPosition(column, rowChar - '0');
            return true;
        }

        public override string ToString()
        {
            return $"{column}{row}";
        }
    }
}
