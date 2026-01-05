using WinFormsApp1.src;
using WinFormsApp1.src.GUI;
using WinFormsApp1.src.Utils;

namespace WinFormsApp1
{
    public partial class Home : Form
    {
        private GameMode hienTai = GameMode.NguoiVsNguoi;
        private AudioManagement _audioManager;

        public Home()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            this.DoubleBuffered = true;

            _audioManager = new AudioManagement("./Assets/Music.wav");
            _audioManager.Play();

            SetupResponsiveSize();
            CenterControls();

            labMode.Text = "2 Players";
        }

        private void SetupResponsiveSize()
        {
            int boardMargin = 45;
            int sidePanelWidth = 320;
            int formPadding = 20;

            // Lấy kích thước màn hình
            int screenH = Screen.PrimaryScreen?.WorkingArea.Height ?? 768;

            // Mục tiêu: Form cao khoảng 85% màn hình
            int targetHeight = (int)(screenH * 0.85);
            // Chiều cao khả dụng cho bàn cờ = Target - (Header/Padding trên dưới)
            int availableHeight = targetHeight - 40 - (2 * boardMargin);

            int cellSize = availableHeight / 10;

            // Giới hạn cellSize 
            if (cellSize < 50) cellSize = 50;
            if (cellSize > 90) cellSize = 90;

            int boardRealWidth = 9 * cellSize + 2 * boardMargin;
            int boardRealHeight = 10 * cellSize + 2 * boardMargin;

            int formWidth = formPadding + boardRealWidth + sidePanelWidth + formPadding;
            int formHeight = boardRealHeight + (2 * formPadding);

            // Áp dụng kích thước
            this.ClientSize = new Size(formWidth, formHeight);
        }

        private void CenterControls()
        {
            // Lấy trục giữa của Form
            int centerX = this.ClientSize.Width / 2;

            // Căn giữa Slogan (Tiêu đề)
            if (labSlogan != null)
            {
                labSlogan.Left = centerX - (labSlogan.Width / 2);
                // Đặt vị trí Y khoảng 20% từ trên xuống
                labSlogan.Top = (int)(this.ClientSize.Height * 0.2);
            }

            // Danh sách các nút cần căn giữa
            Control[] buttons = { labPlay, labMode, labStudent, labAbout, labExit };

            // Vị trí bắt đầu của nút đầu tiên (khoảng 40% từ trên xuống)
            int startY = (int)(this.ClientSize.Height * 0.4);
            int gap = 20; // Khoảng cách giữa các nút

            foreach (var btn in buttons)
            {
                if (btn != null)
                {
                    // Căn giữa theo chiều ngang
                    btn.Left = centerX - (btn.Width / 2);

                    // Sắp xếp dọc theo chiều cao
                    btn.Top = startY;

                    // Tăng Y cho nút tiếp theo
                    startY += btn.Height + gap;
                }
                if (btn == labMode)
                    btn.Left = centerX - (btn.Width / 2) - 20;
            }
        }

        private void labPlay_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 gameForm = new Form1(hienTai);
            gameForm.FormClosed += (s, args) =>
            {
                this.Show();
                this.CenterToScreen();
            };
            gameForm.Show();
        }

        private void labExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void labMode_Click(object sender, EventArgs e)
        {
            if (hienTai == GameMode.NguoiVsNguoi)
            {
                hienTai = GameMode.NguoiVsMay;
                labMode.Text = "With Computer";
                labMode.Left = ClientSize.Width / 2 - (labMode.Width / 2);
            }
            else
            {
                hienTai = GameMode.NguoiVsNguoi;
                labMode.Text = "2 Player Offline";
                labMode.Left = ClientSize.Width / 2 - (labMode.Width / 2);
            }
        }

        private void labAbout_Click(object sender, EventArgs e)
        {
            this.Hide();
            About aboutForm = new About();

            aboutForm.FormClosed += (s, args) =>
            {
                this.Show();
                this.CenterToScreen();
            };

            aboutForm.Show();
        }

        private void HoverEffect(object sender, bool isEnter)
        {
            if (sender is Label lbl)
            {
                lbl.ForeColor = isEnter ? Color.Black : Color.Snow;
                lbl.Cursor = Cursors.Hand;
            }
        }

        private void labSlogan_Click(object sender, EventArgs e) { }

        private void labPlay_MouseEnter(object sender, EventArgs e) => HoverEffect(sender, true);
        private void labPlay_MouseLeave(object sender, EventArgs e) => HoverEffect(sender, false);

        private void labMode_MouseEnter(object sender, EventArgs e) => HoverEffect(sender, true);
        private void labMode_MouseLeave(object sender, EventArgs e) => HoverEffect(sender, false);

        private void labStudent_MouseEnter(object sender, EventArgs e) => HoverEffect(sender, true);
        private void labStudent_MouseLeave(object sender, EventArgs e) => HoverEffect(sender, false);

        private void labAbout_MouseEnter(object sender, EventArgs e) => HoverEffect(sender, true);
        private void labAbout_MouseLeave(object sender, EventArgs e) => HoverEffect(sender, false);

        private void labExit_MouseEnter(object sender, EventArgs e) => HoverEffect(sender, true);
        private void labExit_MouseLeave(object sender, EventArgs e) => HoverEffect(sender, false);

        private void labAbout_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            About aboutForm = new About();

            aboutForm.FormClosed += (s, args) =>
            {
                this.Show();
                this.CenterToScreen();
            };

            aboutForm.Show();
        }
    }
}
