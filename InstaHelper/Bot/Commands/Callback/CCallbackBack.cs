using InstaHelper.DataBase;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace InstaHelper.Bot.Commands.Callback
{
    class CCallbackBack : ICallbackCommand
    {
        public string Name => "back";

        public async void Call(CallbackQuery callback, TelegramBotClient client, IDataBaseWorker dataBase)
        {
            string state = dataBase.GetState(callback.Message.Chat.Id);

            switch(state)
            {
                case Constants.STATE_START:
                    await client.DeleteMessageAsync(callback.Message.Chat.Id, callback.Message.MessageId);
                    await client.SendTextMessageAsync(callback.Message.Chat.Id, "Введите /start чтобы начать работу со мной");
                    break;
                case Constants.STATE_INPARSER:
                    dataBase.ChangeState(callback.Message.Chat.Id, Constants.STATE_START);

                    await client.EditMessageTextAsync(callback.Message.Chat.Id, callback.Message.MessageId, "---MAIN MENU---\nС чем будем работать?");
                    await client.EditMessageReplyMarkupAsync(callback.Message.Chat.Id, callback.Message.MessageId, Keyboards.StartKeyboard());
                    break;
                case Constants.STATE_INCOMPET_CHOOSE_UNIQUE:
                    dataBase.ChangeState(callback.Message.Chat.Id, Constants.STATE_START);

                    await client.EditMessageTextAsync(callback.Message.Chat.Id, callback.Message.MessageId, "---MAIN MENU---\nС чем будем работать?");
                    await client.EditMessageReplyMarkupAsync(callback.Message.Chat.Id, callback.Message.MessageId, Keyboards.StartKeyboard());
                    break;
                case Constants.STATE_INCOMPET_CHOOSE_AUTH:
                    dataBase.ChangeState(callback.Message.Chat.Id, Constants.STATE_INCOMPET_CHOOSE_UNIQUE);

                    await client.EditMessageTextAsync(callback.Message.Chat.Id, callback.Message.MessageId, "Чем больше комментариев, тем больше шансов выиграть?");
                    await client.EditMessageReplyMarkupAsync(callback.Message.Chat.Id, callback.Message.MessageId, Keyboards.CompetUniqueKeyboard());
                    break;
                case Constants.STATE_INCOMPET_LOGIN:
                    dataBase.ChangeState(callback.Message.Chat.Id, Constants.STATE_INCOMPET_CHOOSE_AUTH);

                    await client.EditMessageTextAsync(callback.Message.Chat.Id, callback.Message.MessageId, "Проверять подписку на Ваш аккаунт?");
                    await client.EditMessageReplyMarkupAsync(callback.Message.Chat.Id, callback.Message.MessageId, Keyboards.CompetAuthKeyboard());
                    break;
                case Constants.STATE_INCOMPET_POSTLINK:
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
