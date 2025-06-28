using Mapster;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.BizCodeGeneration
{
    // 基于配置规则的编号生成器
    public class ConfigurableNumberGenerator : ISerialNumberGenerator
    {
        private readonly IDatabase _redisDb;
        private readonly ConcurrentDictionary<string, tb_sys_BillNoRule> _ruleCache = new ConcurrentDictionary<string, tb_sys_BillNoRule>();
        public ApplicationContext _appContext;
        public ConfigurableNumberGenerator(IDatabase redisDb, ApplicationContext appContext)
        {
            _redisDb = redisDb;
            _appContext = appContext;
        }

        public string Generate(string ruleName)
        {
            // 从缓存或数据库获取规则
            var rule = GetRule(ruleName);

            // 生成日期部分
            string datePart = GenerateDatePart((DateFormat)rule.DateFormat);

            // 生成Redis键
            string redisKey = $"{rule.RedisKeyPattern}:{datePart}";

            // 获取流水号
            long sequence = _redisDb.StringIncrement(redisKey);
            if (sequence == 1) // 如果是当天第一个，设置过期时间
            {
                _redisDb.KeyExpire(redisKey, TimeSpan.FromDays(rule.ResetMode));
            }

            // 格式化流水号
            string formattedSequence = sequence.ToString($"D{rule.SequenceLength}");

            // 组合完整编号
            string code = $"{rule.Prefix}{datePart}{formattedSequence}";

            // 添加校验位(如果启用)
            if (rule.UseCheckDigit)
            {
                code += CalculateCheckDigit(code);
            }

            return code;
        }

        private tb_sys_BillNoRule GetRule(string ruleName)
        {
            // 尝试从缓存获取
            if (_ruleCache.TryGetValue(ruleName, out var cachedRule))
            {
                return cachedRule;
            }

            // 从数据库获取

            var rule = _appContext.BillNoRules.FirstOrDefault(c => c.RuleName == ruleName);
            if (rule != null)
            {
                _ruleCache[ruleName] = rule;
            }
            else
            {
                //没有找到的话。应该要提供一个默认的生成规则？
            }

            return rule ?? throw new ArgumentException($"未找到规则: {ruleName}");
        }

        private string GenerateDatePart(DateFormat format)
        {
            switch (format)
            {
                case DateFormat.YearMonthDay:
                    return DateTime.Now.ToString("yyyyMMdd");
                case DateFormat.YearMonth:
                    return DateTime.Now.ToString("yyyyMM");
                case DateFormat.YearWeek:
                    return $"{DateTime.Now:yyyy}{GetIso8601WeekOfYear(DateTime.Now):00}";
                default:
                    return DateTime.Now.ToString("yyyyMMdd");
            }
        }

        // 计算ISO 8601周数
        private int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        // 计算校验位
        private string CalculateCheckDigit(string code)
        {
            // 简化的校验位计算示例
            int sum = 0;
            for (int i = 0; i < code.Length; i++)
            {
                sum += (code[i] - '0') * (i + 1);
            }
            return (sum % 10).ToString();
        }
    }
}
