using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    public class Rides
    {
        private static List<Rides> _rides = new List<Rides>();
        public static List<Rides> rides { get { return _rides; } set { _rides = value; } }
        private static string ridesFile = "riders.xml";

        public Bus bus;
        public Direction direction;
        public Driver driver;
        public DateTime dateTime;

        public Rides()
        {
        }

        public Rides(DateTime dateTime, Direction direction)
        {
            this.dateTime = dateTime;
            this.direction = direction;
        }

        public static void LoadRides()
        {
            rides = (List<Rides>)XmlLoader.RearDataFromFile<List<Rides>>(ridesFile);
            if (rides == null) rides = new List<Rides>();

            //Удаление просроченных поездок
            foreach (Rides rider in rides)
            {
                if (rider.dateTime < DateTime.Now)
                {
                    rides.Remove(rider);
                }
            }
        }

        public static void SaveRides()
        {
            XmlLoader.SaveDataToFile<List<Rides>>(rides, ridesFile);
        }
    }
}
