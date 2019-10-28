using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace BBotCore
{
    public partial class Commands
    {
        [Command("windmill")]
        [Description("Windmill of friendship! <3")]
        // REFACTOR: Should just remove this lol
        public async Task Windmill(CommandContext ctx,
            [Description("First icon in the windmill.")] DiscordEmoji centre,
            [Description("Second icon in the windmill.")] DiscordEmoji background
        )
        {
            await ctx.RespondAsync($@"
{centre}{background}{centre}{centre}{centre}
{centre}{background}{centre}{background}{background}
{centre}{centre}{centre}{centre}{centre}
{background}{background}{centre}{background}{centre}
{centre}{centre}{centre}{background}{centre}");
        }
    }
}
