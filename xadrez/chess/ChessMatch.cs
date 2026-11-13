using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;
using board;

namespace chess
{
    internal class ChessMatch
    {
        public Board board { get; private set; }
        public int turn { get; private set; }
        public Color currentPlayer { get; private set; }

        public bool finished { get; private set; }
        private HashSet<Piece> pieces;
        private HashSet<Piece> capturedPieces;
        public bool check { get; private set; }

        public ChessMatch()
        {
            board = new Board(8, 8);
            turn = 1;
            currentPlayer = Color.White;
            finished = false;
            pieces = new HashSet<Piece>();
            capturedPieces = new HashSet<Piece>();
            check = false;

            placePieces();

        }

        public Piece executeMove(Position origin, Position destination)
        {
            Piece p = board.removePiece(origin);
            p.increaseMoveCount();
            Piece capturedPiece = board.removePiece(destination);
            p.position = destination;
            board.placePiece(p, destination);
            if (capturedPiece != null)
            {
                capturedPieces.Add(capturedPiece);
            }
            return capturedPiece;
        }

        public void UndoMove(Position origin, Position destination, Piece capturedPiece)
        {
            Piece p = board.removePiece(destination);
            p.decreaseMoveCount();
            if (capturedPiece != null)
            {
                board.placePiece(capturedPiece, destination);
                capturedPieces.Remove(capturedPiece);
            }
            board.placePiece(p, origin);
        }

        public void ExecutePlay(Position origin, Position destination)
        {
            Piece capturedPiece = executeMove(origin, destination);

            if (isInCheck(currentPlayer))
            {
                UndoMove(origin, destination, capturedPiece);
                throw new BoardException("You cannot put yourself in check!");
            }

            if (isInCheck(opponent(currentPlayer)))
            {
                check = true;
            }
            else
            {
                check = false;
            }
            if (isCheckMate(opponent(currentPlayer)))
            {
                finished = true;
            }
            else
            {
                turn++;
                changePlayer();
            }
        }

        public void validateOriginPosition(Position pos)
        {
            if (board.getPiece(pos) == null)
            {
                throw new BoardException("There is no piece in this position!");
            }
            if (currentPlayer != board.getPiece(pos).color)
            {
                throw new BoardException("The chosen piece is not yours!");
            }
            if (!board.getPiece(pos).isThereAnyPossibleMove())
            {
                throw new BoardException("There are no possible moves for the chosen piece!");
            }
        }

        public void validateDestinationPosition(Position origin, Position destination)
        {
            if (!board.getPiece(origin).canMoveTo(destination))
            {
                throw new BoardException("Invalid destination position!");
            }
        }

        private void changePlayer()
        {
            currentPlayer = (currentPlayer == Color.White) ? Color.Black : Color.White;
        }

        public HashSet<Piece> capturedPiecesByColor(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece x in capturedPieces)
            {
                if (x.color == color)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Piece> piecesInGameByColor(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece x in pieces)
            {
                if (x.color == color)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(capturedPiecesByColor(color));
            return aux;
        }

        private Color opponent(Color color)
        {
            return (color == Color.White) ? Color.Black : Color.White;
        }

        private Piece king(Color color)
        {
            foreach (Piece x in piecesInGameByColor(color))
            {
                if (x is King)
                {
                    return x;
                }
            }
            return null;
        }

        public bool isInCheck(Color color)
        {
            Piece K = king(color);
            if (K == null)
            {
                throw new BoardException($"There is no {color} king on the board!");
            }
            foreach (Piece x in piecesInGameByColor(opponent(color)))
            {
                bool[,] mat = x.possibleMoves();
                if (mat[K.position.row, K.position.column])
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCheckMate(Color color)
        {
            if (!isInCheck(color))
            {
                return false;
            }
            foreach (Piece x in piecesInGameByColor(color))
            {
                bool[,] mat = x.possibleMoves();
                for (int i = 0; i < board.rows; i++)
                {
                    for (int j = 0; j < board.columns; j++)
                    {
                        if (mat[i, j])
                        {
                           Position origin = x.position;
                            Position destination = new Position(i, j);
                            Piece capturedPiece = executeMove(origin, destination);
                            bool testCheck = isInCheck(color);
                            UndoMove(origin, destination, capturedPiece);
                            if (!testCheck)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void placeNewPiece(char column, int row, Piece piece)
        {
            board.placePiece(piece, new ChessPosition(column, row).toPosition());
            pieces.Add(piece);
        }

        private void placePieces()
        {
            placeNewPiece('c', 1, new Rook(board, Color.White));
            placeNewPiece('c', 2, new Rook(board, Color.White));
            placeNewPiece('d', 2, new Rook(board, Color.White));
            placeNewPiece('e', 2, new Rook(board, Color.White));
            placeNewPiece('e', 1, new Rook(board, Color.White));
            placeNewPiece('d', 1, new King(board, Color.White));

            placeNewPiece('c', 8, new Rook(board, Color.Black));
            placeNewPiece('c', 7, new Rook(board, Color.Black));
            placeNewPiece('d', 7, new Rook(board, Color.Black));
            placeNewPiece('e', 7, new Rook(board, Color.Black));
            placeNewPiece('e', 8, new Rook(board, Color.Black));
            placeNewPiece('d', 8, new King(board, Color.Black));
        }
    }
}
