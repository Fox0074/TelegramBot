using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelegramBot
{
    public partial class DirectionCart : Form
    {
        public Direction direction;
        public DirectionCart(Direction direction)
        {
            InitializeComponent();
            this.direction = direction;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox3.Text);
            textBox3.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            direction.from = textBox1.Text;
            direction.to = textBox2.Text;
            foreach (string item in listBox1.Items)
            {
                direction.through.Add(item);
            }

            Direction.SaveUserData();
            this.Close();
        }
    }
}
