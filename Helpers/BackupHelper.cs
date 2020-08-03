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
    public class BackupHelper
    {
        // Backup from src to dst, used in both the backup command and the autobackup feature.
        // Does not perform any kind of permission checking, the caller must perform this.
        public async Task DoBackup(DiscordChannel src, DiscordChannel dst)
        {
            var Pins = await src.GetPinnedMessagesAsync();
            var Imgur = new ImgurHelper(Commands.ImgurClient);

            await src.SendMessageAsync(embed: new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "💾 $backup",
                Description = $"Backing up {Pins.Count} pins to #{dst.Name}.",
            });

            // We want to reverse here so that the oldest pins are posted first => newest pin is final
            foreach (var pin in Pins.Reverse())
            {
                // Create a link pointing to the original message which can be visited
                // We put it in the header (author) to avoid spam

                // Move all this information into a postable embed
                DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
                {
                    Description = GetContentFromMessage(pin),
                    Color = new DiscordColor(Consts.EMBED_COLOUR),
                    Author = new DiscordEmbedBuilder.EmbedAuthor()
                    {
                        Name = pin.Author.Username,
                        IconUrl = await Imgur.GetURLFromUser(pin.Author),
                        Url = pin.JumpLink.ToString(),
                    },
                    Footer = new DiscordEmbedBuilder.EmbedFooter()
                    {
                        Text = $"#{pin.Channel.Name} | {pin.Timestamp.ToString("yyyy-MM-dd")}",
                    },
                };

                string ImageUrl = GetImageURLFromMessage(pin);
                // There are still some cases this command can't handle, e.g. videos
                // This is a contingency for this one case - we can link directly to the video
                bool IsKnownImageExtension =
                    new string[] { ".jpg", ".jpeg", ".png", ".bmp" }
                    .Any(ext => ImageUrl?.EndsWith(ext) ?? false);
                // bool IsKnownVideoExtension =
                //     new string[] { ".mp4", ".m4a" }
                //     .Any(ext => ImageUrl?.EndsWith(ext) ?? false);

                if (ImageUrl != null)
                    Builder.ImageUrl = ImageUrl;

                // Include link to OP iff we're dealing with a file, but the format isn't an image that can be embedded
                if (!IsKnownImageExtension && ImageUrl != null)
                    Builder.AddField(
                        "A file included in this message could not be displayed properly.",
                        $"You can view the original post by clicking on the author's name above, or by clicking [this]({pin.JumpLink}) link."
                    );

                await dst.SendMessageAsync(embed: Builder.Build());

                await pin.UnpinAsync();
            }

            await src.SendMessageAsync(embed: new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "💾 $backup",
                Description = $"Backup finished successfully.",
            });
        }

        // Used above to decide which URL, if any, is used as the single image from several sources
        private static string GetImageURLFromMessage(DiscordMessage msg)
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
            // Or as a file (but we do this last, since it may be a non-image file)
            foreach (var attachment in msg.Attachments)
                if (attachment.FileSize > 0)
                    return attachment.Url;
            // We don't have anything that could possibly be a thumbnail
            return null;
        }

        // Used above to decide the content of the message from several sources
        private static string GetContentFromMessage(DiscordMessage msg)
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