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
            labPlay.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labPlay.ForeColor = Color.Snow;
            labPlay.Location = new Point(421, 216);
            labPlay.Name = "labPlay";
            labPlay.Size = new Size(63, 29);
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
            label1.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.Snow;
            label1.Location = new Point(364, 280);
            label1.Name = "label1";
            label1.Size = new Size(0, 29);
            label1.TabIndex = 2;
            // 
            // labStudent
            // 
            labStudent.AutoSize = true;
            labStudent.BackColor = Color.FromArgb(192, 0, 0);
            labStudent.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labStudent.ForeColor = Color.Snow;
            labStudent.Location = new Point(421, 323);
            labStudent.Name = "labStudent";
            labStudent.Size = new Size(102, 29);
            labStudent.TabIndex = 3;
            labStudent.Text = "Student";
            labStudent.MouseEnter += labStudent_MouseEnter;
            labStudent.MouseLeave += labStudent_MouseLeave;
            // 
            // labAbout
            // 
            labAbout.AutoSize = true;
            labAbout.BackColor = Color.FromArgb(192, 0, 0);
            labAbout.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labAbout.ForeColor = Color.Snow;
            labAbout.Location = new Point(420, 376);
            labAbout.Name = "labAbout";
            labAbout.Size = new Size(80, 29);
            labAbout.TabIndex = 4;
            labAbout.Text = "About";
            labAbout.Click += labAbout_Click_1;
            labAbout.MouseEnter += labAbout_MouseEnter;
            labAbout.MouseLeave += labAbout_MouseLeave;
            // 
            // labExit
            // 
            labExit.AutoSize = true;
            labExit.BackColor = Color.FromArgb(192, 0, 0);
            labExit.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labExit.ForeColor = Color.Snow;
            labExit.Location = new Point(421, 422);
            labExit.Name = "labExit";
            labExit.Size = new Size(56, 29);
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
            labMode.Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labMode.ForeColor = Color.Snow;
            labMode.Location = new Point(405, 270);
            labMode.Name = "labMode";
            labMode.Size = new Size(79, 29);
            labMode.TabIndex = 7;
            labMode.Text = "Mode";
            labMode.Click += labMode_Click;
            labMode.MouseEnter += labMode_MouseEnter;
            labMode.MouseLeave += labMode_MouseLeave;
            // 
            // labSlogan
            // 
            labSlogan.AutoSize = true;
            labSlogan.BackColor = Color.Transparent;
            labSlogan.FlatStyle = FlatStyle.Flat;
            labSlogan.Font = new Font("Bradley Hand ITC", 48F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labSlogan.ForeColor = Color.Snow;
            labSlogan.Location = new Point(115, 96);
            labSlogan.Name = "labSlogan";
            labSlogan.Size = new Size(598, 79);
            labSlogan.TabIndex = 8;
            labSlogan.Text = "Welcome to XiangQi";
            labSlogan.Click += labSlogan_Click;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(900, 612);
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