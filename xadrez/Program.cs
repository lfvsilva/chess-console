using board;
using chess;
using xadrez;
internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            Board board = new Board(8, 8);
            board.placePiece(new Rook(board, Color.White), new Position(0, 0));
            board.placePiece(new Rook(board, Color.White), new Position(1, 3));
            board.placePiece(new Rook(board, Color.White), new Position(0, 0));
            board.placePiece(new Rook(board, Color.White), new Position(0, 0));
            Screen.PrintBoard(board);

            Screen.PrintBoard(board);

            Console.ReadLine();
        }
        catch (BoardException e)
        {
            Console.WriteLine(e.Message);
        }
        


        
    }
}