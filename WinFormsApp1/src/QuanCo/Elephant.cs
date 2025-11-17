using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.src.QuanCo
{
    // ==================== ELEPHANT (TUONG) ====================
    public class Elephant : BasePiece
    {
        public Elephant(PieceColor color, int x, int y) : base((color == PieceColor.Red ? "相" : "象"), color, x, y) { }

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
}
