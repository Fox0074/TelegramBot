using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

public static class BotBehaviour
{
    public static Telegram.Bot.Types.User me;
    public static readonly TelegramBotClient Bot = new TelegramBotClient("609216764:AAE2EgSmz3uRUOkrtNcALarvAv2rNnRwreI");
        

    public static bool Start()
    {
        try
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            me = Bot.GetMeAsync().Result;

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnInlineQuery += BotOnInlineQueryReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            Bot.OnReceiveError += BotOnReceiveError;

            Bot.StartReceiving(new List<UpdateType>().ToArray());
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static void Stop()
    {
        try
        {
            Bot.StopReceiving();
        }
        catch { }
    }

    private static void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
    {
        var message = messageEventArgs.Message;
    }

    private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
    {
        var callbackQuery = callbackQueryEventArgs.CallbackQuery;

        await Bot.AnswerCallbackQueryAsync(
            callbackQuery.Id,
            $"Received {callbackQuery.Data}");

        await Bot.SendTextMessageAsync(
            callbackQuery.Message.Chat.Id,
            $"Received {callbackQuery.Data}");
    }

    private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
    {
        Console.WriteLine($"Received inline query from: {inlineQueryEventArgs.InlineQuery.From.Id}");

        InlineQueryResultBase[] results = {
            new InlineQueryResultLocation(
                id: "1",
                latitude: 40.7058316f,
                longitude: -74.2581888f,
                title: "New York")   // displayed result
                {
                    InputMessageContent = new InputLocationMessageContent(
                        latitude: 40.7058316f,
                        longitude: -74.2581888f)    // message if result is selected
                },

            new InlineQueryResultLocation(
                id: "2",
                latitude: 13.1449577f,
                longitude: 52.507629f,
                title: "Berlin") // displayed result
                {

                    InputMessageContent = new InputLocationMessageContent(
                        latitude: 13.1449577f,
                        longitude: 52.507629f)   // message if result is selected
                }
        };

        await Bot.AnswerInlineQueryAsync(
            inlineQueryEventArgs.InlineQuery.Id,
            results,
            isPersonal: true,
            cacheTime: 0);
    }

    private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
    {
        Console.WriteLine($"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
    }

    private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
    {
        Console.WriteLine("Received error: {0} — {1}",
            receiveErrorEventArgs.ApiRequestException.ErrorCode,
            receiveErrorEventArgs.ApiRequestException.Message);
    }
}
