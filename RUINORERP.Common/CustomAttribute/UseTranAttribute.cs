using System;
using System.Threading.Tasks;
using RUINORERP.Common.DB;
using StackExchange.Redis;

namespace RUINORERP.Common
{
    /// <summary>
    /// 这个Attribute就是使用时候的验证，把它添加到需要执行事务的方法中，即可完成事务的操作。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class UseTranAttribute : Attribute
    {
        /// <summary>
        /// 事务传播方式
        /// </summary>
        //public Propagation Propagation { get; set; } = Propagation.Required;


        public Propagation Propagation { get; }

        public UseTranAttribute(Propagation propagation = Propagation.Required)
        {
            Propagation = propagation;
        }


        //使用示例
        //public class OrderService
        //{
        //    [UseTran(Propagation.Required)]
        //    public void PlaceOrder(Order order)
        //    {
        //        // 业务逻辑...
        //    }

        //    [UseTran(Propagation.RequiresNew)]
        //    public async Task UpdateInventoryAsync(int productId, int quantity)
        //    {
        //        // 异步库存更新...
        //    }

        //    [UseTran(Propagation.Mandatory)]
        //    public void ApplyDiscount(string couponCode)
        //    {
        //        // 必须在事务中调用
        //    }
        //}
    }
}