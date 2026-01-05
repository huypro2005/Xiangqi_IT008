using WinFormsApp1.src;
using WinFormsApp1.src.GUI;
using WinFormsApp1.src.Utils;

namespace WinFormsApp1
{
    public partial class Home : Form
    {
        private GameMode hienTai = GameMode.NguoiVsNguoi;
        private AudioManagement _audioManager;

        // Dùng Label làm nút bấm, ContextMenu làm danh sách thả xuống
        private Label labLevel;
        private ContextMenuStrip levelMenu;
        private int selectedDepth = 2; // Mặc định Easy

        // Màu sắc chủ đạo (Đỏ đậm như trong ảnh)
        private Color themeRed = Color.FromArgb(192, 0, 0);
        private Font themeFont = new Font("Segoe UI", 12, FontStyle.Bold);

        public Home()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.DoubleBuffered = true;

            try
            {
                _audioManager = new AudioManagement("./Assets/Music.wav");
                _audioManager.Play();
            }
            catch { }

            SetupUI_Level(); // Tạo nút Level mới
            SetupResponsiveSize();
       

            // Mặc định vào là 2 người chơi -> Ẩn Level
            labMode.Text = "2 Players";
            labLevel.Visible = false;
            CenterControls();
        }

        // --- KHỞI TẠO NÚT LEVEL & MENU ---
        private void SetupUI_Level()
        {
            // 1. Tạo Nút hiển thị (Label giả làm Button)
            labLevel = new Label();
            labLevel.Text = "LEVEL: Easy ▼";
            labLevel.Font = themeFont;
            labLevel.BackColor = themeRed;
            labLevel.ForeColor = Color.White;
            labLevel.TextAlign = ContentAlignment.MiddleCenter;
            labLevel.AutoSize = false; // Tắt tự động co giãn để chỉnh kích thước khối
            labLevel.Size = new Size(150, 30); // Kích thước khối chữ nhật
            labLevel.Cursor = Cursors.Hand;
            labLevel.Visible = false; // Ẩn lúc đầu

            // 2. Tạo Menu thả xuống (ContextMenuStrip)
            levelMenu = new ContextMenuStrip();
            levelMenu.ShowImageMargin = false; // Bỏ lề chứa icon để menu gọn hơn
            levelMenu.BackColor = themeRed;    // Nền menu đỏ
            levelMenu.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            levelMenu.Renderer = new RedDropdownRenderer(); // Dùng bộ vẽ màu riêng (ở dưới cùng file)

            // Thêm các lựa chọn
            AddItemToMenu("LEVEL: Easy", 2);
            AddItemToMenu("LEVEL: Medium", 4);
            AddItemToMenu("LEVEL: Hard", 5);

            // 3. Sự kiện Click vào Label -> Hiện Menu
            labLevel.Click += (s, e) => {
                // Hiện menu ngay bên dưới nút
                levelMenu.Show(labLevel, 0, labLevel.Height);
            };

            this.Controls.Add(labLevel);
        }

        private void AddItemToMenu(string text, int depth)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(text);
            item.ForeColor = Color.Black; // Chữ đen trên nền đỏ (như ảnh)

            item.Click += (s, e) => {
                selectedDepth = depth;
                labLevel.Text = text + " ▼"; // Cập nhật chữ trên nút
            };
            levelMenu.Items.Add(item);
        }

        private void SetupResponsiveSize()
        {
            int boardMargin = 45;
            int sidePanelWidth = 320;
            int formPadding = 20;
            int screenH = Screen.PrimaryScreen?.WorkingArea.Height ?? 768;
            int targetHeight = (int)(screenH * 0.85);
            int availableHeight = targetHeight - 40 - (2 * boardMargin);
            int cellSize = availableHeight / 10;

            if (cellSize < 50) cellSize = 50;
            if (cellSize > 90) cellSize = 90;

            int boardRealWidth = 9 * cellSize + 2 * boardMargin;
            int boardRealHeight = 10 * cellSize + 2 * boardMargin;
            int formWidth = formPadding + boardRealWidth + sidePanelWidth + formPadding;
            int formHeight = boardRealHeight + (2 * formPadding);

            this.ClientSize = new Size(formWidth, formHeight);
        }

        private void CenterControls()
        {
            int centerX = this.ClientSize.Width / 2;

            if (labSlogan != null)
            {
                labSlogan.Left = centerX - (labSlogan.Width / 2);
                labSlogan.Top = (int)(this.ClientSize.Height * 0.2);
            }


            Control[] buttons = { labPlay, labMode, labAbout, labExit };

            int startY = (int)(this.ClientSize.Height * 0.4);
            int gap = 20;

            foreach (var btn in buttons)
            {
                if (btn != null)
                {
                    btn.Left = centerX - (btn.Width / 2);
                    btn.Top = startY;
                    startY += btn.Height + gap;
                }
            }

            // --- CĂN CHỈNH NÚT LEVEL ---
            if (labLevel != null && labMode != null)
            {
                // Đặt bên PHẢI của nút Mode, cách 10px
                labLevel.Left = labMode.Right + 15;
                // Căn giữa theo chiều dọc
                labLevel.Top = labMode.Top + (labMode.Height - labLevel.Height) / 2;

                // Chỉ hiện khi chơi với máy
                labLevel.Visible = (hienTai == GameMode.NguoiVsMay);
            }
        }

        private void labPlay_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 gameForm = new Form1(hienTai, selectedDepth);
            gameForm.FormClosed += (s, args) => {
                this.Show();
                this.CenterToScreen();
            };
            gameForm.Show();
        }

        private void labExit_Click(object sender, EventArgs e) => this.Close();

        private void labMode_Click(object sender, EventArgs e)
        {
            if (hienTai == GameMode.NguoiVsNguoi)
            {
                hienTai = GameMode.NguoiVsMay;
                labMode.Text = "With Computer";

                // Reset về Easy khi chuyển mode
                selectedDepth = 2;
                labLevel.Text = "LEVEL: Easy ▼";
                labLevel.Visible = true;
            }
            else
            {
                hienTai = GameMode.NguoiVsNguoi;
                labMode.Text = "2 Players";
                labLevel.Visible = false;
            }

            // Căn giữa lại labMode vì độ dài text thay đổi
            labMode.Left = (this.ClientSize.Width - labMode.Width) / 2;

            // Gọi lại CenterControls để nút Level bám theo nút Mode
            CenterControls();
        }

        private void labAbout_Click(object sender, EventArgs e)
        {
            this.Hide();
            About aboutForm = new About();
            aboutForm.FormClosed += (s, args) => {
                this.Show();
                this.CenterToScreen();
            };
            aboutForm.Show();
        }

        // --- HOVER EFFECT ---
        private void HoverEffect(object sender, bool isEnter)
        {
            if (sender is Label lbl && lbl != labLevel) // Không áp dụng cho nút Level mới (vì nó style riêng)
            {
                lbl.ForeColor = isEnter ? Color.Black : Color.Snow;
                lbl.Cursor = Cursors.Hand;
            }
        }

        // Sự kiện Hover cho nút Level (Làm sáng màu đỏ lên chút khi hover)
        // Bạn có thể thêm vào SetupUI_Level nếu muốn:
        // labLevel.MouseEnter += (s,e) => labLevel.BackColor = Color.Red; 
        // labLevel.MouseLeave += (s,e) => labLevel.BackColor = themeRed;

        private void labSlogan_Click(object sender, EventArgs e) { }
        private void labPlay_MouseEnter(object sender, EventArgs e) => HoverEffect(sender, true);
        private void labPlay_MouseLeave(object sender, EventArgs e) => HoverEffect(sender, false);
        private void labMode_MouseEnter(object sender, EventArgs e) => HoverEffect(sender, true);
        private void labMode_MouseLeave(object sender, EventArgs e) => HoverEffect(sender, false);
        private void labAbout_MouseEnter(object sender, EventArgs e) => HoverEffect(sender, true);
        private void labAbout_MouseLeave(object sender, EventArgs e) => HoverEffect(sender, false);
        private void labExit_MouseEnter(object sender, EventArgs e) => HoverEffect(sender, true);
        private void labExit_MouseLeave(object sender, EventArgs e) => HoverEffect(sender, false);
        private void labStudent_MouseEnter(object sender, EventArgs e) { }
        private void labStudent_MouseLeave(object sender, EventArgs e) { }
        private void labAbout_Click_1(object sender, EventArgs e) { labAbout_Click(sender, e); }
    }

    // --- CLASS TÙY CHỈNH MÀU SẮC MENU (RENDERER) ---
    // Class này giúp menu có màu đỏ đẹp thay vì màu trắng mặc định
    public class RedDropdownRenderer : ToolStripProfessionalRenderer
    {
        public RedDropdownRenderer() : base(new RedColors()) { }
    }

    public class RedColors : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(160, 0, 0); // Màu khi di chuột vào
        public override Color MenuItemBorder => Color.Transparent;
        public override Color MenuBorder => Color.Black;
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(160, 0, 0);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(160, 0, 0);
        public override Color ToolStripDropDownBackground => Color.FromArgb(192, 0, 0); // Màu nền menu
        public override Color ImageMarginGradientBegin => Color.FromArgb(192, 0, 0);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(192, 0, 0);
        public override Color ImageMarginGradientEnd => Color.FromArgb(192, 0, 0);
    }
}