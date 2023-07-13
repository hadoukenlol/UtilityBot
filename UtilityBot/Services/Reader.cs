using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace UtilityBot.Services
{
    internal class Reader : IStringReader
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IStorage _memoryStorage;
        public Reader(ITelegramBotClient telegramBotClient, IStorage memoryStorage) {
            _telegramBotClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public void Process(Message message)
        {
            string questType = _memoryStorage.GetSession(message.Chat.Id).QuestType;
            Console.WriteLine(questType);
        }
    }
}
