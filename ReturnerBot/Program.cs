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
            while (Console.ReadKey().KeyChar != '~') ;
        }
        static async Task RunBot()
        {

            try
            {
                Console.WriteLine("Bot is initializing...");
                if (Properties.Settings.Default.APIToken == "")
                {
                    //Console.WriteLine("Please enter your API: ");
                    //string rAPI = Console.ReadLine();
                    //string API = rAPI;
                    //Properties.Settings.Default.APIToken = rAPI;
                    var Bot = new TelegramBotClient("343174196:AAGRMRC1HTTdSymyL6pocecjw4XMm8AwuQw");
                    var Me = await Bot.GetMeAsync();
                    Console.WriteLine($"{Me.Username}");
                    Console.WriteLine($"{Me.Username} has been initialized.");
                    Console.WriteLine("\n");
                    System.Threading.Thread.Sleep(3000);
                    Console.Clear();
                    Console.WriteLine("Press '~' for exit.");
                    Console.WriteLine($"{Me.Username} is ready to receive message.\n");
                    int offset = 0;
                    while (true)
                    {
                        var updates = await Bot.GetUpdatesAsync(offset);
                        foreach (var update in updates)
                        {

                            offset = update.Id + 1;
                            var ChatId = update.Message.Chat.Id;
                            var MessageId = update.Message.MessageId;
                            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.MessageUpdate)
                            {
                                Console.WriteLine($"{update.Message.From.FirstName} {update.Message.From.LastName} ({update.Message.From.Username}): {update.Message.Text}");
                                if ((update.Message.Text.Contains("http") ||
                                    update.Message.Text.Contains("https") ||
                                    update.Message.Text.Contains(".ir") ||
                                    update.Message.Text.Contains("https") ||
                                    update.Message.Text.Contains(".com") ||
                                    update.Message.Text.Contains(".net")))
                                {
                                    var Message = update.Message.Text;
                                    if (Message.Contains("[")|| Message.Contains("]") || Message.Contains("(") || Message.Contains(")") || Message.Contains(":") || Message.Contains(";") || Message.Contains("@") || Message.Contains("*") || Message.Contains("_"))
                                    {
                                        await Bot.SendTextMessageAsync(chatId: ChatId, replyToMessageId: MessageId, text: Message, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                                        await Bot.SendTextMessageAsync(chatId: ChatId, replyToMessageId: MessageId, text: Message, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,disableWebPagePreview:true);
                                        continue;
                                    }
                                    else
                                    {
                                        await Bot.SendTextMessageAsync(chatId: ChatId, replyToMessageId: MessageId, text: @"لطفا برای استفاده از امکانات توسعه دهندگان از این الگو استفاده کنید:
*bold text*
_italic text_
[inline URL](http://www.example.com/)
[inline mention of a user](tg://user?id=123456789)
`inline fixed-width code`
```block_language
pre - formatted fixed-width code block
```");
                                        continue;
                                    }
                                }
                                else
                                {
                                    Console.Write("You: ");
                                    string Message = Console.ReadLine();
                                    //var Message = update.Message.Text;
                                    await Bot.SendTextMessageAsync(chatId: ChatId, replyToMessageId: MessageId, text: Message);
                                    continue;
                                }
                            }
                            else
                            {
                                await Bot.SendTextMessageAsync(chatId: ChatId, text: "ما تنها قادر به پاسخ به پیام های متنی شما هستیم.");
                                continue;
                            }
                        }
                    }
                }
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException error)
            {

                Console.WriteLine($"{error.Message} , we`ll reset for you in 5 seconds.");
                System.Threading.Thread.Sleep(5000);
                Console.Clear();
                Properties.Settings.Default.APIToken = "";
                var t = Task.Run(() => RunBot());
            }
        }
    }
}
