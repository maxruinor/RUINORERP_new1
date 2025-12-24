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
using System.Collections;
using Org.BouncyCastle.Asn1.X509;

using RUINORERP.Business.Cache;


namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 用于解析DataGridView显示ID列时，转换为对应名称的工具类
    /// 用于视图分析
    /// </summary>
    public class GridViewDisplayTextResolver : AbstractGridViewDisplayTextResolver
    {
        private static IList _relatedTableTypesCache;
        
        public GridViewDisplayTextResolver(Type type) : base(type)
        {
            displayHelper.InitializeFixedDictionaryMappings(_entityType);
            displayHelper.InitializeReferenceKeyMapping(_entityType);
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

        // 初始化方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataGridView">DataGridView控件</param>
        /// <param name="ColDisplayTypes">外部指定类型</param>
        public void Initialize(DataGridView dataGridView, params Type[] ColDisplayTypes)
        {
            // 初始化固定字典映射
            displayHelper.InitializeFixedDictionaryMappings(_entityType);
            displayHelper.InitializeReferenceKeyMapping(_entityType);
            if (ColDisplayTypes != null)
            {
                //视图统计时指定的表，根据这个表去找外键
                foreach (var ColDisplayType in ColDisplayTypes)
                {
                    displayHelper.InitializeReferenceKeyMapping(ColDisplayType);
                }
              
            }
            dataGridView.CellFormatting += DataGridView_CellFormatting;
        }

        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <param name="ColDisplayTypes">外部指定类型</param>
        public void Initialize( Type[] ColDisplayTypes)
        {
            if (ColDisplayTypes != null)
            {
                //视图统计时指定的表，根据这个表去找外键
                foreach (var ColDisplayType in ColDisplayTypes)
                {
                    displayHelper.InitializeReferenceKeyMapping(ColDisplayType);
                }
            }
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
            if (HandleImageDisplay(e, columnName))
            {
                return;
            }

            if (_entityType.Name.Contains("View_") || _entityType.Name.Contains("Proc_"))
            {
                //视图优先添加本身 subsequent to 关联表
                List<Type> relatedTableTypes = new List<Type>();
                relatedTableTypes.Add(_entityType);
                relatedTableTypes.AddRange(GetRelatedTableTypes(_entityType));
                foreach (var item in relatedTableTypes)
                {
                    string displayName = GetGridViewDisplayText(item.Name, columnName, e.Value);
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
                e.Value = GetGridViewDisplayText(_entityType.Name, columnName, e.Value);
                return;
            }
        }



    }

    // 外键映射类 多种情况 
    //普通映射 一个表对应一个主键字段 一个显示名称字段
    //比方菜单表中，引用了模块用的ID，如果ID名和模块名一样时。是一种情况。如果不一样，则要指定别名。


}

