using RUINORERP.Common.CustomAttribute;
using RUINORERP.Extensions.Redis;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    public class AutoComplete
    {
        public AutoComplete(SearchType searcherType)
        {
            SearcherType = searcherType;
        }
        public SearchType SearcherType { get; private set; }

    }

    
 

}
