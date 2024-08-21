
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:36:22
// **************************************
using System;
﻿using SqlSugar;
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
    /// 货架信息表验证类
    /// </summary>
    public partial class tb_StorageRackValidator:AbstractValidator<tb_StorageRack>
    {
     public tb_StorageRackValidator() 
     {
      RuleFor(tb_StorageRack =>tb_StorageRack.Location_ID).Must(CheckForeignKeyValueCanNull).WithMessage("所属仓库:下拉选择值不正确。");
 RuleFor(tb_StorageRack =>tb_StorageRack.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
 RuleFor(tb_StorageRack =>tb_StorageRack.RackNO).MaximumLength(50).WithMessage("货架编号:不能超过最大长度,50.");
 RuleFor(tb_StorageRack =>tb_StorageRack.RackNO).NotEmpty().WithMessage("货架编号:不能为空。");
 RuleFor(tb_StorageRack =>tb_StorageRack.RackName).MaximumLength(50).WithMessage("货架名称:不能超过最大长度,50.");
 RuleFor(tb_StorageRack =>tb_StorageRack.RackName).NotEmpty().WithMessage("货架名称:不能为空。");
 RuleFor(tb_StorageRack =>tb_StorageRack.RackLocation).MaximumLength(100).WithMessage("货架位置:不能超过最大长度,100.");
 RuleFor(tb_StorageRack =>tb_StorageRack.Desc).MaximumLength(100).WithMessage("描述:不能超过最大长度,100.");
       	
           	
     }




        private bool DetailedRecordsNotEmpty(List<tb_FinishedGoodsInvDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
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
        

        private bool DetailedRecordsNotEmpty(List<tb_StockOutDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_BOM_SDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_StocktakeDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_SaleOutDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_PurEntryDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_SaleOutReDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_PurEntryReDetail> details)
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

