using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.ClientCmdService
{
    public interface IClientCommand 
    {

        void Execute(object parameters = null);
        Task ExecuteAsync(CancellationToken cancellationToken, object parameters=null);
    }


  
}
