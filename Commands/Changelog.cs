using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;

namespace BBotCore
{
    public partial class Commands
    {
        [Command("changelog")]
        [Aliases("changes", "version")]
        [Description("posts changelog information for BBot")]
        public async Task Changelog(CommandContext ctx,
            [Description("version to view changes for, but can be omitted to view all versions")] string version
        )
        {
            if (!Consts.VERSION_INFO.ContainsKey(version))
            {
                throw new ArgumentException($"Version number {version} does not exist.");
            }

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder()
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "🕒 changelog",
                Description = "Showing specified version."
            }
            .AddField($"Version {version}", Consts.VERSION_INFO[version]));
        }

        [Command("changelog")]
        public async Task Changelog(CommandContext ctx)
        {
            IEnumerable<Page> Pages = Consts.VERSION_INFO.Select(pair =>
            {
                return new Page(embed: new DiscordEmbedBuilder()
                {
                    Color = new DiscordColor(Consts.EMBED_COLOUR),
                    Title = "🕒 changelog",
                    Description = "Showing all versions."

                }
                .AddField($"Version {pair.Key}", pair.Value));
            });

            await ctx.Client.GetInteractivity().SendPaginatedMessageAsync(
                c: ctx.Channel,
                u: ctx.User,
                pages: Pages,
                behaviour: PaginationBehaviour.Ignore,
                deletion: PaginationDeletion.DeleteEmojis,
                emojis: new PaginationEmojis()
                {
                    SkipLeft = null,
                    SkipRight = null,
                    Stop = null,
                }
            );
        }
    }
}
