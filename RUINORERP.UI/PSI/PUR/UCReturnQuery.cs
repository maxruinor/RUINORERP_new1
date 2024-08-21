using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using RUINORERP.Model;
using RUINORERP.Global;
using RUINORERP.Business;
using RUINORERP.AutoMapper;
using AutoMapper;
using RUINORERP.Common.CollectionExtension;


namespace RUINORERP.UI.PSI.SAL
{

    [MenuAttrAssemblyInfo("采购入库查询", ModuleMenuDefine.模块定义.进销存管理, ModuleMenuDefine.进销存管理.采购管理, BizType.采购入库单)]
    public partial class UCPurEntryQuery : BaseBillQueryGeneric<tb_PurEntry>
    {
        public UCPurEntryQuery()
        {
            InitializeComponent();
            InitListData();
        }

        object newDto;

        protected async override void Query()
        {
            tb_PurEntryController<tb_PurEntry> ctr = Startup.GetFromFac<tb_PurEntryController<tb_PurEntry>>();
            newDto = new tb_PurEntry();
            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            //既然前台指定的查询哪些字段，到时可以配置。这里应该是 除软件删除外的。其它字段不需要
            List<tb_PurEntry> list = await ctr.BaseQueryByAdvancedNavWithConditionAsync(true, QueryConditions, newDto) as List<tb_PurEntry>;
            bindingSourceList.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            newSumDataGridView1.DataSource = bindingSourceList;
        }



        /// <summary>
        /// 查询条件 但是这里结果显示的更适合于单据，应该要做一个基础数据的UI
        /// </summary>
        public override void QueryConditionBuilder()
        {
            //base.QueryConditions.Add(c => c.ProductNo);
            //base.QueryConditions.Add(c => c.CNName);
            //base.QueryConditions.Add(c => c.Created_at);
            //base.QueryConditions.Add(c => c.Created_by);
        }


        /// <summary>
        /// 初始化列表数据
        /// </summary>
        internal void InitListData()
        {
            base.newSumDataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            base.newSumDataGridView1.XmlFileName = this.Name + nameof(tb_PurEntry);
            base.newSumDataGridView1.FieldNameList = UIHelper.GetFieldNameColList(typeof(tb_PurEntry));
            bindingSourceList.DataSource = new List<tb_PurEntry>();
            base.newSumDataGridView1.DataSource = null;
            //绑定导航
            base.newSumDataGridView1.DataSource = bindingSourceList.DataSource;
        }
    }



}
