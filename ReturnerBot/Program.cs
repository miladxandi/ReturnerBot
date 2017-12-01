using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;

namespace ReturnerBot
{
    class Program
    {
        private static string fileName = "";
        private static string image_save_url = $"C://robot//{fileName}.png";
        private static string video_save_url = $"C://robot//{fileName}.mp4";
        private static string music_save_url = $"C://robot//{fileName}.mp3";
        private static string ogg_save_url = $"C://robot//{fileName}.ogg";
        private static string gif_save_url = $"C://robot//{fileName}.gif";
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
                //Console.WriteLine("Please enter your API: ");
                //string API = Console.ReadLine();
                var Bot = new TelegramBotClient("486675268:AAE7jCcfI6FNgS57lS39t1BBqb6lVNZhP2E");
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
                        if (update.Message.Text != null)
                        {
                            Console.WriteLine($"{update.Message.From.FirstName} {update.Message.From.LastName} ({update.Message.From.Username}): {update.Message.Text}");
                            if (update.Message.Chat.Type != Telegram.Bot.Types.Enums.ChatType.Private)
                            {
                                continue;
                            }
                            else if (update.Message.Text.Contains("/start"))
                            {
                                await Bot.SendTextMessageAsync(chatId: ChatId, replyToMessageId: MessageId, text: $@"سلام {update.Message.Chat.FirstName}, به روبات انتشار سرویس ابری ما خوش آمدی!
این روبات قابلیت دریافت و ذخیره، ارسال و بارگذاری و برخی فعالیت های جانبی دیگر مانند:
        1.تولید کد
        2.بازگشت دادن انواع پرونده
        3.تولید متن هایپرلینک
        4.ویرایش محتوا
را دارا میباشد.
برای شروع 'help' را تایپ کنید.");

                            }

                            else if ((update.Message.Text.Contains("http") ||
                                update.Message.Text.Contains("https") ||
                                update.Message.Text.Contains(".ir") ||
                                update.Message.Text.Contains("https") ||
                                update.Message.Text.Contains(".com") ||
                                update.Message.Text.Contains(".net") ||
                                update.Message.Text.Contains("link") ||
                                update.Message.Text.Contains("help") ||
                                update.Message.Text.Contains("www") ||
                                update.Message.Text.Contains(".org") ||
                                update.Message.Text.Contains("code")
                                ))
                            {
                                var Message = update.Message.Text;
                                if (Message.Contains("[") || Message.Contains("]") || Message.Contains("(") || Message.Contains(")") || Message.Contains(":") || Message.Contains(";") || Message.Contains("@") || Message.Contains("*") || Message.Contains("_"))
                                {
                                    await Bot.SendTextMessageAsync(chatId: ChatId, replyToMessageId: MessageId, text: Message, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                                    continue;
                                }
                                else
                                {
                                    await Bot.SendTextMessageAsync(chatId: ChatId, replyToMessageId: MessageId, text: @"لطفا برای استفاده از امکانات توسعه دهندگان از این الگو استفاده کنید:
                                *متن بلد*
                                _متن ایتالیک_
                                [متن](http://www.example.com/)
                                [متن](tg://user?id=123456789)
                                `کد`
                                ```بلاک زبان
                                کد پیش تنظیم شده Fixed Width
                                ```");
                                }
                            }
                            else
                            {
                                Console.Write("You: ");
                                //string Message = Console.ReadLine();
                                var Message = update.Message.Text;
                                await Bot.SendTextMessageAsync(chatId: ChatId, replyToMessageId: MessageId, text: " متاسفانه هنوز نوشتاری با دستور " + Message + " برای روبات تعریف نشده است.");
                                continue;
                            }
                        }
                        else if (update.Message.Photo != null)
                        {
                            {
                                var FileId = update.Message.Photo.LastOrDefault().FileId;
                                var Caption = update.Message.Caption;
                                fileName = update.Message.Photo.LastOrDefault().FileSize.ToString();
                                var file = await Bot.GetFileAsync(fileId: FileId);

                                using (var saveFile = System.IO.File.Open(image_save_url, FileMode.OpenOrCreate))
                                {
                                    await file.FileStream.CopyToAsync(saveFile);
                                    Console.WriteLine($"New file has been received with {fileName}.png filename.");
                                    await Bot.SendTextMessageAsync(chatId: ChatId, text: $"پرونده ی شما دریافت شد و با نام {fileName}.png در ابر ذخیره شد، منتظر بازگشت آن باشید ...");
                                }
                                using (var stream = System.IO.File.Open(image_save_url, FileMode.OpenOrCreate))
                                {
                                    Console.WriteLine($"{fileName}.png is uploading...");
                                    await Bot.SendPhotoAsync(chatId: ChatId, photo: new Telegram.Bot.Types.FileToSend($"{fileName}", stream), replyToMessageId: MessageId);
                                }
                            }
                        }
                        else if (update.Message.Video != null)
                        {
                            {
                                var FileId = update.Message.Video.FileId;
                                fileName = update.Message.Video.FileSize.ToString();
                                var file = await Bot.GetFileAsync(fileId: FileId);

                                using (var saveFile = System.IO.File.Open(video_save_url, FileMode.OpenOrCreate))
                                {
                                    await file.FileStream.CopyToAsync(saveFile);
                                    Console.WriteLine($"New file has been received with {fileName}.mp4 filename.");
                                    await Bot.SendTextMessageAsync(chatId: ChatId, text: $"پرونده ی شما دریافت شد و با نام {fileName}.mp4 در ابر ذخیره شد، منتظر بازگشت آن باشید ...");
                                }
                                using (var stream = System.IO.File.Open(video_save_url, FileMode.OpenOrCreate))
                                {
                                    Console.WriteLine($"{fileName}.mp4 is uploading...");
                                    await Bot.SendVideoAsync(chatId: ChatId, video: new Telegram.Bot.Types.FileToSend($"{fileName}", stream), replyToMessageId: MessageId);
                                }
                            }
                        }
                        else if (update.Message.Voice != null)
                        {
                            {
                                var FileId = update.Message.Voice.FileId;
                                var file = await Bot.GetFileAsync(fileId: FileId);
                                fileName = update.Message.Voice.FileSize.ToString();
                                using (var saveFile = System.IO.File.Open(ogg_save_url, FileMode.OpenOrCreate))
                                {
                                    await file.FileStream.CopyToAsync(saveFile);
                                    Console.WriteLine($"New file has been received with {fileName}.ogg filename.");
                                    await Bot.SendTextMessageAsync(chatId: ChatId, text: $"پرونده ی شما دریافت شد و با نام {fileName}.ogg در ابر ذخیره شد، منتظر بازگشت آن باشید ...");
                                }
                                using (var stream = System.IO.File.Open(ogg_save_url, FileMode.OpenOrCreate))
                                {
                                    Console.WriteLine($"{fileName}.ogg is uploading...");
                                    await Bot.SendVoiceAsync(chatId: ChatId, voice: new Telegram.Bot.Types.FileToSend($"{fileName}", stream), replyToMessageId: MessageId);
                                }
                            }
                        }
                        else if (update.Message.Audio != null)
                        {
                            {
                                var FileId = update.Message.Audio.FileId;
                                var Title = update.Message.Audio.Title;
                                var Duration = update.Message.Audio.Duration;
                                var Caption = "Created by: @eastPublishBot";
                                var Performer = update.Message.Audio.Performer;
                                fileName = $"{Title} - {Performer} -{Duration}";
                                var file = await Bot.GetFileAsync(fileId: FileId);

                                using (var saveFile = System.IO.File.Open(music_save_url, FileMode.OpenOrCreate))
                                {
                                    await file.FileStream.CopyToAsync(saveFile);
                                    Console.WriteLine($"New file has been received with {fileName} filename.");
                                    await Bot.SendTextMessageAsync(chatId: ChatId, text: $"پرونده ی شما دریافت شد و با نام {fileName}.mp3 در ابر ذخیره شد، منتظر بازگشت آن باشید ...");
                                }
                                using (var stream = System.IO.File.Open(music_save_url, FileMode.OpenOrCreate))
                                {
                                    Console.WriteLine($"{fileName}.mp3 is uploading...");
                                    await Bot.SendAudioAsync(chatId: ChatId, title: Title, duration: Duration, performer: Performer, caption: Caption, audio: new Telegram.Bot.Types.FileToSend($"{fileName}", stream), replyToMessageId: MessageId);
                                }
                            }
                        }
                        else if (update.Message.Document != null && update.Message.Document.MimeType == "video/mp4")
                        {
                            {
                                var FileId = update.Message.Document.FileId;
                                fileName = update.Message.Document.FileSize.ToString();
                                var file = await Bot.GetFileAsync(fileId: FileId);

                                using (var saveFile = System.IO.File.Open(gif_save_url, FileMode.OpenOrCreate))
                                {
                                    await file.FileStream.CopyToAsync(saveFile);
                                    Console.WriteLine($"New file has been received with {fileName}.gif filename.");
                                    await Bot.SendTextMessageAsync(chatId: ChatId, text: $"پرونده ی شما دریافت شد و با نام {fileName}.gif در ابر ذخیره شد، منتظر بازگشت آن باشید ...");
                                }
                                using (var stream = System.IO.File.Open(gif_save_url, FileMode.OpenOrCreate))
                                {
                                    Console.WriteLine($"{fileName}.gif is uploading...");
                                    await Bot.SendDocumentAsync(chatId: ChatId, document: new Telegram.Bot.Types.FileToSend($"{fileName}", stream), replyToMessageId: MessageId);
                                }
                            }
                        }
                        else
                        {
                            await Bot.SendTextMessageAsync(chatId: ChatId, text: $@"{update.Message.Chat.FirstName} عزیز؛
ما نتوانستیم پرونده ی ارسالی شما را پردازش کنیم، این موضوع مشخصا به دلیل فرمت آن است.
در آینده مشکل را حل خواهیم کرد!");
                        }
                    }
                }
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException error)
            {
                Console.WriteLine($"{error.Message} , we`ll reset for you in 5 seconds.");
                System.Threading.Thread.Sleep(5000);
                Console.Clear();
                var t = Task.Run(() => RunBot());
            }
            catch (NullReferenceException error)
            {
                Console.WriteLine($"{error.Message} , we`ll reset for you in 5 seconds.");
                System.Threading.Thread.Sleep(5000);
                Console.Clear();
                var t = Task.Run(() => RunBot());
            }
            catch (ArgumentNullException error)
            {
                Console.WriteLine($"{error.Message} , we`ll reset for you in 5 seconds.");
                System.Threading.Thread.Sleep(5000);
                Console.Clear();
                var t = Task.Run(() => RunBot());
            }
            catch (Exception error)
            {
                Console.WriteLine($"{error.Message} , we`ll reset for you in 5 seconds.");
                System.Threading.Thread.Sleep(5000);
                Console.Clear();
                var t = Task.Run(() => RunBot());
            }
        }
    }
}
