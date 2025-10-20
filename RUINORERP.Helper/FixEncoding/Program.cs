using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.WriteLine("开始修复HTML文件编码问题...");
        
        // 获取当前目录的上级目录（RUINORERP.Helper）
        string helperDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ".."));
        
        // 检查目录是否存在
        if (!Directory.Exists(helperDirectory))
        {
            Console.WriteLine($"错误: 目录不存在: {helperDirectory}");
            return;
        }
        
        Console.WriteLine($"正在处理目录: {helperDirectory}");
        
        // 查找所有HTML文件，包括子目录
        List<string> htmlFiles = new List<string>();
        htmlFiles.AddRange(Directory.GetFiles(helperDirectory, "*.htm*", SearchOption.TopDirectoryOnly));
        htmlFiles.AddRange(Directory.GetFiles(Path.Combine(helperDirectory, "controls"), "*.htm*", SearchOption.TopDirectoryOnly));
        
        Console.WriteLine($"找到 {htmlFiles.Count} 个HTML文件");
        
        // 定义替换规则
        Dictionary<string, string> replacements = new Dictionary<string, string>
        {
            {"鍏ㄥ眬鏍峰紡", "全局样式"},
            {"鏍囬鏍峰紡", "标题样式"},
            {"娈佃惤鏍峰紡", "段落样式"},
            {"鍒楄〃鏍峰紡", "列表样式"},
            {"琛ㄦ牸鏍峰紡", "表格样式"},
            {"寮鸿皟鏂囨湰", "强调文本"},
            {"閾炬帴鏍峰紡", "链接样式"},
            {"鎻愮ず妗嗘牱寮", "提示框样式"},
            {"瀵艰埅閾炬帴锛", "导航链接："},
            {"鐢熶骇绠＄悊", "生产管理"},
            {"杩涢攢瀛樼鐞", "进销存管理"},
            {"鍞悗绠＄悊", "售后管理"},
            {"瀹㈡埛鍏崇郴绠＄悊", "客户关系管理"},
            {"璐㈠姟绠＄悊", "财务管理"},
            {"琛屾斂绠＄悊", "行政管理"},
            {"鎶ヨ〃绠＄悊", "报表管理"},
            {"鐢靛晢杩愯惀", "电商运营"},
            {"鍩虹璧勬枡绠＄悊", "基础资料管理"},
            {"绯荤粺璁剧疆", "系统设置"},
            {"閫氱敤鍔熻兘", "通用功能"},
            {"瀹℃牳鍙嶅鏍哥粨妗堜笟鍔℃祦绋", "审核反审核结案业务流程"},
            {"瀹㈡埛鍏崇郴", "客户关系"},
            {"鍩虹璧勬枡", "基础资料"},
            {"瀹℃牳娴佺▼", "审核流程"},
            {"鏌ヨ鎸夐挳甯姪", "查询按钮帮助"},
            {"淇濆瓨鎸夐挳甯姪", "保存按钮帮助"}
        };
        
        int totalFilesProcessed = 0;
        int totalReplacementsMade = 0;
        
        // 处理每个HTML文件
        foreach (string filePath in htmlFiles)
        {
            try
            {
                // 读取文件内容
                string content = File.ReadAllText(filePath, Encoding.UTF8);
                
                // 检查是否包含乱码
                bool hasGarbledText = replacements.Keys.Any(key => content.Contains(key));
                
                if (hasGarbledText)
                {
                    Console.WriteLine($"处理文件: {Path.GetFileName(filePath)}");
                    
                    // 应用替换
                    string fixedContent = content;
                    int replacementsMade = 0;
                    foreach (var pair in replacements)
                    {
                        if (fixedContent.Contains(pair.Key))
                        {
                            int count = Regex.Matches(fixedContent, Regex.Escape(pair.Key)).Count;
                            fixedContent = fixedContent.Replace(pair.Key, pair.Value);
                            replacementsMade += count;
                        }
                    }
                    
                    // 特殊处理一些链接中的乱码
                    // 处理 href 属性中的乱码
                    fixedContent = Regex.Replace(fixedContent, @"href=""[^""]*瀹㈡埛鍏崇郴[^""]*""", "href=\"客户关系管理.htm\"");
                    fixedContent = Regex.Replace(fixedContent, @"href=""[^""]*鍩虹璧勬枡[^""]*""", "href=\"基础资料管理.htm\"");
                    fixedContent = Regex.Replace(fixedContent, @"href=""[^""]*瀹℃牳娴佺▼[^""]*""", "href=\"审核反审核结案业务流程.htm\"");
                    
                    // 清理特殊字符
                    fixedContent = fixedContent.Replace("?/strong>", "</strong>");
                    fixedContent = fixedContent.Replace("?htm", ".htm");
                    fixedContent = fixedContent.Replace("?/a>", "</a>");
                    fixedContent = fixedContent.Replace("提示框样式?", "提示框样式");
                    
                    // 确保文件以UTF-8 BOM编码保存
                    File.WriteAllText(filePath, fixedContent, new UTF8Encoding(true));
                    
                    Console.WriteLine($"  完成修复，替换 {replacementsMade} 处乱码");
                    totalReplacementsMade += replacementsMade;
                    totalFilesProcessed++;
                }
                else
                {
                    // 即使没有检测到乱码，也确保文件以UTF-8 BOM编码保存
                    File.WriteAllText(filePath, content, new UTF8Encoding(true));
                    Console.WriteLine($"  文件已转换为UTF-8 BOM编码: {Path.GetFileName(filePath)}");
                    totalFilesProcessed++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  处理文件 {Path.GetFileName(filePath)} 时出错: {ex.Message}");
            }
        }
        
        Console.WriteLine($"\n处理完成!");
        Console.WriteLine($"共处理了 {totalFilesProcessed} 个文件");
        Console.WriteLine($"共修复了 {totalReplacementsMade} 处乱码");
    }
}