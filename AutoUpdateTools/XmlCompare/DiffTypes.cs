using System;

namespace AutoUpdateTools.XmlCompare
{
    /// <summary>
    /// 差异段落类
    /// </summary>
    public class DiffSegment
    {
        public string Text { get; set; } = string.Empty;
        public bool IsModified { get; set; }
        public string RightText { get; set; } = string.Empty; // 用于显示右侧不同的文本
        public DiffType DiffType { get; internal set; }
        public int StartPosition { get; internal set; }
        public int Length { get; internal set; }
    }

    /// <summary>
    /// 差异类型枚举
    /// </summary>
    public enum DiffType
    {
        Unchanged,
        Modified,
        Added,
        Removed
    }
}