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
        [Command("about")]
        [Aliases("info")]
        [Description("posts general descriptions of, and useful links for, bbot")]
        public async Task About(CommandContext ctx
        )
        {
            string Links = "https://top.gg/bot/362666654452416524\n" +
                "https://discord.bots.gg/bots/362666654452416524\n" +
                "https://discordbotlist.com/bots/bbot\n" +
                "https://bots.ondiscord.xyz/bots/362666654452416524";

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder()
            {
                Title = "📚 $about",
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Description = "Manage your pins and server with automatic message pinning and backing-up in stylish embeds.",
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    Text = "If you're looking for command descriptions, try `$help`."
                }
            }
            .AddField("Version", $"{Consts.VERSION_INFO.First().Key}")
            .AddField("Links", Links)
            .AddField("Github", "https://github.com/robooooo/bbot")
            .AddField("Invite", "https://discordapp.com/oauth2/authorize?client_id=362666654452416524&scope=bot&permissions=92224")
            .AddField("Support", "https://discord.com/invite/YAXQC2Q"));
        }
    }
}
