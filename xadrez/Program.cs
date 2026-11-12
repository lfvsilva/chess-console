using board;
using chess;
internal class Program
{
    private static void Main(string[] args)
    {
        Board board = new Board(8, 8);

        board.placePiece(new Rook(board, Color.White), new Position(0, 0));
        board.placePiece(new Rook(board, Color.White), new Position(1, 3));
        board.placePiece(new Rook(board, Color.White), new Position(0, 0));
        board.placePiece(new Rook(board, Color.White), new Position(0, 0));


        Screen.PrintBoard(board);

        Console.ReadLine();
    }
}