using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using EasyDiscordWhitelister.Commands;
using EasyDiscordWhitelister.config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EasyDiscordWhitelister
{
    internal class Program
    {
        private static DiscordClient Client { get; set; }
        private static CommandsNextExtension Commands { get; set; }
        public static async Task Main(string[] args)
        {
            var configJSON = new ConfigJSON();
            await configJSON.ReadJSON();
            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = configJSON.BotToken,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };
            Client = new DiscordClient(discordConfig);
            Client.Ready += Client_Ready;
            commandsreg();
            await Client.ConnectAsync();
            await Task.Delay(-1);
        }
        private static void commandsreg()
        {
            var commands = Client.UseSlashCommands();
            commands.RegisterCommands<EasyWhitelist>();
        }
        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask; 
        }
    }
    
}