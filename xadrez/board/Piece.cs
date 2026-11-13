using board;

namespace board
{
    abstract class Piece
    {
        public Position position { get; set; }
        public Color color { get; set; }
        public int moveCount { get; protected set; }
        public Board board { get; protected set; }

        public Piece(Board board, Color color)
        {
            this.position = null;
            this.color = color;
            this.board = board;
            this.moveCount = 0;
        }

        public void increaseMoveCount()
        {
            moveCount++;
        }

        public void decreaseMoveCount()
        {
            moveCount--;
        }

        public bool isThereAnyPossibleMove()
        {
            bool[,] mat = possibleMoves();
            for (int i = 0; i < board.rows; i++)
            {
                for (int j = 0; j < board.columns; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool possibleMove(Position pos)
        {
            return possibleMoves()[pos.row, pos.column];
        }

        public abstract bool[,] possibleMoves();
    }
}
