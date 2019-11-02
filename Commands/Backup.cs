using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace BBotCore
{
    public partial class Commands
    {
        [Command("backup")]
        [Description("backs up all the pins in the current channel to another channel")]
        public async Task Backup(CommandContext ctx,
            [Description("channel the pins will be backed up to")] DiscordChannel channel
        )
        {
            // Important so that unpriveliged users cannnot backup to channel they don't have post permissions for
            // Also provides error handling for the case where the bot itself is unpriveliged
            Permissions UserPerms = channel.PermissionsFor(ctx.Member);
            // We need to manually check for admin because it overrides these permissions
            // Apply permission checks only to non-admins
            if (!UserPerms.HasPermission(Permissions.Administrator) && !ctx.Member.IsOwner)
            {
                if (!UserPerms.HasPermission(Permissions.SendMessages))
                    throw new Exception("You don't have permission to send messages in the target channel.");
                if (!UserPerms.HasPermission(Permissions.ManageMessages))
                    throw new Exception("You do not have permission to manage pins in the current channel.");
            }

            var Pins = await ctx.Channel.GetPinnedMessagesAsync();
            var Imgur = new ImgurHelper(ImgurClient);

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "💾 $backup",
                Description = $"Backing up {Pins.Count} pins to #{channel.Name}.",
            });

            // We want to reverse here so that the oldest pins are posted first => newest pin is final
            foreach (var pin in Pins.Reverse())
            {
                // Create a link pointing to the original message which can be visited
                // We put it in the header (author) to avoid spam
                string Link = $"https://discordapp.com/channels/{pin.Channel.GuildId}/{pin.ChannelId}/{pin.Id}";

                // Move all this information into a postable embed
                DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
                {
                    Description = GetContentFromMessage(pin),
                    Color = new DiscordColor(Consts.EMBED_COLOUR),
                    // Author includes the image and link at the top of the page (why?)
                    Author = new DiscordEmbedBuilder.EmbedAuthor()
                    {
                        Name = pin.Author.Username,
                        IconUrl = await Imgur.GetURLFromUser(pin.Author),
                        Url = Link,
                    },
                    Footer = new DiscordEmbedBuilder.EmbedFooter()
                    {
                        Text = $"#{pin.Channel.Name} | {pin.Timestamp.ToString("yyyy-MM-dd")}",
                    },

                };

                string ImageUrl = GetImageURLFromMessage(pin);
                if (ImageUrl != null)
                    Builder.ImageUrl = ImageUrl;

                // There are still some cases this command can't handle, e.g. videos
                // This is a contingency for this one case - we can link directly to the video
                bool IsKnownImageExtension =
                    new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" }
                    .Any(ext => ImageUrl?.EndsWith(ext) ?? false);

                // Include link iff we're dealing with a link, but the format isn't an image
                if (!IsKnownImageExtension && ImageUrl != null)
                    await channel.SendMessageAsync(content: Link, embed: Builder.Build());
                else
                    await channel.SendMessageAsync(embed: Builder.Build());

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
            {
                string ThumbnailURL = embed.Thumbnail?.Url?.ToString();
                string ImageURL = embed.Image?.Url?.ToString();

                if (!String.IsNullOrWhiteSpace(ThumbnailURL))
                    return ThumbnailURL;
                else if (!String.IsNullOrWhiteSpace(ImageURL))
                    return ImageURL;
            }
            // Or as a file (but we do this last, since it may be a file)
            foreach (var attachment in msg.Attachments)
                if (attachment.FileSize > 0)
                    return attachment.Url;
            // We don't have anything that could possibly be a thumbnail
            return null;
        }

        // Used above to decide the content of the message from several sources
        static string GetContentFromMessage(DiscordMessage msg)
        {
            // We want to decide between the content in the post or embeds
            // So we select in the desired priority, removing if null or empty
            if (!string.IsNullOrWhiteSpace(msg.Content))
            {
                return msg.Content;
            }
            else
            {
                foreach (var embed in msg.Embeds)
                    if (!string.IsNullOrWhiteSpace(embed.Description))
                        return embed.Description;
            }
            return "";
        }
    }
}