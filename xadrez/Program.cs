using board;
using chess;
using xadrez;
internal class Program
{
    private static void Main(string[] args)
    {

        try
        {
            ChessMatch match = new ChessMatch();

            while (!match.finished)
            {
                try
                {
                    Console.Clear();
                    Screen.printBoard(match.board);

                    Console.WriteLine();
                    Console.Write("Origin: ");
                    Position origin = Screen.readChessPosition().toPosition();

                    bool[,] possibleMoves = match.board.getPiece(origin).possibleMoves();

                    Console.Clear();
                    Screen.printBoard(match.board, possibleMoves);

                    Console.WriteLine();
                    Console.Write("Destination: ");
                    Position destination = Screen.readChessPosition().toPosition();

                    match.executeMove(origin, destination);
                }
                catch (BoardException e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }

            Screen.printBoard(match.board);
        }
        catch (BoardException e)
        {

            Console.WriteLine( e.Message); 
        }

        

        Console.ReadLine();
        
        


        
    }
}