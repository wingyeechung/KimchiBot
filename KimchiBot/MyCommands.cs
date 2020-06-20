using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;

namespace KimchiBot
{
    public class MyCommands
    {
        //Greet Command
        [Command("greet")]
        public async Task Greet(CommandContext ctx)
        {
            await ctx.RespondAsync($"Hi, {ctx.User.Mention}!");

            var interactivity = ctx.Client.GetInteractivityModule();
            var msg = await interactivity.WaitForMessageAsync(xm => 
            xm.Author.Id == ctx.User.Id && xm.Content.ToLower() == "how are you?", TimeSpan.FromMinutes(1));
            if (msg != null)
                await ctx.RespondAsync($"I'm fine, thank you!");
        }
        //Menu list Command
        [Command("menu")]
        public async Task Menu(CommandContext ctx)
        {
            await ctx.RespondAsync($"What would you like to do?\norder food\ncook food");

            var interactivity = ctx.Client.GetInteractivityModule();
            var msg = await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id, TimeSpan.FromMinutes(1));
            switch (msg.Message.Content.ToLower())
            {
                case "order":
                case "order food":
                    await ctx.RespondAsync($"Your food is coming up!");
                    break;
                case "cook":
                case "cook food":
                    await ctx.RespondAsync($"Cooking food");
                    break;
            }
        }
    }
}
