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
        [Description("simulates a roll for repeated digits or 'dubs'")]
        public async Task RollDigits(CommandContext ctx,
            [Description("size of the roll, for example dubs or trips")] string roll
        )
        {
            // REFACTOR: Maybe clean this up?
            int Value;
            string[] Quantifiers = new string[] { "dubs", "trips", "quads", "pents", "hexts", "septs", "octs" };

            int Ind = Array.IndexOf(Quantifiers, roll);
            if (Ind == -1)
                throw new Exception($"The parameter '{roll}' is not valid. Dubs, trips, quads, pents, hexts, septs and octs are the avaliable rolls.");
            // We add one because of zero-indexing, and add one again because we start from two (skipping singles)
            Value = Ind + 2;

            // The range is from [0, 10^n) as RNG.Next excludes the upper bound
            int Result = RNG.Next(0, (int)Math.Pow(10, Value));
            string Out = Result.ToString().PadLeft(Value, '0');

            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "🍀 $roll",
                Description = $"Check em'"
            };
            Builder.AddField(name: "Result:", value: $"{Out}");
            await ctx.RespondAsync(embed: Builder.Build());
        }
    }
}
