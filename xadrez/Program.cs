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
                    Screen.printMatch(match);

                    Console.WriteLine();
                    Console.Write("Origin: ");
                    Position origin = Screen.readChessPosition().toPosition();
                    match.validateOriginPosition(origin);

                    bool[,] possibleMoves = match.legalMoves(origin);

                    Console.Clear();
                    Screen.printBoard(match.board, possibleMoves);

                    Console.WriteLine();
                    Console.Write("Destination: ");
                    Position destination = Screen.readChessPosition().toPosition();
                    match.validateDestinationPosition(origin, destination);

                    char promotionPiece = 'Q';
                    if (match.isPromotionMove(origin, destination))
                    {
                        promotionPiece = Screen.readPromotionPiece();
                    }

                    match.ExecutePlay(origin, destination, promotionPiece);
                }
                catch (BoardException e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }
            Console.Clear();
            Screen.printMatch(match);
        }
        catch (BoardException e)
        {

            Console.WriteLine( e.Message); 
        }

        

        Console.ReadLine();
        
        


        
    }
}