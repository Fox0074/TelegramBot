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
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            RefreshCells(); 
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main.current.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Direction newDirection = new Direction();
            Direction.directions.Add(newDirection);
            DirectionCart directionCart = new DirectionCart(newDirection);
            directionCart.Show();
            directionCart.FormClosing += (object sender1, FormClosingEventArgs e1) => RefreshCells();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Driver newDriver = new Driver();
            Driver.drivers.Add(newDriver);
            DriverCart driverCart = new DriverCart(newDriver);
            driverCart.Show();
            driverCart.FormClosing += (object sender1, FormClosingEventArgs e1) => RefreshCells();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bus newBus = new Bus();
            Bus.buss.Add(newBus);
            BusCart busCart = new BusCart(newBus);
            busCart.Show();
            busCart.FormClosing += (object sender1, FormClosingEventArgs e1) => RefreshCells();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Direction.directions.RemoveAt(listBox1.SelectedIndex);
            Direction.SaveUserData();
            RefreshCells();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Driver.drivers.RemoveAt(listBox2.SelectedIndex);
            Driver.SaveUserData();
            RefreshCells();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Bus.buss.RemoveAt(listBox3.SelectedIndex);
            Bus.SaveUserData();
            RefreshCells();
        }

        private void RefreshCells()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();

            foreach (Direction direction in Direction.directions)
            {
                listBox1.Items.Add(direction.from + " - " + direction.to);
            }
            foreach (Driver driver in Driver.drivers)
            {
                listBox2.Items.Add(driver.name + " " + driver.familyName);
            }
            foreach (Bus bus in Bus.buss)
            {
                listBox3.Items.Add(bus.name + " " + bus.countPlaces);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DirectionCart directionCart = new DirectionCart(Direction.directions.ElementAt(listBox1.SelectedIndex));
            directionCart.Show();
            directionCart.FormClosing += (object sender1, FormClosingEventArgs e1) => RefreshCells();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DriverCart driverCart = new DriverCart(Driver.drivers.ElementAt(listBox2.SelectedIndex));
            driverCart.Show();
            driverCart.FormClosing += (object sender1, FormClosingEventArgs e1) => RefreshCells();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            BusCart busCart = new BusCart(Bus.buss.ElementAt(listBox3.SelectedIndex));
            busCart.Show();
            busCart.FormClosing += (object sender1, FormClosingEventArgs e1) => RefreshCells();
        }
    }
}
