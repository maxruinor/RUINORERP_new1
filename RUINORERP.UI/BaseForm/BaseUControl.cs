using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Model;
using Krypton.Workspace;
using Krypton.Navigator;
using RUINORERP.Business.Processor;
using RUINORERP.UI.UserCenter;

namespace RUINORERP.UI.BaseForm
{

    /// <summary>
    /// 应用于容器中，以UControl为基类 
    /// </summary>
    public partial class BaseUControl : UserControl
    {

        public BaseUControl()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 从工作台点击过来的时候，这个保存初始化时的查询参数
        ///  这个可用可不用。
        /// </summary>
        public object QueryDtoProxy { get; set; }


        

        protected virtual void Refreshs()
        {

        }

        /// <summary>
        /// 传查询参数对象，对象已经给了查询参数具体值，具体在窗体那边判断
        /// </summary>
        /// <param name="QueryParameters"></param>
        internal virtual void LoadQueryParametersToUI(object QueryParameters, QueryParameter nodeParameter = null)
        {
            Refreshs();
        }

        public virtual void Query(object QueryDto, bool UseNavQuery = false)
        {

        }

        public virtual void Query(bool UseNavQuery = false)
        {

        }

        /// <summary>
        /// 如果基础是一个列表查询时会加载集合。这里保存后面用
        /// </summary>
        //public BindingSource ListDataSource { get; set; }

        public System.Windows.Forms.BindingSource _ListDataSoure = null;

        [Description("列表中的要显示的数据来源[BindingSource]"), Category("自定属性"), Browsable(true)]
        /// <summary>
        /// 列表的数据源(实际要显示的)
        /// </summary>
        public System.Windows.Forms.BindingSource ListDataSoure
        {
            get { return _ListDataSoure; }
            set { _ListDataSoure = value; }
        }

        private QueryFilter _QueryConditionFilter = new QueryFilter();

        /// <summary>
        /// 查询条件  将来 把querypara优化掉
        /// </summary>
        public QueryFilter QueryConditionFilter { get => _QueryConditionFilter; set => _QueryConditionFilter = value; }


        #region 关闭窗体和退出

        private bool editflag;

        /// <summary>
        /// 是否为编辑 如果为是则true
        /// </summary>
        public bool Edited
        {
            get { return editflag; }
            set { editflag = value; }
        }

        /// <summary>
        /// esc退出窗体
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData) //激活回车键
        {
            int WM_KEYDOWN = 256;
            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)
            {
                switch (keyData)
                {
                    case Keys.Escape:
                        Exit(this);//csc关闭窗体
                        break;
                    case Keys.Enter:
                        Query();
                        break;
                }

            }
            //return false;
            var key = keyData & Keys.KeyCode;
            var modeifierKey = keyData & Keys.Modifiers;
            if (modeifierKey == Keys.Control && key == Keys.F)
            {
                // MessageBox.Show("Control+F is pressed");
                return true;

            }

            var otherkey = keyData & Keys.KeyCode;
            var othermodeifierKey = keyData & Keys.Modifiers;
            if (othermodeifierKey == Keys.Control && otherkey == Keys.F)
            {
                MessageBox.Show("Control+F is pressed");
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);

        }
        private void CloseTheForm(object thisform)
        {
            KryptonWorkspaceCell cell = MainForm.Instance.kryptonDockableWorkspace1.ActiveCell;
            if (cell == null)
            {
                cell = new KryptonWorkspaceCell();
                MainForm.Instance.kryptonDockableWorkspace1.Root.Children.Add(cell);
            }
            if ((thisform as Control).Parent is KryptonPage)
            {
                KryptonPage page = (thisform as Control).Parent as KryptonPage;
                MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
                page.Dispose();
            }
            else
            {
                Form frm = (thisform as Control).Parent.Parent as Form;
                frm.Close();
            }



            /*
           if (page == null)
           {
               //浮动

           }
           else
           {
               //活动内
               if (cell.Pages.Contains(page))
               {
                   cell.Pages.Remove(page);
                   page.Dispose();
               }
           }
           */
        }
        protected virtual void Exit(object thisform)
        {
            if (!Edited)
            {
                //退出
                CloseTheForm(thisform);
            }
            else
            {
                if (MessageBox.Show(this, "有数据没有保存\r\n你确定要退出吗?   这里是不是可以进一步提示 哪些内容没有保存？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    //退出
                    CloseTheForm(thisform);
                }
            }
        }

        #endregion



        /// <summary>
        /// 关联的菜单信息 实际是可以从点击时传入
        /// </summary>
        public tb_MenuInfo CurMenuInfo { get; set; }

        /// <summary>
        /// 子类T的类型
        /// </summary>
        public Type ClassGenericType { get; set; }

        private BaseListRunWay _runway;
        /// <summary>
        /// 窗体运行方式  在关联编辑功能时 这个好像没有起到作用。实际是在frmBaseEditList 这个中实现显示与隐藏。
        /// </summary>
        public BaseListRunWay Runway { get => _runway; set => _runway = value; }

        public virtual void SetSelect()
        {
            Runway = BaseListRunWay.选中模式;
        }


    }
}
