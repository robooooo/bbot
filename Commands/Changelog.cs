using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace BBotCore
{
    public partial class Commands
    {
        [Command("changelog")]
        [Aliases("changes", "version")]
        [Description("posts changelog information for BBot")]
        public async Task Changelog(CommandContext ctx,
            [Description("version to view changes for, but can be omitted to view the latest versions")] string version
        )
        {
            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder()
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "🕒 $changelog",
            };

            if (!Consts.VERSION_INFO.ContainsKey(version))
            {
                throw new ArgumentException($"A version called {version} does not exist.");
            }

            Builder.Description = "Showing specified version.";
            Builder.AddField($"Version {version}", Consts.VERSION_INFO[version]);

            await ctx.RespondAsync(embed: Builder.Build());
        }

        [Command("changelog")]
        public async Task Changelog(CommandContext ctx)
        {
            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder()
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "🕒 $changelog",
                Description = "Showing latest 3 versions."
            };

            foreach (var pair in Consts.VERSION_INFO.Take(3))
                Builder.AddField($"Version {pair.Key}", pair.Value);

            await ctx.RespondAsync(embed: Builder.Build());

        }
    }
}
