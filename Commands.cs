using System;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using Imgur.API.Authentication.Impl;
using DSharpPlus.CommandsNext;

namespace BBotCore
{
    public partial class Commands : BaseCommandModule
    {
        private static Random RNG = new Random();
        private static CustomsearchService CSS = new CustomsearchService(new BaseClientService.Initializer()
        {
            ApplicationName = "BBot",
            ApiKey = Environment.GetEnvironmentVariable("SEARCH_KEY"),
        });

        // As of now, the client is put here (as opposed to the helper) so that too large of a cache is not kept.
        // If the bot is put on a more powerful device, or a more intelligent cache is implemented, this may change?
        public static ImgurClient ImgurClient { get; private set; } = new ImgurClient(Environment.GetEnvironmentVariable("IMGUR_TOKEN"));

        public static SearchHelper SearchHelper { get; private set; } = new SearchHelper(CSS);

        public static BackupHelper BackupHelper { get; private set; } = new BackupHelper();

        public static DatabaseHelper DatabaseHelper { get; private set; } = new DatabaseHelper();
    }
}