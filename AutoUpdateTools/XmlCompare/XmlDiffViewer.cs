using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using AutoUpdateTools.XmlCompare;

namespace AutoUpdateTools
{
    public partial class XmlDiffViewer : UserControl
    {
        private SafeScrollSynchronizer _syncOldToNew;
        private SafeScrollSynchronizer _syncNewToOld;

        public XmlDiffViewer()
        {
            InitializeComponent();
            SetupRichTextBoxes();
            SetupSafeScrollSync();
        }

        private void SetupRichTextBoxes()
        {
            // 设置左右两个RichTextBox的样式
            foreach (var rtb in new[] { leftBox, rightBox })
            {
                rtb.Font = new Font("Consolas", 10);
                rtb.WordWrap = false;
                rtb.ScrollBars = RichTextBoxScrollBars.Both;
                rtb.DetectUrls = false;
                rtb.ReadOnly = true;
            }
        }

        private void SetupSafeScrollSync()
        {
            // 关键：使用单向同步，并交换主从关系
            _syncOldToNew = new SafeScrollSynchronizer(leftBox, rightBox);
            _syncNewToOld = new SafeScrollSynchronizer(rightBox, leftBox);
        }

        /// <summary>
        /// 显示XML差异结果
        /// </summary>
        public void DisplayDifferences(List<DiffBlock> diffBlocks)
        {
            if (diffBlocks == null) return;

            leftBox.Clear();
            rightBox.Clear();

            foreach (var block in diffBlocks)
            {
                Color backColor = GetBackColorForDiffType(block.Type);
                Color textColor = GetTextColorForDiffType(block.Type);
                FontStyle fontStyle = GetFontStyleForDiffType(block.Type);

                int maxLines = Math.Max(block.LeftLines.Count, block.RightLines.Count);
                for (int i = 0; i < maxLines; i++)
                {
                    // 处理左侧文本
                    if (i < block.LeftLines.Count)
                    {
                        AppendDiffLine(leftBox, block.LeftLines[i], textColor,
                                     backColor, fontStyle,
                                     block.InlineDiffs.Count > i ? block.InlineDiffs[i] : null,
                                     false);
                    }
                    else
                    {
                        // 左侧没有对应行，添加空行
                        AppendDiffLine(leftBox, "", textColor, backColor, fontStyle, null, false);
                    }

                    // 处理右侧文本
                    if (i < block.RightLines.Count)
                    {
                        AppendDiffLine(rightBox, block.RightLines[i], textColor,
                                     backColor, fontStyle,
                                     block.InlineDiffs.Count > i ? block.InlineDiffs[i] : null,
                                     true);
                    }
                    else
                    {
                        // 右侧没有对应行，添加空行
                        AppendDiffLine(rightBox, "", textColor, backColor, fontStyle, null, true);
                    }
                }
            }
        }

        private void AppendDiffLine(RichTextBox box, string text, Color textColor,
                                  Color backColor, FontStyle fontStyle,
                                  List<DiffSegment> inlineDiffs, bool isRightSide)
        {
            if (box == null || text == null) return;
            
            box.SelectionBackColor = backColor;
            box.SelectionColor = textColor;
            box.SelectionFont = new Font(box.Font, fontStyle);

            if (inlineDiffs == null || inlineDiffs.Count == 0)
            {
                box.AppendText(text + Environment.NewLine);
                return;
            }

            foreach (var segment in inlineDiffs)
            {
                if (segment == null) continue;
                
                if (segment.IsModified)
                {
                    box.SelectionColor = XmlDiffColorScheme.ModifiedTextColor;
                    box.AppendText(isRightSide && !string.IsNullOrEmpty(segment.RightText)
                        ? segment.RightText : segment.Text);
                }
                else
                {
                    box.SelectionColor = textColor;
                    box.AppendText(segment.Text ?? string.Empty);
                }
            }
            box.AppendText(Environment.NewLine);
        }

        private Color GetBackColorForDiffType(DiffType type)
        {
            switch (type)
            {
                case DiffType.Added: return XmlDiffColorScheme.AddedBackColor;
                case DiffType.Removed: return XmlDiffColorScheme.RemovedBackColor;
                case DiffType.Modified: return XmlDiffColorScheme.ModifiedBackColor;
                default: return XmlDiffColorScheme.UnchangedBackColor;
            }
        }

        private Color GetTextColorForDiffType(DiffType type)
        {
            switch (type)
            {
                case DiffType.Added: return XmlDiffColorScheme.AddedTextColor;
                case DiffType.Removed: return XmlDiffColorScheme.RemovedTextColor;
                case DiffType.Modified: return XmlDiffColorScheme.ModifiedTextColor;
                default: return XmlDiffColorScheme.UnchangedTextColor;
            }
        }

        private FontStyle GetFontStyleForDiffType(DiffType type)
        {
            return type == DiffType.Removed
                ? XmlDiffColorScheme.RemovedFontStyle
                : FontStyle.Regular;
        }
    }
}