
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/25/2023 13:09:43
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using FluentValidation;
using RUINORERP.Model;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace RUINORERP.Business
{
    /// <summary>
    /// 产品表验证类
    /// </summary>
    public partial class tb_ProdValidator : BaseValidatorGeneric<tb_Prod>
    {

        public override void Initialize()
        {

            RuleFor(tb_Product => tb_Product.Category_ID).NotEmpty().WithMessage("类别不能为空。");
            RuleFor(tb_Product => tb_Product.ProductNo)
            .Custom((value, context) =>
            {
                var prod = context.InstanceToValidate as tb_Prod; 

                // 确保实体不为null  并且是新增时才判断
                if (prod != null && prod.ProdBaseID == 0)
                {
                    string propertyName = context.PropertyName;
                    // 在这里使用 propertyName
                    // System.Diagnostics.Debug.WriteLine($"正在验证的属性: {propertyName}");
                    // 实际的唯一性验证逻辑
                    if (!BeUniqueName(propertyName, value))
                    {
                        context.AddFailure("品号不能重复。");
                    }
                }
            });

            RuleFor(tb_Prod => tb_Prod.PropertyType).NotNull().WithMessage("产品类型:不能为空。");
            RuleFor(x => x.PropertyType).NotEmpty().WithMessage("属性类型必须输入。");
            RuleFor(tb_Prod => tb_Prod.Type_ID).Must(CheckForeignKeyValue).WithMessage("产品类型:下拉选择值不正确。");
            RuleFor(x => x.Location_ID).NotNull().WithMessage("默认仓库不能为空。");
            RuleFor(tb_Prod => tb_Prod.Location_ID).Must(CheckForeignKeyValueCanNull).WithMessage("默认仓库:下拉选择值不正确。");
        }

       
    }




}