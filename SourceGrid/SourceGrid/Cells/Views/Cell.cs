using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using DevAge.Drawing;


namespace SourceGrid.Cells.Views
{
    /// <summary>
    /// Class to manage the visual aspect of a cell. This class can be shared beetween multiple cells.
    /// </summary>
    [Serializable]
    public class Cell : ViewBase
    {
        /// <summary>
        /// Represents a default Model
        /// </summary>
        public readonly static Cell Default = new Cell();

        #region Constructors

        /// <summary>
        /// Use default setting and construct a read and write VisualProperties
        /// </summary>
        public Cell()
        {
            ElementsDrawMode = DevAge.Drawing.ElementsDrawMode.Align;
        }

        /// <summary>
        /// Copy constructor.  This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
        /// </summary>
        /// <param name="p_Source"></param>
        public Cell(Cell p_Source) : base(p_Source)
        {
            ElementImage = (DevAge.Drawing.VisualElements.IImage)p_Source.ElementImage.Clone();
            ElementText = (DevAge.Drawing.VisualElements.IText)p_Source.ElementText.Clone();
        }
        #endregion

        #region Clone
        /// <summary>
        /// Clone this object. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Cell(this);
        }
        #endregion

        #region Visual elements

        protected override IEnumerable<DevAge.Drawing.VisualElements.IVisualElement> GetElements()
        {

            if (ElementImage != null)
                yield return ElementImage;

            if (this.GetType() == typeof(SourceGrid.Cells.Image))
            {

            }
            if (this.ImageStretch)
            {

            }
            if (ElementText != null)
                yield return ElementText;
        }

        public override void Refresh(CellContext context)
        {
            // 在这里实现您的刷新逻辑
        }
        protected override void PrepareView(CellContext context)
        {
            base.PrepareView(context);

            PrepareVisualElementText(context);

            //start by watson 2024-1-11
            if (context.Value == null)
            {
                return;
            }
            /*
            //显示图片  要是图片列才处理
            if (context.Cell is SourceGrid.Cells.Image || context.Value is Bitmap || context.Value is Image || context.Value is byte[])
            {
                //end by watson 2024-08-28 TODO:
                PrepareVisualElementImage(context);

                //Read the image
                System.Drawing.Image img = null;

                if (context.Value != null && context.Value is byte[])
                {
                    // 使用 MemoryStream 从字节数组创建流
                    using (MemoryStream stream = new MemoryStream(context.Value as byte[]))
                    {
                        // 从流中创建 Image 对象
                        img = System.Drawing.Image.FromStream(stream);
                        if (img != null)
                        {
                            // context.Cell = new SourceGrid.Cells.Image(img);
                            //context.Cell.View = new SourceGrid.Cells.Views.SingleImage(img);
                        }
                    }

                }
            }
            */
        }


        protected override void OnDraw(GraphicsCache graphics, RectangleF area)
        {
            base.OnDraw(graphics, area);
        }

        protected override void OnDrawContent(GraphicsCache graphics, RectangleF area)
        {
            base.OnDrawContent(graphics, area);
        }
        protected override void OnDrawBackground(GraphicsCache graphics, RectangleF area)
        {
            base.OnDrawBackground(graphics, area);
        }

        private DevAge.Drawing.VisualElements.IText mElementText = new DevAge.Drawing.VisualElements.TextGDI();
        /// <summary>
        /// Gets or sets the IText visual element used to draw the cell text.
        /// Default is DevAge.Drawing.VisualElements.TextGDI
        /// </summary>
        public DevAge.Drawing.VisualElements.IText ElementText
        {
            get { return mElementText; }
            set { mElementText = value; }
        }

        /// <summary>
        ///将当前视图的图像属性应用于指定的VisualElement。
        ///派生类可以调用此方法将设置应用于自定义VisualElement。
        /// </summary>
        protected virtual void PrepareVisualElementText(CellContext context)
        {
            if (ElementText is DevAge.Drawing.VisualElements.TextRenderer)
            {
                DevAge.Drawing.VisualElements.TextRenderer elementText = (DevAge.Drawing.VisualElements.TextRenderer)ElementText;

                elementText.TextFormatFlags = TextFormatFlags.Default | TextFormatFlags.NoPrefix;
                if (WordWrap)
                    elementText.TextFormatFlags |= TextFormatFlags.WordBreak;
                if (TrimmingMode == TrimmingMode.Char)
                    elementText.TextFormatFlags |= TextFormatFlags.EndEllipsis;
                else if (TrimmingMode == TrimmingMode.Word)
                    elementText.TextFormatFlags |= TextFormatFlags.WordEllipsis;
                elementText.TextFormatFlags |= DevAge.Windows.Forms.Utilities.ContentAligmentToTextFormatFlags(TextAlignment);
            }
            else if (ElementText is DevAge.Drawing.VisualElements.TextGDI)
            {
                DevAge.Drawing.VisualElements.TextGDI elementTextGDI = (DevAge.Drawing.VisualElements.TextGDI)ElementText;

                if (WordWrap)
                    elementTextGDI.StringFormat.FormatFlags = (StringFormatFlags)0;
                else
                    elementTextGDI.StringFormat.FormatFlags = StringFormatFlags.NoWrap;
                if (TrimmingMode == TrimmingMode.Char)
                    elementTextGDI.StringFormat.Trimming = StringTrimming.EllipsisCharacter;
                else if (TrimmingMode == TrimmingMode.Word)
                    elementTextGDI.StringFormat.Trimming = StringTrimming.EllipsisWord;
                else
                    elementTextGDI.StringFormat.Trimming = StringTrimming.None;
                elementTextGDI.Alignment = TextAlignment;
            }

            ElementText.Font = GetDrawingFont(context.Grid);
            ElementText.ForeColor = ForeColor;
            //I have already set the TextFormatFlags for the alignment so the Anchor is not necessary. I have removed this code for performance reasons.
            //element.AnchorArea = new DevAge.Drawing.AnchorArea(TextAlignment, false);

            ElementText.Value = context.DisplayText;
        }

        private DevAge.Drawing.VisualElements.IImage mElementImage = new DevAge.Drawing.VisualElements.Image();
        /// <summary>
        ///获取或设置用于绘制单元格图像的IImage视觉元素。
        ///默认设置为DevAge。绘图。视觉元素。图片
        /// </summary>
        public DevAge.Drawing.VisualElements.IImage ElementImage
        {
            get { return mElementImage; }
            set { mElementImage = value; }
        }

        /// <summary>
        ///将当前视图的图像属性应用于指定的VisualElement。
        ///派生类可以调用此方法将设置应用于自定义VisualElement。
        /// </summary>
        protected virtual void PrepareVisualElementImage(CellContext context)
        {
            ElementImage.AnchorArea = new DevAge.Drawing.AnchorArea(ImageAlignment, ImageStretch);

            //Read the image
            System.Drawing.Image img = null;
            Models.IImage imgModel = (Models.IImage)context.Cell.Model.FindModel(typeof(Models.IImage));
            if (imgModel != null)
            {
                img = imgModel.GetImage(context);
                ElementImage.Value = img;
                return;
            }

            Models.IImageWeb imgWebModel = (Models.IImageWeb)context.Cell.Model.FindModel(typeof(Models.IImageWeb));
            if (imgWebModel != null)
            {
                img = imgWebModel.GetImage(context);
                ElementImage.Value = img;
                return;
            }

            //context.Value = img;
            //ElementImage.AnchorArea = AnchorArea.HasBottom
        }
        #endregion
    }


}
