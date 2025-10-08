using System;
using TestSerialization;

namespace TestSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== MessagePack 核心序列化测试 ===");
            Console.WriteLine($"测试时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            
            // 运行核心测试
            CoreTests.RunAllTests();

            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}