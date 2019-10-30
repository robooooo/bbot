using System.Text;
using System.Linq;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Interactivity;
using System.Collections.Generic;

namespace BBotCore
{
    public class HelpFormatter : IHelpFormatter
    {
        private DiscordEmbedBuilder Builder;

        public HelpFormatter()
        {
            // Initialise with constant information 
            Builder = new DiscordEmbedBuilder()
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "❓ $help",
            };
        }

        public IHelpFormatter WithCommandName(string name)
        {
            Builder.Description = $"Helping you with ${name}";
            return this;
        }

        public IHelpFormatter WithDescription(string description)
        {
            Builder.AddField("Description:", description, inline: false);
            return this;
        }

        public IHelpFormatter WithArguments(IEnumerable<CommandArgument> arguments)
        {
            foreach (var arg in arguments)
            {
                // We want a human-readable string
                StringBuilder sb = new StringBuilder();
                if (arg.DefaultValue != null)
                    sb.Append($"This argument has default value `{arg.DefaultValue}`. ");
                sb.Append("It is ");
                if (arg.IsOptional)
                    sb.Append($"optional and ");
                sb.Append($"of type `{arg.Type}`. ");
                sb.Append(arg.Description);

                Builder.AddField($"{arg.Name}:", sb.ToString(), inline: true);
            }
            return this;
        }

        public IHelpFormatter WithAliases(IEnumerable<string> aliases)
        {
            return this;
        }

        public IHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            return this;
        }

        public IHelpFormatter WithGroupExecutable()
        {
            return this;
        }

        public CommandHelpMessage Build()
        {
            return new CommandHelpMessage(embed: Builder.Build());
        }
    }
}