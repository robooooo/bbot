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

                // e.g.
                // This parameter is an optional string of text with default value `4.0.5`
                sb.Append("This parameter is a");
                if (arg.IsOptional)
                    sb.Append("n optional");
                sb.Append($" {TypeToReadableString(arg.Type)}");
                if (arg.DefaultValue != null)
                    sb.Append($", with default value `{arg.DefaultValue}`");
                sb.Append(".");

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

        private string TypeToReadableString(System.Type type)
        {
            if (type == typeof(string))
                return "string of text";
            else if (type == typeof(int))
                return "number";
            else if (type == typeof(uint))
                return "positive number";
            else if (type == typeof(DiscordChannel))
                return "channel in this server";
            else if (type == typeof(DiscordEmoji))
                return "emoji in this server";
            else
                return "unknown (submit a bug report?)";
        }
    }
}