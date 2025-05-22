
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:58
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
    /// 文档表
    /// </summary>
    [Serializable()]
    [Description("文档表")]
    [SugarTable("tb_Documents")]
    public partial class tb_Documents: BaseEntity, ICloneable
    {
        public tb_Documents()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("文档表tb_Documents" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Doc_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Doc_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Doc_ID
        { 
            get{return _Doc_ID;}
            set{
            SetProperty(ref _Doc_ID, value);
                base.PrimaryKeyID = _Doc_ID;
            }
        }

        private string _Files_Path;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Files_Path",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Files_Path" ,Length=500,IsNullable = true,ColumnDescription = "" )]
        public string Files_Path
        { 
            get{return _Files_Path;}
            set{
            SetProperty(ref _Files_Path, value);
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
            tb_Documents loctype = (tb_Documents)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

