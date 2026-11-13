using board;

namespace chess
{
    internal class Pawn : Piece
    {
        public Pawn(Board board, Color color) : base(board, color)
        {
        }

        public override string ToString()
        {
            return "P";
        }

        private bool existsEnemy(Position pos)
        {
            Piece p = board.getPiece(pos);
            return p != null && p.color != color;
        }
        private bool isFree(Position pos)
        {
            return board.getPiece(pos) == null;
        }

        public override bool[,] possibleMoves()
        {
            bool[,] mat = new bool[board.rows, board.columns];
            Position pos = new Position(0, 0);
            if (color == Color.White)
            {
                pos.setValues(position.row - 1, position.column);
                if (board.positionExists(pos) && isFree(pos))
                {
                    mat[pos.row, pos.column] = true;
                }
                pos.setValues(position.row - 2, position.column);
                if (board.positionExists(pos) && isFree(pos) && moveCount == 0)
                {
                    mat[pos.row, pos.column] = true;
                }
                pos.setValues(position.row - 1, position.column - 1);
                if (board.positionExists(pos) && existsEnemy(pos))
                {
                    mat[pos.row, pos.column] = true;
                }
                pos.setValues(position.row - 1, position.column + 1);
                if (board.positionExists(pos) && existsEnemy(pos))
                {
                    mat[pos.row, pos.column] = true;
                }
            }
            else
            {
                pos.setValues(position.row + 1, position.column);
                if (board.positionExists(pos) && isFree(pos))
                {
                    mat[pos.row, pos.column] = true;
                }
                pos.setValues(position.row + 2, position.column);
                if (board.positionExists(pos) && isFree(pos) && moveCount == 0)
                {
                    mat[pos.row, pos.column] = true;
                }
                pos.setValues(position.row + 1, position.column - 1);
                if (board.positionExists(pos) && existsEnemy(pos))
                {
                    mat[pos.row, pos.column] = true;
                }
                pos.setValues(position.row + 1, position.column + 1);
                if (board.positionExists(pos) && existsEnemy(pos))
                {
                    mat[pos.row, pos.column] = true;
                }
            }
            return mat;
        }
    }
}
