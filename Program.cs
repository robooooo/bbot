using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;


namespace BBotCore
{
    public class Program
    {
        static DiscordClient Discord;
        static CommandsNextModule Commands;
        static InteractivityModule Interactivity;

        public static void Main(string[] args)
        {
            // Init. with bot token
            string Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            Discord = new DiscordClient(new DiscordConfiguration
            {
                Token = Token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug,
                MessageCacheSize = 128,
                AutoReconnect = false,
            });
            Commands = Discord.UseCommandsNext(new CommandsNextConfiguration
            {
                CaseSensitive = false,
                StringPrefix = "$",
                EnableDefaultHelp = true,
                EnableDms = false,
                EnableMentionPrefix = true,
            });
            Commands.RegisterCommands<Commands>();
            Interactivity = Discord.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(1),
            });

            Commands.CommandErrored += Commands_CommandErrored;
            Discord.Ready += async (e) =>
            {
                await Discord.UpdateStatusAsync(new DiscordGame("$changelog 4.0.2"));
            };

            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            DiscordEmbedBuilder Builder = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(0xB00020),
                Title = "❌ Error!",
                Description = $"In ${e.Command.Name}",
            };
            Builder.AddField("Reason:", e.Exception.Message);
            //Builder.AddField("Stack Trace:", $"```{e.Exception.StackTrace}```");
            await e.Context.Channel.SendMessageAsync(embed: Builder.Build());
        }

        private static async Task MainAsync(string[] args)
        {
            await Discord.ConnectAsync();
            await Task.Delay(-1);
        }

    }
}
