using System.Text;
using System.Linq;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using System.Collections.Generic;

namespace BBotCore
{
    public class HelpFormatter : IHelpFormatter
    {
        // To my knowledge, the order in which WithName and WithDescription are called is not guaranteed
        // So we set two variables to equal to their information and then display the information when Build is called
        string Name;
        string Description;
        // True if a command was passsed as an argument to help
        bool IsCommandPassed = true;
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
            Name = name;
            return this;
        }

        public IHelpFormatter WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public IHelpFormatter WithArguments(IEnumerable<CommandArgument> arguments)
        {
            foreach (var arg in arguments)
            {
                // We want a human-readable string
                StringBuilder sb = new StringBuilder();
                
                // Special case for calling the help command to override the default description
                // (because it meshes badly with our formatting)
                if (arg.Description.Equals("Command to provide help for.")) {
                    Builder.AddField(
                        "Command",
                        $"This parameter is an optional {TypeToReadableString(typeof(string))}. It is the command to provide help for.",
                        inline: false
                    );
                    return this;
                }

                // e.g.
                // This parameter is an optional string of text with default value `4.0.5`
                sb.Append("This parameter is a");
                if (arg.IsOptional)
                    sb.Append("n optional");
                sb.Append($" {TypeToReadableString(arg.Type)}");
                if (arg.DefaultValue != null)
                    sb.Append($", with default value `{arg.DefaultValue}`");
                sb.Append($". It is the {arg.Description}.");

                // Used for TitleCase
                var TextInfo = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo;
                Builder.AddField($"{TextInfo.ToTitleCase(arg.Name)}", sb.ToString(), inline: true);
            }
            return this;
        }

        public IHelpFormatter WithAliases(IEnumerable<string> aliases)
        {
            return this;
        }

        // Will be called when `$help` is called without specifying a command
        public IHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            IsCommandPassed = false;
            // Used here for proper formatting
            // Note we can't use ToTitleCase as we only want the first word in a sentence to be capitalised,
            // and it won't work correctly on the SCP command, listing it as the weird-looking "Scp" command
            string FirstLetterToUpper(string str)
                => char.ToUpper(str[0]) + str.Substring(1);

            foreach (var cmd in subcommands)
            {
                // Hacky special case: We want to use the utilty provided to us by the default help formatter,
                // But want to change the description provided by it
                if (cmd.Name.Equals("help"))
                {
                    Builder.AddField("Help", "Lists all commands or display help for a certain command.", inline: false);
                }
                else
                {
                    Builder.AddField(
                        FirstLetterToUpper(cmd.Name),
                        $"{FirstLetterToUpper(cmd.Description)}.",
                        inline: false);
                }
            }

            return this;
        }

        public IHelpFormatter WithGroupExecutable()
        {
            return this;
        }

        public CommandHelpMessage Build()
        {
            if (!IsCommandPassed)
                Builder.Description = "Listing all of BBot's commands.";
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