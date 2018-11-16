using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    public class User
    {
        public static List<User> users = new List<User>();
        public long id;
        public string userName;
        public DateTime lastMessageTime;

        public UserStatement statement = new UserStatement();

        public User()
        {
            users.Add(this);
        }

        public static User CheckUser(Chat chat)
        {
            User result = null;

            foreach (User u in users)
            {
                if (u.id == chat.Id)
                    result = u;
            }

            if (result == null)
            {
                result = new User();
                result.id = chat.Id;
                result.userName = chat.FirstName;
            }

            result.lastMessageTime = DateTime.Now;
            return result;
        }

        public async void Command(Telegram.Bot.Types.Message message)
        {
            if (message.Text.Split(' ').First() == "/break")
            {
                statement.isProcess = false;
                statement.stadi = stadiya.направление;
                await BotBehaviour.Bot.SendTextMessageAsync(
                           message.Chat.Id,
                           "Вы прервали регистрацию", replyMarkup: new ReplyKeyboardRemove());
            }

            if (statement.isProcess)
            {
                statement.SetParams(message);
                return;
            }

            switch (message.Text.Split(' ').First())
            {
                case "/setline":
                    statement.StartStatement(message);
                    break;

                case "/deleteline":
                    statement.DeleteStatement();
                    break;

                default:
                    const string usage = @"
                                    Используйте команды:
                                    /setline  - Забронировать место
                                    /deleteline - Отменить бронь 
                                    /break    - Прервать регистрацию";

                    await BotBehaviour.Bot.SendTextMessageAsync(
                        message.Chat.Id,
                        usage,
                        replyMarkup: new ReplyKeyboardRemove());
                    break;
            }
        }

    }
}
