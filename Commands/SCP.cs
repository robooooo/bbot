using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace BBotCore
{
    public partial class Commands
    {
        [Command("scp")]
        [Description("Attempt to fetch an SCP from the offical SCP site.")]
        public async Task SCP(CommandContext ctx,
            [RemainingText(), Description("SCP no. or tale to view.")] string page)
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
