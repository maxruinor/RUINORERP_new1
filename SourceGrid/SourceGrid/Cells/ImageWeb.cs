using System;
using System.Windows.Forms;

namespace SourceGrid.Cells.Virtual
{
    /// <summary>
    /// A Cell with an Image. Write and read byte[] values.
    /// Զ�����ص�ͼƬ��Ԫ��
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
    /// Ϊ��ʵ��Զ������ͼƬ����Ҫ�̳�Cell�࣬����дValue����
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
            Model.RemoveModel(Model.FindModel(typeof(Models.Image)));//�о����п���ɾ�� ע�͵�,���ǵ��Է��ֻ����������͡�ȷʵҪɾ����Ϊʲô����ӽ���������������濴

            //Then I add a new IImage model that takes the image directly from the value.
            //Model.AddModel(Models.ValueImageWeb.Default); �����Ѿ���ӡ����ﲻ�ظ����

            Editor = Editors.ImageWebPicker.Default;
        }
        #endregion
    }
}
