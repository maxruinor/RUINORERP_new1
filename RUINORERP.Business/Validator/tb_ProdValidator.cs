
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:16
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using RUINORERP.Model.Context;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 产品基本信息表验证类
    /// </summary>
    /*public partial class tb_ProdValidator:AbstractValidator<tb_Prod>*/
    public partial class tb_ProdValidator : BaseValidatorGeneric<tb_Prod>
    {


        public tb_ProdValidator(ApplicationContext appContext = null) : base(appContext)
        {

            RuleFor(tb_Prod => tb_Prod.ProductNo).MaximumMixedLength(40).WithMessage("品号:不能超过最大长度,40.");
            RuleFor(tb_Prod => tb_Prod.ProductNo).NotEmpty().WithMessage("品号:不能为空。");
            RuleFor(tb_Prod => tb_Prod.Type_ID).Must(CheckForeignKeyValue).WithMessage("产品类型:下拉选择值不正确。");
            RuleFor(tb_Prod => tb_Prod.CNName).MaximumMixedLength(255).WithMessage("品名:不能超过最大长度,255.");
            RuleFor(tb_Prod => tb_Prod.CNName).NotEmpty().WithMessage("品名:不能为空。");

            RuleFor(tb_Prod => tb_Prod.ImagesPath).MaximumMixedLength(2000).WithMessage("图片组:不能超过最大长度,2000.");


            RuleFor(tb_Prod => tb_Prod.ENName).MaximumMixedLength(255).WithMessage("英文名称:不能超过最大长度,255.");

            RuleFor(tb_Prod => tb_Prod.Model).MaximumMixedLength(50).WithMessage("型号:不能超过最大长度,50.");

            RuleFor(tb_Prod => tb_Prod.VendorModelCode).MaximumMixedLength(50).WithMessage("厂商型号:不能超过最大长度,50.");

            RuleFor(tb_Prod => tb_Prod.ShortCode).MaximumMixedLength(50).WithMessage("短码:不能超过最大长度,50.");

            RuleFor(tb_Prod => tb_Prod.Specifications).MaximumMixedLength(1000).WithMessage("规格:不能超过最大长度,1000.");

            RuleFor(tb_Prod => tb_Prod.SourceType).NotEmpty().When(x => x.SourceType.HasValue);

            RuleFor(tb_Prod => tb_Prod.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("所属部门:下拉选择值不正确。");
            RuleFor(tb_Prod => tb_Prod.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

            //***** 
            RuleFor(tb_Prod => tb_Prod.PropertyType).NotNull().WithMessage("属性类型:不能为空。");

            RuleFor(tb_Prod => tb_Prod.Unit_ID).Must(CheckForeignKeyValue).WithMessage("单位:下拉选择值不正确。");

            RuleFor(tb_Prod => tb_Prod.Category_ID).Must(CheckForeignKeyValueCanNull).WithMessage("类别:下拉选择值不正确。");
            RuleFor(tb_Prod => tb_Prod.Category_ID).NotEmpty().When(x => x.Category_ID.HasValue);



            RuleFor(tb_Prod => tb_Prod.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("厂商:下拉选择值不正确。");
            RuleFor(tb_Prod => tb_Prod.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

            RuleFor(tb_Prod => tb_Prod.Location_ID).Must(CheckForeignKeyValueCanNull).WithMessage("默认仓库:下拉选择值不正确。");
            RuleFor(tb_Prod => tb_Prod.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);

            RuleFor(tb_Prod => tb_Prod.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("默认货架:下拉选择值不正确。");
            RuleFor(tb_Prod => tb_Prod.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);

            RuleFor(tb_Prod => tb_Prod.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("业务员:下拉选择值不正确。");
            RuleFor(tb_Prod => tb_Prod.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

            RuleFor(tb_Prod => tb_Prod.Brand).MaximumMixedLength(50).WithMessage("品牌:不能超过最大长度,50.");



            RuleFor(x => x.TaxRate).PrecisionScale(8, 3, true).WithMessage("税率:小数位不能超过3。");

            RuleFor(tb_Prod => tb_Prod.CustomsCode).MaximumMixedLength(30).WithMessage("海关编码:不能超过最大长度,30.");

            RuleFor(tb_Prod => tb_Prod.Tag).MaximumMixedLength(250).WithMessage("标签:不能超过最大长度,250.");


            //有默认值

            //有默认值

            RuleFor(tb_Prod => tb_Prod.Notes).MaximumMixedLength(255).WithMessage("备注:不能超过最大长度,255.");


            RuleFor(tb_Prod => tb_Prod.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


            RuleFor(tb_Prod => tb_Prod.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


            RuleFor(tb_Prod => tb_Prod.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

            //long?
            //ProdBaseID
            //tb_ProdDetail
            //RuleFor(x => x.tb_ProdDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
            //视图不需要验证，目前认为无编辑新增操作
            //RuleFor(c => c.tb_ProdDetails).NotNull();
            //RuleForEach(x => x.tb_ProdDetails).NotNull();
            //RuleFor(x => x.tb_ProdDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        }




        private bool DetailedRecordsNotEmpty(List<tb_ProdDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
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

