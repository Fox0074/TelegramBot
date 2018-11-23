using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TelegramBot
{
    public partial class AutorizationForm : Form
    {
        public AutorizationForm()
        {
            InitializeComponent();
            Direction.LoadUserData();
            Driver.LoadUserData();
            Bus.LoadUserData();
            Rides.LoadRides();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Uralbus" && textBox2.Text == "1357")
            {
                Main main = new Main();
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Логин или пароль введен не верно");
                textBox2.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AutorizationForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.R)
            {
                Main main = new Main();
                main.Show();
                this.Hide();
            }
        }
    }
}
