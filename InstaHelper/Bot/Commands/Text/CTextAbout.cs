using InstaHelper.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InstaHelper.Bot.Commands.Text
{
    class CTextAbout : ITextCommand
    {
        public string Name => "/about";

        public async void Call(Message message, TelegramBotClient client, IDataBaseWorker dataBase)
        {
            string state = dataBase.GetState(message.Chat.Id);

            switch (state)
            {
                case Constants.STATE_START:
                    try
                    {
                        await client.DeleteMessageAsync(message.Chat.Id, message.MessageId - 1);
                    }
                    catch { }
                    finally
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, "---ABOUT---\nCreated by Anton Katkov aka SixWinged\nthesixwinged@gmail.com", replyMarkup: Keyboards.LinkGitHubKeyboard());
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
