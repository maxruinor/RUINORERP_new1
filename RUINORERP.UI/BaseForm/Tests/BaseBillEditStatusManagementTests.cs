using System;
using System.Threading.Tasks;
using RUINORERP.Model.Base;
using RUINORERP.UI.StateManagement;
using Xunit;

namespace RUINORERP.UI.BaseForm.Tests
{
    /// <summary>
    /// BaseBillEditGeneric状态管理方法集成测试类
    /// 用于验证BaseBillEditGeneric中状态管理相关方法的正确性
    /// </summary>
    public class BaseBillEditStatusManagementTests
    {
        /// <summary>
        /// 测试状态缓存机制的正确性
        /// 验证缓存能正确存储和清除状态
        /// </summary>
        [Fact]
        public void TestBusinessStatusCacheMechanism()
        {
            // 创建测试实体
            var testEntity = new TestEntity();
            testEntity.ID = 1;
            testEntity.DataStatus = (int)DataStatus.草稿;
            
            // 创建测试基类实例
            var testForm = new TestBillEditForm();
            
            // 首次获取状态（应从实体中读取）
            var status1 = testForm.GetBusinessStatus(testEntity);
            Assert.NotNull(status1);
            Assert.Equal(DataStatus.草稿, status1);
            
            // 再次获取状态（应从缓存中读取）
            var status2 = testForm.GetBusinessStatus(testEntity);
            Assert.Equal(status1, status2);
            
            // 修改实体状态并清除缓存
            testEntity.DataStatus = (int)DataStatus.已提交;
            testForm.ClearBusinessStatusCache(testEntity);
            
            // 重新获取状态（应反映最新变更）
            var status3 = testForm.GetBusinessStatus(testEntity);
            Assert.Equal(DataStatus.已提交, status3);
        }

        /// <summary>
        /// 测试异步设置实体状态方法
        /// 验证状态变更、缓存清除和UI更新的完整性
        /// </summary>
        [Fact]
        public async Task TestSetEntityStatusAsync()
        {
            // 创建测试实体
            var testEntity = new TestEntity();
            testEntity.ID = 2;
            
            // 创建测试基类实例
            var testForm = new TestBillEditForm();
            
            // 异步设置状态
            await testForm.SetEntityStatusAsync(testEntity, DataStatus.已审核);
            
            // 验证状态已正确设置
            Assert.Equal((int)DataStatus.已审核, testEntity.DataStatus);
            
            // 验证状态缓存已被清除（通过重新获取状态验证）
            var updatedStatus = testForm.GetBusinessStatus(testEntity);
            Assert.Equal(DataStatus.已审核, updatedStatus);
        }

        /// <summary>
        /// 测试多种业务状态类型的检测和转换
        /// 验证优先级顺序和错误处理
        /// </summary>
        [Fact]
        public void TestMultipleBusinessStatusTypes()
        {
            // 创建具有多种状态的测试实体
            var testEntity = new MultiStatusTestEntity();
            testEntity.ID = 3;
            testEntity.DataStatus = (int)DataStatus.草稿;
            testEntity.PrePaymentStatus = (int)PrePaymentStatus.已生效;
            
            // 创建测试基类实例
            var testForm = new TestBillEditForm();
            
            // 获取业务状态（应返回优先级较高的PrePaymentStatus）
            var businessStatus = testForm.GetBusinessStatus(testEntity);
            Assert.NotNull(businessStatus);
            Assert.IsType<PrePaymentStatus>(businessStatus);
            Assert.Equal(PrePaymentStatus.已生效, businessStatus);
        }

        /// <summary>
        /// 测试UI状态根据业务状态的自动更新
        /// 验证V3状态管理系统的集成
        /// </summary>
        [Fact]
        public async Task TestUIUpdateByBusinessStatus()
        {
            // 创建测试实体
            var testEntity = new TestEntity();
            testEntity.ID = 4;
            
            // 创建测试基类实例
            var testForm = new TestBillEditForm();
            
            // 测试草稿状态下的UI更新
            await testForm.SetEntityStatusAsync(testEntity, DataStatus.草稿);
            bool uiUpdatedDraft = await testForm.UpdateUIByBusinessStatusAsync(testEntity);
            Assert.True(uiUpdatedDraft);
            
            // 测试已审核状态下的UI更新
            await testForm.SetEntityStatusAsync(testEntity, DataStatus.已审核);
            bool uiUpdatedReviewed = await testForm.UpdateUIByBusinessStatusAsync(testEntity);
            Assert.True(uiUpdatedReviewed);
        }

        #region 测试辅助类

        /// <summary>
        /// 测试实体类
        /// </summary>
        private class TestEntity : BaseEntity
        {
            public int DataStatus { get; set; }
        }

        /// <summary>
        /// 多状态测试实体类
        /// </summary>
        private class MultiStatusTestEntity : BaseEntity
        {
            public int DataStatus { get; set; }
            public int PrePaymentStatus { get; set; }
            public int ARAPStatus { get; set; }
        }

        /// <summary>
        /// 测试表单类，用于访问BaseBillEditGeneric的保护方法
        /// </summary>
        private class TestBillEditForm : BaseBillEditGeneric<TestEntity, object>
        {
            // 公开保护方法以便测试
            public new Enum GetBusinessStatus(BaseEntity entity) => base.GetBusinessStatus(entity);
            public new void ClearBusinessStatusCache(BaseEntity entity) => base.ClearBusinessStatusCache(entity);
            public new async Task SetEntityStatusAsync<TStatus>(BaseEntity entity, TStatus status) where TStatus : Enum
                => await base.SetEntityStatusAsync(entity, status);
            public new async Task<bool> UpdateUIByBusinessStatusAsync(BaseEntity entity)
                => await base.UpdateUIByBusinessStatusAsync(entity);
        }

        #endregion
    }
}
