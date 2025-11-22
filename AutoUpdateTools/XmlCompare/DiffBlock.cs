using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoUpdateTools.XmlCompare
{
    public class DiffBlock
    {
        public DiffType Type { get; set; }
        public List<string> LeftLines { get; set; } = new List<string>();
        public List<string> RightLines { get; set; } = new List<string>();
        public List<List<DiffSegment>> InlineDiffs { get; set; } = new List<List<DiffSegment>>();

        // 添加辅助方法
        public void AddLeftLine(string line, List<DiffSegment> inlineDiff = null)
        {
            if (line == null) line = string.Empty;
            LeftLines.Add(line);
            InlineDiffs.Add(inlineDiff ?? new List<DiffSegment>());
        }

        public void AddRightLine(string line, List<DiffSegment> inlineDiff = null)
        {
            if (line == null) line = string.Empty;
            RightLines.Add(line);
            InlineDiffs.Add(inlineDiff ?? new List<DiffSegment>());
        }

        public bool HasInlineDiffs()
        {
            return InlineDiffs.Any(d => d != null && d.Any(s => s != null && s.IsModified));
        }
    }
}