using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.src
{
    // ==================== ENUMS ====================
    public enum PieceColor { Red, Black, None }
    public enum GameState { Playing, Checkmate, Stalemate }

    // ==================== BOARD CLASS ====================
    public class Board
    {
        public BasePiece[,] grid; // 10 rows (y), 9 columns (x)

        public Board()
        {
            grid = new BasePiece[10, 9];
        }

        public void InitializePieces()
        {
            // Red pieces (bottom)
            grid[9, 0] = new Chariot(PieceColor.Red, 0, 9);
            grid[9, 1] = new Horse(PieceColor.Red, 1, 9);
            grid[9, 2] = new Elephant(PieceColor.Red, 2, 9);
            grid[9, 3] = new Advisor(PieceColor.Red, 3, 9);
            grid[9, 4] = new General(PieceColor.Red, 4, 9);
            grid[9, 5] = new Advisor(PieceColor.Red, 5, 9);
            grid[9, 6] = new Elephant(PieceColor.Red, 6, 9);
            grid[9, 7] = new Horse(PieceColor.Red, 7, 9);
            grid[9, 8] = new Chariot(PieceColor.Red, 8, 9);
            grid[7, 1] = new Cannon(PieceColor.Red, 1, 7);
            grid[7, 7] = new Cannon(PieceColor.Red, 7, 7);
            for (int x = 0; x < 9; x += 2)
            {
                grid[6, x] = new Soldier(PieceColor.Red, x, 6);
            }

            // Black pieces (top)
            grid[0, 0] = new Chariot(PieceColor.Black, 0, 0);
            grid[0, 1] = new Horse(PieceColor.Black, 1, 0);
            grid[0, 2] = new Elephant(PieceColor.Black, 2, 0);
            grid[0, 3] = new Advisor(PieceColor.Black, 3, 0);
            grid[0, 4] = new General(PieceColor.Black, 4, 0);
            grid[0, 5] = new Advisor(PieceColor.Black, 5, 0);
            grid[0, 6] = new Elephant(PieceColor.Black, 6, 0);
            grid[0, 7] = new Horse(PieceColor.Black, 7, 0);
            grid[0, 8] = new Chariot(PieceColor.Black, 8, 0);
            grid[2, 1] = new Cannon(PieceColor.Black, 1, 2);
            grid[2, 7] = new Cannon(PieceColor.Black, 7, 2);
            for (int x = 0; x < 9; x += 2)
            {
                grid[3, x] = new Soldier(PieceColor.Black, x, 3);
            }
        }

        public BasePiece GetPiece(int x, int y)
        {
            if (!IsWithinBounds(x, y)) return null;
            return grid[y, x];
        }

        public void MovePiece(int fromX, int fromY, int toX, int toY)
        {
            BasePiece piece = grid[fromY, fromX];
            BasePiece target = grid[toY, toX];

            if (target != null)
            {
                target.IsAlive = false;
            }

            grid[toY, toX] = piece;
            grid[fromY, fromX] = null;
            piece.X = toX;
            piece.Y = toY;
        }

        public bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x < 9 && y >= 0 && y < 10;
        }

        public Board Clone()
        {
            Board cloned = new Board();
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (grid[y, x] != null)
                    {
                        cloned.grid[y, x] = grid[y, x].Clone();
                    }
                }
            }
            return cloned;
        }
    }

    // ==================== BASE PIECE CLASS ====================
    public abstract class BasePiece
    {
        public string Name { get; set; }
        public PieceColor Color { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsAlive { get; set; }

        public BasePiece(string name, PieceColor color, int x, int y)
        {
            Name = name;
            Color = color;
            X = x;
            Y = y;
            IsAlive = true;
        }

        public abstract List<(int x, int y)> GetValidMoves(Board board);

        public abstract BasePiece Clone();
    }

    // ==================== CHARIOT (XE) ====================
    public class Chariot : BasePiece
    {
        public Chariot(PieceColor color, int x, int y) : base("Xe", color, x, y) { }

        public override List<(int x, int y)> GetValidMoves(Board board)
        {
            List<(int x, int y)> moves = new List<(int x, int y)>();
            int[] dx = { 0, 0, 1, -1 };
            int[] dy = { 1, -1, 0, 0 };

            for (int dir = 0; dir < 4; dir++)
            {
                for (int step = 1; step < 10; step++)
                {
                    int nx = X + dx[dir] * step;
                    int ny = Y + dy[dir] * step;

                    if (!board.IsWithinBounds(nx, ny)) break;

                    BasePiece target = board.GetPiece(nx, ny);
                    if (target == null)
                    {
                        moves.Add((nx, ny));
                    }
                    else
                    {
                        if (target.Color != this.Color)
                        {
                            moves.Add((nx, ny));
                        }
                        break;
                    }
                }
            }
            return moves;
        }

        public override BasePiece Clone()
        {
            return new Chariot(Color, X, Y) { IsAlive = IsAlive };
        }
    }

    // ==================== HORSE (MA) ====================
    public class Horse : BasePiece
    {
        public Horse(PieceColor color, int x, int y) : base("Mã", color, x, y) { }

        public override List<(int x, int y)> GetValidMoves(Board board)
        {
            List<(int x, int y)> moves = new List<(int x, int y)>();
            int[,] offsets = { { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 }, { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 } };
            int[,] blocks = { { 1, 0 }, { 1, 0 }, { -1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, { 0, 1 }, { 0, -1 } };

            for (int i = 0; i < 8; i++)
            {
                int blockX = X + blocks[i, 0];
                int blockY = Y + blocks[i, 1];

                if (board.IsWithinBounds(blockX, blockY) && board.GetPiece(blockX, blockY) != null)
                {
                    continue;
                }

                int nx = X + offsets[i, 0];
                int ny = Y + offsets[i, 1];

                if (board.IsWithinBounds(nx, ny))
                {
                    BasePiece target = board.GetPiece(nx, ny);
                    if (target == null || target.Color != this.Color)
                    {
                        moves.Add((nx, ny));
                    }
                }
            }
            return moves;
        }

        public override BasePiece Clone()
        {
            return new Horse(Color, X, Y) { IsAlive = IsAlive };
        }
    }

    // ==================== ELEPHANT (TUONG) ====================
    public class Elephant : BasePiece
    {
        public Elephant(PieceColor color, int x, int y) : base("Tượng", color, x, y) { }

        public override List<(int x, int y)> GetValidMoves(Board board)
        {
            List<(int x, int y)> moves = new List<(int x, int y)>();
            int[,] offsets = { { 2, 2 }, { 2, -2 }, { -2, 2 }, { -2, -2 } };
            int[,] blocks = { { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };

            for (int i = 0; i < 4; i++)
            {
                int blockX = X + blocks[i, 0];
                int blockY = Y + blocks[i, 1];

                if (board.IsWithinBounds(blockX, blockY) && board.GetPiece(blockX, blockY) != null)
                {
                    continue;
                }

                int nx = X + offsets[i, 0];
                int ny = Y + offsets[i, 1];

                // Cannot cross river
                if (Color == PieceColor.Red && ny < 5) continue;
                if (Color == PieceColor.Black && ny > 4) continue;

                if (board.IsWithinBounds(nx, ny))
                {
                    BasePiece target = board.GetPiece(nx, ny);
                    if (target == null || target.Color != this.Color)
                    {
                        moves.Add((nx, ny));
                    }
                }
            }
            return moves;
        }

        public override BasePiece Clone()
        {
            return new Elephant(Color, X, Y) { IsAlive = IsAlive };
        }
    }

    // ==================== ADVISOR (SI) ====================
    public class Advisor : BasePiece
    {
        public Advisor(PieceColor color, int x, int y) : base("Sĩ", color, x, y) { }

        public override List<(int x, int y)> GetValidMoves(Board board)
        {
            List<(int x, int y)> moves = new List<(int x, int y)>();
            int[,] offsets = { { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };

            for (int i = 0; i < 4; i++)
            {
                int nx = X + offsets[i, 0];
                int ny = Y + offsets[i, 1];

                // Must stay in palace
                if (nx < 3 || nx > 5) continue;
                if (Color == PieceColor.Red && (ny < 7 || ny > 9)) continue;
                if (Color == PieceColor.Black && (ny < 0 || ny > 2)) continue;

                if (board.IsWithinBounds(nx, ny))
                {
                    BasePiece target = board.GetPiece(nx, ny);
                    if (target == null || target.Color != this.Color)
                    {
                        moves.Add((nx, ny));
                    }
                }
            }
            return moves;
        }

        public override BasePiece Clone()
        {
            return new Advisor(Color, X, Y) { IsAlive = IsAlive };
        }
    }

    // ==================== GENERAL (TUONG) ====================
    public class General : BasePiece
    {
        public General(PieceColor color, int x, int y) : base("Tướng", color, x, y) { }

        public override List<(int x, int y)> GetValidMoves(Board board)
        {
            List<(int x, int y)> moves = new List<(int x, int y)>();
            int[] dx = { 0, 0, 1, -1 };
            int[] dy = { 1, -1, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                int nx = X + dx[i];
                int ny = Y + dy[i];

                // Must stay in palace
                if (nx < 3 || nx > 5) continue;
                if (Color == PieceColor.Red && (ny < 7 || ny > 9)) continue;
                if (Color == PieceColor.Black && (ny < 0 || ny > 2)) continue;

                if (board.IsWithinBounds(nx, ny))
                {
                    BasePiece target = board.GetPiece(nx, ny);
                    if (target == null || target.Color != this.Color)
                    {
                        moves.Add((nx, ny));
                    }
                }
            }
            return moves;
        }

        public override BasePiece Clone()
        {
            return new General(Color, X, Y) { IsAlive = IsAlive };
        }
    }

    // ==================== SOLDIER (TOT) ====================
    public class Soldier : BasePiece
    {
        public Soldier(PieceColor color, int x, int y) : base("Tốt", color, x, y) { }

        public override List<(int x, int y)> GetValidMoves(Board board)
        {
            List<(int x, int y)> moves = new List<(int x, int y)>();
            bool crossedRiver = (Color == PieceColor.Red && Y < 5) || (Color == PieceColor.Black && Y > 4);

            // Forward move
            int forwardY = Color == PieceColor.Red ? Y - 1 : Y + 1;
            if (board.IsWithinBounds(X, forwardY))
            {
                BasePiece target = board.GetPiece(X, forwardY);
                if (target == null || target.Color != this.Color)
                {
                    moves.Add((X, forwardY));
                }
            }

            // Sideways moves (only after crossing river)
            if (crossedRiver)
            {
                int[] dx = { 1, -1 };
                foreach (int d in dx)
                {
                    int nx = X + d;
                    if (board.IsWithinBounds(nx, Y))
                    {
                        BasePiece target = board.GetPiece(nx, Y);
                        if (target == null || target.Color != this.Color)
                        {
                            moves.Add((nx, Y));
                        }
                    }
                }
            }
            return moves;
        }

        public override BasePiece Clone()
        {
            return new Soldier(Color, X, Y) { IsAlive = IsAlive };
        }
    }

    // ==================== CANNON (PHAO) ====================
    public class Cannon : BasePiece
    {
        public Cannon(PieceColor color, int x, int y) : base("Pháo", color, x, y) { }

        public override List<(int x, int y)> GetValidMoves(Board board)
        {
            List<(int x, int y)> moves = new List<(int x, int y)>();
            int[] dx = { 0, 0, 1, -1 };
            int[] dy = { 1, -1, 0, 0 };

            for (int dir = 0; dir < 4; dir++)
            {
                bool foundScreen = false;
                for (int step = 1; step < 10; step++)
                {
                    int nx = X + dx[dir] * step;
                    int ny = Y + dy[dir] * step;

                    if (!board.IsWithinBounds(nx, ny)) break;

                    BasePiece target = board.GetPiece(nx, ny);

                    if (!foundScreen)
                    {
                        if (target == null)
                        {
                            moves.Add((nx, ny));
                        }
                        else
                        {
                            foundScreen = true;
                        }
                    }
                    else
                    {
                        if (target != null)
                        {
                            if (target.Color != this.Color)
                            {
                                moves.Add((nx, ny));
                            }
                            break;
                        }
                    }
                }
            }
            return moves;
        }

        public override BasePiece Clone()
        {
            return new Cannon(Color, X, Y) { IsAlive = IsAlive };
        }
    }

    // ==================== GAME ENGINE ====================
    public class GameEngine
    {
        public Board board;
        public PieceColor CurrentTurn;
        public GameState State;

        public GameEngine()
        {
            board = new Board();
            board.InitializePieces();
            CurrentTurn = PieceColor.Red;
            State = GameState.Playing;
        }

        public void SwitchTurn()
        {
            CurrentTurn = CurrentTurn == PieceColor.Red ? PieceColor.Black : PieceColor.Red;
        }

        public bool IsKingInCheck(PieceColor playerColor)
        {
            // Find the general
            General myGeneral = null;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (board.grid[y, x] is General g && g.Color == playerColor)
                    {
                        myGeneral = g;
                        break;
                    }
                }
                if (myGeneral != null) break;
            }

            if (myGeneral == null) return false;

            // Check flying generals rule
            General opponentGeneral = null;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (board.grid[y, x] is General g && g.Color != playerColor)
                    {
                        opponentGeneral = g;
                        break;
                    }
                }
                if (opponentGeneral != null) break;
            }

            if (opponentGeneral != null && myGeneral.X == opponentGeneral.X)
            {
                bool blocked = false;
                int minY = Math.Min(myGeneral.Y, opponentGeneral.Y);
                int maxY = Math.Max(myGeneral.Y, opponentGeneral.Y);
                for (int y = minY + 1; y < maxY; y++)
                {
                    if (board.GetPiece(myGeneral.X, y) != null)
                    {
                        blocked = true;
                        break;
                    }
                }
                if (!blocked) return true;
            }

            // Check all opponent pieces
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    BasePiece piece = board.grid[y, x];
                    if (piece != null && piece.Color != playerColor && piece.IsAlive)
                    {
                        var moves = piece.GetValidMoves(board);
                        if (moves.Contains((myGeneral.X, myGeneral.Y)))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool MakeMove(int fromX, int fromY, int toX, int toY)
        {
            BasePiece piece = board.GetPiece(fromX, fromY);

            if (piece == null || piece.Color != CurrentTurn || !piece.IsAlive)
            {
                return false;
            }

            var validMoves = piece.GetValidMoves(board);
            if (!validMoves.Contains((toX, toY)))
            {
                return false;
            }

            // Simulate move
            Board tempBoard = board.Clone();
            tempBoard.MovePiece(fromX, fromY, toX, toY);

            // Check if our king is in check after this move
            GameEngine tempEngine = new GameEngine();
            tempEngine.board = tempBoard;
            if (tempEngine.IsKingInCheck(CurrentTurn))
            {
                return false;
            }

            // Execute the move
            board.MovePiece(fromX, fromY, toX, toY);
            SwitchTurn();

            // Check game state
            if (IsCheckmate(CurrentTurn))
            {
                State = GameState.Checkmate;
            }

            return true;
        }

        public bool IsCheckmate(PieceColor playerColor)
        {
            if (!IsKingInCheck(playerColor))
            {
                return false;
            }

            // Try all possible moves
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    BasePiece piece = board.grid[y, x];
                    if (piece != null && piece.Color == playerColor && piece.IsAlive)
                    {
                        var moves = piece.GetValidMoves(board);
                        foreach (var (mx, my) in moves)
                        {
                            Board tempBoard = board.Clone();
                            tempBoard.MovePiece(x, y, mx, my);
                            GameEngine tempEngine = new GameEngine();
                            tempEngine.board = tempBoard;
                            if (!tempEngine.IsKingInCheck(playerColor))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
    internal class logic
    {
    }
}
