using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WF.BizOperation
{
    public interface IBaseStep
    {
        string Id { get; set; }
        string Name { get; set; }
    }
}
