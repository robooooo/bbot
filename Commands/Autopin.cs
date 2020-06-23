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
        [Command("autopin")]
        [Description("configures automatically pinning messages when they accumulate a large number of 📌 reactions")]
        public async Task Autopin(CommandContext ctx,
            [Description("number of 📌 reactions that must be reached for the message to be pinned, which can be set to 0 to disable the feature")] uint limit
        )
        {
            Permissions UserPerms = ctx.Channel.PermissionsFor(ctx.Member);
            // Apply permission checks only to non-admins
            if (!UserPerms.HasPermission(Permissions.Administrator) && !ctx.Member.IsOwner)
            {
                if (!UserPerms.HasPermission(Permissions.ManageMessages))
                    throw new Exception("You do not have permission to manage pins in the current channel.");
            }

            await DatabaseHelper.SetAutopinLimit(ctx.Channel.Id, limit);

            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "📌 $autopin",
                Description = $"Updated Configuration"
            };
            if (limit == 0)
                Builder.AddField(name: "Configuration", value: $"Messages in this channel will never be pinned by bbot.");
            else
                Builder.AddField(name: "Configuration", value: $"Messages in this channel will be pinned when they reach **{limit}** pushpin reactions.");
            Builder.AddField(name: "Info", value: $"If you haven't already, consider setting up the `$autobackup` command so messages can be pinned at any time.");
            await ctx.RespondAsync(embed: Builder.Build());
        }
    }
}