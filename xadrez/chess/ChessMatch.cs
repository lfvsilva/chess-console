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
        private HashSet<Piece> pieces;
        private HashSet<Piece> capturedPieces;

        public ChessMatch()
        {
            board = new Board(8, 8);
            turn = 1;
            currentPlayer = Color.White;
            finished = false;
            pieces = new HashSet<Piece>();
            capturedPieces = new HashSet<Piece>();

            placePieces();

        }

        public void executeMove(Position origin, Position destination)
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
