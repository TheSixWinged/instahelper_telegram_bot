using InstaHelper.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InstaHelper.Bot.Commands.Text
{
    class CTextHelp : ITextCommand
    {
        public string Name => "/help";

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
                        await client.SendTextMessageAsync(message.Chat.Id, "---HELP---\nДля начала работы введите /start\nВыбирайте пункты меню в сообщениях от бота для управления программой\nОсновные команды:\n/start - Главное меню\n/help - Помощь и навигация\n/about - Информация о разработчике\n\nПарсер аудитории рекомендуется использовать на небольших аккаунтах в связи с ограничением в 2000 получаемых username\n\nПарсер аудитории собирает информацию только с открытых аккаунтов");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
