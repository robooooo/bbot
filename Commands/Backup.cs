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

            // Important so that unpriveliged users cannnot backup to channel they don't have post permissions for
            // Also provides error handling for the case where the bot itself is unpriveliged
            Permissions UserPerms = channel.PermissionsFor(ctx.Member);
            // We need to manually check for admin because it overrides these permissions
            // Apply permission checks only to non-admins
            if (!UserPerms.HasPermission(Permissions.Administrator))
            {
                if (!UserPerms.HasPermission(Permissions.SendMessages))
                    throw new Exception("You don't have permission to send messages in the target channel.");
                if (!UserPerms.HasPermission(Permissions.ManageMessages))
                    throw new Exception("You do not have permission to manage pins in the current channel.");
            }

            var Pins = await ctx.Channel.GetPinnedMessagesAsync();

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "💾 $backup",
                Description = $"Backing up {Pins.Count} pins to #{channel.Name}.",
            });

            // We want to reverse here so that the oldest pins are posted first => newest pin is final
            // POSSIBLE REFACTOR: Move loop body to function
            foreach (var pin in Pins.Reverse())
            {
                // Create a link pointing to the original message which can be visited
                // We put it in the header (author) to avoid spam
                string Link = $"https://discordapp.com/channels/{pin.Channel.GuildId}/{pin.ChannelId}/{pin.Id}";

                // REFACTOR: try testing w/ UploadFile(s)Async to save avatar

                // Move all this information into a postable embed
                DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
                {
                    Description = GetContentFromMessage(pin),
                    Color = new DiscordColor(Consts.EMBED_COLOUR),
                    // Author includes the image and link at the top of the page (why?)
                    Author = new DiscordEmbedBuilder.EmbedAuthor()
                    {
                        Name = pin.Author.Username,
                        // REFACTOR: Work-around avatar changes 
                        IconUrl = pin.Author.AvatarUrl,
                        Url = Link,
                    },
                    Footer = new DiscordEmbedBuilder.EmbedFooter()
                    {
                        Text = $"#{pin.Channel.Name} | {pin.Timestamp.ToString("yyyy-MM-dd")}",
                    },
                    ImageUrl = GetImageURLFromMessage(pin),
                };

                await channel.SendMessageAsync(content: Link, embed: Builder.Build());
                await pin.UnpinAsync();
            }

            DiscordEmbedBuilder FinalBuilder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "💾 $backup",
                Description = $"Backup finished successfully.",
            };
            await ctx.RespondAsync(embed: FinalBuilder.Build());
        }

        // Used above to decide which URL, if any, is used as the single image from several sources
        static string GetImageURLFromMessage(DiscordMessage msg)
        {
            // Images can be uploaded inside embeds (like a yt thumbnail)...
            foreach (var embed in msg.Embeds)
                if (embed.Thumbnail != null)
                    return embed.Thumbnail.Url.ToString();
                else if (embed.Image != null)
                    return embed.Image.Url.ToString();
            // Or as a file (but we do this last, since it may be a file)
            foreach (var attachment in msg.Attachments)
                return attachment.Url; // TODO: Check if image
            // We don't have anything that could possibly be a thumbnail
            return "";
        }

        // Used above to decide the content of the message from several sources
        static string GetContentFromMessage(DiscordMessage msg) 
        {
            // We want to decide between the content in the post or embeds
            // So we select in the desired priority, removing if null or empty
            if (!string.IsNullOrEmpty(msg.Content))
            {
                return msg.Content;
            } 
            else 
            {
                foreach (var embed in msg.Embeds)
                    if (!string.IsNullOrEmpty(embed.Description))
                        return embed.Description;
            }
            return "";
        }
    }
}

