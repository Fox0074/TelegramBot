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
    public partial class BusCart : Form
    {
        Bus bus;
        public BusCart(Bus bus)
        {
            InitializeComponent();
            this.bus = bus;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bus.name = textBox1.Text;
            bus.number = textBox2.Text;
            Driver.SaveUserData();
            this.Close();
        }
    }
}
