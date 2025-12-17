using System;
using NPinyin;

namespace TestNPinyinUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            // 测试NPinyin库的基本用法
            Console.WriteLine("测试NPinyin库的基本用法:");
            
            // 测试字符串
            string testString = "学生相";
            Console.WriteLine($"原始字符串: {testString}");
            
            try
            {
                // 测试GetPinyin方法
                string pinyin = Pinyin.GetPinyin(testString);
                Console.WriteLine($"GetPinyin结果: {pinyin}");
                
                // 测试GetInitials方法
                string initials = Pinyin.GetInitials(testString);
                Console.WriteLine($"GetInitials结果: {initials}");
                
                // 测试单个字符
                foreach (char c in testString)
                {
                    string charPinyin = Pinyin.GetPinyin(c.ToString());
                    string charInitial = Pinyin.GetInitials(c.ToString());
                    Console.WriteLine($"字符 '{c}': 拼音={charPinyin}, 首字母={charInitial}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试出错: {ex.Message}");
                Console.WriteLine($"堆栈跟踪: {ex.StackTrace}");
            }
        }
    }
}