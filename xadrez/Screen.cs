using board;
using chess;
using xadrez;
using System.Collections.Generic;

namespace xadrez
{
    class Screen
    {

        public static void printMatch(ChessMatch match)
        {
            printBoard(match.board);
            Console.WriteLine();
            printCapturedPieces(match);
            Console.WriteLine();
            Console.WriteLine($"Turn: {match.turn}");
            if (!match.finished)
            {
                if(match.check)
                {
                    Console.WriteLine("CHECK!");
                }
                Console.WriteLine($"Waiting for player: {match.currentPlayer}");
            }
            else if (match.stalemate)
            {
                Console.WriteLine("STALEMATE!");
                Console.WriteLine($"Draw: {match.opponent(match.currentPlayer)} has no legal moves.");
            }
            else
            {
                Console.WriteLine("CHECKMATE!");
                Console.WriteLine($"Winner: {match.currentPlayer}");
            }
        }

        public static void printCapturedPieces(ChessMatch match)
        {
            Console.WriteLine("Captured pieces: ");
            Console.Write("White: ");
            printSet(match.capturedPiecesByColor(Color.White));
            Console.WriteLine();
            Console.Write("Black: ");
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            printSet(match.capturedPiecesByColor(Color.Black));
            Console.ForegroundColor = aux;
            Console.WriteLine();
        }
        public static void printSet(HashSet<Piece> set)
        {
            Console.Write("[");
            foreach (Piece x in set)
            {
                Console.Write(x + " ");
            }
            Console.Write("]");
        }

        public static void printBoard(Board board)
        {
            for (int i = 0; i < board.rows; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < board.columns; j++)
                {
                    Screen.PrintPiece(board.getPiece(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine($"  a b c d e f g h");
        }
        public static void printBoard(Board board, bool[,] possibleMoves)
        {   
            for (int i = 0; i < board.rows; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < board.columns; j++)
                {
                    Screen.PrintPiece(board.getPiece(i, j), possibleMoves[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine($"  a b c d e f g h");
        }
        public static void PrintPiece(Piece piece)
        {
            if (piece == null)
            {
                Console.Write("- ");
            }
            else
            {
                if (piece.color == Color.White)
                {
                    Console.Write(piece);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(piece);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }

        public static void PrintPiece(Piece piece, bool isPossibleMove)
        {
            ConsoleColor originalBackground = Console.BackgroundColor;
            ConsoleColor changedBackground = ConsoleColor.DarkGray;

            if(isPossibleMove)
            {
                Console.BackgroundColor = changedBackground;
            }
            else
            {
                Console.BackgroundColor = originalBackground;
            }
            if (piece == null)
            {
                Console.Write("- ");     
            }
            else
            {
                if (piece.color == Color.White)
                {
                    Console.Write(piece);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(piece);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
            Console.BackgroundColor = originalBackground;
        }
        public static ChessPosition readChessPosition()
        {
            string? s = Console.ReadLine();
            if (!ChessPosition.TryParse(s, out ChessPosition? position))
            {
                throw new BoardException("Invalid position! Type a column a-h followed by a row 1-8 (e.g. e2).");
            }
            return position;
        }
    }
}
