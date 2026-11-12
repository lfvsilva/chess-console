using board;

namespace board
{
    internal class Piece
    {
        public Position position { get; set; }
        public Color color { get; set; }
        public int moveCount { get; protected set; }
        public Board board { get; protected set; }

        public Piece(Position position, Board board, Color color)
        {
            this.position = position;
            this.color = color;
            this.board = board;
            this.moveCount = 0;
        }
    }
}
