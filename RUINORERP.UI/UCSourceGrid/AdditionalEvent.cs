using RUINOR.WinFormsUI.CustomPictureBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.UCSourceGrid
{
    public class AdditionalEvent : SourceGrid.Cells.Controllers.ControllerBase
    {
        public override void OnDoubleClick(SourceGrid.CellContext sender, EventArgs e)
        {
            // 检查是否有 ValueImageWeb 模型，如果有，让 PopupMenuForRemoteImageView 处理
            var model = sender.Cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
            if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb)
            {
                // 如果有 ValueImageWeb 模型，说明是远程图片，让 PopupMenuForRemoteImageView 处理
                // 这里直接返回，不处理
                return;
            }

            base.OnDoubleClick(sender, e);
            if (sender.Value == null) 
            {
                return;
            }
            //图片特殊处理
            if (sender.Value is System.Drawing.Bitmap)
            {
                if (sender.Value != null)
                {
                    frmPictureViewer frmShow = new frmPictureViewer();
                    frmShow.PictureBoxViewer.Image = sender.Value as System.Drawing.Bitmap;
                    frmShow.ShowDialog();
                }
            }

            if (sender.Value.GetType().Name == "Byte[]")
            {
                if (sender.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])sender.Value);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(buf, true);
                    if (image != null)
                    {
                        frmPictureViewer frmShow = new frmPictureViewer();
                        frmShow.PictureBoxViewer.Image = image;
                        frmShow.ShowDialog();
                    }
                }
            }

        }
    }
}
