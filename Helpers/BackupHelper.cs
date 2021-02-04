using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using LanguageExt;

using static LanguageExt.Prelude;

namespace BBotCore
{
    public class BackupHelper
    {
        public DiscordChannel Source { get; private set; }
        public DiscordChannel Destination { get; private set; }

        public DiscordEmbed HeaderMessage;
        public DiscordEmbed FooterMessage;

        // Configurable backup helper for moving pins from src into dst
        public BackupHelper(DiscordChannel src, DiscordChannel dst)
        {
            this.Source = src;
            this.Destination = dst;
        }

        // Backup using this user for permission checks.
        public async Task DoBackup()
        {
            var Pins = await Source.GetPinnedMessagesAsync();
            if (Pins.Count == 0)
                throw new InvalidOperationException("There are no pins in the source channel. Pin something and try again.");

            var Imgur = new ImgurHelper(Services.ImgurClient);

            if (!(HeaderMessage is null))
                await Source.SendMessageAsync(embed: HeaderMessage);

            // We want to reverse here so that the oldest pins are posted first => newest pin is final
            foreach (var Pin in Pins.Reverse())
            {
                DiscordEmbed Embed = await ProcessMessage(Imgur, Pin);
                await Destination.SendMessageAsync(embed: Embed);
                await Pin.UnpinAsync();
                // Experimental 1/5 second delay: might fix performance degrading while backups are happening. 
                // TODO: Revist with new db? Probably best to leave it anyway.
                await Task.Delay(200);

            }

            if (!(FooterMessage is null))
                await Source.SendMessageAsync(embed: FooterMessage);
        }

        private async Task<DiscordEmbed> ProcessMessage(ImgurHelper im, DiscordMessage pin)
        {
            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Description = GetContentFromMessage(pin),
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Author = new DiscordEmbedBuilder.EmbedAuthor()
                {
                    Name = pin.Author.Username,
                    IconUrl = await im.GetURLFromUser(pin.Author),
                    Url = pin.JumpLink.ToString(),
                },
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    Text = $"#{pin.Channel.Name} | {pin.Timestamp.ToString("yyyy-MM-dd")}",
                },
            };

            var Images = GetImagesFromMessage(pin);
            Option<string> MaybeUrl = Images.FirstOrDefault(opt => opt.IsSome);
            bool ImproperDisplay = false;
            ImproperDisplay |= Images.Count > 1;
            ImproperDisplay |= Images.Any(opt => opt.IsNone);
            ImproperDisplay |= String.IsNullOrWhiteSpace(Builder.Description) && MaybeUrl.IsNone;

            MaybeUrl.IfSome(url => Builder.ImageUrl = url);
            if (ImproperDisplay)
                Builder.AddField(
                        "A part of this message could not be displayed properly.",
                        $"You can view the original post by clicking on the author's name above, or by clicking [this]({pin.JumpLink}) link."
                );

            return Builder.Build();
        }

        // Ugly, but god this was difficult to write in a nice way
        // Some values are valid URLs, None values are invalid
        private List<Option<string>> GetImagesFromMessage(DiscordMessage message)
        {
            var Res = new List<Option<string>>();

            Res.AddRange(
                message.Embeds
                .Map(em => em.Thumbnail?.Url?.ToString())
                .Filter(em => !String.IsNullOrWhiteSpace(em))
                .Map(em => Some(em))
            );
            Res.AddRange(
                message.Embeds
                .Map(em => em.Image?.Url?.ToString())
                .Filter(em => !String.IsNullOrWhiteSpace(em))
                .Map(em => Some(em))
            );

            Res.AddRange(message.Attachments.Map(at =>
            {
                bool IsKnownImageExtension =
                    new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" }
                    .Any(ext => at.Url?.Contains(ext) ?? false);

                if (IsKnownImageExtension && at.Height > 0)
                    return Optional(at.Url);
                else
                    return None;
            }));

            return Res;
        }

        // Used above to decide the content of the message's embed from several sources
        private string GetContentFromMessage(DiscordMessage msg)
        {
            // We want to decide between the content in the post or embeds
            // So we select in the desired priority, removing if null or empty
            if (!string.IsNullOrWhiteSpace(msg.Content))
                return msg.Content;

            foreach (var embed in msg.Embeds)
                if (!string.IsNullOrWhiteSpace(embed.Description))
                    return embed.Description;

            foreach (var embed in msg.Embeds)
                if (!string.IsNullOrWhiteSpace(embed.Footer?.Text))
                    return embed.Description;

            return "";
        }

    }
}