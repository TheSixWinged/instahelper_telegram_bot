using InstaHelper.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InstaHelper.Bot.Commands.Callback
{
    class DevCCallbackYesAuth : ICallbackCommand
    {
        public string Name => "dev_compet_auth";

        public async void Call(CallbackQuery callback, TelegramBotClient client, IDataBaseWorker dataBase)
        {
            string state = dataBase.GetState(callback.Message.Chat.Id);

            switch (state)
            {
                case Constants.STATE_INCOMPET_CHOOSE_AUTH:
                    dataBase.SetCompetAuth(callback.Message.Chat.Id, false);
                    dataBase.ChangeState(callback.Message.Chat.Id, Constants.STATE_INCOMPET_POSTLINK);

                    await client.EditMessageTextAsync(callback.Message.Chat.Id, callback.Message.MessageId, "Сервер перегружен, в данный момент проверка недоступна, приносим свои извинения\n\nВведите ссылку на пост-розыгрыш:");
                    await client.EditMessageReplyMarkupAsync(callback.Message.Chat.Id, callback.Message.MessageId, Keyboards.BackKeyboard());
                    break;
                default:
                    break;
            }
        }
    }
}
