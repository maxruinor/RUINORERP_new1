using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.IServices;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Business.Services
{
    /// <summary>
    /// 本地业务编码生成服务实现
    /// 作为服务器通信失败时的备用方案，提供基本的编号生成功能
    /// </summary>
    public class LocalBizCodeGenerateService : IBizCodeService
    {
        private readonly object _lockObject = new object();
        private Dictionary<string, int> _sequenceCache = new Dictionary<string, int>();

        /// <summary>
        /// 生成业务单据编号（支持BizType枚举）
        /// 本地简单实现，仅用于备用
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的单据编号</returns>
        public Task<string> GenerateBizBillNoAsync(BizType bizType, CancellationToken ct = default)
        {
            return Task.FromResult(GenerateBizBillNo(bizType, null));
        }

        /// <summary>
        /// 生成业务单据编号（支持BizType枚举和额外参数）
        /// 本地简单实现，仅用于备用
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <param name="parameter">业务编码参数</param>
        /// <returns>生成的单据编号</returns>
        public string GenerateBizBillNo(BizType bizType, BizCodeParameter parameter = null)
        {
            string prefix = bizType.ToString();
            return $"{prefix}{DateTime.Now:yyyyMMdd}{GenerateLocalSequence($"BIZ_{bizType}", 6)}";
        }

        /// <summary>
        /// 生成基础信息编号（枚举版本）
        /// 本地简单实现，仅用于备用
        /// </summary>
        /// <param name="baseInfoType">基础信息类型枚举</param>
        /// <param name="paraConst">参数常量（可选）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的基础信息编号</returns>
        public Task<string> GenerateBaseInfoNoAsync(BaseInfoType baseInfoType, string paraConst = null, CancellationToken ct = default)
        {
            return GenerateBaseInfoNoAsync(baseInfoType.ToString(), paraConst, ct);
        }

        /// <summary>
        /// 生成基础信息编号（字符串版本）
        /// 本地简单实现，仅用于备用
        /// </summary>
        /// <param name="baseInfoType">基础信息类型</param>
        /// <param name="paraConst">参数常量（可选）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的基础信息编号</returns>
        public Task<string> GenerateBaseInfoNoAsync(string baseInfoType, string paraConst = null, CancellationToken ct = default)
        {
            string prefix = GetInfoTypePrefix(baseInfoType);
            if (!string.IsNullOrEmpty(paraConst))
            {
                return Task.FromResult($"{prefix}{paraConst}{GenerateLocalSequence($"INFO_{baseInfoType}_{paraConst}", 4)}");
            }
            return Task.FromResult($"{prefix}{DateTime.Now:yyyyMMdd}{GenerateLocalSequence($"INFO_{baseInfoType}", 4)}");
        }

        /// <summary>
        /// 生成产品编码
        /// 本地简单实现，仅用于备用
        /// </summary>
        /// <param name="categoryId">产品类别ID</param>
        /// <param name="customPrefix">自定义前缀（可选）</param>
        /// <param name="includeDate">是否包含日期</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的产品编码</returns>
        public Task<string> GenerateProductNoAsync(long categoryId, string customPrefix = null, bool includeDate = false, CancellationToken ct = default)
        {
            string prefix = string.IsNullOrEmpty(customPrefix) ? "PROD" : customPrefix.ToUpper().Substring(0, 3);
            string datePart = includeDate ? DateTime.Now.ToString("yyyyMM") : string.Empty;
            return Task.FromResult($"{prefix}{datePart}{GenerateLocalSequence($"PROD_{categoryId}_{customPrefix}", 4)}");
        }

        /// <summary>
        /// 生成产品SKU编码
        /// 本地简单实现，仅用于备用
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="attributes">属性组合信息</param>
        /// <param name="seqLength">序号长度</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的产品SKU编码</returns>
        public Task<string> GenerateProductSKUNoAsync(long productId, string productCode, string attributes = null, int seqLength = 3, CancellationToken ct = default)
        {
            string baseCode = string.IsNullOrEmpty(productCode) ? "SKU" : productCode;
            if (!string.IsNullOrEmpty(attributes))
            {
                // 简单处理属性，实际可能需要更复杂的编码规则
                string attrCode = string.Join("", attributes.Split(new[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => a.Trim().Substring(0, 1).ToUpper()));
                return Task.FromResult($"{baseCode}-{attrCode}");
            }
            return Task.FromResult($"{baseCode}-{GenerateLocalSequence($"SKU_{productId}", seqLength)}");
        }

        /// <summary>
        /// 根据规则生成编号
        /// 本地简单实现，仅支持基本规则
        /// </summary>
        /// <param name="rule">编码规则</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>生成的编号</returns>
        public string GenerateByRule(string rule, Dictionary<string, object> parameters = null)
        {
            string result = rule;

            // 替换日期变量
            result = result.Replace("{DATE}", DateTime.Now.ToString("yyyyMMdd"));
            result = result.Replace("{YEAR}", DateTime.Now.Year.ToString());
            result = result.Replace("{MONTH}", DateTime.Now.Month.ToString("00"));
            result = result.Replace("{DAY}", DateTime.Now.Day.ToString("00"));

            // 替换参数
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    result = result.Replace($"{{{param.Key}}}", param.Value?.ToString() ?? string.Empty);
                }
            }

            // 添加序列号
            result += GenerateLocalSequence($"RULE_{rule}", 4);

            return result;
        }

        /// <summary>
        /// 生成条码
        /// 简单实现，仅用于备用
        /// </summary>
        /// <param name="originalCode">原始编码</param>
        /// <param name="paddingChar">补位字符</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的条码</returns>
        public Task<string> GenerateBarCodeAsync(string originalCode, char paddingChar = '0', CancellationToken ct = default)
        {
            // 简单实现，实际可能需要更复杂的条码生成逻辑
            // 这里只是在原始编码基础上添加校验位或格式化
            if (string.IsNullOrEmpty(originalCode))
            {
                throw new ArgumentNullException(nameof(originalCode), "原始编码不能为空");
            }

            // 简单的校验逻辑示例
            int checkSum = originalCode.Sum(c => (int)c % 10);
            return Task.FromResult($"{originalCode.PadRight(12, paddingChar)}{checkSum % 10}");
        }

        /// <summary>
        /// 生成本地序列号
        /// 线程安全的简单序列号生成
        /// </summary>
        /// <param name="key">序列号键</param>
        /// <param name="digits">位数</param>
        /// <returns>格式化的序列号</returns>
        private string GenerateLocalSequence(string key, int digits)
        {
            lock (_lockObject)
            {
                if (!_sequenceCache.TryGetValue(key, out int sequence))
                {
                    sequence = 0;
                }

                sequence++;
                _sequenceCache[key] = sequence;

                return sequence.ToString().PadLeft(digits, '0');
            }
        }


        /// <summary>
        /// 获取信息类型前缀
        /// </summary>
        /// <param name="infoType">信息类型</param>
        /// <returns>前缀字符串</returns>
        private string GetInfoTypePrefix(string infoType)
        {
            // 简化实现，实际应该根据业务需求定义
            if (string.IsNullOrEmpty(infoType))
            {
                return "INFO";
            }

            // 取信息类型名称的前几个字符作为前缀
            return infoType.ToUpper().Substring(0, Math.Min(4, infoType.Length));
        }
    }
}