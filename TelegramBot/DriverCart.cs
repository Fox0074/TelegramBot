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
    public partial class DriverCart : Form
    {
        Driver driver;
        public DriverCart(Driver driver)
        {
            InitializeComponent();
            this.driver = driver;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            driver.name = textBox1.Text;
            driver.familyName = textBox2.Text;
            driver.phone = textBox3.Text;
            Driver.SaveUserData();
            this.Close();
        }
    }
}
