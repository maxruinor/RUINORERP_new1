using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Drawing.Design;

namespace WinLib
{
    /* 作者：Starts_2000
     * 日期：2009-11-26
     * 网站：http://www.WinLib.com CS 程序员之窗。
     * 你可以免费使用或修改以下代码，但请保留版权信息。
     * 具体请查看 CS程序员之窗开源协议（http://www.WinLib.com/csol.html）。
     */

    [Serializable]
    [DefaultProperty("Text")]
    [TypeConverter(
        typeof(ExpandableObjectConverter))]
    public class ImageComboBoxItem : 
        IDisposable, ISerializable
    {
        #region Fields

        private ImageComboBox _imageComboBox;
        private string _text = "ImageComboBoxItem";
        private ImageComboBoxItemImageIndexer _imageIndexer;
        private object _tag;
        private int _level;

        #endregion

        #region Constructors

        public ImageComboBoxItem()
        {
        }

        public ImageComboBoxItem(string text)
            : this(text, -1, 0)
        {
        }

        public ImageComboBoxItem(
            string text, int imageIndex)
            : this(text, imageIndex, 0)
        {
        }

        public ImageComboBoxItem(
            string text, string imageKey)
            : this(text, imageKey, 0)
        {
        }

        public ImageComboBoxItem(
            string text, int imageIndex, int level)
            : this()
        {
            _text = text;
            ImageIndexer.Index = imageIndex;
            _level = level;
        }

        public ImageComboBoxItem(
           string text, string imageKey, int level)
            : this()
        {
            _text = text;
            ImageIndexer.Key = imageKey;
            _level = level;
        }

        protected ImageComboBoxItem(
            SerializationInfo info,
            StreamingContext context)
            : this()
        {
            Deserialize(info, context);
        }

        #endregion

        #region Properties

        [Localizable(true)]
        public string Text
        {
            get 
            {
                if (_text != null)
                {
                    return _text;
                }
                return "";
            }
            set
            {
                _text = value; 
            }
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

        [DefaultValue(0)]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DesignerSerializationVisibility(
           DesignerSerializationVisibility.Hidden)]
        public int Level
        {
            get { return _level; }
            set 
            {
                if (_level < 0)
                {
                    throw new ArgumentOutOfRangeException("level");
                }
                _level = value; 
            }
        }

        [DefaultValue(-1)]
        [Localizable(true)]
        [RelatedImageList("ImageComboBox.ImageList")]
        [Editor(
            EditorAssemblyName.ImageIndexEditor,
            typeof(UITypeEditor))]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(NoneExcludedImageIndexConverter))]
        public int ImageIndex
        {
            get
            {
                if (((ImageIndexer.Index != -1) && 
                    (ImageList != null)) && 
                    (ImageIndexer.Index >= ImageList.Images.Count))
                {
                    return ImageList.Images.Count - 1;
                }
                return ImageIndexer.Index;
            }
            set
            {
                if (value < -1)
                {
                    throw new ArgumentOutOfRangeException("ImageIndex");
                }
                ImageIndexer.Index = value;
            }
        }

        [DefaultValue("")]
        [Localizable(true)]
        [RelatedImageList("ImageComboBox.ImageList")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [Editor(
            EditorAssemblyName.ImageIndexEditor, 
            typeof(UITypeEditor))]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(ImageKeyConverter))]
        public string ImageKey
        {
            get
            {
                return ImageIndexer.Key;
            }
            set
            {
                ImageIndexer.Key = value;
            }
        }

        [Browsable(false)]
        public ImageComboBox ImageComboBox
        {
            get { return _imageComboBox; }
        }

        internal Image Image
        {
            get
            {
                int actualIndex = ImageIndexer.ActualIndex;
                if (ImageList != null &&
                    ImageList.Images.Count > 0 &&
                    actualIndex != -1)
                {
                    return ImageList.Images[actualIndex];
                }
                return null;
            }
        }

        [Browsable(false)]
        internal ImageList ImageList
        {
            get
            {
                if (ImageComboBox != null)
                {
                    return ImageComboBox.ImageList;
                }
                return null;
            }
        }

        internal ImageComboBoxItemImageIndexer ImageIndexer
        {
            get
            {
                if (_imageIndexer == null)
                {
                    _imageIndexer = 
                        new ImageComboBoxItemImageIndexer(this);
                }
                return _imageIndexer;
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return _text;
        }

        internal void Host(ImageComboBox parent)
        {
            _imageComboBox = parent;
        }

        [SecurityPermission(
            SecurityAction.Demand, 
            Flags = SecurityPermissionFlag.SerializationFormatter), 
        SecurityPermission(
            SecurityAction.InheritanceDemand, 
            Flags = SecurityPermissionFlag.SerializationFormatter)]
        protected virtual void Serialize(
            SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Text", Text);
            info.AddValue("Level", Level);
            info.AddValue("ImageIndex", ImageIndexer.Index);
            if (!string.IsNullOrEmpty(ImageIndexer.Key))
            {
                info.AddValue("ImageKey", ImageIndexer.Key);
            }
        }

        protected virtual void Deserialize(
            SerializationInfo info,
            StreamingContext context)
        {
            string imageKey = null;
            int imageIndex = -1;
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                if (current.Name == "Text")
                {
                    Text = info.GetString(current.Name);
                }
                else if (current.Name == "Level")
                {
                    Level = info.GetInt32(current.Name);
                }
                else
                {
                    if (current.Name == "ImageIndex")
                    {
                        imageIndex = info.GetInt32(current.Name);
                        continue;
                    }
                    if (current.Name == "ImageKey")
                    {
                        imageKey = info.GetString(current.Name);
                        continue;
                    }
                }
            }
            if (imageKey != null)
            {
                ImageKey = imageKey;
            }
            else if (imageIndex != -1)
            {
                ImageIndex = imageIndex;
            }
        }

        #endregion

        #region ISerializable 成员

        [SecurityPermission(
            SecurityAction.LinkDemand,
            Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(
            SerializationInfo info, StreamingContext context)
        {
            Serialize(info, context);
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            _imageComboBox = null;
            _imageIndexer = null;
            _tag = null;
        }

        #endregion

        #region ImageComboBoxItemImageIndexer Class

        internal class ImageComboBoxItemImageIndexer
            : ImageIndexer
        {
            private ImageComboBoxItem _owner;

            public ImageComboBoxItemImageIndexer(
                ImageComboBoxItem owner)
            {
                _owner = owner;
            }

            public override ImageList ImageList
            {
                get
                {
                    if (_owner != null)
                    {
                        return _owner.ImageList;
                    }
                    return null;
                }
                set
                {
                }
            }
        }

        #endregion
    }
}
