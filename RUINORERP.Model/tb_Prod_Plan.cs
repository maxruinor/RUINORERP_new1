
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2023 00:04:46
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Collections.Concurrent;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.Model
{
    /// <summary>
    /// 生产计划表
    /// </summary>
    [Serializable()]
    [SugarTable("tb_Prod_Plan")]
    public partial class tb_Prod_Plan: BaseEntity, ICloneable
    {
        public tb_Prod_Plan()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("tb_Prod_Plan" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
    
        
        private int? _id;
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "id",IsNullable = true,ColumnDescription = "" )]
        public int? id
        { 
            get{return _id;}
            set{
            SetProperty(ref _id, value);
            }
        }

        #endregion

        #region 扩展属性


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}




        public override object Clone()
        {
            tb_Prod_Plan loctype = (tb_Prod_Plan)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

