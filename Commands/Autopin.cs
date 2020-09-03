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
        [RequireUserPermissions(Permissions.ManageMessages)]
        public async Task Autopin(CommandContext ctx,
            [Description("number of 📌 reactions that must be reached for the message to be pinned, but can be omitted to disable the feature instead")] uint limit
        )
        {
            if (limit == 0)
                throw new ArgumentException("The reaction threshold for this command cannot be zero.");

            // await Services.DatabaseHelper.SetAutopinLimit(ctx.Channel.Id, limit);
            Services.DatabaseHelper.Channels.Update(ctx.Channel.Id, dat => dat.AutopinLimit = limit);

            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "📌 $autopin",
                Description = $"Updated Configuration"
            };

            Builder.AddField(name: "Configuration", value: $"Messages in this channel will be pinned when they reach **{limit}** pushpin reactions.");
            Builder.AddField(name: "Info", value: $"This feature can be disabled by typing only `$autopin` with no arguments.");
            Builder.AddField(name: "Info", value: $"If you haven't already, consider setting up the `$autobackup` command so messages can be pinned by bbot if needed.");
            await ctx.RespondAsync(embed: Builder);
        }

        [Command("autopin")]
        public async Task Autopin(CommandContext ctx)
        {
            // await Services.DatabaseHelper.SetAutopinLimit(ctx.Channel.Id, 0);
            Services.DatabaseHelper.Channels.Update(ctx.Channel.Id, dat => dat.AutopinLimit = 0);

            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "📌 $autopin",
                Description = $"Updated Configuration"
            };

            Builder.AddField(name: "Configuration", value: $"Messages in this channel will never be pinned by bbot.");
            await ctx.RespondAsync(embed: Builder.Build());
        }
    }
}