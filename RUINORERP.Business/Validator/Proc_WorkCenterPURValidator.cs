
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2024 19:41:21
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 验证类
    /// </summary>
    public partial class Proc_WorkCenterPURValidator : AbstractValidator<Proc_WorkCenterPUR>
    {
        public Proc_WorkCenterPURValidator()
        {
            RuleFor(Proc_WorkCenterPUR => Proc_WorkCenterPUR.订单状态).MaximumLength(6).WithMessage(":不能超过最大长度,6.");
            RuleFor(Proc_WorkCenterPUR => Proc_WorkCenterPUR.订单状态).NotEmpty().WithMessage(":不能为空。");
            RuleFor(Proc_WorkCenterPUR => Proc_WorkCenterPUR.数量).NotEmpty().When(x => x.数量.HasValue);


        }








        private bool CheckForeignKeyValue(long ForeignKeyID)
        {
            bool rs = true;
            if (ForeignKeyID == 0 || ForeignKeyID == -1)
            {
                return false;
            }
            return rs;
        }

        private bool CheckForeignKeyValueCanNull(long? ForeignKeyID)
        {
            bool rs = true;
            if (ForeignKeyID.HasValue)
            {
                if (ForeignKeyID == 0 || ForeignKeyID == -1)
                {
                    return false;
                }
            }
            return rs;

        }
    }

}

