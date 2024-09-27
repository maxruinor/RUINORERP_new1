using DevAge.Windows.Forms;
using System;
using System.IO;

namespace SourceGrid.Cells.Controllers
{
    /// <summary>
    ///  一个图片预览控制器 鼠标移动在上面时显示图片.但是显示的位置并不是在鼠标的位置
    /// </summary>
    public class PictureViewerController : ControllerBase
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
                    // 如果 sender.Value 已经是 Image，直接使用它
                    PickerImage = sender.Value as System.Drawing.Image;
                    frmPictureViewer.PictureBoxViewer.Image = PickerImage;
                }
                else if (sender.Value is byte[])
                {
                    // 如果 sender.Value 是 byte[]，从 byte[] 创建 Image
                    byte[] bytes = sender.Value as byte[];
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        PickerImage = System.Drawing.Image.FromStream(ms, true);
                    }
                    frmPictureViewer.PictureBoxViewer.Image = PickerImage;
                }

                // 获取鼠标当前位置
                System.Drawing.Point cursorPosition = System.Windows.Forms.Cursor.Position;

                // 设置 frmPictureViewer 窗体的位置
                frmPictureViewer.Location = new System.Drawing.Point(cursorPosition.X, cursorPosition.Y);

                // 显示窗体
                frmPictureViewer.Show();
            }

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
