using board;
using xadrez;
internal class Program
{
    private static void Main(string[] args)
    {
        Board board = new Board(8, 8);
        

        Screen.PrintBoard(board);

        Console.ReadLine();
    }
}