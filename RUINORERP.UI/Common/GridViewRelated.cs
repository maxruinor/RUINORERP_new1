using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using Netron.GraphLib;
using Pipelines.Sockets.Unofficial.Arenas;
using RUINORERP.Business;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.EntityLoadService;
using RUINORERP.Business.Processor;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global.Model;
using RUINORERP.Model;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.UI.FM;
using RUINORERP.UI.PSI.INV;
using SourceGrid2.Win32;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Common
{

    /// <summary>
    /// 表格显示关联单据等
    /// 有两种方式，一种是通过单号关联，一种是直接关联，主子表形式等，即：按列关联
    /// 另一种方式是库存跟踪时，按某列每行中的固定值关联
    /// 重构说明：
    /// 1. 支持通过业务类型枚举自动映射到对应的数据库表、实体类和窗体类型
    /// 2. 完善字段映射机制，同时支持主键ID和业务编号两种查询方式
    /// 3. 处理复杂业务场景如库存跟踪，能够根据SKU关联到采购入库、销售出库等多种业务单据
    /// 4. 提供灵活的配置方式，允许特殊单据重写默认的打开逻辑
    /// 5. 优化命名规范和处理机制，使代码更清晰易维护
    /// 6. 保持与现有架构的兼容性，特别是已定义的基类和参数传递机制
    /// 7. 增强错误处理，当无法确定对应业务类型时提供明确的反馈
    /// </summary>
    public class GridViewRelated
    {
        /// <summary>
        /// 实体加载服务，用于根据表名和单号加载实体数据
        /// </summary>
        private readonly EntityLoader _loader;

        /// <summary>
        /// 实体映射服务，用于业务类型、实体类型和表名之间的映射
        /// </summary>
        private readonly IEntityMappingService _mappingService;

        /// <summary>
        /// 菜单权限助手，用于打开目标窗体
        /// </summary>
        private readonly MenuPowerHelper menuPowerHelper;

        /// <summary>
        /// 自定义加载器，用于特殊实体加载逻辑
        /// </summary>
        private Dictionary<string, Func<tb_MenuInfo, object, Task>> customLoaders;

        /// <summary>
        /// 特殊菜单映射，用于处理菜单名称与实体名称不一致的情况
        /// </summary>
        private static readonly Dictionary<string, string> SpecialMenuMappings = new Dictionary<string, string>
        {
            { typeof(tb_ProductionDemand).Name, "UCProduceRequirement" },
            { typeof(tb_BOM_S).Name, "UCBillOfMaterials" }
        };

        public GridViewRelated()
        {
            menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
            _loader = Startup.GetFromFac<EntityLoader>();
            _mappingService = Startup.GetFromFac<IEntityMappingService>();
            customLoaders = new Dictionary<string, Func<tb_MenuInfo, object, Task>>();
            InitializeCustomLoaders();
        }

        /// <summary>
        /// 双击时当前窗体的菜单信息
        /// 相当于是要打开目标的上级菜单信息
        /// </summary>
        public tb_MenuInfo FromMenuInfo { get; set; } = new tb_MenuInfo();

        private bool complexType = false;

        /// <summary>
        /// 用一个属性来标识是不是复杂的类型。默认不是。
        /// 复杂是像库存跟踪一样。目标是由另一列中的具体的值来决定哪一个表
        /// </summary>
        public bool ComplexType { get => complexType; set => complexType = value; }

        private List<KeyValuePair<string, string>> complexTargtetField = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// 复杂类型时，目标字段
        /// </summary>
        public List<KeyValuePair<string, string>> ComplexTargtetField { get => complexTargtetField; set => complexTargtetField = value; }

        /// <summary>
        /// 设置复杂关联字段，由类型决定编号是哪种业务类型的窗体。
        /// </summary>
        /// <typeparam name="T1">实体类型</typeparam>
        /// <param name="_ExpBizType">业务类型字段表达式</param>
        /// <param name="_ExpBillNo">单据编号字段表达式</param>
        public void SetComplexTargetField<T1>(Expression<Func<T1, object>> _ExpBizType, Expression<Func<T1, object>> _ExpBillNo)
        {
            ComplexTargtetField.Add(new KeyValuePair<string, string>(_ExpBizType.GetMemberInfo().Name, _ExpBillNo.GetMemberInfo().Name));
        }

        /// <summary>
        /// 设置关联单据的列，T1:来源表，显示的实体，目标是指向，要打开的窗体用的实体，以及关联的列的字段名，可能是ID，也可能是单号
        /// </summary>
        /// <typeparam name="T1">来源 表格目前显示的实体</typeparam>
        /// <param name="_ExpSourceUniqueField">要打开的窗体用的实体名</param>
        /// <param name="TargetTableName">目标表名</param>
        /// <param name="TargetDisplayField">以及关联的列的字段名，可能是ID，也可能是单号</param>
        public void SetRelatedInfo<T1>(Expression<Func<T1, object>> _ExpSourceUniqueField, string TargetTableName, string TargetDisplayField)
        {
            RelatedInfo relatedInfo = new RelatedInfo();
            relatedInfo.SourceTableName = typeof(T1).Name;
            relatedInfo.TargetTableName = new KeyNamePair(TargetTableName, string.Empty);

            relatedInfo.SourceUniqueField = _ExpSourceUniqueField.GetMemberInfo().Name;
            relatedInfo.TargetDisplayField = TargetDisplayField;
            if (!RelatedInfoList.Any(c => c.TargetTableName.Key == TargetTableName))
            {
                RelatedInfoList.Add(relatedInfo);
            }
        }

        /// <summary>
        /// 设置关联单据的列，T1:来源表，显示的实体，目标是指向，要打开的窗体用的实体
        /// 分析out表格中 实际只要知道目标表名即可，和 来源实体中的对应列名
        /// </summary>
        /// <typeparam name="T1">来源 表格目前显示的实体</typeparam>
        /// <param name="_ExpSourceUniqueField">双击的列名</param>
        /// <param name="TargetTableNameFromField">这个参数能指定一个单据编号是来自不同的实体，类似由这个表其它列来指定如biztype</param>
        public void SetRelatedInfo<T1>(Expression<Func<T1, object>> _ExpSourceUniqueField, KeyNamePair TargetTableNameFromField)
        {
            RelatedInfo relatedInfo = new RelatedInfo();
            relatedInfo.SourceTableName = typeof(T1).Name;
            relatedInfo.TargetTableName = TargetTableNameFromField;
            relatedInfo.SourceUniqueField = _ExpSourceUniqueField.GetMemberInfo().Name;
            if (!RelatedInfoList.Any(c => c.TargetTableName.Key == TargetTableNameFromField.Key && c.SourceUniqueField == relatedInfo.SourceUniqueField))
            {
                RelatedInfoList.Add(relatedInfo);
            }
        }

        /// <summary>
        /// 打开自己本身的窗体（双击哪一列会跳到单据编辑菜单）
        /// </summary>
        /// <typeparam name="T1">来源 表格目前显示的实体</typeparam>
        /// <param name="_ExpSourceUniqueField">来源的唯一字段</param>
        public void SetRelatedInfo<T1>(Expression<Func<T1, object>> _ExpSourceUniqueField)
        {
            RelatedInfo relatedInfo = new RelatedInfo();
            relatedInfo.SourceTableName = typeof(T1).Name;
            relatedInfo.TargetTableName = new KeyNamePair(typeof(T1).Name, string.Empty);

            relatedInfo.SourceUniqueField = _ExpSourceUniqueField.GetMemberInfo().Name;
            relatedInfo.TargetDisplayField = _ExpSourceUniqueField.GetMemberInfo().Name;
            if (!RelatedInfoList.Any(c => c.TargetTableName.Key == typeof(T1).Name))
            {
                RelatedInfoList.Add(relatedInfo);
            }
        }

        /// <summary>
        /// 设置关联信息（使用字符串参数）
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="FieldName">字段名</param>
        public void SetRelatedInfo(string TableName, string FieldName)
        {
            RelatedInfo relatedInfo = new RelatedInfo();
            relatedInfo.SourceTableName = TableName;
            relatedInfo.TargetTableName = new KeyNamePair(TableName, string.Empty);
            relatedInfo.SourceUniqueField = FieldName;
            relatedInfo.TargetDisplayField = FieldName;

            if (!RelatedInfoList.Any(c => c.TargetTableName.Key == TableName && c.SourceTableName == TableName && c.SourceUniqueField == FieldName))
            {
                RelatedInfoList.Add(relatedInfo);
            }
        }

        /// <summary>
        /// 设置关联单据信息
        /// </summary>
        /// <typeparam name="T1">来源 表格目前显示的实体</typeparam>
        /// <typeparam name="T2">目标 要打开的窗体用的实体</typeparam>
        /// <param name="_ExpSourceUniqueField">来源的唯一字段</param>
        /// <param name="_ExpTargetDisplayField">目标的显示字段</param>
        public void SetRelatedInfo<T1, T2>(Expression<Func<T1, object>> _ExpSourceUniqueField, Expression<Func<T2, object>> _ExpTargetDisplayField)
        {
            RelatedInfo relatedInfo = new RelatedInfo();
            relatedInfo.SourceTableName = typeof(T1).Name;
            relatedInfo.TargetTableName = new KeyNamePair(typeof(T2).Name, string.Empty);

            relatedInfo.SourceUniqueField = _ExpSourceUniqueField.GetMemberInfo().Name;
            relatedInfo.TargetDisplayField = _ExpTargetDisplayField.GetMemberInfo().Name;
            if (!RelatedInfoList.Any(c => c.TargetTableName.Key == typeof(T2).Name))
            {
                RelatedInfoList.Add(relatedInfo);
            }
        }

        /// <summary>
        /// 设置关联单据信息
        /// </summary>
        /// <typeparam name="T1">来源 表格目前显示的实体</typeparam>
        /// <param name="_ExpSourceUniqueField">来源的唯一字段</param>
        /// <param name="_ExpTargetDisplayField">目标的显示字段</param>
        public void SetRelatedInfo<T1>(Expression<Func<T1, object>> _ExpSourceUniqueField, Expression<Func<T1, object>> _ExpTargetDisplayField)
        {
            RelatedInfo relatedInfo = new RelatedInfo();
            relatedInfo.SourceTableName = typeof(T1).Name;
            relatedInfo.TargetTableName = new KeyNamePair(typeof(T1).Name, string.Empty);

            relatedInfo.SourceUniqueField = _ExpSourceUniqueField.GetMemberInfo().Name;
            relatedInfo.TargetDisplayField = _ExpTargetDisplayField.GetMemberInfo().Name;
            if (!RelatedInfoList.Any(c => c.TargetTableName.Key == typeof(T1).Name))
            {
                RelatedInfoList.Add(relatedInfo);
            }
        }

        /// <summary>
        /// 关联单据时 业务和数据表不一致时，需要指定业务表名
        /// </summary>
        public List<RelatedInfo> RelatedInfoList { get; set; } = new List<RelatedInfo>();

        /// <summary>
        /// 初始化自定义加载器
        /// </summary>
        private void InitializeCustomLoaders()
        {
            // 注册 tb_ProductionDemand 的自定义加载器
            RegisterCustomLoader(typeof(tb_ProductionDemand).Name, async (menuInfo, billNo) =>
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_ProductionDemand>()
                    .Includes(c => c.tb_ProductionDemandTargetDetails)
                    .Includes(c => c.tb_ProductionDemandDetails)
                    .Includes(c => c.tb_ProduceGoodsRecommendDetails)
                    .Includes(c => c.tb_ManufacturingOrders)
                    .Includes(c => c.tb_PurGoodsRecommendDetails)
                    .Includes(c => c.tb_productionplan)
                    .WhereIF(billNo.GetType() == typeof(long), c => c.PDID == billNo.ToLong())
                    .WhereIF(billNo.GetType() == typeof(string), c => c.PDNo == billNo.ToString())
                    .Single();
                await menuPowerHelper.ExecuteEvents(menuInfo, obj);
            });

            // 注册 tb_BOM_S 的自定义加载器
            RegisterCustomLoader(typeof(tb_BOM_S).Name, async (menuInfo, billNo) =>
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_S>()
                    .Includes(c => c.tb_BOM_SDetails, d => d.tb_BOM_SDetailSubstituteMaterials)
                    .Includes(c => c.view_ProdInfo)
                    .Includes(c => c.tb_BOM_SDetailSecondaries)
                    .WhereIF(billNo.GetType() == typeof(long), c => c.BOM_ID == billNo.ToLong())
                    .WhereIF(billNo.GetType() == typeof(string), c => c.BOM_No == billNo.ToString())
                    .Single();
                await menuPowerHelper.ExecuteEvents(menuInfo, obj);
            });
        }

        /// <summary>
        /// 注册自定义加载器，用于特殊实体的加载逻辑
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="loader">自定义加载器函数</param>
        public void RegisterCustomLoader(string tableName, Func<tb_MenuInfo, object, Task> loader)
        {
            if (customLoaders.ContainsKey(tableName))
            {
                customLoaders[tableName] = loader;
            }
            else
            {
                customLoaders.Add(tableName, loader);
            }
        }

        /// <summary>
        /// 用于要打开的窗体是由源来中的一个列名决定ID或编号，目标表名是由源表格中某一列的值来决定
        /// </summary>
        /// <param name="GridViewColumnFieldName">SourceBillNo</param>
        /// <param name="CurrentRowEntity">当前行实体</param>
        public void GuideToForm(string GridViewColumnFieldName, object CurrentRowEntity)
        {
            if (CurrentRowEntity is DataGridViewRow CurrentRow)
            {
                CurrentRowEntity = CurrentRow.DataBoundItem;
            }

            tb_MenuInfo relatedMenuInfo = null;
            RelatedInfo relatedRelationship = new RelatedInfo();

            if (ComplexType)
            {
                relatedRelationship = FindRelatedInfoComplex(GridViewColumnFieldName, CurrentRowEntity);
            }
            else
            {
                relatedRelationship = RelatedInfoList.FirstOrDefault(c => c.SourceUniqueField == GridViewColumnFieldName);
            }

            if (relatedRelationship != null)
            {
                string tableName = relatedRelationship.TargetTableName.Key;

                // 检查 Key 是否为 BizType 枚举值（如 "2"），如果是则需要转换为表名
                if (IsBizTypeEnumValue(tableName))
                {
                    tableName = ConvertBizTypeToTableName(tableName);
                }
                //预收款中，有点击打开 预付款单时 要用 CurrentRowEntity 中是否存收付款类型来区别。


                relatedMenuInfo = FindMenuByTableName(tableName);
                if (relatedMenuInfo == null)
                {
                    //库存跟踪的情况
                    #region
                    tableName = relatedRelationship.TargetTableName.Name;
                    relatedMenuInfo = FindMenuByTableName(tableName);
                    #endregion

                }

                if (relatedMenuInfo != null)
                {
                    var billno = CurrentRowEntity.GetPropertyValue(relatedRelationship.SourceUniqueField);
                    if (billno != null)
                    {
                        OpenTargetEntity(relatedMenuInfo, tableName, billno);
                    }
                }
                else
                {




                    MessageBox.Show($"未找到表 {tableName} 对应的菜单，请联系管理员。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// 判断字符串是否为 BizType 枚举值
        /// </summary>
        /// <param name="value">要检查的值</param>
        /// <returns>如果是 BizType 枚举值返回 true，否则返回 false</returns>
        private bool IsBizTypeEnumValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            // 尝试解析为整数
            if (int.TryParse(value, out int enumValue))
            {
                // 检查是否在 BizType 枚举范围内
                return Enum.IsDefined(typeof(BizType), enumValue);
            }

            return false;
        }

        /// <summary>
        /// 将 BizType 枚举值转换为对应的表名
        /// </summary>
        /// <param name="bizTypeEnumValue">BizType 枚举值的字符串表示（如 "2"）</param>
        /// <returns>对应的表名，如果转换失败返回 null</returns>
        private string ConvertBizTypeToTableName(string bizTypeEnumValue)
        {
            try
            {
                int bizTypeValue = int.Parse(bizTypeEnumValue);
                BizType bizType = (BizType)bizTypeValue;

                // 方法1：使用映射服务根据业务类型获取实体信息，直接获取表名
                var entityInfo = _mappingService.GetEntityInfo(bizType);
                if (entityInfo != null && !string.IsNullOrEmpty(entityInfo.TableName))
                {
                    return entityInfo.TableName;
                }

                // 方法2：如果映射服务没有找到，尝试从 RelatedInfo 中获取
                // 因为在使用 SetRelatedInfo 时，KeyNamePair 的 Name 可能已经包含表名
                var relatedInfo = RelatedInfoList
                    .FirstOrDefault(c => c.TargetTableName.Key == bizTypeEnumValue);

                if (relatedInfo != null && !string.IsNullOrEmpty(relatedInfo.TargetTableName.Name))
                {
                    return relatedInfo.TargetTableName.Name;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 在复杂类型模式下查找关联信息
        /// </summary>
        /// <param name="GridViewColumnFieldName">表格列字段名</param>
        /// <param name="CurrentRowEntity">当前行实体</param>
        /// <returns>找到的关联信息，如果未找到则返回null</returns>
        private RelatedInfo FindRelatedInfoComplex(string GridViewColumnFieldName, object CurrentRowEntity)
        {
            string targetTableKey = string.Empty;
            var result = ComplexTargtetField.FirstOrDefault(pair => pair.Value == GridViewColumnFieldName);

            if (result.Key != null)
            {
                targetTableKey = CurrentRowEntity.GetPropertyValue(result.Key).ToString();
                System.Diagnostics.Debug.WriteLine($"找到的键值对: Key = {result.Key}, Value = {result.Value}");

                var info = RelatedInfoList.FirstOrDefault(c => c.SourceUniqueField == GridViewColumnFieldName && c.TargetTableName.Key == targetTableKey);
                return info;
            }
            else
            {
                string billno = CurrentRowEntity.GetPropertyValue(GridViewColumnFieldName).ToString();
                return RelatedInfoList.FirstOrDefault(c => c.SourceUniqueField == GridViewColumnFieldName && c.SourceTableName == CurrentRowEntity.GetType().Name);
            }
        }

        /// <summary>
        /// 根据表名查找对应的菜单
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>找到的菜单信息，如果未找到则返回null</returns>
        private tb_MenuInfo FindMenuByTableName(string tableName  )
        {

            //tb_MenuInfo menuInfo = null;
            //if (entity.ContainsProperty(nameof(ReceivePaymentType)))
            //{
            //    string flag = ((SharedFlag)entity.GetPropertyValue(nameof(ReceivePaymentType)).ToInt()).ToString();
            //    var adjustedMenu = MainForm.Instance.MenuList
            //        .Where(m => m.IsVisble
            //            && m.EntityName == tableName
            //            && m.BIBaseForm == "BaseBillEditGeneric`2"
            //            && m.UIPropertyIdentifier == flag)
            //        .FirstOrDefault();

            //    if (adjustedMenu != null)
            //    {
            //        menuInfo = adjustedMenu;
            //    }
            //}
            //else
            //{
            //    var adjustedMenu = MainForm.Instance.MenuList
            //        .Where(m => m.IsVisble
            //            && m.EntityName == tableName
            //            && m.BIBaseForm == "BaseBillEditGeneric`2")
            //        .FirstOrDefault();

            //    if (adjustedMenu != null)
            //    {
            //        menuInfo = adjustedMenu;
            //    }
            //}

            //return menuInfo;

         
         var menuInfo = MainForm.Instance.MenuList
             .Where(m => m.IsVisble && m.EntityName == tableName && m.BIBaseForm == "BaseBillEditGeneric`2")
             .FirstOrDefault();
         
         if (menuInfo == null && SpecialMenuMappings.ContainsKey(tableName))
         {
             var className = SpecialMenuMappings[tableName];
             menuInfo = MainForm.Instance.MenuList
                 .Where(m => m.IsVisble && m.EntityName == tableName && m.ClassPath.Contains(className))
                 .FirstOrDefault();
         }
         
         return menuInfo;
        }

        /// <summary>
        /// 打开目标实体
        /// </summary>
        /// <param name="relatedMenuInfo">关联菜单信息</param>
        /// <param name="tableName">表名</param>
        /// <param name="billno">单据号（可以是ID或编号）</param>
        public async void OpenTargetEntity(tb_MenuInfo relatedMenuInfo, string tableName, object billno)
        {
            if (relatedMenuInfo == null)
            {
                MessageBox.Show("菜单信息为空，无法打开单据。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 首先尝试使用自定义加载器
            if (customLoaders.ContainsKey(tableName))
            {
                await customLoaders[tableName](relatedMenuInfo, billno);
                return;
            }

            // 尝试使用 EntityLoader 加载实体
            var entityType = _mappingService.GetEntityTypeByTableName(tableName);
            if (entityType != null)
            {
                var entity = _loader.LoadEntityInternal(entityType, billno);
                if (entity != null)
                {
                    // 处理有收付款类型的特殊情况
                    relatedMenuInfo = GetMenuInfoForPaymentType(relatedMenuInfo,entity, tableName);
                    
                    await menuPowerHelper.ExecuteEvents(relatedMenuInfo, entity);
                    return;
                }
            }

            // 如果无法通过映射服务加载，提供友好提示
            MessageBox.Show($"无法加载表 {tableName} 的数据，请检查实体映射配置。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 调整菜单以适应收付款类型
        /// 前面取过一次。再传入！！
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="tableName">表名</param>
        /// <returns>调整后的菜单信息</returns>
        private tb_MenuInfo GetMenuInfoForPaymentType(tb_MenuInfo relatedMenuInfo,object entity, string tableName)
        {
            tb_MenuInfo menuInfo = relatedMenuInfo;
            if (entity.ContainsProperty(nameof(ReceivePaymentType)))
            {
                string flag = ((SharedFlag)entity.GetPropertyValue(nameof(ReceivePaymentType)).ToInt()).ToString();
                var adjustedMenu = MainForm.Instance.MenuList
                    .Where(m => m.IsVisble
                        && m.EntityName == tableName
                        && m.BIBaseForm == "BaseBillEditGeneric`2"
                        && m.UIPropertyIdentifier == flag)
                    .FirstOrDefault();

                if (adjustedMenu != null)
                {
                    menuInfo = adjustedMenu;
                }
            }

            return menuInfo;
        }
    }
}
