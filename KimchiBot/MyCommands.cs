using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;

namespace KimchiBot
{
    public class MyCommands
    {
        static MyCommands()
        {
            if (File.Exists("userData.json"))
                UserDict = JsonConvert.DeserializeObject<Dictionary<string, UserData>>(File.ReadAllText("userData.json"));
        }

        public static Dictionary<string, UserData> UserDict { get; set; } = new Dictionary<string, UserData>();

        //Help Command
        [Command("kimchi!")]
        public async Task Help(CommandContext ctx)
        {
            DiscordEmbedBuilder b = new DiscordEmbedBuilder()
            {
                Color = new DiscordColor(12742030),
                Title = "Kimchi Command List",
                Description = "all available commands"
            };
            b.AddField("menu", "to order or cook food");
            b.AddField("bank", "to deposit money or check balance");
            await ctx.RespondAsync(embed: b);
        }

        /// <summary>
        /// Waits 5 seconds before responding with a users' food order.
        /// </summary>
        /// <param name="ctx">Discord Command Context</param>
        public static void FoodDeliveryTimer(CommandContext ctx, DiscordMessage msg, string foodOrder)
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            // Set the callback for when the timer reaches 0
            aTimer.Elapsed += async (sender, e) => await ModifyOrder(ctx, msg, foodOrder);
            aTimer.Interval = 5000;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }

        /// <summary>
        /// Sets the order of the user that invoked this command to the given <paramref name="foodOrder">.
        /// </summary>
        /// <param name="ctx">The discord command context for the command</param>
        public static async Task ModifyOrder(CommandContext ctx, DiscordMessage msg, string foodOrder)
        {
            await msg.ModifyAsync($"`Thank You for using Kimchi service (ﾉ◕ヮ◕)ﾉ*:･ﾟ✧`");

            UserData user = UserDict[ctx.User.Id.ToString()];
            user.Food.Add(foodOrder);
        }

        [Command("Food")]
        public async Task Use(CommandContext ctx)
        {
            // Create a string with all foodOrders on a new line where each foodOrder is surrounded by backticks (`)
            string options = string.Join("\n", UserDict[ctx.User.Id.ToString()].Food.Select(x => $"`{x}`"));
            await ctx.RespondAsync($"What would you like to eat?\n{options}");
            var interactivity = ctx.Client.GetInteractivityModule();
            var msg = await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id, TimeSpan.FromMinutes(1));

            UserData user = UserDict[ctx.User.Id.ToString()];

            // Checks if any foodOrders equal the message
            string food = user.Food.FirstOrDefault(x => x.ToLower() == msg.Message.Content.ToString().ToLower());
            if (!(food is null))
            {
                user.Food.Remove(food);
                await ctx.RespondAsync($"Enjoy your {food} (ﾉ◕ヮ◕)ﾉ*:･ﾟ✧`");
            }
        }

        public static async Task Payment(string link, string orderedFood, string name, CommandContext ctx,int coins)
        {
            if(UserDict[ctx.User.Id.ToString()].Balance > coins)
            {
                UserDict[ctx.User.Id.ToString()].Balance -= coins;
                await ctx.RespondAsync($"{name} has been ordered!\n");
                DiscordMessage bbnmsg = await ctx.RespondAsync("`ಠ_ಠ Open your door in 5 seconds`");
                await ctx.RespondAsync($"{link}");
                FoodDeliveryTimer(ctx, bbnmsg, orderedFood);
            }
            else
            {
                await ctx.RespondAsync($"`Order Failed. Insufficient coins`");
            }

        }
        //Menu list Command
        [Command("menu")]
        public async Task Menu(CommandContext ctx)
        {
            await ctx.RespondAsync($"What would you like to do?\norder food\ncook food");

            var interactivity = ctx.Client.GetInteractivityModule();
            var msg = await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id, TimeSpan.FromMinutes(1));

            string orderedFood = "";
            string cookFood = "";
            switch (msg.Message.Content.ToLower())
            {
                case "order":
                case "order food":
                    await ctx.RespondAsync($"What would you like to order?\n`25 coins - Black Bean Noodles`\n`10 coins - Strawberry Cake`\n`17 coins - Fried Chicken`\n`10 coins - Vanilla Icecream`");
                    orderedFood = (await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id, TimeSpan.FromMinutes(1))).Message.Content;
                    break;
                case "cook":
                case "cook food":
                    await ctx.RespondAsync($"What would you like to cook?\n`40 coins - Beef and Rice`\n`13 coins - Fried Rice`\n`10 coins - Kimchi`\n`12 coins - Omelet`\n`15 coins - Pasta`");
                    cookFood = (await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id, TimeSpan.FromMinutes(1))).Message.Content;
                    break;
            }

            switch (orderedFood.ToLower())
            {
                case "black bean noodles":
                    string name = "Black Bean Noodles";
                    string link = $"https://i.pinimg.com/originals/5e/f9/d7/5ef9d70b03d22b1acd145b9422d3a33f.gif";
                    await Payment(link, orderedFood, name, ctx, 25);
                    break;
                case "strawberry cake":
                    name = "Strawberry Cake";
                    link = $"https://i.pinimg.com/originals/5e/f9/d7/5ef9d70b03d22b1acd145b9422d3a33f.gif";
                    await Payment(link, orderedFood, name, ctx, 10);
                    break;
                case "fried chicken":
                    name = "Fried Chicken";
                    link = $"https://i.pinimg.com/originals/5e/f9/d7/5ef9d70b03d22b1acd145b9422d3a33f.gif";
                    await Payment(link, orderedFood, name, ctx, 17);
                    break;
                case "vanilla icecream":
                    name = "Vanilla Icecream";
                    link = $"https://i.pinimg.com/originals/5e/f9/d7/5ef9d70b03d22b1acd145b9422d3a33f.gif";
                    await Payment(link, orderedFood, name, ctx, 10);
                    break;
            }

            switch (cookFood.ToLower())
            {
                case "beef and rice":
                    await ctx.RespondAsync($"Beef and Rice has been cooked!");
                    await ctx.RespondAsync($"https://i.pinimg.com/originals/bd/24/dd/bd24dd4d0c982ebe3f19d7fc0ca4e348.gif");
                    break;
                case "fried rice":
                    await ctx.RespondAsync($"Fried Rice has been cooked!");
                    await ctx.RespondAsync($"https://i.pinimg.com/originals/05/a6/ac/05a6ac3663cb807556aebd61ebf77761.gif");
                    break;
                case "kimchi":
                    await ctx.RespondAsync($"Kimchi has been cooked!");
                    await ctx.RespondAsync($"https://vignette.wikia.nocookie.net/ramen-daisuki-koizumi-san/images/6/63/Rare-cheese-anime.jpg/revision/latest?cb=20180111172736");
                    break;
                case "omelet":
                    await ctx.RespondAsync($"Omelet has been cooked!");
                    await ctx.RespondAsync($"https://thumbs.gfycat.com/FatalExhaustedBlackrussianterrier-size_restricted.gif");
                    break;
                case "pasta":
                    await ctx.RespondAsync($"Pasta has been cooked!");
                    await ctx.RespondAsync($"https://data.whicdn.com/images/280115726/original.gif");
                    break;
            }
        }

        //adds new user to json file
        public void AddUser(CommandContext ctx)
        {
            UserDict.Add(ctx.User.Id.ToString(), new UserData { Name = ctx.User.Username.ToString(), Balance = 0 });
        }

        //Bank System Command
        [Command("bank")]
        public async Task BankSystem(CommandContext ctx)
        {
            await ctx.RespondAsync($"Welcome to Kimchi Bank! {ctx.User.Username} ＼（＾○＾）人（＾○＾）／\n");
            var interactivity = ctx.Client.GetInteractivityModule();

            //Checks if userID exists in json file, if not it creates a bankaccount for user with userid as key
            if (UserDict.ContainsKey(ctx.User.Id.ToString()))
            {
                await ctx.RespondAsync($"Please `-type-` your choice\n`dep` to deposit money\n`bal` to check balance");
                var response = await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id, TimeSpan.FromMinutes(1));
                //deposit money
                if (response.Message.Content.ToLower() == "dep")
                {
                    await ctx.RespondAsync($"Please enter amount");
                    var dep = await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id, TimeSpan.FromMinutes(1));
                    if(Int32.Parse(dep.Message.Content) > 0)
                    {
                        unchecked { UserDict[ctx.User.Id.ToString()].Balance += Int32.Parse(dep.Message.Content); }
                        await ctx.RespondAsync($"{Int32.Parse(dep.Message.Content)} coins deposited");
                        File.WriteAllText("userData.json", JsonConvert.SerializeObject(UserDict));
                    }
                    else
                    {
                        await ctx.RespondAsync($"Invalid Amount");
                    }

                }
                //checks balance
                else if (response.Message.Content.ToLower() == "bal")
                {
                    await ctx.RespondAsync($"Your balance is {UserDict[ctx.User.Id.ToString()].Balance}"); // balance check
                }
            }
            else
            {
                await ctx.RespondAsync($"You do not have a bankaccount. Type `yes` to open one");
                var response = await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id, TimeSpan.FromMinutes(1));
                if (response.Message.Content.ToLower() == "yes")
                {
                    AddUser(ctx);
                    await ctx.RespondAsync($"Thank You for joining Kimchi Bank `{ctx.User.Username}`");
                }
                else
                {
                    await ctx.RespondAsync($"ಠ_ಠ Your loss `{ctx.User.Username}`");
                }
            }
        }
    }
}
