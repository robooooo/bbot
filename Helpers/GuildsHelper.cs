using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using Imgur.API.Authentication.Impl;
using DSharpPlus.CommandsNext;

namespace BBotCore
{
    // Update the member counts for bbot on topgg, discordbotslist, etc.
    public class GuildsHelper
    {
        private WebClient Client;
        private string TopGG;
        private string DiscordBots;
        private string DiscordBotList;

        // Not yet authenticated
        // private string BotsOnDiscord;
        public GuildsHelper()
        {
            Client = new WebClient();
            TopGG = Environment.GetEnvironmentVariable("TOP_GG");
            DiscordBots = Environment.GetEnvironmentVariable("DISCORD_BOTS");
            DiscordBotList = Environment.GetEnvironmentVariable("DISCORD_BOT_LIST");
            // BotsOnDiscord = Environment.GetEnvironmentVariable("DISCORD_BETA_TOKEN");
        }

        public void UpdateGuildsAsync(ulong id, int guildsCount)
        {
            try
            {   
                // Client.Headers[HttpRequestHeader.ContentType] = "application/json";

                // Not using asynchronous methods because no proper support for async in the WebClient.
                // Cannot start uploading second set of values until first is done.
                NameValueCollection values = new NameValueCollection() {{"server_count", $"{guildsCount}"}};
                Client.Headers[HttpRequestHeader.Host] = "top.gg";
                Client.Headers[HttpRequestHeader.Authorization] = TopGG;
                Client.UploadValues(new System.Uri($"https://top.gg/api/bots/{id}/stats"), "POST", values);

                values = new NameValueCollection() {{"guildCount", $"{guildsCount}"}};
                Client.Headers[HttpRequestHeader.Host] = "discord.bots.gg";
                Client.Headers[HttpRequestHeader.Authorization] = DiscordBots;
                Client.UploadValues(new System.Uri($"https://discord.bots.gg/api/v1/bots/{id}/stats"), "POST", values);
 
                values = new NameValueCollection() {{"guilds", $"{guildsCount}"}};
                Client.Headers[HttpRequestHeader.Host] = "discordbotlist.com";
                Client.Headers[HttpRequestHeader.Authorization] = DiscordBotList;
                Client.UploadValues(new System.Uri($"https://discordbotlist.com/api/v1/bots/{id}/stats"), "POST", values);


            }
            catch (WebException e)
            {
                Console.Write(e.Message);
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}