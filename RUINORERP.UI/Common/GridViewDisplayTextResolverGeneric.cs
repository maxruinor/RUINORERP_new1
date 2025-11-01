using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using RUINORERP.Business.CommService;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;
using Autofac;
using Krypton.Toolkit;
using RUINORERP.Common.Helper;
using Krypton.Workspace;
using Krypton.Navigator;
using RUINORERP.Model;
using RUINOR.Core;
using RUINORERP.UI.Common;
using RUINORERP.UI.BI;
using RUINORERP.Common.CustomAttribute;
using System.Collections.Concurrent;
using RUINORERP.Business;
using RUINORERP.Global.CustomAttribute;
using ObjectsComparer;
using RUINORERP.Common.Extensions;
using RUINORERP.Business.AutoMapper;
using AutoMapper;
using RUINORERP.Model.Base;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.Business.Processor;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;


using RUINORERP.Model.Models;
using System.Diagnostics;
using RUINORERP.Global.Model;
using RUINORERP.UI.CommonUI;
using RUINORERP.UI.FormProperty;
using System.Web.UI;
using Control = System.Windows.Forms.Control;
using SqlSugar;
using SourceGrid.Cells.Models;
using SixLabors.ImageSharp.Memory;
using Netron.NetronLight;
using RUINOR.WinFormsUI.CustomPictureBox;
using RUINORERP.UI.UserCenter;
using RUINORERP.UI.UserPersonalized;
using RUINORERP.UI.UControls;
using Newtonsoft.Json;

using RUINORERP.Global;
using FastReport.Table;
using Newtonsoft.Json.Linq;
using FastReport.DevComponents.DotNetBar;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Business.Cache;


namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 用于解析DataGridView显示ID列时，转换为对应名称的工具类
    /// </summary>
    public class GridViewDisplayTextResolverGeneric<T> where T : class
    {


        public GridViewDisplayTextResolverGeneric()
        {

        }

        GridViewDisplayHelper displayHelper = new GridViewDisplayHelper();

        // 初始化方法
        public void Initialize(DataGridView dataGridView)
        {
            // 初始化固定字典映射
            displayHelper.InitializeFixedDictionaryMappings<T>();
            displayHelper.InitializeReferenceKeyMapping<T>();
            dataGridView.CellFormatting += DataGridView_CellFormatting;
        }


        /// <summary>
        /// 手动添加外键关联指向
        /// </summary>
        /// <typeparam name="source">from单位表</typeparam>
        /// <typeparam name="target">to单位换算，产品等引用单位的表，字段和主键不一样时使用</typeparam>
        /// <param name="SourceField"></param>
        /// <param name="TargetField"></param>
        /// <param name="CustomDisplaySourceField">如果有特殊指定显示为其它列时</param>
        public void AddReferenceKeyMapping<source, target>(Expression<Func<source, object>> SourceField, Expression<Func<target, object>> TargetField, Expression<Func<source, object>> CustomDisplaySourceField = null)
        {
            MemberInfo Sourceinfo = SourceField.GetMemberInfo();
            MemberInfo Targetinfo = TargetField.GetMemberInfo();

            if (displayHelper.ReferenceKeyMappings == null)
            {
                displayHelper.ReferenceKeyMappings = new HashSet<ReferenceKeyMapping>();
            }
            ReferenceKeyMapping mapping = new ReferenceKeyMapping(typeof(source).Name, Sourceinfo.Name, typeof(target).Name, Targetinfo.Name);
            if (typeof(source).Name == typeof(target).Name)
            {
                mapping.IsSelfReferencing = true;
            }
            //只处理需要缓存的表
            // 使用新的缓存管理器，不再需要检查表是否在缓存中
              {
                 KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                 if (MyCacheManager.Instance.NewTableList.TryGetValue(mapping.ReferenceTableName, out pair))
                 {
                     //要显示的默认值是从缓存表中获取的字段名，默认是主键ID字段对应的名称
                     mapping.ReferenceDefaultDisplayFieldName = pair.Value;
                 }
              }
            if (CustomDisplaySourceField != null)
            {
                MemberInfo CustomDisplayColInfo = CustomDisplaySourceField.GetMemberInfo();
                mapping.CustomDisplayColumnName = CustomDisplayColInfo.Name;
            }
            //以目标为主键，原始的相同的只能为值
            displayHelper.ReferenceKeyMappings.Add(mapping);
        }


        // 添加固定字典映射
        public void AddFixedDictionaryMapping(string tableName, string columnName, List<KeyValuePair<object, string>> mappings)
        {
            if (typeof(T).GetProperty(tableName) != null)
            {
                displayHelper.FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(T).Name, columnName, CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus))));
                displayHelper.FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(T).Name, nameof(ApprovalStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus))));
            }
        }

        /// <summary>
        /// 添加枚举类型的固定字典映射，枚举类型做为显示值，枚举的名称做为键值
        /// </summary>
        /// <typeparam name="target">指定了目标表</typeparam>
        /// <param name="TargetField"></param>
        /// <param name="enumType"></param>
        public void AddFixedDictionaryMappingByEnum<target>(Expression<Func<target, object>> TargetField, Type enumType)
        {
            MemberInfo Targetinfo = TargetField.GetMemberInfo();
            displayHelper.FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(target).Name, Targetinfo.Name, CommonHelper.Instance.GetKeyValuePairs(enumType)));
        }

        /// <summary>
        /// 目标表为T
        /// </summary>
        /// <param name="TargetField"></param>
        /// <param name="enumType"></param>
        public void AddFixedDictionaryMappingByEnum(Expression<Func<T, object>> TargetField, Type enumType)
        {
            MemberInfo Targetinfo = TargetField.GetMemberInfo();
            displayHelper.FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(T).Name, Targetinfo.Name, CommonHelper.Instance.GetKeyValuePairs(enumType)));
        }



        // 添加列显示类型
        public void AddColumnDisplayType(string columnName, string displayType)
        {
            displayHelper.ColumnDisplayTypes[columnName] = displayType;
        }

        // 添加外键列映射
        public void AddReferenceKeyColumnMapping(string columnName, string foreignKeyColumnName)
        {
            displayHelper.ReferenceKeyColumnMappings[columnName] = foreignKeyColumnName;
        }

        // 单元格格式化事件处理
        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;

            // 如果列是隐藏的，直接返回
            if (!dataGridView.Columns[e.ColumnIndex].Visible)
            {
                return;
            }

            if (e.Value == null)
            {
                e.Value = "";
                return;
            }

            // 获取列名
            string columnName = dataGridView.Columns[e.ColumnIndex].Name;
            // 处理特殊列类型（如图片）
            if (displayHelper.ColumnDisplayTypes.ContainsKey(columnName))
            {
                string displayType = displayHelper.ColumnDisplayTypes[columnName];
                if (displayType == "Image")
                {
                    if (e.Value is byte[])
                    {
                        using (MemoryStream ms = new MemoryStream((byte[])e.Value))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                            e.Value = image;
                            return;
                        }
                    }
                }
            }
            if (typeof(T).Name.Contains("tb_"))
            {
                MyCacheManager.Instance.SetFkColList(typeof(T));
            }
            e.Value = displayHelper.GetGridViewDisplayText(typeof(T).Name, columnName, e.Value);
            return;
        }



    }

    // 外键映射类 多种情况 
    //普通映射 一个表对应一个主键字段 一个显示名称字段
    //比方菜单表中，引用了模块用的ID，如果ID名和模块名一样时。是一种情况。如果不一样，则要指定别名。


    /// <summary>
    /// 以显示tb_Unit_Conversion为例子
    /// </summary>
    public class ReferenceKeyMapping
    {
        /// <summary>
        /// 引用的表名
        /// Unit_ID
        /// </summary>
        public string ReferenceTableName { get; set; } // 表名

        /// <summary>
        /// 引用的表中的键字段名
        /// 通常是关联表的主键字段名，比方菜单表中引用了模块用的ID
        /// Unit_ID
        /// </summary>
        public string ReferenceOriginalFieldName { get; set; } // ID键字段名


        /// <summary>
        /// 要显示的表的目标的字段名，通常可能会和引用的关联表主键值一样。
        /// 如果不一样时才要特别指定
        /// tb_Unit_Conversion
        /// </summary>
        public string MappedTargetTableName { get; set; } // ID键字段名

        /// <summary>
        /// 要显示的表的目标的字段名，通常可能会和引用的关联表主键值一样。
        /// 如果不一样时才要特别指定
        /// Source_unit_id
        /// </summary>
        public string MappedTargetFieldName { get; set; } // ID键字段名

        /// <summary>
        /// 一般是键值对，但是有时可能是编号，
        /// 如：仓库ID 默认是仓库名称，也可以显示为仓库编号
        /// 这里是默认的
        /// UnitName
        /// </summary>
        public string ReferenceDefaultDisplayFieldName { get; set; } // 将要显示的名称字段名

        /// <summary>
        /// 一般是键值对，但是有时可能是编号，
        /// 如：仓库ID 默认是仓库名称，也可以显示为仓库编号
        /// UnitNo 【实际单据没有这个字段】 这里代指  有名称 有编号的情况
        /// </summary>
        public string CustomDisplayColumnName { get; set; }


        /// <summary>
        /// 一般是键值对，但是有时可能是编号，
        /// 如：仓库ID 默认是仓库名称，也可以显示为仓库编号
        /// </summary>
        public string ReferenceDisplayFieldName { get; set; } // 将要显示的名称字段名


        // 是否是自引用表
        public bool IsSelfReferencing { get; set; }
        //public List<KeyValuePair<object, string>> KeyValuePairList { get; set; } // 键值对集合

        public ReferenceKeyMapping(string referenceTableName, string referenceOriginalFieldName, string mappedTargetTableName, string mappedTargetFieldName = null, string referenceDefaultDisplayFieldName = null, string referenceTargetedDisplayFieldName = null)
        {
            ReferenceTableName = referenceTableName;
            MappedTargetTableName = mappedTargetTableName;
            ReferenceOriginalFieldName = referenceOriginalFieldName;
            MappedTargetFieldName = mappedTargetFieldName;
            if (string.IsNullOrEmpty(mappedTargetFieldName))
            {
                MappedTargetFieldName = ReferenceOriginalFieldName;
            }

        }

        //public ReferenceKeyMapping(string tableName, string keyFieldName, string valueFieldName = null, bool isSpecialField = false, string specialFieldName = null, bool isSelfReferencing = false)
        //{
        //    ReferenceTableName = tableName;
        //    KeyFieldName = keyFieldName;
        //    DisplayFieldName = valueFieldName;
        //    IsSpecialField = isSpecialField;
        //    SpecialFieldName = specialFieldName;
        //    IsSelfReferencing = isSelfReferencing;
        //}

    }


    /// <summary>
    /// 固定字典值映射类,比方单据主表中单据状态字段，一个审核状态有 提交、审核、结案等状态
    /// </summary>
    public class FixedDictionaryMapping
    {
        public string TableName { get; set; } // 表名
        public string KeyFieldName { get; set; } // 要显示字段名
        public List<KeyValuePair<object, string>> KeyValuePairList { get; set; } // 键值对集合

        public FixedDictionaryMapping(string tableName, string keyFieldName, List<KeyValuePair<object, string>> keyValuePairList)
        {
            TableName = tableName;
            KeyFieldName = keyFieldName;
            KeyValuePairList = keyValuePairList;
        }

    }
}

