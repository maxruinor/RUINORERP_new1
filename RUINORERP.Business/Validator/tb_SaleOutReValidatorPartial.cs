
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/08/2023 18:58:40
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
namespace RUINORERP.Business
{
    /// <summary>
    /// 销售出库退回单验证类
    /// </summary>
    public partial class tb_SaleOutReValidator : AbstractValidator<tb_SaleOutRe>
    {
        //public class CustomerValidator : AbstractValidator<tb_SaleOutRe>
        //{
        //    public CustomerValidator()
        //    {
        //        RuleFor(c => c.tb_SaleOutReDetails).NotNull();
        //        RuleForEach(x => x.tb_SaleOutReDetails).NotNull();
        //        //RuleFor(x => x.tb_SaleOutReDetails).SetInheritanceValidator(t =>
        //        //{
        //        //    t.Add<tb_SaleOutReDetail>(new tb_SaleOutReDetailValidator());
        //        //});
        //    }
        //}

    }
}

