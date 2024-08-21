using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.ComponentModel;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-11-16
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ListBoxExItem : IDisposable
    {
        
        #region Fields

        private string _text = "ListBoxExItem";
        private Image _image;
        private object _tag;

        #endregion

        #region Constructors

        public ListBoxExItem()
        {
        }

        public ListBoxExItem(string text)
            : this(text, null)
        {
        }

        public ListBoxExItem(string text, Image image)
        {
            _text = text;
            _image = image;
        }

        #endregion

        #region Properties

        [DefaultValue("ImageComboBoxItem")]
        [Localizable(true)]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        [DefaultValue(typeof(Image), "null")]
        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }

        [Bindable(true)]
        [Localizable(false)]
        [DefaultValue("")]
        [TypeConverter(typeof(StringConverter))]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return _text;
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            _image = null;
            _tag = null;
        }

        #endregion
    }
}
