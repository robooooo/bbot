using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBotCore
{
    public partial class Commands
    {
        [Command("backup")]
        [Description("Backup all the pins in the current channel to a different channel.")]
        public async Task Backup(CommandContext ctx,
            [Description("Channel to backup the pins to.")] DiscordChannel channel
        )
        {
            if (!(ctx.Member.IsOwner || channel.PermissionsFor(ctx.Member).HasPermission(Permissions.Administrator) || ctx.Member.Id == 110161277707399168))
            {
                if (!channel.PermissionsFor(ctx.Member).HasPermission(Permissions.SendMessages))
                    throw new Exception("You don't have permission to send messages in the target channel.");
                if (!ctx.Channel.PermissionsFor(ctx.Member).HasPermission(Permissions.ManageMessages))
                    throw new Exception("You do not have permission to manage pins in the current channel.");
            }

            var Pins = await ctx.Channel.GetPinnedMessagesAsync();
            DiscordEmbedBuilder FirstBuilder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(0xFFC800),
                Title = "💾 $backup",
                Description = $"Backing up {Pins.Count} pins to #{channel.Name}.",
            };
            await ctx.RespondAsync(embed: FirstBuilder);

            var UnpinQueue = new Queue<DiscordMessage>();
            foreach (var p in Pins.Reverse())
            {
                string EmbedURL = null;
                if (p.Attachments.Count > 0)
                    EmbedURL = p.Attachments[0].Url;
                else if (p.Embeds.Count > 0 && p.Embeds[0].Thumbnail != null)
                    EmbedURL = p.Embeds[0].Thumbnail.Url.ToString();

                // Create a link for the message
                string Link = $"https://discordapp.com/channels/{p.Channel.GuildId}/{p.ChannelId}/{p.Id}";

                DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
                {
                    Description = p.Content,
                    Color = new DiscordColor(0xFFC800),
                    Author = new DiscordEmbedBuilder.EmbedAuthor()
                    {
                        Name = p.Author.Username,
                        IconUrl = p.Author.AvatarUrl,
                        Url = Link,
                    },
                    Footer = new DiscordEmbedBuilder.EmbedFooter()
                    {
                        Text = $"#{p.Channel.Name} | {p.Timestamp.ToString("yyyy-MM-dd")}",
                    },
                    ImageUrl = "",
                };
                if (EmbedURL != null)
                    Builder.ImageUrl = EmbedURL;
                await channel.SendMessageAsync(content: Link, embed: Builder.Build());
                UnpinQueue.Enqueue(p);
            }

            DiscordEmbedBuilder FinalBuilder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(0xFFC800),
                Title = "💾 $backup",
                Description = $"Backup finished successfully.",
            };
            await ctx.RespondAsync(embed: FinalBuilder.Build());
            foreach (DiscordMessage m in UnpinQueue)
                await m.UnpinAsync();
        }
    }
}
