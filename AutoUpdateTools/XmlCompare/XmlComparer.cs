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
            if (leftDoc == null || rightDoc == null)
                return new List<DiffBlock>();
                
            // 1. 标准化XML格式
            var leftLines = PrettyPrintXml(leftDoc).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var rightLines = PrettyPrintXml(rightDoc).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // 2. 使用Differ计算差异
            var differ = new Differ();
            var changes = differ.DiffText(string.Join("\n", leftLines), string.Join("\n", rightLines));

            // 3. 将差异转换为DiffBlock
            var blocks = new List<DiffBlock>();
            DiffBlock currentBlock = null;

            foreach (var change in changes)
            {
                if (change == null) continue;
                
                if (currentBlock == null || currentBlock.Type != GetDiffType(change.Type))
                {
                    currentBlock = new DiffBlock { Type = GetDiffType(change.Type) };
                    blocks.Add(currentBlock);
                }

                switch (change.Type)
                {
                    case DiffType.Unchanged:
                        currentBlock.AddLeftLine(change.LeftText ?? string.Empty);
                        currentBlock.AddRightLine(change.RightText ?? string.Empty);
                        break;

                    case DiffType.Added:
                        currentBlock.AddRightLine(change.RightText ?? string.Empty);
                        break;

                    case DiffType.Removed:
                        currentBlock.AddLeftLine(change.LeftText ?? string.Empty);
                        break;

                    case DiffType.Modified:
                        // 计算内联差异
                        var inlineDiff = ComputeInlineDiff(change.LeftText ?? string.Empty, change.RightText ?? string.Empty);
                        currentBlock.AddLeftLine(change.LeftText ?? string.Empty, inlineDiff);
                        currentBlock.AddRightLine(change.RightText ?? string.Empty, inlineDiff);
                        break;
                }
            }

            return blocks;
        }

        private List<DiffSegment> ComputeInlineDiff(string left, string right)
        {
            if (left == right)
                return new[] { new DiffSegment { Text = left ?? string.Empty, IsModified = false } }.ToList();

            // 使用简单的逐字符比较算法
            var segments = new List<DiffSegment>();
            if (string.IsNullOrEmpty(left) && string.IsNullOrEmpty(right))
                return segments;
                
            int minLen = Math.Min(left?.Length ?? 0, right?.Length ?? 0);
            int start = 0;

            // 查找前面相同的部分
            while (start < minLen && left[start] == right[start])
                start++;

            // 查找后面相同的部分
            int end = 0;
            int leftLen = left?.Length ?? 0;
            int rightLen = right?.Length ?? 0;
            
            while (end < minLen - start &&
                   left[leftLen - 1 - end] == right[rightLen - 1 - end])
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

            int leftDiffLen = leftLen - start - end;
            int rightDiffLen = rightLen - start - end;

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
                    Text = left.Substring(leftLen - end),IsModified = false
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
            if (doc == null) return string.Empty;
            
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
