
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:32
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 调拨单-两个仓库之间的库存转移验证类
    /// </summary>
    /*public partial class tb_StockTransferValidator:AbstractValidator<tb_StockTransfer>*/
    public partial class tb_StockTransferValidator:BaseValidatorGeneric<tb_StockTransfer>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_StockTransferValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_StockTransfer =>tb_StockTransfer.StockTransferNo).MaximumLength(25).WithMessage("调拨单号:不能超过最大长度,25.");

//***** 
 RuleFor(tb_StockTransfer =>tb_StockTransfer.Location_ID_from).NotNull().WithMessage("调出仓库:不能为空。");

//***** 
 RuleFor(tb_StockTransfer =>tb_StockTransfer.Location_ID_to).NotNull().WithMessage("调入仓库:不能为空。");

 RuleFor(tb_StockTransfer =>tb_StockTransfer.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");

//***** 
 RuleFor(tb_StockTransfer =>tb_StockTransfer.TotalQty).NotNull().WithMessage("总数量:不能为空。");

 RuleFor(x => x.TotalCost).PrecisionScale(19,4,true).WithMessage("总成本:小数位不能超过4。");

 RuleFor(x => x.TotalTransferAmount).PrecisionScale(19,4,true).WithMessage("调拨金额:小数位不能超过4。");



 RuleFor(tb_StockTransfer =>tb_StockTransfer.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_StockTransfer =>tb_StockTransfer.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_StockTransfer =>tb_StockTransfer.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");


//***** 
 RuleFor(tb_StockTransfer =>tb_StockTransfer.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_StockTransfer =>tb_StockTransfer.ApprovalOpinions).MaximumLength(250).WithMessage("审批意见:不能超过最大长度,250.");

 RuleFor(tb_StockTransfer =>tb_StockTransfer.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_StockTransfer =>tb_StockTransfer.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long
                //StockTransferID
                //tb_StockTransferDetail
                //RuleFor(x => x.tb_StockTransferDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_StockTransferDetails).NotNull();
                //RuleForEach(x => x.tb_StockTransferDetails).NotNull();
                //RuleFor(x => x.tb_StockTransferDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_StockTransferDetail> details)
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

