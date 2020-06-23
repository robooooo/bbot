using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using ImgurSharp;

namespace BBotCore
{
    // Abstraction to be used when managing a set of profile pictures through imgur
    // Provides caching and upload
    public class ImgurHelper
    {
        private Imgur ImgurClient;
        private Dictionary<ulong, string> Cache = new Dictionary<ulong, string>();
        public ImgurHelper(Imgur client)
        {
            ImgurClient = client;
        }

        public async Task<string> GetURLFromUser(DiscordUser user)
        {
            // Return from cache if cached
            if (Cache.ContainsKey(user.Id))
                return Cache[user.Id];

            // Else add to cache and return
            string URL = await Upload(user.AvatarUrl);
            Cache[user.Id] = URL;
            return URL;
        }

        private async Task<string> Upload(string ProfileURL)
        {
            // Provide as little information as possible 
            ImgurImage img = await ImgurClient.UploadImageAnonymous(ProfileURL, "", "", "");
            return img.Link;
        }

    }
}