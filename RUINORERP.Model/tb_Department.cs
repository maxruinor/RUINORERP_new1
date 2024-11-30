
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/29/2024 23:20:19
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
    /// 部门表是否分层
    /// </summary>
    [Serializable()]
    [Description("部门表是否分层")]
    [SugarTable("tb_Department")]
    public partial class tb_Department: BaseEntity, ICloneable
    {
        public tb_Department()
        {
            base.FieldNameList = fieldNameList;
            if (!PK_FK_ID_Check())
            {
                throw new Exception("部门表是否分层tb_Department" + "外键ID与对应主主键名称不一致。请修改数据库");
            }
        }


        #region 属性
        private long _DepartmentID;
        /// <summary>
        /// 部门
        /// </summary>
 
        [SugarColumn(ColumnDataType = "bigint", SqlParameterDbType ="Int64",  ColumnName = "DepartmentID" , DecimalDigits = 0,IsNullable = false,ColumnDescription = "部门" , IsPrimaryKey = true)]
        public long DepartmentID
        { 
            get{return _DepartmentID;}
            set{
            base.PrimaryKeyID = _DepartmentID;
            SetProperty(ref _DepartmentID, value);
            }
        }

        private string _DepartmentCode;
        /// <summary>
        /// 部门代号
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentCode",ColDesc = "部门代号")] 
        [SugarColumn(ColumnDataType = "char", SqlParameterDbType ="String",  ColumnName = "DepartmentCode" ,Length=20,IsNullable = false,ColumnDescription = "部门代号" )]
        public string DepartmentCode
        { 
            get{return _DepartmentCode;}
            set{
            SetProperty(ref _DepartmentCode, value);
            }
        }

        private string _DepartmentName;
        /// <summary>
        /// 部门名称
        /// </summary>
        [AdvQueryAttribute(ColName = "DepartmentName",ColDesc = "部门名称")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "DepartmentName" ,Length=255,IsNullable = false,ColumnDescription = "部门名称" )]
        public string DepartmentName
        { 
            get{return _DepartmentName;}
            set{
            SetProperty(ref _DepartmentName, value);
            }
        }

        private string _TEL;
        /// <summary>
        /// 电话
        /// </summary>
        [AdvQueryAttribute(ColName = "TEL",ColDesc = "电话")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "TEL" ,Length=20,IsNullable = true,ColumnDescription = "电话" )]
        public string TEL
        { 
            get{return _TEL;}
            set{
            SetProperty(ref _TEL, value);
            }
        }

        private string _Notes;
        /// <summary>
        /// 备注
        /// </summary>
        [AdvQueryAttribute(ColName = "Notes",ColDesc = "备注")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Notes" ,Length=200,IsNullable = true,ColumnDescription = "备注" )]
        public string Notes
        { 
            get{return _Notes;}
            set{
            SetProperty(ref _Notes, value);
            }
        }

        private string _Director;
        /// <summary>
        /// 责任人
        /// </summary>
        [AdvQueryAttribute(ColName = "Director",ColDesc = "责任人")] 
        [SugarColumn(ColumnDataType = "varchar", SqlParameterDbType ="String",  ColumnName = "Director" ,Length=20,IsNullable = true,ColumnDescription = "责任人" )]
        public string Director
        { 
            get{return _Director;}
            set{
            SetProperty(ref _Director, value);
            }
        }

        #endregion

        #region 扩展属性

        //[Browsable(false)]
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(tb_Employee.DepartmentID))]
        public virtual List<tb_Employee> tb_Employees { get; set; }
        //tb_Employee.DepartmentID)
        //DepartmentID.FKTB_EMPLOTB_DEPAR_aB)
        //tb_Department.DepartmentID)


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
                    Type type = typeof(tb_Department);
                    
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
            tb_Department loctype = (tb_Department)this.MemberwiseClone(); //创建当前对象的浅拷贝。
            return loctype;
        }
    }
}

