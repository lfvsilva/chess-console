# ♟️ Chess (Console)

A two-player chess game that runs in the terminal, written in **C# / .NET 10**.

It started as a project from an Object-Oriented Programming course in C#. After finishing the course version, I extended it with AI-assisted development using [Claude Code](https://claude.com/claude-code). That work added rules and edge cases the course version didn't cover, plus automated checks of the move logic.

```
8 R N B Q K B N R
7 P P P P P P P P
6 - - - - - - - -
5 - - - - - - - -
4 - - - - - - - -
3 - - - - - - - -
2 P P P P P P P P
1 R N B Q K B N R
  a b c d e f g h

Captured pieces:
White: []
Black: []

Turn: 1
Waiting for player: White
```

## Features

**Moves for every piece**: King, Queen, Rook, Bishop, Knight and Pawn.

**Special moves**
- Castling, kingside and queenside
- En passant
- Pawn promotion, with your choice of Queen, Rook, Bishop or Knight

**Game rules**
- Only legal moves are accepted. You can't leave your own king in check, and a pinned piece can't move off its pin.
- The king can't castle out of check or through a square under attack.
- Check, checkmate and stalemate are detected.
- A promoted piece counts as already moved, so a promoted rook can never castle.

**Console interface**
- When you pick a piece, the squares it can legally move to are highlighted.
- Black pieces are drawn in yellow.
- Captured pieces are listed for each player.
- Invalid input such as `z9`, an empty line or `a10` shows an error message instead of crashing the game.

## Getting started

### Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run

```bash
git clone https://github.com/lfvsilva/chess-console.git
```

```bash
cd chess-console
```

```bash
dotnet run --project xadrez
```

Or open `xadrez.slnx` in Visual Studio and press **F5**.

## How to play

1. **Origin**: type the square of the piece you want to move, e.g. `e2`. Case doesn't matter.
2. The board is redrawn, with the possible destinations highlighted.
3. **Destination**: type the target square, e.g. `e4`.
4. If a pawn reaches the last rank, choose the promotion piece: `Q`, `R`, `B` or `N`. Pressing **Enter** keeps the Queen.

To castle, move the king two squares toward the rook (e.g. `e1` → `g1`), and the rook moves automatically.

| Letter | Piece  |
|:------:|--------|
| `K`    | King   |
| `Q`    | Queen  |
| `R`    | Rook   |
| `B`    | Bishop |
| `N`    | Knight |
| `P`    | Pawn   |
| `-`    | Empty square |

## Project structure

```
xadrez/
├── Program.cs           # Game loop: reads moves and handles errors
├── Screen.cs            # Console rendering and user input
├── board/               # Generic board layer (independent of chess rules)
│   ├── Board.cs         # 8x8 grid: place, remove and look up pieces
│   ├── Piece.cs         # Abstract base: possibleMoves() / attackedSquares()
│   ├── Position.cs      # Row/column coordinates (matrix-based)
│   ├── Color.cs
│   └── BoardException.cs
└── chess/               # Chess rules
    ├── ChessMatch.cs    # Turns, legal moves, check/checkmate/stalemate, special moves
    ├── ChessPosition.cs # Chess notation (e.g. "e2") ↔ matrix position, input parsing
    └── King.cs, Queen.cs, Rook.cs, Bishop.cs, Knight.cs, Pawn.cs
```

### Design notes

- **Layered design**: `board` knows nothing about chess. It only manages a grid of pieces. All chess logic lives in `chess`.
- **Polymorphism**: each piece overrides `possibleMoves()`. `ChessMatch.legalMoves()` filters those moves by simulating each one and undoing it if it leaves the king in check.
- **Attacked squares vs. moves**: `attackedSquares()` is separate from `possibleMoves()`. A pawn attacks its diagonals even when they're empty, and castling never counts as an attack. This is what makes the "can't castle through check" rule work, without infinite recursion between the two kings.

## Testing

The move generator was validated with **perft**, a standard technique for testing chess engines. Perft counts every legal move sequence from the starting position and compares the totals with known values:

| Depth | Positions |
|:-----:|----------:|
| 1     | 20        |
| 2     | 400       |
| 3     | 8,902     |
| 4     | 197,281   |

Scenario tests also covered input parsing, Fool's mate, Sam Loyd's 10-move stalemate, castling through an attacked square, pinned pieces, en passant, and promotion to each piece. These tests were run outside this repository. A unit test project is on the roadmap below.

## Roadmap

- [ ] Draw rules: 50-move rule, threefold repetition, insufficient material
- [ ] Move history in algebraic notation (`e4`, `Nf3`, `O-O`)
- [ ] Load and save games using FEN/PGN
- [ ] Unit test project (xUnit) with the perft and scenario tests
- [ ] Refactor names to C# conventions (PascalCase) and remove duplicated movement code

## Acknowledgements

- The base project comes from a Udemy course on Object-Oriented Programming in C#.
- The review, bug fixes and new rules were done with the help of [Claude Code](https://claude.com/claude-code).
