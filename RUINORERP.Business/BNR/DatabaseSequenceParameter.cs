using System;
using System.IO;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace RUINORERP.Business.BNR
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

        public object Factory { get; set; }

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
                System.Diagnostics.Debug.WriteLine($"警告: 无效的重置类型 '{resetType}'，使用默认值 'None'");
            }
            
            // 生成规范的序列键
            string sequenceKey = GenerateStandardSequenceKey(keyPattern, resetType);
            
            // 记录生成的序列键，便于调试
            System.Diagnostics.Debug.WriteLine($"正在为序列键 '{sequenceKey}' 生成下一个序号");
            
            // 获取业务类型（从BNRFactory中获取）
            string businessType = BNRFactory.GetCurrentBusinessType();
            
            // 获取下一个序号值，传入重置类型、格式和业务类型
            var number = _sequenceService.GetNextSequenceValue(sequenceKey, resetType, format, null, businessType);
            
            // 智能处理格式掩码，支持固定格式+无限增长
            try
            {
                // 如果格式以"/"开头，包含数字格式（如"/000"）
                if (format.StartsWith("/") && format.Length > 1)
                {
                    // 提取前缀和数字格式部分
                    string prefix = "/";
                    string numberFormat = format.Substring(1); // 如"000"
                    
                    // 计算格式能表示的最大数字
                    int maxDigits = numberFormat.Length;
                    long maxValue = (long)Math.Pow(10, maxDigits) - 1; // 如000最大是999
                    
                    if (number <= maxValue)
                    {
                        // 当数字小于等于最大可表示值时，使用完整格式
                        sb.Append(number.ToString(format));
                    }
                    else
                    {
                        // 当数字超过格式限制时，前缀+原始数字
                        sb.Append(prefix);
                        sb.Append(number); // 直接显示完整数字，如/1000, /1001
                    }
                }
                else
                {
                    // 其他格式正常处理
                    sb.Append(number.ToString(format));
                }
            }
            catch (FormatException)
            {
                // 如果格式化失败，直接使用前缀+原始数字作为备用方案
                sb.Append("/");
                sb.Append(number);
            }
        }
        
        /// <summary>
        /// 生成规范的序列键
        /// 优化目标：统一序列键格式，避免混合使用动态规则和直接字符串
        /// 采用固定前缀+业务标识+时间标识的格式
        /// </summary>
        /// <param name="keyPattern">键模式</param>
        /// <param name="resetType">重置类型</param>
        /// <returns>标准化的序列键</returns>
        private string GenerateStandardSequenceKey(string keyPattern, string resetType)
        {
            StringBuilder sequenceKeyBuilder = new StringBuilder();
            
            // 固定前缀，便于在数据库中识别和管理
            sequenceKeyBuilder.Append("SEQ_");
            
            // 处理嵌套表达式或直接键值
            if (keyPattern.Contains('{') && keyPattern.Contains('}'))
            {
                // 使用 RuleAnalysis 处理嵌套表达式
                string[] items = RuleAnalysis.Execute(keyPattern);
                
                if (items != null && items.Length > 0)
                {
                    // 创建一个临时字符串构建器来处理嵌套表达式结果
                    StringBuilder tempBuilder = new StringBuilder();
                    
                    foreach (string item in items)
                    {
                        string[] sps = RuleAnalysis.GetProperties(item);
                        if (sps != null && sps.Length >= 2)
                        {
                            IParameterHandler handler = null;
                            if (((BNRFactory)Factory).Handlers.TryGetValue(sps[0], out handler))
                            {
                                handler.Execute(tempBuilder, sps[1]);
                            }
                        }
                    }
                    
                    // 清理生成的临时键，移除特殊字符，确保键的有效性
                    string tempKey = tempBuilder.ToString();
                    if (!string.IsNullOrEmpty(tempKey))
                    {
                        // 移除可能导致键不规范的特殊字符
                        tempKey = System.Text.RegularExpressions.Regex.Replace(tempKey, "[^a-zA-Z0-9_一-龥]", "_");
                        sequenceKeyBuilder.Append(tempKey);
                    }
                    else
                    {
                        // 如果处理结果为空，使用默认业务标识
                        sequenceKeyBuilder.Append("DEFAULT");
                    }
                }
                else
                {
                    sequenceKeyBuilder.Append("DEFAULT");
                }
            }
            else
            {
                // 如果没有嵌套表达式，直接使用清理后的键值
                string cleanKey = System.Text.RegularExpressions.Regex.Replace(keyPattern, "[^a-zA-Z0-9_一-龥]", "_");
                sequenceKeyBuilder.Append(cleanKey);
            }
            
            // 根据重置类型添加时间标识
            // 注意：这里只添加标识而不添加具体日期，具体的日期部分由DatabaseSequenceService中的GenerateDynamicKey处理
            // 这样可以保持键的简洁性，同时不影响按时间重置的功能
            if (!string.IsNullOrEmpty(resetType) && resetType.ToUpper() != "NONE")
            {
                sequenceKeyBuilder.Append("_");
                sequenceKeyBuilder.Append(resetType.ToUpper());
            }
            
            // 获取最终的序列键
            string finalKey = sequenceKeyBuilder.ToString();
            
            // 确保生成的序列键不为空且长度合理
            if (string.IsNullOrEmpty(finalKey) || finalKey.Length > 200) // 避免键过长
            {
                // 使用默认键格式
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                finalKey = $"SEQ_DEFAULT_{timestamp}";
                System.Diagnostics.Debug.WriteLine($"警告: 生成的序列键为空或过长，使用默认键: {finalKey}");
            }
            
            return finalKey;
        }
    }
}