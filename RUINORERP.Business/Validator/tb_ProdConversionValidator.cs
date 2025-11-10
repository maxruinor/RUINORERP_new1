
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
    /// 产品转换单 A变成B出库,AB相近。可能只是换说明书或刷机  A  数量  加或减 。B数量增加或减少。验证类
    /// </summary>
    /*public partial class tb_ProdConversionValidator:AbstractValidator<tb_ProdConversion>*/
    public partial class tb_ProdConversionValidator:BaseValidatorGeneric<tb_ProdConversion>
    {
     

     public tb_ProdConversionValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_ProdConversion =>tb_ProdConversion.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("经办人:下拉选择值不正确。");
 RuleFor(tb_ProdConversion =>tb_ProdConversion.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_ProdConversion =>tb_ProdConversion.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_ProdConversion =>tb_ProdConversion.ConversionNo).MaximumMixedLength(50).WithMessage("转换单号:不能超过最大长度,50.");


//***** 
 RuleFor(tb_ProdConversion =>tb_ProdConversion.TotalConversionQty).NotNull().WithMessage("转换数量:不能为空。");

 RuleFor(tb_ProdConversion =>tb_ProdConversion.Reason).MaximumMixedLength(300).WithMessage("备注:不能超过最大长度,300.");

 RuleFor(tb_ProdConversion =>tb_ProdConversion.Notes).MaximumMixedLength(1500).WithMessage("备注:不能超过最大长度,1500.");


 RuleFor(tb_ProdConversion =>tb_ProdConversion.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_ProdConversion =>tb_ProdConversion.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


//***** 
 RuleFor(tb_ProdConversion =>tb_ProdConversion.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_ProdConversion =>tb_ProdConversion.ApprovalOpinions).MaximumMixedLength(500).WithMessage("审批意见:不能超过最大长度,500.");

 RuleFor(tb_ProdConversion =>tb_ProdConversion.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_ProdConversion =>tb_ProdConversion.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long
                //ConversionID
                //tb_ProdConversionDetail
                //RuleFor(x => x.tb_ProdConversionDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_ProdConversionDetails).NotNull();
                //RuleForEach(x => x.tb_ProdConversionDetails).NotNull();
                //RuleFor(x => x.tb_ProdConversionDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_ProdConversionDetail> details)
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

