
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:57:00
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
    /// 打印模板
    /// </summary>
    [Serializable()]
    [Description("打印模板")]
    [SugarTable("tb_PrintTemplate")]
    public partial class tb_PrintTemplate: BaseEntity, ICloneable
    {
        public tb_PrintTemplate()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("打印模板tb_PrintTemplate" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _ID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long ID
        { 
            get{return _ID;}
            set{
            base.PrimaryKeyID = _ID;
            SetProperty(ref _ID, value);
            }
        }

        private long? _PrintConfigID;
        /// <summary>
        /// 
        /// </summary>
        [AdvQueryAttribute(ColName = "PrintConfigID",ColDesc = "")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PrintConfigID" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "" )]
        [FKRelationAttribute("tb_PrintConfig","PrintConfigID")]
        public long? PrintConfigID
        { 
            get{return _PrintConfigID;}
            set{
            SetProperty(ref _PrintConfigID, value);
            }
        }

        private string _Template_NO;
        /// <summary>
        /// 模板编号
        /// </summary>
        [AdvQueryAttribute(ColName = "Template_NO",ColDesc = "模板编号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Template_NO" ,Length=20,IsNullable = true,ColumnDescription = "模板编号" )]
        public string Template_NO
        { 
            get{return _Template_NO;}
            set{
            SetProperty(ref _Template_NO, value);
            }
        }

        private string _Template_Name;
        /// <summary>
        /// 模板名称
        /// </summary>
        [AdvQueryAttribute(ColName = "Template_Name",ColDesc = "模板名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Template_Name" ,Length=100,IsNullable = true,ColumnDescription = "模板名称" )]
        public string Template_Name
        { 
            get{return _Template_Name;}
            set{
            SetProperty(ref _Template_Name, value);
            }
        }

        private int? _BizType;
        /// <summary>
        /// 业务类型
        /// </summary>
        [AdvQueryAttribute(ColName = "BizType",ColDesc = "业务类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BizType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "业务类型" )]
        public int? BizType
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
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "BizName" ,Length=30,IsNullable = true,ColumnDescription = "业务名称" )]
        public string BizName
        { 
            get{return _BizName;}
            set{
            SetProperty(ref _BizName, value);
            }
        }

        private string _Templatet_Path;
        /// <summary>
        /// 模板路径
        /// </summary>
        [AdvQueryAttribute(ColName = "Templatet_Path",ColDesc = "模板路径")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Templatet_Path" ,Length=200,IsNullable = true,ColumnDescription = "模板路径" )]
        public string Templatet_Path
        { 
            get{return _Templatet_Path;}
            set{
            SetProperty(ref _Templatet_Path, value);
            }
        }

        private string _Template_DataSource;
        /// <summary>
        /// 模板数据源
        /// </summary>
        [AdvQueryAttribute(ColName = "Template_DataSource",ColDesc = "模板数据源")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Template_DataSource" ,Length=200,IsNullable = true,ColumnDescription = "模板数据源" )]
        public string Template_DataSource
        { 
            get{return _Template_DataSource;}
            set{
            SetProperty(ref _Template_DataSource, value);
            }
        }

        private string _TemplateFileData;
        /// <summary>
        /// 模板文件数据
        /// </summary>
        [AdvQueryAttribute(ColName = "TemplateFileData",ColDesc = "模板文件数据")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "TemplateFileData" ,Length=2147483647,IsNullable = false,ColumnDescription = "模板文件数据" )]
        public string TemplateFileData
        { 
            get{return _TemplateFileData;}
            set{
            SetProperty(ref _TemplateFileData, value);
            }
        }

        private byte[] _TemplateFileStream;
        /// <summary>
        /// 模板流数据
        /// </summary>
        [AdvQueryAttribute(ColName = "TemplateFileStream",ColDesc = "模板流数据")] 
        [SugarColumn(ColumnDataType = "varbinary", SqlParameterDbType ="Binary",  ColumnName = "TemplateFileStream" ,Length=-1,IsNullable = true,ColumnDescription = "模板流数据" )]
        public byte[] TemplateFileStream
        { 
            get{return _TemplateFileStream;}
            set{
            SetProperty(ref _TemplateFileStream, value);
            }
        }

        private bool? _IsDefaultTemplate= false;
        /// <summary>
        /// 默认模板
        /// </summary>
        [AdvQueryAttribute(ColName = "IsDefaultTemplate",ColDesc = "默认模板")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsDefaultTemplate" ,IsNullable = true,ColumnDescription = "默认模板" )]
        public bool? IsDefaultTemplate
        { 
            get{return _IsDefaultTemplate;}
            set{
            SetProperty(ref _IsDefaultTemplate, value);
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
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(PrintConfigID))]
        public virtual tb_PrintConfig tb_printconfig { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






        #region 字段描述对应列表
        private ConcurrentDictionary<string, string> fieldNameList;


        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("列名中文描述"), Category("自定属性")]
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        public override ConcurrentDictionary<string, string> FieldNameList
        {
            get
            {
                if (fieldNameList == null)
                {
                    fieldNameList = new ConcurrentDictionary<string, string>();
                    SugarColumn entityAttr;
                    Type type = typeof(tb_PrintTemplate);
                    
                       foreach (PropertyInfo field in type.GetProperties())
                            {
                                foreach (Attribute attr in field.GetCustomAttributes(true))
                                {
                                    entityAttr = attr as SugarColumn;
                                    if (null != entityAttr)
                                    {
                                        if (entityAttr.ColumnDescription == null)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsIdentity)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.IsPrimaryKey)
                                        {
                                            continue;
                                        }
                                        if (entityAttr.ColumnDescription.Trim().Length > 0)
                                        {
                                            fieldNameList.TryAdd(field.Name, entityAttr.ColumnDescription);
                                        }
                                    }
                                }
                            }
                }
                
                return fieldNameList;
            }
            set
            {
                fieldNameList = value;
            }

        }
        #endregion
        

        public override object Clone()
        {
            tb_PrintTemplate loctype = (tb_PrintTemplate)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

