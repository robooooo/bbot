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
            [Description("channel the pins will be backed up to")] DiscordChannel destination
        )
        {
            // Important so that unpriveliged users cannnot backup to channel they don't have post permissions for
            // Also provides error handling for the case where the bot itself is unpriveliged
            Permissions HerePerms = destination.PermissionsFor(ctx.Member);
            Permissions TherePerms = destination.PermissionsFor(ctx.Member);
            // We need to manually check for admin because it overrides these permissions
            // Apply permission checks only to non-admins
            if (!ctx.Member.IsOwner && !(HerePerms.HasPermission(Permissions.Administrator) && TherePerms.HasPermission(Permissions.Administrator)))
            {
                if (!TherePerms.HasPermission(Permissions.SendMessages))
                    throw new Exception("You don't have permission to send messages in the target channel.");
                if (!HerePerms.HasPermission(Permissions.ManageMessages))
                    throw new Exception("You do not have permission to manage pins in the current channel.");
            }

            await BackupHelper.DoBackup(ctx.Channel, destination);
        }
    }
}