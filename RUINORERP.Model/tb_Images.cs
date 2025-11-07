
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:55
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
    /// 图片表
    /// </summary>
    [Serializable()]
    [Description("图片表")]
    [SugarTable("tb_Images")]
    public partial class tb_Images: BaseEntity, ICloneable
    {
        public tb_Images()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("图片表tb_Images" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Images_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Images_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Images_ID
        { 
            get{return _Images_ID;}
            set{
            SetProperty(ref _Images_ID, value);
                base.PrimaryKeyID = _Images_ID;
            }
        }

        private string _Images;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Images",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Images" ,Length=255,IsNullable = true,ColumnDescription = "" )]
        public string Images
        { 
            get{return _Images;}
            set{
            SetProperty(ref _Images, value);
                        }
        }

        private string _Images_Path;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "Images_Path",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Images_Path" ,Length=500,IsNullable = true,ColumnDescription = "" )]
        public string Images_Path
        { 
            get{return _Images_Path;}
            set{
            SetProperty(ref _Images_Path, value);
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
            tb_Images loctype = (tb_Images)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

