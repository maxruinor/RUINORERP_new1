using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransInstruction;
using TransInstruction.CommandService;

namespace RUINORERP.UI.ClientCmdService
{
    public class ReceiveCacheCommand : IClientCommand
    {
        public string productName { get; set; }
        public CmdOperation OperationType { get ; set; }
        public OriginalData DataPacket { get; set; }

        public bool AnalyzeDataPacket(OriginalData gd)
        {
            throw new NotImplementedException();
        }

        public void BuildDataPacket(object request = null)
        {
            throw new NotImplementedException();
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken, object parameters)
        {
            if (parameters == null)
            {
                await Task.Run(
                   () =>
                   //只是一行做对。为了编译通过
                   productName == "1"
                   //ProductService.AddProduct(productName, price)
                   ,

                   cancellationToken
                   );
            }
            else
            {
                throw new ArgumentException("parameters is required.");
            }

        }

  
    }

}
