using System;
using System.Collections.Generic;
using System.Text;

namespace board
{
    internal class Board
    {
        public int rows { get; set; }
        public int columns { get; set; }
        private Piece[,] pieces;

        public Board(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            pieces = new Piece[rows, columns];
        }

        public Piece getPiece(int row, int column)
        {
            return pieces[row, column];
        }

        public Piece getPiece(Position position)
        {
            return pieces[position.row, position.column];
        }

        public bool thereIsAPiece(Position position)
        {
            validatePosition(position);
            return getPiece(position) != null;
        }

        public void placePiece(Piece piece, Position position)
        {

            if (thereIsAPiece(position))
            {
                throw new BoardException("There is already a piece in this position!");
            }


            pieces[position.row, position.column] = piece;
            piece.position = position;
        }

        public Piece removePiece(Position position)
        {
            if (getPiece(position) == null)
            {
                return null;
            }
            Piece aux = getPiece(position);
            aux.position = null;
            pieces[position.row, position.column] = null;
            return aux;
        }

        public bool positionExists(Position position)
        {
            if (position.row < 0 || position.row >= rows || position.column < 0 || position.column >= columns)
            {
                return false;
            }
            return true;
        }

        public bool validatePosition(Position position)
        {
            if (!positionExists(position))
            {
                throw new BoardException("Invalid position!");
            }
            return true;
        }
    }
}
