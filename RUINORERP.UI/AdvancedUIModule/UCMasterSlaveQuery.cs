using Krypton.Toolkit;
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
using Krypton.Workspace;
using Krypton.Navigator;
using System.Collections.Concurrent;
using System.Reflection;
using System.Diagnostics;
using SqlSugar;
using System.Linq.Dynamic.Core;
using System.Reflection.Emit;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global;
using AutoMapper;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business;
using RUINORERP.Common.CollectionExtension;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.AdvancedUIModule
{
    /// <summary>
    /// 初步判断 没有使用了。
    /// </summary>
    /// <typeparam name="T">实体类</typeparam>
    /// <typeparam name="Q">查询的dto</typeparam>
    public partial class UCMasterSlaveQuery<T, Q> : UserControl where T : class
    {
        public UCMasterSlaveQuery()
        {
            InitializeComponent();
            InitListData();
            SetBaseValue();
            this.BaseToolStrip.ItemClicked += ToolStrip1_ItemClicked;
            this.bindingSourceList.ListChanged += BindingSourceList_ListChanged;
            string tableName = typeof(T).Name;
            DtoEntityTalbeName = tableName;
            DtoEntityType = typeof(T);
            DtoEntityFieldNameList = UIHelper.GetDtoFieldNameList<T>().OrderBy(v => v.ColName).ToList();

        }



        public delegate void SelectDataRowHandler(T entity);

        [Browsable(true), Description("双击将数据载入到明细外部事件")]
        public event SelectDataRowHandler OnSelectDataRow;


        /// <summary>
        /// 设置关联表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">可为空用另一个两名方法无参的</param>
       // public void SetBaseValue(Expression<Func<T, string>> exp)
        public void SetBaseValue()
        {
            //  var mb = exp.GetMemberInfo();
            // string key = mb.Name;
            string tableName = typeof(T).Name;


            //这里是不是与那个缓存 初始化时的那个字段重复？
            ///显示列表对应的中文
            FieldNameList = UIHelper.GetFieldNameColList(typeof(T));//  UIHelper.GetFieldNameList<T>();

            //重构？
            dataGridView1.XmlFileName = tableName;
        }
        private void BindingSourceList_ListChanged(object sender, ListChangedEventArgs e)
        {
            BaseEntity entity = new BaseEntity();
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    break;
                case ListChangedType.ItemAdded:
                    //如果这里为空出错， 需要先查询一个空的。绑定一下数据源的类型。之前是默认查询了所有
                    entity = bindingSourceList.List[e.NewIndex] as BaseEntity;
                    entity.ActionStatus = ActionStatus.新增;
                    break;
                case ListChangedType.ItemDeleted:
                    if (e.NewIndex < bindingSourceList.Count)
                    {
                        entity = bindingSourceList.List[e.NewIndex] as BaseEntity;
                        entity.ActionStatus = ActionStatus.删除;
                    }
                    break;
                case ListChangedType.ItemMoved:
                    break;
                case ListChangedType.ItemChanged:
                    entity = bindingSourceList.List[e.NewIndex] as BaseEntity;
                    if (entity.ActionStatus == ActionStatus.无操作)
                    {
                        entity.ActionStatus = ActionStatus.修改;
                    }
                    break;
                case ListChangedType.PropertyDescriptorAdded:
                    break;
                case ListChangedType.PropertyDescriptorDeleted:
                    break;
                case ListChangedType.PropertyDescriptorChanged:
                    break;
                default:
                    break;
            }
        }

        public delegate void AdvQueryHandler<E>(bool useLike, BaseEntityDto dto);

        [Browsable(true), Description("引发外部查询事件")]
        public event AdvQueryHandler<T> OnAdvQueryEvent;

        //public delegate void CheckHandler<E>();

        //[Browsable(true), Description("引发外部事件")]
        //public event CheckHandler<T> CheckEvent;


        //private BaseEntityDto queryDto = new BaseEntityDto();

        // public BaseEntityDto QueryDto { get => queryDto; set => queryDto = value; }

        private BaseEntityDto queryDto = new BaseEntityDto();

        public BaseEntityDto QueryDto { get => queryDto; set => queryDto = value; }

        private string dtoEntityTalbeName;
        private Type dtoEntityType;


        private static void Test(BaseEntityDto dto)
        {
            // 获取程序集中继承抽象实体类的实体类
            // Type[] tt = new Type[2];
            //  tt.Where(x => x.IsAssignableTo(typeof(Entity))); //Entity 抽象实体类
            // 遍历实体类


            // 租户动态处理，同上

        }

        /*
         用 PropertyType.IsGenericType 决定property是否是generic类型
用 ProprtyType.GetGenericTypeDefinition() == typeof(Nullable<>) 检测它是否是一个nullable类型
用 PropertyType.GetGenericArguments() 获取基类型。
         
         */

        private List<BaseDtoField> dtoEntityfieldNameList;


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

        public List<BaseDtoField> DtoEntityFieldNameList { get => dtoEntityfieldNameList; set => dtoEntityfieldNameList = value; }


        // [AdvAttribute("asd")]
        public string DtoEntityTalbeName { get => dtoEntityTalbeName; set => dtoEntityTalbeName = value; }

        public Type DtoEntityType { get => dtoEntityType; set => dtoEntityType = value; }



        /// <summary>
        /// 是否使用模糊查询  思路是动态创建一个类型，动态添加一些特殊的属性，后面再检测出来对应处理
        /// </summary>
        /// <param name="useLike"></param>
        private void BindData(bool useLike)
        {
            var type = typeof(Q);

            var aName = new System.Reflection.AssemblyName(Assembly.GetExecutingAssembly().GetName().Name);
            var ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
            var mb = ab.DefineDynamicModule(aName.Name);
            var tb = mb.DefineType(type.Name + "Proxy", System.Reflection.TypeAttributes.Public, type);

            #region 前期处理

            if (useLike)
            {
                #region 模糊查询
                //这里构建AdvExtQueryAttribute一个构造函数，注意参数个数
                var attrCtorParams = new Type[] { typeof(string), typeof(string), typeof(string), typeof(AdvQueryProcessType) };
                var attrCtorInfo = typeof(AdvExtQueryAttribute).GetConstructor(attrCtorParams);
                var DtoEntityFieldNameList = UIHelper.GetDtoFieldNameList(type);
                foreach (var oldCol in DtoEntityFieldNameList)
                {
                    var coldata = oldCol as BaseDtoField;
                    if (coldata.ColDataType.IsGenericType && coldata.ColDataType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        coldata.ColDataType = Nullable.GetUnderlyingType(coldata.ColDataType);
                        //如果类型是例如此代码可为空，返回int部分(底层类型)。如果只需要将对象转换为特定类型，则可以使用System.Convert.ChangeType方法。
                    }
                    if (coldata.ColDataType.Name == "Byte[]")
                    {
                        continue;
                    }
                    EnumDataType edt = (EnumDataType)Enum.Parse(typeof(EnumDataType), coldata.ColDataType.Name);
                    switch (edt)
                    {
                        case EnumDataType.Boolean:
                            string newBoolProName1 = coldata.ColName + "_Enable";
                            var attrBoolBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.ColName, "是", newBoolProName1, AdvQueryProcessType.useYesOrNoToAll });
                            //动态属性要提前创建生成，后面要实体化传入控件
                            PropertyBuilder newBoolProp1 = AddProperty(tb, newBoolProName1);
                            newBoolProp1.SetCustomAttribute(attrBoolBuilder1);
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
                        case EnumDataType.IntPtr:
                        case EnumDataType.UIntPtr:
                            //下拉
                            if (coldata.IsFKRelationAttribute)
                            {
                                string newSelectProName1 = coldata.ColName + "_请选择";
                                var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.ColName, "请选择", newSelectProName1, AdvQueryProcessType.defaultSelect });
                                //动态属性要提前创建生成，后面要实体化传入控件
                                PropertyBuilder newlikeProp1 = AddProperty(tb, newSelectProName1);
                                newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                            }
                            break;
                        case EnumDataType.Object:
                            break;
                        case EnumDataType.String:
                            //if (coldata.UseLike)
                            //{
                            string newlikeProNameString = coldata.ColName + "_Like";
                            var attrlikeBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.ColName, "like", newlikeProNameString, AdvQueryProcessType.stringLike });
                            //动态属性要提前创建生成，后面要实体化传入控件
                            PropertyBuilder newlikePropstring = AddProperty(tb, newlikeProNameString);
                            newlikePropstring.SetCustomAttribute(attrlikeBuilder1);
                            break;
                        case EnumDataType.DateTime:

                            string newProName1 = coldata.ColName + "_Start";
                            string newProName2 = coldata.ColName + "_End";
                            var attrBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.ColName, "时间起", newProName1, AdvQueryProcessType.datetimeRange });
                            var attrBuilder2 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.ColName, "时间止", newProName2, AdvQueryProcessType.datetimeRange });
                            //动态属性要提前创建生成，后面要实体化传入控件
                            PropertyBuilder newProp1 = AddProperty(tb, newProName1);
                            //动态属性要提前创建生成，后面要实体化传入控件
                            PropertyBuilder newProp2 = AddProperty(tb, newProName2);
                            newProp1.SetCustomAttribute(attrBuilder1);
                            newProp2.SetCustomAttribute(attrBuilder2);
                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }
            else
            {
                #region 普通查询
                //这里构建AdvExtQueryAttribute一个构造函数，注意参数个数
                var attrCtorParams = new Type[] { typeof(string), typeof(string), typeof(string), typeof(AdvQueryProcessType) };
                var attrCtorInfo = typeof(AdvExtQueryAttribute).GetConstructor(attrCtorParams);
                var DtoEntityFieldNameList = UIHelper.GetDtoFieldNameList(type);
                foreach (var oldCol in DtoEntityFieldNameList)
                {
                    var coldata = oldCol as BaseDtoField;
                    if (coldata.ColDataType.IsGenericType && coldata.ColDataType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        coldata.ColDataType = Nullable.GetUnderlyingType(coldata.ColDataType);
                        //如果类型是例如此代码可为空，返回int部分(底层类型)。如果只需要将对象转换为特定类型，则可以使用System.Convert.ChangeType方法。
                    }
                    EnumDataType edt = (EnumDataType)Enum.Parse(typeof(EnumDataType), coldata.ColDataType.Name);
                    switch (edt)
                    {
                        case EnumDataType.Boolean:
                            //string newBoolProName1 = coldata.ColName + "_Enable";
                            //var attrBoolBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.ColName, "是", newBoolProName1, AdvQueryProcessType.useYesOrNoToAll });
                            ////动态属性要提前创建生成，后面要实体化传入控件
                            //PropertyBuilder newBoolProp1 = AddProperty(tb, newBoolProName1);
                            //newBoolProp1.SetCustomAttribute(attrBoolBuilder1);
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
                        case EnumDataType.IntPtr:
                        case EnumDataType.UIntPtr:
                            //下拉
                            if (coldata.IsFKRelationAttribute)
                            {
                                string newSelectProName1 = coldata.ColName + "_请选择";
                                var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.ColName, "请选择", newSelectProName1, AdvQueryProcessType.defaultSelect });
                                //动态属性要提前创建生成，后面要实体化传入控件
                                PropertyBuilder newlikeProp1 = AddProperty(tb, newSelectProName1);
                                newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                            }
                            break;
                        case EnumDataType.Object:
                            break;
                        case EnumDataType.String:
                            if (coldata.UseLike)
                            {
                                string newlikeProName1 = coldata.ColName + "_Like";
                                var attrlikeBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.ColName, "like", newlikeProName1, AdvQueryProcessType.stringLike });
                                //动态属性要提前创建生成，后面要实体化传入控件
                                PropertyBuilder newlikeProp1 = AddProperty(tb, newlikeProName1);
                                newlikeProp1.SetCustomAttribute(attrlikeBuilder1);
                            }
                            break;
                        case EnumDataType.DateTime:

                            //string newProName1 = coldata.ColName + "_Start";
                            //string newProName2 = coldata.ColName + "_End";
                            //var attrBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.ColName, "时间起", newProName1, AdvQueryProcessType.datetimeRange });
                            //var attrBuilder2 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.ColName, "时间止", newProName2, AdvQueryProcessType.datetimeRange });
                            ////动态属性要提前创建生成，后面要实体化传入控件
                            //PropertyBuilder newProp1 = AddProperty(tb, newProName1);
                            ////动态属性要提前创建生成，后面要实体化传入控件
                            //PropertyBuilder newProp2 = AddProperty(tb, newProName2);
                            //newProp1.SetCustomAttribute(attrBuilder1);
                            //newProp2.SetCustomAttribute(attrBuilder2);
                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }



            #endregion

            Type newtype = tb.CreateType();
            object newDto = Activator.CreateInstance(newtype);
            QueryDto = newDto as BaseEntityDto;
            var new_DtoEntityFieldNameList = UIHelper.GetDtoFieldNameList(newtype);
            var neworderlist = new_DtoEntityFieldNameList.OrderBy(d => d.ExtendedAttribute.Count);

            // 定义表格布局的行和列
            //第一行给了固定的按钮 高30px
            #region
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();

            int columnCount = 5; // 列数 
                                 //int rowCount = DtoEntityFieldNameList.Count + 1; // 行数 
            int rowCount = new_DtoEntityFieldNameList.Count + 2; // 行数 
            //设置列宽
            for (int i = 0; i < columnCount; i++)
            {
                if (i == 0 || i == 3)
                {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 87f));
                }
                if (i == 1 || i == 4)
                {
                    //tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columnCount));
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                }
                if (i == 2)
                {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
                }
            }

            // 设置行高 
            for (int i = 0; i < rowCount; i++)
            {
                /// tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rowCount));
            }

            #endregion

            int row = 1;// 本来是0开始  现在1开始因为0给了固定按钮 //第一行给了固定的按钮 高30px
            tableLayoutPanel1.Controls.Add(chkUseLike, 0, 0);
            tableLayoutPanel1.Controls.Add(kryptonbtnQuery, 1, 0);
            int col = 0;

            int rowcounter = 1;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (var item in neworderlist)
            {

                var coldata = item as BaseDtoField;
                if (rowcounter % 2 != 0)//奇数
                {
                    col = 0;
                }
                else
                {
                    row--;
                    col = 3;
                }
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

                KryptonLabel lbl = new KryptonLabel();
                lbl.Text = coldata.Caption;
                lbl.Dock = DockStyle.Right;
                tableLayoutPanel1.Controls.Add(lbl, col, row);

                //if (coldata.ExtendedAttribute.Count == 0)
                //{

                //}

                EnumDataType edt = (EnumDataType)Enum.Parse(typeof(EnumDataType), coldata.ColDataType.Name);
                switch (edt)
                {
                    case EnumDataType.Boolean:
                        if (item.UseLike)
                        {
                            UCAdvYesOrNO chkgroup = new UCAdvYesOrNO();
                            tableLayoutPanel1.Controls.Add(chkgroup, col + 1, row);
                            object datetimeValue1 = ReflectionHelper.GetPropertyValue(newDto, coldata.ExtendedAttribute[0].ColName);
                            chkgroup.rdb1.Text = "是";
                            chkgroup.rdb2.Text = "否";
                            DataBindingHelper.BindData4CheckBox(newDto, coldata.ExtendedAttribute[0].ColName, chkgroup.chk, false);
                            DataBindingHelper.BindData4RadioGroupTrueFalse(newDto, coldata.ExtendedAttribute[0].RelatedFields, chkgroup.rdb1, chkgroup.rdb2);
                        }
                        else
                        {
                            KryptonCheckBox chk = new KryptonCheckBox();
                            chk.Name = item.ColName;
                            chk.Text = "";
                            tableLayoutPanel1.Controls.Add(chk, col + 1, row);
                            //newDto
                            DataBindingHelper.BindData4CheckBox(newDto, item.ColName, chk, false);
                        }

                        break;
                    case EnumDataType.DateTime:
                        if (item.UseLike)
                        {
                            UCAdvDateTimerPickerGroup dtpgroup = new UCAdvDateTimerPickerGroup();
                            tableLayoutPanel1.Controls.Add(dtpgroup, col + 1, row);
                            object datetimeValue1 = ReflectionHelper.GetPropertyValue(newDto, coldata.ExtendedAttribute[0].ColName);
                            DataBindingHelper.BindData4DataTime(newDto, datetimeValue1, coldata.ExtendedAttribute[0].ColName, dtpgroup.dtp1, false);
                            object datetimeValue2 = ReflectionHelper.GetPropertyValue(newDto, coldata.ExtendedAttribute[1].ColName);
                            DataBindingHelper.BindData4DataTime(newDto, datetimeValue2, coldata.ExtendedAttribute[1].ColName, dtpgroup.dtp2, false);
                        }
                        else
                        {
                            KryptonDateTimePicker dtp = new KryptonDateTimePicker();
                            dtp.Name = item.ColName;
                            dtp.ShowCheckBox = true;
                            dtp.Checked = false;
                            tableLayoutPanel1.Controls.Add(dtp, col + 1, row);
                            object datetimeValue = ReflectionHelper.GetPropertyValue(newDto, item.ColName);
                            DataBindingHelper.BindData4DataTime(newDto, datetimeValue, item.ColName, dtp, false);
                        }

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
                            cmb.Name = item.ColName;
                            cmb.Text = "";
                            cmb.Width = 150;
                            tableLayoutPanel1.Controls.Add(cmb, col + 1, row);
                            //只处理需要缓存的表
                            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                            if (CacheHelper.Manager.NewTableList.TryGetValue(coldata.FKTableName, out pair))
                            {

                                string IDColName = pair.Key;
                                string ColName = pair.Value;
                                //DataBindingHelper.BindData4Cmb<T>(QueryDto, key, value, coldata.FKTableName, cmb);
                                //这里加载时 是指定了相差的外键表的对应实体的类型
                                //PropertyInfo PI = ReflectionHelper.GetPropertyInfo(DtoEntityType, newDto, coldata.FKTableName);
                                //if (PI != null)
                                //{
                                //    Type typeDatetime = PI.PropertyType;
                                //    //通过反射来执行类的静态方法
                                //    DataBindingHelper dbh = new DataBindingHelper();
                                //    MethodInfo mf = dbh.GetType().GetMethod("BindData4CmbRef").MakeGenericMethod(new Type[] { typeDatetime });
                                //    object[] args = new object[5] { newDto, IDColName, ColName, coldata.FKTableName, cmb };
                                //    mf.Invoke(dbh, args);
                                //}
                                PropertyInfo PI = ReflectionHelper.GetPropertyInfo(typeof(T), newDto, coldata.FKTableName.ToLower());
                                if (PI != null)
                                {
                                    Type typeDatetime = PI.PropertyType;
                                    //通过反射来执行类的静态方法
                                    DataBindingHelper dbh = new DataBindingHelper();
                                    MethodInfo mf = dbh.GetType().GetMethod("BindData4CmbRef").MakeGenericMethod(new Type[] { typeDatetime });
                                    object[] args = new object[5] { newDto, IDColName, ColName, coldata.FKTableName, cmb };
                                    mf.Invoke(dbh, args);
                                }
                                else
                                {

                                    MainForm.Instance.logger.LogError("动态加载外键数据时出错，" + typeof(T).Name + "在代理类中的属性名不对，自动生成规则可能有变化" + coldata.FKTableName.ToLower());
                                    MainForm.Instance.uclog.AddLog("加载数据出错。请联系管理员。");
                                }
                            }
                            //cmb.SelectedIndex = -1;
                        }
                        break;
                    case EnumDataType.IntPtr:
                        break;
                    case EnumDataType.UIntPtr:
                        break;
                    case EnumDataType.Object:
                        break;
                    case EnumDataType.String:
                        //if (item.UseLike)
                        //{
                        KryptonTextBox tb_box = new KryptonTextBox();
                        tb_box.Name = item.ColName;
                        tb_box.Width = 150;
                        tableLayoutPanel1.Controls.Add(tb_box, col + 1, row);
                        DataBindingHelper.BindData4TextBox(newDto, item.ColName, tb_box, BindDataType4TextBox.Text, false);

                        //}
                        //else
                        //{

                        //}
                        //KryptonTextBox tb_box = new KryptonTextBox();
                        //tb_box.Name = item.ColName;
                        //tb_box.Width = 150;
                        //tableLayoutPanel1.Controls.Add(tb_box, col + 1, row);
                        //DataBindingHelper.BindData4TextBox(QueryDto, item.ColName, tb_box, BindDataType4TextBox.Text, false);
                        break;
                    default:
                        break;
                }
                row++;
                rowcounter++;
            }
            sw.Stop();
            TimeSpan dt = sw.Elapsed;
            MainForm.Instance.uclog.AddLog("性能", "加载控件耗时：" + dt.TotalMilliseconds.ToString());
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
        }


        private PropertyBuilder AddProperty(TypeBuilder tb, string MemberName)
        {
            #region  动态创建字段


            // string MemberName = "Watson_ok";
            Type memberType = typeof(string);
            FieldBuilder fbNumber = tb.DefineField("m_" + MemberName, memberType, FieldAttributes.Private);


            PropertyBuilder pbNumber = tb.DefineProperty(
                MemberName,
                System.Reflection.PropertyAttributes.HasDefault,
                memberType,
                null);


            MethodAttributes getSetAttr = MethodAttributes.Public |
                MethodAttributes.SpecialName | MethodAttributes.HideBySig;


            MethodBuilder mbNumberGetAccessor = tb.DefineMethod(
                "get_" + MemberName,
                getSetAttr,
                memberType,
                Type.EmptyTypes);

            ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();

            numberGetIL.Emit(OpCodes.Ldarg_0);
            numberGetIL.Emit(OpCodes.Ldfld, fbNumber);
            numberGetIL.Emit(OpCodes.Ret);

            // Define the "set" accessor method for Number, which has no return
            // type and takes one argument of type int (Int32).
            MethodBuilder mbNumberSetAccessor = tb.DefineMethod(
                "set_" + MemberName,
                getSetAttr,
                null,
                new Type[] { memberType });

            ILGenerator numberSetIL = mbNumberSetAccessor.GetILGenerator();
            // Load the instance and then the numeric argument, then store the
            // argument in the field.
            numberSetIL.Emit(OpCodes.Ldarg_0);
            numberSetIL.Emit(OpCodes.Ldarg_1);
            numberSetIL.Emit(OpCodes.Stfld, fbNumber);
            numberSetIL.Emit(OpCodes.Ret);

            // Last, map the "get" and "set" accessor methods to the 
            // PropertyBuilder. The property is now complete. 
            pbNumber.SetGetMethod(mbNumberGetAccessor);
            pbNumber.SetSetMethod(mbNumberSetAccessor);
            #endregion

            return pbNumber;
        }

        private void mytest()
        {
            var type = typeof(T);

            var aName = new System.Reflection.AssemblyName(Assembly.GetExecutingAssembly().GetName().Name);
            var ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
            var mb = ab.DefineDynamicModule(aName.Name);
            var tb = mb.DefineType(type.Name + "Proxy", System.Reflection.TypeAttributes.Public, type);
            //动态创建字段
            FieldBuilder fb1 = tb.DefineField("usetime", typeof(System.String), FieldAttributes.Private);

            //PropertyInfo info = typeof(ClassA).GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);

            PropertyBuilder newProp = tb.DefineProperty("UseStartTime", System.Reflection.PropertyAttributes.None, typeof(bool), Type.EmptyTypes);

            object ReturnSumInst = Activator.CreateInstance(tb);


        }

        private void test()
        {
            var type = typeof(ClassA);

            var aName = new System.Reflection.AssemblyName(Assembly.GetExecutingAssembly().GetName().Name);
            var ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
            var mb = ab.DefineDynamicModule(aName.Name);
            var tb = mb.DefineType(type.Name + "Proxy", System.Reflection.TypeAttributes.Public, type);

            var attrCtorParams = new Type[] { typeof(string) };
            var attrCtorInfo = typeof(SomeAttribute).GetConstructor(attrCtorParams);
            var attrBuilder = new CustomAttributeBuilder(attrCtorInfo, new object[] { "Some Value" });
            tb.SetCustomAttribute(attrBuilder);

            //不成功
            //fb_usetime.SetCustomAttribute(attrBuilder);
            //PropertyInfo info = typeof(ClassA).GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
            //PropertyBuilder newProp = tb.DefineProperty("UseStartTime", System.Reflection.PropertyAttributes.None, typeof(bool), Type.EmptyTypes);

            ///SugarColumn
            // var attrSugarColumnCtorParams = new Type[] { typeof(string) };
            //var attrSugarColumnCtorInfo = typeof(SugarColumn).GetConstructor(Type.EmptyTypes);
            //var attrSugarColumnBuilder = new CustomAttributeBuilder(attrSugarColumnCtorInfo, null);

            // PropertyInfo  newPropertyInfo = newtype.GetProperty(MemberName);
            // mg.AddAttributes(pbNumber, memberType, newPropertyInfo);


            PropertyInfo info = typeof(ClassA).GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);

            PropertyBuilder newProp = tb.DefineProperty(info.Name, System.Reflection.PropertyAttributes.None, info.PropertyType, Type.EmptyTypes);

            newProp.SetCustomAttribute(attrBuilder);


            //var tbValue = mb.DefineType(info.Name, System.Reflection.TypeAttributes.Public, type);

            FieldBuilder ValueField = tb.DefineField("_Value", typeof(string), FieldAttributes.Private);
            MethodAttributes GetSetAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual;
            MethodBuilder ValuePropertyGet = tb.DefineMethod("get_Value", GetSetAttributes, typeof(string), Type.EmptyTypes);
            ILGenerator Generator = ValuePropertyGet.GetILGenerator();
            Generator.Emit(OpCodes.Ldarg_0);
            Generator.Emit(OpCodes.Ldfld, ValueField);
            Generator.Emit(OpCodes.Ret);

            MethodBuilder ValuePropertySet = tb.DefineMethod("set_Value", GetSetAttributes, null, new Type[] { typeof(string) });
            Generator = ValuePropertySet.GetILGenerator();
            Generator.Emit(OpCodes.Ldarg_0);
            Generator.Emit(OpCodes.Ldarg_1);
            Generator.Emit(OpCodes.Stfld, ValueField);
            Generator.Emit(OpCodes.Ret);


            newProp.SetSetMethod(ValuePropertySet);
            newProp.SetGetMethod(ValuePropertyGet);

            var newType = tb.CreateType();
            var instance = (ClassA)Activator.CreateInstance(newType);

            var attr = (SomeAttribute)instance.GetType().GetCustomAttributes(typeof(SomeAttribute), false).SingleOrDefault();

            var attr1 = (SomeAttribute)instance.Value.GetType().GetCustomAttributes(typeof(SomeAttribute), false).SingleOrDefault();

            var attr2 = (SomeAttribute)instance.GetType().GetProperty(nameof(instance.Value), BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).GetCustomAttributes(typeof(SomeAttribute), false).SingleOrDefault();
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
            MainForm.Instance.AppContext.log.ActionName = e.ClickedItem.Text.ToString();
            if (e.ClickedItem.Text.Length > 0)
            {
                DoButtonClick(EnumHelper.GetEnumByString<AdvQueryMenuItemEnums>(e.ClickedItem.Text));
            }
            else
            {

            }
        }

        /// <summary>
        /// 控制功能按钮
        /// </summary>
        /// <param name="p_Text"></param>
        protected virtual void DoButtonClick(AdvQueryMenuItemEnums menuItem)
        {
            //操作前将数据收集
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);
            switch (menuItem)
            {
                case AdvQueryMenuItemEnums.查询:
                    Query();
                    break;
                case AdvQueryMenuItemEnums.新增:
                    // Add();
                    break;
                case AdvQueryMenuItemEnums.关闭:
                    Exit(this);
                    break;

                default:
                    break;
            }

        }


        #region 定义所有工具栏的方法
        BaseController<T> ctr = Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
        protected async virtual void Query()
        {
            dataGridView1.ReadOnly = true;
            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            List<T> list = await ctr.BaseQueryByAdvancedNavAsync(true, QueryDto) as List<T>;
            ListDataSoure.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            dataGridView1.DataSource = ListDataSoure;
            ToolBarEnabledControl(AdvQueryMenuItemEnums.查询);
        }





        #endregion




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
                page.Hide(); //高级查询 如果移除会 工具栏失效一次，找不到原因。目前暂时隐藏处理
                             //如果上一级的窗体关闭则删除？
                             //MainForm.Instance.kryptonDockingManager1.RemovePage(page.UniqueName, true);
                             //page.Dispose();
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
            CloseTheForm(thisform);
        }




        /// <summary>
        /// 只为得到一个结构显示出来
        /// </summary>
        protected async void InitQueryForDg()
        {
            this.dataGridView1.FieldNameList = this.FieldNameList;
            dataGridView1.ReadOnly = true;
            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            List<T> list = await ctr.BaseQueryAsync("1>2");
            ListDataSoure.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            dataGridView1.DataSource = ListDataSoure;
         
            ToolBarEnabledControl(AdvQueryMenuItemEnums.查询);
        }


        /// <summary>
        /// 初始化列表数据
        /// </summary>
        internal void InitListData()
        {
            this.dataGridView1.DataSource = null;
            toolStripButtonSave.Enabled = false;
            ListDataSoure = bindingSourceList;
            //绑定导航
            this.bindingNavigatorList.BindingSource = ListDataSoure;

            this.dataGridView1.DataSource = ListDataSoure.DataSource;
        }


        //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
        ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();

        /// <summary>
        /// 固定的值显示，入库ture 出库false
        /// 每个列表对应的值 ，单独设置
        /// </summary>
        public ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDictionary { get => _DataDictionary; set => _DataDictionary = value; }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null)
            {
                e.Value = "";
                return;
            }
            //固定字典值显示
            string colDbName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (ColNameDataDictionary.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDictionary.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == e.Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        e.Value = kv.Value;
                    }

                }
            }


            //动态字典值显示
            string colName = UIHelper.ShowGridColumnsNameValue<T>(colDbName, e.Value);
            if (!string.IsNullOrEmpty(colName))
            {
                e.Value = colName;
            }

            //图片特殊处理
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Image")
            {
                if (e.Value != null)
                {
                    System.IO.MemoryStream buf = new System.IO.MemoryStream((byte[])e.Value);
                    Image image = Image.FromStream(buf, true);
                    e.Value = image;
                    //这里用缓存
                }
            }


        }

        #region 画行号

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewPaintParts paintParts =
                    e.PaintParts & ~DataGridViewPaintParts.Focus;

                e.Paint(e.ClipBounds, paintParts);
                e.Handled = true;
            }

            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);

                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
        }

        #endregion
        private ConcurrentDictionary<string, KeyValuePair<string, bool>> fieldNameList;

        /// <summary>
        /// 表列名的中文描述集合
        /// </summary>
        [Description("表列名的中文描述集合"), Category("自定属性"), Browsable(true)]
        public ConcurrentDictionary<string, KeyValuePair<string, bool>> FieldNameList
        {
            get
            {
                return fieldNameList;
            }
            set
            {
                fieldNameList = value;
            }

        }


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


        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            ListDataSoure.MovePrevious();
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            ListDataSoure.MoveNext();
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            ListDataSoure.MoveLast();
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            ListDataSoure.MoveFirst();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (OnSelectDataRow != null)
            {
                OnSelectDataRow(ListDataSoure.Current as T);
            }
            //if (Runway == BaseListRunWay.窗体)
            //{
            //    tsbtnSelected.Visible = true;
            //    Selected();
            //}
            //else
            //{
            //    tsbtnSelected.Visible = false;
            //    Modify();
            //}
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void UCAdvQuery_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
               | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel1, true, null);
            tableLayoutPanel1.Visible = false;
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.SuspendLayout();
            BindData(chkUseLike.Checked);
            tableLayoutPanel1.ResumeLayout();
            tableLayoutPanel1.Visible = true;
            InitQueryForDg();
        }

        private void kryptonbtnQuery_Click(object sender, EventArgs e)
        {
            Query();
        }



        private void chkUseLike_CheckedChanged(object sender, EventArgs e)
        {
            tableLayoutPanel1.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
| System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel1, true, null);
            tableLayoutPanel1.Visible = false;
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.SuspendLayout();
            BindData(chkUseLike.Checked);
            tableLayoutPanel1.ResumeLayout();
            tableLayoutPanel1.Visible = true;
        }
    }
}
