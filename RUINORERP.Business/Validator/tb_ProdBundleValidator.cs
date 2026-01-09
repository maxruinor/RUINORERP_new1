
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:17
// **************************************
using System;
﻿using SqlSugar;
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
    /// 产品套装表验证类
    /// </summary>
    /*public partial class tb_ProdBundleValidator:AbstractValidator<tb_ProdBundle>*/
    public partial class tb_ProdBundleValidator:BaseValidatorGeneric<tb_ProdBundle>
    {
     

     public tb_ProdBundleValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_ProdBundle =>tb_ProdBundle.BundleName).MaximumMixedLength(255).WithMessage("套装名称:不能超过最大长度,255.");
 RuleFor(tb_ProdBundle =>tb_ProdBundle.BundleName).NotEmpty().WithMessage("套装名称:不能为空。");

 RuleFor(tb_ProdBundle =>tb_ProdBundle.Description).MaximumMixedLength(255).WithMessage("描述:不能超过最大长度,255.");

 RuleFor(tb_ProdBundle =>tb_ProdBundle.Unit_ID).Must(CheckForeignKeyValue).WithMessage("套装单位:下拉选择值不正确。");

//***** 
 RuleFor(tb_ProdBundle =>tb_ProdBundle.TargetQty).NotNull().WithMessage("目标数量:不能为空。");

 RuleFor(tb_ProdBundle =>tb_ProdBundle.ImagesPath).MaximumMixedLength(2000).WithMessage("产品图片:不能超过最大长度,2000.");


 RuleFor(x => x.Weight).PrecisionScale(10,3,true).WithMessage("重量（千克）:小数位不能超过3。");

 RuleFor(x => x.Market_Price).PrecisionScale(19,4,true).WithMessage("市场零售价:小数位不能超过4。");

 RuleFor(tb_ProdBundle =>tb_ProdBundle.Notes).MaximumMixedLength(255).WithMessage("备注:不能超过最大长度,255.");

//有默认值

//有默认值


 RuleFor(tb_ProdBundle =>tb_ProdBundle.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_ProdBundle =>tb_ProdBundle.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


//***** 
 RuleFor(tb_ProdBundle =>tb_ProdBundle.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_ProdBundle =>tb_ProdBundle.ApprovalOpinions).MaximumMixedLength(500).WithMessage("审批意见:不能超过最大长度,500.");

 RuleFor(tb_ProdBundle =>tb_ProdBundle.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_ProdBundle =>tb_ProdBundle.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long
                //BundleID
                //tb_ProdBundleDetail
                //RuleFor(x => x.tb_ProdBundleDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_ProdBundleDetails).NotNull();
                //RuleForEach(x => x.tb_ProdBundleDetails).NotNull();
                //RuleFor(x => x.tb_ProdBundleDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
     }




        private bool DetailedRecordsNotEmpty(List<tb_ProdBundleDetail> details)
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

