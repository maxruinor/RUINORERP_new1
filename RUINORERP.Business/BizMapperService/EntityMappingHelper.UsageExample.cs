using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// EntityMappingHelper使用示例类
    /// 此类仅用于展示如何使用EntityMappingHelper，不应直接在生产环境中使用
    /// </summary>
    public class EntityMappingHelperUsageExample
    {
        /// <summary>
        /// 示例：在应用程序启动时初始化EntityMappingHelper
        /// 通常在Startup.cs或Program.cs中调用
        /// </summary>
        public void InitializeEntityMappingHelperExample(IEntityMappingService entityMappingService)
        {
            // 在应用程序启动时设置当前实体映射服务
            // 这通常在依赖注入容器配置完成后调用
            EntityMappingHelper.SetCurrent(entityMappingService);
            
            // 初始化实体映射
            EntityMappingHelper.Initialize();
        }

        /// <summary>
        /// 示例：使用EntityMappingHelper获取实体信息
        /// </summary>
        public void GetEntityInfoExample()
        {
            // 根据业务类型获取实体信息
            BizEntityInfo saleOrderInfo = EntityMappingHelper.GetEntityInfo(BizType.销售订单);
            System.Diagnostics.Debug.WriteLine($"销售订单实体类型: {saleOrderInfo.EntityType.Name}");
            System.Diagnostics.Debug.WriteLine($"销售订单表名: {saleOrderInfo.TableName}");

            // 使用泛型方法获取实体信息
            BizEntityInfo purOrderInfo = EntityMappingHelper.GetEntityInfo<tb_PurOrder>();
            System.Diagnostics.Debug.WriteLine($"采购订单实体类型: {purOrderInfo.EntityType.Name}");
            System.Diagnostics.Debug.WriteLine($"采购订单业务类型: {purOrderInfo.BizType}");

            // 根据表名获取实体信息
            BizEntityInfo entityInfoByTable = EntityMappingHelper.GetEntityInfoByTableName("tb_SaleOrder");
            if (entityInfoByTable != null)
            {
                System.Diagnostics.Debug.WriteLine($"表tb_SaleOrder对应的业务类型: {entityInfoByTable.BizType}");
            }
        }

        /// <summary>
        /// 示例：使用EntityMappingHelper进行类型转换
        /// </summary>
        public void TypeConversionExample()
        {
            // 根据业务类型获取实体类型
            Type saleOrderType = EntityMappingHelper.GetEntityType(BizType.销售订单);
            System.Diagnostics.Debug.WriteLine($"销售订单对应的实体类型: {saleOrderType.Name}");

            // 根据实体类型获取业务类型
            BizType bizType = EntityMappingHelper.GetBizType(typeof(tb_PurOrder));
            System.Diagnostics.Debug.WriteLine($"tb_PurOrder对应的业务类型: {bizType}");

            // 根据表名获取实体类型
            Type entityTypeByTable = EntityMappingHelper.GetEntityTypeByTableName("tb_SaleOrder");
            if (entityTypeByTable != null)
            {
                System.Diagnostics.Debug.WriteLine($"表tb_SaleOrder对应的实体类型: {entityTypeByTable.Name}");
            }
        }

        /// <summary>
        /// 示例：使用EntityMappingHelper处理实体对象
        /// </summary>
        public void EntityObjectExample()
        {
            // 创建一个实体对象示例
            var saleOrder = new tb_SaleOrder { SOrder_ID = 1, SOrderNo = "SO2023001" };

            // 根据实体对象获取业务类型
            BizType bizType = EntityMappingHelper.GetBizTypeByEntity(saleOrder);
            System.Diagnostics.Debug.WriteLine($"实体对象对应的业务类型: {bizType}");

            // 使用泛型方法获取业务类型
            BizType genericBizType = EntityMappingHelper.GetBizType(saleOrder);
            System.Diagnostics.Debug.WriteLine($"泛型方法获取的业务类型: {genericBizType}");

            // 获取实体对象的ID和名称
            var (id, name) = EntityMappingHelper.GetIdAndName(saleOrder);
            System.Diagnostics.Debug.WriteLine($"实体ID: {id}, 实体名称: {name}");
        }

        /// <summary>
        /// 示例：使用EntityMappingHelper检查注册状态
        /// </summary>
        public void CheckRegistrationExample()
        {
            // 检查业务类型是否已注册
            bool isSaleOrderRegistered = EntityMappingHelper.IsRegistered(BizType.销售订单);
            System.Diagnostics.Debug.WriteLine($"销售订单业务类型是否已注册: {isSaleOrderRegistered}");

            // 检查实体类型是否已注册
            bool isPurOrderRegistered = EntityMappingHelper.IsRegistered(typeof(tb_PurOrder));
            System.Diagnostics.Debug.WriteLine($"tb_PurOrder实体类型是否已注册: {isPurOrderRegistered}");

            // 检查表名是否已注册
            bool isTableRegistered = EntityMappingHelper.IsRegisteredByTableName("tb_SaleOrder");
            System.Diagnostics.Debug.WriteLine($"表tb_SaleOrder是否已注册: {isTableRegistered}");
        }

        /// <summary>
        /// 示例：使用EntityMappingHelper处理共享表实体
        /// </summary>
        public void SharedTableExample()
        {
            // 假设tb_FM_PaymentRecord是一个共享表，用于存储收款单和付款单
            // 我们可以通过枚举标志获取对应的实体信息
            BizEntityInfo receiptInfo = EntityMappingHelper.GetEntityInfo(typeof(tb_FM_PaymentRecord), (int)ReceivePaymentType.收款);
            System.Diagnostics.Debug.WriteLine($"收款单对应的业务类型: {receiptInfo.BizType}");

            BizEntityInfo paymentInfo = EntityMappingHelper.GetEntityInfo(typeof(tb_FM_PaymentRecord), (int)ReceivePaymentType.付款);
            System.Diagnostics.Debug.WriteLine($"付款单对应的业务类型: {paymentInfo.BizType}");
        }
    }

    // 注意：在实际使用中，您可以直接在任何需要访问实体映射服务的地方使用EntityMappingHelper
    // 而不需要在每个类的构造函数中注入IEntityMappingService
    //
    // 例如：
    // public class YourBusinessClass
    // {
    //     public void YourMethod()
    //     {
    //         // 直接使用静态方法，无需注入
    //         var entityInfo = EntityMappingHelper.GetEntityInfo<BizType.销售订单>();
    //         // 执行其他操作...
    //     }
    // }
    //
    // 重要：请确保在应用程序启动时调用EntityMappingHelper.SetCurrent()方法设置服务实例
    // 否则在首次使用时会抛出InvalidOperationException异常
}