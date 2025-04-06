using AutoUpdateTools.XmlCompare;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AutoUpdateTools
{
    public class EnhancedXmlDiff
    {
        public List<DiffBlock> CompareXmlFiles(XDocument leftDoc, XDocument rightDoc)
        {
            // 规范化XML格式（保持一致的缩进和换行）
            var leftLines = XmlComparer.PrettyPrintXml(leftDoc).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var rightLines = XmlComparer.PrettyPrintXml(rightDoc).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // 使用改进的差异算法
            var diff = new AdvancedDiff(leftLines, rightLines);
            diff.SetLineMatchFunction((a, b) => NormalizeXmlLine(a) == NormalizeXmlLine(b));

            var rawResults = diff.ComputeDiff();
            return PostProcessResults(rawResults);
        }

        private List<DiffBlock> PostProcessResults(List<DiffBlock> rawResults)
        {
            if (rawResults.Count == 0)
                return rawResults;

            var processed = new List<DiffBlock> { rawResults[0] };

            for (int i = 1; i < rawResults.Count; i++)
            {
                var last = processed.Last();
                var current = rawResults[i];

                if (last.Type == current.Type)
                {
                    // 合并相同类型的块
                    last.LeftLines.AddRange(current.LeftLines);
                    last.RightLines.AddRange(current.RightLines);
                    last.InlineDiffs.AddRange(current.InlineDiffs);
                }
                else
                {
                    processed.Add(current);
                }
            }

            return processed;
        }

        private string NormalizeXmlLine(string line)
        {
            // 移除空白和格式化差异，只比较内容
            return Regex.Replace(line, @"\s+", " ").Trim();
        }

 
    }
}
