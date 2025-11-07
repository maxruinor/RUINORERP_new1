
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 20:41:42
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
    /// 合同模板表
    /// </summary>
    [Serializable()]
    [Description("合同模板表")]
    [SugarTable("tb_ContractTemplate")]
    public partial class tb_ContractTemplate: BaseEntity, ICloneable
    {
        public tb_ContractTemplate()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("合同模板表tb_ContractTemplate" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _TemplateId;
        /// <summary>
        /// 合同模板
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "TemplateId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "合同模板" , IsPrimaryKey = true)]
        public long TemplateId
        { 
            get{return _TemplateId;}
            set{
            SetProperty(ref _TemplateId, value);
                base.PrimaryKeyID = _TemplateId;
            }
        }

        private long? _TemplateName;
        /// <summary>
        /// 模板名称
        /// </summary>
        [AdvQueryAttribute(ColName = "TemplateName",ColDesc = "模板名称")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "TemplateName" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "模板名称" )]
        public long? TemplateName
        { 
            get{return _TemplateName;}
            set{
            SetProperty(ref _TemplateName, value);
                        }
        }

        private byte[] _TemplateFile;
        /// <summary>
        /// 模板文件
        /// </summary>
        [AdvQueryAttribute(ColName = "TemplateFile",ColDesc = "模板文件")] 
        [SugarColumn(ColumnDataType = "varbinary", SqlParameterDbType ="Binary",  ColumnName = "TemplateFile" ,Length=-1,IsNullable = true,ColumnDescription = "模板文件" )]
        public byte[] TemplateFile
        { 
            get{return _TemplateFile;}
            set{
            SetProperty(ref _TemplateFile, value);
                        }
        }

        private string _FieldsConfig;
        /// <summary>
        /// 字段映射配置
        /// </summary>
        [AdvQueryAttribute(ColName = "FieldsConfig",ColDesc = "字段映射配置")] 
        [SugarColumn(ColumnDataType = "text", SqlParameterDbType ="String",  ColumnName = "FieldsConfig" ,Length=2147483647,IsNullable = true,ColumnDescription = "字段映射配置" )]
        public string FieldsConfig
        { 
            get{return _FieldsConfig;}
            set{
            SetProperty(ref _FieldsConfig, value);
                        }
        }

        private string _Remarks;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Remarks",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Remarks" ,Length=500,IsNullable = true,ColumnDescription = "备注" )]
        public string Remarks
        { 
            get{return _Remarks;}
            set{
            SetProperty(ref _Remarks, value);
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
        [Navigate(NavigateType.OneToMany, nameof(tb_SO_Contract.TemplateId))]
        public virtual List<tb_SO_Contract> tb_SO_Contracts { get; set; }
        //tb_SO_Contract.TemplateId)
        //TemplateId.FK_TB_SO_CO_REF_TB_CONTRACTTEMPLATE)
        //tb_ContractTemplate.TemplateId)

        //[Browsable(false)]打印报表时的数据源会不显示
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_PO_Contract.TemplateId))]
        public virtual List<tb_PO_Contract> tb_PO_Contracts { get; set; }
        //tb_PO_Contract.TemplateId)
        //TemplateId.FK_TB_PO_CO_REF_TB_CONTRACTEMPLATE)
        //tb_ContractTemplate.TemplateId)


        #endregion


 

//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_ContractTemplate loctype = (tb_ContractTemplate)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

