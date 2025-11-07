
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:46
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
    /// 收藏表 收藏订单 产品 库存报警等
    /// </summary>
    [Serializable()]
    [Description("收藏表 收藏订单 产品 库存报警等")]
    [SugarTable("tb_Favorite")]
    public partial class tb_Favorite: BaseEntity, ICloneable
    {
        public tb_Favorite()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("收藏表 收藏订单 产品 库存报警等tb_Favorite" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ID;
        /// <summary>
        /// 我的收藏
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "我的收藏" , IsPrimaryKey = true)]
        public long ID
        { 
            get{return _ID;}
            set{
            SetProperty(ref _ID, value);
                base.PrimaryKeyID = _ID;
            }
        }

        private long? _ReferenceID;
        /// <summary>
        /// 引用ID
        /// </summary>
        [AdvQueryAttribute(ColName = "ReferenceID",ColDesc = "引用ID")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ReferenceID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "引用ID" )]
        public long? ReferenceID
        { 
            get{return _ReferenceID;}
            set{
            SetProperty(ref _ReferenceID, value);
                        }
        }

        private string _Ref_Table_Name;
        /// <summary>
        /// 引用表名
        /// </summary>
        [AdvQueryAttribute(ColName = "Ref_Table_Name",ColDesc = "引用表名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Ref_Table_Name" ,Length=100,IsNullable = true,ColumnDescription = "引用表名" )]
        public string Ref_Table_Name
        { 
            get{return _Ref_Table_Name;}
            set{
            SetProperty(ref _Ref_Table_Name, value);
                        }
        }

        private string _ModuleName;
        /// <summary>
        /// 模块名
        /// </summary>
        [AdvQueryAttribute(ColName = "ModuleName",ColDesc = "模块名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "ModuleName" ,Length=255,IsNullable = true,ColumnDescription = "模块名" )]
        public string ModuleName
        { 
            get{return _ModuleName;}
            set{
            SetProperty(ref _ModuleName, value);
                        }
        }

        private string _BusinessType;
        /// <summary>
        /// 业务类型
        /// </summary>
        [AdvQueryAttribute(ColName = "BusinessType",ColDesc = "业务类型")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BusinessType" ,Length=255,IsNullable = true,ColumnDescription = "业务类型" )]
        public string BusinessType
        { 
            get{return _BusinessType;}
            set{
            SetProperty(ref _BusinessType, value);
                        }
        }

        private bool _Public_enabled;
        /// <summary>
        /// 是否公开
        /// </summary>
        [AdvQueryAttribute(ColName = "Public_enabled",ColDesc = "是否公开")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Public_enabled" ,IsNullable = false,ColumnDescription = "是否公开" )]
        public bool Public_enabled
        { 
            get{return _Public_enabled;}
            set{
            SetProperty(ref _Public_enabled, value);
                        }
        }

        private bool _is_enabled;
        /// <summary>
        /// 是否启用
        /// </summary>
        [AdvQueryAttribute(ColName = "is_enabled",ColDesc = "是否启用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "is_enabled" ,IsNullable = false,ColumnDescription = "是否启用" )]
        public bool is_enabled
        { 
            get{return _is_enabled;}
            set{
            SetProperty(ref _is_enabled, value);
                        }
        }

        private bool _is_available;
        /// <summary>
        /// 是否可用
        /// </summary>
        [AdvQueryAttribute(ColName = "is_available",ColDesc = "是否可用")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "is_available" ,IsNullable = false,ColumnDescription = "是否可用" )]
        public bool is_available
        { 
            get{return _is_available;}
            set{
            SetProperty(ref _is_available, value);
                        }
        }

        private string _Notes;
        /// <summary>
        /// 备注说明
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注说明")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=500,IsNullable = true,ColumnDescription = "备注说明" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
                        }
        }

        private DateTime? _Created_at;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at",ColDesc = "创建时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Created_at" ,IsNullable = true,ColumnDescription = "创建时间" )]
        public DateTime? Created_at
        { 
            get{return _Created_at;}
            set{
            SetProperty(ref _Created_at, value);
                        }
        }

        private long? _Owner_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Owner_by",ColDesc = "创建人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Owner_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "创建人" )]
        public long? Owner_by
        { 
            get{return _Owner_by;}
            set{
            SetProperty(ref _Owner_by, value);
                        }
        }

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by",ColDesc = "创建人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Created_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "创建人" )]
        public long? Created_by
        { 
            get{return _Created_by;}
            set{
            SetProperty(ref _Created_by, value);
                        }
        }

        private DateTime? _Modified_at;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at",ColDesc = "修改时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Modified_at" ,IsNullable = true,ColumnDescription = "修改时间" )]
        public DateTime? Modified_at
        { 
            get{return _Modified_at;}
            set{
            SetProperty(ref _Modified_at, value);
                        }
        }

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by",ColDesc = "修改人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Modified_by" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "修改人" )]
        public long? Modified_by
        { 
            get{return _Modified_by;}
            set{
            SetProperty(ref _Modified_by, value);
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
            tb_Favorite loctype = (tb_Favorite)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

