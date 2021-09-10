using InstaHelper.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InstaHelper.Bot.Commands.Text
{
    class CTextStart : ITextCommand
    {
        public string Name => "/start";

        public async void Call(Message message, TelegramBotClient client, IDataBaseWorker dataBase)
        {
            string state = dataBase.GetState(message.Chat.Id);

            switch(state)
            {
                case Constants.STATE_START:
                    try
                    {
                        await client.DeleteMessageAsync(message.Chat.Id, message.MessageId - 1);
                    }
                    catch { }
                    finally
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, "---MAIN MENU---\nС чем будем работать?", replyMarkup: Keyboards.StartKeyboard());
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
