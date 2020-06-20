using System;
using System.Threading.Tasks;
using DSharpPlus;

namespace KimchiBot
{
    class Program
    {
        static DiscordClient discord;
        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "NzIzOTIyOTcwNDc2ODA2MjQ0.Xu4r_w.VExP8zqCDFAXsp2RxYrJfiekxZY",
                TokenType = TokenType.Bot
            });

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("kimchi"))
                    await e.Message.RespondAsync("hi, want sum Kimchi?");
            };

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("pls no"))
                    await e.Message.RespondAsync("get outta here!! ಠ_ಠ");
            };

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
