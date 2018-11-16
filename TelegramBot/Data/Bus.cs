using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    public class Bus
    {
        private static List<Bus> _buss = new List<Bus>();
        public static List<Bus> buss { get { return _buss; } set { _buss = value; } }
        private static string directionFile = "buss.xml";

        public string name;
        public string number;
        public int countPlaces = 0;
        public Driver driver;

        public static void LoadUserData()
        {
            buss = (List<Bus>)XmlLoader.RearDataFromFile<List<Bus>>(directionFile);
            if (buss == null) buss = new List<Bus>();
        }

        public static void SaveUserData()
        {
            XmlLoader.SaveDataToFile<List<Bus>>(buss, directionFile);
        }
    }
}
