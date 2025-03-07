
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2024 17:24:06
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using Castle.Core.Resource;
using System.Linq;
using RUINORERP.Global;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 箱规表验证类
    /// </summary>
    public partial class tb_SaleOrderValidator : BaseValidatorGeneric<tb_SaleOrder>
    {
        public override void Initialize()
        {
            // 这里添加额外的初始化代码
            RuleFor(x => x.tb_SaleOrderDetails).Must(list => list.Count > 0).WithMessage("销售明细不能为空。");
            RuleFor(x => x.TotalAmount).GreaterThan(0).When(x => x.tb_SaleOrderDetails.Any(s => s.Gift == false)).WithMessage("总金额：明细中有非赠品产品时，总金额要大于零。");//可非全赠品时 总金额要大于0.订单。
            RuleFor(x => x.TotalQty).GreaterThan(0).WithMessage("总数量：要大于零。");
            RuleFor(x => x.PlatformOrderNo).NotEmpty().When(c => c.IsFromPlatform).WithMessage("平台单时，平台订单号不能为空。");
            RuleFor(x => x.PayStatus).GreaterThan(0).WithMessage("付款状态:不能为空。");
            RuleFor(x => x.Paytype_ID).GreaterThan(0).When(c => c.PayStatus != (int)PayStatus.未付款).WithMessage("付款类型:有付款的情况下，付款类型不能为空。");
            RuleFor(x => x.Notes).MinimumLength(5).When(c => c.IsCustomizedOrder == true).WithMessage("备注:定制订单时下，备注内容长度必须超过5。");
        }
    }
}

