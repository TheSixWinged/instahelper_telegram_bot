using InstaHelper.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InstaHelper.Bot.Commands.Callback
{
    class CCallbackNoUnique : ICallbackCommand
    {
        public string Name => "no_compet_unique";

        public async void Call(CallbackQuery callback, TelegramBotClient client, IDataBaseWorker dataBase)
        {
            string state = dataBase.GetState(callback.Message.Chat.Id);

            switch (state)
            {
                case Constants.STATE_INCOMPET_CHOOSE_UNIQUE:
                    dataBase.SetCompetUnique(callback.Message.Chat.Id, false);
                    dataBase.ChangeState(callback.Message.Chat.Id, Constants.STATE_INCOMPET_CHOOSE_AUTH);

                    await client.EditMessageTextAsync(callback.Message.Chat.Id, callback.Message.MessageId, "Проверять подписку на Ваш аккаунт?");
                    await client.EditMessageReplyMarkupAsync(callback.Message.Chat.Id, callback.Message.MessageId, Keyboards.CompetAuthKeyboard());
                    break;
                default:
                    break;
            }
        }
    }
}
