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
        private static int Heartbeats = 0;
        private static int StatusIndex = 0;
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
                if (e.Exception.Message == "One or more pre-execution checks failed.")
                    Builder.AddField("Reason", "You lack the required permissions to run this command in this channel.");
                else
                    Builder.AddField("Reason", e.Exception.Message);
                ex = ex.InnerException;
            }
            // Builder.AddField("Stack Trace:", $"```{e.Exception.StackTrace}```");
            Builder = Builder.WithFooter(text: "If you think this shouldn't be happening, consider submitting a bug report in our support server.");
            await e.Context.Channel.SendMessageAsync(embed: Builder);
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
                // uint? MaybeThreshhold = await Services.DatabaseHelper.GetAutopinLimit(e.Channel.Id);
                uint? MaybeThreshhold = (await Services.DatabaseHelper.Channels.Get(e.Channel.Id)).AutopinLimit;
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
                if (Pins.Count >= Consts.AUTOBACKUP_THRESHOLD)
                {
                    // ulong? MaybeDestId = await Services.DatabaseHelper.GetAutobackupDestination(e.Channel.Id);
                    ulong? MaybeDestId = (await Services.DatabaseHelper.Channels.Get(e.Channel.Id)).AutobackupDest;
                    if (MaybeDestId is ulong destId)
                    {
                        DiscordChannel destination = e.Channel.Guild.GetChannel(destId);
                        await Services.BackupHelper.DoBackup(e.Channel, destination);
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

        // Using heartbeats as a timer to trigger status changes (hacky!)
        // Heartbeats are sent at around a 40-second interval
        // So 15 heartbeats are around 10 minutes
        public static async Task HeartbeatTimer(HeartbeatEventArgs e)
        {
            Heartbeats = (Heartbeats + 1) % (Consts.BEATS_BETWEEN_STATUSES);
            if (Heartbeats == 0)
            {
                // This runs synchronously, so let's only do it so often
                int guilds = e.Client.Guilds.Count();
                if (guilds > 0)
                    Services.GuildsHelper.UpdateGuildsAsync(e.Client.CurrentUser.Id, guilds);

                StatusIndex = (StatusIndex + 1) % Consts.STATUS_MESSAGES.Length;
                string newStatus = Consts.STATUS_MESSAGES[StatusIndex]
                    .Replace("LATEST_VERSION", $"{Consts.VERSION_INFO.First().Key}")
                    .Replace("GUILDS_JOINED", $"{guilds}");
                await e.Client.UpdateStatusAsync(new DiscordActivity(newStatus, ActivityType.Playing));
            }
        }
    }

}
