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
                    Console.WriteLine($"Turn: {match.turn}");
                    Console.WriteLine($"Waiting for: {match.currentPlayer}");

                    Console.WriteLine();
                    Console.Write("Origin: ");
                    Position origin = Screen.readChessPosition().toPosition();
                    match.validateOriginPosition(origin);

                    bool[,] possibleMoves = match.board.getPiece(origin).possibleMoves();

                    Console.Clear();
                    Screen.printBoard(match.board, possibleMoves);

                    Console.WriteLine();
                    Console.Write("Destination: ");
                    Position destination = Screen.readChessPosition().toPosition();
                    match.validateDestinationPosition(origin, destination);

                    match.executePlay(origin, destination);
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