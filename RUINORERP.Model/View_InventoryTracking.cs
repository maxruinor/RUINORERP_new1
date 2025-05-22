
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/26/2024 11:47:02
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Base;

namespace RUINORERP.Model
{
    /// <summary>
    /// 库存跟踪视图
    /// </summary>
    [Serializable()]
    [SugarTable("View_InventoryTracking")]
    public partial class View_InventoryTracking: BaseViewEntity
    { 
        public View_InventoryTracking()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("View_InventoryTracking" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    






//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}

 

        public override object Clone()
        {
            tb_UserInfo loctype = (tb_UserInfo)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }



    }
}

