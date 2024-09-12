using DevAge.Windows.Forms;
using System;
using System.IO;

namespace SourceGrid.Cells.Controllers
{
    /// <summary>
    ///  Ò»¸öÍ¼Æ¬Ô¤ÀÀ¿ØÖÆÆ÷ 
    /// </summary>
    public class PictureViewer : ControllerBase
    {
        /// <summary>
        /// Default tooltiptext
        /// </summary>
        public readonly static frmPictureViewer frmPictureViewer = new frmPictureViewer();

        #region IBehaviorModel Members


        public override void OnMouseEnter(CellContext sender, EventArgs e)
        {
            base.OnMouseEnter(sender, e);
            ApplyPreviewImage(sender, e);
        }

        public override void OnMouseLeave(CellContext sender, EventArgs e)
        {
            base.OnMouseLeave(sender, e);
            ResetPreviewImage(sender, e);
        }
        #endregion

        private string mToolTipTitle = string.Empty;
        public string ToolTipTitle
        {
            get { return mToolTipTitle; }
            set { mToolTipTitle = value; }
        }



        private System.Drawing.Color mBackColor = System.Drawing.Color.Empty;
        public System.Drawing.Color BackColor
        {
            get { return mBackColor; }
            set { mBackColor = value; }
        }
        private System.Drawing.Color mForeColor = System.Drawing.Color.Empty;
        public System.Drawing.Color ForeColor
        {
            get { return mForeColor; }
            set { mForeColor = value; }
        }

        public System.Drawing.Image PickerImage { get; set; }
        /// <summary>
        /// Change the cursor with the cursor of the cell
        /// </summary>
        public virtual void ApplyPreviewImage(CellContext sender, EventArgs e)
        {
            if (sender.Value != null)
            {
                if (sender.Value is System.Drawing.Image)
                { 
                
                }
                else if (sender.Value is byte[])
                {
                    byte[] bytes = sender.Value as byte[];
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        PickerImage = System.Drawing.Image.FromStream(ms, true);
                    }
                    frmPictureViewer.PictureBoxViewer.Image = PickerImage;
                    frmPictureViewer.Show();
                }
            }
            //Models.IToolTipText toolTip;
            //if ((toolTip = (Models.IToolTipText)sender.Cell.Model.FindModel(typeof(Models.IToolTipText))) != null)
            //{
            //    string text = toolTip.GetToolTipText(sender);
            //    if (text != null && text.Length > 0)
            //    {
            //        sender.Grid.ToolTipText = text;
            //        sender.Grid.ToolTip.ToolTipTitle = ToolTipTitle;
            //        if (BackColor.IsEmpty == false)
            //            sender.Grid.ToolTip.BackColor = BackColor;
            //        if (ForeColor.IsEmpty == false)
            //            sender.Grid.ToolTip.ForeColor = ForeColor;
            //    }
            //}
        }

        /// <summary>
        /// Reset the original cursor
        /// </summary>
        protected virtual void ResetPreviewImage(CellContext sender, EventArgs e)
        {
            if (frmPictureViewer!=null  )
            {
                frmPictureViewer.Hide();
            }
           
        }
    }
}
