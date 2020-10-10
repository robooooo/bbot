using System.Threading.Tasks;
using System;
using System.Linq;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace BBotCore
{
    public partial class Commands
    {
        [Command("SCP")]
        [Description("links to an SCP or tale from the SCP wiki")]
        public async Task SCP(CommandContext ctx,
            [RemainingText(), Description("SCP number or tale name to search for")] string page)
        {

            string Query;
            // Craft a search parameter to get more accurate results
            // We limit our search to the scp-wiki site
            // And format numerica SCPs, padding left with 0s to length 3
            if (int.TryParse(page, out int num))
                Query = $"site:www.scpwiki.com {page.PadLeft(3, '0')}";
            else
                Query = $"site:www.scpwiki.com {page}";


            SearchHelper Search = Services.SearchHelper;
            string Result = (await Search.AsyncSearchFor(Query, 1)).First();

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder()
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "🕵️ scp",
            }
            .AddField("Result", Result));
        }
    }
}
