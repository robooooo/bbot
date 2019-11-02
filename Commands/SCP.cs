using System.Threading.Tasks;
using System;
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
                Query = $"site:www.scp-wiki.net {page.PadLeft(3, '0')}";
            else
                Query = $"site:www.scp-wiki.net {page}";


            var CSE = CSS.Cse.List(Query);
            CSE.Cx = Environment.GetEnvironmentVariable("SEARCH_CX");
            var Search = await CSE.ExecuteAsync();

            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "🕵️ $scp",
                // Description = $"Access authorized.",
            };
            Builder.AddField("Result", Search.Items[0].Link);

            await ctx.RespondAsync(embed: Builder.Build());
        }
    }
}
