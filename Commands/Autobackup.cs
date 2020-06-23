using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace BBotCore
{
    public partial class Commands
    {
        [Command("autobackup")]
        [Description("configures automatically backing up the pins to an overflow channel when the limit is near")]
        public async Task Autobackup(CommandContext ctx,
            [Description("channel the pins will be backed up to")] DiscordChannel channel
        )
        {

        }
    }
}