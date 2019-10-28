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
        [Description("Generates a random number.\nWith two arguments provided, the range is `arg1` to `arg2`.\nWith one, it is 1 to `arg1`.\nWith none, it is 1 to 6.")]
        public async Task Roll(
            CommandContext ctx,
            [Description("An optional range of values for the command.")] params int[] arguments
        )
        {
            int First = 1, Second = 6;
            if (arguments.Length >= 2)
            {
                First = arguments[0];
                Second = arguments[1];
            }
            else if (arguments.Length == 1)
            {
                Second = arguments[0];
            }

            int Num = RNG.Next(First, Second + 1);

            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(0xFFC800),
                Title = "🎲 $random",
                Description = $"From {First} to {Second}"
            };
            Builder.AddField(name: "Result:", value: $"{Num}");
            await ctx.RespondAsync(embed: Builder.Build());
        }
    }
}
