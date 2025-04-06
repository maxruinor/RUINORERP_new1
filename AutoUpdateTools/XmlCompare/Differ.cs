using AutoUpdateTools.XmlCompare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdateTools
{
    public class Differ
    {
        public List<TextChange> DiffText(string left, string right)
        {
            var changes = new List<TextChange>();

            // 简单的基于行的差异比较
            var leftLines = left.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var rightLines = right.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            int i = 0, j = 0;
            while (i < leftLines.Length || j < rightLines.Length)
            {
                if (i < leftLines.Length && j < rightLines.Length &&
                    NormalizeLine(leftLines[i]) == NormalizeLine(rightLines[j]))
                {
                    // 匹配行
                    changes.Add(new TextChange
                    {
                        Type = DiffType.Unchanged,
                        LeftText = leftLines[i],
                        RightText = rightLines[j],
                        LeftStart = i,
                        LeftLength = 1,
                        RightStart = j,
                        RightLength = 1
                    });
                    i++;
                    j++;
                }
                else
                {
                    // 不匹配行
                    if (j < rightLines.Length &&
                        (i >= leftLines.Length || IsLineEarlier(rightLines[j], leftLines[i])))
                    {
                        // 右侧新增
                        changes.Add(new TextChange
                        {
                            Type = DiffType.Added,
                            RightText = rightLines[j],
                            RightStart = j,
                            RightLength = 1
                        });
                        j++;
                    }
                    else if (i < leftLines.Length)
                    {
                        // 左侧删除
                        changes.Add(new TextChange
                        {
                            Type = DiffType.Removed,
                            LeftText = leftLines[i],
                            LeftStart = i,
                            LeftLength = 1
                        });
                        i++;
                    }
                }
            }

            return changes;
        }

        private bool IsLineEarlier(string line1, string line2)
        {
            // 实现您的行比较逻辑
            return string.Compare(line1, line2, StringComparison.Ordinal) < 0;
        }

        private string NormalizeLine(string line)
        {
            // 标准化行以进行比较
            return line.Trim();
        }
    }

    public class TextChange
    {
        public DiffType Type { get; set; }
        public string LeftText { get; set; }
        public string RightText { get; set; }
        public int LeftStart { get; set; }
        public int LeftLength { get; set; }
        public int RightStart { get; set; }
        public int RightLength { get; set; }
    }
}
