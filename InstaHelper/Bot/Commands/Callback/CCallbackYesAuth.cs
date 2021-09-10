using InstaHelper.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InstaHelper.Bot.Commands.Callback
{
    class CCallbackYesAuth : ICallbackCommand
    {
        public string Name => "yes_compet_auth";

        public async void Call(CallbackQuery callback, TelegramBotClient client, IDataBaseWorker dataBase)
        {
            string state = dataBase.GetState(callback.Message.Chat.Id);

            switch(state)
            {
                case Constants.STATE_INCOMPET_CHOOSE_AUTH:
                    dataBase.SetCompetAuth(callback.Message.Chat.Id, true);
                    dataBase.ChangeState(callback.Message.Chat.Id, Constants.STATE_INCOMPET_LOGIN);

                    await client.EditMessageTextAsync(callback.Message.Chat.Id, callback.Message.MessageId, "Введите Ваш логин:");
                    await client.EditMessageReplyMarkupAsync(callback.Message.Chat.Id, callback.Message.MessageId, Keyboards.BackKeyboard());
                    break;
                default:
                    break;
            } 
        }
    }
}
