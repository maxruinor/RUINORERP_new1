using System;
using System.Windows.Forms;
namespace SourceGrid.Cells
{
    /// <summary>
    /// 
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
            Model.RemoveModel(Model.FindModel(typeof(Models.Image)));

            //Then I add a new IImage model that takes the image directly from the value.
       
            if (Editor == null)
            {
                //Editor = Editors.ImageWebPickEditor.Default;
                Editor = new Editors.ImageWebPickEditor(typeof(string));
                Editor.EditableMode = EditableMode.Default;
                Editor.AllowNull = true;
                Editor.EnableEdit = true;
            }
        }
        #endregion
    }
}


