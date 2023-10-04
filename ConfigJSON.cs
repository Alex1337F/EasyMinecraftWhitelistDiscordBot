using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;

namespace EasyDiscordWhitelister.config
{
    internal class ConfigJSON
    {
        public string BotToken { get; set; }
        public ulong MemberRoleID { get; set; }
        public ulong AdminRoleID { get; set; }
        public ulong PublicWelcomeChannelID { get; set; }
        public string WelcomeDM { get; set; }
        public string PublicWelcomeMessage { get; set; }
        public string PterodactylAPIAdress { get; set; }
        public string PterodactylAPIKey { get; set; }
        public string PterodactylServerID { get; set; }

        public async Task ReadJSON()
        {
            using (StreamReader reader = new StreamReader("config.json"))
            {
                string json = await reader.ReadToEndAsync();
                JSONlayout data = JsonConvert.DeserializeObject<JSONlayout>(json);

                this.BotToken = data.BotToken;
                this.MemberRoleID = data.MemberRoleID;
                this.AdminRoleID = data.AdminRoleID;
                this.WelcomeDM = data.WelcomeDM;
                this.PublicWelcomeChannelID = data.PublicWelcomeChannelID;
                this.PublicWelcomeMessage = data.PublicWelcomeMessage;
                this.PterodactylAPIAdress = data.PterodactylAPIAdress;
                this.PterodactylAPIKey = data.PterodactylAPIKey;
                this.PterodactylServerID = data.PterodactylServerID;
            }
        }
    }

    internal sealed class JSONlayout
    {
        public string BotToken { get; set; }
        public ulong MemberRoleID { get; set; }
        public ulong AdminRoleID { get; set; }
        public string WelcomeDM { get; set; }
        public ulong PublicWelcomeChannelID { get; set; }
        public string PublicWelcomeMessage { get; set; }
        public string PterodactylAPIAdress { get; set; }
        public string PterodactylAPIKey { get; set; }
        public string PterodactylServerID { get; set; }
    }
}
