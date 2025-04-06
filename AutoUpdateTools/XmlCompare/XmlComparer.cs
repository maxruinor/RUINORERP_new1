using AutoUpdateTools.XmlCompare;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace AutoUpdateTools
{
    public class XmlComparer
    {
        public List<DiffBlock> CompareXml(XDocument leftDoc, XDocument rightDoc)
        {
            // 1. 标准化XML格式
            var leftLines = PrettyPrintXml(leftDoc).Split('\n');
            var rightLines = PrettyPrintXml(rightDoc).Split('\n');

            // 2. 使用Differ计算差异
            var differ = new Differ();
            var changes = differ.DiffText(string.Join("\n", leftLines), string.Join("\n", rightLines));

            // 3. 将差异转换为DiffBlock
            var blocks = new List<DiffBlock>();
            DiffBlock currentBlock = null;

            foreach (var change in changes)
            {
                if (currentBlock == null || currentBlock.Type != GetDiffType(change.Type))
                {
                    currentBlock = new DiffBlock { Type = GetDiffType(change.Type) };
                    blocks.Add(currentBlock);
                }

                switch (change.Type)
                {
                    case DiffType.Unchanged:
                        currentBlock.AddLeftLine(change.LeftText);
                        currentBlock.AddRightLine(change.RightText);
                        break;

                    case DiffType.Added:
                        currentBlock.AddRightLine(change.RightText);
                        break;

                    case DiffType.Removed:
                        currentBlock.AddLeftLine(change.LeftText);
                        break;

                    case DiffType.Modified:
                        // 计算内联差异
                        var inlineDiff = ComputeInlineDiff(change.LeftText, change.RightText);
                        currentBlock.AddLeftLine(change.LeftText, inlineDiff);
                        currentBlock.AddRightLine(change.RightText, inlineDiff);
                        break;
                }
            }

            return blocks;
        }

        private List<DiffSegment> ComputeInlineDiff(string left, string right)
        {
            if (left == right)
                return new[] { new DiffSegment { Text = left, IsModified = false } }.ToList();

            // 使用简单的逐字符比较算法
            var segments = new List<DiffSegment>();
            int minLen = Math.Min(left.Length, right.Length);
            int start = 0;

            // 查找前面相同的部分
            while (start < minLen && left[start] == right[start])
                start++;

            // 查找后面相同的部分
            int end = 0;
            while (end < minLen - start &&
                   left[left.Length - 1 - end] == right[right.Length - 1 - end])
                end++;

            // 中间不同的部分
            if (start > 0)
            {
                segments.Add(new DiffSegment
                {
                    Text = left.Substring(0, start),
                    IsModified = false
                });
            }

            int leftDiffLen = left.Length - start - end;
            int rightDiffLen = right.Length - start - end;

            if (leftDiffLen > 0 || rightDiffLen > 0)
            {
                segments.Add(new DiffSegment
                {
                    Text = leftDiffLen > 0 ? left.Substring(start, leftDiffLen) : "",
                    IsModified = true,
                    RightText = rightDiffLen > 0 ? right.Substring(start, rightDiffLen) : ""
                });
            }

            if (end > 0)
            {
                segments.Add(new DiffSegment
                {
                    Text = left.Substring(left.Length - end),
                    IsModified = false
                });
            }

            return segments;
        }

        private DiffType GetDiffType(DiffType changeType)
        {
            switch (changeType)
            {
                case DiffType.Added: return DiffType.Added;
                case DiffType.Removed: return DiffType.Removed;
                case DiffType.Modified: return DiffType.Modified;
                default: return DiffType.Unchanged;
            }
        }

        /// <summary>
        /// 美化xml格式
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static string PrettyPrintXml(XDocument doc)
        {
            var stringWriter = new StringWriter();
            var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            });

            doc.Save(xmlWriter);
            xmlWriter.Flush();
            return stringWriter.GetStringBuilder().ToString();

        }


    }
}
