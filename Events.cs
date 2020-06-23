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
                Description = $"In ${e.Command.Name}",
            };
            Builder.AddField("Reason:", e.Exception.Message);
            //Builder.AddField("Stack Trace:", $"```{e.Exception.StackTrace}```");
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
                uint Threshhold = Commands.DatabaseHelper.getAutopinLimit(e.Channel.Id);
                int PinReacts = e.Message.Reactions.Where(r => r.Emoji.Equals(PinEmoji)).Count();
                if (Threshhold != 0 && PinReacts >= Threshhold)
                    await e.Message.PinAsync();
                    
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
            // Trigger autobackup iff over 45 pins: sane limit with some leeway in case a few are missed.
            var Pins = await e.Channel.GetPinnedMessagesAsync();
            try
            {
                if (Pins.Count >= 3)
                {
                    ulong destinationId = Commands.DatabaseHelper.getAutobackupDestination(e.Channel.Id);
                    DiscordChannel destination = e.Channel.Guild.GetChannel(destinationId);
                    await Commands.BackupHelper.DoBackup(e.Channel, destination);
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
