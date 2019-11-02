using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace BBotCore
{
    public partial class Commands
    {
        [Command("random")]
        [Aliases("dice")]
        [Description("generates a random number between two bounds")]
        public async Task Roll(
            CommandContext ctx,
            [Description("inclusive lower bound of the random number")] int lower,
            [Description("inclusive higher bound of the random number")] int higher
        )
        {
            // Add one as RNG.Next excludes its upper bound
            int Num = RNG.Next(lower, higher + 1);

            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "🎲 $random",
                Description = $"From {lower} to {higher}"
            };
            Builder.AddField(name: "Result", value: $"{Num}");
            await ctx.RespondAsync(embed: Builder.Build());
        }
    }
}
