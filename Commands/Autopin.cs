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
        [Command("autopin")]
        [Description("configures automatically pinning messages when they accumulate a large number of reactions")]
        public async Task Autopin(CommandContext ctx,
            [Description("number of 📌 reactions that must be reached for the message to be pinned, which can be set to 0 to disable the feature")] uint limit
        )
        {
            
        }
    }
}