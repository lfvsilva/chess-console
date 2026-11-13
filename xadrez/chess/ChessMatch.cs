using System;
using System.Collections.Generic;
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

        public ChessMatch()
        {
            board = new Board(8, 8);
            turn = 1;
            currentPlayer = Color.White;
            finished = false;
            placePieces();

        }

        public void executeMove(Position origin, Position destination)
        {
            Piece p = board.removePiece(origin);
            p.increaseMoveCount();
            Piece capturedPiece = board.removePiece(destination);
            p.position = destination;
            board.placePiece(p, destination);
        }

        public void executePlay(Position origin, Position destination)
        {
            executeMove(origin, destination);
            turn++;
            changePlayer();

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

        private void placePieces()
        {
            board.placePiece(new Rook(board, Color.White), new ChessPosition('c', 1).toPosition());
            board.placePiece(new Rook(board, Color.White), new ChessPosition('c', 2).toPosition());
            board.placePiece(new Rook(board, Color.White), new ChessPosition('d', 2).toPosition());
            board.placePiece(new Rook(board, Color.White), new ChessPosition('e', 2).toPosition());
            board.placePiece(new Rook(board, Color.White), new ChessPosition('e', 1).toPosition());
            board.placePiece(new King(board, Color.White), new ChessPosition('d', 1).toPosition());

            board.placePiece(new Rook(board, Color.Black), new ChessPosition('c', 8).toPosition());
            board.placePiece(new Rook(board, Color.Black), new ChessPosition('c', 7).toPosition());
            board.placePiece(new Rook(board, Color.Black), new ChessPosition('d', 7).toPosition());
            board.placePiece(new Rook(board, Color.Black), new ChessPosition('e', 7).toPosition());
            board.placePiece(new Rook(board, Color.Black), new ChessPosition('e', 8).toPosition());
            board.placePiece(new King(board, Color.Black), new ChessPosition('d', 8).toPosition());
        }
    }
}
