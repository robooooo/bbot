using System;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using Imgur.API.Authentication.Impl;
using DSharpPlus.CommandsNext;

namespace BBotCore
{
    // A collection of external services that commands and other parts of the program can call upon.
    public static class Services
    {
        public static Random RNG { get; private set; } = new Random();

        // As of now, the client is put here (as opposed to the helper) so that too large of a cache is not kept.
        // If the bot is put on a more powerful device, or a more intelligent cache is implemented, this may change?
        public static ImgurClient ImgurClient { get; private set; } = new ImgurClient(Environment.GetEnvironmentVariable("IMGUR_TOKEN"));

        public static SearchHelper SearchHelper { get; private set; } = new SearchHelper(new CustomsearchService(new BaseClientService.Initializer()
        {
            ApplicationName = "BBot",
            ApiKey = Environment.GetEnvironmentVariable("SEARCH_KEY"),
        }));

        public static BackupHelper BackupHelper { get; private set; } = new BackupHelper();

        public static DatabaseHelper DatabaseHelper { get; private set; } = new DatabaseHelper();

        public static GuildsHelper GuildsHelper { get; private set; } = new GuildsHelper();
    }
}