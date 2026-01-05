using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.src.QuanCo;

namespace WinFormsApp1.src
{
    // ==================== ENUMS ====================
    public enum PieceColor { Red, Black, None }
    public enum GameState { Playing, Checkmate, Stalemate }
    public enum PlayerType { Human, AI }
    public enum AIDifficulty { Easy, Medium, Hard }

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

        public int EvaluateBoard(PieceColor playerColor)
        {
            int score = 0;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    BasePiece piece = grid[y, x];
                    if (piece != null && piece.IsAlive)
                    {
                        score += (piece.Color == playerColor) ? piece.GetPointValue() : -piece.GetPointValue();
                    }
                }
            }
            return score;
        }

    }



    // ==================== Move Record ====================

    public class MoveRecord
    {
        public int FromX, FromY, ToX, ToY;
        public BasePiece CapturedPiece;
        public MoveRecord(int fromX, int fromY, int toX, int toY, BasePiece capturedPiece)
        {
            FromX = fromX;
            FromY = fromY;
            ToX = toX;
            ToY = toY;
            CapturedPiece = capturedPiece;
        }
    }

    // ==================== GAME ENGINE ====================
    public class GameEngine
    {
        public Board board;
        public PieceColor CurrentTurn;
        public GameState State;
        private Stack<MoveRecord> moveHistory;

        public GameEngine()
        {
            board = new Board();
            board.InitializePieces();
            CurrentTurn = PieceColor.Red;
            State = GameState.Playing;
            moveHistory = new Stack<MoveRecord>();
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
            // Không cho đi khi game đã kết thúc
            if (State != GameState.Playing)
                return false;

            // 1. Lấy quân xuất phát
            BasePiece piece = board.GetPiece(fromX, fromY);
            if (piece == null || piece.Color != CurrentTurn || !piece.IsAlive)
                return false;

            // 2. Kiểm tra nước đi có nằm trong list hợp lệ không
            var validMoves = piece.GetValidMoves(board);
            if (!validMoves.Contains((toX, toY)))
                return false;

            // 3. Backup quân bị ăn (nếu có) để dùng cho history + rollback
            BasePiece captured = board.GetPiece(toX, toY);

            // 4. Thực hiện TẠM nước đi trên board thật
            board.MovePiece(fromX, fromY, toX, toY);

            // 5. Nếu sau khi đi mà tướng của bên đang đi vẫn bị chiếu -> rollback, không cho đi
            if (IsKingInCheck(piece.Color))   // hoặc IsKingInCheck(CurrentTurn) vì chưa SwitchTurn
            {
                // Rollback thủ công
                board.grid[fromY, fromX] = piece;
                piece.X = fromX;
                piece.Y = fromY;

                board.grid[toY, toX] = captured;
                if (captured != null)
                    captured.IsAlive = true;

                return false;
            }

            // 6. Đến đây là nước đi HỢP LỆ -> log vào history (dùng captured backup)
            moveHistory.Push(new MoveRecord(fromX, fromY, toX, toY, captured));

            // 7. Đổi lượt
            SwitchTurn();

            // 8. Cập nhật trạng thái game
            if (IsCheckmate(CurrentTurn))
                State = GameState.Checkmate;

            return true;
        }

        // ==================== UNDO MOVE ====================
        public bool UndoMove()
        {
            if (moveHistory.Count == 0)
            {
                return false;
            }
            MoveRecord lastMove = moveHistory.Pop();
            BasePiece piece = board.GetPiece(lastMove.ToX, lastMove.ToY);
            board.grid[lastMove.FromY, lastMove.FromX] = piece;
            try
            {
                piece.X = lastMove.FromX;
                piece.Y = lastMove.FromY;
                //MessageBox.Show($"{piece.Name}");

            }
            catch 
            {
           
                return false;
                    
            }

            if (lastMove.CapturedPiece != null)
            {
                board.grid[lastMove.ToY, lastMove.ToX] = lastMove.CapturedPiece;
                lastMove.CapturedPiece.IsAlive = true;
            }
            else
            {
                board.grid[lastMove.ToY, lastMove.ToX] = null;
            }
            SwitchTurn();
            State = GameState.Playing;
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
   
        public GameEngine Clone()
        {
            GameEngine cloned = new GameEngine();
            cloned.board = this.board.Clone();
            cloned.CurrentTurn = this.CurrentTurn;
            cloned.State = this.State;
            return cloned;
        }
    }
    internal class logic
    {
    }
}
