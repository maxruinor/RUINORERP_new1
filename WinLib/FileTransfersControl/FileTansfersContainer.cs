using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-08-05
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    public class FileTansfersContainer : Panel
    {
        private IFileTransfersItemText _fileTransfersItemText;

        public FileTansfersContainer()
            : base()
        {
            AutoScroll = true;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IFileTransfersItemText FileTransfersItemText
        {
            get 
            {
                if (_fileTransfersItemText == null)
                {
                    _fileTransfersItemText = new FileTransfersItemText();
                }
                return _fileTransfersItemText;
            }
            set
            {
                _fileTransfersItemText = value;
                foreach (FileTransfersItem item in Controls)
                {
                    item.FileTransfersText = _fileTransfersItemText;
                }
            }
        }

        public FileTransfersItem AddItem(
            string text,
            string fileName,
            Image image,
            long fileSize,
            FileTransfersItemStyle style)
        {
            FileTransfersItem item = new FileTransfersItem();
            item.Text = text;
            item.FileName = fileName;
            item.Image = image;
            item.FileSize = fileSize;
            item.Style = style;
            item.FileTransfersText = FileTransfersItemText;
            item.Dock = DockStyle.Top;

            SuspendLayout();
            Controls.Add(item);
            item.BringToFront();
            ResumeLayout(true);

            return item;
        }

        public FileTransfersItem AddItem(
            string name,
            string text,
            string fileName,
            Image image,
            long fileSize,
            FileTransfersItemStyle style)
        {
            FileTransfersItem item = new FileTransfersItem();
            item.Name = name;
            item.Text = text;
            item.FileName = fileName;
            item.Image = image;
            item.FileSize = fileSize;
            item.Style = style;
            item.FileTransfersText = FileTransfersItemText;
            item.Dock = DockStyle.Top;

            SuspendLayout();
            Controls.Add(item);
            item.BringToFront();
            ResumeLayout(true);

            return item;
        }

        public void RemoveItem(FileTransfersItem item)
        {
            Controls.Remove(item);
        }

        public void RemoveItem(string name)
        {
            Controls.RemoveByKey(name);
        }

        public void RemoveItem(Predicate<FileTransfersItem> match)
        {
            FileTransfersItem itemRemove = null;
            foreach (FileTransfersItem item in Controls)
            {
                if (match(item))
                {
                    itemRemove = item;
                }
            }
            Controls.Remove(itemRemove);
        }

        public FileTransfersItem Search(string name)
        {
            return Controls[name] as FileTransfersItem;
        }

        public FileTransfersItem Search(Predicate<FileTransfersItem> match)
        {
            foreach (FileTransfersItem item in Controls)
            {
                if (match(item))
                {
                    return item;
                }
            }

            return null;
        }
    }
}
