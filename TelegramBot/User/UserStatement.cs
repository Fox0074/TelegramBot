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
        public string date;
        public string time;
        public string name;
        public string phoneNumber;
        public int count;
        public bool isProcess = false;

        public stadiya stadi = stadiya.направление;


        public void DeleteStatement()
        {
            direction = "";
            time = "";
            name = "";
            phoneNumber = "";
            count = 0;
            stadi = stadiya.направление;
            isProcess = false;
            Task.Factory.StartNew(() => MailSender.SendBreakEmailAsync(this));
        }

        public async void StartStatement(Telegram.Bot.Types.Message message)
        {
            ReplyKeyboardMarkup ReplyKeyboard = new[]
{
                    new[] { "Челябинск - Куса", "Челябинск - Миасс" },
                    new[] { "Куса - Челябинск", "Миасс - Челябинск" },
                    };
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
                        ReplyKeyboardMarkup ReplyKeyboard = new[]
   {
                            new[] { "27", "28" ,"29", "30" , "31"},
                            new[] { "1", "2" ,"3", "4" , "5"},
                            };
                        await BotBehaviour.Bot.SendTextMessageAsync(
                            message.Chat.Id,
                            "Выберите дату поездки",
                            replyMarkup: ReplyKeyboard);
                    }
                    break;

                case stadiya.дата:
                    if (CheckDate(message.Text))
                    {
                        ReplyKeyboardMarkup ReplyKeyboard = new[]
    {
                            new[] { "11:00", "13:00" },
                            new[] { "15:00", "18:00" },
                            };
                        await BotBehaviour.Bot.SendTextMessageAsync(
                            message.Chat.Id,
                            "Выберите время",
                            replyMarkup: ReplyKeyboard);
                    }
                    else
                    {
                        ReplyKeyboardMarkup ReplyKeyboard = new[]
{
                            new[] { "27", "28" ,"29", "30" , "31"},
                            new[] { "1", "2" ,"3", "4" , "5"},
                            };
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
                int.Parse(value);
                this.date = value;
                stadi = stadiya.время;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CheckTime(string value)
        {
            this.time = value;
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
