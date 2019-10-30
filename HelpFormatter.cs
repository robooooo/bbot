using System.Text;
using System.Linq;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Interactivity;

namespace BBotCore
{
    public class HelpFormatter : IHelpFormatter
    {
        private DiscordEmbedBuilder Builder;

        public HelpFormatter()
        {
            Builder = new DiscordEmbedBuilder()
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = ". $help",
            };
        }


    }
}