using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.src.QuanCo
{
    // ==================== ADVISOR (SI) ====================
    public class Advisor : BasePiece
    {
        public Advisor(PieceColor color, int x, int y) : base((color == PieceColor.Red? "仕" : "士"), color, x, y) { }

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
}
