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

        public stadiya _stadi = stadiya.направление;
        public stadiya stadi { get { return _stadi; } set { _stadi = value; pageNumber = 0; } }
        private int pageNumber = 0;

        private List<Rides> sample = new List<Rides>();

        private ReplyMarkupBase GetKeyboard(List<string> veriables, int page = 0)
        {
            ReplyKeyboardMarkup ReplyKeyboard;

            if (veriables.Count < (page) * 10)
            {
                return ReplyKeyboard = "Назад" ;
            }
            if (page < 0)
            {
                return ReplyKeyboard= "Далее" ;
            }
            List<string> answer = new List<string>();

            if (page != 0)
            {
                answer = (veriables.Count > (page + 1) * 10) ? veriables.GetRange(page * 10 + 1, 10) :
                                                               veriables.GetRange(page * 10 + 1, veriables.Count - page * 10 - 1);
                answer.Add("Назад");
                if (veriables.Count > (page + 1) * 10) answer.Add("Дальше");
            }
            else
            {
                if (veriables.Count > 10)
                {
                    answer = veriables.GetRange(0, 11);
                    answer.Add("Дальше");
                }
                else
                {
                    answer = veriables;
                }
            }

            if (answer.Count > 11)
            {
                ReplyKeyboard = new[]
                {
                answer.GetRange(0,4).ToArray(),
                answer.GetRange(4,4).ToArray(),
                answer.GetRange(8,4).ToArray()
                };
            }
            else
            {
                ReplyKeyboard = answer.ToArray();
                ReplyKeyboard.ResizeKeyboard = true;
            }
            return ReplyKeyboard;
        }

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
            await BotBehaviour.Bot.SendTextMessageAsync(
                message.Chat.Id,
                "Выберите маршрут",
                replyMarkup: GetKeyboard(ways,pageNumber));
            isProcess = true;
        }

        public async void SetParams(Telegram.Bot.Types.Message message)
        {
            if (message.Text == "Дальше") pageNumber++;
            if (message.Text == "Назад") pageNumber--;

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
                        if (answer.Count <= 0)
                        {
                            isProcess = false;
                            stadi = stadiya.направление;
                            await BotBehaviour.Bot.SendTextMessageAsync(
                           message.Chat.Id,
                           "Извините, на данный момент маршруты в этом направлении не зарегистрированы", replyMarkup: new ReplyKeyboardRemove());
                            return;
                        }
                        await BotBehaviour.Bot.SendTextMessageAsync(
                            message.Chat.Id,
                            "Выберите дату поездки",
                            replyMarkup: GetKeyboard(answer, pageNumber));
                    }
                    else
                    {
                        List<string> ways = new List<string>();
                        foreach (Direction dir in Direction.directions)
                        {
                            ways.Add(dir.from + "-" + dir.to);
                        }
                        await BotBehaviour.Bot.SendTextMessageAsync(
                            message.Chat.Id,
                            "Выберите маршрут",
                            replyMarkup: GetKeyboard(ways, pageNumber));
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
                        if (answer.Count <= 0)
                        {
                            isProcess = false;
                            stadi = stadiya.направление;
                            await BotBehaviour.Bot.SendTextMessageAsync(
                           message.Chat.Id,
                           "Извините, на данный момент маршруты в этом направлении не зарегистрированы", replyMarkup: new ReplyKeyboardRemove());
                            return;
                        }
                        await BotBehaviour.Bot.SendTextMessageAsync(
                            message.Chat.Id,
                            "Выберите время",
                            replyMarkup: GetKeyboard(answer,pageNumber));
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
                        if (answer.Count <= 0)
                        {
                            isProcess = false;
                            stadi = stadiya.направление;
                            await BotBehaviour.Bot.SendTextMessageAsync(
                           message.Chat.Id,
                           "Извините, на данный момент маршруты в этом направлении не зарегистрированы", replyMarkup: new ReplyKeyboardRemove());
                            return;
                        }
                        await BotBehaviour.Bot.SendTextMessageAsync(
                            message.Chat.Id,
                            "Произошла ошибка, повторите ввод",
                            replyMarkup: GetKeyboard(answer,pageNumber));
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
                               "Спасибо! Вы забронировались на рейс: " + date.Date + " числа в " + time.ToShortTimeString() + " по направлению " + direction + ".Остались вопросы, напишите нам в чат @perevoz74");
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
            try
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
            catch { return false; }
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
