using InstaHelper.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InstaHelper.Bot.Commands.Callback
{
    class CCallbackChoiceCompet : ICallbackCommand
    {
        public string Name => "choice_compet";

        public async void Call(CallbackQuery callback, TelegramBotClient client, IDataBaseWorker dataBase)
        {
            string state = dataBase.GetState(callback.Message.Chat.Id);

            switch (state)
            {
                case Constants.STATE_START:
                    dataBase.ChangeState(callback.Message.Chat.Id, Constants.STATE_INCOMPET_CHOOSE_UNIQUE);

                    await client.EditMessageTextAsync(callback.Message.Chat.Id, callback.Message.MessageId, "Чем больше комментариев, тем больше шансов выиграть?");
                    await client.EditMessageReplyMarkupAsync(callback.Message.Chat.Id, callback.Message.MessageId, Keyboards.CompetUniqueKeyboard());
                    break;
                default:
                    break;
            }
        }
    }
}
