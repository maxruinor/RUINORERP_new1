
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:42:02
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
    /// 报表打印配置表
    /// </summary>
    [Serializable()]
    [Description("报表打印配置表")]
    [SugarTable("tb_PrintConfig")]
    public partial class tb_PrintConfig: BaseEntity, ICloneable
    {
        public tb_PrintConfig()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("报表打印配置表tb_PrintConfig" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PrintConfigID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PrintConfigID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PrintConfigID
        { 
            get{return _PrintConfigID;}
            set{
            SetProperty(ref _PrintConfigID, value);
                base.PrimaryKeyID = _PrintConfigID;
            }
        }

        private string _Config_Name;
        /// <summary>
        /// 配置名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Config_Name",ColDesc = "配置名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Config_Name" ,Length=100,IsNullable = false,ColumnDescription = "配置名称" )]
        public string Config_Name
        { 
            get{return _Config_Name;}
            set{
            SetProperty(ref _Config_Name, value);
                        }
        }

        private int _BizType;
        /// <summary>
        /// 业务类型
        /// </summary>
        [AdvQueryAttribute(ColName = "BizType",ColDesc = "业务类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BizType" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "业务类型" )]
        public int BizType
        { 
            get{return _BizType;}
            set{
            SetProperty(ref _BizType, value);
                        }
        }

        private string _BizName;
        /// <summary>
        /// 业务名称
        /// </summary>
        [AdvQueryAttribute(ColName = "BizName",ColDesc = "业务名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BizName" ,Length=30,IsNullable = false,ColumnDescription = "业务名称" )]
        public string BizName
        { 
            get{return _BizName;}
            set{
            SetProperty(ref _BizName, value);
                        }
        }

        private string _PrinterName;
        /// <summary>
        /// 打印机名称
        /// </summary>
        [AdvQueryAttribute(ColName = "PrinterName",ColDesc = "打印机名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "PrinterName" ,Length=200,IsNullable = true,ColumnDescription = "打印机名称" )]
        public string PrinterName
        { 
            get{return _PrinterName;}
            set{
            SetProperty(ref _PrinterName, value);
                        }
        }

        private bool? _PrinterSelected= false;
        /// <summary>
        /// 设置了默认打印机
        /// </summary>
        [AdvQueryAttribute(ColName = "PrinterSelected",ColDesc = "设置了默认打印机")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "PrinterSelected" ,IsNullable = true,ColumnDescription = "设置了默认打印机" )]
        public bool? PrinterSelected
        { 
            get{return _PrinterSelected;}
            set{
            SetProperty(ref _PrinterSelected, value);
                        }
        }

        private bool? _Landscape= false;
        /// <summary>
        /// 设置横向打印
        /// </summary>
        [AdvQueryAttribute(ColName = "Landscape",ColDesc = "设置横向打印")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Landscape" ,IsNullable = true,ColumnDescription = "设置横向打印" )]
        public bool? Landscape
        { 
            get{return _Landscape;}
            set{
            SetProperty(ref _Landscape, value);
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

        private bool _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = false,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        public bool isdeleted
        { 
            get{return _isdeleted;}
            set{
            SetProperty(ref _isdeleted, value);
                        }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PrintTemplate.PrintConfigID))]
        public virtual List<tb_PrintTemplate> tb_PrintTemplates { get; set; }
        //tb_PrintTemplate.PrintConfigID)
        //PrintConfigID.FK_PRINTTEMPLATE_REF_PRINTCONFIG)
        //tb_PrintConfig.PrintConfigID)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_PrintConfig loctype = (tb_PrintConfig)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

