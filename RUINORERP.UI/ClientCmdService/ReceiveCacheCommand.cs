using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.ClientCmdService
{
    public class ReceiveCacheCommand : IClientCommand
    {
        public string productName { get; set; }

        public void Execute(object parameters = null)
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
