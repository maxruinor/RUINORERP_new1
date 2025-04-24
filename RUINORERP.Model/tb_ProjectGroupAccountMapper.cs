
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/24/2025 10:43:38
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
    /// 项目组与账号映射配置表-销售订单收款时付款方式即可指定到收到哪个账号下面
    /// </summary>
    [Serializable()]
    [Description("项目组与账号映射配置表-销售订单收款时付款方式即可指定到收到哪个账号下面")]
    [SugarTable("tb_ProjectGroupAccountMapper")]
    public partial class tb_ProjectGroupAccountMapper: BaseEntity, ICloneable
    {
        public tb_ProjectGroupAccountMapper()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("项目组与账号映射配置表-销售订单收款时付款方式即可指定到收到哪个账号下面tb_ProjectGroupAccountMapper" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _PGAMID;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "PGAMID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long PGAMID
        { 
            get{return _PGAMID;}
            set{
            SetProperty(ref _PGAMID, value);
                base.PrimaryKeyID = _PGAMID;
            }
        }

        private long _ProjectGroup_ID;
        /// <summary>
        /// 项目组
        /// </summary>
        [AdvQueryAttribute(ColName = "ProjectGroup_ID",ColDesc = "项目组")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "ProjectGroup_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "项目组" )]
        [FKRelationAttribute("tb_ProjectGroup","ProjectGroup_ID")]
        public long ProjectGroup_ID
        { 
            get{return _ProjectGroup_ID;}
            set{
            SetProperty(ref _ProjectGroup_ID, value);
                        }
        }

        private long _Account_id;
        /// <summary>
        /// 公司账户
        /// </summary>
        [AdvQueryAttribute(ColName = "Account_id",ColDesc = "公司账户")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Account_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "公司账户" )]
        [FKRelationAttribute("tb_FM_Account","Account_id")]
        public long Account_id
        { 
            get{return _Account_id;}
            set{
            SetProperty(ref _Account_id, value);
                        }
        }

        private string _Description;
        /// <summary>
        /// 描述
        /// </summary>
        [AdvQueryAttribute(ColName = "Description",ColDesc = "描述")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Description" ,Length=50,IsNullable = true,ColumnDescription = "描述" )]
        public string Description
        { 
            get{return _Description;}
            set{
            SetProperty(ref _Description, value);
                        }
        }

        private DateTime _EffectiveDate;
        /// <summary>
        /// 生效日期
        /// </summary>
        [AdvQueryAttribute(ColName = "EffectiveDate",ColDesc = "生效日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "EffectiveDate" ,IsNullable = false,ColumnDescription = "生效日期" )]
        public DateTime EffectiveDate
        { 
            get{return _EffectiveDate;}
            set{
            SetProperty(ref _EffectiveDate, value);
                        }
        }

        private DateTime _ExpiryDate;
        /// <summary>
        /// 失效日期
        /// </summary>
        [AdvQueryAttribute(ColName = "ExpiryDate",ColDesc = "失效日期")] 
        [SugarColumn(ColumnDataType = "datetime", SqlParameterDbType ="DateTime",  ColumnName = "ExpiryDate" ,IsNullable = false,ColumnDescription = "失效日期" )]
        public DateTime ExpiryDate
        { 
            get{return _ExpiryDate;}
            set{
            SetProperty(ref _ExpiryDate, value);
                        }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Account_id))]
        public virtual tb_FM_Account tb_fm_account { get; set; }

        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(ProjectGroup_ID))]
        public virtual tb_ProjectGroup tb_projectgroup { get; set; }



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
                    Type type = typeof(tb_ProjectGroupAccountMapper);
                    
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
            tb_ProjectGroupAccountMapper loctype = (tb_ProjectGroupAccountMapper)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

