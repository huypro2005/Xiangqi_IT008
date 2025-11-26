using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.src;

namespace WinFormsApp1
{
    public partial class Home : Form
    {
        private GameMode hienTai = GameMode.NguoiVsNguoi;
        public Home()
        {
            InitializeComponent();
            labMode.Text = "2 Players";
        }

        private void labPlay_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 gameForm = new Form1(GameMode.NguoiVsMay);
            gameForm.FormClosed += (s, args) =>
            {
                this.Show();
            };
            gameForm.Show();
        }

        private void labExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void labPlay_MouseEnter(object sender, EventArgs e)
        {
            labPlay.ForeColor = Color.Black;
        }

        private void labPlay_MouseLeave(object sender, EventArgs e)
        {
            labPlay.ForeColor = Color.Snow;
        }

        private void labExit_MouseEnter(object sender, EventArgs e)
        {
            labExit.ForeColor = Color.Black;
        }

        private void labExit_MouseLeave(object sender, EventArgs e)
        {
            labExit.ForeColor = Color.Snow;
        }

        private void labMode_Click(object sender, EventArgs e)
        {
            if (hienTai == GameMode.NguoiVsNguoi)
            {
                hienTai = GameMode.NguoiVsMay;
                labMode.Text = "1 Player";
            }
            else
            {
                hienTai = GameMode.NguoiVsNguoi;
                labMode.Text = "2 Players";
            }
        }

        private void labSlogan_Click(object sender, EventArgs e)
        {

        }
    }
}
