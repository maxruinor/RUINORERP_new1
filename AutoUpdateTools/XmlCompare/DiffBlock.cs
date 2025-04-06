using AutoUpdateTools.XmlCompare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdateTools
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
            LeftLines.Add(line);
            InlineDiffs.Add(inlineDiff ?? new List<DiffSegment>());
        }

        public void AddRightLine(string line, List<DiffSegment> inlineDiff = null)
        {
            RightLines.Add(line);
            InlineDiffs.Add(inlineDiff ?? new List<DiffSegment>());
        }

        public bool HasInlineDiffs()
        {
            return InlineDiffs.Any(d => d != null && d.Any(s => s.IsModified));
        }
    }
}
