using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TelegramBot
{
    public static class XmlLoader
    {


        public static void SaveDataToFile<T>(object obj,string file)
        {
            if (File.Exists(file)) File.Delete(file);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextWriter writer = new StreamWriter(file);

            serializer.Serialize(writer, obj);
            writer.Close();
        }

        public static object RearDataFromFile<T>(string file)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.UnknownNode += new XmlNodeEventHandler(Serializer_UnknownNode);
                serializer.UnknownAttribute += new XmlAttributeEventHandler(Serializer_UnknownAttribute);

                FileStream fs = new FileStream(file, FileMode.Open);
                object result;
                result = serializer.Deserialize(fs);
                fs.Close();
                return result;
            }
            catch
            {
                MessageBox.Show("Ошибка, файл не найден");
                return null;
            }
        }

        private static void Serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            //Log.Send("UserData: Unknown Node:" + e.Name + "\t" + e.Text);
            MessageBox.Show("Ошибка чтения файла");
        }

        private static void Serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            //Log.Send("UserData: Unknown attribute " + attr.Name + "='" + attr.Value + "'");
            MessageBox.Show("Ошибка чтения файла");
        }
    }
}
