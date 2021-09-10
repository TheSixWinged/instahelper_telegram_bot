using InstaHelper.Bot.Commands.Callback;
using InstaHelper.Bot.Commands.Text;
using InstaHelper.DataBase;
using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace InstaHelper.Bot
{
    class BotWorker: IDisposable
    {
        private static readonly TelegramBotClient botClient = null;

        private static readonly IDataBaseWorker dataBase = null;

        private static readonly List<ITextCommand> textCommandsList = new List<ITextCommand>();
        private static readonly List<ICallbackCommand> callbackCommandsList = new List<ICallbackCommand>();

        private static readonly ITextCommand textCommandInput = null;

        //private static readonly string token = "";
        private static readonly string token = Environment.GetEnvironmentVariable("BOT_TOKEN");

        static BotWorker()
        {
            botClient = new TelegramBotClient(token) { Timeout = TimeSpan.FromSeconds(10) };

            dataBase = MySqlWorker.getInstance();

            //TODO: initialize all comands
            textCommandsList.Add(new CTextStart());
            textCommandsList.Add(new CTextHelp());
            textCommandsList.Add(new CTextAbout());
            callbackCommandsList.Add(new CCallbackBack());
            callbackCommandsList.Add(new CCallbackChoiceParser());
            callbackCommandsList.Add(new CCallbackChoiceCompet());
            callbackCommandsList.Add(new CCallbackNoAuth());
            callbackCommandsList.Add(new CCallbackYesAuth());
            callbackCommandsList.Add(new CCallbackNoUnique());
            callbackCommandsList.Add(new CCallbackYesUnique());
            callbackCommandsList.Add(new DevCCallbackYesAuth());

            textCommandInput = new CTextInput();

            botClient.StartReceiving();

            botClient.OnMessage += OnMessageReceiving;
            botClient.OnCallbackQuery += OnCallbackQueryReceiving;

            Console.WriteLine(botClient.GetMeAsync().Result);
        }

        private static void OnMessageReceiving(object sender, MessageEventArgs e)
        {

            foreach (var command in textCommandsList)
            {
                if (e.Message.Text == command.Name)
                {
                    command.Call(e.Message, botClient, dataBase);
                }
            }

            textCommandInput.Call(e.Message, botClient, dataBase);
        }

        private static void OnCallbackQueryReceiving(object sender, CallbackQueryEventArgs e)
        {
            foreach (var command in callbackCommandsList)
            {
                if (e.CallbackQuery.Data == command.Name)
                {
                    command.Call(e.CallbackQuery, botClient, dataBase);
                }
            }
        }

        public void Dispose()
        {
            if (botClient != null && botClient.IsReceiving)
            {
                botClient.StopReceiving();
            }
        }
    }
}
