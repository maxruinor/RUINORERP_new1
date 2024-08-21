using System;
using System.Windows.Forms;

namespace WinLib
{
    internal class ImageIndexer
    {
        private ImageList _imageList;
        private int _index = -1;
        private string _key = string.Empty;
        private bool _useIntegerIndex = true;

        public virtual int ActualIndex
        {
            get
            {
                if (_useIntegerIndex)
                {
                    return Index;
                }
                if (ImageList != null)
                {
                    return ImageList.Images.IndexOfKey(Key);
                }
                return -1;
            }
        }

        public virtual ImageList ImageList
        {
            get
            {
                return _imageList;
            }
            set
            {
                _imageList = value;
            }
        }

        public virtual int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _key = string.Empty;
                _index = value;
                _useIntegerIndex = true;
            }
        }

        public virtual string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _index = -1;
                _key = (value == null) ? string.Empty : value;
                _useIntegerIndex = false;
            }
        }
    }
}
