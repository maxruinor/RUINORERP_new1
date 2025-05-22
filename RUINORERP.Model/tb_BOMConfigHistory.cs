
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:53
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
    /// BOM配置历史 数据保存在BOM中 只是多份一样，细微区别用版本号标识
    /// </summary>
    [Serializable()]
    [Description("BOM配置历史 数据保存在BOM中 只是多份一样，细微区别用版本号标识")]
    [SugarTable("tb_BOMConfigHistory")]
    public partial class tb_BOMConfigHistory: BaseEntity, ICloneable
    {
        public tb_BOMConfigHistory()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("BOM配置历史 数据保存在BOM中 只是多份一样，细微区别用版本号标识tb_BOMConfigHistory" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _BOM_S_VERID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BOM_S_VERID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long BOM_S_VERID
        { 
            get{return _BOM_S_VERID;}
            set{
            SetProperty(ref _BOM_S_VERID, value);
                base.PrimaryKeyID = _BOM_S_VERID;
            }
        }

        private string _VerNo;
        /// <summary>
        /// 版本号
        /// </summary>
        [AdvQueryAttribute(ColName = "VerNo",ColDesc = "版本号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "VerNo" ,Length=50,IsNullable = false,ColumnDescription = "版本号" )]
        public string VerNo
        { 
            get{return _VerNo;}
            set{
            SetProperty(ref _VerNo, value);
                        }
        }

        private DateTime? _Effective_at;
        /// <summary>
        /// 生效时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Effective_at",ColDesc = "生效时间")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "Effective_at" ,IsNullable = true,ColumnDescription = "生效时间" )]
        public DateTime? Effective_at
        { 
            get{return _Effective_at;}
            set{
            SetProperty(ref _Effective_at, value);
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

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_BOM_S.BOM_S_VERID))]
        public virtual List<tb_BOM_S> tb_BOM_Ss { get; set; }
        //tb_BOM_S.BOM_S_VERID)
        //BOM_S_VERID.FK_TB_BOM_S_REFERENCE_TB_BOMCO)
        //tb_BOMConfigHistory.BOM_S_VERID)


        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}





 
        

        public override object Clone()
        {
            tb_BOMConfigHistory loctype = (tb_BOMConfigHistory)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

