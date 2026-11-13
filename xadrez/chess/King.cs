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
        // A king never attacks via castling, so castling is left out here.
        // This also prevents infinite recursion between the two kings.
        public override bool[,] attackedSquares()
        {
            return basicMoves();
        }

        public override bool[,] possibleMoves()
        {
            bool[,] mat = basicMoves();
            Color enemy = chessMatch.opponent(color);
            // #specialMove Castling
            // The king cannot castle out of check, nor through an attacked square.
            // (The destination square is checked by ChessMatch.legalMoves.)
            if (moveCount == 0 && !chessMatch.isSquareAttacked(position, enemy))
            {
                // #specialMove Castling Kingside Rook
                Position posT1 = new Position(position.row, position.column + 3);
                if (testRookCastling(posT1))
                {
                    Position p1 = new Position(position.row, position.column + 1);
                    Position p2 = new Position(position.row, position.column + 2);
                    if (board.getPiece(p1) == null && board.getPiece(p2) == null
                        && !chessMatch.isSquareAttacked(p1, enemy))
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
                    if (board.getPiece(p1) == null && board.getPiece(p2) == null && board.getPiece(p3) == null
                        && !chessMatch.isSquareAttacked(p1, enemy))
                    {
                        mat[position.row, position.column - 2] = true;
                    }
                }
            }
            return mat;
        }

        private bool[,] basicMoves()
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
