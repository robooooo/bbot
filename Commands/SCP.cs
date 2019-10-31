using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace BBotCore
{
    public partial class Commands
    {
        [Command("SCP")]
        [Description("links to an SCP or tale from the SCP wiki")]
        public async Task SCP(CommandContext ctx,
            [RemainingText(), Description("SCP number or tale name to search for")] string page)
        {
            // REFACTOR: Use a search instead?
            string URL;
            if (int.TryParse(page, out int num))
                URL = $"http://www.scp-wiki.net/scp-{num.ToString().PadLeft(3, '0')}";
            else
                URL = $"http://www.scp-wiki.net/{page.Replace(" ", "-")}";

            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "🕵️ $scp",
                // Description = $"Access authorized.",
            };
            Builder.AddField("Result:", URL);

            await ctx.RespondAsync(embed: Builder.Build());
        }
    }
}
