using System;
using System.Diagnostics;
using RUINORERP.Common.Helper;

namespace EntityStateProtectorTests
{
    /// <summary>
    /// 实体状态保护器 - 克隆完整性测试
    /// </summary>
    class CloneIntegrityTest
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 实体状态保护器 - 克隆完整性测试 ===\n");

            // 测试1：简单实体克隆
            TestSimpleEntityClone();

            // 测试2：含集合的实体克隆
            TestEntityWithCollectionClone();

            // 测试3：验证快照独立性
            TestSnapshotIndependence();

            Console.WriteLine("\n=== 所有测试完成 ===");
            Console.ReadKey();
        }

        /// <summary>
        /// 测试1：简单实体克隆
        /// </summary>
        static void TestSimpleEntityClone()
        {
            Console.WriteLine("测试1：简单实体克隆");
            Console.WriteLine("-------------------");

            var original = new TestSimpleEntity
            {
                ID = 1,
                Name = "测试订单",
                DataStatus = 2,
                ApprovalStatus = 1,
                Amount = 1000.50m,
                CreateTime = DateTime.Now
            };

            var stopwatch = Stopwatch.StartNew();
            var snapshot = CloneHelper.DeepCloneObject(original);
            stopwatch.Stop();

            Console.WriteLine($"克隆耗时: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"原实体 ID: {original.ID}, Name: {original.Name}, DataStatus: {original.DataStatus}");
            Console.WriteLine($"快照   ID: {snapshot.ID}, Name: {snapshot.Name}, DataStatus: {snapshot.DataStatus}");

            // 验证克隆是否正确
            bool passed = (original.ID == snapshot.ID &&
                          original.Name == snapshot.Name &&
                          original.DataStatus == snapshot.DataStatus &&
                          original.ApprovalStatus == snapshot.ApprovalStatus &&
                          original.Amount == snapshot.Amount);

            Console.WriteLine($"测试结果: {(passed ? "✅ 通过" : "❌ 失败")}\n");
        }

        /// <summary>
        /// 测试2：含集合的实体克隆
        /// </summary>
        static void TestEntityWithCollectionClone()
        {
            Console.WriteLine("测试2：含集合的实体克隆");
            Console.WriteLine("----------------------");

            var original = new TestEntityWithDetails
            {
                ID = 1,
                OrderNo = "SO001",
                DataStatus = 2
            };

            // 添加子项
            original.Details.Add(new TestOrderDetail { ID = 1, ProductName = "产品A", Qty = 10 });
            original.Details.Add(new TestOrderDetail { ID = 2, ProductName = "产品B", Qty = 20 });

            var stopwatch = Stopwatch.StartNew();
            var snapshot = CloneHelper.DeepCloneObject(original);
            stopwatch.Stop();

            Console.WriteLine($"克隆耗时: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"原实体子项数量: {original.Details.Count}");
            Console.WriteLine($"快照子项数量: {snapshot.Details.Count}");

            // 验证集合是否正确克隆
            bool collectionCloned = (original.Details.Count == snapshot.Details.Count);

            if (collectionCloned)
            {
                for (int i = 0; i < original.Details.Count; i++)
                {
                    var origDetail = original.Details[i];
                    var snapDetail = snapshot.Details[i];
                    Console.WriteLine($"  子项{i + 1}: 原=[ID={origDetail.ID}, Qty={origDetail.Qty}], 快照=[ID={snapDetail.ID}, Qty={snapDetail.Qty}]");
                }
            }

            Console.WriteLine($"测试结果: {(collectionCloned ? "✅ 通过" : "❌ 失败")}\n");
        }

        /// <summary>
        /// 测试3：验证快照独立性（修改原实体不影响快照）
        /// </summary>
        static void TestSnapshotIndependence()
        {
            Console.WriteLine("测试3：验证快照独立性");
            Console.WriteLine("--------------------");

            var original = new TestEntityWithDetails
            {
                ID = 1,
                OrderNo = "SO001",
                DataStatus = 2
            };
            original.Details.Add(new TestOrderDetail { ID = 1, ProductName = "产品A", Qty = 10 });

            // 创建快照
            var snapshot = CloneHelper.DeepCloneObject(original);

            // 修改原实体
            original.DataStatus = 5;
            original.Details[0].Qty = 999;

            Console.WriteLine($"修改后 - 原实体 DataStatus: {original.DataStatus}, 子项Qty: {original.Details[0].Qty}");
            Console.WriteLine($"修改后 - 快照   DataStatus: {snapshot.DataStatus}, 子项Qty: {snapshot.Details[0].Qty}");

            // 验证快照是否保持原始值
            bool independent = (snapshot.DataStatus == 2 && snapshot.Details[0].Qty == 10);

            Console.WriteLine($"测试结果: {(independent ? "✅ 通过 - 快照独立" : "❌ 失败 - 快照被影响")}\n");
        }
    }

    #region 测试实体类

    /// <summary>
    /// 简单测试实体
    /// </summary>
    [Serializable]
    public class TestSimpleEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int DataStatus { get; set; }
        public int ApprovalStatus { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 含集合的测试实体
    /// </summary>
    [Serializable]
    public class TestEntityWithDetails
    {
        public int ID { get; set; }
        public string OrderNo { get; set; }
        public int DataStatus { get; set; }
        public System.Collections.Generic.List<TestOrderDetail> Details { get; set; } = new System.Collections.Generic.List<TestOrderDetail>();
    }

    /// <summary>
    /// 测试子项实体
    /// </summary>
    [Serializable]
    public class TestOrderDetail
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
    }

    #endregion
}
