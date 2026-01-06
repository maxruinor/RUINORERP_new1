using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ATechnologyStack
{
    /// <summary>
    /// 窗体模块合且时 实际窗体分开时的标识
    /// </summary>
    public interface ISharedIdentification
    {
        public SharedFlag sharedFlag { get; set; }
    }




}
