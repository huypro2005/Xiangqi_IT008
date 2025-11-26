using WinFormsApp1.src;
using WinFormsApp1.src.AI_Play;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private GameMode _cheDoChoi;
        private GameEngine gameEngine;
        private (int x, int y)? selectedPiecePos;
        private Panel boardPanel;
        private Label statusLabel;
        private const int cellSize = 60;
        private const int margin = 30;
        private Image _woodTexture = null;
        private AI_Run _ai;
        private PieceColor _aiColor = PieceColor.Black;
        public Form1(GameMode mode)
        {
            InitializeControls();
            gameEngine = new GameEngine();
            selectedPiecePos = null;
            _woodTexture = Image.FromFile("./src/Images/giay.jpg");
            _cheDoChoi = mode;
            _ai = new AI_Run(_aiColor);
        }

        private void InitializeControls()
        {
            this.Text = "Cờ Tướng - Chinese Chess";
            this.Size = new Size(9 * cellSize + 2 * margin + 200, 10 * cellSize + 2 * margin + 100);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.BurlyWood;

            // Board Panel
            boardPanel = new Panel();
            boardPanel.Location = new Point(0, 0);
            boardPanel.Size = new Size(9 * cellSize + 2 * margin, 10 * cellSize + 2 * margin);
            boardPanel.Paint += BoardPanel_Paint;
            boardPanel.MouseClick += BoardPanel_MouseClick;
            this.Controls.Add(boardPanel);

            // Status Label
            statusLabel = new Label();
            statusLabel.Location = new Point(boardPanel.Width + 20, 10); 
            statusLabel.Size = new Size(this.ClientSize.Width - boardPanel.Width - 40, 30); 
            statusLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            statusLabel.Text = "Lượt: ĐỎ";
            statusLabel.ForeColor = Color.Red;
            this.Controls.Add(statusLabel);

            // New Game Button
            Button newGameBtn = new Button();
            newGameBtn.Text = "Ván mới";
            newGameBtn.Location = new Point(boardPanel.Width + 20, statusLabel.Bottom + 10);
            newGameBtn.Size = new Size(100, 30);
            newGameBtn.Click += (s, e) =>
            {
                gameEngine = new GameEngine();
                selectedPiecePos = null;
                boardPanel.Invalidate();
                UpdateStatus();
            };
            this.Controls.Add(newGameBtn);

            // Back to Home Button
            Button backHomeBtn = new Button();
            backHomeBtn.Text = "Back to Home";
            backHomeBtn.Location = new Point(boardPanel.Width + 20, newGameBtn.Bottom + 10);
            backHomeBtn.Size = new Size(100, 30);
            backHomeBtn.Click += (s, e) =>
            { 
                this.Close();
            };
            this.Controls.Add(backHomeBtn);

            // Back to Home Button
            Button backMove = new Button();
            backMove.Text = "Back move";
            backMove.Location = new Point(boardPanel.Width + 20, backHomeBtn.Bottom + 10);
            backMove.Size = new Size(100, 30);
            backMove.Click += (s, e) =>
            {
                if (gameEngine.UndoMove())
                {
                    selectedPiecePos = null;
                    boardPanel.Invalidate();
                    UpdateStatus();
                }
            };
            this.Controls.Add(backMove);
        }

        private void BackHomeBtn_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void backMove_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BoardPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Draw background
            Rectangle boardRect = new Rectangle(margin, margin, 8 * cellSize, 9 * cellSize);

            using (TextureBrush textureBrush = new TextureBrush(_woodTexture, System.Drawing.Drawing2D.WrapMode.Tile))
            {
                g.FillRectangle(textureBrush, boardRect);
            }

            // Draw grid
            Pen thinPen = new Pen(Color.Black, 1);
            Pen thickPen = new Pen(Color.Black, 3); 

            // Draw vertical line
            for (int y = 0; y < 10; y++)
            {
                int py = margin + y * cellSize;
                Pen currentPen = (y == 0 || y == 9) ? thickPen : thinPen;
                g.DrawLine(currentPen, margin, py, margin + 8 * cellSize, py);
            }

            // Draw horizontal line
            for (int x = 0; x < 9; x++)
            {
                int px = margin + x * cellSize;
                Pen currentPen = (x == 0 || x == 8) ? thickPen : thinPen;
                g.DrawLine(currentPen, px, margin, px, margin + 4 * cellSize);
                g.DrawLine(currentPen, px, margin + 5 * cellSize, px, margin + 9 * cellSize);
            }

            // Draw Palaces
            g.DrawLine(thinPen, margin + 3 * cellSize, margin, margin + 5 * cellSize, margin + 2 * cellSize);
            g.DrawLine(thinPen, margin + 5 * cellSize, margin, margin + 3 * cellSize, margin + 2 * cellSize);
            g.DrawLine(thinPen, margin + 3 * cellSize, margin + 7 * cellSize, margin + 5 * cellSize, margin + 9 * cellSize);
            g.DrawLine(thinPen, margin + 5 * cellSize, margin + 7 * cellSize, margin + 3 * cellSize, margin + 9 * cellSize);

            DrawSpecialMarkers(g);

            // Draw words "Sở Hà" (楚 河) and "Hán Giới" (漢 界) ---
            Font riverFont = new Font("MingLiU", 16, FontStyle.Bold); 
            StringFormat verticalFormat = new StringFormat { Alignment = StringAlignment.Center };

            // Position of "Sở Hà" (bên trái)
            float x1 = margin + 2 * cellSize + (cellSize / 2);
            float y1 = margin + 4 * cellSize + 10;
            g.DrawString("楚", riverFont, Brushes.Red, x1, y1, verticalFormat);
            g.DrawString("河", riverFont, Brushes.Red, x1, y1 + 25, verticalFormat); 

            // Position of "Hán Giới" (bên phải)
            float x2 = margin + 5 * cellSize + (cellSize / 2);
            g.DrawString("漢", riverFont, Brushes.Red, x2, y1, verticalFormat);
            g.DrawString("界", riverFont, Brushes.Red, x2, y1 + 25, verticalFormat);


            // Draw legal move
            if (selectedPiecePos.HasValue)
            {
                var (sx, sy) = selectedPiecePos.Value;
                BasePiece selectedPiece = gameEngine.board.GetPiece(sx, sy);
                if (selectedPiece != null)
                {
                    var validMoves = selectedPiece.GetValidMoves(gameEngine.board);
                    
                    foreach (var (mx, my) in validMoves)
                    {
                        int px = margin + mx * cellSize;
                        int py = margin + my * cellSize;
                        g.FillEllipse(new SolidBrush(Color.FromArgb(150, Color.Gold)), px - 8, py - 8, 16, 16);
                    }

                    // Highlight selected piece
                    int hx = margin + sx * cellSize;
                    int hy = margin + sy * cellSize;
                    g.DrawEllipse(new Pen(Color.FromArgb(200, Color.Gold), 4), hx - 24, hy - 24, 48, 48);
                }
            }

            // Draw piece
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    BasePiece piece = gameEngine.board.grid[y, x];
                    if (piece != null && piece.IsAlive)
                    {
                        DrawPiece3D(g, piece, x, y);
                    }
                }
            }
            if (gameEngine.State == GameState.Checkmate)
            {
                PieceColor winner = gameEngine.CurrentTurn == PieceColor.Red ? PieceColor.Black : PieceColor.Red;
                string winnerText = winner == PieceColor.Red ? "ĐỎ" : "ĐEN";
                MessageBox.Show($"Chiếu bí! {winnerText} thắng!", "Kết thúc", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// Draw piece with 3D animation (shadow, border)
        private void DrawPiece3D(Graphics g, BasePiece piece, int x, int y)
        {
            int px = margin + x * cellSize;
            int py = margin + y * cellSize;
            int radius = 22; 

            // 1. Xác định màu cơ bản
            Color pieceColor = piece.Color == PieceColor.Red ? Color.FromArgb(255, 220, 180) : Color.FromArgb(240, 240, 240);
            Color textColor = piece.Color == PieceColor.Red ? Color.DarkRed : Color.Black;

            // Draw piece using Gradient
            using (System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddEllipse(px - radius, py - radius, radius * 2, radius * 2);
                using (System.Drawing.Drawing2D.PathGradientBrush pgb = new System.Drawing.Drawing2D.PathGradientBrush(path))
                {
                    pgb.CenterColor = pieceColor;
                    Color edgeColor = piece.Color == PieceColor.Red ? Color.FromArgb(200, 170, 130) : Color.FromArgb(180, 180, 180);
                    pgb.SurroundColors = new Color[] { edgeColor };
                    pgb.FocusScales = new PointF(0.3f, 0.3f);

                    g.FillEllipse(pgb, px - radius, py - radius, radius * 2, radius * 2);
                }
            }

            // Draw border
            g.DrawEllipse(new Pen(Color.Black, 3), px - radius, py - radius, radius * 2, radius * 2);
            g.DrawEllipse(new Pen(Color.Gray, 1), px - radius + 3, py - radius + 3, (radius - 3) * 2, (radius - 3) * 2);

            // Draw piece's name
            Font pieceFont = new Font("MingLiU", 16, FontStyle.Bold);
            SizeF textSize = g.MeasureString(piece.Name, pieceFont);
            g.DrawString(piece.Name, pieceFont, new SolidBrush(textColor),
                px - textSize.Width / 2, py - textSize.Height / 2 + 1);
        }

        /// Draw marker "L" at position of Cannon and Chariot
        private void DrawSpecialMarkers(Graphics g)
        {
            Pen markerPen = new Pen(Color.FromArgb(150, Color.Black), 1);
            int size = 6; 
            int gap = 4; 

            int[] cannonX = { 1, 7 };
            int[] cannonY = { 2, 7 };
            int[] soldierX = { 0, 2, 4, 6, 8 };
            int[] soldierY = { 3, 6 };

            // 
            foreach (int x in cannonX)
                foreach (int y in cannonY)
                    DrawMarkerAt(g, markerPen, x, y, size, gap);

            // 
            foreach (int x in soldierX)
                foreach (int y in soldierY)
                    DrawMarkerAt(g, markerPen, x, y, size, gap);
        }

        /// 
        private void DrawMarkerAt(Graphics g, Pen pen, int x, int y, int size, int gap)
        {
            int px = margin + x * cellSize;
            int py = margin + y * cellSize;

            // Top-left
            g.DrawLine(pen, px - gap - size, py - gap, px - gap, py - gap);
            g.DrawLine(pen, px - gap, py - gap - size, px - gap, py - gap);
            // Top-right
            g.DrawLine(pen, px + gap, py - gap, px + gap + size, py - gap);
            g.DrawLine(pen, px + gap, py - gap - size, px + gap, py - gap);
            // Bottom-left
            g.DrawLine(pen, px - gap - size, py + gap, px - gap, py + gap);
            g.DrawLine(pen, px - gap, py + gap, px - gap, py + gap + size);
            // Bottom-right
            g.DrawLine(pen, px + gap, py + gap, px + gap + size, py + gap);
            g.DrawLine(pen, px + gap, py + gap, px + gap, py + gap + size);
        }

        private void BoardPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (_cheDoChoi == GameMode.NguoiVsMay && gameEngine.CurrentTurn == _aiColor)
            {
                return;
            }

            // Convert pixel to grid coordinates
            int gridX = (e.X - margin + cellSize / 2) / cellSize;
            int gridY = (e.Y - margin + cellSize / 2) / cellSize;

            if (!gameEngine.board.IsWithinBounds(gridX, gridY))
            {
                return;
            }

            if (!selectedPiecePos.HasValue)
            {
                // First click - select piece
                BasePiece piece = gameEngine.board.GetPiece(gridX, gridY);
                if (piece != null && piece.Color == gameEngine.CurrentTurn && piece.IsAlive)
                {
                    selectedPiecePos = (gridX, gridY);
                }
                else
                {
                    return;
                }
                boardPanel.Invalidate();
            }
            else
            {
                // Second click - move piece
                var (fromX, fromY) = selectedPiecePos.Value;
                if (fromX == gridX && fromY == gridY)
                {
                    // Deselect if clicked the same piece
                    selectedPiecePos = null;
                    boardPanel.Invalidate();
                    return;
                }
                bool success = gameEngine.MakeMove(fromX, fromY, gridX, gridY);

                if (success)
                {
                    UpdateStatus();
                }
                else
                {
                    return;
                }

                selectedPiecePos = null;
                boardPanel.Invalidate();
                if (_cheDoChoi == GameMode.NguoiVsMay && gameEngine.State == GameState.Playing)
                {
                    //MessageBox.Show("Đến lượt máy chơi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MakeAIMove();
                }
            }

            
        }

        private void MakeAIMove()
        {
            // Nếu hiện tại không phải lượt của máy thì thôi
            if (gameEngine.CurrentTurn != _aiColor)
                return;

            // Tìm nước đi tốt nhất cho máy (độ sâu 3–4 tuỳ bạn)
            var bestMove = _ai.FindBestMove(gameEngine, depth: 4);

                
            if (bestMove.HasValue)
            {
                var m = bestMove.Value;
                //string s = $"Máy đi từ ({m.fromX}, {m.fromY}) đến ({m.toX}, {m.toY})";
                //MessageBox.Show(s, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Thực hiện nước đi
                gameEngine.MakeMove(m.fromX, m.fromY, m.toX, m.toY);
                UpdateStatus();
                boardPanel.Invalidate();
            }
            else
            {
                // Không có nước hợp lệ -> coi như thua (trường hợp hiếm)
                MessageBox.Show("Máy không còn nước đi!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateStatus()
        {
            string turn = gameEngine.CurrentTurn == PieceColor.Red ? "ĐỎ" : "ĐEN";
            statusLabel.Text = $"Lượt: {turn}";
            statusLabel.ForeColor = gameEngine.CurrentTurn == PieceColor.Red ? Color.Red : Color.Black;

            if (gameEngine.IsKingInCheck(gameEngine.CurrentTurn))
            {
                statusLabel.Text += " - CHIẾU!";
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}