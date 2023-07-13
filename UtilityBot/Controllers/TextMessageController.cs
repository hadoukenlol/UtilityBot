using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using System.Threading;

namespace UtilityBot.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;
        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }
        public async Task Handle(Message message, CancellationToken ct)
        {
            string questType = _memoryStorage.GetSession(message.Chat.Id).QuestType;
            switch (message.Text)
            {
                case "/start":
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Кол-во символов в строке" , $"length"),
                        InlineKeyboardButton.WithCallbackData($" Сумму цифр" , $"count")
                    });
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Этот бот считает сумму цифр или кол-во символов в строке.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}Если совсем делать нечего, можно поиграться.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
                default:
                    StringReg(message, questType, ct);
                    break;
            }
            
        }

        public async void StringReg(Message message, string questType, CancellationToken ct)
        {
            switch (questType)
            {
                case "length":
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Длина сообщения: {message.Text.Length} знаков", cancellationToken: ct);
                    break;
                case "count":
                    Calc(message, ct);
                    break;
                default:
                    break;
            }
        }
        public async void Calc(Message message, CancellationToken ct)
        {
            int res = 0;
            try
            {
                string s = message.Text.ToString();
                string[] subs = s.Split(';', '.', ' ', ','); 
                foreach (var sub in subs) {
                    int number = Convert.ToInt32(sub);
                    res += number;
                }
                Console.WriteLine(res);
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Сумма чисел {res}", cancellationToken: ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка {ex.Message}");
            }            
        }
    }
}
