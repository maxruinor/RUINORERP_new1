using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("开始修复系统简介文件编码问题...");
        
        string filePath = @"系统简介.htm";
        
        // 检查文件是否存在
        if (!File.Exists(filePath))
        {
            Console.WriteLine("错误: 文件不存在");
            return;
        }
        
        // 读取文件内容
        string content = File.ReadAllText(filePath, Encoding.UTF8);
        Console.WriteLine("文件读取成功");
        
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
            {"瀹℃牳鍙嶅鏍哥粨妗堜笟鍔℃祦绋", "审核反审核结案业务流程"}
        };
        
        // 应用替换
        string fixedContent = content;
        int replacementsMade = 0;
        foreach (var pair in replacements)
        {
            if (fixedContent.Contains(pair.Key))
            {
                Console.WriteLine($"替换: {pair.Key} -> {pair.Value}");
                fixedContent = fixedContent.Replace(pair.Key, pair.Value);
                replacementsMade++;
            }
        }
        
        Console.WriteLine($"共进行了 {replacementsMade} 次替换");
        
        // 写入修复后的内容
        File.WriteAllText(filePath, fixedContent, Encoding.UTF8);
        
        Console.WriteLine("文件编码修复完成!");
    }
}