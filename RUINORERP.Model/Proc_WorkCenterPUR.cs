
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
    [SugarTable("Proc_WorkCenterPUR")]
    public class Proc_WorkCenterPUR:BaseEntity, ICloneable
    {
        public Proc_WorkCenterPUR()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("Proc_WorkCenterPUR" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }

    
        private string _订单状态;
        
        
        /// <summary>
        /// 
        /// </summary>

        [AdvQueryAttribute(ColName = "订单状态",ColDesc = "")]
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "订单状态" , DecimalDigits = 255,Length=6,IsNullable = false,ColumnDescription = "" )]
        [Display(Name = "")]
        public string 订单状态 
        { 
            get{return _订单状态;}            set{                SetProperty(ref _订单状态, value);                }
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

