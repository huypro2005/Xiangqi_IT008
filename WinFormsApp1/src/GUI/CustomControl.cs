using System.Drawing.Drawing2D;

namespace WinFormsApp1.src.GUI
{
    // Panel chống giật hình (Double Buffered)
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
        }
    }

    // Nút bấm bo tròn phong cách gỗ
    public class RoundedButton : Button
    {
        public RoundedButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = Color.Transparent;
            this.Cursor = Cursors.Hand;
            this.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.ForeColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            int radius = 15;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();

                // Nền Gradient giả gỗ
                using (LinearGradientBrush brush = new LinearGradientBrush(rect,
                        Color.FromArgb(160, 82, 45), Color.FromArgb(100, 50, 20), 90F))
                {
                    g.FillPath(brush, path);
                }

                // Viền sáng
                using (Pen pen = new Pen(Color.FromArgb(100, 255, 255, 255), 2))
                {
                    g.DrawPath(pen, path);
                }
            }

            // Vẽ Text
            TextRenderer.DrawText(g, this.Text, this.Font, rect, this.ForeColor,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}