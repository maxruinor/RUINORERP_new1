using RUINORERP.IServices;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business;
using System;
using System.Collections.Generic;
using System.Text;
using RUINORERP.Global;
using RUINORERP.Model;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.Server.Services.BizCode;
using System.Text.RegularExpressions;
using System.Numerics;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Network.CommandHandlers;
using RUINORERP.Business.BNR;
using RUINORERP.PacketSpec.Models.BizCodeGenerate;

namespace RUINORERP.Server.Services.BizCode
{

    /// <summary>
    /// 业务编号生成服务
    /// 负责各类业务单据和基础信息编号的生成、规则管理
    /// 根据规则类型（单据类规则或基础类规则）区分不同编号的生成逻辑
    /// 包装BNRFactory功能，提供业务编码生成服务
    /// </summary>
    public class ServerBizCodeGenerateService : IBizCodeGenerateService
    {
        private readonly BNRFactory _bnrFactory;
        private readonly tb_sys_BillNoRuleController<tb_sys_BillNoRule> _ruleConfigService;
        private readonly ILogger<ServerBizCodeGenerateService> logger;
        private readonly ProductSKUCodeGenerator _productSKUCodeGenerator;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="bnrFactory">编号生成工厂实例</param>
        /// <param name="ruleConfigService">规则配置服务</param>
        /// <param name="productSKUCodeGenerator">产品SKU编码生成器</param>
        public ServerBizCodeGenerateService(ILogger<ServerBizCodeGenerateService> logger,
            BNRFactory bnrFactory,
            tb_sys_BillNoRuleController<tb_sys_BillNoRule> ruleConfigService,
            ProductSKUCodeGenerator productSKUCodeGenerator
            )
        {
            this.logger = logger;
            _bnrFactory = bnrFactory;
            _ruleConfigService = ruleConfigService;
            _productSKUCodeGenerator = productSKUCodeGenerator;
        }

        #region 混淆/加密相关方法

        /// <summary>
        /// 根据加密方法对编号进行混淆处理
        /// 支持三种可逆加密方案：16进制转换、字符串混淆和Base62编码
        /// </summary>
        /// <param name="originalNumber">原始编号</param>
        /// <param name="encryptionMethod">加密方法枚举值</param>
        /// <returns>混淆后的编号</returns>
        /// <remarks>
        /// 该方法实现了编号的可逆转换，确保系统内部可以通过DeobfuscateNumber方法还原原始编号
        /// 用于保护业务敏感信息，如订单数量、销售规模等数据不被直观识别
        /// </remarks>
        private string ObfuscateNumber(string originalNumber, int encryptionMethod)
        {
            if (string.IsNullOrEmpty(originalNumber))
                return originalNumber;

            switch ((EncryptionMethod)encryptionMethod)
            {
                case EncryptionMethod.十六进制转换:
                    return ConvertToHex(originalNumber);
                case EncryptionMethod.字符串混淆:
                    return ApplyStringObfuscation(originalNumber);
                case EncryptionMethod.Base62编码:
                    return ConvertToBase62(originalNumber);
                default:
                    return originalNumber;
            }
        }

        /// <summary>
        /// 根据加密方法对混淆后的编号进行解密处理
        /// 是ObfuscateNumber方法的反向操作，确保编号的可逆性
        /// </summary>
        /// <param name="obfuscatedNumber">混淆后的编号</param>
        /// <param name="encryptionMethod">加密方法枚举值</param>
        /// <returns>解密后的原始编号</returns>
        /// <remarks>
        /// 系统内部使用此方法解析外部显示的混淆编号
        /// 确保即使在显示模式为混淆/加密的情况下，系统仍能处理和识别原始业务数据
        /// </remarks>
        private string DeobfuscateNumber(string obfuscatedNumber, int encryptionMethod)
        {
            if (string.IsNullOrEmpty(obfuscatedNumber))
                return obfuscatedNumber;

            switch ((EncryptionMethod)encryptionMethod)
            {
                case EncryptionMethod.十六进制转换:
                    return ConvertFromHex(obfuscatedNumber);
                case EncryptionMethod.字符串混淆:
                    return ReverseStringObfuscation(obfuscatedNumber);
                case EncryptionMethod.Base62编码:
                    return ConvertFromBase62(obfuscatedNumber);
                default:
                    return obfuscatedNumber;
            }
        }

        /// <summary>
        /// 16进制转换 - 将编号中的数字部分转换为16进制
        /// 基础混淆方案，通过进制转换改变数字的表示形式
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>转换后的字符串</returns>
        /// <remarks>
        /// 工作原理：提取字符串中的数字部分，将其转换为大写16进制表示
        /// 例如：ORDER-20230101-001 转换为 ORDER-20230101-65
        /// 可逆性：通过ConvertFromHex方法可以将16进制转换回十进制
        /// </remarks>
        private string ConvertToHex(string input)
        {
            // 提取数字部分并转换为16进制
            var numericPart = Regex.Match(input, @"\d+").Value;
            if (!string.IsNullOrEmpty(numericPart) && long.TryParse(numericPart, out long number))
            {
                string hex = number.ToString("X");
                return input.Replace(numericPart, hex);
            }
            return input;
        }

        /// <summary>
        /// 从16进制转换回原始数字
        /// 是ConvertToHex方法的反向操作
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>转换后的原始字符串</returns>
        /// <remarks>
        /// 工作原理：识别字符串中的16进制部分，将其转换回十进制表示
        /// 使用正则表达式匹配可能的16进制字符序列，通过NumberStyles.HexNumber进行解析
        /// </remarks>
        private string ConvertFromHex(string input)
        {
            // 查找可能是16进制的部分
            var matches = Regex.Matches(input, @"[A-F0-9]+");
            foreach (Match match in matches)
            {
                if (long.TryParse(match.Value, System.Globalization.NumberStyles.HexNumber, null, out long number))
                {
                    input = input.Replace(match.Value, number.ToString());
                    break;
                }
            }
            return input;
        }

        /// <summary>
        /// 字符串混淆 - 使用字符替换和数字反转的方式混淆字符串
        /// 进阶混淆方案，通过多种转换组合增强混淆效果
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>混淆后的字符串</returns>
        /// <remarks>
        /// 工作原理：
        /// 1. 字符替换：使用反向字母表替换字母字符（A<->Z, B<->Y...）
        /// 2. 数字反转：将字符串中的数字部分反转
        /// 例如：ORDER-20230101-001 转换为 LIVNK-01013202-100
        /// 可逆性：通过ReverseStringObfuscation方法按照相反顺序执行操作即可还原
        /// </remarks>
        private string ApplyStringObfuscation(string input)
        {
            // 1. 字符替换：A<->Z, B<->Y, ..., 数字部分反转
            char[] chars = input.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (char.IsLetter(chars[i]))
                {
                    if (char.IsUpper(chars[i]))
                    {
                        chars[i] = (char)('Z' - (chars[i] - 'A'));
                    }
                    else
                    {
                        chars[i] = (char)('z' - (chars[i] - 'a'));
                    }
                }
            }

            // 2. 反转数字部分
            string result = new string(chars);
            var numericMatches = Regex.Matches(result, @"\d+");
            foreach (Match match in numericMatches)
            {
                char[] numChars = match.Value.ToCharArray();
                Array.Reverse(numChars);
                result = result.Replace(match.Value, new string(numChars));
            }

            return result;
        }

        /// <summary>
        /// 反转字符串混淆 - 将混淆后的字符串恢复为原始形式
        /// 是ApplyStringObfuscation方法的反向操作
        /// </summary>
        /// <param name="input">混淆后的字符串</param>
        /// <returns>原始字符串</returns>
        /// <remarks>
        /// 工作原理：按照与ApplyStringObfuscation相反的顺序执行操作
        /// 1. 先反转数字部分
        /// 2. 再进行反向字符替换
        /// 确保能够准确还原原始字符串
        /// </remarks>
        private string ReverseStringObfuscation(string input)
        {
            // 1. 先反转数字部分
            string result = input;
            var numericMatches = Regex.Matches(result, @"\d+");
            foreach (Match match in numericMatches)
            {
                char[] numChars = match.Value.ToCharArray();
                Array.Reverse(numChars);
                result = result.Replace(match.Value, new string(numChars));
            }

            // 2. 反向字符替换
            char[] chars = result.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (char.IsLetter(chars[i]))
                {
                    if (char.IsUpper(chars[i]))
                    {
                        chars[i] = (char)('Z' - (chars[i] - 'A'));
                    }
                    else
                    {
                        chars[i] = (char)('z' - (chars[i] - 'a'));
                    }
                }
            }

            return new string(chars);
        }

        // Base62编码使用的字符集 - 62个可打印ASCII字符，提高混淆强度
        private static readonly char[] Base62Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        /// <summary>
        /// Base62编码 - 将数字部分转换为Base62编码
        /// 高级混淆方案，使用62进制表示法，提供更高的混淆强度
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>转换后的字符串</returns>
        /// <remarks>
        /// 工作原理：
        /// 1. 提取字符串中的数字部分
        /// 2. 将数字转换为基于62个字符集的表示形式
        /// 3. 使用Base62可以在相同位数下表示更大的数字范围，或者使用更少的字符表示相同范围的数字
        /// 例如：ORDER-20230101-1000 转换为 ORDER-20230101-G8
        /// 可逆性：通过ConvertFromBase62方法可以准确还原原始数字
        /// </remarks>
        private string ConvertToBase62(string input)
        {
            // 提取数字部分并转换为Base62
            var numericPart = Regex.Match(input, @"\d+").Value;
            if (!string.IsNullOrEmpty(numericPart) && long.TryParse(numericPart, out long number))
            {
                StringBuilder sb = new StringBuilder();
                do
                {
                    sb.Insert(0, Base62Chars[number % 62]);
                    number /= 62;
                } while (number > 0);
                return input.Replace(numericPart, sb.ToString());
            }
            return input;
        }

        /// <summary>
        /// 从Base62编码转换回原始数字
        /// 是ConvertToBase62方法的反向操作
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>转换后的原始字符串</returns>
        /// <remarks>
        /// 工作原理：
        /// 1. 使用正则表达式识别可能的Base62编码部分
        /// 2. 遍历字符，查找每个字符在Base62字符集中的索引
        /// 3. 通过索引计算还原原始数字值
        /// 4. 使用异常捕获确保即使遇到非Base62编码部分也能正常处理
        /// </remarks>
        private string ConvertFromBase62(string input)
        {
            // 查找可能是Base62编码的部分（字母+数字组合）
            var matches = Regex.Matches(input, @"[A-Za-z0-9]+");
            foreach (Match match in matches)
            {
                try
                {
                    long number = 0;
                    foreach (char c in match.Value)
                    {
                        int index = Array.IndexOf(Base62Chars, c);
                        if (index < 0)
                            throw new ArgumentException();
                        number = number * 62 + index;
                    }
                    input = input.Replace(match.Value, number.ToString());
                    break;
                }
                catch { }
            }
            return input;
        }



        #endregion

        /// <summary>
        /// 生成业务单据编号（支持BizType枚举）
        /// 支持显示模式配置：可选择清晰可读或混淆/加密模式
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的单据编号</returns>
        /// <remarks>
        /// 功能特性：
        /// 1. 从数据库获取完整的规则配置，包括显示模式和加密方法
        /// 2. 默认配置：如果数据库中没有配置，则使用清晰可读模式和16进制转换
        /// 3. 支持根据配置的显示模式对生成的编号进行处理
        /// 4. 错误处理：当生成过程发生异常时，使用默认规则确保系统可用性
        /// </remarks>
        public async Task<string> GenerateBizBillNoAsync(BizType bizType, CancellationToken ct = default)
        {
            try
            {
                // 获取完整的规则配置对象，包含显示模式和加密方法
                var ruleConfig = await GetBillNoRuleConfigFromDatabaseAsync((int)bizType, (int)RuleType.业务单据编号, ct);

                string rule;
                int displayMode = 0; // 默认清晰可读模式
                int encryptionMethod = 0; // 默认16进制转换

                if (ruleConfig != null && !string.IsNullOrEmpty(ruleConfig.RulePattern))
                {
                    rule = ruleConfig.RulePattern;
                    encryptionMethod = ruleConfig.EncryptionMethod;
                }
                else
                {
                    // 如果数据库中没有配置，则使用默认规则
                    rule = GetDefaultBillNoRule(bizType);
                }

                // 生成原始编号
                string generatedNumber = _bnrFactory.Create(rule);

                // 根据显示模式处理编号
                //  string finalNumber = ProcessNumberByDisplayMode(generatedNumber, displayMode, encryptionMethod);

                return generatedNumber;
            }
            catch (Exception ex)
            {
                // 记录错误日志
                logger?.LogError(ex, "生成业务单据编号失败: {BizType}", bizType);
                // 出错时使用默认规则，确保系统可用性
                string fallbackRule = GetDefaultBillNoRule(bizType);
                string fallbackNumber = _bnrFactory.Create(fallbackRule);

                return fallbackNumber;
            }
        }


        /// <summary>
        /// 从数据库获取业务单据编号规则
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>编码规则</returns>
        private async Task<string> GetBillNoRuleFromDatabaseAsync(BizType bizType, CancellationToken ct = default)
        {
            try
            {
                // 首先尝试从缓存获取规则（如果有缓存机制）
                // 这里预留了缓存接口，实际项目中可以根据需要实现
                // string cachedRule = GetRuleFromCache(bizType, ruleType);
                // if (!string.IsNullOrEmpty(cachedRule)) return cachedRule;
                int ruleType = (int)RuleType.业务单据编号;
                // 查询数据库中的规则配置，增加按规则类型过滤
                var rules = await _ruleConfigService.QueryAsync();
                var rule = rules?.FirstOrDefault(r => r.BizType == (int)bizType && r.RuleType == ruleType && r.IsActive);

                // 缓存规则（如果有缓存机制）
                // if (rule?.RulePattern != null) CacheRule(bizType, ruleType, rule.RulePattern);

                return rule?.RulePattern;
            }
            catch
            {
                // 如果查询失败，返回null，将使用默认规则
                return null;
            }
        }

        /// <summary>
        /// 获取完整的规则配置对象（包含显示模式和加密方法等信息）
        /// 用于获取编号生成所需的全部配置参数
        /// </summary>
        /// <param name="bizType">业务类型枚举值</param>
        /// <param name="ruleType">规则类型（单据类/基础类）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>规则配置对象</returns>
        /// <remarks>
        /// 重要说明：
        /// 1. 该方法是显示模式功能的核心支持方法
        /// 2. 返回tb_sys_BillNoRule对象，包含RulePattern（规则模式）、DisplayMode（显示模式）和EncryptionMethod（加密方法）
        /// 3. 只有IsActive为true的规则配置会被返回
        /// 4. 异常处理确保即使数据库查询失败也不会影响系统运行
        /// </remarks>
        private async Task<tb_sys_BillNoRule> GetBillNoRuleConfigFromDatabaseAsync(int bizType, int ruleType, CancellationToken ct = default)
        {
            try
            {
                // 查询数据库中的规则配置
                var rules = await _ruleConfigService.QueryAsync();
                return rules?.FirstOrDefault(r => r.BizType == bizType && r.RuleType == ruleType && r.IsActive);
            }
            catch
            {
                // 如果查询失败，返回null，将使用默认规则
                return null;
            }
        }

        /// <summary>
        /// 获取默认的业务单据编号规则
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <returns>编码规则</returns>
        private string GetDefaultBillNoRule(BizType bizType)
        {
            // 根据业务类型枚举返回对应的默认规则
            switch (bizType)
            {
                case BizType.销售订单:
                    return "{S:SO:upper}{D:yyMMdd}{DB:{S:销售订单}{D:yyMMdd}/000/daily}";
                case BizType.销售出库单:
                    return "{S:SOD:upper}{Hex:yyMM}{DB:{S:销售出库单}{D:yyMM}/0000}";
                case BizType.销售退回单:
                    return "{S:SODR:upper}{D:yyMMdd}{DB:{S:销售退回单}{D:yyMM}/000}";
                case BizType.采购订单:
                    return "{S:PO:upper}{Hex:yyMMdd}{DB:{S:采购订单}{D:yyMM}/000}";
                case BizType.采购入库单:
                    return "{S:PIR:upper}{D:yyMMdd}{DB:{S:采购入库单}{D:yyMM}/000}";
                case BizType.采购退货单:
                    return "{S:PIRR:upper}{D:yyMMdd}{DB:{S:采购退货单}{D:yyMM}/000}";
                case BizType.其他入库单:
                    return "{S:OIR:upper}{D:yyMMdd}{DB:{S:其他入库单}{D:yyMM}/000}";
                case BizType.其他出库单:
                    return "{S:OQD:upper}{D:yyMMdd}{DB:{S:其他出库单}{D:yyMM}/000}";
                case BizType.盘点单:
                    return "{S:CS:upper}{D:yyMMdd}{DB:{S:盘点单}{D:yyMM}/000}";
                case BizType.BOM物料清单:
                    return "{S:BS:upper}{D:yyMMdd}{DB:{S:BOM物料清单}{D:yyMM}/000}";
                case BizType.其他费用支出:
                    return "{S:EXP:upper}{D:yyMMdd}{DB:{S:其他费用支出}{D:yyMM}/000}";
                case BizType.其他费用收入:
                    return "{S:INC:upper}{D:yyMMdd}{DB:{S:其他费用收入}{D:yyMM}/000}";
                case BizType.费用报销单:
                    return "{S:EC:upper}{D:yyMMdd}{DB:{S:费用报销单}{D:yyMM}/000}";
                case BizType.生产计划单:
                    return "{S:PP:upper}{D:yyMMdd}{DB:{S:生产计划单}{D:yyMM}/00}";
                case BizType.需求分析:
                    return "{S:PD:upper}{D:yyMMdd}{DB:{S:生产需求分析}{D:yyMM}/00}";
                case BizType.制令单:
                    return "{S:MO:upper}{D:yyMMdd}{DB:{S:制令单}{D:yyMM}/00}";
                case BizType.请购单:
                    return "{S:RP:upper}{D:yyMMdd}{DB:{S:请购单}{D:yyMM}/000}";
                case BizType.生产领料单:
                    return "{S:PRD:upper}{D:yyMMdd}{DB:{S:生产领料单}{D:yyMM}/000}";
                case BizType.生产退料单:
                    return "{S:PRR:upper}{D:yyMMdd}{DB:{S:生产退料单}{D:yyMM}/000}";
                case BizType.缴库单:
                    return "{S:PR:upper}{D:yyMMdd}{DB:{S:缴库单}{D:yyMM}/000}";
                case BizType.产品分割单:
                    return "{S:PS:upper}{D:yyMMdd}{DB:{S:产品分割单}{D:yyMM}/00}";
                case BizType.产品组合单:
                    return "{S:PM:upper}{D:yyMMdd}{DB:{S:产品组合单}{D:yyMM}/00}";
                case BizType.借出单:
                    return "{S:JC:upper}{D:yyMMdd}{DB:{S:借出单}{D:yyMM}/000}";
                case BizType.归还单:
                    return "{S:GH:upper}{D:yyMMdd}{DB:{S:归还单}{D:yyMM}/000}";
                case BizType.产品转换单:
                    return "{S:ZH:upper}{D:yyMMdd}{DB:{S:产品转换单}{D:yyMM}/000}";
                case BizType.调拨单:
                    return "{S:DB:upper}{D:yyMMdd}{DB:{S:调拨单}{D:yyMM}/000}";
                case BizType.返工退库单:
                    return "{S:RW:upper}{D:yyMMdd}{DB:{S:返工退库单}{D:yyMM}/00}";
                case BizType.返工入库单:
                    return "{S:RE:upper}{D:yyMMdd}{DB:{S:返工入库单}{D:yyMM}/00}";
                case BizType.付款申请单:
                    return "{S:PA:upper}{D:yyMMdd}{DB:{S:付款申请单}{D:yyMM}/00}";
                case BizType.销售合同:
                    return "{S:SC-:upper}{D:yyMMdd}{DB:{S:销售合同}{D:yyMM}/00}";
                case BizType.预付款单:
                    return "{S:YF:upper}{D:yyMMdd}{DB:{S:预付款单}{D:yyMM}/000}";
                case BizType.预收款单:
                    return "{S:YS:upper}{D:yyMMdd}{DB:{S:预收款单}{D:yyMM}/000}";
                case BizType.付款单:
                    return "{S:FK:upper}{D:yyMMdd}{DB:{S:付款单}{D:yyMM}/000}";
                case BizType.收款单:
                    return "{S:SK:upper}{D:yyMMdd}{DB:{S:收款单}{D:yyMM}/000}";
                case BizType.应付款单:
                    return "{S:AP:upper}{D:yyMMdd}{DB:{S:应付款单}{D:yyMM}/000}";
                case BizType.应收款单:
                    return "{S:AR:upper}{D:yyMMdd}{DB:{S:应收款单}{D:yyMM}/000}";
                case BizType.对账单:
                    return "{S:PS:upper}{D:yyMMdd}{DB:{S:对账单}{D:yyMM}/000}";
                case BizType.损失确认单:
                    return "{S:LO:upper}{D:yyMMdd}{DB:{S:损失确认单}{D:yyMM}/00}";
                case BizType.溢余确认单:
                    return "{S:OY:upper}{D:yyMMdd}{DB:{S:溢余确认单}{D:yyMM}/00}";
                case BizType.付款核销:
                    return "{S:FKHX:upper}{D:yyMMdd}{DB:{S:付款核销}{D:yyMM}/0000}";
                case BizType.收款核销:
                    return "{S:SKHX:upper}{D:yyMMdd}{DB:{S:收款核销}{D:yyMM}/0000}";
                case BizType.销售价格调整单:
                    return "{S:SPA:upper}{D:yyMMdd}{DB:{S:销售价格调整单}{D:yyMM}/000}";
                case BizType.采购价格调整单:
                    return "{S:PPA:upper}{D:yyMMdd}{DB:{S:采购价格调整单}{D:yyMM}/000}";
                case BizType.采购退货入库:
                    return "{S:PIRRE:upper}{D:yyMMdd}{DB:{S:采购退货入库}{D:yyMM}/000}";
                case BizType.售后申请单:
                    return "{S:ASAP:upper}{D:yyMMdd}{DB:{S:售后申请单}{D:yyMM}/000}";
                case BizType.售后交付单:
                    return "{S:ASAPD:upper}{D:yyMMdd}{DB:{S:售后交付单}{D:yyMM}/000}";
                case BizType.维修工单:
                    return "{S:ASRO:upper}{D:yyMMdd}{DB:{S:维修工单}{D:yyMM}/000}";
                case BizType.维修入库单:
                    return "{S:ASRIS:upper}{D:yyMMdd}{DB:{S:维修入库单}{D:yyMM}/000}";
                case BizType.维修领料单:
                    return "{S:ASRMR:upper}{D:yyMMdd}{DB:{S:维修领料单}{D:yyMM}/000}";
                case BizType.报废单:
                    return "{S:ASSD:upper}{D:yyMMdd}{DB:{S:报废单}{D:yyMM}/000}";
                case BizType.售后借出单:
                    return "{S:ASBR:upper}{D:yyMMdd}{DB:{S:售后借出单}{D:yyMM}/000}";
                case BizType.售后归还单:
                    return "{S:ASRE:upper}{D:yyMMdd}{DB:{S:售后归还单}{D:yyMM}/000}";
                default:
                    // 默认规则 - 为前缀添加大写转换
                    return "{S:{bizType}:upper}{D:yyMMdd}{DB:{bizType}{D:yyMM}/000}";
            }
        }

        /// <summary>
        /// 生成基础信息编号
        /// 支持显示模式配置：可选择清晰可读或混淆/加密模式
        /// 支持基于类目的独立序列
        /// </summary>
        /// <param name="infoType">信息类型枚举</param>
        /// <param name="paraConst">常量参数</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的信息编号</returns>
        /// <remarks>
        /// 功能特性：
        /// 1. 从数据库获取完整的规则配置，包括显示模式和加密方法
        /// 2. 默认配置：如果数据库中没有配置，则使用清晰可读模式和16进制转换
        /// 3. 支持根据配置的显示模式对生成的编号进行处理
        /// 4. 自定义常量：可通过paraConst参数替换规则中的{S:Const}占位符
        /// 5. 支持基于类目的独立序列（对产品类编号特别有效）
        /// 6. 错误处理：当生成过程发生异常时，使用默认规则确保系统可用性
        /// </remarks>
        public async Task<string> GenerateBaseInfoNoAsync(BaseInfoType infoType, string paraConst = null, CancellationToken ct = default)
        {

            string rule;
            int encryptionMethod = 0;

            try
            {
                // 获取完整的规则配置对象
                var ruleConfig = await GetBillNoRuleConfigFromDatabaseAsync((int)infoType, (int)RuleType.基础信息编号, ct);

                if (ruleConfig != null && !string.IsNullOrEmpty(ruleConfig.RulePattern))
                {
                    rule = ruleConfig.RulePattern;
                    encryptionMethod = ruleConfig.EncryptionMethod;
                }
                else
                {
                    // 如果数据库中没有配置，则使用默认规则
                    rule = GetDefaultBaseInfoNoRule(infoType, paraConst);
                }

                // 如果有自定义常量，则替换规则中的相关参数
                if (!string.IsNullOrEmpty(paraConst))
                {
                    rule = rule.Replace("{S:Const}", $"{{S:{paraConst}}}");
                }

                // 生成原始编号1
                string generatedNumber = _bnrFactory.Create(rule);

                // 根据显示模式处理编号
                //  string finalNumber = ProcessNumberByDisplayMode(generatedNumber, encryptionMethod);

                return generatedNumber;
            }
            catch (Exception ex)
            {
                // 记录错误日志
                logger?.LogError(ex, "生成基础信息编号失败: {InfoType}", infoType);
                // 出错时使用默认规则，确保系统可用性
                rule = GetDefaultBaseInfoNoRule(infoType, paraConst);
                if (!string.IsNullOrEmpty(paraConst))
                {
                    rule = rule.Replace("{S:Const}", $"{{S:{paraConst}}}");
                }

                string fallbackNumber = _bnrFactory.Create(rule);

                return fallbackNumber;
            }
        }

        /// <summary>
        /// 从数据库获取基础信息编号规则
        /// </summary>
        /// <param name="infoType">信息类型枚举</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>编码规则</returns>
        private async Task<string> GetBaseInfoNoRuleFromDatabaseAsync(BaseInfoType infoType, CancellationToken ct = default)
        {
            try
            {
                // 复用新的方法获取规则配置对象，保持与现有逻辑的兼容性
                var ruleConfig = await GetBillNoRuleConfigFromDatabaseAsync((int)infoType, (int)RuleType.基础信息编号, ct);
                return ruleConfig?.RulePattern;
            }
            catch
            {
                // 如果查询失败，返回null，将使用默认规则
                return null;
            }
        }

        /// <summary>
        /// 获取默认的基础信息编号规则
        /// </summary>
        /// <param name="infoType">信息类型枚举</param>
        /// <param name="paraConst">常量参数</param>
        /// <returns>编码规则</returns>
        private string GetDefaultBaseInfoNoRule(BaseInfoType infoType, string paraConst = null)
        {
            // 根据信息类型枚举返回对应的默认规则
            switch (infoType)
            {
                case BaseInfoType.Employee: // 员工编号
                    return "{S:EMP}{DB:{S:Employee}/000}";
                case BaseInfoType.Supplier: // 供应商编号
                    return "{S:SU}{DB:{S:Supplier}/000}";
                case BaseInfoType.Customer: // 客户编号
                    return "{S:CU}{DB:{S:Customer}/000}";
                case BaseInfoType.Storehouse: // 仓库编号
                    return "{S:ST}{DB:{S:Storehouse}/000}";
                case BaseInfoType.ProductNo: // 产品编号
                    return "{S:P}{Hex:yyMM}{DB:{S:ProductNo}/000}";
                case BaseInfoType.Location: // 库位编号
                    return "{S:L}{DB:{S:LOC}/000}";
                case BaseInfoType.SKU_No: // SKU编号
                    return "{S:SK}{Hex:yyMM}{DB:SKU_No/0000}";
                case BaseInfoType.ModuleDefinition: // 模块定义
                    return "{S:MD}{DB:{S:ModuleDefinition}/000}";
                case BaseInfoType.Department: // 部门编号
                    return "{S:D}{DB:{S:Department}/000}";
                case BaseInfoType.CVOther: // CVOther编号
                    return "{S:CV}{DB:{S:CVOther}/000}";
                case BaseInfoType.StoreCode: // 门店编号
                    return "{S:SHOP}{DB:{S:StoreCode}/000}";
                case BaseInfoType.ProCategories: // 产品分类编号
                    return "{S:C}{DB:{S:ProCategories}/000}";
                case BaseInfoType.BusinessPartner: // 业务伙伴编号
                    return "{S:BP}{DB:{S:BusinessPartner}/0000}";
                case BaseInfoType.ShortCode: // 简码
                    if (!string.IsNullOrEmpty(paraConst))
                    {
                        return $"{{DB:S:{paraConst}/000}}";
                    }
                    return "{S:SC}{DB:{S:ShortCode}/000}";
                case BaseInfoType.FMSubject: // 会计科目
                    return "{DB:BST/000}";
                case BaseInfoType.CRM_RegionCode: // 地区编码
                    return "{DB:CRC/00}";
                case BaseInfoType.ProjectGroupCode: // 项目组编号
                    return "{S:PG}{DB:{S:ProjectGroupCode}/000}";
                default:
                    // 默认规则
                    return "{S:{infoType}}{DB:{infoType}/000}";
            }
        }




        /// <summary>
        /// 生成产品相关编码（产品编号、SKU编号、简码等）
        /// 支持SKU属性更新逻辑：
        /// - 编辑已有产品（ProdDetailID > 0）：SKU永远不变
        /// - 新增产品详情（ProdDetailID == 0）：
        ///   - 如果SKU已有值 → 使用 UpdateSKUAttributePart 更新（保持序号不变，只更新属性部分）
        ///   - 如果SKU为空 → 使用 GenerateProductRelatedCodeWithAttributesAsync 生成全新SKU
        /// </summary>
        /// <param name="baseInfoType">基础信息类型</param>
        /// <param name="prod">产品实体</param>
        /// <param name="attributeInfos">属性信息列表（可选，用于SKU编码生成）</param>
        /// <param name="PrefixParaConst">前缀参数常量</param>
        /// <param name="seqLength">序号长度</param>
        /// <param name="includeDate">是否包含日期</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的产品相关编码</returns>
        public async Task<string> GenerateProductRelatedCodeAsync(
            BaseInfoType baseInfoType, 
            tb_Prod prod,
            string PrefixParaConst = null, 
            int seqLength = 3, 
            bool includeDate = false, 
            CancellationToken ct = default)
        {
            // 调用带有attributeInfos参数的方法，传递null作为默认值
            return await GenerateProductRelatedCodeWithAttributesAsync(
                baseInfoType, 
                prod, 
                null, // 没有attributeInfos参数，传递null
                PrefixParaConst, 
                seqLength, 
                includeDate, 
                ct);
        }

        /// <summary>
        /// 生成产品相关编码，支持属性信息参数
        /// </summary>
        /// <param name="baseInfoType">基础信息类型</param>
        /// <param name="prod">产品实体</param>
        /// <param name="attributeInfos">产品属性信息列表</param>
        /// <param name="PrefixParaConst">前缀常量参数</param>
        /// <param name="seqLength">序号长度</param>
        /// <param name="includeDate">是否包含日期</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的产品相关编码</returns>
        public async Task<string> GenerateProductRelatedCodeWithAttributesAsync(
            BaseInfoType baseInfoType, 
            tb_Prod prod, 
            List<ProductAttributeInfo> attributeInfos = null,
            string PrefixParaConst = null, 
            int seqLength = 3, 
            bool includeDate = false, 
            CancellationToken ct = default)
        {

            switch (baseInfoType)
            {
                case BaseInfoType.ProductNo:
                    return  _productSKUCodeGenerator.GenerateProdNoAsync(prod);
                    
                case BaseInfoType.ShortCode:
                    return  _productSKUCodeGenerator.GenerateShortCodeAsync(prod);
                case BaseInfoType.SKU_No:
                    #region SKU
                    // SKU编码生成逻辑
                    // 编辑已有产品（ProdDetailID > 0）：SKU永远不变
                    // 注意：ProdDetailID 来自于 prod.tb_ProdDetails 集合中的详情实体
                    if (prod?.tb_ProdDetails != null && prod.tb_ProdDetails.Count > 0)
                    {
                        var detail = prod.tb_ProdDetails.FirstOrDefault();
                        if (detail != null && detail.ProdDetailID > 0)
                        {
                            // 编辑模式：如果产品详情已保存（ProdDetailID > 0），SKU不再改变
                            return ""; // 返回空字符串表示不生成新SKU
                        }
                    }

                    // 新增产品详情（ProdDetailID == 0）
                    // 检查是否需要更新SKU属性部分
                    string existingSku = GetExistingSkuFromProdDetail(prod);
                    
                    if (!string.IsNullOrEmpty(existingSku))
                    {
                        // 如果SKU已有值，则更新SKU属性部分（保持序号不变）
                        if ((attributeInfos != null && attributeInfos.Count > 0) || 
                            (prod.tb_Prod_Attr_Relations != null && prod.tb_Prod_Attr_Relations.Count > 0))
                        {
                            // 使用属性信息或产品属性关系更新SKU
                            return _productSKUCodeGenerator.UpdateSKUAttributePart(existingSku, prod);
                        }
                        else
                        {
                            // 如果没有属性信息，返回原有SKU
                            return existingSku;
                        }
                    }
                    else
                    {
                        // SKU为空，生成全新的SKU
                        if ((attributeInfos != null && attributeInfos.Count > 0) || 
                            (prod.tb_Prod_Attr_Relations != null && prod.tb_Prod_Attr_Relations.Count > 0))
                        {
                            // 使用ProductSKUCodeGenerator生成基于属性的SKU编码
                            // 优先使用attributeInfos，如果为空则使用prod内部的属性关系
                            return _productSKUCodeGenerator.GenerateSKUCodeAsync(prod, attributeInfos);
                        }
                        else
                        {
                            // 如果没有属性信息，则使用默认的SKU编码生成方式
                            string rule;

                            // 优先从数据库获取规则配置
                            rule = await GetRuleFromDatabaseAsync(baseInfoType, ct);

                            // 如果数据库中没有配置，则使用默认规则
                            if (string.IsNullOrEmpty(rule))
                            {
                                rule = "{S:SK}{Hex:yyMM}{DB:SKU_No/0000}";
                            }

                            return _bnrFactory.Create(rule);
                        }
                    }
                    #endregion
                    break;
                default:
                    break;
            }


            return "";

        }

        /// <summary>
        /// 从产品实体中读取现有的SKU值
        /// 直接从产品详情集合中视四，无需在转辂对象中准备
        /// </summary>
        /// <param name="prod">产品实体（会自动获取tb_ProdDetails集合）</param>
        /// <returns>现有SKU值（如果不存在则返回空字符串）</returns>
        private string GetExistingSkuFromProdDetail(tb_Prod prod)
        {
            try
            {
                // 从产品的详情集合中查找ProdDetailID == 0（新增）的详情，并获取其SKU值
                if (prod?.tb_ProdDetails != null && prod.tb_ProdDetails.Count > 0)
                {
                    // 优先查找ProdDetailID == 0的详情（新增模式）
                    var newDetail = prod.tb_ProdDetails.FirstOrDefault(d => d.ProdDetailID == 0);
                    if (newDetail != null && !string.IsNullOrEmpty(newDetail.SKU))
                    {
                        return newDetail.SKU;
                    }

                    // 如果没有新增详情，查找首个有SKU值的详情
                    var anyDetail = prod.tb_ProdDetails.FirstOrDefault(d => !string.IsNullOrEmpty(d.SKU));
                    if (anyDetail != null)
                    {
                        return anyDetail.SKU;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "读取现有SKU值失败");
                return string.Empty;
            }
        }

        /// <summary>
        /// 从数据库获取产品SKU编码规则
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>编码规则</returns>
        private async Task<string> GetRuleFromDatabaseAsync(BaseInfoType infoType, CancellationToken ct = default)
        {
            try
            {
                // 首先尝试从缓存获取规则（如果有缓存机制）
                // string cachedRule = GetRuleFromCache(productId, "SKU");
                // if (!string.IsNullOrEmpty(cachedRule)) return cachedRule;

                // 查询数据库中的规则配置，指定规则类型为基础类规则
                var rules = await _ruleConfigService.QueryAsync();
                var rule = rules?.FirstOrDefault(r => r.BizType == (int)infoType && r.RuleType == (int)RuleType.基础信息编号 && r.IsActive);

                // 缓存规则（如果有缓存机制）
                // if (rule?.RulePattern != null) CacheRule(productId, "SKU", rule.RulePattern);

                return rule?.RulePattern;
            }
            catch
            {
                // 如果查询失败，返回null，将使用默认规则
                return null;
            }
        }

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="originalCode">原始编码</param>
        /// <param name="paddingChar">补位字符</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的条码</returns>
        public async Task<string> GenerateBarCodeAsync(string originalCode, char paddingChar = '0', CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(originalCode))
            {
                throw new ArgumentNullException(nameof(originalCode), "原始编码不能为空");
            }

            // 为了确保唯一性，我们生成一个基于原始编码但添加了时间戳和随机数的组合编码
            string uniqueBaseCode = GenerateUniqueBaseCode(originalCode);

            //条码校验
            string ENA_13str = "131313131313";
            //定义输出条码
            string barcode = "";
            //临时生成条码
            string tmpbarcode = uniqueBaseCode;

            //判断条码长度不足12位用补位码补足
            if (tmpbarcode.Length < 12)
            {
                tmpbarcode = tmpbarcode.PadLeft(12, paddingChar);
            }
            // 如果超过12位，只取后12位
            else if (tmpbarcode.Length > 12)
            {
                tmpbarcode = tmpbarcode.Substring(tmpbarcode.Length - 12);
            }

            //计算校验位
            string checkstr = "";
            int sum = 0, j = 0;
            for (int i = 0; i < 12; i++)
            {
                sum = sum + int.Parse(tmpbarcode[i].ToString())
                      * int.Parse(ENA_13str[i].ToString());
            }

            //取余数，如果余数大于0则校验位为10-J，否则为0
            j = sum % 10;
            if (j > 0) checkstr = (10 - j).ToString();
            else checkstr = "0";

            //获取最后条码
            barcode = tmpbarcode + checkstr;

            return barcode;
        }

        /// <summary>
        /// 生成唯一的基础编码
        /// 结合原始编码、时间戳和随机数，确保生成的编码具有高度唯一性
        /// </summary>
        /// <param name="originalCode">原始编码</param>
        /// <returns>唯一的基础编码</returns>
        private string GenerateUniqueBaseCode(string originalCode)
        {
            // 使用当前时间戳（精确到毫秒）和一个随机数来增强唯一性
            string timestamp = DateTime.Now.ToString("HHmmssfff");
            string random = new Random().Next(100, 999).ToString();

            // 将原始编码、时间戳和随机数组合起来
            // 为了避免过长，只取原始编码的前几位（如果原始编码很长）
            string shortOriginalCode = originalCode.Length > 5 ? originalCode.Substring(0, 5) : originalCode;

            // 移除可能的非数字字符，只保留数字
            string numericCode = new string(shortOriginalCode.Where(char.IsDigit).ToArray());

            // 如果原始编码中没有数字，则使用ASCII码值
            if (string.IsNullOrEmpty(numericCode))
            {
                numericCode = string.Join("", shortOriginalCode.Take(5).Select(c => ((int)c % 10).ToString()));
            }

            // 组合最终的唯一编码
            string uniqueCode = numericCode + timestamp + random;

            // 确保编码不会太长（最多20位）
            if (uniqueCode.Length > 20)
            {
                uniqueCode = uniqueCode.Substring(uniqueCode.Length - 20);
            }

            return uniqueCode;
        }

        /// <summary>
        /// 获取所有规则配置
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>规则配置列表</returns>
        public async Task<List<tb_sys_BillNoRule>> GetAllRuleConfigsAsync(CancellationToken ct = default)
        {
            return await _ruleConfigService.QueryAsync();
        }

        /// <summary>
        /// 保存规则配置
        /// </summary>
        /// <param name="config">规则配置</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>任务</returns>
        public async Task SaveRuleConfigAsync(tb_sys_BillNoRule config, CancellationToken ct = default)
        {
            // 已经在模型中设置了默认值，这里不需要额外处理
            await _ruleConfigService.SaveOrUpdate(config);
        }

        /// <summary>
        /// 删除规则配置
        /// </summary>
        /// <param name="id">规则配置ID</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>任务</returns>
        public async Task DeleteRuleConfigAsync(long id, CancellationToken ct = default)
        {
            await _ruleConfigService.DeleteAsync(id);
        }
    }
}