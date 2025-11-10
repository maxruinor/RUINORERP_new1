using RUINORERP.Server.BNR;
using System;

namespace RUINORERP.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // 测试单大括号格式
            string singleBraceRule = "{S:RP:upper}{D:yyMMdd}{DB:{S:请购单}{D:yyMM}/000}";
            Console.WriteLine("测试单大括号格式: " + singleBraceRule);
            string[] singleBraceResults = RuleAnalysis.Execute(singleBraceRule);
            Console.WriteLine("解析结果数量: " + singleBraceResults.Length);
            for (int i = 0; i < singleBraceResults.Length; i++)
            {
                Console.WriteLine($"表达式 {i + 1}: {singleBraceResults[i]}");
            }
            
            // 测试双大括号格式（保持兼容性）
            string doubleBraceRule = "{{S:RP:upper}}{{D:yyMMdd}}{{DB:{{S:请购单}}{{D:yyMM}}/000}}";
            Console.WriteLine("\n测试双大括号格式: " + doubleBraceRule);
            string[] doubleBraceResults = RuleAnalysis.Execute(doubleBraceRule);
            Console.WriteLine("解析结果数量: " + doubleBraceResults.Length);
            for (int i = 0; i < doubleBraceResults.Length; i++)
            {
                Console.WriteLine($"表达式 {i + 1}: {doubleBraceResults[i]}");
            }
        }
    }
}