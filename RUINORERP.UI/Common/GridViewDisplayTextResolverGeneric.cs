using Autofac;
using AutoMapper;
using FastReport.DevComponents.DotNetBar;
using FastReport.Table;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Workspace;
using Microsoft.Extensions.Logging;
using Netron.NetronLight;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ObjectsComparer;
using OfficeOpenXml;
using RUINOR.Core;
using RUINOR.WinFormsUI.CustomPictureBox;
using RUINORERP.Business;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.CustomAttribute;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global.Model;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.Models;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.BI;
using RUINORERP.UI.Common;
using RUINORERP.UI.CommonUI;
using RUINORERP.UI.FormProperty;
using RUINORERP.UI.UControls;
using RUINORERP.UI.UserCenter;
using RUINORERP.UI.UserPersonalized;
using SixLabors.ImageSharp.Memory;
using SourceGrid.Cells.Models;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using Winista.Text.HtmlParser.Lex;
using Control = System.Windows.Forms.Control;


namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 用于解析DataGridView显示ID列时，转换为对应名称的工具类
    /// 泛型版本
    /// </summary>
    public class GridViewDisplayTextResolverGeneric<T> : AbstractGridViewDisplayTextResolver where T : class
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GridViewDisplayTextResolverGeneric() : base(typeof(T))
        {
        }

        // 初始化方法
        public void Initialize(DataGridView dataGridView)
        {
            // 初始化固定字典映射
            displayHelper.InitializeFixedDictionaryMappings<T>();
            displayHelper.InitializeReferenceKeyMapping<T>();
            dataGridView.CellFormatting += DataGridView_CellFormatting;
        }

        /// <summary>
        /// 添加枚举类型的固定字典映射，目标表为T
        /// </summary>
        /// <param name="TargetField">目标字段表达式</param>
        /// <param name="enumType">枚举类型</param>
        public void AddFixedDictionaryMappingByEnum(Expression<Func<T, object>> TargetField, Type enumType)
        {
            // 使用displayHelper的AddFixedDictionaryMapping方法添加映射
            displayHelper.AddFixedDictionaryMapping(TargetField, enumType);
        }

        // 单元格格式化事件处理
        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            string oldValue = e.Value?.ToString();
            if (e.FormattingApplied)
            {
                return;
            }
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
            if (HandleImageDisplay(e, columnName))
            {
                return;
            }

            e.Value = GetGridViewDisplayText(typeof(T).Name, columnName, e.Value);
            if (!e.Value.Equals(oldValue))
            {
                e.FormattingApplied = true;
            }
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

        /// <summary>
        /// 重写Equals方法，基于关键字段判断相等性
        /// </summary>
        /// <param name="obj">要比较的对象</param>
        /// <returns>如果对象相等返回true，否则返回false</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ReferenceKeyMapping other = (ReferenceKeyMapping)obj;
            return string.Equals(ReferenceTableName, other.ReferenceTableName, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(ReferenceOriginalFieldName, other.ReferenceOriginalFieldName, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(MappedTargetTableName, other.MappedTargetTableName, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(MappedTargetFieldName, other.MappedTargetFieldName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 重写GetHashCode方法，基于关键字段生成哈希码
        /// </summary>
        /// <returns>哈希码</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (ReferenceTableName?.ToLowerInvariant().GetHashCode() ?? 0);
                hash = hash * 23 + (ReferenceOriginalFieldName?.ToLowerInvariant().GetHashCode() ?? 0);
                hash = hash * 23 + (MappedTargetTableName?.ToLowerInvariant().GetHashCode() ?? 0);
                hash = hash * 23 + (MappedTargetFieldName?.ToLowerInvariant().GetHashCode() ?? 0);
                return hash;
            }
        }



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

