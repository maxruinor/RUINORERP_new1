using RUINORERP.Model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.BizCodeGeneration
{

    // 基于Redis的序号生成器（核心实现）
    public class RedisSerialNumberGenerator : ISerialNumberGenerator
    {
        private readonly IDatabase _redisDb;
        private readonly Dictionary<string, tb_sys_BillNoRule> _ruleCache = new Dictionary<string, tb_sys_BillNoRule>();
        private readonly object _lockObj = new object();

        public RedisSerialNumberGenerator(IDatabase redisDb)
        {
            _redisDb = redisDb;
        }

        public string Generate(string rule)
        {
            // 解析规则并缓存
            var ruleModel = ParseRule(rule);

            // 生成Redis键（包含业务类型和日期）
            string redisKey = $"{ruleModel.BizType}:{ruleModel.DateFormat}";

            // 使用Lua脚本确保原子性（获取并递增序号）
            string luaScript = @"
            local key = KEYS[1]
            local seq = redis.call('incr', key)
            local format = ARGV[1]
            return string.format(format, seq)
        ";

            // 执行Lua脚本，返回格式化后的序号
            string sequence = _redisDb.ScriptEvaluate(
                luaScript,
                new RedisKey[] { redisKey },
                new RedisValue[] { ruleModel.SequenceLength }
            ).ToString();

            // 组合完整编号
            return $"{ruleModel.Prefix}{ruleModel.DateFormat}{sequence}";
        }

        // 规则解析与缓存
        private tb_sys_BillNoRule ParseRule(string rule)
        {
            if (_ruleCache.TryGetValue(rule, out tb_sys_BillNoRule ruleModel))
                return ruleModel;

            lock (_lockObj)
            {
                if (_ruleCache.TryGetValue(rule, out ruleModel))
                    return ruleModel;

                // 解析规则字符串，示例：{S:SO}{D:yyMMdd}{redis:{S:销售订单}{D:yyMM}/000}
                // 通过规则字符串来 给出规则 。可以是默认的，暂时不实现
                //ruleModel = new tb_sys_BillNoRule
                //{
                //    Prefix = ExtractPrefix(rule),
                //    DateFormat = ExtractDatePart(rule),
                //    BizType = ExtractBizType(rule),
                //    SeqFormat = ExtractSeqFormat(rule)
                //};

                //_ruleCache[rule] = ruleModel;
                //return ruleModel;
            }
            return null;
        }

 

        // 规则解析辅助方法（需根据实际规则语法实现）
        private string ExtractPrefix(string rule) => "SO"; // 简化实现，实际需解析规则
        private string ExtractDatePart(string rule) => DateTime.Now.ToString("yyMMdd");
        private string ExtractBizType(string rule) => "销售订单";
        private string ExtractSeqFormat(string rule) => "0000";
    }// 序号生成器工厂（支持扩展多种生成策略）
}
