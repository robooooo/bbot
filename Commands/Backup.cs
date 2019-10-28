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
            // REFACTOR: Add support for users using citador or similar

            // REFACTOR: Manual permission checks w/ owner override 
            // Important so that unpriveliged users cannnot backup to channel they don't have post permissions for
            // Also provides error handling for the case where the bot itself is unpriveliged
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

            //var UnpinQueue = new Queue<DiscordMessage>();

            // We want to reverse here so that the oldest pins are posted first => newest pin is final
            // POSSIBLE REFACTOR: Move loop body to function
            foreach (var pin in Pins.Reverse())
            {
                // REFACTOR: Determine the image to use 
                string EmbedURL = null;
                if (pin.Attachments.Count > 0)
                    EmbedURL = pin.Attachments[0].Url;
                else if (pin.Embeds.Count > 0 && pin.Embeds[0].Thumbnail != null)
                    EmbedURL = pin.Embeds[0].Thumbnail.Url.ToString();

                // Create a link pointing to the original message which can be visited
                string Link = $"https://discordapp.com/channels/{pin.Channel.GuildId}/{pin.ChannelId}/{pin.Id}";

                // Move all this information into a postable embed
                DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
                {
                    Description = pin.Content,
                    Color = new DiscordColor(0xFFC800),
                    Author = new DiscordEmbedBuilder.EmbedAuthor()
                    {
                        Name = pin.Author.Username,
                        IconUrl = pin.Author.AvatarUrl,
                        Url = Link,
                    },
                    Footer = new DiscordEmbedBuilder.EmbedFooter()
                    {
                        Text = $"#{pin.Channel.Name} | {pin.Timestamp.ToString("yyyy-MM-dd")}",
                    },
                    ImageUrl = "",
                };
                if (EmbedURL != null)
                    Builder.ImageUrl = EmbedURL;

                await channel.SendMessageAsync(content: Link, embed: Builder.Build());
                await pin.UnpinAsync();
            }

            DiscordEmbedBuilder FinalBuilder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(0xFFC800),
                Title = "💾 $backup",
                Description = $"Backup finished successfully.",
            };
            await ctx.RespondAsync(embed: FinalBuilder.Build());
            //foreach (DiscordMessage m in UnpinQueue)
            //  await m.UnpinAsync();
        }
    }
}
