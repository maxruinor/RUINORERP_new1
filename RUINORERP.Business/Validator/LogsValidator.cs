
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:32
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
    /*public partial class LogsValidator:AbstractValidator<Logs>*/
    public partial class LogsValidator : BaseValidatorGeneric<Logs>
    {
        public LogsValidator()
        {
            RuleFor(Logs => Logs.Level).MaximumLength(5).WithMessage("级别:不能超过最大长度,5.");
            RuleFor(Logs => Logs.Logger).MaximumLength(25).WithMessage("记录器:不能超过最大长度,25.");
            RuleFor(Logs => Logs.Message).MaximumLength(1500).WithMessage("消息:不能超过最大长度,1500.");
            RuleFor(Logs => Logs.Operator).MaximumLength(25).WithMessage("操作者:不能超过最大长度,25.");
            RuleFor(Logs => Logs.ModName).MaximumLength(25).WithMessage("模块名:不能超过最大长度,25.");
            RuleFor(Logs => Logs.Path).MaximumLength(50).WithMessage("路径:不能超过最大长度,50.");
            RuleFor(Logs => Logs.ActionName).MaximumLength(25).WithMessage("动作:不能超过最大长度,25.");
            RuleFor(Logs => Logs.IP).MaximumLength(10).WithMessage("网络地址:不能超过最大长度,10.");
            RuleFor(Logs => Logs.MAC).MaximumLength(15).WithMessage("物理地址:不能超过最大长度,15.");
            RuleFor(Logs => Logs.MachineName).MaximumLength(25).WithMessage("电脑名:不能超过最大长度,25.");
            RuleFor(Logs => Logs.User_ID).Must(CheckForeignKeyValueCanNull).WithMessage("用户:下拉选择值不正确。");
            RuleFor(Logs => Logs.User_ID).NotEmpty().When(x => x.User_ID.HasValue);

            Initialize();
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

