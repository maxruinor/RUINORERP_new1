using Microsoft.Extensions.Logging;
using RUINORERP.UI.UControls;
using RUINORERP.UI.UCSourceGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{


    // 通用比较结果模型
    public class ComparisonResult<T> where T : class
    {
        public List<T> AddedItems { get; set; } = new List<T>();
        public List<T> RemovedItems { get; set; } = new List<T>();
        public List<ModifiedItem<T>> ModifiedItems { get; set; } = new List<ModifiedItem<T>>();

        public List<ReorderedItem<T>> ReorderedItems { get; set; } = new List<ReorderedItem<T>>();
        public bool HasDifferences => AddedItems.Any() || RemovedItems.Any() || ModifiedItems.Any() || ReorderedItems.Any();
    }

    // 修改项信息
    public class ModifiedItem<T> where T : class
    {
        public T OriginalItem { get; set; }
        public T NewItem { get; set; }
        public List<PropertyDifference> PropertyDifferences { get; set; } = new List<PropertyDifference>();
    }
    // 重排序项信息
    public class ReorderedItem<T> where T : class
    {
        public T Item { get; set; }
        public int OldIndex { get; set; }
        public int NewIndex { get; set; }
    }
    // 通用对象比较器
    public static class GenericComparer
    {
        // 比较两个集合的差异，包括顺序变化
        /// <summary>
        /// 比较过程如果出错。默认返回true表示有不同
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldItems"></param>
        /// <param name="newItems"></param>
        /// <param name="keySelector"></param>
        /// <param name="propertiesToCompare"></param>
        /// <returns></returns>
        public static ComparisonResult<T> CompareCollections<T>(
            List<T> oldItems,
            List<T> newItems,
            Func<T, string> keySelector,
            params string[] propertiesToCompare) where T : class
        {
            var result = new ComparisonResult<T>();
            try
            {
                #region 比较
                if (oldItems == null) oldItems = new List<T>();
                if (newItems == null) newItems = new List<T>();

                // 使用字典加速查找，同时记录原始索引位置
                var oldLookup = oldItems
                    .Select((item, index) => new { Item = item, Index = index })
                    .ToDictionary(x => keySelector(x.Item), x => new { x.Item, x.Index });

                var newLookup = newItems
                    .Select((item, index) => new { Item = item, Index = index })
                    .ToDictionary(x => keySelector(x.Item), x => new { x.Item, x.Index });

                // 查找新增的项
                result.AddedItems = newItems
                    .Where(item => !oldLookup.ContainsKey(keySelector(item)))
                    .ToList();

                // 查找已删除的项
                result.RemovedItems = oldItems
                    .Where(item => !newLookup.ContainsKey(keySelector(item)))
                    .ToList();

                // 用于记录已处理的新项索引，避免重复检测顺序变化
                var processedNewIndices = new HashSet<int>();

                // 查找修改的项和顺序变化
                foreach (var oldItemPair in oldLookup)
                {
                    var oldKey = oldItemPair.Key;
                    var oldItemInfo = oldItemPair.Value;

                    if (newLookup.TryGetValue(oldKey, out var newItemInfo))
                    {
                        // 比较属性差异
                        var differences = CompareObjectProperties(oldItemInfo.Item, newItemInfo.Item, propertiesToCompare);
                        if (differences.Any())
                        {
                            result.ModifiedItems.Add(new ModifiedItem<T>
                            {
                                OriginalItem = oldItemInfo.Item,
                                NewItem = newItemInfo.Item,
                                PropertyDifferences = differences
                            });
                        }

                        // 比较位置差异
                        if (oldItemInfo.Index != newItemInfo.Index)
                        {
                            result.ReorderedItems.Add(new ReorderedItem<T>
                            {
                                Item = oldItemInfo.Item,
                                OldIndex = oldItemInfo.Index,
                                NewIndex = newItemInfo.Index
                            });
                        }

                        processedNewIndices.Add(newItemInfo.Index);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex.Message, ex);
            }
            return result;
        }

        // 使用反射比较对象属性
        private static List<PropertyDifference> CompareObjectProperties<T>(
            T oldObj,
            T newObj,
            string[] propertiesToCompare) where T : class
        {
            var differences = new List<PropertyDifference>();
            var type = typeof(T);

            // 如果没有指定要比较的属性，则比较所有公共属性
            if (propertiesToCompare == null || propertiesToCompare.Length == 0)
            {
                propertiesToCompare = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(p => p.Name)
                    .ToArray();
            }

            foreach (var propertyName in propertiesToCompare)
            {
                var property = type.GetProperty(propertyName);
                if (property == null) continue;

                var oldValue = property.GetValue(oldObj);
                var newValue = property.GetValue(newObj);

                if (!Equals(oldValue, newValue))
                {
                    differences.Add(new PropertyDifference
                    {
                        PropertyName = propertyName,
                        OldValue = oldValue?.ToString() ?? "null",
                        NewValue = newValue?.ToString() ?? "null"
                    });
                }
            }

            return differences;
        }


    }
    // 列比较器扩展 - 提供针对特定类型的比较方法
    public static class ColumnComparerExtensions
    {
        // 比较 ColDisplayController 类型的列
        public static ComparisonResult<ColDisplayController> CompareColumns(
            this List<ColDisplayController> oldColumns,
            List<ColDisplayController> newColumns)
        {
            return GenericComparer.CompareCollections(
                oldColumns,
                newColumns,
                c => c.ColName,//这里是指定比较的标准。不能重复值的列名
                nameof(ColDisplayController.ColDisplayText),
                nameof(ColDisplayController.IsFixed),
                nameof(ColDisplayController.ColDisplayIndex),
                nameof(ColDisplayController.ColWidth),
                nameof(ColDisplayController.Visible),
                nameof(ColDisplayController.Disable),
                nameof(ColDisplayController.IsPrimaryKey)
            );
        }

        // 比较 SGColDisplayHandler 类型的列
        public static ComparisonResult<SGColDisplayHandler> CompareColumns(
            this List<SGColDisplayHandler> oldColumns,
            List<SGColDisplayHandler> newColumns)
        {
            return GenericComparer.CompareCollections(
                oldColumns,
                newColumns,
                //c => c.ColName,
                c => c.CompositeKey, // 修改这里，使用UniqueId作为键
                nameof(SGColDisplayHandler.ColCaption),
                nameof(SGColDisplayHandler.IsFixed),
                nameof(SGColDisplayHandler.ColWidth),
                nameof(SGColDisplayHandler.Visible),
                nameof(SGColDisplayHandler.Disable),
                nameof(SGColDisplayHandler.ColDisplayIndex)
            );
        }
    }

    // 比较结果模型
    public class ColumnComparisonResult
    {
        public List<ColDisplayController> AddedColumns { get; set; } = new List<ColDisplayController>();
        public List<ColDisplayController> RemovedColumns { get; set; } = new List<ColDisplayController>();
        public List<ModifiedColumn> ModifiedColumns { get; set; } = new List<ModifiedColumn>();

        // 检查是否有差异
        public bool HasDifferences => AddedColumns.Any() || RemovedColumns.Any() || ModifiedColumns.Any();
    }

    // 列修改信息
    public class ModifiedColumn
    {
        public string ColumnName { get; set; }
        public List<PropertyDifference> PropertyDifferences { get; set; } = new List<PropertyDifference>();
    }

    // 属性差异信息
    public class PropertyDifference
    {
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
