using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using Microsoft.Data.Sqlite;
using System.Linq;

// TODO:
// [!] SEO on bot listing pages
// [+] Switch to new db backend
// [!] Async for litedb
// [+] Set prefix handler
// [+] Set prefix command
// [!] Command framework for replies
// [ ] Command framework for missing arguments
// [ ] Setup dialog
// [+] Pagination on search
// [+] Pagination on changelog
// [+] Fix search expansion bug
// [+] Change permission checks to use attributes
// [!] Rework permissions/structure in backup funcs 
// ^ is this even possible to do cleanly?
// [+] Add announcements to $about
// [+] Remove default prefix from commands
// [ ] Pin archive/backup to file functionality

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
            string Token = Environment.GetEnvironmentVariable("DISCORD_BETA_TOKEN");
            // string Token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
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
                EnableDefaultHelp = true,
                EnableDms = false,
                EnableMentionPrefix = true,
                PrefixResolver = async (message) => {
                    var Prefix = (await Services.DatabaseHelper.Guilds.Get(message.Channel.GuildId)).Prefix ?? "$";
                    return message.GetStringPrefixLength(Prefix, StringComparison.OrdinalIgnoreCase);
                }
            });

            Interactivity = Discord.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(1),
            });

            Discord.Ready += async (e) =>
            {
                string LatestVersion = Consts.VERSION_INFO.First().Key;
                await Discord.UpdateStatusAsync(new DiscordActivity($"$changelog {LatestVersion}", ActivityType.Playing));
            };

            Commands.CommandErrored += Events.CommandErrored;
            Discord.MessageReactionAdded += Events.MessageReactionAdded;
            Discord.ChannelPinsUpdated += Events.ChannelPinsUpdated;
            Discord.Heartbeated += Events.HeartbeatTimer;

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
