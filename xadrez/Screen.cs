using board;

namespace xadrez
{
    class Screen
    {
        public static void PrintBoard(Board board)
        {
            for (int i = 0; i < board.rows; i++)
            {
                for (int j = 0; j < board.columns; j++)
                {
                    if (board.getPiece(i, j) == null)
                    {
                        Console.Write("- ");
                    }
                    else
                    {
                        Console.Write(board.getPiece(i, j) + " ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
