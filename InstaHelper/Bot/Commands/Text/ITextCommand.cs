using InstaHelper.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InstaHelper.Bot.Commands.Text
{
    interface ITextCommand
    {
        string Name { get; }
        void Call(Message message, TelegramBotClient client, IDataBaseWorker dataBase);
    }
}
