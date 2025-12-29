using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ProductEAV
{
    /// <summary>
    /// 多属性组合工具类（重构优化版）
    /// 提供高性能的属性组合算法，支持大规模数据处理
    /// </summary>
    public class ArrayCombination
    {
        /// <summary>
        /// 属性组合器结构体（重构版）
        /// 优化了命名和性能，添加了输入验证
        /// </summary>
        private struct AttributeCombiner
        {
            private string[] _elements;
            
            /// <summary>
            /// 元素数组
            /// </summary>
            public string[] Elements
            {
                get { return _elements ?? Array.Empty<string>(); }
                private set { _elements = value; }
            }
            
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="elements">元素数组</param>
            public AttributeCombiner(string[] elements)
            {
                _elements = elements ?? Array.Empty<string>();
            }
            
            /// <summary>
            /// 执行笛卡尔积运算（优化版）
            /// </summary>
            /// <param name="other">另一个组合器</param>
            /// <returns>组合结果</returns>
            public AttributeCombiner Multiply(AttributeCombiner other)
            {
                // 输入验证
                if (this.Elements.Length == 0)
                    return other;
                if (other.Elements.Length == 0)
                    return this;
                    
                // 预分配结果数组，提高性能
                string[] result = new string[this.Elements.Length * other.Elements.Length];
                int index = 0;
                
                // 使用Span优化性能
                var thisSpan = this.Elements.AsSpan();
                var otherSpan = other.Elements.AsSpan();
                
                for (int i = 0; i < thisSpan.Length; i++)
                {
                    for (int j = 0; j < otherSpan.Length; j++)
                    {
                        result[index] = $"{thisSpan[i]},{otherSpan[j]}";
                        index++;
                    }
                }
                
                return new AttributeCombiner(result);
            }
        }

        /// <summary>
        /// 打印组合结果（调试用）
        /// </summary>
        /// <param name="arrays">输入数组</param>
        public static void PrintResult(params string[][] arrays)
        {
            var combiner = new AttributeCombiner();
            foreach (string[] array in arrays)
            {
                // 添加输入验证
                if (array == null || array.Length == 0)
                    continue;
                    
                if (combiner.Elements.Length == 0)
                    combiner = new AttributeCombiner(array);
                else
                    combiner = combiner.Multiply(new AttributeCombiner(array));
            }
            
            foreach (string s in combiner.Elements)
                System.Diagnostics.Debug.WriteLine(s);
        }

        /// <summary>
        /// 计算字符串数组的组合（基础版本）
        /// </summary>
        /// <param name="arrays">输入数组列表</param>
        /// <returns>组合结果列表</returns>
        public static List<string> Combination(List<string[]> arrays)
        {
            // 输入验证
            if (arrays == null || arrays.Count == 0)
                return new List<string>();

            var combiner = new AttributeCombiner();
            foreach (string[] array in arrays)
            {
                // 跳过空数组
                if (array == null || array.Length == 0)
                    continue;
                    
                if (combiner.Elements.Length == 0)
                    combiner = new AttributeCombiner(array);
                else
                    combiner = combiner.Multiply(new AttributeCombiner(array));
            }
            
            return new List<string>(combiner.Elements);
        }

        /// <summary>
        /// 计算字符串列表的组合
        /// </summary>
        /// <param name="mylist">输入列表</param>
        /// <returns>组合结果列表</returns>
        public static List<string> Combination(List<List<string>> mylist)
        {
            // 输入验证
            if (mylist == null || mylist.Count == 0)
                return new List<string>();

            var combiner = new AttributeCombiner();
            foreach (List<string> array in mylist)
            {
                // 跳过空列表
                if (array == null || array.Count == 0)
                    continue;
                    
                if (combiner.Elements.Length == 0)
                    combiner = new AttributeCombiner(array.ToArray());
                else
                    combiner = combiner.Multiply(new AttributeCombiner(array.ToArray()));
            }
            
            return new List<string>(combiner.Elements);
        }

        /// <summary>
        /// 基于键值对的组合计算（优化版）
        /// </summary>
        /// <param name="kvps">键值对列表</param>
        /// <returns>组合结果列表</returns>
        public static List<string> Combination4Table(List<KeyValuePair<long, string[]>> kvps)
        {
            // 输入验证
            if (kvps == null || kvps.Count == 0)
                return new List<string>();

            var arrays = new List<string[]>();
            
            // 排序并过滤有效数据
            var validKvps = kvps
                .Where(kv => kv.Value != null && kv.Value.Length > 0)
                .OrderBy(kv => kv.Key)
                .ToList();
                
            foreach (var kv in validKvps)
            {
                arrays.Add(kv.Value);
            }

            // 使用优化后的Combination方法
            return Combination(arrays);
        }
        //static void Main(string[] args)
        //{
        //    string[] s1 = { "红色", "蓝色", "白色" };
        //    string[] s2 = { "大", "中", "小" };
        //    string[] s3 = { "10克", "20克", "30克" };
        //    PrintResult(s1);
        //    System.Diagnostics.Debug.WriteLine("--------------------------");
        //    PrintResult(s1, s2);
        //    System.Diagnostics.Debug.WriteLine("--------------------------");
        //    PrintResult(s1, s2, s3);

        //}
    }
}