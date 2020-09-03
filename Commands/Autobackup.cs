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
        [Command("autobackup")]
        [Description("configures automatically backing up pinned messages to an overflow channel when the limit is near")]
        public async Task Autobackup(CommandContext ctx,
            [Description("channel the pins will be backed up to, but can be omitted to disable the feature in this channel instead")] DiscordChannel destination
        )
        {
            Permissions HerePerms = destination.PermissionsFor(ctx.Member);
            Permissions TherePerms = destination.PermissionsFor(ctx.Member);
            // Apply permission checks only to non-admins
            if (!ctx.Member.IsOwner && !(HerePerms.HasPermission(Permissions.Administrator) && TherePerms.HasPermission(Permissions.Administrator)))
            {
                if (!TherePerms.HasPermission(Permissions.SendMessages))
                    throw new Exception("You don't have permission to send messages in the target channel.");
                if (!HerePerms.HasPermission(Permissions.ManageMessages))
                    throw new Exception("You do not have permission to manage pins in the current channel.");
            }

            // await Services.DatabaseHelper.SetAutobackupDestination(ctx.Channel.Id, destination.Id);
            Services.DatabaseHelper.Channels.Update(ctx.Channel.Id, dat => dat.AutobackupDest = destination.Id);

            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "⤵️ $autobackup",
                Description = $"Updated Configuration"
            };
            Builder.AddField(name: "Configuration", value: $"When there are nearly fifty pinned messages in this channel, they will all be backed up to **#{destination.Name}**.");
            Builder.AddField(name: "Info", value: $"This feature can be disabled by typing only `$autobackup` with no arguments.");
            await ctx.RespondAsync(embed: Builder.Build());
        }

        [Command("autobackup")]
        public async Task Autobackup(CommandContext ctx)
        {
            Permissions HerePerms = ctx.Channel.PermissionsFor(ctx.Member);
            // Apply permission checks only to non-admins
            if (!ctx.Member.IsOwner && !HerePerms.HasPermission(Permissions.Administrator))
            {
                if (!HerePerms.HasPermission(Permissions.ManageMessages))
                    throw new Exception("You do not have permission to manage pins in the current channel.");
            }

            // await Services.DatabaseHelper.ClearAutobackupDestination(ctx.Channel.Id);
            Services.DatabaseHelper.Channels.Update(ctx.Channel.Id, dat => dat.AutobackupDest = null);

            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "⤵️ $autobackup",
                Description = $"Updated Configuration"
            };
            Builder.AddField(name: "Configuration", value: $"Pinned messages in this channel will not automatically be backed up.");
            await ctx.RespondAsync(embed: Builder.Build());
        }
    }


}