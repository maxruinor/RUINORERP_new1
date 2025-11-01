using System;

namespace RUINORERP.Server.BNR.Examples
{
    /// <summary>
    /// 数据库序号生成使用示例
    /// </summary>
    public class UsageExample
    {
        public static void Main(string[] args)
        {
            // 设置数据库连接字符串（在实际项目中，这应该是您的主数据库连接字符串）
           // DatabaseSequenceParameter.SetDefaultConnectionString("Server=your_server;Database=your_database;User Id=your_user;Password=your_password;");
            
            // 创建BNR工厂实例
            var factory = new BNRFactory();
            
            // 注册所有默认的参数处理器（包括我们新创建的DatabaseSequenceParameter）
            factory.Initialize();
            
            // 也可以单独注册数据库序号参数处理器
            // factory.Register("DB", new DatabaseSequenceParameter());
            
            // 使用示例1: 基本序号生成
            // 规则: {DB:ORDER/00000} 表示使用"ORDER"作为键生成序号，格式化为5位数字
            string rule1 = "{DB:ORDER/00000}";
            string result1 = factory.Create(rule1);
            Console.WriteLine($"规则: {rule1} => 结果: {result1}");
            
            // 使用示例2: 结合日期参数
            // 规则: {DB:{D:yyyyMMdd}/000} 表示使用当前日期作为键生成序号，格式化为3位数字
            string rule2 = "{DB:{D:yyyyMMdd}/000}";
            string result2 = factory.Create(rule2);
            Console.WriteLine($"规则: {rule2} => 结果: {result2}");
            
            // 使用示例3: 结合常量和日期
            // 规则: {S:OD}{D:yyMM}{DB:{S:ORDER}{D:yyMM}/0000} 
            // 表示生成如"OD2301ORDER23010001"这样的序号
            string rule3 = "{S:OD}{D:yyMM}{DB:{S:ORDER}{D:yyMM}/0000}";
            string result3 = factory.Create(rule3);
            Console.WriteLine($"规则: {rule3} => 结果: {result3}");
            
            // 使用示例4: 多个序号生成
            // 规则: {DB:ORDER/00000}-{DB:INVOICE/000}
            string rule4 = "{DB:ORDER/00000}-{DB:INVOICE/000}";
            string result4 = factory.Create(rule4);
            Console.WriteLine($"规则: {rule4} => 结果: {result4}");
            
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}