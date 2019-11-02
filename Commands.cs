using System;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using ImgurSharp;

namespace BBotCore
{
    public partial class Commands
    {
        private static Random RNG = new Random();
        private static CustomsearchService CSS = new CustomsearchService(new BaseClientService.Initializer()
        {
            ApplicationName = "BBot",
            ApiKey = Environment.GetEnvironmentVariable("SEARCH_KEY"),
        });

        private static Imgur ImgurClient = new Imgur(Environment.GetEnvironmentVariable("IMGUR_TOKEN"));
    }
}