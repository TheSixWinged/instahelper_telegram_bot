using Telegram.Bot.Types.ReplyMarkups;

namespace InstaHelper.Bot
{
    static class Keyboards
    {
        public static InlineKeyboardMarkup StartKeyboard()
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Парсить аудиторию","choice_parser"),
                },

                new []
                {
                    InlineKeyboardButton.WithCallbackData("Провести конкурс","choice_compet"),
                },

                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Выход","back"),
                },

            });

            return keyboard;
        }

        public static InlineKeyboardMarkup CompetUniqueKeyboard()
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Да","no_compet_unique"),
                    InlineKeyboardButton.WithCallbackData("Нет","yes_compet_unique"),
                },

                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Назад","back"),
                },

            });

            return keyboard;
        }

        public static InlineKeyboardMarkup CompetAuthKeyboard()
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    //InlineKeyboardButton.WithCallbackData("Да","dev_compet_auth"),
                    InlineKeyboardButton.WithCallbackData("Да","yes_compet_auth"),
                    InlineKeyboardButton.WithCallbackData("Нет","no_compet_auth"),
                },

                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Назад","back"),
                },

            });

            return keyboard;
        }

        public static InlineKeyboardMarkup BackKeyboard()
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithCallbackData("Назад","back"),
                },
            });

            return keyboard;
        }

        public static InlineKeyboardMarkup LinkGitHubKeyboard()
        {
            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithUrl("My GitHub", "https://github.com/TheSixWinged"),
                },
            });

            return keyboard;
        }
    }
}
