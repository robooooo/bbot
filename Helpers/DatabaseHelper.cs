using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Data.Sqlite;

namespace BBotCore
{
    public class DatabaseHelper
    {
        private bool Ready = false;
        private string ConnString = $"DataSource={Environment.GetEnvironmentVariable("BBOT_DB_PATH")}";

        // We can't execute queries in the constructor 
        // (doing it synchronously crashes the bot for some reason, and we can't do it async because the constructor isn't asynchronous).
        // Therefore, run this at the start of each command, but only if we haven't done it already.
        public async Task InitSelf()
        {
            if (Ready)
                return;

            using (var conn = new SqliteConnection(ConnString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS Channels (
                        channelId INTEGER PRIMARY KEY,
                        autopinLimit INTEGER,
                        autobackupDest INTEGER
                    )
                ";
                await cmd.ExecuteNonQueryAsync();
            }

            Ready = true;
        }

        public async Task SetAutopinLimit(ulong channelId, uint limit)
        {
            await InitSelf();
            using (var conn = new SqliteConnection(ConnString))
            {
                conn.Open();

                var create = conn.CreateCommand();
                create.CommandText =
                @"
                    INSERT OR IGNORE INTO Channels
                    VALUES ($channel, NULL, NULL)
                ";
                create.Parameters.AddWithValue("$channel", channelId);
                await create.ExecuteNonQueryAsync();

                var update = conn.CreateCommand();
                update.CommandText =
                @"
                    UPDATE Channels
                    SET autopinLimit = $limit
                    WHERE channelId = $channel
                ";
                update.Parameters.AddWithValue("$limit", limit);
                update.Parameters.AddWithValue("$channel", channelId);
                await update.ExecuteNonQueryAsync();

            }
        }

        public async Task SetAutobackupDestination(ulong channelId, ulong destinationId)
        {
            await InitSelf();
            using (var conn = new SqliteConnection(ConnString))
            {
                conn.Open();

                var create = conn.CreateCommand();
                create.CommandText =
                @"
                    INSERT OR IGNORE INTO Channels
                    VALUES ($channel, NULL, NULL)
                ";
                // The parameter for this is supposed to be a uint, and we have a ulong.
                // We can't simply cast between them because of the possibility of overflow.
                // We have to cast between the two while keeping the bit pattern identical.
                create.Parameters.AddWithValue("$channel", unchecked((long)channelId));
                await create.ExecuteNonQueryAsync();

                var update = conn.CreateCommand();
                update.CommandText =
                @"
                    UPDATE Channels
                    SET autobackupDest = $dest
                    WHERE channelId = $channel
                ";
                update.Parameters.AddWithValue("$dest", destinationId);
                update.Parameters.AddWithValue("$channel", channelId);
                await update.ExecuteNonQueryAsync();
            }
        }

        public async Task<uint> GetAutopinLimit(ulong channelId)
        {
            await InitSelf();
            using (var conn = new SqliteConnection(ConnString))
            {
                conn.Open();

                var select = conn.CreateCommand();
                select.CommandText =
                @"
                    SELECT autopinLimit
                    FROM Channels
                    WHERE channelId = $channel
                ";
                select.Parameters.AddWithValue("$channel", channelId);
                var res = await select.ExecuteScalarAsync();
                return res == null ? 0 : (uint)((long)res);
            }
        }

        public async Task<ulong?> GetAutobackupDestination(ulong channelId)
        {
            await InitSelf();
            using (var conn = new SqliteConnection(ConnString))
            {
                conn.Open();

                var select = conn.CreateCommand();
                select.CommandText =
                @"
                    SELECT autobackupDest
                    FROM Channels
                    WHERE channelId = $channel
                ";
                select.Parameters.AddWithValue("$channel", channelId);
                var res = await select.ExecuteScalarAsync();
                if (res == null)
                    return null;
                // Unsure how this differs from null
                if (res is System.DBNull)
                    return null;

                long rawRes = (long)res;
                ulong castRes = unchecked((ulong)rawRes);
                return (ulong?)castRes;
            }
        }


        public async Task ClearAutobackupDestination(ulong channelId)
        {
            await InitSelf();
            using (var conn = new SqliteConnection(ConnString))
            {
                conn.Open();

                var create = conn.CreateCommand();
                create.CommandText =
                @"
                    INSERT OR IGNORE INTO Channels
                    VALUES ($channel, NULL, NULL)
                ";
                create.Parameters.AddWithValue("$channel", channelId);
                await create.ExecuteNonQueryAsync();

                var update = conn.CreateCommand();
                update.CommandText =
                @"
                    UPDATE Channels
                    SET autobackupDest = NULL
                    WHERE channelId = $channel
                ";
                update.Parameters.AddWithValue("$channel", channelId);
                await update.ExecuteNonQueryAsync();
            }
        }
    }
}