using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using Microsoft.Data.Sqlite;

namespace BBotCore
{
    public class Program
    {
        static DiscordClient Discord;
        static CommandsNextExtension Commands;
        static InteractivityExtension Interactivity;

        public static void Main(string[] args)
        {
            // Initialise with bot token
            // string Token = Environment.GetEnvironmentVariable("DISCORD_BETA_TOKEN");
            string Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            Discord = new DiscordClient(new DiscordConfiguration
            {
                Token = Token,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug,
                MessageCacheSize = 128,
                // Caused issues with crashing & stalling 
                // However, we'll try it for now
                AutoReconnect = true,
            });

            Commands = Discord.UseCommandsNext(new CommandsNextConfiguration
            {
                CaseSensitive = false,
                StringPrefixes = new String[] { "$" },
                EnableDefaultHelp = true,
                EnableDms = false,
                EnableMentionPrefix = true,
            });

            Interactivity = Discord.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(1),
            });

            Discord.Ready += async (e) =>
            {
                await Discord.UpdateStatusAsync(new DiscordActivity("$changelog 4.3.0", ActivityType.Watching));
            };

            Commands.CommandErrored += Events.CommandErrored;
            Discord.MessageReactionAdded += Events.MessageReactionAdded;
            Discord.ChannelPinsUpdated += Events.ChannelPinsUpdated;

            Commands.RegisterCommands<Commands>();
            Commands.SetHelpFormatter<HelpFormatter>();
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            await Discord.ConnectAsync();
            await Task.Delay(-1);
        }

    }
}
