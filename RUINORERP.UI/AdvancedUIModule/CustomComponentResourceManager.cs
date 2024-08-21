using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.AdvancedUIModule
{
    internal class CustomComponentResourceManager : ComponentResourceManager
    {
        public CustomComponentResourceManager(Type type, string resourceName)
           : base(type)
        {
            this.BaseNameField = resourceName;
        }

        public CustomComponentResourceManager(string resourceName)
        {
            this.BaseNameField = resourceName;
        }

    }
}
