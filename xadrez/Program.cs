using board;
using chess;
using xadrez;
internal class Program
{
    private static void Main(string[] args)
    {
        Board board = new Board(8, 8);

        board.placePiece(new Rook(board, Color.White), new Position(0, 0));
        board.placePiece(new Rook(board, Color.White), new Position(1, 3));
        board.placePiece(new Rook(board, Color.White), new Position(0, 2));

        board.placePiece(new Rook(board, Color.Black), new Position(3, 5));

        Screen.printBoard(board);

        Console.ReadLine();
        
        


        
    }
}