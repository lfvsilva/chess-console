using board;
using chess;
using xadrez;
internal class Program
{
    private static void Main(string[] args)
    {
        ChessPosition pos = new ChessPosition('c', 7);

        Console.WriteLine(pos);

        Console.WriteLine(pos.toPosition());

        Console.ReadLine();
        
        


        
    }
}