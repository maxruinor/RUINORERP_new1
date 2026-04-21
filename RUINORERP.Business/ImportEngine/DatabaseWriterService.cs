using RUINORERP.Business.ImportEngine.Models;
using RUINORERP.Model.ImportEngine.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.Business.ImportEngine
{
    /// <summary>
    /// 数据库写入服务
    /// 负责将映射后的数据写入数据库，支持Upsert操作
    /// </summary>
    public class DatabaseWriterService
    {
        private readonly ISqlSugarClient _db;

        public DatabaseWriterService(ISqlSugarClient db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// 批量Upsert数据
        /// </summary>
        /// <param name="data">数据表</param>
        /// <param name="profile">导入配置</param>
        /// <param name="remapper">ID重映射引擎（已废弃参数，忽略）</param>
        public async Task<int> BatchUpsertAsync(DataTable data, ImportProfile profile, object remapper = null)
        {
            if (data == null || data.Rows.Count == 0)
            {
                return 0;
            }

            var businessKeys = profile.BusinessKeys?.Where(k => data.Columns.Contains(k)).ToList();
            if (businessKeys == null || !businessKeys.Any())
            {
                // 如果没有业务键，直接插入
                return await InsertRowsAsync(data.Rows.Cast<DataRow>().ToList(), profile.TargetTable);
            }

            int totalAffected = 0;

            // 使用事务确保数据一致性
            try
            {
                await _db.Ado.UseTranAsync(async () =>
                {
                    // 1. 分离新增和更新的数据
                    var (insertRows, updateRows) = await SeparateInsertAndUpdateAsync(data, profile.TargetTable, businessKeys);

                    // 2. 执行插入
                    if (insertRows.Any())
                    {
                        var insertedCount = await InsertRowsAsync(insertRows, profile.TargetTable);
                        totalAffected += insertedCount;
                    }

                    // 3. 执行更新
                    if (updateRows.Any())
                    {
                        totalAffected += await UpdateRowsAsync(updateRows, profile.TargetTable, businessKeys);
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DatabaseWriter] Upsert 失败: {ex.Message}");
                throw;
            }

            return totalAffected;
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        public async Task<int> BatchInsertAsync(DataTable data, ImportProfile profile)
        {
            if (data == null || data.Rows.Count == 0)
            {
                return 0;
            }

            return await InsertRowsAsync(data.Rows.Cast<DataRow>().ToList(), profile.TargetTable);
        }

        /// <summary>
        /// 分离需要插入和更新的数据
        /// </summary>
        private async Task<(List<DataRow> InsertRows, List<DataRow> UpdateRows)> SeparateInsertAndUpdateAsync(
            DataTable data, 
            string tableName, 
            List<string> businessKeys)
        {
            var insertRows = new List<DataRow>();
            var updateRows = new List<DataRow>();

            // 构建查询条件
            var whereConditions = businessKeys.Select(k => $"[{k}] = @{k}").ToList();
            var whereClause = string.Join(" AND ", whereConditions);

            foreach (DataRow row in data.Rows)
            {
                // 检查记录是否已存在
                var parameters = businessKeys.Select(k => new SugarParameter($"@{k}", row[k])).ToArray();
                var sql = $"SELECT COUNT(1) FROM [{tableName}] WHERE {whereClause}";
                var count = Convert.ToInt32(await _db.Ado.GetScalarAsync(sql, parameters));

                if (count > 0)
                {
                    updateRows.Add(row);
                }
                else
                {
                    insertRows.Add(row);
                }
            }

            return (insertRows, updateRows);
        }

        /// <summary>
        /// 插入数据行
        /// </summary>
        private async Task<int> InsertRowsAsync(List<DataRow> rows, string tableName)
        {
            if (!rows.Any()) return 0;

            int insertedCount = 0;
            
            // 批量插入（每批100条）
            const int batchSize = 100;
            for (int i = 0; i < rows.Count; i += batchSize)
            {
                var batch = rows.Skip(i).Take(batchSize).ToList();
                
                // 构建INSERT语句
                var columns = batch[0].Table.Columns.Cast<DataColumn>()
                    .Where(c => c.ColumnName.ToUpper() != "ID") // 排除自增ID
                    .Select(c => c.ColumnName)
                    .ToList();

                if (!columns.Any()) continue;

                var columnNames = string.Join(", ", columns.Select(c => $"[{c}]"));
                var paramNames = string.Join(", ", columns.Select(c => $"@{c}"));

                foreach (var row in batch)
                {
                    var parameters = columns.Select(c => new SugarParameter($"@{c}", row[c] == DBNull.Value ? null : row[c])).ToArray();
                    var sql = $"INSERT INTO [{tableName}] ({columnNames}) VALUES ({paramNames})";
                    
                    await _db.Ado.ExecuteCommandAsync(sql, parameters);
                    insertedCount++;
                }
            }

            return insertedCount;
        }

        /// <summary>
        /// 更新数据行
        /// </summary>
        private async Task<int> UpdateRowsAsync(List<DataRow> rows, string tableName, List<string> businessKeys)
        {
            if (!rows.Any()) return 0;

            int updatedCount = 0;

            foreach (var row in rows)
            {
                // 构建UPDATE语句
                var columns = row.Table.Columns.Cast<DataColumn>()
                    .Where(c => !businessKeys.Contains(c.ColumnName)) // 排除业务键
                    .Select(c => c.ColumnName)
                    .ToList();

                if (!columns.Any()) continue;

                var setClause = string.Join(", ", columns.Select(c => $"[{c}] = @{c}"));
                var whereClause = string.Join(" AND ", businessKeys.Select(k => $"[{k}] = @{k}_where"));

                var parameters = new List<SugarParameter>();
                
                // SET部分的参数
                foreach (var col in columns)
                {
                    parameters.Add(new SugarParameter($"@{col}", row[col] == DBNull.Value ? null : row[col]));
                }
                
                // WHERE部分的参数
                foreach (var key in businessKeys)
                {
                    parameters.Add(new SugarParameter($"@{key}_where", row[key]));
                }

                var sql = $"UPDATE [{tableName}] SET {setClause} WHERE {whereClause}";
                await _db.Ado.ExecuteCommandAsync(sql, parameters.ToArray());
                updatedCount++;
            }

            return updatedCount;
        }
    }
}
