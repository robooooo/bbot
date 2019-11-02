using System.Collections.Generic;

namespace BBotCore
{
    public static class Consts
    {
        public static int EMBED_COLOUR = 0xFFC800;


        public static Dictionary<string, string> VERSION_INFO = new Dictionary<string, string>()
            {
                {
                    "4.1.0",

                    " - **Major overhaul of BBot**\n" +
                    " - Plenty of under-the-hood optimisations and improvements.\n" +
                    " - Added a custom help formatter, try `$help` and take it for a spin!\n" +
                    " - Changed around some older commands to work a bit more intuitively\n" +
                    " - `$scp` will now provide much more accurate results\n" +
                    " - **Major improvements to `$backup`.**\n" +
                    " - `$backup` now links to posts when something failed to be displayed properly.\n" +
                    " - Otherwise, the post is linked under the author's name.\n" +
                    " - `$backup` will no longer stop displaying author's avatars when they change them.\n" +
                    " - `$backup` will now support users using citador or similar posts.\n" +
                    " - More specifically, when a message has no text, but an embed in it does...\n" +
                    " - ...it will take the message content from an embed."

                },
                {
                    "4.0.5",

                    "- It used to be the case that `$backup` would crash the bot. Now it isn't.\n" +
                    "- Isn't that just neat?"
                },
                {
                    "4.0.4",

                    "- Added an `$SCP` command due to a user's request.\n" +
                    "- The command fetches an SCP or tale from the scp site."
                },
                {
                    "4.0.3",

                    "- Trialing the `$search` command having inline results (saves space?)\n" +
                    "- `$backup` now links to the original post in the footer."
                },
                {
                    "4.0.2",

                    "- (Hopefully) fixed an issue with the bot crashing and not being able to resume."
                },
                {
                    "4.0.1",

                    "- Fixed a bug where `$search` results would contain spaces.\n" +
                    "- Fixed a bug where `$search` results would have malformatted links.\n" +
                    "- `$backup` results now feature a link to the post.\n" +
                    "- Currently, these may open in a browser tab - discord's fault, they won't if you paste them into chat."
                },
                {
                    "4.0.0",

                    "**Fourth re-write of BBot!**\n" +
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

                    "Previous version of BBot."
                },
            };
    }
}