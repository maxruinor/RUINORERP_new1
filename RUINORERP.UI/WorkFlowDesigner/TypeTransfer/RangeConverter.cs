using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.WorkFlowDesigner.TypeConverter
{
    public class RangeConverter : System.ComponentModel.TypeConverter
    {
        public override PropertyDescriptorCollection
       GetProperties(ITypeDescriptorContext context, object value, Attribute[] filter)
        {
            return TypeDescriptor.GetProperties(value, filter);

        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {

            return true;

        }
    }
}
