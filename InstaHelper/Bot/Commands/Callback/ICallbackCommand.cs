using InstaHelper.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InstaHelper.Bot.Commands.Callback
{
    interface ICallbackCommand
    {
        string Name { get; }

        void Call(CallbackQuery callback, TelegramBotClient client, IDataBaseWorker dataBase);
    }
}
