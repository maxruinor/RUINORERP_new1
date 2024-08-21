using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using WinLib.ToolTipExNS;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-12-20
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    public class ProfessionalToolStripRendererEx 
        : ToolStripRenderer
    {
        private static readonly int OffsetMargin = 24;

        private string _menuLogoString = "更多精彩尽在 www.WinLib.com";
        private ToolStripColorTable _colorTable;

        public ProfessionalToolStripRendererEx()
            : base()
        {
        }

        public ProfessionalToolStripRendererEx(
            ToolStripColorTable colorTable) : base()
        {
            _colorTable = colorTable;
        }

        public string MenuLogoString
        {
            get { return _menuLogoString; }
            set { _menuLogoString = value; }
        }

        protected virtual ToolStripColorTable ColorTable
        {
            get
            {
                if (_colorTable == null)
                {
                    _colorTable = new ToolStripColorTable();
                }
                return _colorTable;
            }
        }

        protected override void OnRenderToolStripBackground(
            ToolStripRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            Rectangle bounds = e.AffectedBounds;

            if (toolStrip is ToolStripDropDown)
            {
                RegionHelper.CreateRegion(toolStrip, bounds);
                using (SolidBrush brush = new SolidBrush(ColorTable.BackNormal))
                {
                    g.FillRectangle(brush, bounds);
                }
            }
            else
            {
                LinearGradientMode mode =
                    toolStrip.Orientation == Orientation.Horizontal ?
                    LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                RenderHelper.RenderBackgroundInternal(
                    g,
                    bounds,
                    ColorTable.Base,
                    ColorTable.Border,
                    ColorTable.BackNormal,
                    RoundStyle.None,
                    0,
                    .35f,
                    false,
                    false,
                    mode);
            }
        }

        protected override void OnRenderImageMargin(
            ToolStripRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            Rectangle bounds = e.AffectedBounds;
            if (toolStrip is ToolStripDropDown)
            {
                bool bDrawLogo = NeedDrawLogo(toolStrip);
                bool bRightToLeft = toolStrip.RightToLeft == RightToLeft.Yes;

                Rectangle imageBackRect = bounds;
                imageBackRect.Width = OffsetMargin;

                if (bDrawLogo)
                {
                    Rectangle logoRect = bounds;
                    logoRect.Width = OffsetMargin;
                    if (bRightToLeft)
                    {
                        logoRect.X -= 2;
                        imageBackRect.X = logoRect.X - OffsetMargin;
                    }
                    else
                    {
                        logoRect.X += 2;
                        imageBackRect.X = logoRect.Right;
                    }
                    logoRect.Y += 1;
                    logoRect.Height -= 2;

                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        logoRect,
                        ColorTable.BackHover,
                        ColorTable.BackNormal,
                        90f))
                    {
                        Blend blend = new Blend();
                        blend.Positions = new float[] { 0f, .2f, 1f };
                        blend.Factors = new float[] { 0f, 0.1f, .9f };
                        brush.Blend = blend;
                        logoRect.Y += 1;
                        logoRect.Height -= 2;
                        using (GraphicsPath path =
                            GraphicsPathHelper.CreatePath(logoRect, 8, RoundStyle.All, false))
                        {
                            using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
                            {
                                g.FillPath(brush, path);
                            }
                        }
                    }

                    StringFormat sf = new StringFormat(StringFormatFlags.NoWrap);
                    Font font = new Font(
                        toolStrip.Font.FontFamily, 11, FontStyle.Bold);
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Trimming = StringTrimming.EllipsisCharacter;

                    g.TranslateTransform(logoRect.X, logoRect.Bottom);
                    g.RotateTransform(270f);

                    if (!string.IsNullOrEmpty(MenuLogoString))
                    {
                        Rectangle newRect = new Rectangle(
                            0, 0, logoRect.Height, logoRect.Width);

                        using (Brush brush = new SolidBrush(ColorTable.Fore))
                        {
                            using (TextRenderingHintGraphics tg = 
                                new TextRenderingHintGraphics(g))
                            {
                                g.DrawString(
                                    MenuLogoString,
                                    font,
                                    brush,
                                    newRect,
                                    sf);
                            }
                        }
                    }

                    g.ResetTransform();
                }
                else
                {
                    if (bRightToLeft)
                    {
                        imageBackRect.X -= 3;
                    }
                    else
                    {
                        imageBackRect.X += 3;
                    }
                }

                imageBackRect.Y += 2;
                imageBackRect.Height -= 4;
                using (SolidBrush brush = new SolidBrush(ColorTable.DropDownImageBack))
                {
                    g.FillRectangle(brush, imageBackRect);
                }

                Point ponitStart;
                Point pointEnd;
                if (bRightToLeft)
                {
                    ponitStart = new Point(imageBackRect.X, imageBackRect.Y);
                    pointEnd = new Point(imageBackRect.X, imageBackRect.Bottom);
                }
                else
                {
                    ponitStart = new Point(imageBackRect.Right - 1, imageBackRect.Y);
                    pointEnd = new Point(imageBackRect.Right - 1, imageBackRect.Bottom);
                }

                using (Pen pen = new Pen(ColorTable.DropDownImageSeparator))
                {
                    g.DrawLine(pen, ponitStart, pointEnd);
                }
            }
            else
            {
                base.OnRenderImageMargin(e);
            }
        }

        protected override void OnRenderToolStripBorder(
            ToolStripRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            Rectangle bounds = e.AffectedBounds;

            if (toolStrip is ToolStripDropDown)
            {
                using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
                {
                    using (GraphicsPath path =
                        GraphicsPathHelper.CreatePath(bounds, 8, RoundStyle.All, true))
                    {
                        using (Pen pen = new Pen(ColorTable.DropDownImageSeparator))
                        {
                            path.Widen(pen);
                            g.DrawPath(pen, path);
                        }
                    }
                }

                if (!(toolStrip is ToolStripOverflow))
                {
                    bounds.Inflate(-1, -1);
                    using (GraphicsPath innerPath = GraphicsPathHelper.CreatePath(
                        bounds, 8, RoundStyle.All, true))
                    {
                        using (Pen pen = new Pen(ColorTable.BackNormal))
                        {
                            g.DrawPath(pen, innerPath);
                        }
                    }
                }
            }
            else
            {
                base.OnRenderToolStripBorder(e);
            }
        }

        protected override void OnRenderMenuItemBackground(
           ToolStripItemRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripItem item = e.Item;

            if (!item.Enabled)
            {
                return;
            }

            Graphics g = e.Graphics;
            Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);

            if (toolStrip is MenuStrip)
            {
                LinearGradientMode mode =
                    toolStrip.Orientation == Orientation.Horizontal ?
                    LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                if (item.Selected)
                {
                    RenderHelper.RenderBackgroundInternal(
                        g,
                        rect,
                        ColorTable.BackHover,
                        ColorTable.Border,
                        ColorTable.BackNormal,
                        RoundStyle.All,
                        true,
                        true,
                        mode);
                }
                else if (item.Pressed)
                {
                    RenderHelper.RenderBackgroundInternal(
                       g,
                       rect,
                       ColorTable.BackPressed,
                       ColorTable.Border,
                       ColorTable.BackNormal,
                       RoundStyle.All,
                       true,
                       true,
                       mode);
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
            }
            else if (toolStrip is ToolStripDropDown)
            {
                bool bDrawLogo = NeedDrawLogo(toolStrip);

                int offsetMargin = bDrawLogo ? OffsetMargin : 0;

                if (item.RightToLeft == RightToLeft.Yes)
                {
                    rect.X += 4;
                }
                else
                {
                    rect.X += offsetMargin + 4;
                }
                rect.Width -= offsetMargin + 8;
                rect.Height--;

                if (item.Selected)
                {
                    RenderHelper.RenderBackgroundInternal(
                       g,
                       rect,
                       ColorTable.BackHover,
                       ColorTable.Border,
                       ColorTable.BackNormal,
                       RoundStyle.All,
                       true,
                       true,
                       LinearGradientMode.Vertical);
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }

        protected override void OnRenderItemImage(
            ToolStripItemImageRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;

            if (toolStrip is ToolStripDropDown &&
               e.Item is ToolStripMenuItem)
            {
                bool bDrawLogo = NeedDrawLogo(toolStrip);
                int offsetMargin = bDrawLogo ? OffsetMargin : 0;

                ToolStripMenuItem item = (ToolStripMenuItem)e.Item;
                if (item.Checked)
                {
                    return;
                }
                Rectangle rect = e.ImageRectangle;
                if (e.Item.RightToLeft == RightToLeft.Yes)
                {
                    rect.X -= offsetMargin + 2;
                }
                else
                {
                    rect.X += offsetMargin + 2;
                }

                using (InterpolationModeGraphics ig = 
                    new InterpolationModeGraphics(g))
                {
                    ToolStripItemImageRenderEventArgs ne =
                        new ToolStripItemImageRenderEventArgs(
                        g, e.Item, e.Image, rect);
                    base.OnRenderItemImage(ne);
                }
            }
            else
            {
                base.OnRenderItemImage(e);
            }
        }

        protected override void OnRenderItemText(
            ToolStripItemTextRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;

            e.TextColor = ColorTable.Fore;

            if (toolStrip is ToolStripDropDown &&
                e.Item is ToolStripMenuItem)
            {
                bool bDrawLogo = NeedDrawLogo(toolStrip);

                int offsetMargin = bDrawLogo ? 18 : 0;

                Rectangle rect = e.TextRectangle;
                if (e.Item.RightToLeft == RightToLeft.Yes)
                {
                    rect.X -= offsetMargin;
                }
                else
                {
                    rect.X += offsetMargin;
                }
                e.TextRectangle = rect;
            }

            base.OnRenderItemText(e);
        }

        protected override void OnRenderItemCheck(
            ToolStripItemImageRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;

            if (toolStrip is ToolStripDropDown &&
               e.Item is ToolStripMenuItem)
            {
                bool bDrawLogo = NeedDrawLogo(toolStrip);
                int offsetMargin = bDrawLogo ? OffsetMargin : 0;
                Rectangle rect = e.ImageRectangle;

                if (e.Item.RightToLeft == RightToLeft.Yes)
                {
                    rect.X -= offsetMargin + 2;
                }
                else
                {
                    rect.X += offsetMargin + 2;
                }

                rect.Width = 13;
                rect.Y += 1;
                rect.Height -= 3;

                using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
                {
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddRectangle(rect);
                        using (PathGradientBrush brush = new PathGradientBrush(path))
                        {
                            brush.CenterColor = ColorTable.DropDownImageBack;
                            brush.SurroundColors = new Color[] { ColorTable.BackPressed };
                            Blend blend = new Blend();
                            blend.Positions = new float[] { 0f, 0.3f, 1f };
                            blend.Factors = new float[] { 0f, 0.5f, 1f };
                            brush.Blend = blend;
                            g.FillRectangle(brush, rect);
                        }
                    }

                    using (Pen pen = new Pen(ColorTable.BackPressed))
                    {
                        g.DrawRectangle(pen, rect);
                    }

                    ControlPaintEx.DrawCheckedFlag(g, rect, ColorTable.Fore);
                }
            }
            else
            {
                base.OnRenderItemCheck(e);
            }
        }

        protected override void OnRenderArrow(
            ToolStripArrowRenderEventArgs e)
        {
            if (e.Item.Enabled)
            {
                e.ArrowColor = ColorTable.Fore;
            }

            ToolStrip toolStrip = e.Item.Owner;

            if (toolStrip is ToolStripDropDown &&
                e.Item is ToolStripMenuItem)
            {
                bool bDrawLogo = NeedDrawLogo(toolStrip);

                int offsetMargin = bDrawLogo ? 3 : 0;

                Rectangle rect = e.ArrowRectangle;
                if (e.Item.RightToLeft == RightToLeft.Yes)
                {
                    rect.X -= offsetMargin;
                }
                else
                {
                    rect.X += offsetMargin;
                }

                e.ArrowRectangle = rect;
            }

            base.OnRenderArrow(e);
        }

        protected override void OnRenderSeparator(
            ToolStripSeparatorRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Rectangle rect = e.Item.ContentRectangle;
            Graphics g = e.Graphics;

            Color baseColor = ColorTable.Base;

            if (toolStrip is ToolStripDropDown)
            {
                bool bDrawLogo = NeedDrawLogo(toolStrip);

                int offsetMargin = bDrawLogo ? 
                    OffsetMargin * 2 : OffsetMargin;

                if (e.Item.RightToLeft != RightToLeft.Yes)
                {
                    rect.X += offsetMargin + 2;
                }
                rect.Width -= offsetMargin + 4;

                baseColor = ColorTable.DropDownImageSeparator;
            }

            RenderSeparatorLine(
               g,
               rect,
               baseColor,
               ColorTable.BackNormal,
               Color.Snow,
               e.Vertical);
        }

        protected override void OnRenderButtonBackground(
            ToolStripItemRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripButton item = e.Item as ToolStripButton;
            Graphics g = e.Graphics;

            if (item != null)
            {
                LinearGradientMode mode =
                    toolStrip.Orientation == Orientation.Horizontal ?
                    LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                SmoothingModeGraphics sg = new SmoothingModeGraphics(g);
                Rectangle bounds = new Rectangle(Point.Empty, item.Size);

                if (item.BackgroundImage != null)
                {
                    Rectangle clipRect = item.Selected ? item.ContentRectangle : bounds;
                    ControlPaintEx.DrawBackgroundImage(
                        g,
                        item.BackgroundImage,
                        ColorTable.BackNormal,
                        item.BackgroundImageLayout,
                        bounds,
                        clipRect);
                }

                if (item.CheckState == CheckState.Unchecked)
                {
                    if (item.Selected)
                    {
                        Color color = ColorTable.BackHover;
                        if (item.Pressed)
                        {
                            color = ColorTable.BackPressed;
                        }
                        RenderHelper.RenderBackgroundInternal(
                            g,
                            bounds,
                            color,
                            ColorTable.Border,
                            ColorTable.BackNormal,
                            RoundStyle.All,
                            true,
                            true,
                            mode);
                    }
                    else
                    {
                        if (toolStrip is ToolStripOverflow)
                        {
                            using (Brush brush = new SolidBrush(ColorTable.BackHover))
                            {
                                g.FillRectangle(brush, bounds);
                            }
                        }
                    }
                }
                else
                {
                    Color color = ControlPaint.Light(ColorTable.BackHover);
                    if (item.Selected)
                    {
                        color = ColorTable.BackHover;
                    }
                    if (item.Pressed)
                    {
                        color = ColorTable.BackPressed;
                    }
                    RenderHelper.RenderBackgroundInternal(
                        g,
                        bounds,
                        color,
                        ColorTable.Border,
                        ColorTable.BackNormal,
                        RoundStyle.All,
                        true,
                        true,
                        mode);
                }

                sg.Dispose();
            }
        }

        protected override void OnRenderDropDownButtonBackground(
            ToolStripItemRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripDropDownItem item = e.Item as ToolStripDropDownItem;

            if (item != null)
            {
                LinearGradientMode mode =
                   toolStrip.Orientation == Orientation.Horizontal ?
                   LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                Graphics g = e.Graphics;
                SmoothingModeGraphics sg = new SmoothingModeGraphics(g);
                Rectangle bounds = new Rectangle(Point.Empty, item.Size);

                if (item.Pressed && item.HasDropDownItems)
                {
                    RenderHelper.RenderBackgroundInternal(
                      g,
                      bounds,
                      ColorTable.BackPressed,
                      ColorTable.Border,
                      ColorTable.BackNormal,
                      RoundStyle.All,
                      true,
                      true,
                      mode);
                }
                else if (item.Selected)
                {
                    RenderHelper.RenderBackgroundInternal(
                      g,
                      bounds,
                      ColorTable.BackHover,
                      ColorTable.Border,
                      ColorTable.BackNormal,
                      RoundStyle.All,
                      true,
                      true,
                      mode);
                }
                else if (toolStrip is ToolStripOverflow)
                {
                    using (Brush brush = new SolidBrush(ColorTable.BackNormal))
                    {
                        g.FillRectangle(brush, bounds);
                    }
                }
                else
                {
                    base.OnRenderDropDownButtonBackground(e);
                }

                sg.Dispose();
            }
        }

        protected override void OnRenderSplitButtonBackground(
            ToolStripItemRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripSplitButton item = e.Item as ToolStripSplitButton;

            if (item != null)
            {
                Graphics g = e.Graphics;
                LinearGradientMode mode =
                    toolStrip.Orientation == Orientation.Horizontal ?
                    LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                Rectangle bounds = new Rectangle(Point.Empty, item.Size);
                SmoothingModeGraphics sg = new SmoothingModeGraphics(g);

                Color arrowColor = toolStrip.Enabled ?
                    ColorTable.Fore : SystemColors.ControlDark;

                if (item.BackgroundImage != null)
                {
                    Rectangle clipRect = item.Selected ? item.ContentRectangle : bounds;
                    ControlPaintEx.DrawBackgroundImage(
                        g,
                        item.BackgroundImage,
                        ColorTable.BackNormal,
                        item.BackgroundImageLayout,
                        bounds,
                        clipRect);
                }

                if (item.ButtonPressed)
                {
                    Rectangle buttonBounds = item.ButtonBounds;
                    Padding padding = (item.RightToLeft == RightToLeft.Yes) ?
                        new Padding(0, 1, 1, 1) : new Padding(1, 1, 0, 1);
                    buttonBounds = LayoutUtils.DeflateRect(buttonBounds, padding);
                    RenderHelper.RenderBackgroundInternal(
                       g,
                       bounds,
                       ColorTable.BackHover,
                       ColorTable.Border,
                       ColorTable.BackNormal,
                       RoundStyle.All,
                       true,
                       true,
                       mode);

                    buttonBounds.Inflate(-1, -1);
                    g.SetClip(buttonBounds);
                    RenderHelper.RenderBackgroundInternal(
                       g,
                       buttonBounds,
                       ColorTable.BackPressed,
                       ColorTable.Border,
                       ColorTable.BackNormal,
                       RoundStyle.Left,
                       false,
                       true,
                       mode);
                    g.ResetClip();

                    using (Pen pen = new Pen(ColorTable.Border))
                    {
                        g.DrawLine(
                            pen,
                            item.SplitterBounds.Left,
                            item.SplitterBounds.Top,
                            item.SplitterBounds.Left,
                            item.SplitterBounds.Bottom);
                    }
                    base.DrawArrow(
                        new ToolStripArrowRenderEventArgs(
                        g,
                        item,
                        item.DropDownButtonBounds,
                        arrowColor,
                        ArrowDirection.Down));
                    return;
                }

                if (item.Pressed || item.DropDownButtonPressed)
                {
                    RenderHelper.RenderBackgroundInternal(
                      g,
                      bounds,
                      ColorTable.BackPressed,
                      ColorTable.Border,
                      ColorTable.BackNormal,
                      RoundStyle.All,
                      true,
                      true,
                      mode);
                    base.DrawArrow(
                       new ToolStripArrowRenderEventArgs(
                       g,
                       item,
                       item.DropDownButtonBounds,
                       arrowColor,
                       ArrowDirection.Down));
                    return;
                }

                if (item.Selected)
                {
                    RenderHelper.RenderBackgroundInternal(
                      g,
                      bounds,
                      ColorTable.BackHover,
                      ColorTable.Border,
                      ColorTable.BackNormal,
                      RoundStyle.All,
                      true,
                      true,
                      mode);
                    using (Pen pen = new Pen(ColorTable.Border))
                    {
                        g.DrawLine(
                           pen,
                           item.SplitterBounds.Left,
                           item.SplitterBounds.Top,
                           item.SplitterBounds.Left,
                           item.SplitterBounds.Bottom);
                    }
                    base.DrawArrow(
                        new ToolStripArrowRenderEventArgs(
                        g,
                        item,
                        item.DropDownButtonBounds,
                        arrowColor,
                        ArrowDirection.Down));
                    return;
                }

                base.DrawArrow(
                   new ToolStripArrowRenderEventArgs(
                   g,
                   item,
                   item.DropDownButtonBounds,
                   arrowColor,
                   ArrowDirection.Down));
                return;
            }

            base.OnRenderSplitButtonBackground(e);
        }

        protected override void OnRenderOverflowButtonBackground(
            ToolStripItemRenderEventArgs e)
        {
            ToolStripItem item = e.Item;
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            bool rightToLeft = item.RightToLeft == RightToLeft.Yes;

            SmoothingModeGraphics sg = new SmoothingModeGraphics(g);

            RenderOverflowBackground(e, rightToLeft);

            bool bHorizontal = toolStrip.Orientation == Orientation.Horizontal;
            Rectangle empty = Rectangle.Empty;

            if (rightToLeft)
            {
                empty = new Rectangle(0, item.Height - 8, 10, 5);
            }
            else
            {
                empty = new Rectangle(item.Width - 12, item.Height - 8, 10, 5);
            }
            ArrowDirection direction = bHorizontal ? 
                ArrowDirection.Down : ArrowDirection.Right;
            int x = (rightToLeft && bHorizontal) ? -1 : 1;
            empty.Offset(x, 1);

            Color arrowColor = toolStrip.Enabled ?
                ColorTable.Fore : SystemColors.ControlDark;

            using (Brush brush = new SolidBrush(arrowColor))
            {
                RenderHelper.RenderArrowInternal(g, empty, direction, brush);
            }

            if (bHorizontal)
            {
                using (Pen pen = new Pen(arrowColor))
                {
                    g.DrawLine(
                        pen,
                        empty.Right - 8,
                        empty.Y - 2,
                        empty.Right - 2,
                        empty.Y - 2);
                    g.DrawLine(
                        pen, 
                        empty.Right - 8,
                        empty.Y - 1, 
                        empty.Right - 2, 
                        empty.Y - 1);
                }
            }
            else
            {
                using (Pen pen = new Pen(arrowColor))
                {
                    g.DrawLine(
                        pen, 
                        empty.X, 
                        empty.Y, 
                        empty.X, 
                        empty.Bottom - 1);
                    g.DrawLine(
                        pen, 
                        empty.X, 
                        empty.Y + 1,
                        empty.X, 
                        empty.Bottom);
                }
            }
        }

        protected override void OnRenderGrip(
            ToolStripGripRenderEventArgs e)
        {
            if (e.GripStyle == ToolStripGripStyle.Visible)
            {
                Rectangle bounds = e.GripBounds;
                bool vert = e.GripDisplayStyle == ToolStripGripDisplayStyle.Vertical;
                ToolStrip toolStrip = e.ToolStrip;
                Graphics g = e.Graphics;

                if (vert)
                {
                    bounds.X = e.AffectedBounds.X;
                    bounds.Width = e.AffectedBounds.Width;
                    if (toolStrip is MenuStrip)
                    {
                        if (e.AffectedBounds.Height > e.AffectedBounds.Width)
                        {
                            vert = false;
                            bounds.Y = e.AffectedBounds.Y;
                        }
                        else
                        {
                            toolStrip.GripMargin = new Padding(0, 2, 0, 2);
                            bounds.Y = e.AffectedBounds.Y;
                            bounds.Height = e.AffectedBounds.Height;
                        }
                    }
                    else
                    {
                        toolStrip.GripMargin = new Padding(2, 2, 4, 2);
                        bounds.X++;
                        bounds.Width++;
                    }
                }
                else
                {
                    bounds.Y = e.AffectedBounds.Y;
                    bounds.Height = e.AffectedBounds.Height;
                }

                DrawDottedGrip(
                    g, 
                    bounds, 
                    vert, 
                    false, 
                    ColorTable.BackNormal,
                    ControlPaint.Dark(ColorTable.Base, 0.3F));
            }
        }

        protected override void OnRenderStatusStripSizingGrip(
            ToolStripRenderEventArgs e)
        {
           DrawSolidStatusGrip(
               e.Graphics, 
               e.AffectedBounds,
               ColorTable.BackNormal,
               ControlPaint.Dark(ColorTable.Base, 0.3f));
        }

        internal void RenderSeparatorLine(
            Graphics g,
            Rectangle rect,
            Color baseColor,
            Color backColor,
            Color shadowColor,
            bool vertical)
        {
            if (vertical)
            {
                rect.Y += 2;
                rect.Height -= 4;
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect,
                    baseColor,
                    backColor,
                    LinearGradientMode.Vertical))
                {
                    using (Pen pen = new Pen(brush))
                    {
                        g.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom);
                    }
                }
            }
            else
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect,
                    baseColor,
                    backColor,
                    180F))
                {
                    Blend blend = new Blend();
                    blend.Positions = new float[] { 0f, .2f, .5f, .8f, 1f };
                    blend.Factors = new float[] { 1f, .3f, 0f, .3f, 1f };
                    brush.Blend = blend;
                    using (Pen pen = new Pen(brush))
                    {
                        g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);

                        brush.LinearColors = new Color[] {
                        shadowColor, backColor };
                        pen.Brush = brush;
                        g.DrawLine(pen, rect.X, rect.Y + 1, rect.Right, rect.Y + 1);
                    }
                }
            }
        }

        internal void RenderOverflowBackground(
            ToolStripItemRenderEventArgs e,
            bool rightToLeft)
        {
            Color color = Color.Empty;
            Graphics g = e.Graphics;
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripOverflowButton item = e.Item as ToolStripOverflowButton;
            Rectangle bounds = new Rectangle(Point.Empty, item.Size);
            Rectangle withinBounds = bounds;
            bool bParentIsMenuStrip = !(item.GetCurrentParent() is MenuStrip);
            bool bHorizontal = toolStrip.Orientation == Orientation.Horizontal;

            if (bHorizontal)
            {
                bounds.X += (bounds.Width - 12) + 1;
                bounds.Width = 12;
                if (rightToLeft)
                {
                    bounds = LayoutUtils.RTLTranslate(bounds, withinBounds);
                }
            }
            else
            {
                bounds.Y = (bounds.Height - 12) + 1;
                bounds.Height = 12;
            }

            if (item.Pressed)
            {
                color = ColorTable.BackPressed;
            }
            else if (item.Selected)
            {
                color = ColorTable.BackHover;
            }
            else
            {
                color = ColorTable.Base;
            }
            if (bParentIsMenuStrip)
            {
                using (Pen pen = new Pen(ColorTable.Base))
                {
                    Point point = new Point(bounds.Left - 1, bounds.Height - 2);
                    Point point2 = new Point(bounds.Left, bounds.Height - 2);
                    if (rightToLeft)
                    {
                        point.X = bounds.Right + 1;
                        point2.X = bounds.Right;
                    }
                    g.DrawLine(pen, point, point2);
                }
            }

            LinearGradientMode mode = bHorizontal ? 
                LinearGradientMode.Vertical : LinearGradientMode.Horizontal;

            RenderHelper.RenderBackgroundInternal(
                g,
                bounds,
                color,
                ColorTable.Border,
                ColorTable.BackNormal,
                RoundStyle.None,
                0,
                .35f,
                false,
                false,
                mode);

            if (bParentIsMenuStrip)
            {
                using (Brush brush = new SolidBrush(ColorTable.Base))
                {
                    if (bHorizontal)
                    {
                        Point point3 = new Point(bounds.X - 2, 0);
                        Point point4 = new Point(bounds.X - 1, 1);
                        if (rightToLeft)
                        {
                            point3.X = bounds.Right + 1;
                            point4.X = bounds.Right;
                        }
                        g.FillRectangle(brush, point3.X, point3.Y, 1, 1);
                        g.FillRectangle(brush, point4.X, point4.Y, 1, 1);
                    }
                    else
                    {
                        g.FillRectangle(brush, bounds.Width - 3, bounds.Top - 1, 1, 1);
                        g.FillRectangle(brush, bounds.Width - 2, bounds.Top - 2, 1, 1);
                    }
                }
                using (Brush brush = new SolidBrush(ColorTable.Base))
                {
                    if (bHorizontal)
                    {
                        Rectangle rect = new Rectangle(bounds.X - 1, 0, 1, 1);
                        if (rightToLeft)
                        {
                            rect.X = bounds.Right;
                        }
                        g.FillRectangle(brush, rect);
                    }
                    else
                    {
                        g.FillRectangle(brush, bounds.X, bounds.Top - 1, 1, 1);
                    }
                }
            }
        }

        private void DrawDottedGrip(
            Graphics g, 
            Rectangle bounds, 
            bool vertical,
            bool largeDot,
            Color innerColor,
            Color outerColor)
        {
            bounds.Height -= 3;
            Point position = new Point(bounds.X, bounds.Y);
            int sep;
            Rectangle posRect = new Rectangle(0, 0, 2, 2);

            using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
            {
                IntPtr hdc;

                if (vertical)
                {
                    sep = bounds.Height;
                    position.Y += 8;
                    for (int i = 0; position.Y > 4; i += 4)
                    {
                        position.Y = sep - (2 + i);
                        if (largeDot)
                        {
                            posRect.Location = position;
                            DrawCircle(
                                g, 
                                posRect, 
                                outerColor, 
                                innerColor);
                        }
                        else
                        {
                            int innerWin32Corlor = ColorTranslator.ToWin32(innerColor);
                            int outerWin32Corlor = ColorTranslator.ToWin32(outerColor);

                            hdc = g.GetHdc();

                            SetPixel(
                                hdc, 
                                position.X,
                                position.Y,
                                innerWin32Corlor);
                            SetPixel(
                                hdc, 
                                position.X + 1, 
                                position.Y,
                                outerWin32Corlor);
                            SetPixel(
                                hdc, 
                                position.X, 
                                position.Y + 1,
                                outerWin32Corlor);

                            SetPixel(
                                hdc, 
                                position.X + 3, 
                                position.Y,
                                innerWin32Corlor);
                            SetPixel(
                                hdc, 
                                position.X + 4, 
                                position.Y,
                                outerWin32Corlor);
                            SetPixel(
                                hdc, 
                                position.X + 3,
                                position.Y + 1,
                                outerWin32Corlor);

                            g.ReleaseHdc(hdc);
                        }
                    }
                }
                else
                {
                    bounds.Inflate(-2, 0);
                    sep = bounds.Width;
                    position.X += 2;

                    for (int i = 1; position.X > 0; i += 4)
                    {
                        position.X = sep - (2 + i);
                        if (largeDot)
                        {
                            posRect.Location = position;
                            DrawCircle(g, posRect, outerColor, innerColor);
                        }
                        else
                        {
                            int innerWin32Corlor = ColorTranslator.ToWin32(innerColor);
                            int outerWin32Corlor = ColorTranslator.ToWin32(outerColor);
                            hdc = g.GetHdc();

                            SetPixel(
                                hdc, 
                                position.X, 
                                position.Y, 
                                innerWin32Corlor);
                            SetPixel(
                                hdc, 
                                position.X + 1, 
                                position.Y, 
                                outerWin32Corlor);
                            SetPixel(
                                hdc, 
                                position.X,
                                position.Y + 1, 
                                outerWin32Corlor);

                            SetPixel(
                                hdc,
                                position.X + 3, 
                                position.Y, 
                                innerWin32Corlor);
                            SetPixel(
                                hdc,
                                position.X + 4,
                                position.Y, 
                                outerWin32Corlor);
                            SetPixel(
                                hdc, 
                                position.X + 3, 
                                position.Y + 1, 
                                outerWin32Corlor);

                            g.ReleaseHdc(hdc);
                        }
                    }
                }
            }
        }

        private void DrawCircle(
            Graphics g, 
            Rectangle bounds, 
            Color borderColor, 
            Color fillColor)
        {
            using (GraphicsPath circlePath = new GraphicsPath())
            {
                circlePath.AddEllipse(bounds);
                circlePath.CloseFigure();

                using (Pen borderPen = new Pen(borderColor))
                {
                    g.DrawPath(borderPen, circlePath);
                }

                using (Brush backBrush = new SolidBrush(fillColor))
                {
                    g.FillPath(backBrush, circlePath);
                }
            }
        }

        private void DrawDottedStatusGrip(
            Graphics g, 
            Rectangle bounds,
            Color innerColor,
            Color outerColor)
        {
            Rectangle shape = new Rectangle(0, 0, 2, 2);
            shape.X = bounds.Width - 17;
            shape.Y = bounds.Height - 8;
            using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
            {
                DrawCircle(g, shape, outerColor, innerColor);

                shape.X = bounds.Width - 12;
                DrawCircle(g, shape, outerColor, innerColor);

                shape.X = bounds.Width - 7;
                DrawCircle(g, shape, outerColor, innerColor);

                shape.Y = bounds.Height - 13;
                DrawCircle(g, shape, outerColor, innerColor);

                shape.Y = bounds.Height - 18;
                DrawCircle(g, shape, outerColor, innerColor);

                shape.Y = bounds.Height - 13;
                shape.X = bounds.Width - 12;
                DrawCircle(g, shape, outerColor, innerColor);
            }
        }

        private void DrawSolidStatusGrip(
            Graphics g, 
            Rectangle bounds,
            Color innerColor,
            Color outerColor
            )
        {
            using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
            {
                using (Pen innerPen = new Pen(innerColor),
                    outerPen = new Pen(outerColor))
                {
                    //outer line
                    g.DrawLine(
                        outerPen, 
                        new Point(bounds.Width - 14, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 16));
                    g.DrawLine(
                        innerPen, 
                        new Point(bounds.Width - 13, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 15));
                    // line
                    g.DrawLine(
                        outerPen, 
                        new Point(bounds.Width - 12, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 14));
                    g.DrawLine(
                        innerPen, 
                        new Point(bounds.Width - 11, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 13));
                    // line
                    g.DrawLine(
                        outerPen, 
                        new Point(bounds.Width - 10, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 12));
                    g.DrawLine(
                        innerPen, 
                        new Point(bounds.Width - 9, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 11));
                    // line
                    g.DrawLine(
                        outerPen, 
                        new Point(bounds.Width - 8, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 10));
                    g.DrawLine(
                        innerPen, 
                        new Point(bounds.Width - 7, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 9));
                    // inner line
                    g.DrawLine(
                        outerPen, 
                        new Point(bounds.Width - 6, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 8));
                    g.DrawLine(
                        innerPen, 
                        new Point(bounds.Width - 5, bounds.Height - 6),
                        new Point(bounds.Width - 4, bounds.Height - 7));
                }
            }
        }

        internal bool NeedDrawLogo(ToolStrip toolStrip)
        {
            ToolStripDropDown dropDown = toolStrip as ToolStripDropDown;
            bool bDrawLogo =
                (dropDown.OwnerItem != null &&
                dropDown.OwnerItem.Owner is MenuStrip) ||
                (toolStrip is ContextMenuStrip);
            return bDrawLogo;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern uint SetPixel(IntPtr hdc, int X, int Y, int crColor);
    }
}
