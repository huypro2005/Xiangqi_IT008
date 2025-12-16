using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.src
{
    // ==================== BASE PIECE CLASS ====================
    public abstract class BasePiece
    {
        public string Name { get; set; }
        public PieceColor Color { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsAlive { get; set; }
        public int pointValue { get; set; } = 0;

        public BasePiece(string name, PieceColor color, int x, int y, int point)
        {
            Name = name;
            Color = color;
            X = x;
            Y = y;
            IsAlive = true;
            pointValue = point;
        }

        public abstract List<(int x, int y)> GetValidMoves(Board board);

        public abstract BasePiece Clone();
        public virtual int GetPointValue()
        {
            return pointValue;
        }
    }
}
