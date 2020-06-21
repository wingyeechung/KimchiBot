using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using Newtonsoft.Json;

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

            string orderedFood = "";
            string cookFood = "";
            switch (msg.Message.Content.ToLower())
            {
                case "order":
                case "order food":
                    await ctx.RespondAsync($"What would you like to order?\nBlack Bean Noodles\nStrawberry Cake\nFried Chicken\nVanilla Icecream");
                    orderedFood = (await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id, TimeSpan.FromMinutes(1))).Message.Content;
                    break;
                case "cook":
                case "cook food":
                    await ctx.RespondAsync($"What would you like to cook?\nBeef and Rice\nFried Rice\nKimchi\nOmelet\nPasta");
                    cookFood = (await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id, TimeSpan.FromMinutes(1))).Message.Content;
                    break;
            }

            switch (orderedFood.ToLower())
            {
                case "black bean noodles":
                    await ctx.RespondAsync($"Black Bean Noodles has been ordered!");
                    await ctx.RespondAsync($"https://i.pinimg.com/originals/5e/f9/d7/5ef9d70b03d22b1acd145b9422d3a33f.gif");
                    break;
                case "strawberry cake":
                    await ctx.RespondAsync($"Strawberry Cake has been ordered!");
                    await ctx.RespondAsync($"https://i.pinimg.com/originals/5e/f9/d7/5ef9d70b03d22b1acd145b9422d3a33f.gif");
                    break;
                case "fried chicken":
                    await ctx.RespondAsync($"Fried Chicken has been ordered!");
                    await ctx.RespondAsync($"https://i.pinimg.com/originals/5e/f9/d7/5ef9d70b03d22b1acd145b9422d3a33f.gif");
                    break;
                case "vanilla icecream":
                    await ctx.RespondAsync($"Vanilla Icecream has been ordered!");
                    await ctx.RespondAsync($"https://i.pinimg.com/originals/5e/f9/d7/5ef9d70b03d22b1acd145b9422d3a33f.gif");
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

        //Bank System Command
   
        [Command("bank")]
        public async Task BankSystem(CommandContext ctx)
        {
            Dictionary<string, UserData> userDictionary = new Dictionary<string, UserData>();
            if (File.Exists("userData.json"))
            {
                userDictionary = JsonConvert.DeserializeObject<Dictionary<string, UserData>>(File.ReadAllText("userData.json"));
            }

            //Dictionary<string, UserData> userDictionary = JsonConvert.DeserializeObject<Dictionary<string, UserData>>(File.ReadAllText("userData.json"));
            await ctx.RespondAsync($"Welcome to Kimchi Bank! {ctx.User.Username} ＼（＾○＾）人（＾○＾）／\n");
            var interactivity = ctx.Client.GetInteractivityModule();
            //var msg = await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id, TimeSpan.FromMinutes(1));
            
            //Checks if userID exists in json file, if not it creates a bankaccount for user with userid as key
            if (userDictionary.ContainsKey(ctx.User.Id.ToString()))
            {
                await ctx.RespondAsync($"Please `-type-` your choice\n`dep` to deposit money\n`bal` to check balance");
                var response = await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id, TimeSpan.FromMinutes(1));
                //deposit money
                if (response.Message.Content.ToLower() == "dep")
                {
                    await ctx.RespondAsync($"Please enter amount");
                    var dep = await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id, TimeSpan.FromMinutes(1));
                    userDictionary[ctx.User.Id.ToString()].Balance += Int32.Parse(dep.Message.Content);
                    await ctx.RespondAsync($"{Int32.Parse(dep.Message.Content)} coins deposited");
                    File.WriteAllText("userData.json", JsonConvert.SerializeObject(userDictionary));
                }
                //checks balance
                else if (response.Message.Content.ToLower() == "bal")
                {
                    await ctx.RespondAsync($"Your balance is {userDictionary[ctx.User.Id.ToString()].Balance}"); // balance check
                }   
            }
            else
            {
                await ctx.RespondAsync($"You do not have a bankaccount. Type `yes` to open one");
                var response = await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id, TimeSpan.FromMinutes(1));
                if (response.Message.Content.ToLower() == "yes")
                    AddUser(ctx, userDictionary);
                    await ctx.RespondAsync($"Thank You for joining Kimchi Bank `{ctx.User.Username}`");
            }

            //Adds user to json file
            static void AddUser(CommandContext ctx, Dictionary<string, UserData> userDictionary)
            {

/*                var user = new Dictionary<string, UserData>()
                {
                    {ctx.User.Id.ToString(), new UserData {Name = ctx.User.Username.ToString(), Balance = 0, Food = ""}},
                };*/
                userDictionary.Add(ctx.User.Id.ToString(), new UserData { Name = ctx.User.Username.ToString(), Balance = 0, Food = "" });
                /*File.WriteAllText("userData.json", JsonConvert.SerializeObject(user));*/
                File.WriteAllText("userData.json", JsonConvert.SerializeObject(userDictionary));

                /*using (StreamWriter w = File.AppendText("userData.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(w, user);
                    w.Close();
                }*/

                //{"476690044271460352":{"Name":"Wings","Balance":42342,"Food":""}}
            }
        }
    }
}
