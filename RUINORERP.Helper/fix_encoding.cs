using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string directoryPath = @"e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Helper";
        ProcessDirectory(directoryPath);
        Console.WriteLine("已完成所有HTML文件的乱码修复。");
    }
    
    static void ProcessDirectory(string directoryPath)
    {
        // 获取所有HTML文件
        string[] htmlFiles = Directory.GetFiles(directoryPath, "*.htm", SearchOption.AllDirectories);
        Array.Resize(ref htmlFiles, htmlFiles.Length + Directory.GetFiles(directoryPath, "*.html", SearchOption.AllDirectories).Length);
        Directory.GetFiles(directoryPath, "*.html", SearchOption.AllDirectories).CopyTo(htmlFiles, Directory.GetFiles(directoryPath, "*.htm", SearchOption.AllDirectories).Length);
        
        foreach (string filePath in htmlFiles)
        {
            try
            {
                // 读取文件内容
                string content = File.ReadAllText(filePath, Encoding.UTF8);
                
                // 检查是否包含乱码字符
                if (content.Contains("瀹㈡埛鍏崇郴") || content.Contains("鍩虹璧勬枡") || 
                    content.Contains("瀹℃牳娴佺▼") || content.Contains("鐢熶骇绠＄悊") ||
                    content.Contains("杩涢攢瀛樼鐞") || content.Contains("鍞悗绠＄悊") ||
                    content.Contains("璐㈠姟绠＄悊") || content.Contains("琛屾斂绠＄悊") ||
                    content.Contains("鎶ヨ〃绠＄悊") || content.Contains("鐢靛晢杩愯惀") ||
                    content.Contains("绯荤粺璁剧疆") || content.Contains("閫氱敤鍔熻兘") ||
                    content.Contains("瀹℃牳鍙嶅鏍哥粨妗堜笟鍔℃祦绋") || content.Contains("瀵艰埅閾炬帴"))
                {
                    Console.WriteLine($"正在修复文件: {filePath}");
                    
                    // 修复乱码
                    content = FixEncodingIssues(content);
                    
                    // 以UTF-8 with BOM格式保存文件
                    File.WriteAllText(filePath, content, new UTF8Encoding(true));
                    Console.WriteLine($"已修复文件: {Path.GetFileName(filePath)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理文件时出错 {filePath}: {ex.Message}");
            }
        }
    }
    
    static string FixEncodingIssues(string content)
    {
        // 修复导航链接中的乱码
        content = content.Replace("瀵艰埅閾炬帴锛?", "导航链接：");
        content = content.Replace("鐢熶骇绠＄悊", "生产管理");
        content = content.Replace("杩涢攢瀛樼鐞", "进销存管理");
        content = content.Replace("鍞悗绠＄悊", "售后管理");
        content = content.Replace("瀹㈡埛鍏崇郴绠＄悊", "客户关系管理");
        content = content.Replace("瀹㈡埛鍏崇郴", "客户关系");
        content = content.Replace("璐㈠姟绠＄悊", "财务管理");
        content = content.Replace("琛屾斂绠＄悊", "行政管理");
        content = content.Replace("鎶ヨ〃绠＄悊", "报表管理");
        content = content.Replace("鐢靛晢杩愯惀", "电商运营");
        content = content.Replace("鍩虹璧勬枡绠＄悊", "基础资料管理");
        content = content.Replace("鍩虹璧勬枡", "基础资料");
        content = content.Replace("绯荤粺璁剧疆", "系统设置");
        content = content.Replace("閫氱敤鍔熻兘", "通用功能");
        content = content.Replace("瀹℃牳鍙嶅鏍哥粨妗堜笟鍔℃祦绋", "审核反审核结案业务流程");
        content = content.Replace("瀹℃牳娴佺▼", "审核流程");
        
        // 修复链接中的问题
        content = content.Replace("杩涢攢瀛樼鐞?htm", "进销存管理.htm");
        content = content.Replace("瀹℃牳鍙嶅鏍哥粨妗堜笟鍔℃祦绋?htm", "审核反审核结案业务流程.htm");
        
        return content;
    }
}