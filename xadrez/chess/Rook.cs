using board;

namespace chess
{
    internal class Rook : Piece
    {

        public Rook(Board board, Color color) : base(board, color)
        {
        }

        public override string ToString()
        {
            return "R";
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
            pos.setValues(this.position.row - 1, this.position.column);
            while (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
                if (board.getPiece(pos) != null && board.getPiece(pos).color != color)
                {
                    break;
                }
                pos.row--;
            }
            // right
            pos.setValues(this.position.row, this.position.column + 1);
            while (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
                if (board.getPiece(pos) != null && board.getPiece(pos).color != color)
                {
                    break;
                }
                pos.column++;
            }
            // below
            pos.setValues(this.position.row + 1, this.position.column);
            while (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
                if (board.getPiece(pos) != null && board.getPiece(pos).color != color)
                {
                    break;
                }
                pos.row++;
            }
            // left
            pos.setValues(this.position.row, this.position.column - 1);
            while (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
                if (board.getPiece(pos) != null && board.getPiece(pos).color != color)
                {
                    break;
                }
                pos.column--;
            }

            return mat;
        }
    }
}
