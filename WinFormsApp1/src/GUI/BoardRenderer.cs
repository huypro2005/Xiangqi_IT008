using System.Drawing.Drawing2D;
using WinFormsApp1.src; // Namespace chứa Board, Piece, GameEngine

namespace WinFormsApp1.src.GUI
{
    public class BoardRenderer
    {
        // Cấu hình màu sắc
        private Color colorBoardBg = Color.FromArgb(222, 184, 135);
        private Color colorLines = Color.FromArgb(120, 60, 10);
        private Color colorPieceWood = Color.FromArgb(245, 222, 179);
        private Color colorPieceBorder = Color.FromArgb(101, 67, 33);

        // Tài nguyên
        private Image? _woodTexture;

        public BoardRenderer(Image? woodTexture)
        {
            _woodTexture = woodTexture;
        }

        // Hàm vẽ chính (Được gọi từ Form1)
        public void DrawBoard(Graphics g, int width, int height, int cellSize, int boardMargin,
                              GameEngine engine, (int x, int y)? selectedPiecePos)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 1. Tính toán tọa độ căn giữa
            int gridWidth = 8 * cellSize;
            int gridHeight = 9 * cellSize;
            int innerPadding = 40;
            int bgWidth = gridWidth + (innerPadding * 2);
            int bgHeight = gridHeight + (innerPadding * 2);

            int startX = (width - bgWidth) / 2;
            int startY = (height - bgHeight) / 2;
            int gridOriginX = startX + innerPadding;
            int gridOriginY = startY + innerPadding;

            // 2. Vẽ nền gỗ
            DrawBackground(g, startX, startY, bgWidth, bgHeight);

            // 3. Vẽ lưới
            DrawGrid(g, gridOriginX, gridOriginY, cellSize);

            // 4. Vẽ quân cờ & Highlight
            DrawPiecesAndHighlights(g, engine, selectedPiecePos, gridOriginX, gridOriginY, cellSize);
        }

        private void DrawBackground(Graphics g, int x, int y, int w, int h)
        {
            Rectangle bgRect = new Rectangle(x, y, w, h);
            int cornerRadius = 30;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(bgRect.X, bgRect.Y, cornerRadius, cornerRadius, 180, 90);
                path.AddArc(bgRect.Right - cornerRadius, bgRect.Y, cornerRadius, cornerRadius, 270, 90);
                path.AddArc(bgRect.Right - cornerRadius, bgRect.Bottom - cornerRadius, cornerRadius, cornerRadius, 0, 90);
                path.AddArc(bgRect.X, bgRect.Bottom - cornerRadius, cornerRadius, cornerRadius, 90, 90);
                path.CloseFigure();

                g.SetClip(path);
                if (_woodTexture != null)
                {
                    using (TextureBrush tb = new TextureBrush(_woodTexture, WrapMode.Tile)) g.FillPath(tb, path);
                }
                else
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(bgRect, colorBoardBg, Color.FromArgb(205, 133, 63), 45f))
                        g.FillPath(brush, path);
                }
                g.DrawPath(new Pen(Color.FromArgb(80, 40, 0), 4), path);
                g.ResetClip();
            }
        }

        private void DrawGrid(Graphics g, int ox, int oy, int cs)
        {
            Pen p = new Pen(colorLines, 2);
            // Khung viền
            g.DrawRectangle(new Pen(colorLines, 3), ox - 4, oy - 4, 8 * cs + 8, 9 * cs + 8);

            // Ngang & Dọc
            for (int i = 0; i < 10; i++) g.DrawLine(p, ox, oy + i * cs, ox + 8 * cs, oy + i * cs);
            for (int i = 0; i < 9; i++)
            {
                g.DrawLine(p, ox + i * cs, oy, ox + i * cs, oy + 4 * cs);
                g.DrawLine(p, ox + i * cs, oy + 5 * cs, ox + i * cs, oy + 9 * cs);
            }
            // Sông & Cung tướng
            g.DrawLine(p, ox, oy + 4 * cs, ox, oy + 5 * cs);
            g.DrawLine(p, ox + 8 * cs, oy + 4 * cs, ox + 8 * cs, oy + 5 * cs);
            g.DrawLine(p, ox + 3 * cs, oy, ox + 5 * cs, oy + 2 * cs);
            g.DrawLine(p, ox + 5 * cs, oy, ox + 3 * cs, oy + 2 * cs);
            g.DrawLine(p, ox + 3 * cs, oy + 7 * cs, ox + 5 * cs, oy + 9 * cs);
            g.DrawLine(p, ox + 5 * cs, oy + 7 * cs, ox + 3 * cs, oy + 9 * cs);

            // Chữ Sở Hà Hán Giới
            Font f = new Font("Microsoft YaHei", (float)(cs * 0.4), FontStyle.Bold);
            try { f = new Font("Kaiti", (float)(cs * 0.4), FontStyle.Bold); } catch { }
            StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            float yRiver = oy + 4.5f * cs;
            g.DrawString("楚  河", f, new SolidBrush(Color.Red), ox + 2 * cs, yRiver, sf);
            g.DrawString("漢  界", f, new SolidBrush(Color.Red), ox + 6 * cs, yRiver, sf);

            DrawSpecialMarkers(g, ox, oy, cs);
        }

        private void DrawSpecialMarkers(Graphics g, int ox, int oy, int cs)
        {
            Pen p = new Pen(colorLines, 2);
            int s = 4;
            int[] cx = { 1, 7 }; int[] cy = { 2, 7 };
            int[] sx = { 0, 2, 4, 6, 8 }; int[] sy = { 3, 6 };

            Action<int, int> draw = (x, y) => {
                int px = ox + x * cs; int py = oy + y * cs;
                if (x > 0) { g.DrawLine(p, px - s, py - s - 10, px - s, py - s); g.DrawLine(p, px - s, py - s, px - s - 10, py - s); g.DrawLine(p, px - s, py + s, px - s - 10, py + s); g.DrawLine(p, px - s, py + s, px - s, py + s + 10); }
                if (x < 8) { g.DrawLine(p, px + s, py - s - 10, px + s, py - s); g.DrawLine(p, px + s, py - s, px + s + 10, py - s); g.DrawLine(p, px + s, py + s, px + s + 10, py + s); g.DrawLine(p, px + s, py + s, px + s, py + s + 10); }
            };
            foreach (int x in cx) foreach (int y in cy) draw(x, y);
            foreach (int x in sx) foreach (int y in sy) draw(x, y);
        }

        private void DrawPiecesAndHighlights(Graphics g, GameEngine engine, (int x, int y)? selected, int ox, int oy, int cs)
        {
            // Highlight
            if (selected.HasValue)
            {
                var (sx, sy) = selected.Value;
                BasePiece p = engine.board.GetPiece(sx, sy);
                if (p != null)
                {
                    var moves = p.GetValidMoves(engine.board);
                    foreach (var m in moves)
                    {
                        g.FillEllipse(new SolidBrush(Color.FromArgb(150, Color.Green)), ox + m.x * cs - 10, oy + m.y * cs - 10, 20, 20);
                    }
                    g.DrawEllipse(new Pen(Color.Blue, 3), ox + sx * cs - (cs / 2), oy + sy * cs - (cs / 2), cs, cs);
                }
            }

            // Draw Pieces
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 9; x++)
                {
                    BasePiece p = engine.board.grid[y, x];
                    if (p != null && p.IsAlive)
                    {
                        DrawOnePiece(g, p, ox + x * cs, oy + y * cs, cs);
                    }
                }
        }

        private void DrawOnePiece(Graphics g, BasePiece p, int px, int py, int cs)
        {
            int r = (cs / 2) - 4;
            int d = r * 2;
            int dx = px - r, dy = py - r;

            g.FillEllipse(new SolidBrush(Color.FromArgb(60, 0, 0, 0)), dx + 4, dy + 4, d, d);
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(dx, dy, d, d);
                using (PathGradientBrush pgb = new PathGradientBrush(path))
                {
                    pgb.CenterColor = Color.White;
                    pgb.SurroundColors = new Color[] { colorPieceWood };
                    g.FillEllipse(pgb, dx, dy, d, d);
                }
            }
            g.DrawEllipse(new Pen(colorPieceBorder, 2), dx, dy, d, d);
            g.DrawEllipse(new Pen(Color.FromArgb(100, colorPieceBorder), 1), dx + 4, dy + 4, d - 8, d - 8);

            Color txtColor = p.Color == PieceColor.Red ? Color.DarkRed : Color.Black;
            Font f = new Font("Microsoft YaHei", (float)(cs * 0.35), FontStyle.Bold);
            try { f = new Font("Kaiti", (float)(cs * 0.35), FontStyle.Bold); } catch { }
            StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString(p.Name, f, new SolidBrush(txtColor), px, py + 2, sf);
        }
    }
}