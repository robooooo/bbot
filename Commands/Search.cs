using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace BBotCore
{
    public partial class Commands
    {
        [Command("search")]
        [Aliases("s", "google")]
        [Description("searches for a query on google")]
        public async Task Search(CommandContext ctx,
                [RemainingText(), Description("term to search for")] string query
        )
        {
            // Snippet gets our search results stored in Search
            SearchHelper Search = Services.SearchHelper;
            List<string> Results = await Search.AsyncSearchFor(query, 10);

            // Needed to tabulate search results
            string[] Titles = new string[]
            {
                "First", "Second", "Third", "Fourth",
                "Fifth", "Sixth", "Seventh", "Eighth",
                "Ninth", "Tenth", "Eleventh", "Twelfth"
            };

            const int PAGES = 3;
            const int ENTRIES = 3;
            IEnumerable<Page> Pages = Enumerable.Range(0, PAGES).Select(page =>
            {
                DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor(Consts.EMBED_COLOUR),
                    Title = "🔍 search",
                    Description = $"Showing {Titles[page].ToLower()} page of results.",
                };

                for (int i = 0; i < ENTRIES; i++)
                    Builder.AddField($"{Titles[page * ENTRIES + i]} Result", Results[page * ENTRIES + i], inline: false);

                return new Page(embed: Builder);
            });

            var Interact = ctx.Client.GetInteractivity();
            await ctx.RespondAsync($"{Results[0]}");
            await Interact.SendPaginatedMessageAsync(
                c: ctx.Channel,
                u: ctx.User,
                pages: Pages,
                // Ignore extra scrolling as opposed to wrapping around
                behaviour: PaginationBehaviour.Ignore,
                deletion: PaginationDeletion.DeleteEmojis,
                emojis: new PaginationEmojis()
                {
                    SkipLeft = null,
                    SkipRight = null,
                    Stop = null,
                }
            );

            // Interact.SendPaginatedMessageAsync(ctx.Channel, null, new)
        }
    }
}
