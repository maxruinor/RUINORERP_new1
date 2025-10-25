
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/25/2025 15:32:19
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
    /// 文件业务关联表
    /// </summary>
    [Serializable()]
    [Description("文件业务关联表")]
    [SugarTable("tb_FS_BusinessRelation")]
    public partial class tb_FS_BusinessRelation: BaseEntity, ICloneable
    {
        public tb_FS_BusinessRelation()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("文件业务关联表tb_FS_BusinessRelation" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _RelationId;
        /// <summary>
        /// 关联ID
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "RelationId" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "关联ID" , IsPrimaryKey = true)]
        public long RelationId
        { 
            get{return _RelationId;}
            set{
            SetProperty(ref _RelationId, value);
                base.PrimaryKeyID = _RelationId;
            }
        }

        private long? _FileId;
        /// <summary>
        /// 文件ID
        /// </summary>
        [AdvQueryAttribute(ColName = "FileId",ColDesc = "文件ID")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "FileId" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "文件ID" )]
        [FKRelationAttribute("tb_FS_FileStorageInfo","FileId")]
        public long? FileId
        { 
            get{return _FileId;}
            set{
            SetProperty(ref _FileId, value);
                        }
        }

        private int? _BusinessType;
        /// <summary>
        /// 业务类型
        /// </summary>
        [AdvQueryAttribute(ColName = "BusinessType",ColDesc = "业务类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BusinessType" , DecimalDigits = 0,IsNullable = true,ColumnDescription = "业务类型" )]
        public int? BusinessType
        { 
            get{return _BusinessType;}
            set{
            SetProperty(ref _BusinessType, value);
                        }
        }

        private string _BusinessNo;
        /// <summary>
        /// 业务编号
        /// </summary>
        [AdvQueryAttribute(ColName = "BusinessNo",ColDesc = "业务编号")] 
        [SugarColumn(ColumnDataType = "nvarchar", SqlParameterDbType ="String",  ColumnName = "BusinessNo" ,Length=50,IsNullable = true,ColumnDescription = "业务编号" )]
        public string BusinessNo
        { 
            get{return _BusinessNo;}
            set{
            SetProperty(ref _BusinessNo, value);
                        }
        }

        private bool _IsMainFile;
        /// <summary>
        /// 已注册
        /// </summary>
        [AdvQueryAttribute(ColName = "IsMainFile",ColDesc = "已注册")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "IsMainFile" ,IsNullable = false,ColumnDescription = "已注册" )]
        public bool IsMainFile
        { 
            get{return _IsMainFile;}
            set{
            SetProperty(ref _IsMainFile, value);
                        }
        }

        private DateTime? _Created_at = System.DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_at", ColDesc = "创建时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "Created_at", IsNullable = true, ColumnDescription = "创建时间")]
        public DateTime? Created_at
        {
            get { return _Created_at; }
            set
            {
                SetProperty(ref _Created_at, value);
            }
        }

        private long? _Created_by;
        /// <summary>
        /// 创建人
        /// </summary>
        [AdvQueryAttribute(ColName = "Created_by", ColDesc = "创建人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Created_by", DecimalDigits = 0, IsNullable = true, ColumnDescription = "创建人")]
        public long? Created_by
        {
            get { return _Created_by; }
            set
            {
                SetProperty(ref _Created_by, value);
            }
        }

        private DateTime? _Modified_at = System.DateTime.Now;
        /// <summary>
        /// 修改时间
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_at", ColDesc = "修改时间")]
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType = "DateTime", ColumnName = "Modified_at", IsNullable = true, ColumnDescription = "修改时间")]
        public DateTime? Modified_at
        {
            get { return _Modified_at; }
            set
            {
                SetProperty(ref _Modified_at, value);
            }
        }

        private long? _Modified_by;
        /// <summary>
        /// 修改人
        /// </summary>
        [AdvQueryAttribute(ColName = "Modified_by", ColDesc = "修改人")]
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType = "Int64", ColumnName = "Modified_by", DecimalDigits = 0, IsNullable = true, ColumnDescription = "修改人")]
        public long? Modified_by
        {
            get { return _Modified_by; }
            set
            {
                SetProperty(ref _Modified_by, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(FileId))]
        public virtual tb_FS_FileStorageInfo tb_fs_filestorageinfo { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}






       
        

        public override object Clone()
        {
            tb_FS_BusinessRelation loctype = (tb_FS_BusinessRelation)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

