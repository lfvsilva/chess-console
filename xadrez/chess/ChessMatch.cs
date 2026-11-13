using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;
using board;

namespace chess
{
    internal class ChessMatch
    {
        public Board board { get; private set; }
        public int turn { get; private set; }
        public Color currentPlayer { get; private set; }

        public bool finished { get; private set; }
        private HashSet<Piece> pieces;
        private HashSet<Piece> capturedPieces;
        public bool check { get; private set; }
        public bool stalemate { get; private set; }
        public Piece vulnerableEnPassant { get; private set; }

        public ChessMatch()
        {
            board = new Board(8, 8);
            turn = 1;
            currentPlayer = Color.White;
            finished = false;
            pieces = new HashSet<Piece>();
            capturedPieces = new HashSet<Piece>();
            check = false;
            vulnerableEnPassant = null;

            placePieces();

        }

        public Piece executeMove(Position origin, Position destination)
        {
            Piece p = board.removePiece(origin);
            p.increaseMoveCount();
            Piece capturedPiece = board.removePiece(destination);
            p.position = destination;
            board.placePiece(p, destination);
            if (capturedPiece != null)
            {
                capturedPieces.Add(capturedPiece);
            }

            // # special move castling kingside rook
            if (p is King && destination.column == origin.column + 2)
            {
                Position originR = new Position(origin.row, origin.column + 3);
                Position destinationR = new Position(origin.row, origin.column + 1);
                Piece R = board.removePiece(originR);
                R.increaseMoveCount();
                board.placePiece(R, destinationR);
            }
            // # special move castling queenside rook
            if (p is King && destination.column == origin.column - 2)
            {
                Position originR = new Position(origin.row, origin.column - 4);
                Position destinationR = new Position(origin.row, origin.column - 1);
                Piece R = board.removePiece(originR);
                R.increaseMoveCount();
                board.placePiece(R, destinationR);
            }

            // # special move en passant
            if (p is Pawn)
            {
                if (origin.column != destination.column && capturedPiece == null)
                {
                    Position pawnPosition;
                    if (p.color == Color.White)
                    {
                        pawnPosition = new Position(destination.row + 1, destination.column);
                    }
                    else
                    {
                        pawnPosition = new Position(destination.row - 1, destination.column);
                    }
                    capturedPiece = board.removePiece(pawnPosition);
                    capturedPieces.Add(capturedPiece);
                }
            }
            return capturedPiece;
        }

        public void UndoMove(Position origin, Position destination, Piece capturedPiece)
        {
            Piece p = board.removePiece(destination);
            p.decreaseMoveCount();
            if (capturedPiece != null)
            {
                board.placePiece(capturedPiece, destination);
                capturedPieces.Remove(capturedPiece);
            }
            board.placePiece(p, origin);

            // # special move castling kingside rook
            if (p is King && destination.column == origin.column + 2)
            {
                Position originR = new Position(origin.row, origin.column + 3);
                Position destinationR = new Position(origin.row, origin.column + 1);
                Piece R = board.removePiece(destinationR);
                R.decreaseMoveCount();
                board.placePiece(R, originR);
            }
            // # special move castling queenside rook
            if (p is King && destination.column == origin.column - 2)
            {
                Position originR = new Position(origin.row, origin.column - 4);
                Position destinationR = new Position(origin.row, origin.column - 1);
                Piece R = board.removePiece(destinationR);
                R.decreaseMoveCount();
                board.placePiece(R, originR);
            }

            // # special move en passant
            if (p is Pawn)
            {
                if (origin.column != destination.column && capturedPiece == vulnerableEnPassant)
                {
                    Piece pawn = board.removePiece(destination);
                    Position pawnPosition;
                    if (p.color == Color.White)
                    {
                        pawnPosition = new Position(3, destination.column);
                    }
                    else
                    {
                        pawnPosition = new Position(4, destination.column);
                    }
                    board.placePiece(pawn, pawnPosition);
                }
            }
        }

        // 'promotionPiece' is only used when the move is a promotion:
        // Q = Queen (default), R = Rook, B = Bishop, N = Knight.
        public void ExecutePlay(Position origin, Position destination, char promotionPiece = 'Q')
        {
            bool isPromotion = isPromotionMove(origin, destination);
            promotionPiece = char.ToUpperInvariant(promotionPiece);
            // Validate the choice BEFORE moving anything, so an invalid choice
            // leaves the board untouched.
            if (isPromotion && !isValidPromotionChoice(promotionPiece))
            {
                throw new BoardException("Invalid promotion piece! Choose Q, R, B or N.");
            }

            Piece capturedPiece = executeMove(origin, destination);

            if (isInCheck(currentPlayer))
            {
                UndoMove(origin, destination, capturedPiece);
                throw new BoardException("You cannot put yourself in check!");
            }

            Piece p = board.getPiece(destination);

            // # special move promotion
            // Done before evaluating check/checkmate/stalemate, so they consider the
            // new piece (e.g. promoting to a knight may give check where a queen wouldn't).
            if (isPromotion)
            {
                Piece pawn = board.removePiece(destination);
                pieces.Remove(pawn);
                Piece promoted = createPromotedPiece(promotionPiece, pawn.color);
                // The promoted piece counts as already moved: a promoted rook
                // must never be allowed to castle.
                promoted.increaseMoveCount();
                board.placePiece(promoted, destination);
                pieces.Add(promoted);
                p = promoted;
            }
            // # special move en passant
            // Must be updated BEFORE evaluating the opponent's replies, so that an
            // en passant capture is considered when looking for checkmate/stalemate.
            if (p is Pawn && (destination.row == origin.row -2 || destination.row == origin.row + 2))
            {
                vulnerableEnPassant = p;
            }
            else
            {
                vulnerableEnPassant = null;
            }

            Color nextPlayer = opponent(currentPlayer);
            check = isInCheck(nextPlayer);
            if (!hasAnyLegalMove(nextPlayer))
            {
                // No legal moves: checkmate if in check, otherwise stalemate (draw).
                finished = true;
                stalemate = !check;
            }
            else
            {
                turn++;
                changePlayer();
            }
        }

        // True when moving the piece at 'origin' to 'destination' is a pawn reaching
        // the last rank. Lets the UI ask for the promotion piece before the move.
        public bool isPromotionMove(Position origin, Position destination)
        {
            Piece p = board.getPiece(origin);
            if (p is not Pawn)
            {
                return false;
            }
            return (p.color == Color.White && destination.row == 0)
                || (p.color == Color.Black && destination.row == board.rows - 1);
        }

        // A pawn may become a Queen, Rook, Bishop or Knight (never a King or a Pawn).
        public static bool isValidPromotionChoice(char piece)
        {
            return "QRBN".IndexOf(char.ToUpperInvariant(piece)) >= 0;
        }

        private Piece createPromotedPiece(char piece, Color color)
        {
            switch (char.ToUpperInvariant(piece))
            {
                case 'Q': return new Queen(board, color);
                case 'R': return new Rook(board, color);
                case 'B': return new Bishop(board, color);
                case 'N': return new Knight(board, color);
                default: throw new BoardException("Invalid promotion piece! Choose Q, R, B or N.");
            }
        }

        public void validateOriginPosition(Position pos)
        {
            board.validatePosition(pos);
            if (board.getPiece(pos) == null)
            {
                throw new BoardException("There is no piece in this position!");
            }
            if (currentPlayer != board.getPiece(pos).color)
            {
                throw new BoardException("The chosen piece is not yours!");
            }
            if (!anyTrue(legalMoves(pos)))
            {
                throw new BoardException("There are no possible moves for the chosen piece!");
            }
        }

        public void validateDestinationPosition(Position origin, Position destination)
        {
            board.validatePosition(destination);
            if (!legalMoves(origin)[destination.row, destination.column])
            {
                throw new BoardException("Invalid destination position!");
            }
        }

        private void changePlayer()
        {
            currentPlayer = (currentPlayer == Color.White) ? Color.Black : Color.White;
        }

        public HashSet<Piece> capturedPiecesByColor(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece x in capturedPieces)
            {
                if (x.color == color)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Piece> piecesInGameByColor(Color color)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece x in pieces)
            {
                if (x.color == color)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(capturedPiecesByColor(color));
            return aux;
        }

        public Color opponent(Color color)
        {
            return (color == Color.White) ? Color.Black : Color.White;
        }

        private Piece king(Color color)
        {
            foreach (Piece x in piecesInGameByColor(color))
            {
                if (x is King)
                {
                    return x;
                }
            }
            return null;
        }

        public bool isInCheck(Color color)
        {
            Piece K = king(color);
            if (K == null)
            {
                throw new BoardException($"There is no {color} king on the board!");
            }
            return isSquareAttacked(K.position, opponent(color));
        }

        // True if any piece of 'byColor' attacks 'pos'. Uses attackedSquares(), which
        // excludes castling (avoids infinite recursion) and includes empty pawn diagonals.
        public bool isSquareAttacked(Position pos, Color byColor)
        {
            foreach (Piece x in piecesInGameByColor(byColor))
            {
                if (x.attackedSquares()[pos.row, pos.column])
                {
                    return true;
                }
            }
            return false;
        }

        // Pseudo-legal moves of the piece at 'origin', minus those that would leave
        // its own king in check.
        public bool[,] legalMoves(Position origin)
        {
            Piece piece = board.getPiece(origin);
            bool[,] mat = piece.possibleMoves();
            for (int i = 0; i < board.rows; i++)
            {
                for (int j = 0; j < board.columns; j++)
                {
                    if (mat[i, j])
                    {
                        Position destination = new Position(i, j);
                        Piece capturedPiece = executeMove(origin, destination);
                        bool leavesKingInCheck = isInCheck(piece.color);
                        UndoMove(origin, destination, capturedPiece);
                        if (leavesKingInCheck)
                        {
                            mat[i, j] = false;
                        }
                    }
                }
            }
            return mat;
        }

        public bool hasAnyLegalMove(Color color)
        {
            foreach (Piece x in piecesInGameByColor(color))
            {
                if (anyTrue(legalMoves(x.position)))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool anyTrue(bool[,] mat)
        {
            foreach (bool b in mat)
            {
                if (b)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCheckMate(Color color)
        {
            return isInCheck(color) && !hasAnyLegalMove(color);
        }

        public bool isStalemate(Color color)
        {
            return !isInCheck(color) && !hasAnyLegalMove(color);
        }

        public void placeNewPiece(char column, int row, Piece piece)
        {
            board.placePiece(piece, new ChessPosition(column, row).toPosition());
            pieces.Add(piece);
        }

        private void placePieces()
        {
            placeNewPiece('a', 1, new Rook(board, Color.White));
            placeNewPiece('b', 1, new Knight(board, Color.White));
            placeNewPiece('c', 1, new Bishop(board, Color.White));
            placeNewPiece('d', 1, new Queen(board, Color.White));
            placeNewPiece('e', 1, new King(board, Color.White, this));
            placeNewPiece('f', 1, new Bishop(board, Color.White));
            placeNewPiece('g', 1, new Knight(board, Color.White));
            placeNewPiece('h', 1, new Rook(board, Color.White));
            placeNewPiece('a', 2, new Pawn(board, Color.White, this));
            placeNewPiece('b', 2, new Pawn(board, Color.White, this));
            placeNewPiece('c', 2, new Pawn(board, Color.White, this));
            placeNewPiece('d', 2, new Pawn(board, Color.White, this));
            placeNewPiece('e', 2, new Pawn(board, Color.White, this));
            placeNewPiece('f', 2, new Pawn(board, Color.White, this));
            placeNewPiece('g', 2, new Pawn(board, Color.White, this));
            placeNewPiece('h', 2, new Pawn(board, Color.White, this));

            placeNewPiece('a', 8, new Rook(board, Color.Black));
            placeNewPiece('b', 8, new Knight(board, Color.Black));
            placeNewPiece('c', 8, new Bishop(board, Color.Black));
            placeNewPiece('d', 8, new Queen(board, Color.Black));
            placeNewPiece('e', 8, new King(board, Color.Black, this));
            placeNewPiece('f', 8, new Bishop(board, Color.Black));
            placeNewPiece('g', 8, new Knight(board, Color.Black));
            placeNewPiece('h', 8, new Rook(board, Color.Black));
            placeNewPiece('a', 7, new Pawn(board, Color.Black, this));
            placeNewPiece('b', 7, new Pawn(board, Color.Black, this));
            placeNewPiece('c', 7, new Pawn(board, Color.Black, this));
            placeNewPiece('d', 7, new Pawn(board, Color.Black, this));
            placeNewPiece('e', 7, new Pawn(board, Color.Black, this));
            placeNewPiece('f', 7, new Pawn(board, Color.Black, this));
            placeNewPiece('g', 7, new Pawn(board, Color.Black, this));
            placeNewPiece('h', 7, new Pawn(board, Color.Black, this));

        }
    }
}
