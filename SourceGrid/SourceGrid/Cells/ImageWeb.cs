using System;
using System.Windows.Forms;

namespace SourceGrid.Cells.Virtual
{
    /// <summary>
    /// A Cell with an Image. Write and read byte[] values.
    /// 远程下载的图片单元格
    /// </summary>
    public class ImageWeb : CellVirtual
    {
        /// <summary>
        /// Constructor using a ValueImage model to read he image directly from the value of the cell.
        /// </summary>
        public ImageWeb()
        {
            Model.AddModel(Models.ValueImageWeb.Default);
            Editor = Editors.ImageWebPicker.Default;
        }
    }
}

namespace SourceGrid.Cells
{
    /// <summary>
    /// 为了实现远程下载图片，需要继承Cell类，并重写Value属性
    /// </summary>
    public class ImageWebCell : Cell
    {



        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ImageWebCell() : this(null)
        {
        }
        /// <summary>
        /// Constructor using a ValueImage model to read he image directly from the value of the cell.
        /// </summary>
        public ImageWebCell(object value) : base(value)
        {

            //First I remove the old IImage model that the Cell use to link the Image property to an external value.
            Model.RemoveModel(Model.FindModel(typeof(Models.Image)));//感觉这行可以删除 注释掉,但是调试发现会加载这个类型。确实要删除。为什么会添加进到这个容器。后面看

            //Then I add a new IImage model that takes the image directly from the value.
            //Model.AddModel(Models.ValueImageWeb.Default); 基类已经添加。这里不重复添加

            Editor = Editors.ImageWebPicker.Default;
        }
        #endregion
    }
}
