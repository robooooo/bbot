using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;


namespace BBotCore
{
    public class HelpFormatter : BaseHelpFormatter
    {
        private DiscordEmbedBuilder Builder;

        // To my knowledge, the order in which WithName and WithDescription are called is not guaranteed
        // So we set two variables to equal to their information and then display the information when Build is called
        string Name;
        string Description;
        // True if a command was passsed as an argument to help
        bool IsCommandPassed;


        // Convert System.Type to readable String
        private Func<System.Type, String> TypeToString;

        public HelpFormatter(CommandContext ctx) : base(ctx)
        {
            TypeToString = ctx.CommandsNext.GetUserFriendlyTypeName;

            // Initialise with constant information 
            Builder = new DiscordEmbedBuilder()
            {
                Color = new DiscordColor(Consts.EMBED_COLOUR),
                Title = "❓ $help",
                Footer = new DiscordEmbedBuilder.EmbedFooter() {
                    Text = "Also try `$about` for bbot-related information and links."
                }
            };
        }

        public override BaseHelpFormatter WithCommand(Command cmd)
        {
            IsCommandPassed = true;

            this.Name = cmd.Name;

            // Again, hack to get around static descriptions for help command
            if (cmd.Description.Equals("Displays command help."))
                Description = "Lists all commands or display help for a certain command.";
            else
                Description = cmd.Description;

            UseAliases(cmd.Aliases);
            UseArguments(cmd.Overloads.First().Arguments);

            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {

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
                    Builder.AddField("Help", "Lists all commands or displays help for a certain command.", inline: false);
                }
                else
                {
                    Builder.AddField(
                        FirstLetterToUpper(cmd.Name),
                        $"{FirstLetterToUpper(cmd.Description)}.",
                        inline: false
                    );
                }
            }

            return this;
        }

        public void UseArguments(IEnumerable<CommandArgument> arguments)
        {
            foreach (var arg in arguments)
            {
                // We want a human-readable string
                StringBuilder sb = new StringBuilder();

                // Special case for calling the help command to override the default description
                // (because it meshes badly with our formatting)
                if (arg.Description.Equals("Command to provide help for."))
                {
                    Builder.AddField(
                        "Command",
                        $"This parameter is an optional {TypeToString(typeof(string))}. It is the command to provide help for.",
                        inline: false
                    );
                    return;
                }

                // e.g.
                // This parameter is an optional string of text with default value `4.0.5`
                sb.Append("This parameter is a");
                // Special case for commands with overloads, because the current help formatter cannot list overloads (and this format looks nicer)
                if (arg.IsOptional || arg.Description.Contains("can be omitted"))
                    sb.Append("n optional");
                sb.Append($" {TypeToString(arg.Type)}");
                if (arg.DefaultValue != null)
                    sb.Append($", with default value `{arg.DefaultValue}`");
                sb.Append($". It is the {arg.Description}.");

                // Used for TitleCase
                var TextInfo = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo;
                Builder.AddField($"{TextInfo.ToTitleCase(arg.Name)}", sb.ToString(), inline: true);
            }
        }

        public void UseAliases(IEnumerable<string> aliases)
        {
            var len = aliases.Count();
            if (len == 0)
            {
                Builder.WithFooter(text: "This command has no aliases.");
                return;
            }
            else if (len == 1)
            {
                Builder.WithFooter(text: $"This command is also known as {aliases.ElementAt(0)}.");
                return;
            }

            // We want to provide a comma-seperated list of values, grammatically correct with "and" before the last element.
            StringBuilder sb = new StringBuilder();
            sb.Append("This command is also known as ");
            foreach (var alias in aliases.Take(len - 2))
                sb.Append($"{alias}, ");
            sb.Append($"{aliases.ElementAt(len - 2)} and {aliases.ElementAt(len - 1)}.");

            Builder.WithFooter(text: sb.ToString());
            return;
        }

        public override CommandHelpMessage Build()
        {
            if (!IsCommandPassed)
                Builder.Description = "Listing all of BBot's commands.";
            else
                Builder.Description = $"The command `${Name}` {Description}. Its parameters are shown below.";
            return new CommandHelpMessage(embed: Builder.Build());
        }
    }
}