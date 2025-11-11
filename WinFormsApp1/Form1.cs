using WinFormsApp1.src;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private GameEngine gameEngine;
        private (int x, int y)? selectedPiecePos;
        private Panel boardPanel;
        private Label statusLabel;
        private const int cellSize = 60;
        private const int margin = 30;

        public Form1()
        {
            InitializeControls();
            gameEngine = new GameEngine();
            selectedPiecePos = null;
        }

        private void InitializeControls()
        {
            this.Text = "Cờ Tướng - Chinese Chess";
            this.Size = new Size(9 * cellSize + 2 * margin + 20, 10 * cellSize + 2 * margin + 100);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Board Panel
            boardPanel = new Panel();
            boardPanel.Location = new Point(0, 0);
            boardPanel.Size = new Size(9 * cellSize + 2 * margin, 10 * cellSize + 2 * margin);
            boardPanel.BackColor = Color.Wheat;
            boardPanel.Paint += BoardPanel_Paint;
            boardPanel.MouseClick += BoardPanel_MouseClick;
            this.Controls.Add(boardPanel);

            // Status Label
            statusLabel = new Label();
            statusLabel.Location = new Point(10, boardPanel.Height + 10);
            statusLabel.Size = new Size(boardPanel.Width - 20, 30);
            statusLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            statusLabel.Text = "Lượt: ĐỎ";
            statusLabel.ForeColor = Color.Red;
            this.Controls.Add(statusLabel);

            // New Game Button
            Button newGameBtn = new Button();
            newGameBtn.Text = "Ván mới";
            newGameBtn.Location = new Point(10, statusLabel.Bottom + 10);
            newGameBtn.Size = new Size(100, 30);
            newGameBtn.Click += (s, e) =>
            {
                gameEngine = new GameEngine();
                selectedPiecePos = null;
                boardPanel.Invalidate();
                UpdateStatus();
            };
            this.Controls.Add(newGameBtn);
        }

        private void BoardPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Draw board background
            g.FillRectangle(new SolidBrush(Color.BurlyWood), 0, 0, boardPanel.Width, boardPanel.Height);

            // Draw grid lines
            Pen linePen = new Pen(Color.Black, 2);

            // Horizontal lines
            for (int y = 0; y < 10; y++)
            {
                int py = margin + y * cellSize;
                g.DrawLine(linePen, margin, py, margin + 8 * cellSize, py);
            }

            // Vertical lines (with river gap)
            for (int x = 0; x < 9; x++)
            {
                int px = margin + x * cellSize;
                // Top half (0-4)
                g.DrawLine(linePen, px, margin, px, margin + 4 * cellSize);
                // Bottom half (5-9)
                g.DrawLine(linePen, px, margin + 5 * cellSize, px, margin + 9 * cellSize);
            }

            // Draw palaces
            Pen palacePen = new Pen(Color.Black, 2);
            // Black palace (top)
            g.DrawLine(palacePen, margin + 3 * cellSize, margin, margin + 5 * cellSize, margin + 2 * cellSize);
            g.DrawLine(palacePen, margin + 5 * cellSize, margin, margin + 3 * cellSize, margin + 2 * cellSize);
            // Red palace (bottom)
            g.DrawLine(palacePen, margin + 3 * cellSize, margin + 7 * cellSize, margin + 5 * cellSize, margin + 9 * cellSize);
            g.DrawLine(palacePen, margin + 5 * cellSize, margin + 7 * cellSize, margin + 3 * cellSize, margin + 9 * cellSize);

            // Draw river text
            Font riverFont = new Font("Arial", 16, FontStyle.Bold);
            g.DrawString("楚 河", riverFont, Brushes.DarkBlue, margin + cellSize, margin + 4 * cellSize + 10);
            g.DrawString("漢 界", riverFont, Brushes.DarkBlue, margin + 5 * cellSize, margin + 4 * cellSize + 10);

            // Draw valid moves if a piece is selected
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
                        g.FillEllipse(new SolidBrush(Color.FromArgb(100, Color.LightGreen)), px - 8, py - 8, 16, 16);
                    }

                    // Highlight selected piece
                    int hx = margin + sx * cellSize;
                    int hy = margin + sy * cellSize;
                    g.DrawEllipse(new Pen(Color.Yellow, 4), hx - 25, hy - 25, 50, 50);
                }
            }

            // Draw pieces
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    BasePiece piece = gameEngine.board.grid[y, x];
                    if (piece != null && piece.IsAlive)
                    {
                        int px = margin + x * cellSize;
                        int py = margin + y * cellSize;

                        // Draw piece circle
                        Color pieceColor = piece.Color == PieceColor.Red ? Color.FromArgb(255, 220, 180) : Color.FromArgb(240, 240, 240);
                        g.FillEllipse(new SolidBrush(pieceColor), px - 22, py - 22, 44, 44);
                        g.DrawEllipse(new Pen(Color.Black, 3), px - 22, py - 22, 44, 44);

                        // Draw piece text
                        Font pieceFont = new Font("Arial", 16, FontStyle.Bold);
                        Color textColor = piece.Color == PieceColor.Red ? Color.DarkRed : Color.Black;
                        SizeF textSize = g.MeasureString(piece.Name, pieceFont);
                        g.DrawString(piece.Name, pieceFont, new SolidBrush(textColor),
                            px - textSize.Width / 2, py - textSize.Height / 2);
                    }
                }
            }
        }

        private void BoardPanel_MouseClick(object sender, MouseEventArgs e)
        {
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
            }
            else
            {
                // Second click - move piece
                var (fromX, fromY) = selectedPiecePos.Value;
                bool success = gameEngine.MakeMove(fromX, fromY, gridX, gridY);

                if (success)
                {
                    UpdateStatus();

                    if (gameEngine.State == GameState.Checkmate)
                    {
                        PieceColor winner = gameEngine.CurrentTurn == PieceColor.Red ? PieceColor.Black : PieceColor.Red;
                        string winnerText = winner == PieceColor.Red ? "ĐỎ" : "ĐEN";
                        MessageBox.Show($"Chiếu bí! {winnerText} thắng!", "Kết thúc", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                selectedPiecePos = null;
            }

            boardPanel.Invalidate();
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
