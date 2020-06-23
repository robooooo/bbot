using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.EventArgs;

namespace BBotCore
{
    public static class Events
    {
        // Callback from errored command
        // Is used to display error messages
        public static async Task CommandErrored(CommandErrorEventArgs e)
        {
            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(0xB00020),
                Title = "❌ Error!",
                Description = $"In `${e.Command.Name}`",
            };

            Exception ex = e.Exception;
            while (ex != null)
            {
                Builder.AddField("Reason:", e.Exception.Message);
                ex = ex.InnerException;
            }
            // Builder.AddField("Stack Trace:", $"```{e.Exception.StackTrace}```");
            await e.Context.Channel.SendMessageAsync(embed: Builder.Build());
        }

        // Used to trigger the autopin feature.
        public static async Task MessageReactionAdded(MessageReactionAddEventArgs e)
        {
            DiscordEmoji PinEmoji = DiscordEmoji.FromName((DiscordClient)e.Client, ":pushpin:");
            // Exit quickly if the reactions aren't relevant to the bot, saves time?
            if (!e.Emoji.Equals(PinEmoji))
                return;
            if (e.Message.Pinned)
                return;

            try
            {
                uint? MaybeThreshhold = await Commands.DatabaseHelper.GetAutopinLimit(e.Channel.Id);
                if (MaybeThreshhold is uint Threshhold)
                {
                    int PinReacts = e.Message.Reactions.Where(r => r.Emoji.Equals(PinEmoji)).First().Count;
                    // Do not pin when the threshhold is zero - this special value disables the feature for this channel.
                    if (Threshhold != 0 && PinReacts >= Threshhold)
                        await e.Message.PinAsync();
                }
            }
            catch (Exception ex)
            {
                DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor(0xB00020),
                    Title = "❌ Error!",
                    Description = $"During autopinning",
                };
                Builder.AddField("Reason:", ex.Message);
                await e.Channel.SendMessageAsync(embed: Builder.Build());
            }
        }

        // Used to trigger the autobackup feature.
        public static async Task ChannelPinsUpdated(ChannelPinsUpdateEventArgs e)
        {
            var Pins = await e.Channel.GetPinnedMessagesAsync();
            try
            {
                // Trigger autobackup iff over 45 pins: sane limit with some leeway in case a few are missed.
                if (Pins.Count >= 45)
                {
                    ulong? MaybeDestId = await Commands.DatabaseHelper.GetAutobackupDestination(e.Channel.Id);
                    if (MaybeDestId is ulong destId)
                    {
                        DiscordChannel destination = e.Channel.Guild.GetChannel(destId);
                        await Commands.BackupHelper.DoBackup(e.Channel, destination);
                    }
                }
            }
            catch (Exception ex)
            {
                DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor(0xB00020),
                    Title = "❌ Error!",
                    Description = $"During autobackup",
                };
                Builder.AddField("Reason:", ex.Message);
                await e.Channel.SendMessageAsync(embed: Builder.Build());
            }
        }
    }

}
