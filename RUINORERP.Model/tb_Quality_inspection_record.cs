
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:32:25
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
    /// 品质检验记录表
    /// </summary>
    [Serializable()]
    [Description("品质检验记录表")]
    [SugarTable("tb_Quality_inspection_record")]
    public partial class tb_Quality_inspection_record: BaseEntity, ICloneable
    {
        public tb_Quality_inspection_record()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("品质检验记录表tb_Quality_inspection_record" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _id;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long id
        { 
            get{return _id;}
            set{
            SetProperty(ref _id, value);
                base.PrimaryKeyID = _id;
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
            tb_Quality_inspection_record loctype = (tb_Quality_inspection_record)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

