using System;
using System.Collections.Generic;
using RUINORERP.Model;

namespace RUINORERP.UI.HelpSystem.Helper
{
    /// <summary>
    /// 自动生成帮助内容的程序
    /// </summary>
    public class GenerateHelpContent
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("开始生成销售订单帮助内容...");
            
            // 生成销售订单实体字段帮助内容
            var fieldHelpContents = HelpContentGenerator.GenerateFieldHelpContent(typeof(tb_SaleOrder));
            
            // 保存字段帮助内容到正确的路径
            HelpContentGenerator.SaveHelpContents(fieldHelpContents, @"e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\HelpContent");
            
            // 生成销售订单窗体帮助内容
            var formHelpContent = HelpContentGenerator.GenerateFormHelpContent("UCSaleOrder", typeof(tb_SaleOrder));
            
            // 保存窗体帮助内容
            var formPath = @"e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\HelpContent\Forms\UCSaleOrder.md";
            var formDir = System.IO.Path.GetDirectoryName(formPath);
            if (!System.IO.Directory.Exists(formDir))
            {
                System.IO.Directory.CreateDirectory(formDir);
            }
            System.IO.File.WriteAllText(formPath, formHelpContent);
            
            // 生成销售管理模块帮助内容
            var moduleHelpContent = HelpContentGenerator.GenerateModuleHelpContent("销售管理");
            var modulePath = @"e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\HelpContent\Modules\SalesManagement.md";
            var moduleDir = System.IO.Path.GetDirectoryName(modulePath);
            if (!System.IO.Directory.Exists(moduleDir))
            {
                System.IO.Directory.CreateDirectory(moduleDir);
            }
            System.IO.File.WriteAllText(modulePath, moduleHelpContent);
            
            Console.WriteLine("销售订单帮助内容生成完成！");
            Console.WriteLine($"- 字段帮助内容: {fieldHelpContents.Count} 个");
            Console.WriteLine("- 窗体帮助内容: 1 个 (UCSaleOrder)");
            Console.WriteLine("- 模块帮助内容: 1 个 (销售管理)");
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}