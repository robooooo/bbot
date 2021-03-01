using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity.Enums;
using LanguageExt;

using static LanguageExt.Prelude;

namespace BBotCore
{
    public partial class Commands : BaseCommandModule
    {


        [Command("setup")]
        [Description("Allows quickly configuring or re-configuring an entire server in a guided manner.")]
        public async Task Setup(CommandContext ctx)
        {
            async Task<bool> SetupHelper()
            {


                var TICK = DiscordEmoji.FromName(ctx.Client, ":white_check_mark:");
                var STOP = DiscordEmoji.FromName(ctx.Client, ":no_entry:");

                var Interact = ctx.Client.GetInteractivity();
                // Interact.WaitForReactionAsync

                var Desc = "This command is intended to be a guided-walk through configuring your server" +
                " for new users, as well as a way to quickly set up bbot if your server has many channels." +
                " At each stage of configuration, you will be prompted to respond via reacting to my messages.";
                var Curr = await ctx.RespondAsync(embed: new DiscordEmbedBuilder()
                {
                    Color = new DiscordColor(Consts.EMBED_COLOUR),
                    Title = "🔧 setup",
                    Description = "Information"
                }
                .AddField(name: "Setup", value: Desc)
                .AddField(name: "Next Steps", value: "React with ✅ to continue or ⛔ to cancel."));

                var Res = await Interact.WaitForReactionAsync((e) => e.Equals(TICK) || e.Equals(STOP), Curr, ctx.User);
                if (Res.TimedOut || Res.Result.Emoji.Equals(STOP))
                    return false;

                Desc = "This bot has functionality that allows it to **backup** pinned messages to a dedicated channel." +
                "This can be done manually via the `backup` command, but it can also be done automatically." +
                "Would you like bbot to automatically backup your pinned messages?" +
                "Note that the setup command only allows the usage of one destination backup channel.";
                Curr = await ctx.RespondAsync(embed: new DiscordEmbedBuilder()
                {
                    Color = new DiscordColor(Consts.EMBED_COLOUR),
                    Title = "🔧 setup",
                    Description = "Auto-backup"
                }
                .AddField(name: "Info", value: Desc)
                .AddField(name: "Next Steps", value: "React with ✅ to continue or ⛔ to skip this step."));

                Res = await Interact.WaitForReactionAsync((e) => e.Equals(TICK) || e.Equals(STOP), Curr, ctx.User);
                if (!Res.TimedOut && Res.Result.Emoji.Equals(TICK))
                {
                    Desc = "When there are too many pinned messages in a source channel," +
                    " bbot can perform a backup of those pinned messages, moving them into a dedicated destination channel.";
                    Curr = await ctx.RespondAsync(embed: new DiscordEmbedBuilder()
                    {
                        Color = new DiscordColor(Consts.EMBED_COLOUR),
                        Title = "🔧 setup",
                        Description = "Auto-backup"
                    }
                    .AddField(name: "Info", value: Desc)
                    .AddField(name: "Next Steps", value: "Type your destination channel in chat."));

                    var ResP = await Interact.WaitForMessageAsync((e) =>
                        e.Author == ctx.User && e.Channel == ctx.Channel && (e.MentionedChannels?.Length() ?? 0) == 1
                    );
                    if (ResP.TimedOut)
                        return false;
                    var Dest = ResP.Result.MentionedChannels.First();

                    Desc = "Many different channels can be linked to the destination channel." +
                    "Would you like to link **all** channels to the destination channel?";
                    Curr = await ctx.RespondAsync(embed: new DiscordEmbedBuilder()
                    {
                        Color = new DiscordColor(Consts.EMBED_COLOUR),
                        Title = "🔧 setup",
                        Description = "Auto-backup"
                    }
                    .AddField(name: "Info", value: Desc)
                    .AddField(name: "Next Steps", value: "React with ✅ to yes or ⛔ for no."));

                    Res = await Interact.WaitForReactionAsync((e) => e.Equals(TICK) || e.Equals(STOP), Curr, ctx.User);
                    if (Res.TimedOut)
                        return false;
                    else if (Res.Result.Equals(TICK))
                    {
                        foreach (var c in ctx.Guild.Channels.Values
                            .Where(c => c.Type == ChannelType.Text || c.Type == ChannelType.Private))
                        {
                            await Services.DatabaseHelper.Channels.Update(c.Id, dat => dat.AutobackupDest = Dest.Id);
                        }
                    }
                    else
                    {
                        Desc = "Many different channels can be linked to the destination channel.";
                        Curr = await ctx.RespondAsync(embed: new DiscordEmbedBuilder()
                        {
                            Color = new DiscordColor(Consts.EMBED_COLOUR),
                            Title = "🔧 setup",
                            Description = "Auto-backup"
                        }
                        .AddField(name: "Info", value: Desc)
                        .AddField(name: "Next Steps", value: "List your source channels in chat. Ensure they are all contained in a single message."));

                        ResP = await Interact.WaitForMessageAsync((e) =>
                            e.Author == ctx.User && e.Channel == ctx.Channel && (e.MentionedChannels?.Length() ?? 0) > 0
                        );
                        if (ResP.TimedOut)
                            return false;
                        foreach (var c in ResP.Result.MentionedChannels
                            .Where(c => c.Type == ChannelType.Text || c.Type == ChannelType.Private))
                        {
                            await Services.DatabaseHelper.Channels.Update(c.Id, dat => dat.AutobackupDest = Dest.Id);
                        }
                    }



                }
                return true;
            }

            if (!await SetupHelper())
                await ctx.RespondAsync(embed: new DiscordEmbedBuilder()
                {
                    Color = new DiscordColor(Consts.EMBED_COLOUR),
                    Title = "🔧 setup",
                    Description = "Information"
                }.AddField(name: "Exit", value: "Setup has been cancelled."));
        }

        public async Task SetupAutobackup(CommandContext ctx)
        {
            var Interact = ctx.Client.GetInteractivity();



        }

        public async Task SetupAutopin(CommandContext ctx)
        {

        }

        // public async Task SetupFastpin(CommandContext ctx)
        // {

        // }
    }
}