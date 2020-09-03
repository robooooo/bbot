using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace BBotCore
{
    public partial class Commands
    {
        [Command("prefix")]
        [Description("overrides the default prefix for this guild")]
        [RequireUserPermissions(Permissions.ManageGuild)]
        public async Task Prefix(CommandContext ctx,
            [Description("new prefix to use in this guild, but can be omitted to return to the default")] string prefix)
        {
            if (prefix.Length > 10)
                throw new Exception("The new prefix exceeds the maximum of ten characters.");

            Services.DatabaseHelper.Guilds.Update(ctx.Guild.Id, dat => dat.Prefix = prefix);

            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "🤖 $prefix",
                Description = $"Updated Configuration"
            };

            Builder.AddField(name: "Configuration", value: $"The new prefix in this guild is now **{prefix}** for all channels.");
            Builder.AddField(name: "Info", value: $"This change can be reverted by typing only `$prefix` with no arguments.");
            Builder.AddField(name: "Info", value: $"If you forget the prefix here, you can use commands by mentioning bbot instead.");
            await ctx.RespondAsync(embed: Builder);
        }

        [Command("prefix")]
        public async Task Prefix(CommandContext ctx)
        {
            Services.DatabaseHelper.Guilds.Update(ctx.Guild.Id, dat => dat.Prefix = "$");

            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "🤖 $prefix",
                Description = $"Updated Configuration"
            };

            Builder.AddField(name: "Configuration", value: $"The new prefix in this guild is **$**, which is the default.");
            await ctx.RespondAsync(embed: Builder);
        }
    }
}