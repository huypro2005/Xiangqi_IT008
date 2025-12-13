using System.Drawing.Drawing2D;
using WinFormsApp1.src;
using WinFormsApp1.src.AI_Play;
using WinFormsApp1.src.Utils;
using WinFormsApp1.src.GUI;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // LOGIC
        private GameMode _cheDoChoi;
        private GameEngine gameEngine;
        private List<BasePiece> _allPieces;
        private (int x, int y)? selectedPiecePos;
        private AI_Run _ai;
        private PieceColor _aiColor = PieceColor.Black;

        // UI & RENDERER
        private DoubleBufferedPanel boardPanel = null!;
        private Panel sidePanel = null!;
        private Label lblStatus = null!;
        private RoundedButton btnMusic = null!;
        private Label lblP1Name = null!, lblP2Name = null!;

        private BoardRenderer _renderer; // Đối tượng vẽ
        private int cellSize;
        private int boardMargin = 45;
        private Image? _woodTexture = null;
        private AudioManagement _audioManager;

        // COLORS
        private Color colorBgForm = Color.FromArgb(198, 147, 94);
        private Color colorPieceWood = Color.FromArgb(245, 222, 179);
        private Color colorPieceBorder = Color.FromArgb(101, 67, 33);

        public Form1(GameMode mode)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            _cheDoChoi = mode;

            gameEngine = new GameEngine();
            selectedPiecePos = null;
            _ai = new AI_Run(_aiColor);
            _allPieces = new List<BasePiece>();

            try { if (File.Exists("./src/Images/giay.jpg")) _woodTexture = Image.FromFile("./src/Images/giay.jpg"); } catch { }

            // Khởi tạo Renderer và Audio
            _renderer = new BoardRenderer(_woodTexture);
            _audioManager = new AudioManagement("./src/Images/Music.wav");
            _audioManager.Play();

            CalculateResponsiveSize();
            SetupCustomInterface();
            StartNewGame();
        }

        private void Form1_Load(object? sender, EventArgs e) { }

        // --- GAME LOGIC ---
        private void StartNewGame()
        {
            gameEngine = new GameEngine();
            selectedPiecePos = null;

            if (_cheDoChoi == GameMode.NguoiVsMay)
            {
                var res = MessageBox.Show("Bạn muốn đi trước (Cầm Đỏ) không?", "Chọn bên", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                _aiColor = (res == DialogResult.Yes) ? PieceColor.Black : PieceColor.Red;
                _ai = new AI_Run(_aiColor);
            }

            _allPieces.Clear();
            for (int y = 0; y < 10; y++)
                for (int x = 0; x < 9; x++)
                {
                    var p = gameEngine.board.grid[y, x];
                    if (p != null) _allPieces.Add(p);
                }

            UpdatePlayerLabels();
            UpdateStatus();
            boardPanel?.Invalidate();
            sidePanel?.Invalidate();

            if (_cheDoChoi == GameMode.NguoiVsMay && gameEngine.CurrentTurn == _aiColor)
                MakeAIMove();
        }

        private void UpdatePlayerLabels()
        {
            if (lblP1Name == null || lblP2Name == null) return;
            if (_cheDoChoi == GameMode.NguoiVsNguoi)
            {
                lblP1Name.Text = "Người chơi 1 (Đỏ)"; lblP2Name.Text = "Người chơi 2 (Đen)";
            }
            else
            {
                lblP1Name.Text = _aiColor == PieceColor.Black ? "Người chơi (Đỏ)" : "Máy tính (Đỏ)";
                lblP2Name.Text = _aiColor == PieceColor.Black ? "Máy tính (Đen)" : "Người chơi (Đen)";
            }
        }

        private void CalculateResponsiveSize()
        {
            int screenH = Screen.PrimaryScreen?.WorkingArea.Height ?? 768;
            int availableHeight = (int)(screenH * 0.85) - 40 - (2 * boardMargin);
            cellSize = availableHeight / 10;
            if (cellSize < 50) cellSize = 50; if (cellSize > 90) cellSize = 90;
        }

        // --- UI & DRAWING ---
        private void BoardPanel_Paint(object? sender, PaintEventArgs e)
        {
            // GỌI RENDERER ĐỂ VẼ
            _renderer.DrawBoard(e.Graphics, boardPanel.Width, boardPanel.Height,
                                cellSize, boardMargin, gameEngine, selectedPiecePos);

            if (gameEngine.State == GameState.Checkmate)
            {
                string w = gameEngine.CurrentTurn == PieceColor.Red ? "ĐEN" : "ĐỎ";
                MessageBox.Show($"Chiếu bí! {w} thắng!", "Kết thúc", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BoardPanel_MouseClick(object? sender, MouseEventArgs e)
        {
            if (_cheDoChoi == GameMode.NguoiVsMay && gameEngine.CurrentTurn == _aiColor) return;

            // Tính toán lại tọa độ click (Do Renderer căn giữa)
            int gw = 8 * cellSize, gh = 9 * cellSize, pad = 40;
            int sx = (boardPanel.Width - (gw + 2 * pad)) / 2, sy = (boardPanel.Height - (gh + 2 * pad)) / 2;
            int gox = sx + pad, goy = sy + pad;

            int gx = (e.X - gox + cellSize / 2) / cellSize;
            int gy = (e.Y - goy + cellSize / 2) / cellSize;

            if (!gameEngine.board.IsWithinBounds(gx, gy)) return;

            if (!selectedPiecePos.HasValue)
            {
                var p = gameEngine.board.GetPiece(gx, gy);
                if (p != null && p.Color == gameEngine.CurrentTurn && p.IsAlive) { selectedPiecePos = (gx, gy); boardPanel.Invalidate(); }
            }
            else
            {
                var (fx, fy) = selectedPiecePos.Value;
                if (fx == gx && fy == gy) { selectedPiecePos = null; boardPanel.Invalidate(); return; }

                if (gameEngine.MakeMove(fx, fy, gx, gy))
                {
                    UpdateStatus(); selectedPiecePos = null; boardPanel.Invalidate(); sidePanel.Invalidate();
                    if (_cheDoChoi == GameMode.NguoiVsMay && gameEngine.State != GameState.Checkmate) MakeAIMove();
                }
                else
                {
                    var p = gameEngine.board.GetPiece(gx, gy);
                    if (p != null && p.Color == gameEngine.CurrentTurn) { selectedPiecePos = (gx, gy); boardPanel.Invalidate(); }
                }
            }
        }

        private async void MakeAIMove()
        {
            if (_cheDoChoi != GameMode.NguoiVsMay || gameEngine.CurrentTurn != _aiColor) return;
            lblStatus.Text = "Máy đang nghĩ...";
            await Task.Delay(50);
            GameEngine sim = gameEngine.Clone();
            var move = await Task.Run(() => _ai.FindBestMove(sim, depth: 4));
            if (move.HasValue)
            {
                gameEngine.MakeMove(move.Value.fromX, move.Value.fromY, move.Value.toX, move.Value.toY);
                UpdateStatus(); boardPanel.Invalidate(); sidePanel.Invalidate();
            }
            else MessageBox.Show("Máy đầu hàng!", "Thông báo");
        }

        private void UpdateStatus()
        {
            string turn = gameEngine.CurrentTurn == PieceColor.Red ? "ĐỎ" : "ĐEN";
            lblStatus.Text = $"Lượt: {turn}";
            lblStatus.ForeColor = gameEngine.CurrentTurn == PieceColor.Red ? Color.DarkRed : Color.Black;
            if (gameEngine.IsKingInCheck(gameEngine.CurrentTurn)) lblStatus.Text += " - CHIẾU!";
        }

        // --- SETUP GIAO DIỆN (Giữ nguyên phần này) ---
        private void SetupCustomInterface()
        {
            this.Text = "Cờ Tướng - Chinese Chess";
            this.BackColor = colorBgForm;
            this.Paint += (s, e) => {
                if (_woodTexture != null) using (TextureBrush tb = new TextureBrush(_woodTexture, WrapMode.Tile)) e.Graphics.FillRectangle(tb, ClientRectangle);
                else using (LinearGradientBrush b = new LinearGradientBrush(ClientRectangle, colorBgForm, Color.FromArgb(100, 50, 20), 45f)) e.Graphics.FillRectangle(b, ClientRectangle);
            };

            int bw = 9 * cellSize + 2 * boardMargin;
            int bh = 10 * cellSize + 2 * boardMargin;
            int sw = 320, pad = 20;
            this.ClientSize = new Size(pad + bw + sw + pad, bh + 2 * pad);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Controls.Clear();

            boardPanel = new DoubleBufferedPanel { Size = new Size(bw, bh), Location = new Point(pad, pad), BackColor = Color.Transparent };
            boardPanel.Paint += BoardPanel_Paint;
            boardPanel.MouseClick += BoardPanel_MouseClick;
            this.Controls.Add(boardPanel);

            sidePanel = new Panel { Size = new Size(sw, bh), Location = new Point(boardPanel.Right + 10, pad), BackColor = Color.Transparent };
            this.Controls.Add(sidePanel);

            btnMusic = new RoundedButton { Size = new Size(80, 30), Location = new Point(ClientSize.Width - 90, 5), Font = new Font("Segoe UI", 8, FontStyle.Bold), BackColor = Color.FromArgb(100, 0, 0, 0) };
            btnMusic.Text = _audioManager.IsPlaying ? "🔊 Bật" : "🔇 Tắt";
            btnMusic.Click += (s, e) => { btnMusic.Text = _audioManager.Toggle() ? "🔊 Bật" : "🔇 Tắt"; };
            this.Controls.Add(btnMusic); btnMusic.BringToFront();

            AddSidePanelControls();
        }

        private void AddSidePanelControls()
        {
            int sw = sidePanel.Width;
            Panel p1 = CreateFancyPlayerBox("Player 1", Color.DarkRed, out lblP1Name);
            p1.Location = new Point((sw - p1.Width) / 2, 0);
            sidePanel.Controls.Add(p1);

            lblStatus = new Label { Text = "Lượt: ĐỎ", Font = new Font("Segoe UI", 20, FontStyle.Bold), ForeColor = Color.DarkRed, AutoSize = false, TextAlign = ContentAlignment.MiddleCenter, Size = new Size(sw, 50), Location = new Point(0, 180) };
            sidePanel.Controls.Add(lblStatus);

            Panel p2 = CreateFancyPlayerBox("Player 2", Color.Black, out lblP2Name);
            p2.Location = new Point((sw - p2.Width) / 2, 250);
            sidePanel.Controls.Add(p2);

            int bw = 140, bh = 45, gap = 15, sx = (sw - bw) / 2, sy = 430;
            RoundedButton btnNew = new RoundedButton { Text = "Ván mới", Size = new Size(bw, bh), Location = new Point(sx, sy) };
            btnNew.Click += (s, e) => StartNewGame();
            sidePanel.Controls.Add(btnNew);

            RoundedButton btnUndo = new RoundedButton { Text = "Đi lại", Size = new Size(bw, bh), Location = new Point(sx, sy + bh + gap) };
            btnUndo.Click += (s, e) => {
                // 1. Chặn bấm khi Máy đang suy nghĩ (tránh lỗi crash)
                if (_cheDoChoi == GameMode.NguoiVsMay && gameEngine.CurrentTurn == _aiColor) return;

                bool needUpdate = false;

                if (_cheDoChoi == GameMode.NguoiVsNguoi)
                {
                    // Chế độ PvP: Chỉ cần lui 1 nước như bình thường
                    if (gameEngine.UndoMove()) needUpdate = true;
                }
                else
                {
                    // Chế độ PvE: Phải lui 2 nước (Máy + Người) để về lại lượt người chơi
                    // Bước 1: Lui nước của Máy
                    if (gameEngine.UndoMove())
                    {
                        needUpdate = true;

                        // Bước 2: Lui tiếp nước của Người chơi (để mình được đi lại)
                        // Chỉ lui tiếp nếu vẫn chưa hết lịch sử (đề phòng trường hợp mới vào game Máy đi trước)
                        gameEngine.UndoMove();
                    }
                }

                if (needUpdate)
                {
                    selectedPiecePos = null;
                    boardPanel.Invalidate();
                    sidePanel.Invalidate();
                    UpdateStatus();

                    // Nếu sau khi Undo mà lại rơi vào lượt của Máy (Trường hợp Máy đi tiên, và ta Undo về đầu game)
                    // Thì bắt buộc phải gọi Máy đi lại ngay, nếu không sẽ bị treo.
                    if (_cheDoChoi == GameMode.NguoiVsMay && gameEngine.CurrentTurn == _aiColor)
                    {
                        MakeAIMove();
                    }
                }
            };
            sidePanel.Controls.Add(btnUndo);

            RoundedButton btnExit = new RoundedButton { Text = "Thoát", Size = new Size(bw, bh), Location = new Point(sx, sy + 2 * (bh + gap)) };
            btnExit.Click += (s, e) => Close();
            sidePanel.Controls.Add(btnExit);
        }

        private Panel CreateFancyPlayerBox(string defaultName, Color pColor, out Label nameLabel)
        {
            Panel p = new Panel { Size = new Size(200, 160), BackColor = Color.Transparent };
            PieceColor capType = (pColor == Color.DarkRed) ? PieceColor.Black : PieceColor.Red;

            p.Paint += (s, e) => {
                Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, p.Width - 1, p.Height - 1);
                using (GraphicsPath path = new GraphicsPath())
                {
                    int rad = 10;
                    path.AddArc(r.X, r.Y, rad, rad, 180, 90); path.AddArc(r.Right - rad, r.Y, rad, rad, 270, 90);
                    path.AddArc(r.Right - rad, r.Bottom - rad, rad, rad, 0, 90); path.AddArc(r.X, r.Bottom - rad, rad, rad, 90, 90); path.CloseFigure();
                    using (SolidBrush b = new SolidBrush(Color.FromArgb(220, 250, 240, 200))) g.FillPath(b, path);
                    using (Pen pen = new Pen(Color.FromArgb(139, 69, 19), 2)) g.DrawPath(pen, path);
                }
                var caps = _allPieces.Where(x => x.Color == capType && !x.IsAlive).ToList();
                int ms = 24, sx = 15, sy = 85, gp = 4, c = 0;
                g.DrawString("Đã ăn:", new Font("Segoe UI", 9, FontStyle.Italic), Brushes.DimGray, 15, 65);
                foreach (var pc in caps)
                {
                    Rectangle mr = new Rectangle(sx + (c * (ms + gp)), sy, ms, ms);
                    g.FillEllipse(Brushes.White, mr); g.DrawEllipse(Pens.Brown, mr);
                    Color tc = pc.Color == PieceColor.Red ? Color.DarkRed : Color.Black;
                    Font f = new Font("Microsoft YaHei", 10, FontStyle.Bold); try { f = new Font("Kaiti", 10, FontStyle.Bold); } catch { }
                    StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(pc.Name, f, new SolidBrush(tc), mr.X + ms / 2, mr.Y + ms / 2 + 2, sf);
                    c++; if (c >= 6) { c = 0; sy += ms + gp; }
                }
            };
            PictureBox avt = new PictureBox { Size = new Size(50, 50), Location = new Point(15, 15), BackColor = pColor };
            avt.Paint += (s, e) => ControlPaint.DrawBorder(e.Graphics, avt.ClientRectangle, Color.Goldenrod, 3, ButtonBorderStyle.Solid, Color.Goldenrod, 3, ButtonBorderStyle.Solid, Color.Goldenrod, 3, ButtonBorderStyle.Solid, Color.Goldenrod, 3, ButtonBorderStyle.Solid);
            p.Controls.Add(avt);
            Label lbl = new Label { Text = defaultName, Location = new Point(75, 20), Size = new Size(120, 40), Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(80, 40, 0) };
            p.Controls.Add(lbl);
            nameLabel = lbl;
            return p;
        }
    }
}