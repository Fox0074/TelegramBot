using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProj
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = "";
            int k = 0;
            List<string> numeric = new List<string>();

            for (int i = 0; i <= 100; i++)
            {
                numeric.Add(i.ToString());
            }

            do
            {
                s = Console.ReadLine();
                if (s == "1") k++;
                if (s == "2") k--;
                Console.WriteLine();

                foreach (string num in GetKeyboard(numeric,k))
                {
                    Console.Write(num + " ");
                }
                Console.WriteLine();
            } while (s != "exit");
        }

        private static string[] GetKeyboard(List<string> veriables, int page = 0)
        {

            if (veriables.Count < (page) * 13)
            {
                return new string[] { "Назад" };
            }
            if (page < 0 )
            {
                return new string[] { "Далее" };
            }
            List<string> answer = new List<string>();

            if (page != 0)
            {
                answer = (veriables.Count > (page + 1) * 13) ? veriables.GetRange(page * 13+1, 13) :
                                                               veriables.GetRange(page * 13+1, veriables.Count - page * 13 -1);
                answer.Add("Назад");
                if (veriables.Count > (page + 1) * 13) answer.Add("Дальше");
            }
            else
            {
                if (veriables.Count > 13)
                {
                    answer = veriables.GetRange(0, 14);
                    answer.Add("Дальше");
                }
                else
                {
                    answer = veriables;
                }
            }

            return answer.ToArray();
        }
    }
}
