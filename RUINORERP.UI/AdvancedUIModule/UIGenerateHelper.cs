using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krypton.Toolkit;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.QueryDto;
using RUINORERP.UI.Common;

using System.ComponentModel;
using System.Data;
using System.Drawing;

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
using Label = System.Windows.Forms.Label;
using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Business.Processor;
using WorkflowCore.Primitives;
using RUINOR.WinFormsUI.ChkComboBox;
using Netron.GraphLib;
using System.Web.UI;
namespace RUINORERP.UI.AdvancedUIModule
{
    public class UIGenerateHelper
    {
        public static BaseEntity CreateQueryUI<Q>(bool useLike, Krypton.Toolkit.KryptonPanel UcPanel, QueryFilter queryFilter, decimal DefineColNum) where Q : class
        {
            return CreateQueryUI(typeof(Q), useLike, UcPanel, queryFilter, DefineColNum);
        }

        /// <summary>
        /// 创建查询界面
        /// </summary>
        /// <param name="useLike"></param>
        /// <param name="UcPanel"></param>
        /// <param name="queryFilter"></param>
        /// <param name="DefineColNum">4</param>
        /// <returns></returns>
        public static BaseEntity CreateQueryUI(Type type, bool useLike, Krypton.Toolkit.KryptonPanel UcPanel, QueryFilter queryFilter, decimal DefineColNum)
        {
            List<QueryField> queryFields = queryFilter.QueryFields;
            if (queryFilter == null)
            {
                throw new ArgumentNullException(nameof(queryFilter));
            }

            Type newtype = null;
            object newDto = null;
            newtype = AttributesBuilder_New2024(useLike, type, queryFilter);
            newDto = Activator.CreateInstance(newtype);

            BaseEntity Query = newDto as BaseEntity;
            if (queryFilter == null) { return Query; }

            //这里可以优化掉。保留的意义在于更新更多的相关信息
            List<BaseDtoField> baseDtoFields = UIHelper.GetDtoFieldNameList(newtype);
            //为了显示条件的顺序再一次修改，这样循环是利用这个查询参数的数组默认顺序

            //这里更新一些信息，再对类型处理一下。很重要。
            foreach (BaseDtoField item in baseDtoFields)
            {
                QueryField queryField = queryFilter.QueryFields.FirstOrDefault(q => q.FieldName == item.SugarCol.ColumnName);
                if (queryField == null)
                {
                    continue;
                }
                queryField.SugarCol = item.SugarCol;
                queryField.ExtendedAttribute = item.ExtendedAttribute;
                queryField.FieldName = queryField.FieldName;
                queryField.Caption = item.Caption;
                queryField.Description = item.Description;
                queryField.ColDataType = item.ColDataType.GetBaseType();
                queryField.IsRelated = item.IsFKRelationAttribute;
                if (queryField.IsRelated)
                {
                    queryField.fKRelationAttribute = item.fKRelationAttribute;
                    queryField.FKTableName = item.FKTableName;
                    //如果上级指定了就不要覆盖
                    if (queryField.SubQueryTargetType == null)
                    {
                        queryField.SubQueryTargetType = Assembly.LoadFrom("RUINORERP.Model.dll").GetType("RUINORERP.Model." + queryField.FKTableName);
                    }

                }

                #region
                if (queryField.AdvQueryFieldType == AdvQueryProcessType.None)
                {
                    #region  默认处理
                    EnumDataType edt = (EnumDataType)Enum.Parse(typeof(EnumDataType), queryField.ColDataType.Name);
                    switch (edt)
                    {
                        case EnumDataType.Boolean:
                            queryField.AdvQueryFieldType = AdvQueryProcessType.useYesOrNoToAll;
                            break;
                        case EnumDataType.DateTime:
                            queryField.AdvQueryFieldType = AdvQueryProcessType.datetimeRange;
                            break;
                        case EnumDataType.Int16:
                        case EnumDataType.UInt16:
                        case EnumDataType.Int32:
                        case EnumDataType.UInt32:
                        case EnumDataType.Int64:
                            queryField.AdvQueryFieldType = AdvQueryProcessType.defaultSelect;
                            break;
                        case EnumDataType.UInt64:

                            break;
                        case EnumDataType.IntPtr:
                        case EnumDataType.Char:
                        case EnumDataType.Single:
                        case EnumDataType.Double:
                        case EnumDataType.Decimal:
                        case EnumDataType.SByte:
                        case EnumDataType.Byte:
                        case EnumDataType.UIntPtr:
                        case EnumDataType.Object:
                            break;
                        case EnumDataType.String:
                            queryField.AdvQueryFieldType = AdvQueryProcessType.stringLike;
                            break;
                        default:
                            break;
                    }

                    #endregion
                }
                #endregion
                // queryField.QueryTargetType = fkrattr.FKTableName;
            }


            #region 定义表格布局的行和列
            //把时间排后面
            queryFields = queryFields.OrderBy(d => d.ExtendedAttribute.Count).ToList();


            int row = 0;
            //定义每列放几组控件
            int col = DefineColNum.ToInt();
            //计算有多少列 cols没有使用
            int cols = queryFields.Count % col;


            #region 计算有多少行,再计算多少列，每列中的元素下标
            int TotalRows = 0;
            int rows = 0;
            rows = queryFields.Count / col;

            //% 运算符在 C# 中表示取模运算，即返回除法的余数。
            //并且余数是计算后面宽的一个数据来源，比方 10个控件，4个一行，余数是2。第三行就是2个，第一列是3个，第二列也是3个，第三，第四就是2个
            int remainder = queryFields.Count % col;
            if (remainder % col > 0)
            {
                TotalRows = rows + 1;
            }
            else
            {
                TotalRows = rows;
            }

            //==

            #endregion



            int _x = 0, _y = 0, _Tabindex = 210;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int colFlag = 0;
            for (int c = 0; c < queryFields.Count; c++)
            {
                //  var coldata = newFieldlist[c] as BaseDtoField;
                QueryField queryField = queryFields[c];

                _x = 20 + c % col * 250;//计算控件在X轴位置
                int maxtextLen = GetMaxTextLen(queryFields, col, colFlag);
                if (queryField.Caption.Length < maxtextLen)
                {
                    _x = _x + (maxtextLen - queryField.Caption.Length) * 18;
                }
                if (c % col == 0)
                {
                    _y = 20 + 32 * c / col;//计算控件在Y轴的位置
                    colFlag++;
                }
                _Tabindex = c % col + c / col + _Tabindex;

                KryptonLabel lbl = new KryptonLabel();
                lbl.Text = queryField.Caption;
                //lbl.Dock = DockStyle.Right;
                lbl.Size = new Size(100, 30);
                lbl.Location = new System.Drawing.Point(_x, _y);

                //一行控件平行
                //一个字算18px
                _x = _x + 18 * queryField.Caption.Length;

                DataBindingHelper dbh = new DataBindingHelper();
                KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                switch (queryField.AdvQueryFieldType)
                {
                    case AdvQueryProcessType.None:
                        break;

                    case AdvQueryProcessType.TextSelect:
                        #region 绑定文本框
                        KryptonTextBox tb_box_cmb = new KryptonTextBox();
                        tb_box_cmb.Name = queryField.FieldName;
                        tb_box_cmb.Width = 150;

                        DataBindingHelper.BindData4TextBox(newDto, queryField.FriendlyFieldName, tb_box_cmb, BindDataType4TextBox.Text, false);
                        //DataBindingHelper.BindData4TextBoxWithTagQuery(newDto, newFieldlist[c].ColName, tb_box_cmb, false);

                        #region 生成快捷查询

                        string IDColName = queryField.fKRelationAttribute.FK_IDColName;
                        string ColName = queryField.FriendlyFieldName;
                        //DataBindingHelper.BindData4Cmb<T>(QueryDto, key, value, coldata.FKTableName, cmb);
                        //这里加载时 是指定了相关的外键表的对应实体的类型
                        //if (ReladtedEntityType == null)
                        //{
                        //    ReladtedEntityType = typeof(T);
                        //}

                        //如果有最终指定原始的字段就用原始的来绑定
                        if (queryField.FriendlyFieldNameFromRelated.IsNotEmptyOrNull())
                        {
                            ColName = queryField.FriendlyFieldNameFromRelated;
                        }
                        if (queryField.SubFilter.QueryTargetType == null)
                        {
                            MessageBox.Show("子查询对象类型没有指定。请截图后联系管理员。");
                        }
                        //注意这样调用不能用同名重载的方法名
                        MethodInfo mf2 = dbh.GetType().GetMethod("InitFilterForControlRef").MakeGenericMethod(new Type[] { queryField.SubFilter.QueryTargetType });
                        object[] args2 = new object[6] { newDto, tb_box_cmb, ColName, queryField.SubFilter, queryField.SubFilter.QueryTargetType, ColName };
                        mf2.Invoke(dbh, args2);

                        // DataBindingHelper.InitFilterForControlByExp<View_ProdDetail>(entity, txtProdDetailID, c => c.SKU, queryFilterC, typeof(tb_Prod));
                        #endregion


                        //lbl.Text = "";

                        tb_box_cmb.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(tb_box_cmb);
                        UcPanel.Controls.Add(lbl);
                        #endregion
                        break;

                    case AdvQueryProcessType.CmbMultiChoiceCanIgnore:
                        #region 可多选的下拉 可以忽略  这里是子查询的条件设置了。
                        UCCmbMultiChoiceCanIgnore choiceCanIgnore = new UCCmbMultiChoiceCanIgnore();
                        choiceCanIgnore.Name = queryField.FieldName;
                        choiceCanIgnore.Text = "";
                        choiceCanIgnore.Width = 200;


                        //只处理需要缓存的表
                        pair = new KeyValuePair<string, string>();
                        if (queryField.HasSubFilter && CacheHelper.Manager.NewTableList.TryGetValue(queryField.SubQueryTargetType.Name, out pair))
                        {
                            #region 绑定下拉带子查询
                            Type mytype = queryField.SubQueryTargetType;
                            //UI传入过滤条件 下拉可以显示不同的数据
                            ExpConverter expConverter = new ExpConverter();
                            object whereExp = null;
                            if (queryField.SubFilter.GetFilterLimitExpression(mytype) != null)
                            {
                                whereExp = expConverter.ConvertToFuncByClassName(queryField.SubFilter.QueryTargetType, queryField.SubFilter.GetFilterLimitExpression(mytype));
                            }

                            //绑定可忽略的那个chkbox
                            DataBindingHelper.BindData4CheckBox(newDto, pair.Key + "_CmbMultiChoiceCanIgnore", choiceCanIgnore.chkCanIgnore, true);
                            //绑定下拉
                            MethodInfo mf1 = dbh.GetType().GetMethod("BindData4CmbChkRefWithLimited").MakeGenericMethod(new Type[] { mytype });
                            object[] args1 = new object[6] { newDto, pair.Key, pair.Value, queryField.SubQueryTargetType.Name, choiceCanIgnore.chkMulti, whereExp };
                            mf1.Invoke(dbh, args1);
                            #endregion
                        }
                        else
                        {
                            #region 绑定下拉带子查询

                            //绑定可忽略的那个chkbox
                            DataBindingHelper.BindData4CheckBox(newDto, queryField.FieldName + "_CmbMultiChoiceCanIgnore", choiceCanIgnore.chkCanIgnore, true);
                            //绑定下拉
                            Type mytype = queryField.SubQueryTargetType;
                            MethodInfo mf1 = dbh.GetType().GetMethod("BindData4CmbChkRefWithLimited").MakeGenericMethod(new Type[] { mytype });
                            object[] args1 = new object[6] { newDto, pair.Key, pair.Value, queryField.SubFilter.QueryTargetType.Name, choiceCanIgnore.chkMulti, null };
                            mf1.Invoke(dbh, args1);
                            #endregion
                        }


                        choiceCanIgnore.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(choiceCanIgnore);
                        UcPanel.Controls.Add(lbl);
                        #endregion
                        break;
                    case AdvQueryProcessType.CmbMultiChoice:
                        #region 可多选的下拉
                        if (string.IsNullOrEmpty(queryField.FKTableName))
                        {
                            queryField.FKTableName = queryField.SubFilter.QueryTargetType.Name;
                        }
                        CheckBoxComboBox cmb = new CheckBoxComboBox();
                        //ButtonSpecAny buttonSpec = new ButtonSpecAny();
                        //buttonSpec.UniqueName = "btnclear";
                        //cmb.ButtonSpecs.Add(buttonSpec);
                        cmb.Name = queryField.FieldName;
                        cmb.Text = "";
                        cmb.Width = 150;
                        pair = new KeyValuePair<string, string>();
                        //只处理需要缓存的表

                        if (CacheHelper.Manager.NewTableList.TryGetValue(queryField.FKTableName, out pair))
                        {
                            string PIDColName = pair.Key;
                            string PColName = pair.Value;
                            //DataBindingHelper.BindData4Cmb<T>(QueryDto, key, value, coldata.FKTableName, cmb);
                            //这里加载时 是指定了相关的外键表的对应实体的类型

                            //关联要绑定的类型
                            Type mytype = queryField.SubQueryTargetType;

                            //非常值和学习借鉴有代码 TODO 重点学习代码
                            //UI传入过滤条件 下拉可以显示不同的数据
                            ExpConverter expConverter = new ExpConverter();
                            object whereExp = null;
                            if (queryField.SubFilter.GetFilterLimitExpression(mytype) != null)
                            {
                                whereExp = expConverter.ConvertToFuncByClassName(queryField.SubFilter.QueryTargetType, queryField.SubFilter.GetFilterLimitExpression(mytype));
                            }
                            #region 

                            //绑定下拉
                            MethodInfo mf1 = dbh.GetType().GetMethod("BindData4CmbChkRefWithLimited").MakeGenericMethod(new Type[] { mytype });
                            object[] args1 = new object[6] { newDto, PIDColName, PColName, queryField.FKTableName, cmb, whereExp };
                            mf1.Invoke(dbh, args1);
                            #endregion




                        }
                        cmb.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(cmb);
                        UcPanel.Controls.Add(lbl);
                        #endregion
                        break;
                    case AdvQueryProcessType.defaultSelect:
                        #region     单选下拉


                        KryptonComboBox DefaultCmb = new KryptonComboBox();
                        DefaultCmb.Name = queryField.FieldName;
                        DefaultCmb.Text = "";
                        DefaultCmb.Width = 150;
                        //只处理需要缓存的表
                        pair = new KeyValuePair<string, string>();
                        if (queryField.FKTableName.IsNotEmptyOrNull())
                        {
                            if (CacheHelper.Manager.NewTableList.TryGetValue(queryField.SubQueryTargetType.Name, out pair))

                            {

                                //关联要绑定的类型
                                Type mytype = queryField.SubQueryTargetType;

                                if (queryField.SubFilter.FilterLimitExpressions.Count == 0)
                                {
                                    #region 没有限制条件时

                                    if (mytype != null)
                                    {
                                        MethodInfo mf = dbh.GetType().GetMethod("BindData4CmbRef").MakeGenericMethod(new Type[] { mytype });
                                        object[] args = new object[5] { newDto, pair.Key, pair.Value, queryField.FKTableName, DefaultCmb };
                                        mf.Invoke(dbh, args);
                                    }
                                    else
                                    {
                                        MainForm.Instance.logger.LogError("动态加载外键数据时出错，" + queryField.FieldName + "在代理类中的属性名不对，自动生成规则可能有变化" + queryField.FKTableName.ToLower());
                                        MainForm.Instance.uclog.AddLog("动态加载外键数据时出错加载数据出错。请联系管理员。");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    //非常值和学习借鉴有代码 TODO 重点学习代码
                                    //UI传入过滤条件 下拉可以显示不同的数据
                                    ExpConverter expConverter = new ExpConverter();

                                    //var whereExp = expConverter.ConvertToFuncByClassName(queryField.SubFilter.QueryEntityType, queryField.SubFilter.FilterLimitExpressions[0]);
                                    //var whereExp = expConverter.ConvertToFuncByClassName(queryField.SubFilter.QueryEntityType, queryField.SubFilter.GetFilterLimitExpression());
                                    var whereExp = expConverter.ConvertToFuncByClassName(queryField.SubFilter.QueryTargetType, queryField.SubFilter.GetFilterLimitExpression(mytype));
                                    #region 
                                    //绑定下拉
                                    MethodInfo mf1 = dbh.GetType().GetMethod("BindData4CmbRefWithLimited").MakeGenericMethod(new Type[] { mytype });
                                    object[] args1 = new object[6] { newDto, pair.Key, pair.Value, queryField.FKTableName, DefaultCmb, whereExp };
                                    mf1.Invoke(dbh, args1);
                                    #endregion
                                }

                                //注意这样调用不能用同名重载的方法名
                                MethodInfo mf22 = dbh.GetType().GetMethod("InitFilterForControlRef").MakeGenericMethod(new Type[] { mytype });
                                object[] args22 = new object[6] { newDto, DefaultCmb, pair.Value, queryField.SubFilter, null, null };
                                mf22.Invoke(dbh, args22);


                            }
                        }

                        DefaultCmb.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(DefaultCmb);
                        UcPanel.Controls.Add(lbl);
                        #endregion
                        break;
                    case AdvQueryProcessType.EnumSelect:
                        KryptonComboBox eNumCmb = new KryptonComboBox();
                        eNumCmb.Name = queryField.FieldName;
                        eNumCmb.Text = "";
                        eNumCmb.Width = 150;

                        #region 枚举类型处理




                        //非常值和学习借鉴有代码 TODO 重点学习代码
                        //UI传入过滤条件 下拉可以显示不同的数据
                        if (queryField.FieldType == QueryFieldType.CmbEnum)
                        {
                            EnumBindingHelper bindingHelper = new EnumBindingHelper();

                            if (queryField.QueryFieldDataPara != null)
                            {
                                if (queryField.QueryFieldDataPara is QueryFieldEnumData enumData)
                                {

                                    //绑定下拉值

                                    //绑定实体值 这里没有指定 过滤
                                    //枚举的值 默认为-1，则指定为请选择

                                    newDto.SetPropertyValue(eNumCmb.Name, -1);
                                    DataBindingHelper.BindData4CmbByEnumRef(newDto, enumData.EnumValueColName, enumData.EnumType, eNumCmb, enumData.AddSelectItem);



                                }
                            }

                        }
                        eNumCmb.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(eNumCmb);
                        UcPanel.Controls.Add(lbl);

                        //cmb.SelectedIndex = -1;

                        #endregion



                        break;
                    case AdvQueryProcessType.datetimeRange:
                        UCAdvDateTimerPickerGroup dtpgroup = new UCAdvDateTimerPickerGroup();
                        dtpgroup.Name = queryField.FieldName;
                        dtpgroup.dtp1.Name = queryField.ExtendedAttribute[0].ColName;
                        object datetimeValue1 = ReflectionHelper.GetPropertyValue(newDto, queryField.ExtendedAttribute[0].ColName);
                        DataBindingHelper.BindData4DataTime(newDto, datetimeValue1, queryField.ExtendedAttribute[0].ColName, dtpgroup.dtp1, true);
                        dtpgroup.dtp1.Checked = true;

                        dtpgroup.dtp2.Name = queryField.ExtendedAttribute[1].ColName;
                        object datetimeValue2 = ReflectionHelper.GetPropertyValue(newDto, queryField.ExtendedAttribute[1].ColName);
                        DataBindingHelper.BindData4DataTime(newDto, datetimeValue2, queryField.ExtendedAttribute[1].ColName, dtpgroup.dtp2, true);
                        dtpgroup.dtp2.Checked = true;
                        //时间控件更长为260px，这里要特殊处理
                        dtpgroup.Location = new System.Drawing.Point(_x, _y);
                        _x = _x + 260;
                        UcPanel.Controls.Add(dtpgroup);
                        UcPanel.Controls.Add(lbl);
                        break;
                    case AdvQueryProcessType.datetime:
                        KryptonDateTimePicker dtp = new KryptonDateTimePicker();
                        dtp.Name = queryField.FieldName;
                        dtp.ShowCheckBox = true;
                        dtp.Width = 130;
                        dtp.Format = DateTimePickerFormat.Custom;
                        dtp.CustomFormat = "yyyy-MM-dd";
                        object datetimeValue = ReflectionHelper.GetPropertyValue(newDto, queryField.FieldName);
                        //如果时间为00001-1-1-1这种。就改为当前

                        if (datetimeValue.ToDateTime().Year == 1)
                        {
                            datetimeValue = System.DateTime.Now;
                            ReflectionHelper.SetPropertyValue(newDto, queryField.FieldName, datetimeValue);
                        }

                        DataBindingHelper.BindData4DataTime(newDto, datetimeValue, queryField.FieldName, dtp, true);
                        dtp.Checked = true;
                        dtp.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(dtp);
                        UcPanel.Controls.Add(lbl);
                        break;
                    case AdvQueryProcessType.stringLike:
                        KryptonTextBox tb_box = new KryptonTextBox();
                        tb_box.Name = queryField.FieldName;
                        tb_box.Width = 150;

                        DataBindingHelper.BindData4TextBox(newDto, queryField.FieldName, tb_box, BindDataType4TextBox.Text, false);
                        tb_box.Location = new System.Drawing.Point(_x, _y);

                        //动态生成一个右键菜单

                        MenuItem menuItem = new MenuItem();

                        menuItem.Text = "精确查询";
                        menuItem.Click += (sender, e) =>
                        {
                            // queryField.UseLike = true;
                            //复制到剪贴板
                            //Clipboard.SetText(tb_box.Text);
                        };
                        ContextMenu menu = new ContextMenu();
                        menu.MenuItems.Add(menuItem);
                        tb_box.ContextMenu = menu;
                        UcPanel.Controls.Add(tb_box);
                        UcPanel.Controls.Add(lbl);
                        break;
                    case AdvQueryProcessType.useYesOrNoToAll:
                        UCAdvYesOrNO chkgroup = new UCAdvYesOrNO();
                        // object datetimeValue1 = ReflectionHelper.GetPropertyValue(newDto, queryField.ExtendedAttribute[0].ColName);
                        chkgroup.rdb1.Text = "是";
                        chkgroup.rdb2.Text = "否";
                        DataBindingHelper.BindData4CheckBox(newDto, queryField.ExtendedAttribute[0].ColName, chkgroup.chk, false);
                        DataBindingHelper.BindData4RadioGroupTrueFalse(newDto, queryField.ExtendedAttribute[0].RelatedFields, chkgroup.rdb1, chkgroup.rdb2);
                        chkgroup.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(chkgroup);
                        UcPanel.Controls.Add(lbl);
                        break;

                    case AdvQueryProcessType.YesOrNo:
                        KryptonCheckBox chk = new KryptonCheckBox();
                        chk.Name = queryField.FieldName;
                        chk.Text = "";

                        //newDto
                        DataBindingHelper.BindData4CheckBox(newDto, queryField.FieldName, chk, false);
                        chk.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(chk);
                        UcPanel.Controls.Add(lbl);

                        break;
                    default:



                        break;
                }


                row++;

            }

            UcPanel.Parent.Height = _y + 30;
            UcPanel.Height = _y + 30;

            sw.Stop();
            TimeSpan dt = sw.Elapsed;
            MainForm.Instance.uclog.AddLog("性能", "加载控件耗时：" + dt.TotalMilliseconds.ToString());
            /*
               // 设置控件位置和大小 
               for (int i = 0; i < rowCount; i++)
               {
                   for (int j = 0; j < columnCount; j++)
                   {
                       Control control = TableLayoutPanel.GetControlFromPosition(j, i);
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
               }*/

            #endregion

            return Query;
        }

        /// <summary>
        /// 动态构建一些特性，针对不同的数据类型，比方日期等变动一个新的实体类型
        /// 注意这里构建代理类时是在原以字段后面加上Proxy,字段是_加下划线,这个在解析查询条件时会用到
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type AttributesBuilder_New2024(bool useLike, Type type, QueryFilter queryFilter)
        {
            //TypeBuilder
            var aName = new System.Reflection.AssemblyName(Assembly.GetExecutingAssembly().GetName().Name);
            var ab = AppDomain.CurrentDomain.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
            var mb = ab.DefineDynamicModule(aName.Name);
            var tb = mb.DefineType(type.Name + "Proxy", System.Reflection.TypeAttributes.Public, type);
            #region 前期处理  根据指定的类型  生成对应的相关属性

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
                    coldata.ColDataType = coldata.ColDataType.GetBaseType();
                    if (coldata.ColDataType.Name == "Byte[]")
                    {
                        continue;
                    }

                    QueryField queryField = queryFilter.QueryFields.Find(x => x.FieldName == coldata.FieldName);

                    EnumDataType edt = (EnumDataType)Enum.Parse(typeof(EnumDataType), coldata.ColDataType.Name);
                    //应该是没有具体指定就用数据类型来进行统一处理
                    switch (edt)
                    {
                        case EnumDataType.Boolean:
                            if (!coldata.FieldName.Contains("isdeleted"))
                            {
                                string newBoolProName1 = coldata.FieldName + "_Enable";
                                var attrBoolBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "是", newBoolProName1, AdvQueryProcessType.useYesOrNoToAll });
                                //动态属性要提前创建生成，后面要实体化传入控件
                                PropertyBuilder newBoolProp1 = AddProperty(tb, newBoolProName1);
                                newBoolProp1.SetCustomAttribute(attrBoolBuilder1);
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
                            if (queryField == null)
                            {
                                continue;
                            }
                            if (queryField.AdvQueryFieldType == AdvQueryProcessType.CmbMultiChoiceCanIgnore)
                            {
                                //先成一个标记可忽略的属性
                                string newCmbMultiChoiceCanIgnore = coldata.FieldName + "_CmbMultiChoiceCanIgnore";
                                var attrBuilderCmbMultiChoiceCanIgnore = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "多选可忽略", newCmbMultiChoiceCanIgnore, AdvQueryProcessType.CmbMultiChoiceCanIgnore });
                                //动态属性要提前创建生成，后面要实体化传入控件
                                PropertyBuilder newPropCmbMultiChoiceCanIgnore = AddProperty(tb, newCmbMultiChoiceCanIgnore, typeof(bool));
                                newPropCmbMultiChoiceCanIgnore.SetCustomAttribute(attrBuilderCmbMultiChoiceCanIgnore);

                                #region 动态属性要提前创建生成，后面要实体化传入控件

                                string newProNameMultiChoiceResults = coldata.FieldName + "_MultiChoiceResults";
                                var attrBuilderMultiChoiceResults = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "多选结果", newProNameMultiChoiceResults, AdvQueryProcessType.CmbMultiChoice });
                                //动态属性要提前创建生成，后面要实体化传入控件
                                PropertyBuilder newPropMultiChoiceResults = AddProperty(tb, newProNameMultiChoiceResults, typeof(List<object>));
                                newPropMultiChoiceResults.SetCustomAttribute(attrBuilderMultiChoiceResults);
                                #endregion

                                //string newSelectProName1 = coldata.FieldName + "_请选择";
                                //var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "请选择", newSelectProName1, AdvQueryProcessType.CmbMultiChoice });
                                ////动态属性要提前创建生成，后面要实体化传入控件
                                //PropertyBuilder newlikeProp1 = AddProperty(tb, newSelectProName1);
                                //newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                                break;
                            }

                            if (queryField.AdvQueryFieldType == AdvQueryProcessType.CmbMultiChoice)
                            {

                                #region 动态属性要提前创建生成，后面要实体化传入控件

                                string newProNameMultiChoiceResults = coldata.FieldName + "_MultiChoiceResults";
                                var attrBuilderMultiChoiceResults = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "多选结果", newProNameMultiChoiceResults, AdvQueryProcessType.CmbMultiChoice });
                                //动态属性要提前创建生成，后面要实体化传入控件
                                PropertyBuilder newPropMultiChoiceResults = AddProperty(tb, newProNameMultiChoiceResults, typeof(List<object>));
                                newPropMultiChoiceResults.SetCustomAttribute(attrBuilderMultiChoiceResults);
                                #endregion

                                //string newSelectProName1 = coldata.FieldName + "_请选择";
                                //var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "请选择", newSelectProName1, AdvQueryProcessType.CmbMultiChoice });
                                ////动态属性要提前创建生成，后面要实体化传入控件
                                //PropertyBuilder newlikeProp1 = AddProperty(tb, newSelectProName1);
                                //newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                                break;
                            }
                            if (queryField.AdvQueryFieldType == AdvQueryProcessType.defaultSelect)
                            {

                                string newSelectProName1 = coldata.FieldName + "_请选择";
                                var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "请选择", newSelectProName1, AdvQueryProcessType.defaultSelect });
                                //动态属性要提前创建生成，后面要实体化传入控件
                                PropertyBuilder newlikeProp1 = AddProperty(tb, newSelectProName1);
                                newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                                break;
                            }

                            break;
                        case EnumDataType.UInt64:
                        case EnumDataType.IntPtr:
                        case EnumDataType.UIntPtr:
                            //下拉
                            if (coldata.IsFKRelationAttribute)
                            {
                                if (coldata.fKRelationAttribute.CmbMultiChoice)
                                {
                                    #region 动态属性要提前创建生成，后面要实体化传入控件
                                    string newProNameMultiChoiceResults = coldata.FieldName + "_MultiChoiceResults";
                                    var attrBuilderMultiChoiceResults = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "多选结果", newProNameMultiChoiceResults, AdvQueryProcessType.CmbMultiChoice });
                                    //动态属性要提前创建生成，后面要实体化传入控件
                                    //PropertyBuilder newPropMultiChoiceResults = AddProperty(tb, newProNameMultiChoiceResults);

                                    //这个属性 在控件里定义了一个对应的MultiChoiceResults 属性是类型是List<object>
                                    PropertyBuilder newPropMultiChoiceResults = AddProperty(tb, newProNameMultiChoiceResults, typeof(List<object>));
                                    newPropMultiChoiceResults.SetCustomAttribute(attrBuilderMultiChoiceResults);
                                    #endregion

                                    string newSelectProName1 = coldata.FieldName + "_请选择";
                                    var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "请选择", newSelectProName1, AdvQueryProcessType.defaultSelect });
                                    //动态属性要提前创建生成，后面要实体化传入控件
                                    PropertyBuilder newlikeProp1 = AddProperty(tb, newSelectProName1);
                                    newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                                }
                                else
                                {
                                    string newSelectProName1 = coldata.FieldName + "_请选择";
                                    var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "请选择", newSelectProName1, AdvQueryProcessType.defaultSelect });
                                    //动态属性要提前创建生成，后面要实体化传入控件
                                    PropertyBuilder newlikeProp1 = AddProperty(tb, newSelectProName1);
                                    newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                                }

                            }
                            break;
                        case EnumDataType.Object:
                            break;
                        case EnumDataType.String:
                            //if (coldata.UseLike)
                            //{
                            string newlikeProNameString = coldata.FieldName + "_Like";
                            var attrlikeBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "like", newlikeProNameString, AdvQueryProcessType.stringLike });
                            //动态属性要提前创建生成，后面要实体化传入控件
                            PropertyBuilder newlikePropstring = AddProperty(tb, newlikeProNameString);
                            newlikePropstring.SetCustomAttribute(attrlikeBuilder1);
                            break;
                        case EnumDataType.DateTime:

                            string newProName1 = coldata.FieldName + "_Start";
                            string newProName2 = coldata.FieldName + "_End";
                            var attrBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "时间起", newProName1, AdvQueryProcessType.datetimeRange });
                            var attrBuilder2 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "时间止", newProName2, AdvQueryProcessType.datetimeRange });
                            //动态属性要提前创建生成，后面要实体化传入控件
                            PropertyBuilder newProp1 = AddProperty(tb, newProName1, typeof(DateTime?));//起始时间是可以选空的，实际如果不可空的话，要调试到这里看什么情况
                            //动态属性要提前创建生成，后面要实体化传入控件
                            PropertyBuilder newProp2 = AddProperty(tb, newProName2, typeof(DateTime?));
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
                    QueryField queryField = queryFilter.QueryFields.FirstOrDefault(f => f.FieldName == coldata.FieldName);
                    coldata.ColDataType = coldata.ColDataType.GetBaseType();
                    if (coldata.ColDataType.Name == "Byte[]")
                    {
                        continue;
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

                            if (queryField.AdvQueryFieldType == AdvQueryProcessType.CmbMultiChoiceCanIgnore)
                            {
                                //先成一个标记可忽略的属性
                                string newCmbMultiChoiceCanIgnore = coldata.FieldName + "_CmbMultiChoiceCanIgnore";
                                var attrBuilderCmbMultiChoiceCanIgnore = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "多选可忽略", newCmbMultiChoiceCanIgnore, AdvQueryProcessType.CmbMultiChoiceCanIgnore });
                                //动态属性要提前创建生成，后面要实体化传入控件
                                PropertyBuilder newPropCmbMultiChoiceCanIgnore = AddProperty(tb, newCmbMultiChoiceCanIgnore, typeof(bool));
                                newPropCmbMultiChoiceCanIgnore.SetCustomAttribute(attrBuilderCmbMultiChoiceCanIgnore);

                                #region 动态属性要提前创建生成，后面要实体化传入控件

                                string newProNameMultiChoiceResults = coldata.FieldName + "_MultiChoiceResults";
                                var attrBuilderMultiChoiceResults = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "多选结果", newProNameMultiChoiceResults, AdvQueryProcessType.CmbMultiChoice });
                                //动态属性要提前创建生成，后面要实体化传入控件
                                PropertyBuilder newPropMultiChoiceResults = AddProperty(tb, newProNameMultiChoiceResults, typeof(List<object>));
                                newPropMultiChoiceResults.SetCustomAttribute(attrBuilderMultiChoiceResults);
                                #endregion

                                string newSelectProName1 = coldata.FieldName + "_请选择";
                                var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "请选择", newSelectProName1, AdvQueryProcessType.CmbMultiChoice });
                                //动态属性要提前创建生成，后面要实体化传入控件
                                PropertyBuilder newlikeProp1 = AddProperty(tb, newSelectProName1);
                                newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                                break;
                            }


                            if (coldata.IsFKRelationAttribute)
                            {
                                //MultiChoiceResults 多选时要生成这个特殊的属性。后面选中的结果都 放到这个属性里同步
                                if (coldata.fKRelationAttribute.CmbMultiChoice)
                                {
                                    #region 动态属性要提前创建生成，后面要实体化传入控件
                                    string newProNameMultiChoiceResults = coldata.FieldName + "_MultiChoiceResults";
                                    var attrBuilderMultiChoiceResults = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "多选结果", newProNameMultiChoiceResults, AdvQueryProcessType.CmbMultiChoice });
                                    //动态属性要提前创建生成，后面要实体化传入控件
                                    PropertyBuilder newPropMultiChoiceResults = AddProperty(tb, newProNameMultiChoiceResults, typeof(List<object>));
                                    newPropMultiChoiceResults.SetCustomAttribute(attrBuilderMultiChoiceResults);
                                    #endregion

                                    string newSelectProName1 = coldata.FieldName + "_请选择";
                                    var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "请选择", newSelectProName1, AdvQueryProcessType.CmbMultiChoice });
                                    //动态属性要提前创建生成，后面要实体化传入控件
                                    PropertyBuilder newlikeProp1 = AddProperty(tb, newSelectProName1);
                                    newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                                }
                                else
                                {
                                    string newSelectProName1 = coldata.FieldName + "_请选择";
                                    var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "请选择", newSelectProName1, AdvQueryProcessType.defaultSelect });
                                    //动态属性要提前创建生成，后面要实体化传入控件
                                    PropertyBuilder newlikeProp1 = AddProperty(tb, newSelectProName1);
                                    newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                                }

                            }
                            break;
                        case EnumDataType.Object:
                            break;
                        case EnumDataType.String:
                            if (coldata.UseLike)
                            {
                                string newlikeProName1 = coldata.FieldName + "_Like";
                                var attrlikeBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "like", newlikeProName1, AdvQueryProcessType.stringLike });
                                //动态属性要提前创建生成，后面要实体化传入控件
                                PropertyBuilder newlikeProp1 = AddProperty(tb, newlikeProName1);
                                newlikeProp1.SetCustomAttribute(attrlikeBuilder1);
                            }
                            break;
                        case EnumDataType.DateTime:


                            break;
                        default:
                            break;
                    }
                }
                #endregion
            }

            #endregion
            Type newtype = tb.CreateType();
            return newtype;
        }

        /// <summary>
        /// 取指定列的最大字段名称的长度
        /// </summary>
        /// <param name="Fieldlist"></param>
        /// <param name="colNum">每行放的控件列数(组）</param>
        /// <returns></returns>
        private static int GetMaxTextLen(List<QueryField> Fieldlist, int colNum, int targetCol)
        {
            int MaxTextLen = 0;
            //计算有多少行,再计算多少列，每列中的元素下标
            #region
            int TotalRows = 0;
            int rows = 0;
            rows = Fieldlist.Count / colNum;

            //% 运算符在 C# 中表示取模运算，即返回除法的余数。
            //并且余数是计算后面宽的一个数据来源，比方 10个控件，4个一行，余数是2。第三行就是2个，第一列是3个，第二列也是3个，第三，第四就是2个
            int remainder = Fieldlist.Count % colNum;
            if (remainder % colNum > 0)
            {
                TotalRows = rows + 1;
            }
            else
            {
                TotalRows = rows;
            }

            //==
            for (int i = 0; i < colNum; i++)
            {
                if (Fieldlist.Count > colNum)
                {

                }
            }
            #endregion
            List<QueryField> targetList = new List<QueryField>();
            for (int i = targetCol; i < Fieldlist.Count; i = i + colNum)
            {
                targetList.Add(Fieldlist[i]);
            }
            MaxTextLen = targetList.Max(t => t.Caption.Length);
            return MaxTextLen;
        }


        private static PropertyBuilder AddProperty(TypeBuilder tb, string MemberName)
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

        private static PropertyBuilder AddProperty(TypeBuilder tb, string MemberName, Type memberType)
        {
            #region  动态创建字段


            // string MemberName = "Watson_ok";

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


    }
}
