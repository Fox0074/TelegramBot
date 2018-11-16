using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace TelegramBot
{
    public static class MailSender
    {
        public static async Task SendBreakEmailAsync(UserStatement user)
        {
            MailAddress from = new MailAddress("xismatulin.ru@yandex.ru", "Telegram_Bot");
            MailAddress to = new MailAddress("salebuscom@mail.ru");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Отмена заявки";
            m.Body = "Направление: " + user.direction + "\nВремя: " + user.time + "\nИмя: " + user.name + "\nНомер: " + user.phoneNumber;
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 25);
            smtp.Credentials = new NetworkCredential("xismatulin.ru", "377310349");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }

        public static async Task SendEmailAsync(UserStatement user)
        {
            MailAddress from = new MailAddress("xismatulin.ru@yandex.ru", "Telegram_Bot");
            MailAddress to = new MailAddress("salebuscom@mail.ru");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Заявка";
            m.Body = "Направление: " + user.direction + "\nВремя: " + user.time + "\nИмя: " + user.name + "\nНомер: " + user.phoneNumber;
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 25);
            smtp.Credentials = new NetworkCredential("xismatulin.ru", "377310349");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }
    }
}
