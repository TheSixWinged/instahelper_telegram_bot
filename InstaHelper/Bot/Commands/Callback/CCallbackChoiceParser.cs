using InstaHelper.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InstaHelper.Bot.Commands.Callback
{
    class CCallbackChoiceParser : ICallbackCommand
    {
        public string Name => "choice_parser";

        public async void Call(CallbackQuery callback, TelegramBotClient client, IDataBaseWorker dataBase)
        {
            string state = dataBase.GetState(callback.Message.Chat.Id);

            switch(state)
            {
                case Constants.STATE_START:
                    dataBase.ChangeState(callback.Message.Chat.Id, Constants.STATE_INPARSER);

                    await client.EditMessageTextAsync(callback.Message.Chat.Id, callback.Message.MessageId, "Введите username пользователя:");
                    await client.EditMessageReplyMarkupAsync(callback.Message.Chat.Id, callback.Message.MessageId, Keyboards.BackKeyboard());
                    break;
                default:
                    break;
            }
        }
    }
}
