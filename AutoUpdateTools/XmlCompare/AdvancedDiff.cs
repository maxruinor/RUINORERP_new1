using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdateTools.XmlCompare
{
    public class AdvancedDiff
    {
        private readonly string[] _left;
        private readonly string[] _right;
        private Func<string, string, bool> _lineMatchFunc;
        private Func<string, string, IEnumerable<DiffSegment>> _inlineDiffFunc;

        public AdvancedDiff(string[] left, string[] right)
        {
            _left = left;
            _right = right;
            _lineMatchFunc = (a, b) => a == b;
            _inlineDiffFunc = ComputeInlineDiff;
        }

        public void SetLineMatchFunction(Func<string, string, bool> matchFunc)
        {
            _lineMatchFunc = matchFunc;
        }

        public void SetInlineDiffFunction(Func<string, string, IEnumerable<DiffSegment>> diffFunc)
        {
            _inlineDiffFunc = diffFunc;
        }

        public List<DiffResult> ComputeDiff()
        {
            var results = new List<DiffResult>();
            int leftPos = 0, rightPos = 0;

            while (leftPos < _left.Length || rightPos < _right.Length)
            {
                // 查找最长的匹配序列
                var match = FindLongestMatch(leftPos, rightPos);

                // 处理不匹配的部分
                if (leftPos < match.LeftStart || rightPos < match.RightStart)
                {
                    var leftLines = _left.Skip(leftPos).Take(match.LeftStart - leftPos).ToArray();
                    var rightLines = _right.Skip(rightPos).Take(match.RightStart - rightPos).ToArray();

                    results.Add(new DiffResult
                    {
                        Type = DiffType.Modified,
                        LeftLines = leftLines,
                        RightLines = rightLines,
                        InlineDiffs = leftLines.Zip(rightLines, (l, r) => _inlineDiffFunc(l, r)).ToList()
                    });
                }

                // 添加匹配的部分
                if (match.Length > 0)
                {
                    var matchedLines = _left.Skip(match.LeftStart).Take(match.Length).ToArray();
                    results.Add(new DiffResult
                    {
                        Type = DiffType.Unchanged,
                        LeftLines = matchedLines,
                        RightLines = matchedLines
                    });
                }

                leftPos = match.LeftStart + match.Length;
                rightPos = match.RightStart + match.Length;
            }

            return results;
        }

        private Match FindLongestMatch(int leftStart, int rightStart)
        {
            int maxLength = 0;
            int bestLeft = leftStart;
            int bestRight = rightStart;
            int leftLen = _left.Length;
            int rightLen = _right.Length;

            // 使用字典加速查找
            var potentialMatches = new Dictionary<string, List<int>>();
            for (int j = rightStart; j < rightLen; j++)
            {
                var key = _right[j];
                if (!potentialMatches.ContainsKey(key))
                    potentialMatches[key] = new List<int>();
                potentialMatches[key].Add(j);
            }

            for (int i = leftStart; i < leftLen; i++)
            {
                var leftLine = _left[i];
                if (!potentialMatches.TryGetValue(leftLine, out var rightIndices))
                    continue;

                foreach (var j in rightIndices)
                {
                    if (j < rightStart) continue;

                    int length = 1;
                    while (i + length < leftLen &&
                           j + length < rightLen &&
                           _lineMatchFunc(_left[i + length], _right[j + length]))
                    {
                        length++;
                    }

                    if (length > maxLength)
                    {
                        maxLength = length;
                        bestLeft = i;
                        bestRight = j;
                    }
                }
            }

            return new Match(bestLeft, bestRight, maxLength);
        }

        private IEnumerable<DiffSegment> ComputeInlineDiff(string left, string right)
        {
            if (left == right)
                return new[] { new DiffSegment { Text = left, IsModified = false } };

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
    }

    public class Match
    {
        public int LeftStart { get; }
        public int RightStart { get; }
        public int Length { get; }

        public Match(int left, int right, int length)
        {
            LeftStart = left;
            RightStart = right;
            Length = length;
        }
    }

    public class DiffResult
    {
        public DiffType Type { get; set; }
        public string[] LeftLines { get; set; }
        public string[] RightLines { get; set; }
        public List<IEnumerable<DiffSegment>> InlineDiffs { get; set; }
    }

    public class DiffSegment
    {
        public string Text { get; set; }
        public bool IsModified { get; set; }
        public string RightText { get; set; } // 用于显示右侧不同的文本
    }

    public enum DiffType
    {
        Unchanged,
        Modified,
        Added,
        Removed
    }
}
