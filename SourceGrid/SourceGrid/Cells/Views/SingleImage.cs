using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using DevAge.Drawing;
using System.IO;
using DevAge.Drawing.VisualElements;
using System.Drawing.Imaging;
using SourceGrid.Cells.Editors;
using System.Runtime.CompilerServices;

namespace SourceGrid.Cells.Views
{
    /// <summary>
    /// 单独的一个图片格子。区别于他原来提供的。这种方式，公用设置属性会将值全设置为一个样。得每次给值 都重新设置
    /// </summary>
    [Serializable]
    public class SingleImage : Cell
    {

        #region Constructors

        /// <summary>
        /// Use default setting
        /// </summary>
        public SingleImage()
        {
            ElementsDrawMode = DevAge.Drawing.ElementsDrawMode.Covering;
            FirstBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
            SecondBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCyan);
            DevAge.Drawing.BorderLine border = new DevAge.Drawing.BorderLine(Color.DarkKhaki, 1);
            DevAge.Drawing.RectangleBorder cellBorder = new DevAge.Drawing.RectangleBorder(border, border);
            Border = cellBorder;
        }

        private System.Drawing.Image _GridImage;
        public SingleImage(System.Drawing.Image image)
        {
            _GridImage = image;
            FirstBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
            SecondBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCyan);
            DevAge.Drawing.BorderLine border = new DevAge.Drawing.BorderLine(Color.DarkKhaki, 1);
            DevAge.Drawing.RectangleBorder cellBorder = new DevAge.Drawing.RectangleBorder(border, border);
            Border = cellBorder;
        }
        private DevAge.Drawing.VisualElements.IVisualElement mFirstBackground;
        public DevAge.Drawing.VisualElements.IVisualElement FirstBackground
        {
            get { return mFirstBackground; }
            set { mFirstBackground = value; }
        }

        private DevAge.Drawing.VisualElements.IVisualElement mSecondBackground;
        public DevAge.Drawing.VisualElements.IVisualElement SecondBackground
        {
            get { return mSecondBackground; }
            set { mSecondBackground = value; }
        }

        private string ShowImageHash = string.Empty;

        protected override void PrepareView(CellContext context)
        {
            base.PrepareView(context);
            //start by watson 2024-1-11
            if (context.Value == null)
            {
                return;
            }
            //显示图片  要是图片列才处理
            if (context.Cell is SourceGrid.Cells.Image || context.Value is Bitmap || context.Value is Image || context.Value is byte[])
            {
                //end by watson 2024-08-28 TODO:
                //PrepareVisualElementImage(context);

                //Read the image
                if (context.Value is byte[])
                {
                    //将图像读入到字节数组
                    byte[] buffByte = context.Value as byte[];
                    System.Drawing.Image img = null;
                    // 使用 MemoryStream 从字节数组创建流
                    using (MemoryStream stream = new MemoryStream(context.Value as byte[]))
                    {
                        // 从流中创建 Image 对象
                        img = System.Drawing.Image.FromStream(stream);
                        if (img != null)
                        {
                            GridImage = img;
                            // context.Cell = new SourceGrid.Cells.Image(img);
                            //context.Cell.View = new SourceGrid.Cells.Views.SingleImage(img);
                        }
                    }

                }

                if (context.Value is Bitmap || context.Value is System.Drawing.Image)
                {
                    // 使用 MemoryStream 从字节数组创建流
                    GridImage = context.Value as System.Drawing.Image;
                }

            }
            //else if (context.Value is string && GridImage == null)
            //{
            //    if (context.Cell.Editor != null)
            //    {
            //        if (context.Cell.Editor is ImageWebPicker webPicker)
            //        {
            //            if (System.IO.File.Exists(webPicker.AbsolutelocPath) && !string.IsNullOrEmpty(webPicker.Imagehash))
            //            {
            //                if (string.IsNullOrEmpty(ShowImageHash) || GridImage == null)
            //                {
            //                    GridImage = System.Drawing.Image.FromFile(webPicker.AbsolutelocPath);
            //                }
            //                else if (!ShowImageHash.Equals(webPicker.Imagehash, StringComparison.OrdinalIgnoreCase))//如果不相等
            //                {
            //                    GridImage = System.Drawing.Image.FromFile(webPicker.AbsolutelocPath);
            //                }
            //                if (GridImage != null)
            //                {
            //                    ShowImageHash = ImagePickerHelper.GenerateHash(GridImage);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            //从web下载图片
            //        }
            //    }
            //}

        }


        protected override void OnDraw(GraphicsCache graphics, RectangleF area)
        {
            base.OnDraw(graphics, area);
        }

        protected override void OnDrawContent(GraphicsCache graphics, RectangleF area)
        {
            base.OnDrawContent(graphics, area);
            using (MeasureHelper measure = new MeasureHelper(graphics))
            {
                if (GridImage != null)
                {
                    graphics.Graphics.DrawImage(GridImage, Rectangle.Round(area)); //Note: 如果我不做矩形。有时，图像会以奇怪的拉伸方式绘制（不清晰）。这个问题可能是由于使用浮点重载的图形代码中的某些舍入引起的
                }
            }
        }
        protected override void OnDrawBackground(GraphicsCache graphics, RectangleF area)
        {
            base.OnDrawBackground(graphics, area);
        }


        /// <summary>
        /// Copy constructor.  This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
        /// </summary>
        /// <param name="other"></param>
        public SingleImage(SingleImage other)
            : base(other)
        {
            mImage = (DevAge.Drawing.VisualElements.IVisualElement)other.OneImage.Clone();
        }
        #endregion

        private DevAge.Drawing.VisualElements.IVisualElement mImage = new DevAge.Drawing.VisualElements.Image();
        /// <summary>
        /// Images of the cells
        /// </summary>
        public DevAge.Drawing.VisualElements.IVisualElement OneImage
        {
            get { return mImage; }
        }

        public System.Drawing.Image GridImage { get => _GridImage; set => _GridImage = value; }

        protected override IEnumerable<DevAge.Drawing.VisualElements.IVisualElement> GetElements()
        {
            foreach (DevAge.Drawing.VisualElements.IVisualElement v in GetBaseElements())
                yield return v;
            if (OneImage != null)
            {
                yield return OneImage;
            }

        }



        private IEnumerable<DevAge.Drawing.VisualElements.IVisualElement> GetBaseElements()
        {
            return base.GetElements();
        }

        #region Clone
        /// <summary>
        /// Clone this object. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new SingleImage(this);
        }
        #endregion
    }
}
