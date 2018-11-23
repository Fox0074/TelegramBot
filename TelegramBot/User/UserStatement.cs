using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{

    public enum stadiya {направление, время, имя, телефон, места, дата };

    public class UserStatement
    {
        public string direction;
        public DateTime date = new DateTime();
        public DateTime time = new DateTime();
        public string name;
        public string phoneNumber;
        public int count;
        public bool isProcess = false;

        public stadiya stadi = stadiya.направление;

        private List<Rides> sample = new List<Rides>();

        public void DeleteStatement()
        {
            direction = "";
            name = "";
            phoneNumber = "";
            count = 0;
            stadi = stadiya.направление;
            isProcess = false;
            Task.Factory.StartNew(() => MailSender.SendBreakEmailAsync(this));
        }

        public async void StartStatement(Telegram.Bot.Types.Message message)
        {
            sample.Clear();
            List<string> ways = new List<string>();
            foreach (Direction dir in Direction.directions)
            {
                ways.Add(dir.from + "-" + dir.to);
            }
            ReplyKeyboardMarkup ReplyKeyboard = ways.ToArray();
            await BotBehaviour.Bot.SendTextMessageAsync(
                message.Chat.Id,
                "Выберите маршрут",
                replyMarkup: ReplyKeyboard);
            isProcess = true;
        }

        public async void SetParams(Telegram.Bot.Types.Message message)
        {
            
            switch (stadi)
            {
                case stadiya.направление:
                    if (CheckDirection(message.Text))
                    {
                        List<string> answer = new List<string>();
                        foreach (Rides ride in Rides.rides)
                        {
                            if ((ride.direction.from + "-" + ride.direction.to) == direction)
                            {
                                sample.Add(ride);
                                answer.Add(ride.dateTime.Day.ToString()+"."+ ride.dateTime.Month.ToString());
                            }
                        }
                        ReplyKeyboardMarkup ReplyKeyboard = answer.ToArray();
                        await BotBehaviour.Bot.SendTextMessageAsync(
                            message.Chat.Id,
                            "Выберите дату поездки",
                            replyMarkup: ReplyKeyboard);
                    }
                    break;

                case stadiya.дата:
                    if (CheckDate(message.Text))
                    {
                        List<string> answer = new List<string>();
                        foreach (Rides ride in sample)
                        {
                            answer.Add(ride.dateTime.ToShortTimeString());
                        }
                        ReplyKeyboardMarkup ReplyKeyboard = answer.ToArray();
                        await BotBehaviour.Bot.SendTextMessageAsync(
                            message.Chat.Id,
                            "Выберите время",
                            replyMarkup: ReplyKeyboard);
                    }
                    else
                    {
                        List<string> answer = new List<string>();
                        foreach (Rides ride in Rides.rides)
                        {
                            if ((ride.direction.from + "-" + ride.direction.to) == direction)
                            {
                                answer.Add(ride.dateTime.Day.ToString() + "." + ride.dateTime.Month.ToString());
                            }
                        }
                        ReplyKeyboardMarkup ReplyKeyboard = answer.ToArray();
                        await BotBehaviour.Bot.SendTextMessageAsync(
                            message.Chat.Id,
                            "Произошла ошибка, повторите ввод",
                            replyMarkup: ReplyKeyboard);
                    }
                    break;
                case stadiya.время:

                    CheckTime(message.Text);
                    await BotBehaviour.Bot.SendTextMessageAsync(
                           message.Chat.Id,
                           "Введите колличество мест, которые хотите забронировать", replyMarkup: new ReplyKeyboardRemove());
                    break;
                case stadiya.имя:
                    SetName(message.Text);
                    await BotBehaviour.Bot.SendTextMessageAsync(
                           message.Chat.Id,
                           "Введите ваш номер телефона");
                    break;
                case stadiya.места:
                    if (SetCount(message.Text))
                    {
                        await BotBehaviour.Bot.SendTextMessageAsync(
                               message.Chat.Id,
                               "Введите имя и фамилию");
                    }
                    else
                    {
                        await BotBehaviour.Bot.SendTextMessageAsync(
                           message.Chat.Id,
                           "Повторите ввод используя только цифры, введите колличество мест, которые хотите забронировать");
                    }
                    break;
                case stadiya.телефон:
                    if (CheckPhone(message.Text))
                    {
                        await BotBehaviour.Bot.SendTextMessageAsync(
                               message.Chat.Id,
                               "Спасибо! Вы забронировались на рейс: " + date + " числа в " + time + " по направлению " + direction + ".Остались вопросы, напишите нам в чат @perevoz74");
                        stadi = stadiya.направление;
                        await MailSender.SendEmailAsync(this);
                        isProcess = false;
                    }
                    else
                    {
                        await BotBehaviour.Bot.SendTextMessageAsync(
                                                   message.Chat.Id,
                                                   "Номер введен неверно, введите номер в формате 8 960 123 45 67");
                    }
                    break;

            }
        }

        private bool CheckDirection(string value)
        {
            value = value.Replace(" ", "");
            string from = value.Split('-')[0];
            string to = value.Split('-')[1];

            foreach (Direction direction in Direction.directions)
            {
                if (direction.from == from && direction.to == to)
                {
                    this.direction = value;
                    stadi = stadiya.дата;
                    return true;
                }
            }
            return false;
        }
        
        private bool CheckDate(string value)
        {
            try
            {
                List<Rides> buf = new List<Rides>();
                foreach (Rides ride in sample)
                {
                    if ((ride.dateTime.Day.ToString() + "." + ride.dateTime.Month.ToString()) == value)
                    {
                        buf.Add(ride);
                        date = ride.dateTime.Date;
                        stadi = stadiya.время;   
                    }
                }
                if (buf.Count > 0)
                {
                    sample = buf;
                    return true;
                }
                return false;

            }
            catch
            {
                return false;
            }
        }

        private void CheckTime(string value)
        {
            foreach (Rides ride in sample)
            {
                if (ride.dateTime.ToShortTimeString() == value)
                {
                    time = ride.dateTime;
                }
            }
            stadi = stadiya.места;
        }

        private void SetName(string value)
        {
            this.name = value;
            stadi = stadiya.телефон;
        }

        private bool SetCount(string value)
        {
            try
            {
                this.count = int.Parse(value);
                stadi = stadiya.имя;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool CheckPhone(string value)
        {
            value = value.Replace("+","");
            value = value.Replace("-", "");
            value = value.Replace(" ","");
            try
            {
                long.Parse(value);
                if (value.Length == 10 || value.Length == 11)
                {
                    this.phoneNumber = value;
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
