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
            _lineMatchFunc = matchFunc ?? throw new ArgumentNullException(nameof(matchFunc));
        }

        public void SetInlineDiffFunction(Func<string, string, IEnumerable<DiffSegment>> diffFunc)
        {
            _inlineDiffFunc = diffFunc ?? throw new ArgumentNullException(nameof(diffFunc));
        }
        
        private string NormalizeXmlLine(string line)
        {
            if (string.IsNullOrEmpty(line)) return line;
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
            if (results == null || match == null) return;
            
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
            if (input == null || input.Count == 0) return input ?? new List<DiffBlock>();

            var output = new List<DiffBlock> { input[0] };

            for (int i = 1; i < input.Count; i++)
            {
                var last = output[output.Count - 1];
                var current = input[i];

                if (last == null || current == null) continue;
                
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
            if (leftChunk == null) leftChunk = new string[0];
            if (rightChunk == null) rightChunk = new string[0];
            
            if (leftChunk.Length == 0 && rightChunk.Length > 0)
                return DiffType.Added;

            if (leftChunk.Length > 0 && rightChunk.Length == 0)
                return DiffType.Removed;

            return DiffType.Modified;
        }

        private void ProcessUnmatchedSection(List<DiffBlock> results, int leftPos, int rightPos, Match match)
        {
            if (results == null || match == null) return;
            
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
            if (left == null) left = new string[0];
            if (right == null) right = new string[0];
            
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
                    if (j < rightStart) continue; // 只考虑右侧当前位置之后的匹配

                    int length = 0;
                    // 计算连续匹配长度
                    while (i + length < leftLen && j + length < rightLen &&
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
            // 如果使用自定义函数，则调用它
            if (_inlineDiffFunc != null && _inlineDiffFunc != ComputeInlineDiff)
            {
                return _inlineDiffFunc(left, right);
            }

            // 默认的字符级差异计算
            var segments = new List<DiffSegment>();

            // 简单实现：如果字符串不同，则整个字符串标记为修改
            if (left != right)
            {
                segments.Add(new DiffSegment
                {
                    Text = left ?? string.Empty,
                    RightText = right ?? string.Empty,
                    IsModified = true,
                    DiffType = DiffType.Modified
                });
            }
            else
            {
                segments.Add(new DiffSegment
                {
                    Text = left ?? string.Empty,
                    IsModified = false,
                    DiffType = DiffType.Unchanged
                });
            }

            return segments;
        }

        /// <summary>
        /// 内部匹配结构
        /// </summary>
        private class Match
        {
            public int LeftStart { get; }
            public int RightStart { get; }
            public int Length { get; }

            public Match(int leftStart, int rightStart, int length)
            {
                LeftStart = leftStart;
                RightStart = rightStart;
                Length = length;
            }
        }
    }
}