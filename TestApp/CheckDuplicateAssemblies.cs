using System;
using System.Linq;
using System.Reflection;

namespace CheckDuplicateAssemblies
{
    /// <summary>
    /// 程序集重复加载诊断工具
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== 检查已加载的程序集 ===");
            Console.WriteLine();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().Name.StartsWith("RUINORERP") || a.GetName().Name.StartsWith("企业数字化集成ERP"))
                .GroupBy(a => a.GetName().Name)
                .Select(g => new
                {
                    Name = g.Key,
                    Count = g.Count(),
                    Locations = g.Select(a => a.Location).Distinct().ToList()
                })
                .Where(a => a.Count > 1 || a.Locations.Count > 1)
                .ToList();

            if (assemblies.Any())
            {
                Console.WriteLine($"发现 {assemblies.Count} 个重复加载的程序集:");
                Console.WriteLine();
                foreach (var asm in assemblies)
                {
                    Console.WriteLine($"程序集: {asm.Name}");
                    Console.WriteLine($"  加载次数: {asm.Count}");
                    Console.WriteLine($"  不同路径: {asm.Locations.Count}");
                    foreach (var loc in asm.Locations)
                    {
                        Console.WriteLine($"    - {loc}");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("未发现重复加载的程序集");
                Console.WriteLine();
            }

            Console.WriteLine("=== 检查所有 RUINORERP 程序集 ===");
            Console.WriteLine();
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.GetName().Name.StartsWith("RUINORERP") || a.GetName().Name.StartsWith("企业数字化集成ERP"))
                .OrderBy(a => a.GetName().Name)
                .ToList();

            foreach (var asm in allAssemblies)
            {
                Console.WriteLine($"{asm.GetName().Name, -30} | {asm.Location ?? "(动态生成)", -80}");
            }
        }
    }
}
