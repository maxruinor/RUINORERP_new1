using System;
using System.Windows.Forms;

namespace WinLib
{
    internal sealed class NoneExcludedImageIndexConverter 
        : ImageIndexConverter
    {
        protected override bool IncludeNoneAsStandardValue
        {
            get
            {
                return false;
            }
        }
    }
}
