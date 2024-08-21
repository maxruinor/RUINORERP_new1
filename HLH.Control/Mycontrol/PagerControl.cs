using System;
using System.Windows.Forms;

namespace HLH.WinControl
{

    /// <summary>
    /// 申明委托 为什么以int为结果类型返回，暂时不知道作用
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public delegate void OnPageChangedHandler(EventPagingArg e);

    /// <summary>
    /// winfrom 分页控件
    /// 20161101根据网上代码修改
    /// 只要知道  总记录数 每页显示数
    /// </summary>
    public partial class PagerControl : UserControl
    {
        #region 构造函数

        public PagerControl()
        {
            InitializeComponent();
        }

        #endregion



        #region 分页字段和属性

        private int pageIndex = 1;
        /// <summary>
        /// 当前页面
        /// </summary>
        public virtual int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }

        private int pageSize = 50;
        /// <summary>
        /// 每页记录数
        /// </summary>
        public virtual int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        private int recordCount = 0;
        /// <summary>
        /// 总记录数
        /// </summary>
        public virtual int RecordCount
        {
            get { return recordCount; }
            set
            {
                DrawControl(value);
                recordCount = value;
            }
        }

        private int pageCount = 0;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (pageSize != 0)
                {
                    pageCount = GetPageCount();
                }
                return pageCount;
            }
        }

        private string jumpText;
        /// <summary>
        /// 跳转按钮文本
        /// </summary>
        public string JumpText
        {
            get
            {
                if (string.IsNullOrEmpty(jumpText))
                {
                    jumpText = "Go";
                }
                return jumpText;
            }
            set { jumpText = value; }
        }


        #endregion

        #region 页码变化触发事件
        //  public event EventPagingHandler EventPaging;
        public event OnPageChangedHandler OnPageChanged;

        #endregion

        #region 分页及相关事件功能实现

        private void SetFormCtrEnabled()
        {
            lnkFirst.Enabled = true;
            lnkPrev.Enabled = true;
            lnkNext.Enabled = true;
            lnkLast.Enabled = true;
            btnGo.Enabled = true;
        }

        /// <summary>
        /// 计算总页数
        /// </summary>
        /// <returns></returns>
        private int GetPageCount()
        {
            if (PageSize == 0)
            {
                return 0;
            }
            int pageCount = RecordCount / PageSize;
            if (RecordCount % PageSize == 0)
            {
                pageCount = RecordCount / PageSize;
            }
            else
            {
                pageCount = RecordCount / PageSize + 1;
            }
            return pageCount;
        }
        /// <summary>
        /// 外部调用
        /// </summary>
        public void DrawControl(int count)
        {
            recordCount = count;
            DrawControl(false);
        }
        /// <summary>
        /// 页面控件呈现
        /// </summary>
        private void DrawControl(bool callEvent)
        {
            btnGo.Text = JumpText;
            lblCurrentPage.Text = PageIndex.ToString();
            lblPageCount.Text = PageCount.ToString();
            lblTotalCount.Text = RecordCount.ToString();
            txtPageSize.Text = PageSize.ToString();

            if (callEvent && OnPageChanged != null)
            {
                OnPageChanged(new EventPagingArg(this.PageIndex));//当前分页数字改变时，触发委托事件

            }
            SetFormCtrEnabled();
            if (PageCount == 1)//有且仅有一页
            {
                lnkFirst.Enabled = false;
                lnkPrev.Enabled = false;
                lnkNext.Enabled = false;
                lnkLast.Enabled = false;
                btnGo.Enabled = false;
            }
            else if (PageIndex == 1)//第一页
            {
                lnkFirst.Enabled = false;
                lnkPrev.Enabled = false;
            }
            else if (PageIndex == PageCount)//最后一页
            {
                lnkNext.Enabled = false;
                lnkLast.Enabled = false;
            }
        }

        #endregion

        #region 相关控件事件

        private void lnkFirst_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PageIndex = 1;
            DrawControl(true);
        }

        private void lnkPrev_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PageIndex = Math.Max(1, PageIndex - 1);
            DrawControl(true);
        }

        private void lnkNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PageIndex = Math.Min(PageCount, PageIndex + 1);
            DrawControl(true);
        }

        private void lnkLast_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PageIndex = PageCount;
            DrawControl(true);
        }

        /// <summary>
        /// enter键功能
        /// </summary>
        private void txtPageNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            btnGo_Click(null, null);
        }

        /// <summary>
        /// 跳转页数限制
        /// </summary>
        private void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int num = 0;
            if (int.TryParse(txtPageNum.Text.Trim(), out num) && num > 0)
            {
                if (num > PageCount)
                {
                    txtPageNum.Text = PageCount.ToString();
                }
            }
        }

        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            int num = 0;
            if (int.TryParse(txtPageNum.Text.Trim(), out num) && num > 0)
            {
                PageIndex = num;
                DrawControl(true);
            }
        }

        #endregion
        bool isTextChanged = false;
        /// <summary>
        /// 分页属性改变了。
        /// </summary>
        private void txtPageSize_TextChanged(object sender, EventArgs e)
        {
            int num = 0;
            if (!int.TryParse(txtPageSize.Text.Trim(), out num) || num <= 0)
            {
                //  num = this.PageSize;
                //  txtPageSize.Text = num.ToString();
            }
            else
            {
                isTextChanged = true;
            }
            if (num != 0)
            {
                pageSize = num;
            }

        }
        /// <summary>
        /// 光标离开分页属性
        private void txtPageSize_Leave(object sender, EventArgs e)
        {
            int num = 0;
            if (!int.TryParse(txtPageSize.Text.Trim(), out num) || num <= 0)
            {
                num = this.PageSize;
                txtPageSize.Text = num.ToString();
            }
            //改变每页记录时  不执行。手动执行
            //if (isTextChanged)
            //{
            //    isTextChanged = false;
            //    lnkFirst_LinkClicked(null, null);
            //}
        }
    }


}