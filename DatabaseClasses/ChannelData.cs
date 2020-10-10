using LiteDB;

namespace BBotCore.Db
{
    // POCO class for guild-associated data
    public class ChannelData
    {
        public uint? AutopinLimit { get; set; }

        public ulong? AutobackupDest { get; set; }
    }
}
