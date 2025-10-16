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

using RUINORERP.UI.SuperSocketClient;
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
using Fireasy.Common.Extensions;
using RUINORERP.Global;
using FastReport.Table;
using Newtonsoft.Json.Linq;
using System.Collections;
using Org.BouncyCastle.Asn1.X509;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Business.Cache;


namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 用于解析DataGridView显示ID列时，转换为对应名称的工具类
    /// 用于视图分析
    /// </summary>
    public class GridViewDisplayTextResolver
    {
        GridViewDisplayHelper displayHelper = new GridViewDisplayHelper();
        private Type _type;
        private HashSet<ReferenceKeyMapping> ReferenceKeyMappings { get; set; } = new HashSet<ReferenceKeyMapping>();
        private static IList _relatedTableTypesCache;
        public GridViewDisplayTextResolver(Type type)
        {
            _type = type;
            displayHelper.InitializeFixedDictionaryMappings(_type);
            displayHelper.InitializeReferenceKeyMapping(_type);
        }

        #region 缓存相关显示外键的类型
        private static readonly ConcurrentDictionary<Type, List<Type>> relatedTableCache = new ConcurrentDictionary<Type, List<Type>>();

        public static List<Type> GetRelatedTableTypes(Type type)
        {
            return relatedTableCache.GetOrAdd(type, t =>
            {
                BaseViewEntity instance = (BaseViewEntity)Activator.CreateInstance(t);
                instance.InitRelatedTableTypes();
                List<Type> RelatedTableTypes = instance.InstanceRelatedTableTypes;
                // Using new cache manager, no need to manually set foreign key column list
                foreach (var item in RelatedTableTypes)
                {
                    // Cache initialization happens automatically with the new system
                }
                return RelatedTableTypes;
            });
        }
        #endregion

        // 用于存储固定字典值的映射
        //private Dictionary<string, List<KeyValuePair<object, string>>> FixedDictionaryMappings { get; set; } = new Dictionary<string, List<KeyValuePair<object, string>>>();
        private HashSet<FixedDictionaryMapping> FixedDictionaryMappings { get; set; } = new HashSet<FixedDictionaryMapping>();

        // 用于存储列的显示类型
        private Dictionary<string, string> ColumnDisplayTypes { get; set; } = new Dictionary<string, string>();

        // 用于存储外键列的映射
        private Dictionary<string, string> ReferenceKeyColumnMappings { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 用于存储外键表的列表信息
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<string, string>>> ReferenceTableList { get; set; } = new ConcurrentDictionary<string, List<KeyValuePair<string, string>>>();





        // 初始化方法
        public void Initialize(DataGridView dataGridView)
        {
            // 初始化固定字典映射
            displayHelper.InitializeFixedDictionaryMappings(_type);
            displayHelper.InitializeReferenceKeyMapping(_type);
            dataGridView.CellFormatting += DataGridView_CellFormatting;
        }



        // 添加外键映射
        public void AddReferenceKeyMapping(string columnName, ReferenceKeyMapping mapping)
        {
            if (!ReferenceKeyMappings.Contains(mapping))
            {
                ReferenceKeyMappings.Add(mapping);
            }
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

            if (ReferenceKeyMappings == null)
            {
                ReferenceKeyMappings = new HashSet<ReferenceKeyMapping>();
            }
            ReferenceKeyMapping mapping = new ReferenceKeyMapping(typeof(source).Name, Sourceinfo.Name, typeof(target).Name, Targetinfo.Name);
            if (typeof(source).Name == typeof(target).Name)
            {
                mapping.IsSelfReferencing = true;
            }
            //只处理需要缓存的表
            // Using new cache manager, no need to check table in cache
            {
                KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                //要显示的默认值是从缓存表中获取的字段名，默认是主键ID字段对应的名称
                mapping.ReferenceDefaultDisplayFieldName = pair.Value;
            }
            if (CustomDisplaySourceField != null)
            {
                MemberInfo CustomDisplayColInfo = CustomDisplaySourceField.GetMemberInfo();
                mapping.CustomDisplayColumnName = CustomDisplayColInfo.Name;
            }
            //以目标为主键，原始的相同的只能为值
            ReferenceKeyMappings.Add(mapping);
        }


        // 添加固定字典映射
        public void AddFixedDictionaryMapping(string tableName, string columnName, List<KeyValuePair<object, string>> mappings)
        {
            if (_type.GetProperty(tableName) != null)
            {
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, columnName, CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus))));
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(ApprovalStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus))));
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
            FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(target).Name, Targetinfo.Name, CommonHelper.Instance.GetKeyValuePairs(enumType)));
        }


        // 添加列显示类型
        public void AddColumnDisplayType(string columnName, string displayType)
        {
            ColumnDisplayTypes[columnName] = displayType;
        }

        // 添加外键列映射
        public void AddReferenceKeyColumnMapping(string columnName, string foreignKeyColumnName)
        {
            ReferenceKeyColumnMappings[columnName] = foreignKeyColumnName;
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
            if (ColumnDisplayTypes.ContainsKey(columnName))
            {
                string displayType = ColumnDisplayTypes[columnName];
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

            if (_type.Name.Contains("View_") || _type.Name.Contains("Proc_"))
            {
                //视图优先添加本身 subsequent to 关联表
                List<Type> relatedTableTypes = new List<Type>();
                relatedTableTypes.Add(_type);
                //BaseViewEntity baseView = (BaseViewEntity)Activator.CreateInstance(_type);
                relatedTableTypes.AddRange(GetRelatedTableTypes(_type));
                foreach (var item in relatedTableTypes)
                {
                    if (item.Name.Contains("tb_"))
                    {
                        // Cache initialization happens automatically with the new system
                    }
                    string displayName = displayHelper.GetGridViewDisplayText(item.Name, columnName, e.Value);
                    if (!string.IsNullOrEmpty(displayName))
                    {
                        e.Value = displayName;
                        return;
                    }
                }
            }
            else
            {
                // 处理外键映射
                e.Value = displayHelper.GetGridViewDisplayText(_type.Name, columnName, e.Value);
                return;
            }

        }



    }

    // 外键映射类 多种情况 
    //普通映射 一个表对应一个主键字段 一个显示名称字段
    //比方菜单表中，引用了模块用的ID，如果ID名和模块名一样时。是一种情况。如果不一样，则要指定别名。


}

