using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.UCSourceGrid
{
    public static class CollectionExtensions
    {
        public static void SyncOrderByUniqueId<TMaster, TSlave>(
            this List<TMaster> masterList,
            List<TSlave> slaveList,
            Func<TMaster, string> masterKeySelector,
            Func<TSlave, string> slaveKeySelector,
            Func<TMaster, bool> isSpecialFirstItem = null,
            Func<TMaster, bool> isSpecialSecondItem = null)
            where TMaster : class
            where TSlave : class
        {
            if (masterList == null || slaveList == null) return;

            // 创建字典用于快速查找
            var masterDict = masterList.ToDictionary(masterKeySelector, x => x);

            // 提取特殊项
            var specialFirstItems = new List<TMaster>();
            var specialSecondItems = new List<TMaster>();
            var normalItems = new List<TMaster>();

            foreach (var item in masterList)
            {
                if (isSpecialFirstItem != null && isSpecialFirstItem(item))
                {
                    specialFirstItems.Add(item);
                    masterDict.Remove(masterKeySelector(item));
                }
                else if (isSpecialSecondItem != null && isSpecialSecondItem(item))
                {
                    specialSecondItems.Add(item);
                    masterDict.Remove(masterKeySelector(item));
                }
            }

            // 按照 slaveList 的顺序处理剩余项
            var orderedList = new List<TMaster>();
            foreach (var slaveItem in slaveList)
            {
                var key = slaveKeySelector(slaveItem);
                if (masterDict.TryGetValue(key, out var masterItem))
                {
                    orderedList.Add(masterItem);
                    masterDict.Remove(key);
                }
            }

            // 合并所有项：特殊项 + 按slave排序的项 + 剩余的项
            masterList.Clear();
            masterList.AddRange(specialFirstItems);    // 特殊项排第一
            masterList.AddRange(specialSecondItems);   // 特殊项排第二
            masterList.AddRange(orderedList);          // 按slave顺序的项
            masterList.AddRange(masterDict.Values);    // 剩余的项
        }

        public static void SyncOrderByUniqueId<TMaster, TSlave>(
            this List<TMaster> masterList,
            List<TSlave> slaveList,
            Func<TMaster, string> masterKeySelector,
            Func<TSlave, string> slaveKeySelector)
            where TMaster : class
            where TSlave : class
        {
            if (masterList == null || slaveList == null) return;

            // 创建字典用于快速查找
            var masterDict = masterList.ToDictionary(masterKeySelector, x => x);

            // 创建新的有序列表
            var orderedList = new List<TMaster>();

            // 按照 slaveList 的顺序处理
            foreach (var slaveItem in slaveList)
            {
                var key = slaveKeySelector(slaveItem);
                if (masterDict.TryGetValue(key, out var masterItem))
                {
                    orderedList.Add(masterItem);
                    masterDict.Remove(key);
                }
            }

            // 添加剩余的项
            orderedList.AddRange(masterDict.Values);

            // 更新原始集合
            masterList.Clear();
            masterList.AddRange(orderedList);
        }
    }


 
    

}
