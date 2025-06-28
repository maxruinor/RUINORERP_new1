using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.BizCodeGeneration
{
    // 优化后的业务编号生成器
    public class BizNumberGenerator
    {
        private readonly ISerialNumberGenerator _generator;
        private readonly Dictionary<BizType, string> _bizRules = new Dictionary<BizType, string>
    {
        { BizType.销售订单, "{S:SO}{D:yyMMdd}{redis:{S:销售订单}{D:yyMM}/000}" },
        { BizType.采购订单, "{S:PO}{D:yyMMdd}{redis:{S:采购订单}{D:yyMM}/000}" },
        { BizType.制令单, "{S:MO}{D:yyMMdd}{redis:{S:制令单}{D:yyMM}/00}" }
        // 其他业务类型规则
    };

        public BizNumberGenerator(ISerialNumberGenerator generator)
        {
            _generator = generator;
        }

        // 根据业务类型生成编号
        public string Generate(BizType bizType)
        {
            if (!_bizRules.ContainsKey(bizType))
                throw new ArgumentException($"未配置业务类型: {bizType} 的编号规则");

            string rule = _bizRules[bizType];
            return _generator.Generate(rule);
        }

        // 新增业务类型规则（支持运行时扩展）
        public void AddBizRule(BizType bizType, string rule)
        {
            _bizRules[bizType] = rule;
        }
    }

    // 使用示例
    public class BizCodeService
    {
        private readonly BizNumberGenerator _generator;

        public BizCodeService()
        {
            // 获取Redis生成器
            var generator = SerialNumberGeneratorFactory.GetGenerator();
            _generator = new BizNumberGenerator(generator);
        }

        public string GetSalesOrderNumber()
        {
            return _generator.Generate(BizType.销售订单);
        }
    }
}
