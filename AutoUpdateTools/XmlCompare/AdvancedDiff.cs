using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutoUpdateTools.XmlCompare
{

    /// <summary>
    /// 高级XML差异比较器 (优化版)
    /// </summary>
    public class AdvancedDiff
    {
        private readonly string[] _left;
        private readonly string[] _right;
        private Func<string, string, bool> _lineMatchFunc;
        private Func<string, string, IEnumerable<DiffSegment>> _inlineDiffFunc;

        public AdvancedDiff(string[] left, string[] right)
        {
            _left = left ?? throw new ArgumentNullException(nameof(left));
            _right = right ?? throw new ArgumentNullException(nameof(right));

            _lineMatchFunc = (a, b) => NormalizeXmlLine(a) == NormalizeXmlLine(b);
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
        private string NormalizeXmlLine(string line)
        {
            // 移除空白和格式化差异，只比较内容
            return Regex.Replace(line, @"\s+", " ").Trim();
        }
        /// <summary>
        /// 计算差异结果（安全优化版）
        /// </summary>
        public List<DiffBlock> ComputeDiff()
        {
            var results = new List<DiffBlock>();
            int leftPos = 0, rightPos = 0;
            int maxIterations = _left.Length + _right.Length;
            int iterations = 0;

            while ((leftPos < _left.Length || rightPos < _right.Length) &&
                   iterations++ < maxIterations * 2) // 安全阀
            {
                var match = FindLongestMatch(leftPos, rightPos);

                // 处理不匹配部分
                if (leftPos < match.LeftStart || rightPos < match.RightStart)
                {
                    ProcessUnmatchedSection(results, leftPos, rightPos, match);
                }

                // 处理匹配部分
                if (match.Length > 0)
                {
                    ProcessMatchedSection(results, match);
                }

                // 更新位置
                leftPos = match.LeftStart + match.Length;
                rightPos = match.RightStart + match.Length;
            }

            return GroupResults(results);
        }

        /// <summary>
        /// 处理匹配的部分
        /// </summary>
        private void ProcessMatchedSection(List<DiffBlock> results, Match match)
        {
            var matchedLines = _left.Skip(match.LeftStart).Take(match.Length).ToArray();

            var block = new DiffBlock
            {
                Type = DiffType.Unchanged,
                LeftLines = matchedLines.ToList(),
                RightLines = matchedLines.ToList()
            };

            results.Add(block);
        }

        /// <summary>
        /// 分组差异结果，合并相邻的相同类型块
        /// </summary>
        private List<DiffBlock> GroupResults(List<DiffBlock> input)
        {
            if (input.Count == 0) return input;

            var output = new List<DiffBlock> { input[0] };

            for (int i = 1; i < input.Count; i++)
            {
                var last = output[output.Count - 1];
                var current = input[i];

                if (last.Type == current.Type)
                {
                    // 合并相同类型的块
                    last.LeftLines.AddRange(current.LeftLines);
                    last.RightLines.AddRange(current.RightLines);
                    last.InlineDiffs.AddRange(current.InlineDiffs);
                }
                else
                {
                    output.Add(current);
                }
            }

            return output;
        }

        /// <summary>
        /// 确定差异类型
        /// </summary>
        private DiffType DetermineDiffType(string[] leftChunk, string[] rightChunk)
        {
            if (leftChunk.Length == 0 && rightChunk.Length > 0)
                return DiffType.Added;

            if (leftChunk.Length > 0 && rightChunk.Length == 0)
                return DiffType.Removed;

            return DiffType.Modified;
        }

        private void ProcessUnmatchedSection(List<DiffBlock> results, int leftPos, int rightPos, Match match)
        {
            var leftChunk = _left.Skip(leftPos).Take(match.LeftStart - leftPos).ToArray();
            var rightChunk = _right.Skip(rightPos).Take(match.RightStart - rightPos).ToArray();

            var block = new DiffBlock
            {
                Type = DetermineDiffType(leftChunk, rightChunk),
                LeftLines = leftChunk.ToList(),
                RightLines = rightChunk.ToList(),
                InlineDiffs = CalculateInlineDiffs(leftChunk, rightChunk)
            };

            results.Add(block);
        }

        private List<List<DiffSegment>> CalculateInlineDiffs(string[] left, string[] right)
        {
            var diffs = new List<List<DiffSegment>>();
            int minLength = Math.Min(left.Length, right.Length);

            for (int i = 0; i < minLength; i++)
            {
                diffs.Add(ComputeInlineDiff(left[i], right[i]).ToList());
            }

            return diffs;
        }


        private Match FindLongestMatch(int leftStart, int rightStart)
        {
            int maxLength = 0;
            int bestLeft = leftStart;
            int bestRight = rightStart;
            int leftLen = _left.Length;
            int rightLen = _right.Length;

            // 添加安全检查
            if (leftStart >= leftLen || rightStart >= rightLen)
            {
                return new Match(leftStart, rightStart, 0);
            }

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
                    // 添加最大长度限制防止死循环
                    int maxPossibleLength = Math.Min(leftLen - i, rightLen - j);
                    while (length < maxPossibleLength &&
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

   

    public class DiffSegment
    {
        public string Text { get; set; }
        public bool IsModified { get; set; }
        public string RightText { get; set; } // 用于显示右侧不同的文本
        public DiffType DiffType { get; internal set; }
        public int StartPosition { get; internal set; }
        public int Length { get; internal set; }
    }

    public enum DiffType
    {
        Unchanged,
        Modified,
        Added,
        Removed
    }
}
