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
using System.Runtime.InteropServices;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model.ReminderModel;

using RUINORERP.Business.Cache;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 所有表格显示为名称的代码都放到这里，还有一些没有重构完
    /// UI不直接调用。
    /// </summary>
    public class GridViewDisplayHelper
    {

        public GridViewDisplayHelper()
        {
            // 使用依赖注入容器获取IEntityCacheManager实例
            _cacheManager = Startup.GetFromFac<IEntityCacheManager>();
        }

        // 添加支持DI注入的构造函数
        public GridViewDisplayHelper(IEntityCacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        // 改为私有字段，并添加null检查的属性访问器
        private IEntityCacheManager _cacheManager;

        /// <summary>
        /// 获取缓存管理器实例，确保不为null
        /// </summary>
        protected IEntityCacheManager CacheManager
        {
            get
            {
                // 如果_cacheManager为null，尝试从DI容器获取
                if (_cacheManager == null)
                {
                    _cacheManager = Startup.GetFromFac<IEntityCacheManager>();
                }
                return _cacheManager;
            }
        }

        public HashSet<FixedDictionaryMapping> FixedDictionaryMappings { get; set; } = new HashSet<FixedDictionaryMapping>();

        /// <summary>
        ///  
        /// </summary>
        public HashSet<ReferenceKeyMapping> ReferenceKeyMappings { get; set; } = new HashSet<ReferenceKeyMapping>();


        // 用于存储列的显示类型
        public Dictionary<string, string> ColumnDisplayTypes { get; set; } = new Dictionary<string, string>();

        // 用于存储外键列的映射 
        public Dictionary<string, string> ReferenceKeyColumnMappings { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 用来保存表格显示是处理图片列，是否为内存图片，是否显示为缩略图
        /// </summary>
        public Dictionary<string, (bool IsByteFormat, bool UseThumbnail)> ImagesColumnsMappings { get; set; } = new Dictionary<string, (bool IsByteFormat, bool UseThumbnail)>();



        /// <summary>
        /// 用于存储外键表的列表信息
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<string, string>>> ReferenceTableList { get; set; } = new ConcurrentDictionary<string, List<KeyValuePair<string, string>>>();




        // 已在构造函数区域声明，此处删除重复声明

        /// <summary>
        /// 指定枚举类型
        /// </summary>
        /// <param name="_type"></param>
        public void InitializeFixedDictionaryMappings(Type _type)
        {
            // 动态检查类型是否包含指定的属性
            foreach (var prop in _type.GetProperties())
            {
                // 检查类型 是否包含 DataStatus 列
                if (prop.Name == nameof(DataStatus))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, prop.Name, CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus))));
                }
                else if (prop.Name == nameof(ApprovalStatus))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, prop.Name, CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus))));
                }

                // 检查类型 T 是否包含 PayStatus 列
                else if (prop.Name == nameof(PayStatus))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(PayStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(PayStatus))));
                }
                // 检查类型 T 是否包含 Priority 列
                else if (prop.Name == "OrderPriority" || prop.Name == "Priority")
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(Priority), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(Priority))));
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, "OrderPriority", Common.CommonHelper.Instance.GetKeyValuePairs(typeof(Priority))));
                }
                else if (prop.Name == nameof(Priority))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(Priority), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(Priority))));
                }
                // 检查类型 T 是否包含 PurReProcessWay 列
                else if (prop.Name == nameof(PurReProcessWay) || prop.Name == "ProcessWay")
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(PurReProcessWay), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(PurReProcessWay))));
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, "ProcessWay", Common.CommonHelper.Instance.GetKeyValuePairs(typeof(PurReProcessWay))));
                }
                else if (prop.Name == nameof(GoodsSource) || prop.Name == "SourceType") //应该字段名和枚举名相同才好
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(GoodsSource), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(GoodsSource))));
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, "SourceType", Common.CommonHelper.Instance.GetKeyValuePairs(typeof(GoodsSource))));
                }
                else if (prop.Name == nameof(BusinessPartnerType))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(BusinessPartnerType), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(BusinessPartnerType))));
                }
                else if (prop.Name == nameof(SettlementType))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(SettlementType), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(SettlementType))));
                }
                else if (prop.Name == nameof(NotifyChannel))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(NotifyChannel), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(NotifyChannel))));
                }
                else if (prop.Name == nameof(ARAPWriteOffStatus))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(ARAPWriteOffStatus), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ARAPWriteOffStatus))));
                }
                else if (prop.Name == nameof(ReminderBizType))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(ReminderBizType), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ReminderBizType))));
                }
                else if (prop.Name == nameof(RuleEngineType))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(RuleEngineType), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(RuleEngineType))));
                }
                else if (prop.Name == nameof(ReminderPriority))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(ReminderPriority), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ReminderPriority))));
                }
                else if (prop.Name == nameof(PlatformType))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(PlatformType), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(PlatformType))));
                }
                else if (prop.Name == nameof(RepairStatus))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(RepairStatus), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(RepairStatus))));
                }
                else if (prop.Name == nameof(RefundStatus))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(RefundStatus), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(RefundStatus))));
                }
                else if (prop.Name == nameof(StatementStatus))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(StatementStatus), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(StatementStatus))));
                }
                else if (prop.Name == nameof(PaymentStatus))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(PaymentStatus), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(PaymentStatus))));
                }
                else if (prop.Name == nameof(Adjust_Type))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(Adjust_Type), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(Adjust_Type))));
                }
                else if (prop.Name == nameof(CheckMode))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(CheckMode), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(CheckMode))));
                }

                else if (prop.Name == nameof(ASProcessStatus))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(ASProcessStatus), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ASProcessStatus))));
                }
                else if (prop.Name == nameof(ExpenseBearerType))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(ExpenseBearerType), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ExpenseBearerType))));
                }
                else if (prop.Name == nameof(ExpenseAllocationMode))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(ExpenseAllocationMode), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ExpenseAllocationMode))));
                }

                else if (prop.Name == nameof(PrePaymentStatus))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(PrePaymentStatus), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(PrePaymentStatus))));
                }
                else if (prop.Name == nameof(ARAPStatus))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(ARAPStatus), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ARAPStatus))));
                }
                else if (prop.Name == nameof(BizType) || prop.Name == "TargetBizType" || prop.Name == "SourceBizType")
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(BizType), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(BizType))));
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, "TargetBizType", Common.CommonHelper.Instance.GetKeyValuePairs(typeof(BizType))));
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, "SourceBizType", Common.CommonHelper.Instance.GetKeyValuePairs(typeof(BizType))));
                }
                else if (prop.Name == nameof(ReceivePaymentType))
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, nameof(ReceivePaymentType), Common.CommonHelper.Instance.GetKeyValuePairs(typeof(ReceivePaymentType))));
                }
                else if (prop.Name == "ApprovalResults")
                {
                    List<KeyValuePair<object, string>> ApprovalResultskvlist = new List<KeyValuePair<object, string>>();
                    ApprovalResultskvlist.Add(new KeyValuePair<object, string>(true, "同意"));
                    ApprovalResultskvlist.Add(new KeyValuePair<object, string>(false, "否决"));
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(_type.Name, "ApprovalResults", ApprovalResultskvlist));
                }

                List<KeyValuePair<object, string>> Genderkvlist = new List<KeyValuePair<object, string>>();
                Genderkvlist.Add(new KeyValuePair<object, string>(true, "男"));
                Genderkvlist.Add(new KeyValuePair<object, string>(false, "女"));
                Expression<Func<tb_Employee, bool?>> expr;
                expr = (p) => p.Gender;// == name;
                var mb = expr.GetMemberInfo();
                string colName = mb.Name;
                if (typeof(tb_Employee).Name == _type.Name && prop.Name == colName)
                {
                    FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(tb_Employee).Name, colName, Genderkvlist));
                }

                //List<KeyValuePair<object, string>> kvlist1 = new List<KeyValuePair<object, string>>();
                //kvlist1.Add(new KeyValuePair<object, string>(true, "是"));
                //kvlist1.Add(new KeyValuePair<object, string>(false, "否"));
                //System.Linq.Expressions.Expression<Func<tb_Employee, bool?>> expr2;
                //expr2 = (p) => p.Is_enabled;// == name;
                //string colName2 = expr2.GetMemberInfo().Name;
                //if (typeof(tb_Employee).Name == _type.Name && prop.Name == colName2)
                //{
                //    FixedDictionaryMappings.Add(new FixedDictionaryMapping(typeof(tb_Employee).Name, colName2, kvlist1));
                //}
            }
        }

        public void InitializeFixedDictionaryMappings<T>()
        {
            // 直接调用非泛型版本
            InitializeFixedDictionaryMappings(typeof(T));
        }


        public void InitializeReferenceKeyMapping(Type _type)
        {
            string tableName = _type.Name;
            if (!ReferenceTableList.ContainsKey(tableName))
            {
                List<KeyValuePair<string, string>> kvlist = new List<KeyValuePair<string, string>>();
                foreach (var field in _type.GetProperties())
                {
                    //获取指定类型的自定义特性
                    object[] attrs = field.GetCustomAttributes(false);
                    foreach (var attr in attrs)
                    {
                        if (attr is FKRelationAttribute)
                        {
                            FKRelationAttribute fkrattr = attr as FKRelationAttribute;

                            //TODO:特殊处理：因 为fkrattr.FKTableName 如果是tb_ProdDetail 换为 视图，产品表没有缓存
                            //// SetDictDataSource<tb_ProdDetail>(k => k.ProdDetailID, v => v.SKU);//这个不能缓存 在girdsetvalue时
                            //SetDictDataSource<View_ProdDetail>(k => k.ProdDetailID.Value, v => v.CNName);
                            if (fkrattr.FKTableName == "tb_ProdDetail")
                            {
                                fkrattr.FKTableName = "View_ProdDetail";
                            }

                            KeyValuePair<string, string> kv = new KeyValuePair<string, string>(fkrattr.FK_IDColName, fkrattr.FKTableName);
                            kvlist.Add(kv);

                            ReferenceKeyMapping mapping = new ReferenceKeyMapping(fkrattr.FKTableName, fkrattr.FK_IDColName, tableName);

                            // 使用依赖注入的缓存管理器
                            var schemaInfo = Startup.GetFromFac<ITableSchemaManager>().GetSchemaInfo(mapping.ReferenceTableName);
                            if (schemaInfo != null)
                            {
                                //要显示的默认值是从缓存表中获取的字段名，默认是主键ID字段对应的名称
                                mapping.ReferenceDefaultDisplayFieldName = schemaInfo.DisplayField;
                            }
                            AddReferenceKeyMapping(fkrattr.FK_IDColName, mapping);
                        }
                    }
                }
                if (kvlist.Count > 0)
                {
                    ReferenceTableList.TryAdd(tableName, kvlist);
                }
            }
        }

        public void InitializeReferenceKeyMapping<T>()
        {
            // 直接调用非泛型版本
            InitializeReferenceKeyMapping(typeof(T));
        }
        // 添加外键映射
        public void AddReferenceKeyMapping(string columnName, ReferenceKeyMapping mapping)
        {
            if (!ReferenceKeyMappings.Contains(mapping))
            {
                ReferenceKeyMappings.Add(mapping);
            }
        }

        public void AddFixedDictionaryMapping<T>(string columnName, List<KeyValuePair<object, string>> mappings)
        {

        }

        /// <summary>
        /// 添加指定表名、字段名和枚举类型的固定字典映射
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">字段名</param>
        /// <param name="enumType">枚举类型</param>
        /// <exception cref="ArgumentException">当提供的类型不是枚举类型时抛出</exception>
        public void AddFixedDictionaryMapping(string tableName, string columnName, Type enumType)
        {
            if (enumType == null || !enumType.IsEnum)
            {
                throw new ArgumentException("The provided type must be an enum type.", nameof(enumType));
            }
            
            List<KeyValuePair<object, string>> keyValuePairs = Common.CommonHelper.Instance.GetKeyValuePairs(enumType);
            FixedDictionaryMappings.Add(new FixedDictionaryMapping(tableName, columnName, keyValuePairs));
        }

        /// <summary>
        /// 使用泛型和表达式树添加固定字典映射
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="columnExpression">字段表达式</param>
        /// <param name="enumType">枚举类型</param>
        /// <exception cref="ArgumentException">当提供的类型不是枚举类型时抛出</exception>
        public void AddFixedDictionaryMapping<TEntity>(Expression<Func<TEntity, object>> columnExpression, Type enumType)
        {
            if (enumType == null || !enumType.IsEnum)
            {
                throw new ArgumentException("The provided type must be an enum type.", nameof(enumType));
            }
            
            MemberInfo memberInfo = columnExpression.GetMemberInfo();
            string tableName = typeof(TEntity).Name;
            string columnName = memberInfo.Name;
            
            List<KeyValuePair<object, string>> keyValuePairs = Common.CommonHelper.Instance.GetKeyValuePairs(enumType);
            FixedDictionaryMappings.Add(new FixedDictionaryMapping(tableName, columnName, keyValuePairs));
        }



        public string GetGridViewDisplayText(string tableName, string columnName, object value)
        {
            string DisplayText = string.Empty;
            #region 
            // 处理固定字典值
            var fixedMapping = FixedDictionaryMappings.FirstOrDefault(t => t.TableName == tableName && t.KeyFieldName == columnName);
            if (fixedMapping != null)
            {
                List<KeyValuePair<object, string>> mappings = fixedMapping.KeyValuePairList;
                KeyValuePair<object, string> matchedPair = mappings.FirstOrDefault(pair => pair.Key.ToString().ToLower() == value.ToString().ToLower());
                if (matchedPair.Value != null)
                {
                    DisplayText = matchedPair.Value;
                    return DisplayText;
                }
            }

            // 处理外键映射
            string displayName = GetDisplayNameByReferenceKeyMappings(tableName, columnName, value);
            if (!string.IsNullOrEmpty(displayName))
            {
                DisplayText = displayName;
                return DisplayText;
            }

            #endregion
            return DisplayText;
        }


        // 获取显示名称
        public string GetDisplayNameByReferenceKeyMappings(string TargetTableName, string idColName, object IdValue)
        {
            try
            {
                // 如果值为空，直接返回空字符串
                if (IdValue == null)
                {
                    return string.Empty;
                }

                // 如果值是 decimal 类型，直接返回
                if (IdValue.GetType().Name == "Decimal")
                {
                    return IdValue.ToString();
                }

                // 如果值不是 long 类型，直接返回
                if (IdValue.GetType().Name != "Int64")
                {
                    return IdValue.ToString();
                }

                // 特殊字段处理（如创建人、修改人）
                #region
                //如果是修改人创建人统一处理,并且要放在前面
                //定义两个字段为了怕后面修改，不使用字符串
                Expression<Func<tb_Employee, long>> Created_by;
                Expression<Func<tb_Employee, long>> Modified_by;
                Expression<Func<tb_Employee, long>> Approver_by;

                Approver_by = c => c.Employee_ID;
                Created_by = c => c.Created_by.Value;
                Modified_by = c => c.Modified_by.Value;

                if (idColName == Created_by.GetMemberInfo().Name || idColName == Modified_by.GetMemberInfo().Name || idColName == "Approver_by")
                {
                    if (IdValue.ToString() == "0")
                    {
                        return "";
                    }
                    string SourceTableName = typeof(tb_Employee).Name;
                    // 使用静态缓存管理器方法获取显示值
                    object objText = CacheManager?.GetDisplayValue(SourceTableName, IdValue);

                    if (objText != null && objText.ToString() != "System.Object")
                    {
                        return objText.ToString();
                    }
                }
                #endregion

                ReferenceKeyMapping mapping = ReferenceKeyMappings.Where(c => c.MappedTargetTableName == TargetTableName && c.MappedTargetFieldName == idColName).FirstOrDefault();
                if (mapping != null)
                {
                    if (mapping.MappedTargetFieldName == idColName)
                    {
                        if (!string.IsNullOrEmpty(mapping.CustomDisplayColumnName))
                        {
                            // 使用静态缓存管理器方法获取显示值
                            object displayObj = CacheManager?.GetEntity(mapping.ReferenceTableName, IdValue);
                            if (displayObj != null && displayObj.GetPropertyValue(mapping.CustomDisplayColumnName) != null)
                            {
                                return displayObj.GetPropertyValue(mapping.CustomDisplayColumnName).ToString();
                            }
                        }
                        else
                        {
                            // 使用静态缓存管理器方法获取显示值
                            object displayValue = CacheManager?.GetDisplayValue(mapping.ReferenceTableName, IdValue);
                            if (displayValue != null)
                            {
                                return displayValue.ToString();
                            }
                        }

                    }
                }
                else
                {
                    #region 没有映射的情况
                    // 使用静态缓存管理器方法获取显示值
                    var schemaInfo = Startup.GetFromFac<ITableSchemaManager>().GetSchemaInfo(TargetTableName);
                    // 先尝试直接从目标表获取
                    object nameValue = CacheManager?.GetDisplayValue(TargetTableName, IdValue);
                    if (nameValue != null && !string.IsNullOrWhiteSpace(nameValue.ToString()))
                    {
                        return nameValue.ToString();
                    }

                    // 再尝试外键关联
                    //if (cacheManager.FkPairTableList.TryGetValue(TargetTableName, out List<KeyValuePair<string, string>> kvlist))
                    //{
                    //    var kv = kvlist.Find(k => k.Key == idColName);
                    //    if (kv.Key != null)
                    //    {
                    //        string baseTableName = kv.Value;
                    //        nameValue = cacheManager.GetDisplayValue(baseTableName, IdValue);
                    //        if (nameValue != null)
                    //        {
                    //            return nameValue.ToString();
                    //        }
                    //    }
                    //}
                    #endregion
                }

            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            return IdValue.ToString();
        }

    }
}