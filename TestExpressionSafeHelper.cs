using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using RUINORERP.UI.Common;

namespace TestExpressionSafeHelper
{
    /// <summary>
    /// 测试ExpressionSafeHelper的缓存修复
    /// </summary>
    public class TestCacheFix
    {
        /// <summary>
        /// 测试闭包变量值变化时的缓存行为
        /// </summary>
        public static void TestClosureVariableCache()
        {
            // 创建测试数据
            var testData = new List<TestEntity>
            {
                new TestEntity { ID = 1, CustomerVendor_ID = 1742825427103256576L, Name = "Test1" },
                new TestEntity { ID = 2, CustomerVendor_ID = 1742825427103256577L, Name = "Test2" },
                new TestEntity { ID = 3, CustomerVendor_ID = 1742825427103256578L, Name = "Test3" }
            };

            // 第一次筛选，使用第一个ID值
            long firstId = 1742825427103256576L;
            Expression<Func<TestEntity, bool>> firstExpression = t => t.CustomerVendor_ID == Convert.ToInt64(firstId);
            var firstResult = ExpressionSafeHelper.SafeFilterList(testData, firstExpression);
            Console.WriteLine($"第一次筛选 (ID={firstId}): 找到 {firstResult.Count} 条记录");

            // 第二次筛选，使用不同的ID值
            long secondId = 1742825427103256577L;
            Expression<Func<TestEntity, bool>> secondExpression = t => t.CustomerVendor_ID == Convert.ToInt64(secondId);
            var secondResult = ExpressionSafeHelper.SafeFilterList(testData, secondExpression);
            Console.WriteLine($"第二次筛选 (ID={secondId}): 找到 {secondResult.Count} 条记录");

            // 验证结果
            if (firstResult.Count == 1 && firstResult[0].CustomerVendor_ID == firstId &&
                secondResult.Count == 1 && secondResult[0].CustomerVendor_ID == secondId)
            {
                Console.WriteLine("测试通过：闭包变量值变化时缓存正确工作");
            }
            else
            {
                Console.WriteLine("测试失败：缓存可能存在问题");
                Console.WriteLine($"第一次结果: {string.Join(", ", firstResult.Select(r => r.CustomerVendor_ID))}");
                Console.WriteLine($"第二次结果: {string.Join(", ", secondResult.Select(r => r.CustomerVendor_ID))}");
            }
        }
    }

    /// <summary>
    /// 测试实体类
    /// </summary>
    public class TestEntity
    {
        public long ID { get; set; }
        public long CustomerVendor_ID { get; set; }
        public string Name { get; set; }
    }
}