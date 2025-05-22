
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/08/2025 16:31:55
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
    /// 联系人表-爱好跟进
    /// </summary>
    [Serializable()]
    [Description("联系人表-爱好跟进")]
    [SugarTable("tb_CRM_Contact")]
    public partial class tb_CRM_Contact: BaseEntity, ICloneable
    {
        public tb_CRM_Contact()
        {
            
            if (!PK_FK_ID_Check())
            {
                throw new Exception("联系人表-爱好跟进tb_CRM_Contact" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _Contact_id;
        /// <summary>
        /// 
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Contact_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "" , IsPrimaryKey = true)]
        public long Contact_id
        { 
            get{return _Contact_id;}
            set{
            SetProperty(ref _Contact_id, value);
                base.PrimaryKeyID = _Contact_id;
            }
        }

        private long _Customer_id;
        /// <summary>
        /// 目标客户
        /// </summary>
        [AdvQueryAttribute(ColName = "Customer_id",ColDesc = "目标客户")] 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "Customer_id" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "目标客户" )]
        [FKRelationAttribute("tb_CRM_Customer","Customer_id")]
        public long Customer_id
        { 
            get{return _Customer_id;}
            set{
            SetProperty(ref _Customer_id, value);
                        }
        }

        private string _SocialTools;
        /// <summary>
        /// 社交账号
        /// </summary>
        [AdvQueryAttribute(ColName = "SocialTools",ColDesc = "社交账号")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "SocialTools" ,Length=200,IsNullable = true,ColumnDescription = "社交账号" )]
        public string SocialTools
        { 
            get{return _SocialTools;}
            set{
            SetProperty(ref _SocialTools, value);
                        }
        }

        private string _Contact_Name;
        /// <summary>
        /// 姓名
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Name",ColDesc = "姓名")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Contact_Name" ,Length=50,IsNullable = true,ColumnDescription = "姓名" )]
        public string Contact_Name
        { 
            get{return _Contact_Name;}
            set{
            SetProperty(ref _Contact_Name, value);
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

        private string _Contact_Phone;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "Contact_Phone",ColDesc = "电话")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Contact_Phone" ,Length=30,IsNullable = true,ColumnDescription = "电话" )]
        public string Contact_Phone
        { 
            get{return _Contact_Phone;}
            set{
            SetProperty(ref _Contact_Phone, value);
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

        private string _Preferences;
        /// <summary>
        /// 爱好
        /// </summary>
        [AdvQueryAttribute(ColName = "Preferences",ColDesc = "爱好")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Preferences" ,Length=200,IsNullable = true,ColumnDescription = "爱好" )]
        public string Preferences
        { 
            get{return _Preferences;}
            set{
            SetProperty(ref _Preferences, value);
                        }
        }

        private string _Address;
        /// <summary>
        /// 联系地址
        /// </summary>
        [AdvQueryAttribute(ColName = "Address",ColDesc = "联系地址")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Address" ,Length=255,IsNullable = true,ColumnDescription = "联系地址" )]
        public string Address
        { 
            get{return _Address;}
            set{
            SetProperty(ref _Address, value);
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

        #endregion

        #region 扩展属性
        [SugarColumn(IsIgnore = true)]
        //[Browsable(false)] 打印报表时的数据源会不显示
        [Navigate(NavigateType.OneToOne, nameof(Customer_id))]
        public virtual tb_CRM_Customer tb_crm_customer { get; set; }



        #endregion




//如果为false,则不可以。
private bool PK_FK_ID_Check()
{
  bool rs=true;
return rs;
}





 

        public override object Clone()
        {
            tb_CRM_Contact loctype = (tb_CRM_Contact)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

