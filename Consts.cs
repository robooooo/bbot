using System.Collections.Generic;

namespace BBotCore
{
    public static class Consts
    {
        public static int EMBED_COLOUR = 0xFFC800;

        public static int BEATS_BETWEEN_STATUSES = 15;

        public static int AUTOBACKUP_THRESHOLD = 3;

        public static ulong ANNOUNCEMENTS_CHANNEL = 756608739930407124;

        // I was wanting to have these links to upvote pages as statuses.
        // However, since bots do not allow custom statuses, the text is not copyable.
        // That means there's not much point in having them.
        public static string[] STATUS_MESSAGES = new string[] {
            "$changelog LATEST_VERSION",
            "$about",
            // "$help",
            "In GUILDS_JOINED guilds",
            // "top.gg/bot/362666654452416524/vote",
            // "discordbotlist.com/bots/bbot/upvote",
            // "bots.ondiscord.xyz/bots/362666654452416524/review",
        };

        public static Dictionary<string, string> VERSION_INFO = new Dictionary<string, string>()
            {
                {
                    "4.4.0",

                    "- Added the `prefix` command which allows mods to change bbot's prefix in their servers.\n" + 
                    "- Added an announcements section to the `about` command for status updates and important info.\n" +
                    "- Mitigated some performance concerns in in various places and commands.\n" +
                    "- Changed the `search` command to use paginated results, reducing chat spam.\n" +
                    "- Fixed a bug in the `search` command that caused certain checks to apply to all servers\n." +
                    "- Completely rewrote all of the backup functionality, fixing a few bugs with embeds on the way\n" +
                    "(Please get in touch with me if you find an issue with this!)\n" +
                    "- Changed some commands to use more robust permission checks.\n" +
                    "- Changed the `changelog` command to use paginated results, displaying older versions." +
                    "- Tweaked the display format for a few commands to better align with new features.\n" +
                    "- Changed bbot's internal database and related APIs to a more extensible format.\n" +
                    "- Made huge underlying interal tweaks, with more to come, allowing for future updates to come faster."
                },
                {
                    "4.3.6",

                    "- Amended the `search` function to respect discord's terms of service with respect to NSFW content."
                },
                {
                    "4.3.5",

                    "- Fixed bbot thinking it didn't recognise the `.gif` filetype in backups, even though it does.\n" +
                    "- Fixed bbot thinking it didn't recognise images with certain URLs, even though it does.\n" +
                    "- Fixed bbot ignoring many images/files in a post, now it will link back to the original post."
                },
                {
                    "4.3.4",

                    "- Added the ability for bbot to update guild-count statistics on various bot-listing websites.\n" +
                    "- Minor internal tweaks involving bbot's internal usage of various APIs."
                },
                {
                    "4.3.3",

                    "- Updated the `$backup` command to handle file formats unsupported by discord in a cleaner manner.\n" +
                    "- Thumbnail support for `.mp4` videos in the `$backup` command is not included with this update, but is on the radar."
                },
                {
                    "4.3.2",

                    "- Added an `about` command for bbot-related links.\n" +
                    "- We've been approved on several bot-listing websites!\n" +
                    "- Feel free to upvote bbot on one of discord's many bot lists!\n" +
                    "- Added rotating status messages, including a guild count."
                },
                {
                    "4.3.1",

                    "- Fixed a bug where backup-related commands would sporadically fail.\n" +
                    "- Fixed a related bug where icons in the backup command would stop working if users changed their avatars.\n" +
                    "- Fixed a bug where some search results would be malformatted.\n" +
                    "- Fixed an issue the version number in bbot's status would often be incorrect."
                },
                {
                    "4.3.0",

                    " - Updated the internal framework that BBot uses, allowing for more features to be added.\n" +
                    " - Updated the `help` command to list command's aliases." +
                    " - Updated the `autopin` and `autobackup` commands; now both of these features can be disabled by providing no arguments.\n" +
                    " - Due to this, the `noautobackup` command has been removed.\n" +
                    " - Additionally, the `random` command has been removed.\n" +
                    " - Fixed an issue where disabling auto-backups in a channel did not require the manage messages permission." +
                    " - Updated the wording in many commands to fix inconsistencies and bugs."
                },
                {
                    "4.2.0",

                    " - **Added `autopin` and `autobackup` features!**\n" +
                    " - The `autopin` command allows you to set a limit, if the number of pushpin reactions on a message in the current channel reaches this point, the message is pinned.\n" +
                    " - The `autobackup` command allows you to set a channel, if the pin limit in the current channel is near, the bot will backup all of the messages!\n" +
                    " - For the time being, the `$nobackup` command can be used to disable said autobackup functionality.\n" +
                    " - Minor changes to spelling of some help information."
                },
                {
                    "4.1.0",

                    " - **Major overhaul of BBot**\n" +
                    " - Plenty of under-the-hood optimisations and improvements.\n" +
                    " - Added a custom help formatter, try `help` and take it for a spin!\n" +
                    " - Changed around some older commands to work a bit more intuitively.\n" +
                    " - `scp` will now provide much more accurate results.\n" +
                    " - `search` is now slightly easier to use and looks better.\n" +
                    " - **Major improvements to `backup`.**\n" +
                    " - `backup` now links to posts when something failed to be displayed properly.\n" +
                    " - Otherwise, the post is linked under the author's name.\n" +
                    " - `backup` will no longer stop displaying author's avatars when they change them.\n" +
                    " - `backup` will now support messages contained in embeds."

                },
                {
                    "4.0.5",

                    "- It used to be the case that `backup` would crash the bot. Now it isn't.\n" +
                    "- Isn't that just neat?"
                },
                {
                    "4.0.4",

                    "- Added an `SCP` command due to a user's request.\n" +
                    "- The command fetches an SCP or tale from the scp site."
                },
                {
                    "4.0.3",

                    "- Trialing the `$search` command having inline results (saves space?)\n" +
                    "- `backup` now links to the original post in the footer."
                },
                {
                    "4.0.2",

                    "- (Hopefully) fixed an issue with the bot crashing and not being able to resume."
                },
                {
                    "4.0.1",

                    "- Fixed a bug where `search` results would contain spaces.\n" +
                    "- Fixed a bug where `search` results would have malformatted links.\n" +
                    "- `backup` results now feature a link to the post.\n"
                },
                {
                    "4.0.0",

                    "**Fourth re-write of bbot!**\n" +
                    "**BBot is now hosted**, for a (hopefully) permanent uptime.\n" +
                    "- Added the `$changelog` command.\n" +
                    "- Changed the `$search` provider from DuckDuckGo to Google.\n" +
                    "- Changed the functionality of the `$search` command to show other results.\n" +
                    "- Changed the functonality of the `$search` command to allow showing more results.\n" +
                    "- Tweaked the `$roll` command to allow a numerical identifier.\n" +
                    "TODO: BBot will have a more customised help menu.\n"
                },
                {
                    "3.0.0",

                    "Previous version of bbot."
                },
            };
    }
}