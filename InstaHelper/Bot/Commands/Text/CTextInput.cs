using InstaHelper.DataBase;
using InstaHelper.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace InstaHelper.Bot.Commands.Text
{
    class CTextInput : ITextCommand
    {
        public string Name => "inputAllTextMessages";

        private object sync = new object();

        public async void Call(Message message, TelegramBotClient client, IDataBaseWorker dataBase)
        {
            string state = dataBase.GetState(message.Chat.Id);
            dataBase.ChangeState(message.Chat.Id, Constants.STATE_DONT_LISTENING);

            switch (state)
            {
                case Constants.STATE_START:
                    if(message.Text.StartsWith('/'))
                    {
                        
                    }
                    else
                    {
                        try { await client.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId - 1, null); }
                        catch { }
                        finally { await client.SendTextMessageAsync(message.Chat.Id, "Используйте доступные команды и кнопки меню\nВведите /help чтобы посмотреть справку"); }
                    }
                    dataBase.ChangeState(message.Chat.Id, Constants.STATE_START);
                    break;
                case Constants.STATE_INPARSER:
                    if (message.Text.StartsWith('/'))
                    {
                        await client.DeleteMessageAsync(message.Chat.Id, message.MessageId - 1);
                        await client.SendTextMessageAsync(message.Chat.Id, "Чтобы выполнять основные команды вернитесь в главное меню!");
                        await client.SendTextMessageAsync(message.Chat.Id, "Введите username пользователя:", replyMarkup: Keyboards.BackKeyboard());
                        dataBase.ChangeState(message.Chat.Id, Constants.STATE_INPARSER);
                    }
                    else
                    {
                        string username = message.Text;

                        if (username.IsValidInstagramUsername())
                        {
                            List<string> accounts = new List<string>();
                            username = username.ValidateInstagramUsername();

                            dataBase.InputParser(message.Chat.Id, username);

                            try { await client.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId - 1, null); } catch { }
                            await client.SendTextMessageAsync(message.Chat.Id, $"Username: {username} получен!\nРаботаю, подождите около 5 минут");

                            lock (sync)
                            {
                                using (SeleniumWorker browser = new SeleniumWorker(dataBase))
                                {
                                    accounts = browser.SubscribersParser(message.Chat.Id);
                                }
                            }  

                            if(accounts.Count == 0)
                            {
                                await client.SendTextMessageAsync(message.Chat.Id, "Подписчиков не найдено! Проверьте корректность данных");
                            }
                            else
                            {
                                string filePath = $"{Directory.GetCurrentDirectory()}/{message.Chat.Id}_{username}_inputParser.txt";

                                using (StreamWriter sw = new StreamWriter(filePath))
                                {
                                    foreach (string acc in accounts)
                                    {
                                        sw.WriteLine(acc);
                                    }
                                }

                                using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                                {
                                    InputOnlineFile file = new InputOnlineFile(fileStream);
                                    file.FileName = $"{username}_followers.txt";
                                    await client.SendDocumentAsync(message.Chat.Id, file);
                                }

                                System.IO.File.Delete(filePath);
                            }

                            await client.SendTextMessageAsync(message.Chat.Id, "Введите /start чтобы начать работу со мной");

                            dataBase.ChangeState(message.Chat.Id, Constants.STATE_START);
                        }
                        else
                        {
                            try { await client.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId - 1, null); } catch { }
                            await client.SendTextMessageAsync(message.Chat.Id, "Некорректный ввод!");
                            await client.SendTextMessageAsync(message.Chat.Id, "Введите username пользователя:", replyMarkup: Keyboards.BackKeyboard());

                            dataBase.ChangeState(message.Chat.Id, Constants.STATE_INPARSER);
                        }
                    }
                    break;
                case Constants.STATE_INCOMPET_CHOOSE_UNIQUE:
                    if (message.Text.StartsWith('/'))
                    {
                        await client.DeleteMessageAsync(message.Chat.Id, message.MessageId - 1);
                        await client.SendTextMessageAsync(message.Chat.Id, "Чтобы выполнять основные команды вернитесь в главное меню!");
                        await client.SendTextMessageAsync(message.Chat.Id, "Чем больше комментариев, тем больше шансов выиграть?", replyMarkup: Keyboards.CompetUniqueKeyboard());
                    }
                    else
                    {
                        await client.DeleteMessageAsync(message.Chat.Id, message.MessageId - 1);
                        await client.SendTextMessageAsync(message.Chat.Id, "Пожалуйста, выберите пункт меню");
                        await client.SendTextMessageAsync(message.Chat.Id, "Чем больше комментариев, тем больше шансов выиграть?", replyMarkup: Keyboards.CompetUniqueKeyboard());
                    }
                    dataBase.ChangeState(message.Chat.Id, Constants.STATE_INCOMPET_CHOOSE_UNIQUE);
                    break;
                case Constants.STATE_INCOMPET_CHOOSE_AUTH:
                    if (message.Text.StartsWith('/'))
                    {
                        await client.DeleteMessageAsync(message.Chat.Id, message.MessageId - 1);
                        await client.SendTextMessageAsync(message.Chat.Id, "Чтобы выполнять основные команды вернитесь в главное меню!");
                        await client.SendTextMessageAsync(message.Chat.Id, "Проверять подписку на Ваш аккаунт?", replyMarkup: Keyboards.CompetAuthKeyboard());
                    }
                    else
                    {
                        await client.DeleteMessageAsync(message.Chat.Id, message.MessageId - 1);
                        await client.SendTextMessageAsync(message.Chat.Id, "Пожалуйста, выберите пункт меню");
                        await client.SendTextMessageAsync(message.Chat.Id, "Проверять подписку на Ваш аккаунт?", replyMarkup: Keyboards.CompetAuthKeyboard());
                    }
                    dataBase.ChangeState(message.Chat.Id, Constants.STATE_INCOMPET_CHOOSE_AUTH);
                    break;
                case Constants.STATE_INCOMPET_LOGIN:
                    if (message.Text.StartsWith('/'))
                    {
                        await client.DeleteMessageAsync(message.Chat.Id, message.MessageId - 1);
                        await client.SendTextMessageAsync(message.Chat.Id, "Чтобы выполнять основные команды вернитесь в главное меню!");
                        await client.SendTextMessageAsync(message.Chat.Id, "Введите Ваш логин:", replyMarkup: Keyboards.BackKeyboard());
                        dataBase.ChangeState(message.Chat.Id, Constants.STATE_INCOMPET_LOGIN);
                    }
                    else
                    {
                        string userlogin = message.Text;

                        if (userlogin.IsValidInstagramUsername())
                        {
                            userlogin = userlogin.ValidateInstagramUsername();

                            dataBase.InputCompetUserLogin(message.Chat.Id, userlogin);

                            try { await client.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId - 1, null); } catch { }
                            await client.SendTextMessageAsync(message.Chat.Id, $"Ваш логин: {userlogin}\nВведите ссылку на пост-розыгрыш:", replyMarkup: Keyboards.BackKeyboard());

                            dataBase.ChangeState(message.Chat.Id, Constants.STATE_INCOMPET_POSTLINK);
                        }
                        else
                        {
                            try { await client.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId - 1, null); } catch { }
                            await client.SendTextMessageAsync(message.Chat.Id, "Некорректный ввод!");
                            await client.SendTextMessageAsync(message.Chat.Id, "Введите Ваш логин:", replyMarkup: Keyboards.BackKeyboard());

                            dataBase.ChangeState(message.Chat.Id, Constants.STATE_INCOMPET_LOGIN);
                        }
                    }
                    break;
                case Constants.STATE_INCOMPET_POSTLINK:
                    if (message.Text.StartsWith('/'))
                    {
                        await client.DeleteMessageAsync(message.Chat.Id, message.MessageId - 1);
                        await client.SendTextMessageAsync(message.Chat.Id, "Чтобы выполнять основные команды вернитесь в главное меню!");
                        await client.SendTextMessageAsync(message.Chat.Id, "Введите ссылку на пост-розыгрыш:", replyMarkup: Keyboards.BackKeyboard());
                        dataBase.ChangeState(message.Chat.Id, Constants.STATE_INCOMPET_POSTLINK);
                    }
                    else
                    {
                        string postlink = message.Text;

                        if (postlink.IsValidInstagramPostlink())
                        {
                            List<string> accounts = new List<string>();
                            string winner;
                            postlink = postlink.ValidateInstagramPostlink();

                            dataBase.InputCompetPostlink(message.Chat.Id, postlink);

                            try { await client.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId - 1, null); } catch { }
                            await client.SendTextMessageAsync(message.Chat.Id, "Данные получены!\nРаботаю, подождите около 5 минут");

                            lock(sync)
                            {
                                using (SeleniumWorker browser = new SeleniumWorker(dataBase))
                                {
                                    accounts = browser.Competition(message.Chat.Id, out winner);
                                }
                            }

                            if (accounts.Count == 0)
                            {
                                await client.SendTextMessageAsync(message.Chat.Id, "Участников не найдено! Проверьте корректность данных");
                            }
                            else
                            {
                                string filePath = $"{Directory.GetCurrentDirectory()}/{message.Chat.Id}_link_inputCompet.txt";

                                using (StreamWriter sw = new StreamWriter(filePath))
                                {
                                    sw.WriteLine("Competition results:");

                                    foreach (string acc in accounts)
                                    {
                                        sw.WriteLine(acc);
                                    }
                                }

                                using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
                                {
                                    InputOnlineFile file = new InputOnlineFile(fileStream);
                                    file.FileName = $"compet_members.txt";

                                    await client.SendDocumentAsync(message.Chat.Id, file);
                                }

                                System.IO.File.Delete(filePath);

                                if (!string.IsNullOrEmpty(winner))
                                {
                                    await client.SendTextMessageAsync(message.Chat.Id, $"Побеждает {winner}!");
                                }
                                else
                                {
                                    await client.SendTextMessageAsync(message.Chat.Id, "Победитель не определен, никто из участников не подписан на Ваш аккаунт!");
                                }
                            }

                            await client.SendTextMessageAsync(message.Chat.Id, "Введите /start чтобы начать работу со мной");

                            dataBase.ChangeState(message.Chat.Id, Constants.STATE_START);
                        }
                        else
                        {
                            try { await client.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId - 1, null); } catch { }
                            await client.SendTextMessageAsync(message.Chat.Id, "Некорректный ввод!");
                            await client.SendTextMessageAsync(message.Chat.Id, "Введите ссылку на пост-розыгрыш:", replyMarkup: Keyboards.BackKeyboard());

                            dataBase.ChangeState(message.Chat.Id, Constants.STATE_INCOMPET_POSTLINK);
                        }
                    }
                    break;
                case Constants.STATE_DONT_LISTENING:
                    try { await client.DeleteMessageAsync(message.Chat.Id, message.MessageId); } catch { }
                    break;
                default:
                    break;
            }
        }
    }
}
