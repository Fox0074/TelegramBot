using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    public partial class Main : Form
    {
        public static Main current;
        public Main()
        {
            if (current == null) current = this;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (BotBehaviour.Start())
            {
                Text = BotBehaviour.me.Username;
                label2.Text = "работает";
                button1.Enabled = false;
                BotBehaviour.Bot.OnMessage += BotOnMessageReceived;
            }
            else
            {
                MessageBox.Show("Не удалось запустить бота");
            }
        }

        private void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.Text) return;

            User user = User.CheckUser(message.Chat);
            user.Command(message);



            if (listBox1.InvokeRequired) listBox1.BeginInvoke(new Action(() => { listBox1.Items.Add(message.Chat.FirstName + ": " + message.Text); }));
            else listBox1.Items.Add(message.Text);

        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            BotBehaviour.Stop();
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RidesSettings ridesSettings = new RidesSettings();
            ridesSettings.Show();
            this.Hide();
        }
    }
}
