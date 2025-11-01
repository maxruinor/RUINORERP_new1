using System;
using System.IO;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace RUINORERP.Server.BNR
{
    /// <summary>
    /// {DB:ORDER/00000} 
    /// 基于数据库的序号参数
    /// 支持嵌套表达式，确保每个业务类型有独立的序号序列
    /// 支持格式: {DB:key/format} - 普通序号
    ///          {DB:key/format/daily} - 按天重置序号
    ///          {DB:key/format/monthly} - 按月重置序号
    ///          {DB:key/format/yearly} - 按年重置序号
    /// </summary>
    [ParameterType("DB")]
    public class DatabaseSequenceParameter : IParameterHandler
    {
        // 数据库序号服务
        private readonly DatabaseSequenceService _sequenceService;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sequenceService">数据库序列服务</param>
        public DatabaseSequenceParameter(DatabaseSequenceService sequenceService)
        {
            _sequenceService = sequenceService;
        }

        public BNRFactory Factory { get; set; }

        /// <summary>
        /// 执行数据库序号生成
        /// </summary>
        /// <param name="sb">结果字符串构建器</param>
        /// <param name="value">参数值，格式为 key/format</param>
        public void Execute(StringBuilder sb, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value), "数据库序号参数不能为空");
            }

            // 分割键、格式和重置类型
            string[] properties = value.Split('/');
            if (properties.Length < 2)
            {
                throw new ArgumentException($"数据库序号参数格式错误，应为 'key/format' 或 'key/format/resetType'，当前值: {value}");
            }

            string keyPattern = properties[0];
            string format = properties[1];
            string resetType = properties.Length > 2 ? properties[2] : "None";
            
            // 验证重置类型是否有效
            if (!string.IsNullOrEmpty(resetType) && 
                resetType.ToUpper() != "DAILY" && 
                resetType.ToUpper() != "MONTHLY" && 
                resetType.ToUpper() != "YEARLY" && 
                resetType.ToUpper() != "NONE")
            {
                resetType = "None"; // 无效的重置类型，使用默认值
                Console.WriteLine($"警告: 无效的重置类型 '{resetType}'，使用默认值 'None'");
            }
            
            StringBuilder sequenceKeyBuilder = new StringBuilder();
            
            // 处理嵌套表达式来生成唯一的序列键
            // 先检查是否包含嵌套表达式
            if (keyPattern.Contains('{') && keyPattern.Contains('}'))
            {
                // 使用 RuleAnalysis 处理嵌套表达式
                string[] items = RuleAnalysis.Execute(keyPattern);
                
                if (items != null && items.Length > 0)
                {
                    foreach (string item in items)
                    {
                        string[] sps = RuleAnalysis.GetProperties(item);
                        if (sps != null && sps.Length >= 2)
                        {
                            IParameterHandler handler = null;
                            if (Factory.Handlers.TryGetValue(sps[0], out handler))
                            {
                                handler.Execute(sequenceKeyBuilder, sps[1]);
                            }
                        }
                    }
                }
            }
            else
            {
                // 如果没有嵌套表达式，直接使用键值
                sequenceKeyBuilder.Append(keyPattern);
            }
            
            // 确保生成的序列键不为空
            string sequenceKey = sequenceKeyBuilder.ToString();
            if (string.IsNullOrEmpty(sequenceKey))
            {
                // 如果处理后仍然为空，使用固定格式的备用键（不带时间戳）
                // 根据当前业务类型和重置类型生成一个固定的键，确保同一业务使用同一序列
                string businessType = value.Split('/').FirstOrDefault() ?? "DEFAULT";
                sequenceKey = $"DEFAULT_{businessType}";
                Console.WriteLine($"警告: 生成的序列键为空，使用备用键: {sequenceKey}");
            }
            
            // 记录生成的序列键，便于调试
            Console.WriteLine($"正在为序列键 '{sequenceKey}' 生成下一个序号");
            
            // 获取下一个序号值，传入重置类型和格式
            var number = _sequenceService.GetNextSequenceValue(sequenceKey, resetType, format);
            
            // 按指定格式输出序号
            sb.Append(number.ToString(format));
        }
        
        // 移除静态方法，现在使用依赖注入
    }
}