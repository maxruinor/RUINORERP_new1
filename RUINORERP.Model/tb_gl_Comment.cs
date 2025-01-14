
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:56:54
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
    /// 全局级批注表-对于重点关注的业务帮助记录和跟踪相关的额外信息，提高沟通效率和透明度
    /// </summary>
    [Serializable()]
    [Description("全局级批注表-对于重点关注的业务帮助记录和跟踪相关的额外信息，提高沟通效率和透明度")]
    [SugarTable("tb_gl_Comment")]
    public partial class tb_gl_Comment: BaseEntity, ICloneable
    {
        public tb_gl_Comment()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("全局级批注表-对于重点关注的业务帮助记录和跟踪相关的额外信息，提高沟通效率和透明度tb_gl_Comment" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _CommentID;
        /// <summary>
        /// 批注
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "CommentID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "批注" , IsPrimaryKey = true)]
        public long CommentID
        { 
            get{return _CommentID;}
            set{
            base.PrimaryKeyID = _CommentID;
            SetProperty(ref _CommentID, value);
            }
        }

        private long _Employee_ID;
        /// <summary>
        /// 批注人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "批注人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "批注人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
            }
        }

        private int _BizTypeID;
        /// <summary>
        /// 业务类型
        /// </summary>
        [AdvQueryAttribute(ColName = "BizTypeID",ColDesc = "业务类型")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "BizTypeID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "业务类型" )]
        public int BizTypeID
        { 
            get{return _BizTypeID;}
            set{
            SetProperty(ref _BizTypeID, value);
            }
        }

        private long _BusinessID;
        /// <summary>
        /// 关联业务
        /// </summary>
        [AdvQueryAttribute(ColName = "BusinessID",ColDesc = "关联业务")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "BusinessID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "关联业务" )]
        public long BusinessID
        { 
            get{return _BusinessID;}
            set{
            SetProperty(ref _BusinessID, value);
            }
        }

        private string _DbTableName;
        /// <summary>
        /// 关联表名
        /// </summary>
        [AdvQueryAttribute(ColName = "DbTableName",ColDesc = "关联表名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "DbTableName" ,Length=100,IsNullable = false,ColumnDescription = "关联表名" )]
        public string DbTableName
        { 
            get{return _DbTableName;}
            set{
            SetProperty(ref _DbTableName, value);
            }
        }

        private string _CommentContent;
        /// <summary>
        /// 批注内容
        /// </summary>
        [AdvQueryAttribute(ColName = "CommentContent",ColDesc = "批注内容")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CommentContent" ,Length=200,IsNullable = false,ColumnDescription = "批注内容" )]
        public string CommentContent
        { 
            get{return _CommentContent;}
            set{
            SetProperty(ref _CommentContent, value);
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
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }



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
                    Type type = typeof(tb_gl_Comment);
                    
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
            tb_gl_Comment loctype = (tb_gl_Comment)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

