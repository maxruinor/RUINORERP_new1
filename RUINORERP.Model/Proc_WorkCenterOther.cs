
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2024 19:41:20
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
    /// 
    /// </summary>
    [Serializable()]
    [SugarTable("Proc_WorkCenterOther")]
    public class Proc_WorkCenterOther : BaseEntity, ICloneable
    {
        public Proc_WorkCenterOther()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("Proc_WorkCenterPUR" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private string _单据状态;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "单据状态",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "单据状态", DecimalDigits = 255,Length=6,IsNullable = false,ColumnDescription = "单据状态")]
        [Display(Name = "")]
        public string 单据状态
        { 
            get{return _单据状态; }            set{                SetProperty(ref _单据状态, value);                }
        }

        private int? _数量;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "数量",ColDesc = "")]
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "数量" , DecimalDigits = 255,Length=4,IsNullable = true,ColumnDescription = "" )]
        [Display(Name = "")]
        public int? 数量 
        { 
            get{return _数量;}            set{                SetProperty(ref _数量, value);                }
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

