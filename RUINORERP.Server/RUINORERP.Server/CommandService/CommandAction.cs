using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransInstruction.CommandService;
namespace RUINORERP.Server.CommandService
{

  

    //注入
    // services.AddSingleton<ICommand, AddUserCommand>();
    //public class AddUserCommand : ICommand
    //{
    //    public void Execute() { /* 添加用户 */ }
    //    public void Undo() { /* 撤销添加用户 */ }
    //}

    public class AddProductCommand : IServerCommand
    {
        private readonly string productName;
        private readonly decimal price;
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public AddProductCommand()
        {

        }

        public AddProductCommand(string productName, decimal price)
        {
            this.productName = productName;
            this.price = price;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // 添加产品逻辑
            await Task.Run(
                () =>
                //只是一行做对。为了编译通过
                productName == "1"
                //ProductService.AddProduct(productName, price)


                ,

                cancellationToken
                ); ; ;
        }
        public void Execute()
        {
            // 执行添加产品逻辑
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
