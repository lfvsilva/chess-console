using board;

namespace chess
{
    internal class Bishop : Piece
    {

        public Bishop(Board board, Color color) : base(board, color)
        {
        }

        public override string ToString()
        {
            return "B";
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
            
            // Up-Right
            pos.setValues(position.row - 1, position.column + 1);
            while (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
                if (board.getPiece(pos) != null && board.getPiece(pos).color != color)
                {
                    break;
                }
                pos.column++;
                pos.row--;
            }
            // Down-Right
            pos.setValues(position.row + 1, position.column + 1);
            while (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
                if (board.getPiece(pos) != null && board.getPiece(pos).color != color)
                {
                    break;
                }
                pos.column++;
                pos.row++;
            }
            // Down-Left
            pos.setValues(position.row + 1, position.column - 1);
            while (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
                if (board.getPiece(pos) != null && board.getPiece(pos).color != color)
                {
                    break;
                }
                pos.row++;
                pos.column--;
            }
            // Up-Left
            pos.setValues(position.row - 1, position.column - 1);
            while (board.positionExists(pos) && canMove(pos))
            {
                mat[pos.row, pos.column] = true;
                if (board.getPiece(pos) != null && board.getPiece(pos).color != color)
                {
                    break;
                }
                pos.column--;
                pos.row--;
            }
            return mat;
        }
    }
}
