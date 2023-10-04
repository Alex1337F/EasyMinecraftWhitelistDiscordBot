using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using EasyDiscordWhitelister.config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDiscordWhitelister.Commands
{
    

    public class EasyWhitelist : ApplicationCommandModule
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        [SlashCommand("Whitelist", "Grants the selected user the member role and whitelists the person.")]

        public async Task Accept(InteractionContext ctx, [Option("Discord-username", "Enter the Discord User")] DiscordUser username, [Option("MC-username", "Enter the Minecraft Username to whitelist")] string MCnamn)
        {

            try
            {
                ConfigJSON configJSON = new ConfigJSON();
                await configJSON.ReadJSON();
                var guild = ctx.Guild;
                var member = await guild.GetMemberAsync(username.Id);
                var role = guild.GetRole(configJSON.MemberRoleID);
                var adminRole = guild.GetRole(configJSON.AdminRoleID);

                //Check if the user has permission to run the command
                if (!ctx.Member.Roles.Contains(adminRole))
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder()
                            .WithContent("You do not have permission to use this command!"));
                    return;
                }

                if (role != null)
                {
                    await member.GrantRoleAsync(role);

                    try
                    {
                        await member.SendMessageAsync(configJSON.WelcomeDM);
                        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                             .WithContent($"{member.Username} has successfully been whitelisted. "));
                    }
                    catch (UnauthorizedException)
                    {
                        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                            .WithContent($"The bot was not able to send {member.Username} a DM. The user most likely has their DMs disabled."));
                    }

                    var welcomeChannel = guild.GetChannel(configJSON.PublicWelcomeChannelID);
                    if (welcomeChannel != null)
                    {
                        string message = configJSON.PublicWelcomeMessage;
                        string WelcomeMessage = message.Replace("{member}", $"{member.Mention}");
                        await welcomeChannel.SendMessageAsync(WelcomeMessage);
                    }

                    // Pterodactyl API call to run whitelist command
                    var apiUrl = configJSON.PterodactylAPIAdress;
                    var apiKey = configJSON.PterodactylAPIKey;
                    var serverId = configJSON.PterodactylServerID;
                    var command = $"whitelist add {MCnamn}";
                    var requestBody = new StringContent(JsonConvert.SerializeObject(new { command }), Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage(HttpMethod.Post, $"{apiUrl}/client/servers/{serverId}/command") { Content = requestBody };
                    request.Headers.Add("Authorization", $"Bearer {apiKey}");
                    var response = await _httpClient.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                    await ctx.Channel.SendMessageAsync($"Error running command on server. Status code: {response.StatusCode}");
                        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent($"Something went wrong when trying to whitelist."));
                    }
                }
                else
                {
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                        .WithContent($"The role was not able to be found. Make sure to put in the right role ID in the config."));
                }
            }
            catch (Exception ex)
            {
               await ctx.Channel.SendMessageAsync(ex.Message);
            }


        }


    }
}
