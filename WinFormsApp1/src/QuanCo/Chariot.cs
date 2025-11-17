using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.src.QuanCo
{
    // ==================== CHARIOT (XE) ====================
    public class Chariot : BasePiece
    {
        public Chariot(PieceColor color, int x, int y) : base("車", color, x, y) { }

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

}
