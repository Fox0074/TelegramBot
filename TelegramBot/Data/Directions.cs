using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TelegramBot
{
    [Serializable]
    public class Direction
    {
        private static List<Direction> _directions = new List<Direction>();
        public static List<Direction> directions { get { return _directions; } set { _directions = value;} }
        private static string directionFile = "directions.xml";


        public string from;
        public string to;
        public List<string> through = new List<string>();

        public static void LoadUserData()
        {
            directions = (List<Direction>)XmlLoader.RearDataFromFile<List<Direction>>(directionFile);
            if (directions == null) directions = new List<Direction>();
        }

        public static void SaveUserData()
        {
            XmlLoader.SaveDataToFile<List<Direction>>(directions, directionFile);
        }

    }
}
