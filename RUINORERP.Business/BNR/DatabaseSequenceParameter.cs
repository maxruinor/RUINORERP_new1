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
        /// ✅ 修复:确保序列键稳定性,避免嵌套表达式导致的键不一致
        /// 采用固定前缀+业务标识+重置类型的格式
        /// </summary>
        /// <param name="keyPattern">键模式</param>
        /// <param name="resetType">重置类型</param>
        /// <returns>标准化的序列键</returns>
        private string GenerateStandardSequenceKey(string keyPattern, string resetType)
        {
            StringBuilder sequenceKeyBuilder = new StringBuilder();
                    
            // 固定前缀,便于在数据库中识别和管理
            sequenceKeyBuilder.Append("SEQ_");
                    
            // ✅ 关键修复:对于嵌套表达式,只提取静态部分作为序列键
            // 不要在序列键中包含动态日期,日期由DatabaseSequenceService统一管理
            if (keyPattern.Contains('{') && keyPattern.Contains('}'))
            {
                // 使用 RuleAnalysis 处理嵌套表达式,但只取第一个有意义的标识符
                string[] items = RuleAnalysis.Execute(keyPattern);
                        
                if (items != null && items.Length > 0)
                {
                    // 创建一个临时字符串构建器来处理嵌套表达式结果
                    StringBuilder tempBuilder = new StringBuilder();
                    bool hasAddedStaticPart = false;
                            
                    foreach (string item in items)
                    {
                        string[] sps = RuleAnalysis.GetProperties(item);
                        if (sps != null && sps.Length >= 2)
                        {
                            string handlerName = sps[0].Trim();
                                    
                            // ✅ 只处理常量处理器(S),跳过日期处理器(D/Hex等)
                            // 这样可以确保序列键只包含业务标识,不包含动态日期
                            if (handlerName.Equals("S", StringComparison.OrdinalIgnoreCase))
                            {
                                IParameterHandler handler = null;
                                if (((BNRFactory)Factory).Handlers.TryGetValue(handlerName, out handler))
                                {
                                    handler.Execute(tempBuilder, sps[1]);
                                    hasAddedStaticPart = true;
                                }
                            }
                            // 跳过日期相关的处理器,不在序列键中包含日期
                        }
                    }
                            
                    // 清理生成的临时键,移除特殊字符,确保键的有效性
                    string tempKey = tempBuilder.ToString();
                    if (!string.IsNullOrEmpty(tempKey))
                    {
                        // 移除可能导致键不规范的特殊字符
                        tempKey = System.Text.RegularExpressions.Regex.Replace(tempKey, "[^a-zA-Z0-9_一-龥]", "_");
                        sequenceKeyBuilder.Append(tempKey);
                    }
                    else if (!hasAddedStaticPart)
                    {
                        // 如果没有找到静态部分,使用原始键模式的哈希值作为唯一标识
                        string hashKey = GetStableHash(keyPattern);
                        sequenceKeyBuilder.Append(hashKey);
                        System.Diagnostics.Debug.WriteLine($"警告: 无法从 '{keyPattern}' 提取静态标识,使用哈希: {hashKey}");
                    }
                }
                else
                {
                    sequenceKeyBuilder.Append("DEFAULT");
                }
            }
            else
            {
                // 如果没有嵌套表达式,直接使用清理后的键值
                string cleanKey = System.Text.RegularExpressions.Regex.Replace(keyPattern, "[^a-zA-Z0-9_一-龥]", "_");
                sequenceKeyBuilder.Append(cleanKey);
            }
                    
            // 根据重置类型添加时间标识
            // ✅ 注意:这里只添加重置类型标识,具体的日期由DatabaseSequenceService.GenerateDynamicKey统一处理
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
                System.Diagnostics.Debug.WriteLine($"警告: 生成的序列键为空或过长,使用默认键: {finalKey}");
            }
                    
            // ✅ 记录最终生成的序列键,便于调试
            System.Diagnostics.Debug.WriteLine($"[序列键生成] 输入:'{keyPattern}', 重置类型:'{resetType}', 输出:'{finalKey}'");
                    
            return finalKey;
        }
                
        /// <summary>
        /// 生成稳定的哈希值,用于无法提取静态标识的情况
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>稳定的哈希字符串</returns>
        private string GetStableHash(string input)
        {
            unchecked
            {
                int hash = 17;
                foreach (char c in input)
                {
                    hash = hash * 31 + c;
                }
                // 转换为正数并限制长度
                return Math.Abs(hash).ToString("X8");
            }
        }
    }
}