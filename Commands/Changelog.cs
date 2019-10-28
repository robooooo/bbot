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
            Dictionary<string, string> Information = new Dictionary<string, string>()
            {
                {
                    "4.0.5",

                    "- It used to be the case that `$backup` would crash the bot. Now it isn't.\n" +
                    "- Isn't that just neat?"
                },
                {
                    "4.0.4",

                    "- Added an `$scp` command due to a user's request.\n" +
                    "- The command fetches an SCP or tale from the scp site."
                },
                {
                    "4.0.3",

                    "- Trialing the `$search` command having inline results (saves space?)\n" +
                    "- `$backup` now links to the original post in the footer."
                },
                {
                    "4.0.2",

                    "- (Hopefully) fixed an issue with the bot crashing and not being able to resume."
                },
                {
                    "4.0.1",

                    "- Fixed a bug where `$search` results would contain spaces.\n" +
                    "- Fixed a bug where `$search` results would have malformatted links.\n" +
                    "- `$backup` results now feature a link to the post.\n" +
                    "- Currently, these may open in a browser tab - discord's fault, they won't if you paste them into chat."
                },
                {
                    "4.0.0",

                    "**Fourth re-write of BBot!**\n" +
                    "**BBot is now hosted**, for a (hopefully) permanent uptime.\n" +
                    "- Added the `$changelog` command.\n" +
                    "- Changed the `$search` provider from DuckDuckGo to Google.\n" +
                    "- Changed the functionality of the `$search` command to show other results.\n" +
                    "- Changed the functonality of the `$search` command to allow showing more results.\n" +
                    "- Tweaked the `$roll` command to allow a numerical identifier.\n" +
                    "TODO: BBot will have a more customised help menu.\n"
                },
                {
                    "3.0.0",

                    "Previous version of BBot."
                },
            };

            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder()
            {
                Color = new DiscordColor(0xFFC800),
                Title = "🕒 $changelog",
            };
            if (Information.ContainsKey(version))
            {
                Builder.Description = "Showing specified version.";
                Builder.AddField($"Version {version}", Information[version]);
            }
            else
            {
                Builder.Description = "Showing past 3 versions.";
                foreach (var pair in Information.Take(3))
                    Builder.AddField($"Version {pair.Key}", pair.Value);
            }

            await ctx.RespondAsync(embed: Builder.Build());
        }
    }
}
