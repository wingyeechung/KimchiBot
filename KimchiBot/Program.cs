using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;


namespace KimchiBot
{
    class Program
    {
        static InteractivityModule interactivity;
        static CommandsNextModule commands;
        static DiscordClient discord;
        /// <summary>
        /// hello world
        /// </summary>
        /// <param name="args">arguments</param>
        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
         

            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = args[0],
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            interactivity = discord.UseInteractivity(new InteractivityConfiguration());

            //responds to message with kimchi
            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("kimchi"))
                    await e.Message.RespondAsync("hi, want sum Kimchi?");
            };

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("no ty"))
                    await e.Message.RespondAsync("get outta here!! ಠ_ಠ");
            };

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("yes plis"))
                    await e.Message.RespondAsync("Daily Menu\n```\nNoodles\nCookies```");
            };
            //command using prefix "pls"
            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = "pls"
            });
            commands.RegisterCommands<MyCommands>();

            await discord.ConnectAsync();
            await Task.Delay(-1);

        }
    }
}
