using System;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using Imgur.API;


namespace BBotCore
{
    // Abstraction to be used when managing a set of profile pictures through imgur
    // Provides caching and upload
    public class ImgurHelper
    {
        private ApiClient Client;
        private ImageEndpoint Endpoint;
        private Dictionary<ulong, string> Cache = new Dictionary<ulong, string>();
        public ImgurHelper(ApiClient client)
        {
            Client = client;
            Endpoint = new ImageEndpoint(client, new HttpClient());
        }

        public async Task<string> GetURLFromUser(DiscordUser user)
        {
            // Return from cache if cached
            if (Cache.ContainsKey(user.Id))
                return Cache[user.Id];

            string Url = await Upload(user.AvatarUrl);
            Cache[user.Id] = Url;
            return Url;
        }

        private async Task<string> Upload(string profileUrl)
        {

            var image = await Endpoint.UploadImageAsync(profileUrl);
            return image.Link;
        }
    }
}
