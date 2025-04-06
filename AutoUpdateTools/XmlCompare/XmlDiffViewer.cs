using AutoUpdateTools.XmlCompare;
using AutoUpdateTools.XmlCompare.AutoUpdateTools.XmlCompare;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdateTools
{
    public partial class XmlDiffViewer : UserControl
    {
        private SafeScrollSynchronizer _syncOldToNew;
        private SafeScrollSynchronizer _syncNewToOld;
        private SplitContainer splitContainer;
        private VScrollBar scrollBar;

        private SafeScrollSynchronizer _scrollSync;

        public XmlDiffViewer()
        {
            InitializeComponent();
            leftBox = CreateRichTextBox();
            rightBox = CreateRichTextBox();
            SetupRichTextBoxes();
            SetupSafeScrollSync();
        }

        private RichTextBox CreateRichTextBox()
        {
            return new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10),
                WordWrap = false,
                ScrollBars = RichTextBoxScrollBars.Both
            };
        }
        private void SetupSafeScrollSync()
        {
            // 关键：使用单向同步，并交换主从关系
            _syncOldToNew = new SafeScrollSynchronizer(leftBox, rightBox);
            _syncNewToOld = new SafeScrollSynchronizer(rightBox, leftBox);
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

        #region
        /// <summary>
        /// 显示XML差异结果
        /// </summary>
        public void DisplayDifferences(List<DiffBlock> diffBlocks)
        {
            leftBox.Clear();
            rightBox.Clear();

 

            foreach (var block in diffBlocks)
            {
                for (int i = 0; i < Math.Max(block.LeftLines.Count, block.RightLines.Count); i++)
                {
                    RenderLine(leftBox, block, i, false);
                    RenderLine(rightBox, block, i, true);
                }
            }
            return;
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

                    // 处理右侧文本
                    if (i < block.RightLines.Count)
                    {
                        AppendDiffLine(rightBox, block.RightLines[i], textColor,
                                     backColor, fontStyle,
                                     block.InlineDiffs.Count > i ? block.InlineDiffs[i] : null,
                                     true);
                    }
                }
            }
        }

        private void AppendDiffLine(RichTextBox box, string text, Color textColor,
                                  Color backColor, FontStyle fontStyle,
                                  List<DiffSegment> inlineDiffs, bool isRightSide)
        {
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
                if (segment.IsModified)
                {
                    box.SelectionColor = XmlDiffColorScheme.ModifiedTextColor;
                    box.AppendText(isRightSide && !string.IsNullOrEmpty(segment.RightText)
                        ? segment.RightText : segment.Text);
                }
                else
                {
                    box.SelectionColor = textColor;
                    box.AppendText(segment.Text);
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
        #endregion

   

        private void RenderLine(RichTextBox box, DiffBlock block, int index, bool isRight)
        {
            var lines = isRight ? block.RightLines : block.LeftLines;
            if (index >= lines.Count) return;

            var text = lines[index];
            var color = GetTextColor(block.Type);
            var style = GetFontStyle(block.Type);

            box.SelectionColor = color;
            box.SelectionFont = new Font(box.Font, style);
            box.AppendText(text + Environment.NewLine);
        }

        private Color GetTextColor(DiffType type)
        {
            switch (type)
            {
                case DiffType.Added:
                    return Color.Green;
                case DiffType.Removed:
                    return Color.Red;
                case DiffType.Modified:
                    return Color.Blue;
                default:
                    return Color.Black;
            }
        }

        private FontStyle GetFontStyle(DiffType type)
        {
            return type == DiffType.Removed ? FontStyle.Strikeout : FontStyle.Regular;
        }

        private void AppendLine(RichTextBox box, string text, Color foreColor, Color backColor,
                              List<DiffSegment> inlineDiffs, bool isRightSide)
        {
            box.SelectionBackColor = backColor;

            if (inlineDiffs == null || inlineDiffs.Count == 0)
            {
                box.SelectionColor = foreColor;
                box.AppendText(text + Environment.NewLine);
            }
            else
            {
                foreach (var segment in inlineDiffs)
                {
                    if (segment.IsModified)
                    {
                        box.SelectionColor = Color.Blue;
                        box.AppendText(isRightSide && !string.IsNullOrEmpty(segment.RightText)
                            ? segment.RightText : segment.Text);
                    }
                    else
                    {
                        box.SelectionColor = foreColor;
                        box.AppendText(segment.Text);
                    }
                }
                box.AppendText(Environment.NewLine);
            }
        }

        private void SynchronizeScroll()
        {
            // 实现两个RichTextBox的同步滚动
        }
    }

}
