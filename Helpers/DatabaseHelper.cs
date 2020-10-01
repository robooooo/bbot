using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using LiteDB;
using LiteDB.Async;
using LanguageExt;

namespace BBotCore
{
    public class DatabaseHelper
    {
        public DataHelper<Db.GuildData> Guilds { get; private set; }
        public DataHelper<Db.ChannelData> Channels { get; private set; }

        // Convert ulong id used by dsharpplus to bson id used by litedb
        private static BsonValue ToId(ulong id) => new BsonValue((double)id);

        public class DataHelper<T> where T: new()
        {
            private LiteCollectionAsync<T> Collection;
            public DataHelper(LiteCollectionAsync<T> col) => Collection = col;
            public async Task<T> Get(ulong id) => await Collection.FindByIdAsync(ToId(id)) ?? new T();
            public async Task<bool> Set(ulong id, T value) => await Collection.UpsertAsync(ToId(id), value);

            public async Task<bool> Update(ulong id, Action<T> mapping) {
                T data = await Get(id) ?? new T();
                mapping(data);
                return await Set(id, data);
            }
        }

        public DatabaseHelper()
        {
            var Database = new LiteDatabaseAsync(Environment.GetEnvironmentVariable("BBOT_LITEDB_PATH"));
            Channels = new DataHelper<Db.ChannelData>(Database.GetCollection<Db.ChannelData>("channels"));
            Guilds = new DataHelper<Db.GuildData>(Database.GetCollection<Db.GuildData>("guilds"));
        }
    }
}