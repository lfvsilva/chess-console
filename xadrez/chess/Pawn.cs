using board;

namespace chess
{
    internal class Pawn : Piece
    {
        private ChessMatch match;
        public Pawn(Board board, Color color, ChessMatch match) : base(board, color)
        {
            this.match = match;
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

        // A pawn attacks both forward diagonals, even when they are empty
        // (needed to know if the king may castle through a square).
        public override bool[,] attackedSquares()
        {
            bool[,] mat = new bool[board.rows, board.columns];
            int direction = (color == Color.White) ? -1 : 1;
            foreach (int dc in new[] { -1, 1 })
            {
                Position pos = new Position(position.row + direction, position.column + dc);
                if (board.positionExists(pos))
                {
                    mat[pos.row, pos.column] = true;
                }
            }
            return mat;
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
                Position stepW = new Position(position.row - 1, position.column);
                if (board.positionExists(pos) && isFree(pos) && isFree(stepW) && moveCount == 0)
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

                // # special move en passant
                if (position.row == 3)
                {
                    Position left = new Position(position.row, position.column - 1);
                    if (board.positionExists(left) && existsEnemy(left) && board.getPiece(left) == match.vulnerableEnPassant)
                    {
                        mat[left.row - 1, left.column] = true;
                    }
                    Position right = new Position(position.row, position.column + 1);
                    if (board.positionExists(right) && existsEnemy(right) && board.getPiece(right) == match.vulnerableEnPassant)
                    {
                        mat[right.row - 1, right.column] = true;
                    }
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
                Position stepB = new Position(position.row + 1, position.column);
                if (board.positionExists(pos) && isFree(pos) && isFree(stepB) && moveCount == 0)
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
                // # special move en passant
                if (position.row == 4)
                {
                    Position left = new Position(position.row, position.column - 1);
                    if (board.positionExists(left) && existsEnemy(left) && board.getPiece(left) == match.vulnerableEnPassant)
                    {
                        mat[left.row + 1, left.column] = true;
                    }
                    Position right = new Position(position.row, position.column + 1);
                    if (board.positionExists(right) && existsEnemy(right) && board.getPiece(right) == match.vulnerableEnPassant)
                    {
                        mat[right.row + 1, right.column] = true;
                    }
                }
            }
            return mat;
        }
    }
}
