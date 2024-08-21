using ComponentFactory.Krypton.Toolkit;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.QueryDto;
using RUINORERP.UI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common.Extensions;
using RUINORERP.Common;
using RUINORERP.Common.Helper;
using System.Linq.Expressions;
using ComponentFactory.Krypton.Workspace;
using ComponentFactory.Krypton.Navigator;
using System.Collections.Concurrent;
using System.Reflection;
using System.Diagnostics;

namespace RUINORERP.UI.AdvancedQuery
{
    public partial class frmAdvQuery : KryptonForm
    {
        public frmAdvQuery()
        {
            InitializeComponent();
            this.BaseToolStrip.ItemClicked += ToolStrip1_ItemClicked;
        }

       BaseEntityDto queryDto = new BaseEntityDto();

        public BaseEntityDto QueryDto { get => queryDto; set => queryDto = value; }

        private string dtoEntityTalbeName;
        private Type dtoEntityType; 

        public void SetBaseValue<T>()
        {
            string tableName = typeof(T).Name;
            DtoEntityTalbeName = tableName;
            DtoEntityType = typeof(T);
            DtoEntityFieldNameList = UIHelper.GetDtoFieldNameList<T>();
            Refreshs();
        }

        /*
         用 PropertyType.IsGenericType 决定property是否是generic类型
用 ProprtyType.GetGenericTypeDefinition() == typeof(Nullable<>) 检测它是否是一个nullable类型
用 PropertyType.GetGenericArguments() 获取基类型。
         
         */

        private ConcurrentDictionary<string, BaseDtoField> dtoEntityfieldNameList;


        /// <summary>
        /// 解决解体卡顿问题
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // 用双缓冲绘制窗口的所有子控件
                return cp;
            }
        }

        public ConcurrentDictionary<string, BaseDtoField> DtoEntityFieldNameList { get => dtoEntityfieldNameList; set => dtoEntityfieldNameList = value; }
        public string DtoEntityTalbeName { get => dtoEntityTalbeName; set => dtoEntityTalbeName = value; }
        public Type DtoEntityType { get => dtoEntityType; set => dtoEntityType = value; }

        private void frmAdvQuery_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
               | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel1, true, null);

            tableLayoutPanel1.Visible = false;
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.SuspendLayout();
            LoadData();

            tableLayoutPanel1.ResumeLayout();
            tableLayoutPanel1.Visible = true;

        }


        public Type T;

        private void LoadData()
        {
          

            //tableLayoutPanel1.RowCount = queryDto.FieldNameList.Count;
            // 定义表格布局的行和列
            #region
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();

            int columnCount = 5; // 列数 
            int rowCount = DtoEntityFieldNameList.Count+1; // 行数 

            // 设置列宽 
            for (int i = 0; i < columnCount; i++)
            {
                if (i==2)
                {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50f / columnCount));
                }
                else
                {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columnCount));
                }
                
            }

            // 设置行高 
            for (int i = 0; i < rowCount; i++)
            {
                //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rowCount));
            }

            #endregion

            int row = 0;
            int col = 0;

            int rowcounter = 1;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (var item in DtoEntityFieldNameList)
            {

                var coldata = item.Value as BaseDtoField;
                if (rowcounter % 2 != 0)//奇数
                {
                    col = 0;
                }
                else
                {
                    row--;
                    col = 3;
                }

                if (row == 24)
                {
                    break;
                }
                KryptonLabel lbl = new KryptonLabel();
                lbl.Text = coldata.Caption;
                lbl.Dock = DockStyle.Right;
                tableLayoutPanel1.Controls.Add(lbl, col, row);

                //string tname = col.ColDataType.GetGenericTypeName();
                // tname = RUINORERP.Common.Helper.TypeHelper.GetTypeDisplayName(col.ColDataType);
                // We need to check whether the property is NULLABLE
                if (coldata.ColDataType.IsGenericType && coldata.ColDataType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                    coldata.ColDataType = coldata.ColDataType.GetGenericArguments()[0];
                }
                if (coldata.ColDataType.IsGenericType && coldata.ColDataType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    coldata.ColDataType = Nullable.GetUnderlyingType(coldata.ColDataType);
                    //如果类型是例如此代码可为空，返回int部分(底层类型)。如果只需要将对象转换为特定类型，则可以使用System.Convert.ChangeType方法。
                }

                EnumDataType edt = (EnumDataType)Enum.Parse(typeof(EnumDataType), coldata.ColDataType.Name);
                switch (edt)
                {
                    case EnumDataType.Boolean:


                        KryptonCheckBox chk = new KryptonCheckBox();
                        chk.Name = item.Key;
                        chk.Text = "";
                        tableLayoutPanel1.Controls.Add(chk, col + 1, row);
                        DataBindingHelper.BindData4CehckBox(QueryDto, item.Key.ToString(), chk, false);
                        break;
                    case EnumDataType.DateTime:
                        break;

                    case EnumDataType.Char:
                        break;
                    case EnumDataType.Single:
                        break;
                    case EnumDataType.Double:
                        break;
                    case EnumDataType.Decimal:
                        break;
                    case EnumDataType.SByte:
                        break;
                    case EnumDataType.Byte:
                        break;
                    case EnumDataType.Int16:
                    case EnumDataType.UInt16:
                    case EnumDataType.Int32:
                    case EnumDataType.UInt32:
                    case EnumDataType.Int64:
                    case EnumDataType.UInt64:
                        if (coldata.IsFKRelationAttribute)
                        {
                            KryptonComboBox cmb = new KryptonComboBox();
                            cmb.Name = item.Key;
                            cmb.Text = "";
                            cmb.Width = 150; 
                            tableLayoutPanel1.Controls.Add(cmb, col + 1, row);
                            //只处理需要缓存的表
                            if (CacheHelper.Manager.TableList.ContainsKey(coldata.FKTableName))
                            {

                                string key = CacheHelper.Manager.TableList[coldata.FKTableName].Split(':')[0];
                                string value = CacheHelper.Manager.TableList[coldata.FKTableName].Split(':')[1];

                                //这里加载时 是指定了相差的外键表的对应实体的类型 
                                PropertyInfo PI = ReflectionHelper.GetPropertyInfo(DtoEntityType, QueryDto, coldata.FKTableName);
                                if (PI != null)
                                {
                                    Type type = PI.PropertyType;
                                    //通过反射来执行类的静态方法
                                    DataBindingHelper dbh = new DataBindingHelper();

                                    MethodInfo mf = dbh.GetType().GetMethod("BindData4CmbRef").MakeGenericMethod(new Type[] { type });
                                    object[] args = new object[5] { QueryDto, key, value, coldata.FKTableName, cmb };
                                    mf.Invoke(dbh, args);

                                }
                            }
                        }
                        break;
                    case EnumDataType.IntPtr:
                        break;
                    case EnumDataType.UIntPtr:
                        break;
                    case EnumDataType.Object:
                        break;
                    case EnumDataType.String:

                        KryptonTextBox tb = new KryptonTextBox();
                        tb.Name = item.Key;
                        tb.Width = 150;
                        tableLayoutPanel1.Controls.Add(tb, col + 1, row);
                        DataBindingHelper.BindData4TextBox(QueryDto, item.Key.ToString(), tb, BindDataType4TextBox.Text, false);
                        break;
                    default:
                        break;
                }
                row++;
                rowcounter++;
            }
            sw.Stop();
            TimeSpan dt = sw.Elapsed;
            MainForm.Instance.uclog.AddLog("", "加载控制耗时：" + dt.TotalMilliseconds.ToString());
            // 设置控件位置和大小 
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Control control = tableLayoutPanel1.GetControlFromPosition(j, i);
                    if (control != null)
                    {
                        if (control is KryptonLabel)
                        {
                            control.Dock = DockStyle.Right;
                        }
                        else
                        {
                            control.Dock = DockStyle.Left;
                        }

                    }
                }
            }

            errorProviderForAllInput.DataSource = queryDto;
        }

        /// <summary>
        /// 不同情况，显示不同的可用情况
        /// </summary>
        internal void ToolBarEnabledControl(AdvQueryMenuItemEnums menu)
        {
            switch (menu)
            {

                case AdvQueryMenuItemEnums.关闭:
                    break;


                default:
                    break;
            }
        }
        protected virtual void ToolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DoButtonClick(EnumHelper.GetEnumByString<AdvQueryMenuItemEnums>(e.ClickedItem.Text));
        }

        /// <summary>
        /// 控制功能按钮
        /// </summary>
        /// <param name="p_Text"></param>
        protected virtual void DoButtonClick(AdvQueryMenuItemEnums menuItem)
        {
            switch (menuItem)
            {
                case AdvQueryMenuItemEnums.新增:
                    // Add();
                    break;
                case AdvQueryMenuItemEnums.关闭:
                    
                    Exit(this);
                    break;

                default:
                    break;
            }

            /*
            if (p_Text == "新增" || p_Text == "F9") Add();
            if (p_Text == "修改" || p_Text == "F10") Modify();
            if (p_Text == "删除" || p_Text == "Del") Delete();
            if (p_Text == "保存" || p_Text == "F12")
            {
                Save();
                //if (ReceiptCheck() == false) { return; }
                //ReceiptSave();
            }
            //if (p_Text == "放弃" || p_Text == "Escape") ReceiptCancel();
            //if (p_Text == "预览" || p_Text == "F6") ReceiptPreview();
            // if (p_Text == "高级查询") AdvancedQuery();
            // if (p_Text == "刷新") ReceiptRefresh();
            if (p_Text == "查询") Query();
            //if (p_Text == "首页" || p_Text == "Home") FirstPage();
            //if (p_Text == "上页" || p_Text == "PageUp") UpPage();
            //if (p_Text == "下页" || p_Text == "PageDown") DownPage();
            //if (p_Text == "末页" || p_Text == "End") LastPage(); 导出Excel
            //if (p_Text == "导出Excel") base.OutFastOfDataGridView(this.dataGridView1);

            if (p_Text == "关闭" || p_Text == "Esc" || p_Text == "退出") CloseTheForm();
            */
        }


        #region 定义所有工具栏的方法



        protected virtual void Delete()
        {

        }

        protected virtual void Modify()
        {

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
            }
            else
            {
                if (thisform is Form)
                {
                    Form frm = (thisform as Form);
                    frm.Close();
                }
                else
                {
                    Form frm = (thisform as Control).Parent.Parent as Form;
                    frm.Close();
                }


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
            //if (!Edited)
            //{
            //    //退出
            CloseTheForm(thisform);
            //}
            //else
            //{
            //    if (MessageBox.Show(this, "有数据没有保存\r\n你确定要退出吗?   这里是不是可以进一步提示 哪些内容没有保存？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            //    {
            //        //退出
            //        CloseTheForm(thisform);
            //    }
            //}
        }

        protected virtual void Refreshs()
        {

        }
        #endregion

        public void BindData<T>(BaseEntity entity)
        {

            //DataBindingHelper.BindData4Cmb<tb_LocationType>(entity, k => k.LocationType_ID, v => v.TypeName, txtLocationType_ID);
            // DataBindingHelper.BindData4TextBox<tb_Location>(entity, t => t.Name, txtName, BindDataType4TextBox.Text, false);
            // DataBindingHelper.BindData4TextBox<tb_Location>(entity, t => t.Desc, txtDesc, BindDataType4TextBox.Text, false);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.actionStatus == ActionStatus.新增 || entity.actionStatus == ActionStatus.修改)
            {
                // base.InitRequiredToControl<tb_Location>(new tb_LocationValidator(), kryptonPanel1.Controls);
                // base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
