using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace ReturnerBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = Task.Run(() => RunBot());
            Console.ReadKey();
        }
        static async Task RunBot()
        {
            Console.WriteLine("Bot is initializing...");
            string API = "";
            var Bot = new TelegramBotClient(API);
            var Me = await Bot.GetMeAsync();
            Console.WriteLine($"{Me.Username}");
            Console.WriteLine($"{Me.Username} HashSet been initialized.");
            int offset = 0;
            while (true)
            {


                var updates = await Bot.GetUpdatesAsync(offset);

                foreach (var update in updates)
                {
                    offset = update.Id + 1;
                    var ChatId = update.Message.Chat.Id;
                    var Message = update.Message.Text;
                    await Bot.SendTextMessageAsync(chatId: ChatId, text: Message);
                    continue;
                }
            }
            
        }
    }
}
