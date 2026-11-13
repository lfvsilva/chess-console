using board;

namespace chess
{
    internal class King : Piece
    {
        private ChessMatch chessMatch;


        public King(Board board, Color color, ChessMatch chessMatch) : base(board, color)
        {
            this.chessMatch = chessMatch;
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
        private bool testRookCastling(Position pos)
        {
            Piece p = board.getPiece(pos);
            return p != null && p is Rook && p.color == color && p.moveCount == 0;
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
            // #specialMove Castling
            if(moveCount == 0 && !chessMatch.check)
            {
                // #specialMove Castling Kingside Rook
                Position posT1 = new Position(position.row, position.column + 3);
                if (testRookCastling(posT1))
                {
                    Position p1 = new Position(position.row, position.column + 1);
                    Position p2 = new Position(position.row, position.column + 2);
                    if (board.getPiece(p1) == null && board.getPiece(p2) == null)
                    {
                        mat[position.row, position.column + 2] = true;
                    }
                }
                // #specialMove Castling Queenside Rook
                Position posT2 = new Position(position.row, position.column - 4);
                if (testRookCastling(posT2))
                {
                    Position p1 = new Position(position.row, position.column - 1);
                    Position p2 = new Position(position.row, position.column - 2);
                    Position p3 = new Position(position.row, position.column - 3);
                    if (board.getPiece(p1) == null && board.getPiece(p2) == null && board.getPiece(p3) == null)
                    {
                        mat[position.row, position.column - 2] = true;
                    }
                }
            }


            return mat;
        }
    }
}
