using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.src.Utils
{
    internal class DoubleBufferedPanel: Panel
    {
        public DoubleBufferedPanel()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;

            // Vẽ tất cả trong WM_PAINT + dùng buffer
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true
            );
            this.UpdateStyles();
        }

        // (tuỳ chọn) tránh xóa nền để đỡ nháy
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // base.OnPaintBackground(e); // comment đi: nền do chính OnPaint vẽ rồi
        }
    }
}
