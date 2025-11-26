using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.src.QuanCo
{
    // ==================== HORSE (MA) ====================
    public class Horse : BasePiece
    {
        public Horse(PieceColor color, int x, int y) : base("馬", color, x, y, 600) { }
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

}
