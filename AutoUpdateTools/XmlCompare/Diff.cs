using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdateTools
{
    // 差异算法类
    public class Diff
    {
        private readonly string[] _left;
        private readonly string[] _right;
        private Func<string, string, bool> _matchFunction = (a, b) => a == b;

        public Diff(string[] left, string[] right)
        {
            _left = left;
            _right = right;
        }

        public void AddMatchFunction(Func<string, string, bool> matchFunction)
        {
            _matchFunction = matchFunction;
        }

        public List<Match> Match()
        {
            var matches = new List<Match>();
            int leftPos = 0;
            int rightPos = 0;

            while (leftPos < _left.Length && rightPos < _right.Length)
            {
                var match = FindLongestMatch(leftPos, rightPos);
                if (match.Length > 0)
                {
                    if (leftPos < match.StartA || rightPos < match.StartB)
                    {
                        // 添加不匹配的部分
                    }
                    matches.Add(match);
                    leftPos = match.StartA + match.Length;
                    rightPos = match.StartB + match.Length;
                }
                else
                {
                    leftPos++;
                    rightPos++;
                }
            }

            return matches;
        }

        private Match FindLongestMatch(int leftStart, int rightStart)
        {
            int maxLength = 0;
            int bestLeft = leftStart;
            int bestRight = rightStart;

            for (int i = leftStart; i < _left.Length; i++)
            {
                for (int j = rightStart; j < _right.Length; j++)
                {
                    if (_matchFunction(_left[i], _right[j]))
                    {
                        int length = 1;
                        while (i + length < _left.Length &&
                               j + length < _right.Length &&
                               _matchFunction(_left[i + length], _right[j + length]))
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
            }

            return new Match(bestLeft, bestRight, maxLength);
        }
    }

    public class Match
    {
        public int StartA { get; }
        public int StartB { get; }
        public int Length { get; }

        public Match(int startA, int startB, int length)
        {
            StartA = startA;
            StartB = startB;
            Length = length;
        }
    }

    // 用于同步滚动的Win32 API
    public static class Win32
    {
        public const int WM_VSCROLL = 0x115;
        public const int SB_THUMBPOSITION = 4;
        public const int SB_VERT = 1;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetScrollPos(IntPtr hWnd, int nBar);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
    }

    // RichTextBox的扩展方法

}
