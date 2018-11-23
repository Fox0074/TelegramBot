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
    public partial class RidesSettings : Form
    {
        public RidesSettings()
        {
            InitializeComponent();
            LoadRiders();
        }

        private void RidesSettings_Load(object sender, EventArgs e)
        {
            foreach (Direction dir in Direction.directions)
            {
                comboBox1.Items.Add(dir.from + "-" + dir.to);
            }
            foreach (Bus bus in Bus.buss)
            {
                comboBox2.Items.Add(bus.name + " " + bus.number);
            }
            foreach (Driver driver in Driver.drivers)
            {
                comboBox3.Items.Add(driver.name + " " + driver.familyName);
            }
        }

        private void RidesSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main.current.Show();
            DateTime dtt = dateTimePicker1.Value;
        }

        private void Save()
        {
            Rides ride = new Rides(dateTimePicker1.Value, Direction.directions[comboBox1.SelectedIndex]);
            ride.bus = Bus.buss[comboBox2.SelectedIndex];
            ride.driver = Driver.drivers[comboBox3.SelectedIndex];

            Rides.rides.Add(ride);
            Rides.SaveRides();
            LoadRiders();
        }

        private void LoadRiders()
        {
            listBox1.Items.Clear();
            foreach (Rides ride in Rides.rides)
            {
                listBox1.Items.Add(ride.dateTime + " " + ride.direction.from + "-" + ride.direction.to);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Rides.rides.RemoveAt(listBox1.SelectedIndex);
            Rides.SaveRides();
            LoadRiders();
        }
    }
}
