using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.src.QuanCo
{
    // ==================== SOLDIER (TOT) ====================
    public class Soldier : BasePiece
    {
        public Soldier(PieceColor color, int x, int y) : base((color == PieceColor.Red ? "兵" : "卒"), color, x, y, 300) { }
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
        public override int GetPointValue()
        {
            if (Color == PieceColor.Red && 9 - Y < 5)
            {
                return pointValue + 50;
            }
            return pointValue;
        }
    }

}
