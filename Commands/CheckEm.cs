using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace BBotCore
{
    public partial class Commands
    {
        [Command("roll")]
        [Aliases("check", "checkem")]
        [Description("Roll for repeat digits (use $random for random numbers).")]
        public async Task RollDigits(CommandContext ctx,
            [Description("Quantifier, for example '3' or 'trips'.")] string roll
        )
        {
            // REFACTOR: Maybe clean this up?
            int Value;
            string[] Quantifiers = new string[] { "dubs", "trips", "quads", "pents", "hexts", "septs", "octs" };
            if (int.TryParse(roll, out int n) && n >= 2)
            {
                Value = n;
            }
            else
            {
                int Ind = Array.IndexOf(Quantifiers, roll);
                if (Ind == -1)
                    throw new Exception($"The qualifier '{roll}' is not valid.");
                Value = Ind + 2;
            }

            int Result = RNG.Next(0, (int)Math.Pow(10, Value));
            string Out = Result.ToString().PadLeft(Value, '0');

            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(0xFFC800),
                Title = "🍀 $roll",
                Description = $"Check em'"
            };
            Builder.AddField(name: "Result:", value: $"{Out}");
            await ctx.RespondAsync(embed: Builder.Build());
        }
    }
}
