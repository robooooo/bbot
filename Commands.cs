using System;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;

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
    }
}