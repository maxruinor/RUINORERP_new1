
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
    /// 入库单 非生产领料/退料验证类
    /// </summary>
    /*public partial class tb_StockInValidator:AbstractValidator<tb_StockIn>*/
    public partial class tb_StockInValidator:BaseValidatorGeneric<tb_StockIn>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_StockInValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
//***** 
 RuleFor(tb_StockIn =>tb_StockIn.Type_ID).NotNull().WithMessage("入库类型:不能为空。");

 RuleFor(tb_StockIn =>tb_StockIn.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("外部来源单位:下拉选择值不正确。");
 RuleFor(tb_StockIn =>tb_StockIn.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

 RuleFor(tb_StockIn =>tb_StockIn.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("内部来源人员:下拉选择值不正确。");
 RuleFor(tb_StockIn =>tb_StockIn.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_StockIn =>tb_StockIn.BillNo).MaximumLength(25).WithMessage("其它入库单号:不能超过最大长度,25.");

//***** 
 RuleFor(tb_StockIn =>tb_StockIn.TotalQty).NotNull().WithMessage("总数量:不能为空。");

 RuleFor(x => x.TotalCost).PrecisionScale(19,4,true).WithMessage("总成本:小数位不能超过4。");

 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");



 RuleFor(tb_StockIn =>tb_StockIn.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");

 RuleFor(tb_StockIn =>tb_StockIn.RefBillID).NotEmpty().When(x => x.RefBillID.HasValue);

 RuleFor(tb_StockIn =>tb_StockIn.RefNO).MaximumLength(25).WithMessage("引用单号:不能超过最大长度,25.");

 RuleFor(tb_StockIn =>tb_StockIn.RefBizType).NotEmpty().When(x => x.RefBizType.HasValue);


 RuleFor(tb_StockIn =>tb_StockIn.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_StockIn =>tb_StockIn.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


//***** 
 RuleFor(tb_StockIn =>tb_StockIn.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_StockIn =>tb_StockIn.ApprovalOpinions).MaximumLength(250).WithMessage("审批意见:不能超过最大长度,250.");

 RuleFor(tb_StockIn =>tb_StockIn.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_StockIn =>tb_StockIn.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long
                //MainID
                //tb_StockInDetail
                //RuleFor(x => x.tb_StockInDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_StockInDetails).NotNull();
                //RuleForEach(x => x.tb_StockInDetails).NotNull();
                //RuleFor(x => x.tb_StockInDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_StockInDetail> details)
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

