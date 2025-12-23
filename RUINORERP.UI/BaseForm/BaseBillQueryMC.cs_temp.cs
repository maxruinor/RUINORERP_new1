using Krypton.Docking;
using Krypton.Navigator;
using Krypton.Toolkit;
using Krypton.Workspace;
using Microsoft.Extensions.Logging;
using RUINOR.Core;
using RUINORERP.Business;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.RowLevelAuthService;
using RUINORERP.Business.Security;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.Common;
using RUINORERP.UI.FormProperty;
using RUINORERP.UI.HelpSystem;
using RUINORERP.UI.Network.Services;
using RUINORERP.UI.Report;
using RUINORERP.UI.UserCenter;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;
using CommonHelper = RUINORERP.UI.Common.CommonHelper;
using ContextMenuController = RUINORERP.UI.UControls.ContextMenuController;
using XmlDocument = System.Xml.XmlDocument;
using RUINORERP.UI.UserCenter;
using RUINORERP.UI.UserCenter.DataParts;
namespace RUINORERP.UI.BaseForm
{

    /// <summary>
    /// 主子表查询
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="C"></typeparam>
    public partial class BaseBillQueryMC<M, C> : BaseQuery, IContextMenuInfoAuth, IToolStripMenuInfoAuth where M : class, new() where C : class, new()
    {
        /// <summary>
        /// 统一状态管理器
        /// </summary>
        protected IUnifiedStateManager StateManager { get; set; }
        // 添加TodoListManager属性
        /// <summary>
        /// 工作台任务列表管理器
        /// </summary>
        protected TodoListManager TodoListManager { get; set; }
        
        public virtual List<ContextMenuController> AddContextMenu()
        {
            List<ContextMenuController> list = new List<ContextMenuController>();
            return list;
        }

        public virtual ToolStripItem[] AddExtendButton(tb_MenuInfo menuInfo)
        {
            System.Windows.Forms.ToolStripItem[] extendButtons = new System.Windows.Forms.ToolStripItem[] { };
            this.BaseToolStrip.Items.AddRange(extendButtons);
            return extendButtons;
        }


        /// <summary>
        /// 判断是否需要加载子表明细。为了将这个基类适应于单表单据。如付款申请单
        /// </summary>
        public bool HasChildData { get; set; } = true;

        public bool ResultAnalysis { get; set; } = false;

        private QueryFilter _QueryConditionFilter = new QueryFilter();

        /// <summary>
        /// 查询条件  将来 把querypara优化掉
        /// </summary>
        public QueryFilter QueryConditionFilter { get => _QueryConditionFilter; set => _QueryConditionFilter = value; }


        /// <summary>
        /// 传入的是M,即主表类型的实体数据 
        /// </summary>
        /// <param name="obj"></param>
        public delegate void QueryRelatedRowHandler(object obj, System.Windows.Forms.BindingSource bindingSource);

        /// <summary>
        /// 查询引用单据明细
        /// </summary>
        [Browsable(true), Description("查询引用单据明细")]
        public event QueryRelatedRowHandler OnQueryRelatedChild;



        /// <summary>
        /// 比方采购入库，对应采购入库明细，
        /// 相关联的是 采购订单的明细，主单先不管
        /// 如果为空，则认为没有关联引用数据
        /// 第三方的子表类型
        /// </summary>
        public Type ChildRelatedEntityType;



        /// <summary>
        /// 双击哪一列会跳到单据编辑菜单
        /// </summary>
        public Expression<Func<M, string>> RelatedBillEditCol { get; set; }



        public BaseBillQueryMC()
        {
            InitializeComponent();
            // 初始化状态管理器
            if (RUINORERP.Model.Context.ApplicationContext.Current != null)
            {
                this.StateManager = RUINORERP.Model.Context.ApplicationContext.Current.GetRequiredService<IUnifiedStateManager>();
                this.TodoListManager = RUINORERP.Model.Context.ApplicationContext.Current.GetRequiredService<TodoListManager>();
            }
        }

        // 处理菜单项点击事件
        private async void Item_Click(object sender, EventArgs e)
        {
            // 获取菜单项枚举值
            MenuItemEnums menuItem = MenuItemEnums.无;
            // 获取选中列表
            List<M> selectlist = GetSelectResult();
            
            switch (menuItem)
            {
                case MenuItemEnums.提交:
                    Submit();
                    // 添加同步代码
                    if (TodoListManager != null)
                    {
                        // 转换List<M>为List<TodoUpdate>并批量处理
                        foreach (var entity in selectlist)
                        {
                            try
                            {
                                // 尝试获取业务类型和主键ID
                                string bizType = typeof(M).Name;
                                // 默认假设主键为"ID"
                                var idProp = typeof(M).GetProperty("ID") ?? typeof(M).GetProperty("id");
                                if (idProp != null)
                                {
                                    object pkValue = idProp.GetValue(entity);
                                    if (pkValue != null && pkValue is long billId && billId > 0)
                                    {
                                        // 创建TodoUpdate对象
                                        TodoUpdate update = new TodoUpdate
                                        {
                                            UpdateType = TodoUpdateType.StatusChanged,
                                            BusinessType = bizType,
                                            BillId = billId
                                        };
                                        TodoListManager.ProcessUpdate(update);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"处理任务更新失败: {ex.Message}");
                            }
                        }
                    }
                    break;
                    
                case MenuItemEnums.审核:
                    // 审核逻辑...
                    if (TodoListManager != null)
                    {
                        // 转换List<M>为List<TodoUpdate>并批量处理
                        foreach (var entity in selectlist)
                        {
                            try
                            {
                                // 尝试获取业务类型和主键ID
                                string bizType = typeof(M).Name;
                                // 默认假设主键为"ID"
                                var idProp = typeof(M).GetProperty("ID") ?? typeof(M).GetProperty("id");
                                if (idProp != null)
                                {
                                    object pkValue = idProp.GetValue(entity);
                                    if (pkValue != null && pkValue is long billId && billId > 0)
                                    {
                                        // 创建TodoUpdate对象
                                        TodoUpdate update = new TodoUpdate
                                        {
                                            UpdateType = TodoUpdateType.Approved,
                                            BusinessType = bizType,
                                            BillId = billId
                                        };
                                        TodoListManager.ProcessUpdate(update);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"处理任务更新失败: {ex.Message}");
                            }
                        }
                    }
                    break;
                    
                case MenuItemEnums.反审:
                    // 反审逻辑...
                    if (TodoListManager != null)
                    {
                        // 转换List<M>为List<TodoUpdate>并批量处理
                        foreach (var entity in selectlist)
                        {
                            try
                            {
                                // 尝试获取业务类型和主键ID
                                string bizType = typeof(M).Name;
                                // 默认假设主键为"ID"
                                var idProp = typeof(M).GetProperty("ID") ?? typeof(M).GetProperty("id");
                                if (idProp != null)
                                {
                                    object pkValue = idProp.GetValue(entity);
                                    if (pkValue != null && pkValue is long billId && billId > 0)
                                    {
                                        // 创建TodoUpdate对象
                                        TodoUpdate update = new TodoUpdate
                                        {
                                            UpdateType = TodoUpdateType.StatusChanged,
                                            BusinessType = bizType,
                                            BillId = billId
                                        };
                                        TodoListManager.ProcessUpdate(update);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"处理任务更新失败: {ex.Message}");
                            }
                        }
                    }
                    break;
                    
                case MenuItemEnums.结案:
                    // 结案逻辑...
                    if (TodoListManager != null)
                    {
                        // 转换List<M>为List<TodoUpdate>并批量处理
                        foreach (var entity in selectlist)
                        {
                            try
                            {
                                // 尝试获取业务类型和主键ID
                                string bizType = typeof(M).Name;
                                // 默认假设主键为"ID"
                                var idProp = typeof(M).GetProperty("ID") ?? typeof(M).GetProperty("id");
                                if (idProp != null)
                                {
                                    object pkValue = idProp.GetValue(entity);
                                    if (pkValue != null && pkValue is long billId && billId > 0)
                                    {
                                        // 创建TodoUpdate对象
                                        TodoUpdate update = new TodoUpdate
                                        {
                                            UpdateType = TodoUpdateType.StatusChanged,
                                            BusinessType = bizType,
                                            BillId = billId
                                        };
                                        TodoListManager.ProcessUpdate(update);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"处理任务更新失败: {ex.Message}");
                            }
                        }
                    }
                    break;
                    
                case MenuItemEnums.删除:
                    // 删除逻辑...
                    if (TodoListManager != null)
                    {
                        // 转换List<M>为List<TodoUpdate>并批量处理
                        foreach (var entity in selectlist)
                        {
                            try
                            {
                                // 尝试获取业务类型和主键ID
                                string bizType = typeof(M).Name;
                                // 默认假设主键为"ID"
                                var idProp = typeof(M).GetProperty("ID") ?? typeof(M).GetProperty("id");
                                if (idProp != null)
                                {
                                    object pkValue = idProp.GetValue(entity);
                                    if (pkValue != null && pkValue is long billId && billId > 0)
                                    {
                                        // 创建TodoUpdate对象
                                        TodoUpdate update = new TodoUpdate
                                        {
                                            UpdateType = TodoUpdateType.Deleted,
                                            BusinessType = bizType,
                                            BillId = billId
                                        };
                                        TodoListManager.ProcessUpdate(update);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"处理任务更新失败: {ex.Message}");
                            }
                        }
                    }
                    break;
            }
        }
    }
}
