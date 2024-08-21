using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;
using System.Drawing.Drawing2D;

namespace RUINORERP.UI.BaseUControls
{
    public partial class KtabPage2 : UserControl
    {
        private bool _mouseOver;
        private bool _mouseDown;
        private IPalette _palette;
        public KtabPage2()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw, true);

            // Cache the current global palette setting
            _palette = KryptonManager.CurrentGlobalPalette;

            // Hook into palette events
            if (_palette != null)
                _palette.PalettePaint += new EventHandler<PaletteLayoutEventArgs>(OnPalettePaint);

            // We want to be notified whenever the global palette changes
            KryptonManager.GlobalPaletteChanged += new EventHandler(OnGlobalPaletteChanged);
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Unhook from the palette events
                if (_palette != null)
                {
                    _palette.PalettePaint -= new EventHandler<PaletteLayoutEventArgs>(OnPalettePaint);
                    _palette = null;
                }

                // Unhook from the static events, otherwise we cannot be garbage collected
                KryptonManager.GlobalPaletteChanged -= new EventHandler(OnGlobalPaletteChanged);
            }

            base.Dispose(disposing);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _mouseOver = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _mouseDown = (e.Button == MouseButtons.Left);
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _mouseDown = _mouseDown && (e.Button != MouseButtons.Left);
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _mouseOver = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
          
            if (_palette != null)
            {
                // Save the original state, so any changes we make are easy to undo
                ////保存原始状态，这样我们所做的任何更改都很容易撤消
                GraphicsState state = e.Graphics.Save();

                // We want the inner part of the control to act like a button, so 
                // we need to find the correct palette state based on if the mouse 
                // is over the control and currently being pressed down or not.
                //我们希望控件的内部像按钮一样，因此

                //我们需要根据鼠标是否

                //在控件上，当前是否按下
                PaletteState buttonState = GetButtonState();

                /////////////////////////////////////////////////////

                //获取绘制所需的各种调色板细节//

                //我们的鱼在我们实施的各个州//

                /////////////////////////////////////////////////////

                // Get the two colors and angle used to draw the control background
                Color backColor1 = _palette.GetBackColor1(PaletteBackStyle.PanelAlternate, Enabled ? PaletteState.Normal : PaletteState.Disabled);
                Color backColor2 = _palette.GetBackColor2(PaletteBackStyle.PanelAlternate, Enabled ? PaletteState.Normal : PaletteState.Disabled);
                float backColorAngle = _palette.GetBackColorAngle(PaletteBackStyle.PanelAlternate, Enabled ? PaletteState.Normal : PaletteState.Disabled);

                // Get the two colors and angle used to draw the fish area background
                Color fillColor1 = _palette.GetBackColor1(PaletteBackStyle.ButtonStandalone, buttonState);
                Color fillColor2 = _palette.GetBackColor2(PaletteBackStyle.ButtonStandalone, buttonState);
                float fillColorAngle = _palette.GetBackColorAngle(PaletteBackStyle.ButtonStandalone, buttonState);

                // Get the color used to draw the fish border
                Color borderColor = _palette.GetBorderColor1(PaletteBorderStyle.ButtonStandalone, buttonState);

                // Get the color and font used to draw the text
                Color textColor = _palette.GetContentShortTextColor1(PaletteContentStyle.ButtonStandalone, buttonState);
                Font textFont = _palette.GetContentShortTextFont(PaletteContentStyle.ButtonStandalone, buttonState);

                /////////////////////////////////////////////////////

                //使用调色板值执行实际绘图//

                /////////////////////////////////////////////////////

                //填充图形路径以描述要绘制的形状
                using (GraphicsPath path = CreateFishPath())
                {
                    // We want to anti alias the drawing for nice smooth curves
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    // Fill the entire background in the control background color
                    using (Brush backBrush = new LinearGradientBrush(ClientRectangle, backColor1, backColor2, backColorAngle))
                        e.Graphics.FillRectangle(backBrush, e.ClipRectangle);

                    // Fill the entire fish background using a gradient
                    using (Brush fillBrush = new LinearGradientBrush(ClientRectangle, fillColor1, fillColor2, fillColorAngle))
                        e.Graphics.FillPath(fillBrush, path);

                    // Draw the fish border using a single color
                    using (Pen borderPen = new Pen(borderColor))
                        e.Graphics.DrawPath(borderPen, path);

                    // Draw the text in about the center of the control
                    using (Brush textBrush = new SolidBrush(textColor))
                        e.Graphics.DrawString("Click me!", textFont, textBrush, Width / 2 - 10, Height / 2 - 5);
                }

                // Put graphics back into original state before returning
                e.Graphics.Restore(state);
            }
          


            base.OnPaint(e);
        }

        private PaletteState GetButtonState()
        {
            // Find the correct state when getting button values
            if (!Enabled)
                return PaletteState.Disabled;
            else
            {
                if (_mouseOver)
                {
                    if (_mouseDown)
                        return PaletteState.Pressed;
                    else
                        return PaletteState.Tracking;
                }
                else
                    return PaletteState.Normal;
            }
        }

        private GraphicsPath CreateFishPath()
        {
            // The bounding box for the fish is slightly smaller than the client area
            Rectangle fishRect = ClientRectangle;
            fishRect.Inflate(-5, -5);

            // Find some lengths
            int w3 = Width / 3;
            int w6 = Width / 6;
            int h2 = Height / 2;
            int h4 = Height / 4;

            GraphicsPath fishPath = new GraphicsPath();

            // Create the tail of the fish
            fishPath.AddLine(fishRect.Left + w6, fishRect.Bottom - h4, fishRect.Left, fishRect.Bottom);
            fishPath.AddLine(fishRect.Left, fishRect.Bottom, fishRect.Left, fishRect.Top);
            fishPath.AddLine(fishRect.Left, fishRect.Top, fishRect.Left + w6, fishRect.Top + h4);

            // Create the curving body of the fish
            fishPath.AddCurve(new Point[]{ new Point(fishRect.Left + w6, fishRect.Top + h4),
                                           new Point(fishRect.Right - w3, fishRect.Top),
                                           new Point(fishRect.Right, fishRect.Top + h2),
                                           new Point(fishRect.Right - w3, fishRect.Bottom),
                                           new Point(fishRect.Left + w6, fishRect.Bottom - h4)}, 0.8f);

            return fishPath;
        }

        private void OnGlobalPaletteChanged(object sender, EventArgs e)
        {
            // Unhook events from old palette
            if (_palette != null)
                _palette.PalettePaint -= new EventHandler<PaletteLayoutEventArgs>(OnPalettePaint);

            // Cache the new IPalette that is the global palette
            _palette = KryptonManager.CurrentGlobalPalette;

            // Hook into events for the new palette
            if (_palette != null)
                _palette.PalettePaint += new EventHandler<PaletteLayoutEventArgs>(OnPalettePaint);

            // Change of palette means we should repaint to show any changes
            Invalidate();
        }

        private void OnPalettePaint(object sender, PaletteLayoutEventArgs e)
        {
            // Palette indicates we might need to repaint, so lets do it
            Invalidate();
        }

    }
}
