
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:35:23
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
    /// 流程步骤 转移条件集合验证类
    /// </summary>
    public partial class tb_NextNodesValidator:AbstractValidator<tb_NextNodes>
    {
     public tb_NextNodesValidator() 
     {
      RuleFor(tb_NextNodes =>tb_NextNodes.ConNodeConditions_Id).Must(CheckForeignKeyValueCanNull).WithMessage("条件:下拉选择值不正确。");
 RuleFor(tb_NextNodes =>tb_NextNodes.ConNodeConditions_Id).NotEmpty().When(x => x.ConNodeConditions_Id.HasValue);
 RuleFor(tb_NextNodes =>tb_NextNodes.NexNodeName).MaximumLength(50).WithMessage("下节点名称:不能超过最大长度,50.");
 RuleFor(tb_NextNodes =>tb_NextNodes.NexNodeName).NotEmpty().WithMessage("下节点名称:不能为空。");
       	
           	
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

