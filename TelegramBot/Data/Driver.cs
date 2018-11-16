using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    public class Driver
    {
        private static List<Driver> _drivers = new List<Driver>();
        public static List<Driver> drivers { get { return _drivers; } set { _drivers = value; } }
        private static string directionFile = "drivers.xml";

        public string name;
        public string familyName;
        public string phone;
        public Bus bus;

        public static void LoadUserData()
        {
            drivers = (List<Driver>)XmlLoader.RearDataFromFile<List<Driver>>(directionFile);
            if (drivers == null) drivers = new List<Driver>();
        }

        public static void SaveUserData()
        {
            XmlLoader.SaveDataToFile<List<Driver>>(drivers, directionFile);
        }
    }
}
