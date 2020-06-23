using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace BBotCore
{
    public class DatabaseHelper
    {

        public void setAutopinLimit(uint limit) {

        }

        public void setAutobackupDestination(ulong destinationId) {

        }
    
        public uint getAutopinLimit(ulong channelId) {
            return 1;
        }

        public ulong getAutobackupDestination(ulong channelId) {
            return 534115279358394379;
        }
    }
}