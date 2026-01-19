using System.Drawing.Drawing2D;
using WinFormsApp1.src.GUI;

namespace WinFormsApp1
{
    public partial class About : Form
    {
        private Image? _woodTexture = null;

        // --- KHAI BÁO BIẾN CHO SCROLL ---
        private Panel viewPortPanel = null!;
        private Panel scrollableContent = null!;

        // --- CẤU HÌNH FONT CHỮ (Đã đổi sang Font hỗ trợ Tiếng Việt tuyệt đối) ---
        // Times New Roman: Mang phong cách sách cổ, kiếm hiệp, hỗ trợ 100% tiếng Việt
        private Font fontMainTitle = new Font("Times New Roman", 28, FontStyle.Bold);
        private Font fontHeader = new Font("Times New Roman", 15, FontStyle.Bold);
        private Font fontBody = new Font("Times New Roman", 14, FontStyle.Regular);

        public About()
        {
            this.Text = "Bí Kíp Cờ Tướng";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.DoubleBuffered = true;

            // Load ảnh nền
            try
            {
                if (File.Exists("./Assets/giay.jpg")) _woodTexture = Image.FromFile("./Assets/giay.jpg");
                else if (File.Exists("./src/Images/giay.jpg")) _woodTexture = Image.FromFile("./src/Images/giay.jpg");
            }
            catch { }

            SetupResponsiveSize();
            SetupUI();

            // Đăng ký lăn chuột
            this.MouseWheel += About_MouseWheel;
        }

        private void SetupResponsiveSize()
        {
            int screenH = Screen.PrimaryScreen?.WorkingArea.Height ?? 768;
            int screenW = Screen.PrimaryScreen?.WorkingArea.Width ?? 1024;
            this.ClientSize = new Size((int)(screenW * 0.66), (int)(screenH * 0.85));
        }

        // --- SỬA LỖI BACKGROUND LẶP LẠI ---
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            if (_woodTexture != null)
            {
                g.DrawImage(_woodTexture, this.ClientRectangle);
            }
            else
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle,
                       Color.FromArgb(198, 147, 94), Color.FromArgb(100, 50, 20), 45f))
                {
                    g.FillRectangle(brush, this.ClientRectangle);
                }
            }
        }

        private void SetupUI()
        {
            // Nút Quay Lại
            RoundedButton btnBack = new RoundedButton();
            btnBack.Text = "Quay Lại";
            btnBack.Size = new Size(120, 45);
            btnBack.Location = new Point(20, 20);
            btnBack.Click += (s, e) => this.Close();
            this.Controls.Add(btnBack);

            // Tiêu đề lớn
            Label lblTitle = new Label();
            lblTitle.Text = "BÍ KÍP NHẬP MÔN";
            lblTitle.Font = fontMainTitle;
            lblTitle.ForeColor = Color.FromArgb(80, 20, 0); // Màu nâu đỏ đậm
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            this.Controls.Add(lblTitle);

            // Căn giữa tiêu đề
            lblTitle.Location = new Point((this.ClientSize.Width - lblTitle.PreferredWidth) / 2, 20);

            // --- KHUNG NHÌN CỐ ĐỊNH (VIEWPORT) ---
            viewPortPanel = new Panel();
            viewPortPanel.Location = new Point(50, 90);
            viewPortPanel.Size = new Size(this.ClientSize.Width - 100, this.ClientSize.Height - 110);
            // Để nền trong suốt để thấy background gỗ (hoặc chỉnh màu nhẹ)
            viewPortPanel.BackColor = Color.FromArgb(100, 255, 248, 220);
            viewPortPanel.AutoScroll = false;

            // Viền khung
            viewPortPanel.Paint += (s, e) => {
                ControlPaint.DrawBorder(e.Graphics, viewPortPanel.ClientRectangle,
                    Color.FromArgb(139, 69, 19), 3, ButtonBorderStyle.Solid,
                    Color.FromArgb(139, 69, 19), 3, ButtonBorderStyle.Solid,
                    Color.FromArgb(139, 69, 19), 3, ButtonBorderStyle.Solid,
                    Color.FromArgb(139, 69, 19), 3, ButtonBorderStyle.Solid);
            };
            this.Controls.Add(viewPortPanel);

            // --- NỘI DUNG CUỘN ---
            scrollableContent = new Panel();
            scrollableContent.AutoSize = true;
            scrollableContent.MaximumSize = new Size(viewPortPanel.Width, 0);
            scrollableContent.MinimumSize = new Size(viewPortPanel.Width, viewPortPanel.Height);
            scrollableContent.BackColor = Color.Transparent;
            scrollableContent.Location = new Point(0, 0);
            viewPortPanel.Controls.Add(scrollableContent);

            // --- NỘI DUNG ---
            int currentY = 20;

            currentY = AddSection("1. Thiên Mệnh",
                "Bàn cờ là chiến trường, quân cờ là binh tướng. Nhiệm vụ tối thượng của bạn là bắt sống (Chiếu bí) Tướng của đối phương. \n" +
                "Kẻ nào mất Tướng trước, kẻ đó bại trận!", currentY);

            currentY = AddSection("2. Binh Pháp ",
                "♚ TƯỚNG (Soái): 'Đầu não quân đội'. \n" +
                "Chỉ đi ngang/dọc 1 ô trong Cung Cấm. Tướng mà chết, vạn quân tan nát.\n" +
                "🛡️ SĨ (Cận vệ): 'Kẻ hộ giá'. \n" +
                "Đi chéo 1 ô, luôn túc trực bên Tướng trong Cung để bảo vệ.\n" +
                "🐘 TƯỢNG (Voi chiến): 'Lá chắn thép'. \n" +
                "Đi chéo 2 ô (hình chữ Điền). Voi to xác nên không thể qua sông, chỉ thủ ở nhà.\n" +
                "♜ XE (Chiến xa): 'Thần tốc'. \n" +
                "Đi ngang dọc bao nhiêu ô tùy thích. Gặp ai cản đường là húc bay! Đây là quân mạnh nhất.\n" +
                "💣 PHÁO (Thần công): 'Kẻ đánh lén'. \n" +
                "Đi như Xe, nhưng muốn ăn quân thì phải 'nhảy' qua đầu một quân khác (gọi là Ngòi).\n" +
                "♞ MÃ (Kỵ binh): 'Lả lướt'. \n" +
                "Đi hình chữ L (Nhật). Rất ảo diệu nhưng nếu bị ai đứng chặn ngay bên cạnh thì sẽ bị 'cản chân' không đi được.\n" +
                "♟️ TỐT (Lính): 'Cảm tử quân'. \n" +
                "Một đi không trở lại! Chưa qua sông chỉ đi thẳng. Qua sông rồi được đi ngang và thẳng. Chỉ được đi 1 ô."
                , currentY);

            currentY = AddSection("3. Luật Lệ Khác",
                "⚔️ Ăn quân: Di chuyển đè lên vị trí đối thủ đang đứng.\n" +
                "👀 Lộ mặt Tướng: Hai Tướng không được nhìn nhau trực diện trên 1 hàng dọc. Kẻ nào lộ mặt trước sẽ bại trận!\n" +
                "⚡ Chiếu tướng: Khi Tướng lâm nguy. Bạn bắt buộc phải lo cứu Tướng, không được làm việc khác."
                , currentY);

            Label spacer = new Label() { Text = "", Height = 80, Location = new Point(0, currentY) };
            scrollableContent.Controls.Add(spacer);
        }

        private void About_MouseWheel(object? sender, MouseEventArgs e)
        {
            if (scrollableContent.Height <= viewPortPanel.Height) return;
            int scrollSpeed = 40;
            int newY = scrollableContent.Location.Y + (e.Delta > 0 ? scrollSpeed : -scrollSpeed);
            int minY = viewPortPanel.Height - scrollableContent.Height;
            int maxY = 0;
            if (newY > maxY) newY = maxY;
            if (newY < minY) newY = minY;
            scrollableContent.Location = new Point(0, newY);
        }

        private int AddSection(string title, string content, int startY)
        {
            // Tiêu đề mục
            Label lblHeader = new Label();
            lblHeader.Text = title;
            lblHeader.Font = fontHeader;
            lblHeader.ForeColor = Color.DarkRed;
            lblHeader.AutoSize = true;
            lblHeader.Location = new Point(30, startY);
            lblHeader.MaximumSize = new Size(viewPortPanel.Width - 60, 0);
            scrollableContent.Controls.Add(lblHeader);

            startY += lblHeader.Height + 5;

            // Nội dung
            Label lblContent = new Label();
            lblContent.Text = content;
            lblContent.Font = fontBody;
            lblContent.ForeColor = Color.FromArgb(40, 20, 0);
            lblContent.AutoSize = true;
            lblContent.Location = new Point(30, startY);
            lblContent.MaximumSize = new Size(viewPortPanel.Width - 60, 0);
            scrollableContent.Controls.Add(lblContent);

            Label separator = new Label();
            separator.AutoSize = false;
            separator.Height = 2;
            separator.Width = viewPortPanel.Width - 100;
            separator.BackColor = Color.FromArgb(80, 139, 69, 19);
            separator.Location = new Point(50, startY + lblContent.Height + 15);
            scrollableContent.Controls.Add(separator);

            return startY + lblContent.Height + 35;
        }
    }
}