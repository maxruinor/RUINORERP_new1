
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:39
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
    /// 业务类型
    /// </summary>
    [Serializable()]
    [Description("业务类型")]
    [SugarTable("tb_BizType")]
    public partial class tb_BizType: BaseEntity, ICloneable
    {
        public tb_BizType()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("业务类型tb_BizType" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Type_ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Type_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Type_ID
        { 
            get{return _Type_ID;}
            set{
            SetProperty(ref _Type_ID, value);
                base.PrimaryKeyID = _Type_ID;
            }
        }

        private string _TypeName;
        /// <summary>
        /// 类型名称
        /// </summary>
        [AdvQueryAttribute(ColName = "TypeName",ColDesc = "类型名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "TypeName" ,Length=50,IsNullable = false,ColumnDescription = "类型名称" )]
        public string TypeName
        { 
            get{return _TypeName;}
            set{
            SetProperty(ref _TypeName, value);
                        }
        }

        private string _TypeDesc;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "TypeDesc",ColDesc = "描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "TypeDesc" ,Length=100,IsNullable = true,ColumnDescription = "描述" )]
        public string TypeDesc
        { 
            get{return _TypeDesc;}
            set{
            SetProperty(ref _TypeDesc, value);
                        }
        }

        private string _Module;
        /// <summary>
        /// 所属模块
        /// </summary>
        [AdvQueryAttribute(ColName = "Module",ColDesc = "所属模块")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Module" ,Length=50,IsNullable = true,ColumnDescription = "所属模块" )]
        public string Module
        { 
            get{return _Module;}
            set{
            SetProperty(ref _Module, value);
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
            tb_BizType loctype = (tb_BizType)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

