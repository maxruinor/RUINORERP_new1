using RUINOR.WinFormsUI.CustomPictureBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.UCSourceGrid
{
    /// <summary>
    /// 额外事件控制器，处理通用的单元格事件
    /// </summary>
    public class AdditionalEvent : SourceGrid.Cells.Controllers.ControllerBase
    {
        public override void OnDoubleClick(SourceGrid.CellContext sender, EventArgs e)
        {
            // 检查单元格是否有 PopupMenuForRemoteImageView Controller
            // 如果有，说明这是远程图片单元格，由 PopupMenuForRemoteImageView 处理双击事件
            var remoteImageController = sender.Cell.Controller.FindController(typeof(PopupMenuForRemoteImageView));
            if (remoteImageController != null)
            {
                // 存在 PopupMenuForRemoteImageView，由它处理双击事件
                // 这里直接返回，不处理
                return;
            }

            // 检查单元格的View是否为RemoteImageView2
            if (sender.Cell.View is SourceGrid.Cells.Views.RemoteImageView)
            {
                // RemoteImageView类型的单元格，由 PopupMenuForRemoteImageView 处理
                return;
            }

            // 调用基类方法
            base.OnDoubleClick(sender, e);

            if (sender.Value == null)
            {
                return;
            }

            // 图片特殊处理（本地图片，非远程图片）
            if (sender.Value is System.Drawing.Bitmap bitmap)
            {
                if (bitmap != null)
                {
                    frmPictureViewer frmShow = new frmPictureViewer();
                    frmShow.PictureBoxViewer.Image = bitmap;
                    frmShow.ShowDialog();
                }
            }
            else if (sender.Value is byte[] imageBytes && imageBytes.Length > 0)
            {
                System.IO.MemoryStream buf = new System.IO.MemoryStream(imageBytes);
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
