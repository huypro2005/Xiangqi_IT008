namespace WinFormsApp1
{
    partial class Home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            labPlay = new Label();
            label1 = new Label();
            labStudent = new Label();
            labAbout = new Label();
            labExit = new Label();
            labMode = new Label();
            labSlogan = new Label();
            SuspendLayout();
            // 
            // labPlay
            // 
            labPlay.AutoSize = true;
            labPlay.BackColor = Color.FromArgb(192, 0, 0);
            labPlay.Font = new Font("Tiger", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labPlay.ForeColor = Color.Snow;
            labPlay.Location = new Point(338, 239);
            labPlay.Name = "labPlay";
            labPlay.Size = new Size(74, 24);
            labPlay.TabIndex = 1;
            labPlay.Text = "Play";
            labPlay.Click += labPlay_Click;
            labPlay.MouseEnter += labPlay_MouseEnter;
            labPlay.MouseLeave += labPlay_MouseLeave;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(192, 0, 0);
            label1.Font = new Font("Tiger", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Snow;
            label1.Location = new Point(364, 280);
            label1.Name = "label1";
            label1.Size = new Size(0, 24);
            label1.TabIndex = 2;
            // 
            // labStudent
            // 
            labStudent.AutoSize = true;
            labStudent.BackColor = Color.FromArgb(192, 0, 0);
            labStudent.Font = new Font("Tiger", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labStudent.ForeColor = Color.Snow;
            labStudent.Location = new Point(338, 324);
            labStudent.Name = "labStudent";
            labStudent.Size = new Size(113, 24);
            labStudent.TabIndex = 3;
            labStudent.Text = "Student";
            // 
            // labAbout
            // 
            labAbout.AutoSize = true;
            labAbout.BackColor = Color.FromArgb(192, 0, 0);
            labAbout.Font = new Font("Tiger", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labAbout.ForeColor = Color.Snow;
            labAbout.Location = new Point(338, 363);
            labAbout.Name = "labAbout";
            labAbout.Size = new Size(87, 24);
            labAbout.TabIndex = 4;
            labAbout.Text = "About";
            // 
            // labExit
            // 
            labExit.AutoSize = true;
            labExit.BackColor = Color.FromArgb(192, 0, 0);
            labExit.Font = new Font("Tiger", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labExit.ForeColor = Color.Snow;
            labExit.Location = new Point(338, 404);
            labExit.Name = "labExit";
            labExit.Size = new Size(74, 24);
            labExit.TabIndex = 6;
            labExit.Text = "Exit";
            labExit.Click += labExit_Click;
            labExit.MouseEnter += labExit_MouseEnter;
            labExit.MouseLeave += labExit_MouseLeave;
            // 
            // labMode
            // 
            labMode.AutoSize = true;
            labMode.BackColor = Color.FromArgb(192, 0, 0);
            labMode.Font = new Font("Tiger", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labMode.ForeColor = Color.Snow;
            labMode.Location = new Point(338, 280);
            labMode.Name = "labMode";
            labMode.Size = new Size(74, 24);
            labMode.TabIndex = 7;
            labMode.Text = "Mode";
            labMode.Click += labMode_Click;
            // 
            // labSlogan
            // 
            labSlogan.AutoSize = true;
            labSlogan.BackColor = Color.Transparent;
            labSlogan.FlatStyle = FlatStyle.Flat;
            labSlogan.Font = new Font("Tiger", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labSlogan.ForeColor = Color.Snow;
            labSlogan.Location = new Point(148, 141);
            labSlogan.Name = "labSlogan";
            labSlogan.Size = new Size(496, 24);
            labSlogan.TabIndex = 8;
            labSlogan.Text = "Welcome to ANYUH Chinese Chess";
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(787, 561);
            Controls.Add(labSlogan);
            Controls.Add(labMode);
            Controls.Add(labExit);
            Controls.Add(labAbout);
            Controls.Add(labStudent);
            Controls.Add(label1);
            Controls.Add(labPlay);
            Cursor = Cursors.Hand;
            Name = "Home";
            Text = "Trang Chủ - Cờ Tướng";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label labPlay;
        private Label label1;
        private Label labStudent;
        private Label labAbout;
        private Label labExit;
        private Label labMode;
        private Label labSlogan;
    }
}