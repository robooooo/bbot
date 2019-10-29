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
        [Description("View the changelog for BBot.")]
        public async Task Changelog(CommandContext ctx,
     [Description("Version to view the changelog for")] string version = "No version specified."
 )
        {
            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder()
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "🕒 $changelog",
            };
            // Usage is $changelog ver. or simply $changelog
            // So we search and find ver. if it exists 
            if (Consts.VERSION_INFO.ContainsKey(version))
            {
                Builder.Description = "Showing specified version.";
                Builder.AddField($"Version {version}", Consts.VERSION_INFO[version]);
            }
            else
            {
                Builder.Description = "Showing past 3 versions.";
                foreach (var pair in Consts.VERSION_INFO.Take(3))
                    Builder.AddField($"Version {pair.Key}", pair.Value);
            }

            await ctx.RespondAsync(embed: Builder.Build());
        }
    }
}
