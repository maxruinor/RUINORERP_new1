
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/11/2025 15:53:34
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
    /// 维修入库单验证类
    /// </summary>
    /*public partial class tb_AS_RepairInStockValidator:AbstractValidator<tb_AS_RepairInStock>*/
    public partial class tb_AS_RepairInStockValidator:BaseValidatorGeneric<tb_AS_RepairInStock>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_AS_RepairInStockValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.RepairInStockNo).MaximumLength(25).WithMessage("维修入库单号:不能超过最大长度,25.");
 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.RepairInStockNo).NotEmpty().WithMessage("维修入库单号:不能为空。");

 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.RepairOrderID).Must(CheckForeignKeyValue).WithMessage("维修工单:下拉选择值不正确。");

 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.RepairOrderNo).MaximumLength(25).WithMessage("维修工单号:不能超过最大长度,25.");

 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("业务员:下拉选择值不正确。");
 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("客户:下拉选择值不正确。");

 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目小组:下拉选择值不正确。");
 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

//***** 
 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.TotalQty).NotNull().WithMessage("总数量:不能为空。");




 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");

 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.ApprovalOpinions).MaximumLength(100).WithMessage("审批意见:不能超过最大长度,100.");

 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

//***** 
 RuleFor(tb_AS_RepairInStock =>tb_AS_RepairInStock.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

           	                //long?
                //RepairInStockID
                //tb_AS_RepairInStockDetail
                //RuleFor(x => x.tb_AS_RepairInStockDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_AS_RepairInStockDetails).NotNull();
                //RuleForEach(x => x.tb_AS_RepairInStockDetails).NotNull();
                //RuleFor(x => x.tb_AS_RepairInStockDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_AS_RepairInStockDetail> details)
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

