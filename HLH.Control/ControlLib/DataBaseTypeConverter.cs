using System.ComponentModel;

namespace HLH.WinControl.ControlLib
{
    public class DataBaseTypeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "MySql", "Oracle", "ODBC" }); //为啥添加到属性里就不显示下拉框，而缺省是显示的
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }

}
