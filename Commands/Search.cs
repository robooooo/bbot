﻿using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace BBotCore
{
    public partial class Commands
    {
        [Command("search")]
        [Aliases("s", "google")]
        [Description("searches for a query on google")]
        public async Task Search(CommandContext ctx,
                [RemainingText(), Description("term to search for")] string query
        )
        {
            // Snippet gets our search results stored in Search
            var CSE = CSS.Cse.List(query);
            CSE.Cx = Environment.GetEnvironmentVariable("SEARCH_CX");
            var Search = await CSE.ExecuteAsync();

            // Needed to tabulate search results
            string[] Titles = new string[]
            {
                "First", "Second", "Third", "Fourth", "Fifth", "Sixth", "Seventh", "Eighth", "Ninth", "Tenth",
            };

            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "🔍 $search",
                Description = $"Showing 4 search results",
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    Text = "React to this message to show all results."
                }
            };

            for (int i = 0; i < 4; i++)
                if (Search.Items[i] != null)
                    Builder.AddField($"{Titles[i]} Result", Search.Items[i].Link, inline: false);

            // REFACTOR: Add react?
            var EmbedMessage = await ctx.RespondAsync(embed: Builder.Build());
            var UrlMesage = await ctx.RespondAsync($"{Search.Items[0].Link}");
            // Make it easier for users to react by adding our own reaction

            // This is fine, since DM commands are turned off
            var Permissions = ctx.Channel.PermissionsFor(ctx.Guild.CurrentMember);
            if (Permissions.HasPermission(Permissions.AddReactions) || Permissions.HasPermission(Permissions.Administrator))
                await EmbedMessage.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":mag:"));

            // We want to wait a bit so that we don't trigger from our own reaction
            await Task.Delay(1500);
            var Interact = ctx.Client.GetInteractivityModule();
            var Result = await Interact.WaitForReactionAsync(e => true, timeoutoverride: TimeSpan.FromSeconds(60));

            if (Result != null)
            {
                DiscordEmbedBuilder EditedBuilder = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor(Consts.EMBED_COLOUR),
                    Title = "🔍 $search",
                    Description = $"Showing all search results.",
                    Footer = new DiscordEmbedBuilder.EmbedFooter()
                    {
                        Text = "All results have been shown."
                    }
                };

                for (int i = 0; i < 10; i++)
                    if (Search.Items[i] != null)
                        EditedBuilder.AddField($"{Titles[i]} Result", Search.Items[i].FormattedUrl, inline: false);

                // We delete this because it looks better and will not spam the chat
                await UrlMesage.DeleteAsync();

                await EmbedMessage.ModifyAsync(embed: EditedBuilder.Build());
                if (ctx.Channel.PermissionsFor(ctx.Guild.CurrentMember).HasPermission(Permissions.ManageEmojis))
                    await EmbedMessage.DeleteAllReactionsAsync();
            }
        }
    }
}
