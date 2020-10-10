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
        [Description("posts bbot-related information and currently ongoing announcements")]
        public async Task About(CommandContext ctx
        )
        {
            string Links = "https://top.gg/bot/362666654452416524\n" +
                "https://discord.bots.gg/bots/362666654452416524\n" +
                "https://discordbotlist.com/bots/bbot\n" +
                "https://bots.ondiscord.xyz/bots/362666654452416524";

            string LatestAnnouncement;
            try
            {
                var Channel = await ctx.Client.GetChannelAsync(Consts.ANNOUNCEMENTS_CHANNEL);
                LatestAnnouncement = (await Channel.GetMessagesAsync(1)).FirstOrDefault()?.Content ?? "No announcements so far.";
            }
            catch (DSharpPlus.Exceptions.UnauthorizedException)
            {
                LatestAnnouncement = "Couldn't find the latest announcement. Consider submitting a bug report on bbot's support server.";
            }

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder()
            {
                Title = "📚 about",
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Description = "Manage your pins and server with automatic message pinning and backing-up in stylish embeds.",
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    Text = "If you're looking for command descriptions, try the help command."
                }
            }
            .AddField("Version", $"{Consts.VERSION_INFO.First().Key}")
            .AddField("Announcements", LatestAnnouncement)
            .AddField("Links", Links)
            .AddField("Github", "https://github.com/robooooo/bbot")
            .AddField("Invite", "https://discordapp.com/oauth2/authorize?client_id=362666654452416524&scope=bot&permissions=92224")
            .AddField("Support", "https://discord.com/invite/YAXQC2Q"));
        }
    }
}
