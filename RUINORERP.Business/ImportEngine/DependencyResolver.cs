using System;
using System.Collections.Generic;
using System.Linq;
using RUINORERP.Business.ImportEngine.Models;
using RUINORERP.Model.ImportEngine.Models;

namespace RUINORERP.Business.ImportEngine
{
    /// <summary>
    /// 依赖解析器：使用拓扑排序处理表之间的导入顺序
    /// </summary>
    public class DependencyResolver
    {
        public static List<ImportProfile> ResolveOrder(List<ImportProfile> profiles)
        {
            var graph = new Dictionary<string, HashSet<string>>();
            var inDegree = new Dictionary<string, int>();
            var profileMap = profiles.ToDictionary(p => p.TargetTable);

            // 初始化图
            foreach (var p in profiles)
            {
                if (!graph.ContainsKey(p.TargetTable))
                {
                    graph[p.TargetTable] = new HashSet<string>();
                    inDegree[p.TargetTable] = 0;
                }
            }

            // 构建边
            foreach (var p in profiles)
            {
                foreach (var dep in p.Dependencies)
                {
                    if (graph.ContainsKey(dep))
                    {
                        graph[dep].Add(p.TargetTable);
                        inDegree[p.TargetTable]++;
                    }
                }
            }

            // Kahn's算法进行拓扑排序
            var queue = new Queue<string>(inDegree.Where(x => x.Value == 0).Select(x => x.Key));
            var sorted = new List<ImportProfile>();

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                sorted.Add(profileMap[node]);

                foreach (var neighbor in graph[node])
                {
                    if (--inDegree[neighbor] == 0) queue.Enqueue(neighbor);
                }
            }

            if (sorted.Count != profiles.Count) throw new Exception("检测到循环依赖");
            return sorted;
        }
    }
}
