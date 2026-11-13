using board;

namespace chess
{
    internal class King : Piece
    {

        public King(Board board, Color color) : base(board, color)
        {
        }

        public override string ToString()
        {
            return "K";
        }

        private bool canMove(Position pos)
        {
            Piece p = board.getPiece(pos);
            return p == null || p.color != color;
        }

        public override bool[,] possibleMoves()
        {
            bool[,] mat = new bool[board.rows, board.columns];
            Position pos = new Position(0, 0);
            // above
            pos.setValues(position.row - 1, position.column);
            if (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
            }
            // Up-right
            pos.setValues(position.row - 1, position.column + 1);
            if (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
            }
            // right
            pos.setValues(position.row, position.column + 1);
            if (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
            }
            // Down-right
            pos.setValues(position.row + 1, position.column + 1);
            if (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
            }
            // below
            pos.setValues(position.row + 1, position.column);
            if (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
            }
            // Down-left
            pos.setValues(position.row + 1, position.column - 1);
            if (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
            }
            // left
            pos.setValues(position.row, position.column - 1);
            if (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
            }
            // Up-left
            pos.setValues(position.row - 1, position.column - 1);
            if (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
            }
                      

            return mat;
        }
    }
}
