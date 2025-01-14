
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/14/2025 18:56:48
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
    /// 线索机会-询盘
    /// </summary>
    [Serializable()]
    [Description("线索机会-询盘")]
    [SugarTable("tb_CRM_Leads")]
    public partial class tb_CRM_Leads: BaseEntity, ICloneable
    {
        public tb_CRM_Leads()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("线索机会-询盘tb_CRM_Leads" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _LeadID;
        /// <summary>
        /// 线索
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "LeadID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "线索" , IsPrimaryKey = true)]
        public long LeadID
        { 
            get{return _LeadID;}
            set{
            base.PrimaryKeyID = _LeadID;
            SetProperty(ref _LeadID, value);
            }
        }

        private long _Employee_ID;
        /// <summary>
        /// 收集人
        /// </summary>
        [AdvQueryAttribute(ColName = "Employee_ID",ColDesc = "收集人")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Employee_ID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "收集人" )]
        [FKRelationAttribute("tb_Employee","Employee_ID")]
        public long Employee_ID
        { 
            get{return _Employee_ID;}
            set{
            SetProperty(ref _Employee_ID, value);
            }
        }

        private int _LeadsStatus;
        /// <summary>
        /// 线索状态
        /// </summary>
        [AdvQueryAttribute(ColName = "LeadsStatus",ColDesc = "线索状态")] 
        [SugarColumn(ColumnDataType = "int", SqlParameterDbType ="Int32",  ColumnName = "LeadsStatus" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "线索状态" )]
        public int LeadsStatus
        { 
            get{return _LeadsStatus;}
            set{
            SetProperty(ref _LeadsStatus, value);
            }
        }

        private string _wwSocialTools;
        /// <summary>
        /// 其他/IM工具
        /// </summary>
        [AdvQueryAttribute(ColName = "wwSocialTools",ColDesc = "其他/IM工具")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "wwSocialTools" ,Length=200,IsNullable = true,ColumnDescription = "其他/IM工具" )]
        public string wwSocialTools
        { 
            get{return _wwSocialTools;}
            set{
            SetProperty(ref _wwSocialTools, value);
            }
        }

        private string _SocialTools;
        /// <summary>
        /// 旺旺/IM工具
        /// </summary>
        [AdvQueryAttribute(ColName = "SocialTools",ColDesc = "旺旺/IM工具")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SocialTools" ,Length=200,IsNullable = true,ColumnDescription = "旺旺/IM工具" )]
        public string SocialTools
        { 
            get{return _SocialTools;}
            set{
            SetProperty(ref _SocialTools, value);
            }
        }

        private string _CustomerName;
        /// <summary>
        /// 客户名/线索名
        /// </summary>
        [AdvQueryAttribute(ColName = "CustomerName",ColDesc = "客户名/线索名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "CustomerName" ,Length=100,IsNullable = true,ColumnDescription = "客户名/线索名" )]
        public string CustomerName
        { 
            get{return _CustomerName;}
            set{
            SetProperty(ref _CustomerName, value);
            }
        }

        private string _GetCustomerSource;
        /// <summary>
        /// 获客来源
        /// </summary>
        [AdvQueryAttribute(ColName = "GetCustomerSource",ColDesc = "获客来源")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "GetCustomerSource" ,Length=250,IsNullable = true,ColumnDescription = "获客来源" )]
        public string GetCustomerSource
        { 
            get{return _GetCustomerSource;}
            set{
            SetProperty(ref _GetCustomerSource, value);
            }
        }

        private string _InterestedProducts;
        /// <summary>
        /// 兴趣产品
        /// </summary>
        [AdvQueryAttribute(ColName = "InterestedProducts",ColDesc = "兴趣产品")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "InterestedProducts" ,Length=50,IsNullable = true,ColumnDescription = "兴趣产品" )]
        public string InterestedProducts
        { 
            get{return _InterestedProducts;}
            set{
            SetProperty(ref _InterestedProducts, value);
            }
        }

        private string _Contact_Name;
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Name",ColDesc = "联系人姓名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Contact_Name" ,Length=50,IsNullable = true,ColumnDescription = "联系人姓名" )]
        public string Contact_Name
        { 
            get{return _Contact_Name;}
            set{
            SetProperty(ref _Contact_Name, value);
            }
        }

        private string _Contact_Phone;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Phone",ColDesc = "电话")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Contact_Phone" ,Length=50,IsNullable = true,ColumnDescription = "电话" )]
        public string Contact_Phone
        { 
            get{return _Contact_Phone;}
            set{
            SetProperty(ref _Contact_Phone, value);
            }
        }

        private string _Contact_Email;
        /// <summary>
        /// 邮箱
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Email",ColDesc = "邮箱")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Contact_Email" ,Length=100,IsNullable = true,ColumnDescription = "邮箱" )]
        public string Contact_Email
        { 
            get{return _Contact_Email;}
            set{
            SetProperty(ref _Contact_Email, value);
            }
        }

        private string _Position;
        /// <summary>
        /// 职位
        /// </summary>
        [AdvQueryAttribute(ColName = "Position",ColDesc = "职位")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Position" ,Length=50,IsNullable = true,ColumnDescription = "职位" )]
        public string Position
        { 
            get{return _Position;}
            set{
            SetProperty(ref _Position, value);
            }
        }

        private string _SalePlatform;
        /// <summary>
        /// 销售平台
        /// </summary>
        [AdvQueryAttribute(ColName = "SalePlatform",ColDesc = "销售平台")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SalePlatform" ,Length=50,IsNullable = true,ColumnDescription = "销售平台" )]
        public string SalePlatform
        { 
            get{return _SalePlatform;}
            set{
            SetProperty(ref _SalePlatform, value);
            }
        }

        private string _Address;
        /// <summary>
        /// 地址
        /// </summary>
        [AdvQueryAttribute(ColName = "Address",ColDesc = "地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Address" ,Length=255,IsNullable = true,ColumnDescription = "地址" )]
        public string Address
        { 
            get{return _Address;}
            set{
            SetProperty(ref _Address, value);
            }
        }

        private string _Website;
        /// <summary>
        /// 网址
        /// </summary>
        [AdvQueryAttribute(ColName = "Website",ColDesc = "网址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Website" ,Length=255,IsNullable = true,ColumnDescription = "网址" )]
        public string Website
        { 
            get{return _Website;}
            set{
            SetProperty(ref _Website, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=255,IsNullable = true,ColumnDescription = "备注" )]
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

        private bool? _isdeleted= false;
        /// <summary>
        /// 逻辑删除
        /// </summary>
        [AdvQueryAttribute(ColName = "isdeleted",ColDesc = "逻辑删除")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "isdeleted" ,IsNullable = true,ColumnDescription = "逻辑删除" )]
        [Browsable(false)]
        public bool? isdeleted
        { 
            get{return _isdeleted;}
            set{
            SetProperty(ref _isdeleted, value);
            }
        }

        private bool? _Converted= false;
        /// <summary>
        /// 已转化
        /// </summary>
        [AdvQueryAttribute(ColName = "Converted",ColDesc = "已转化")] 
        [SugarColumn(ColumnDataType = "bit", SqlParameterDbType ="Boolean",  ColumnName = "Converted" ,IsNullable = true,ColumnDescription = "已转化" )]
        public bool? Converted
        { 
            get{return _Converted;}
            set{
            SetProperty(ref _Converted, value);
            }
        }

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)]
        [Navigate(NavigateType.OneToOne, nameof(Employee_ID))]
        public virtual tb_Employee tb_employee { get; set; }


        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CRM_FollowUpRecords.LeadID))]
        public virtual List<tb_CRM_FollowUpRecords> tb_CRM_FollowUpRecordses { get; set; }
        //tb_CRM_FollowUpRecords.LeadID)
        //LeadID.FK_TB_CRM_F_REFERENCE_TB_CRM_L)
        //tb_CRM_Leads.LeadID)

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_CRM_Customer.LeadID))]
        public virtual List<tb_CRM_Customer> tb_CRM_Customers { get; set; }
        //tb_CRM_Customer.LeadID)
        //LeadID.FK_TB_CRM_C_REFERENCE_TB_CRM_L)
        //tb_CRM_Leads.LeadID)


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
                    Type type = typeof(tb_CRM_Leads);
                    
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
            tb_CRM_Leads loctype = (tb_CRM_Leads)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

