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
    /// <summary>
    /// 本来T是用查询querydto，但是他无法查到外键信息
    /// UIGenerateHelper代替了他
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Obsolete]
    public class UIQueryHelper<T> where T : class
    {




        /// <summary>
        /// 相关外键表或实体查询实体。如果是视图传入T时起作用
        /// 如：传入查询条件时。如果是相关的实体字段，则要有标记为外键特性。表才有。视图手动添加？如果在条件中加，不好。另一个功能使用也要加,
        /// 视图手动加，生成会覆盖
        /// </summary>
        public Type ReladtedEntityType
        {
            get; set;
        }

        //通过反射来执行类的静态方法
        DataBindingHelper dbh = new DataBindingHelper();

        /// <summary>
        /// 实际代理优先级的类型有:时间和枚举
        /// </summary>
        /// <param name="useLike"></param>
        /// <param name="UcPanel"></param>
        /// <param name="queryFilter"></param>
        /// <param name="DefineColNum"></param>
        /// <param name="QueryDtoProxy">如果是从工作台这种形式加载，则执行两次，一次是生成，第二次用生成的对象传入
        /// ，再根据对象的值判断UI的显示。特别是时间chked没变化所以用这个来解决</param>
        /// <returns></returns>
        public BaseEntity SetQueryUI(bool useLike, Krypton.Toolkit.KryptonPanel UcPanel, QueryFilter queryFilter, decimal DefineColNum)
        {
            var type = typeof(T);
            //视图或特殊情况指定的显示的条件等
            if (ReladtedEntityType != null)
            {
                type = ReladtedEntityType;
            }

            Type newtype = null;
            object newDto = null;
            //如果代理已经生成过，这个情况是为了将UI的值显示为参数中一样。特别是时间。不会去掉checked
            if (queryFilter.QueryFields.Count > 0)
            {
                newtype = AttributesBuilder_New2024(useLike, type, queryFilter);
            }
            else
            {
                newtype = AttributesBuilder(useLike, type, queryFilter);
            }

            newDto = Activator.CreateInstance(newtype);

            BaseEntity Query = newDto as BaseEntity;
            if (queryFilter == null) { return Query; }
            var new_DtoEntityFieldNameList = UIHelper.GetDtoFieldNameList(newtype);

            List<BaseDtoField> baseDtoFields = new List<BaseDtoField>();
            //为了显示条件的顺序再一次修改，这样循环是利用这个查询参数的数组默认顺序
            foreach (QueryField parameter in queryFilter.QueryFields)
            {
                BaseDtoField bdf = new_DtoEntityFieldNameList.FirstOrDefault(b => b.SugarCol.ColumnName == parameter.FieldName);
                if (bdf != null)
                {
                    if (bdf.ColDataType.Name == "Byte[]")
                    {
                        continue;
                    }

                    baseDtoFields.Add(bdf);
                }
            }
            // 定义表格布局的行和列
            #region

            List<BaseDtoField> newFieldlist = baseDtoFields.OrderBy(d => d.ExtendedAttribute.Count).ToList();
            #endregion

            int row = 0;
            //定义每列放几组控件
            int col = DefineColNum.ToInt();
            //计算有多少列 cols没有使用
            int cols = newFieldlist.Count % col;

            //计算有多少行,再计算多少列，每列中的元素下标
            #region
            int TotalRows = 0;
            int rows = 0;
            rows = newFieldlist.Count / col;

            //% 运算符在 C# 中表示取模运算，即返回除法的余数。
            //并且余数是计算后面宽的一个数据来源，比方 10个控件，4个一行，余数是2。第三行就是2个，第一列是3个，第二列也是3个，第三，第四就是2个
            int remainder = newFieldlist.Count % col;
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
            for (int c = 0; c < newFieldlist.Count; c++)
            {
                var coldata = newFieldlist[c] as BaseDtoField;
                QueryField queryField = null;
                queryField = queryFilter.QueryFields.FirstOrDefault(f => f.FieldName == newFieldlist[c].FieldName);

                _x = 20 + c % col * 250;//计算控件在X轴位置
                int maxtextLen = GetMaxTextLen(newFieldlist, col, colFlag);
                if (coldata.Caption.Length < maxtextLen)
                {
                    _x = _x + (maxtextLen - coldata.Caption.Length) * 18;
                }
                if (c % col == 0)
                {
                    _y = 20 + 32 * c / col;//计算控件在Y轴的位置
                    colFlag++;
                }
                _Tabindex = c % col + c / col + _Tabindex;

                KryptonLabel lbl = new KryptonLabel();
                lbl.Text = coldata.Caption;
                //lbl.Dock = DockStyle.Right;
                lbl.Size = new Size(100, 30);
                lbl.Location = new System.Drawing.Point(_x, _y);

                //一行控件平行
                //一个字算18px
                _x = _x + 18 * coldata.Caption.Length;

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
                        if (newFieldlist[c].UseLike)
                        {
                            UCAdvYesOrNO chkgroup = new UCAdvYesOrNO();

                            object datetimeValue1 = ReflectionHelper.GetPropertyValue(newDto, coldata.ExtendedAttribute[0].ColName);
                            chkgroup.rdb1.Text = "是";
                            chkgroup.rdb2.Text = "否";
                            DataBindingHelper.BindData4CheckBox(newDto, coldata.ExtendedAttribute[0].ColName, chkgroup.chk, false);
                            DataBindingHelper.BindData4RadioGroupTrueFalse(newDto, coldata.ExtendedAttribute[0].RelatedFields, chkgroup.rdb1, chkgroup.rdb2);
                            chkgroup.Location = new System.Drawing.Point(_x, _y);
                            UcPanel.Controls.Add(chkgroup);
                            UcPanel.Controls.Add(lbl);
                        }
                        else
                        {
                            KryptonCheckBox chk = new KryptonCheckBox();
                            chk.Name = newFieldlist[c].FieldName;
                            chk.Text = "";

                            //newDto
                            DataBindingHelper.BindData4CheckBox(newDto, newFieldlist[c].FieldName, chk, false);
                            chk.Location = new System.Drawing.Point(_x, _y);
                            UcPanel.Controls.Add(chk);
                            UcPanel.Controls.Add(lbl);
                        }

                        break;
                    case EnumDataType.DateTime:
                        if (newFieldlist[c].UseLike)
                        {
                            UCAdvDateTimerPickerGroup dtpgroup = new UCAdvDateTimerPickerGroup();
                            dtpgroup.Name = coldata.FieldName;
                            dtpgroup.dtp1.Name = coldata.ExtendedAttribute[0].ColName;
                            object datetimeValue1 = ReflectionHelper.GetPropertyValue(newDto, coldata.ExtendedAttribute[0].ColName);
                            DataBindingHelper.BindData4DataTime(newDto, datetimeValue1, coldata.ExtendedAttribute[0].ColName, dtpgroup.dtp1, true);
                            dtpgroup.dtp1.Checked = true;

                            dtpgroup.dtp2.Name = coldata.ExtendedAttribute[1].ColName;
                            object datetimeValue2 = ReflectionHelper.GetPropertyValue(newDto, coldata.ExtendedAttribute[1].ColName);
                            DataBindingHelper.BindData4DataTime(newDto, datetimeValue2, coldata.ExtendedAttribute[1].ColName, dtpgroup.dtp2, true);
                            dtpgroup.dtp2.Checked = true;


                            //时间控件更长为260px，这里要特殊处理

                            dtpgroup.Location = new System.Drawing.Point(_x, _y);
                            _x = _x + 260;
                            UcPanel.Controls.Add(dtpgroup);
                            UcPanel.Controls.Add(lbl);
                        }
                        else
                        {
                            KryptonDateTimePicker dtp = new KryptonDateTimePicker();
                            dtp.Name = newFieldlist[c].FieldName;
                            dtp.ShowCheckBox = true;
                            dtp.Width = 130;
                            object datetimeValue = ReflectionHelper.GetPropertyValue(newDto, newFieldlist[c].FieldName);
                            //如果时间为00001-1-1-1这种。就改为当前

                            if (datetimeValue.ToDateTime().Year == 1)
                            {
                                datetimeValue = System.DateTime.Now;
                                ReflectionHelper.SetPropertyValue(newDto, newFieldlist[c].FieldName, datetimeValue);
                            }

                            DataBindingHelper.BindData4DataTime(newDto, datetimeValue, newFieldlist[c].FieldName, dtp, true);
                            dtp.Checked = true;
                            dtp.Location = new System.Drawing.Point(_x, _y);
                            UcPanel.Controls.Add(dtp);
                            UcPanel.Controls.Add(lbl);
                        }

                        break;
                    case EnumDataType.Int16:
                    case EnumDataType.UInt16:
                    case EnumDataType.Int32:
                    case EnumDataType.UInt32:
                    case EnumDataType.Int64:
                    case EnumDataType.UInt64:

                        if (queryField.AdvQueryFieldType == AdvQueryProcessType.CmbMultiChoiceCanIgnore)
                        {
                            #region 可多选的下拉 可以忽略
                            if (string.IsNullOrEmpty(coldata.FKTableName))
                            {
                                coldata.FKTableName = queryField.SubFilter.QueryTargetType.Name;
                            }
                            UCCmbMultiChoiceCanIgnore choiceCanIgnore = new UCCmbMultiChoiceCanIgnore();
                            choiceCanIgnore.Name = newFieldlist[c].FieldName;
                            choiceCanIgnore.Text = "";
                            choiceCanIgnore.Width = 180;
                            //只处理需要缓存的表
                            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                            if (CacheHelper.Manager.NewTableList.TryGetValue(coldata.FKTableName, out pair))
                            {
                                string IDColName = pair.Key;
                                string ColName = pair.Value;
                                //DataBindingHelper.BindData4Cmb<T>(QueryDto, key, value, coldata.FKTableName, cmb);
                                //这里加载时 是指定了相关的外键表的对应实体的类型
                                if (ReladtedEntityType == null)
                                {
                                    ReladtedEntityType = typeof(T);
                                }

                                //关联要绑定的类型
                                Type mytype = null;

                                //通过反射来执行类的静态方法
                                DataBindingHelper dbh = new DataBindingHelper();
                                if (queryField.IsView)
                                {
                                    mytype = queryField.SubQueryTargetType;
                                }
                                else
                                {
                                    //注意找这个属性类型是小写了，生成规律
                                    PropertyInfo PI = ReflectionHelper.GetPropertyInfo(ReladtedEntityType, newDto, coldata.FKTableName.ToLower());
                                    if (PI == null)
                                    {
                                        mytype = queryField.SubFilter.QueryTargetType;
                                    }
                                    else
                                    {
                                        mytype = PI.PropertyType;
                                    }
                                }

                                //非常值和学习借鉴有代码 TODO 重点学习代码
                                //UI传入过滤条件 下拉可以显示不同的数据
                                ExpConverter expConverter = new ExpConverter();
                                object whereExp = null;
                                if (queryField.SubFilter.GetFilterLimitExpression(mytype) != null)
                                {
                                    whereExp = expConverter.ConvertToFuncByClassName(queryField.SubFilter.QueryTargetType, queryField.SubFilter.GetFilterLimitExpression(mytype));
                                }
                                #region 

                                //绑定可忽略的那个chkbox
                                DataBindingHelper.BindData4CheckBox(newDto, IDColName + "_CmbMultiChoiceCanIgnore", choiceCanIgnore.chkCanIgnore, true);
                                //绑定下拉
                                MethodInfo mf1 = dbh.GetType().GetMethod("BindData4CmbChkRefWithLimited").MakeGenericMethod(new Type[] { mytype });
                                object[] args1 = new object[6] { newDto, IDColName, ColName, coldata.FKTableName, choiceCanIgnore.chkMulti, whereExp };
                                mf1.Invoke(dbh, args1);
                                #endregion
                            }
                            choiceCanIgnore.Location = new System.Drawing.Point(_x, _y);
                            UcPanel.Controls.Add(choiceCanIgnore);
                            UcPanel.Controls.Add(lbl);
                            #endregion

                            break;
                        }

                        if (coldata.fKRelationAttribute != null && coldata.IsFKRelationAttribute && coldata.fKRelationAttribute.CmbMultiChoice)
                        {
                            #region 可多选的下拉
                            if (string.IsNullOrEmpty(coldata.FKTableName))
                            {
                                coldata.FKTableName = queryField.SubFilter.QueryTargetType.Name;
                            }
                            CheckBoxComboBox cmb = new CheckBoxComboBox();
                            //ButtonSpecAny buttonSpec = new ButtonSpecAny();
                            //buttonSpec.UniqueName = "btnclear";
                            //cmb.ButtonSpecs.Add(buttonSpec);
                            cmb.Name = newFieldlist[c].FieldName;
                            cmb.Text = "";
                            cmb.Width = 150;
                            //只处理需要缓存的表
                            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                            if (CacheHelper.Manager.NewTableList.TryGetValue(coldata.FKTableName, out pair))
                            {
                                string IDColName = pair.Key;
                                string ColName = pair.Value;
                                //DataBindingHelper.BindData4Cmb<T>(QueryDto, key, value, coldata.FKTableName, cmb);
                                //这里加载时 是指定了相关的外键表的对应实体的类型
                                if (ReladtedEntityType == null)
                                {
                                    ReladtedEntityType = typeof(T);
                                }

                                //关联要绑定的类型
                                Type mytype = null;

                                //IsView 和 ReladtedViewEntityType  暂时没有去掉。文本框查询的思路是，查询条件和字段都指定了查询对象的类型。by 2024-7-25
                                //通过反射来执行类的静态方法
                                DataBindingHelper dbh = new DataBindingHelper();
                                if (queryField.IsView)
                                {
                                    mytype = queryField.SubQueryTargetType;
                                }
                                else
                                {
                                    //注意找这个属性类型是小写了，生成规律
                                    PropertyInfo PI = ReflectionHelper.GetPropertyInfo(ReladtedEntityType, newDto, coldata.FKTableName.ToLower());
                                    if (PI == null)
                                    {
                                        mytype = queryField.SubFilter.QueryTargetType;
                                    }
                                    else
                                    {
                                        mytype = PI.PropertyType;
                                    }

                                }

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
                                object[] args1 = new object[6] { newDto, IDColName, ColName, coldata.FKTableName, cmb, whereExp };
                                mf1.Invoke(dbh, args1);
                                #endregion


                                //注意这样调用不能用同名重载的方法名,这个是加载再次查询功能？
                                //  MethodInfo mf2 = dbh.GetType().GetMethod("InitFilterForControlNew").MakeGenericMethod(new Type[] { mytype });
                                // object[] args2 = new object[4] { newDto, cmb, ColName, queryField.SubFilter };
                                // mf2.Invoke(dbh, args2);


                            }
                            cmb.Location = new System.Drawing.Point(_x, _y);
                            UcPanel.Controls.Add(cmb);
                            UcPanel.Controls.Add(lbl);
                            #endregion
                            break;
                        }


                        if ((coldata.IsFKRelationAttribute && queryField.FriendlyFieldName.IsNullOrEmpty()))
                        {
                            #region     单选

                            if (string.IsNullOrEmpty(coldata.FKTableName))
                            {
                                coldata.FKTableName = queryField.SubFilter.QueryTargetType.Name;
                            }
                            KryptonComboBox cmb = new KryptonComboBox();
                            cmb.Name = newFieldlist[c].FieldName;
                            cmb.Text = "";
                            cmb.Width = 150;
                            //只处理需要缓存的表
                            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                            if (CacheHelper.Manager.NewTableList.TryGetValue(coldata.FKTableName, out pair))
                            {
                                string IDColName = pair.Key;
                                string ColName = pair.Value;
                                //DataBindingHelper.BindData4Cmb<T>(QueryDto, key, value, coldata.FKTableName, cmb);
                                //这里加载时 是指定了相关的外键表的对应实体的类型
                                if (ReladtedEntityType == null)
                                {
                                    ReladtedEntityType = typeof(T);
                                }

                                //关联要绑定的类型
                                Type mytype = null;


                                if (queryField.IsView)
                                {
                                    mytype = queryField.SubQueryTargetType;
                                }
                                else
                                {
                                    //注意找这个属性类型是小写了，生成规律
                                    PropertyInfo PI = ReflectionHelper.GetPropertyInfo(ReladtedEntityType, newDto, coldata.FKTableName.ToLower());
                                    if (PI == null)
                                    {
                                        mytype = queryField.SubFilter.QueryTargetType;
                                    }
                                    else
                                    {
                                        mytype = PI.PropertyType;
                                    }

                                }

                                if (queryField.SubFilter.FilterLimitExpressions.Count == 0)
                                {
                                    #region 没有限制条件时

                                    if (mytype != null)
                                    {
                                        MethodInfo mf = dbh.GetType().GetMethod("BindData4CmbRef").MakeGenericMethod(new Type[] { mytype });
                                        object[] args = new object[5] { newDto, IDColName, ColName, coldata.FKTableName, cmb };
                                        mf.Invoke(dbh, args);
                                    }
                                    else
                                    {
                                        MainForm.Instance.logger.LogError("动态加载外键数据时出错，" + typeof(T).Name + "在代理类中的属性名不对，自动生成规则可能有变化" + coldata.FKTableName.ToLower());
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
                                    object[] args1 = new object[6] { newDto, IDColName, ColName, coldata.FKTableName, cmb, whereExp };
                                    mf1.Invoke(dbh, args1);
                                    #endregion
                                }

                                //注意这样调用不能用同名重载的方法名
                                MethodInfo mf2 = dbh.GetType().GetMethod("InitFilterForControlRef").MakeGenericMethod(new Type[] { mytype });
                                object[] args2 = new object[6] { newDto, cmb, ColName, queryField.SubFilter, null, null };
                                mf2.Invoke(dbh, args2);


                            }
                            cmb.Location = new System.Drawing.Point(_x, _y);
                            UcPanel.Controls.Add(cmb);
                            UcPanel.Controls.Add(lbl);
                            #endregion
                            break;
                        }
                        //|| (queryField.SubFilter != null && queryField.SubFilter.QueryFields.Count > 0)
                        if (coldata.IsFKRelationAttribute && queryField.FriendlyFieldName.IsNotEmptyOrNull())
                        {
                            #region 绑定文本框
                            KryptonTextBox tb_box_cmb = new KryptonTextBox();
                            tb_box_cmb.Name = newFieldlist[c].FieldName;
                            tb_box_cmb.Width = 150;

                            DataBindingHelper.BindData4TextBox(newDto, queryField.FriendlyFieldName, tb_box_cmb, BindDataType4TextBox.Text, false);
                            //DataBindingHelper.BindData4TextBoxWithTagQuery(newDto, newFieldlist[c].ColName, tb_box_cmb, false);


                            #region 生成快捷查询

                            string IDColName = coldata.fKRelationAttribute.FK_IDColName;
                            string ColName = queryField.FriendlyFieldName;
                            //DataBindingHelper.BindData4Cmb<T>(QueryDto, key, value, coldata.FKTableName, cmb);
                            //这里加载时 是指定了相关的外键表的对应实体的类型
                            if (ReladtedEntityType == null)
                            {
                                ReladtedEntityType = typeof(T);
                            }

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
                        }
                        else
                        {

                            KryptonComboBox cmb = new KryptonComboBox();
                            cmb.Name = newFieldlist[c].FieldName;
                            cmb.Text = "";
                            cmb.Width = 150;

                            #region 枚举类型处理
                            //DataBindingHelper.BindData4Cmb<T>(QueryDto, key, value, coldata.FKTableName, cmb);
                            //这里加载时 是指定了相关的外键表的对应实体的类型
                            if (ReladtedEntityType == null)
                            {
                                ReladtedEntityType = typeof(T);
                            }

                            //通过反射来执行类的静态方法
                            DataBindingHelper dbh = new DataBindingHelper();

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
                                        // bindingHelper.InitDataToCmbByEnumOnWhere(queryFieldData.BindDataSource, queryFieldData.EnumValueColName, cmb);
                                        //绑定实体值 这里没有指定 过滤
                                        //枚举的值 默认为-1，则指定为请选择

                                        newDto.SetPropertyValue(cmb.Name, -1);
                                        DataBindingHelper.BindData4CmbByEnum<T>(newDto, enumData.EnumValueColName, enumData.EnumType, cmb, enumData.AddSelectItem);
                                    }

                                    if (queryField.QueryFieldDataPara is QueryFieldEnumData)
                                    {
                                        QueryFieldEnumData queryFieldData = new QueryFieldEnumData();
                                        queryFieldData = queryField.QueryFieldDataPara as QueryFieldEnumData;
                                        #region 
                                        //绑定下拉值
                                        // bindingHelper.InitDataToCmbByEnumOnWhere(queryFieldData.BindDataSource, queryFieldData.EnumValueColName, cmb);
                                        //绑定实体值 这里没有指定 过滤
                                        //枚举的值 默认为-1，则指定为请选择
                                        newDto.SetPropertyValue(cmb.Name, -1);
                                        DataBindingHelper.BindData4CmbByEnum<T>(newDto, queryFieldData.EnumValueColName, queryFieldData.EnumType, cmb, queryFieldData.AddSelectItem);

                                        #endregion
                                    }


                                    //if (queryField.QueryFieldDataPara is QueryFieldEnumData<T>)
                                    //{
                                    //    QueryFieldEnumData<T> queryFieldData = new QueryFieldEnumData<T>();
                                    //    queryFieldData = queryField.QueryFieldDataPara as QueryFieldEnumData<T>;
                                    //    #region 
                                    //    //绑定下拉值
                                    //    // bindingHelper.InitDataToCmbByEnumOnWhere(queryFieldData.BindDataSource, queryFieldData.EnumValueColName, cmb);
                                    //    //绑定实体值 这里没有指定 过滤
                                    //    //枚举的值 默认为-1，则指定为请选择
                                    //    newDto.SetPropertyValue(cmb.Name, -1);
                                    //    DataBindingHelper.BindData4CmbByEnum<T>(newDto, queryFieldData.expEnumValueColName, queryFieldData.EnumType, cmb, queryFieldData.AddSelectItem);

                                    //    #endregion
                                    //}

                                }



                            }
                            cmb.Location = new System.Drawing.Point(_x, _y);
                            UcPanel.Controls.Add(cmb);
                            UcPanel.Controls.Add(lbl);

                            //cmb.SelectedIndex = -1;

                            #endregion

                        }


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

                        KryptonTextBox tb_box = new KryptonTextBox();
                        tb_box.Name = newFieldlist[c].FieldName;
                        tb_box.Width = 150;

                        DataBindingHelper.BindData4TextBox(newDto, newFieldlist[c].FieldName, tb_box, BindDataType4TextBox.Text, false);
                        tb_box.Location = new System.Drawing.Point(_x, _y);

                        UcPanel.Controls.Add(tb_box);
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
            return Query;
        }

        /*

        /// <summary>
        /// 是否使用模糊查询  思路是动态创建一个类型，动态添加一些特殊的属性，后面再检测出来对应处理
        /// 如果指定了条件按条件生成。否则按固定逻辑生成
        /// </summary>
        /// <param name="useLike"></param>
        public BaseEntity SetBindData(bool useLike, Krypton.Toolkit.KryptonPanel UcPanel, List<QueryParameter<T>> QueryParameters)
        {
            if (QueryParameters == null)
            {
                QueryParameters = new List<QueryParameter<T>>();
            }
            var type = typeof(T);
            //视图或特殊情况指定的显示的条件等
            if (ReladtedEntityType != null)
            {
                type = ReladtedEntityType;
            }

            Type newtype = AttributesBuilder(useLike, type);
            object newDto = Activator.CreateInstance(newtype);
            BaseEntity Query = newDto as BaseEntity;
            var new_DtoEntityFieldNameList = UIHelper.GetDtoFieldNameList(newtype);

           

            List<BaseDtoField> baseDtoFields = new List<BaseDtoField>();
            //为了显示条件的顺序再一次修改，这样循环是利用这个查询参数的数组默认顺序
            foreach (QueryParameter<T> parameter in QueryParameters)
            {
                BaseDtoField bdf = new_DtoEntityFieldNameList.FirstOrDefault(b => b.SugarCol.ColumnName == parameter.QueryField);
                if (bdf != null)
                {
                    if (bdf.ColDataType.Name == "Byte[]")
                    {
                        continue;
                    }

                    baseDtoFields.Add(bdf);
                }
            }
            // 定义表格布局的行和列
            #region

            List<BaseDtoField> newFieldlist = baseDtoFields.OrderBy(d => d.ExtendedAttribute.Count).ToList();
            #endregion

            int row = 0;
            //定义每列放几组控件
            int col = 4;
            //计算有多少列 cols没有使用
            int cols = newFieldlist.Count % col;

            //计算有多少行,再计算多少列，每列中的元素下标
            #region
            int TotalRows = 0;
            int rows = 0;
            rows = newFieldlist.Count / 4;

            //% 运算符在 C# 中表示取模运算，即返回除法的余数。
            //并且余数是计算后面宽的一个数据来源，比方 10个控件，4个一行，余数是2。第三行就是2个，第一列是3个，第二列也是3个，第三，第四就是2个
            int remainder = newFieldlist.Count % col;
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
            for (int c = 0; c < newFieldlist.Count; c++)
            {
                var coldata = newFieldlist[c] as BaseDtoField;

                _x = 20 + c % col * 250;//计算控件在X轴位置
                int maxtextLen = GetMaxTextLen(newFieldlist, col, colFlag);
                if (coldata.Caption.Length < maxtextLen)
                {
                    _x = _x + (maxtextLen - coldata.Caption.Length) * 18;
                }
                if (c % col == 0)
                {
                    _y = 20 + 32 * c / col;//计算控件在Y轴的位置
                    colFlag++;
                }
                _Tabindex = c % col + c / col + _Tabindex;

                KryptonLabel lbl = new KryptonLabel();
                lbl.Text = coldata.Caption;
                //lbl.Dock = DockStyle.Right;
                lbl.Size = new Size(100, 30);
                lbl.Location = new System.Drawing.Point(_x, _y);

                //一行控件平行
                //一个字算18px
                _x = _x + 18 * coldata.Caption.Length;

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
                        if (newFieldlist[c].UseLike)
                        {
                            UCAdvYesOrNO chkgroup = new UCAdvYesOrNO();

                            object datetimeValue1 = ReflectionHelper.GetPropertyValue(newDto, coldata.ExtendedAttribute[0].ColName);
                            chkgroup.rdb1.Text = "是";
                            chkgroup.rdb2.Text = "否";
                            DataBindingHelper.BindData4CehckBox(newDto, coldata.ExtendedAttribute[0].ColName, chkgroup.chk, false);
                            DataBindingHelper.BindData4RadioGroupTrueFalse(newDto, coldata.ExtendedAttribute[0].RelatedFields, chkgroup.rdb1, chkgroup.rdb2);
                            chkgroup.Location = new System.Drawing.Point(_x, _y);
                            UcPanel.Controls.Add(chkgroup);
                            UcPanel.Controls.Add(lbl);
                        }
                        else
                        {
                            KryptonCheckBox chk = new KryptonCheckBox();
                            chk.Name = newFieldlist[c].ColName;
                            chk.Text = "";

                            //newDto
                            DataBindingHelper.BindData4CehckBox(newDto, newFieldlist[c].ColName, chk, false);
                            chk.Location = new System.Drawing.Point(_x, _y);
                            UcPanel.Controls.Add(chk);
                            UcPanel.Controls.Add(lbl);
                        }

                        break;
                    case EnumDataType.DateTime:
                        if (newFieldlist[c].UseLike)
                        {
                            UCAdvDateTimerPickerGroup dtpgroup = new UCAdvDateTimerPickerGroup();
                            object datetimeValue1 = ReflectionHelper.GetPropertyValue(newDto, coldata.ExtendedAttribute[0].ColName);
                            DataBindingHelper.BindData4DataTime(newDto, datetimeValue1, coldata.ExtendedAttribute[0].ColName, dtpgroup.dtp1, false);
                            dtpgroup.dtp1.Checked = true;
                            object datetimeValue2 = ReflectionHelper.GetPropertyValue(newDto, coldata.ExtendedAttribute[1].ColName);
                            DataBindingHelper.BindData4DataTime(newDto, datetimeValue2, coldata.ExtendedAttribute[1].ColName, dtpgroup.dtp2, false);
                            dtpgroup.dtp2.Checked = true;
                            //时间控件更长为260px，这里要特殊处理

                            dtpgroup.Location = new System.Drawing.Point(_x, _y);
                            _x = _x + 260;
                            UcPanel.Controls.Add(dtpgroup);
                            UcPanel.Controls.Add(lbl);
                        }
                        else
                        {
                            KryptonDateTimePicker dtp = new KryptonDateTimePicker();
                            dtp.Name = newFieldlist[c].ColName;
                            dtp.ShowCheckBox = true;
                            object datetimeValue = ReflectionHelper.GetPropertyValue(newDto, newFieldlist[c].ColName);

                            DataBindingHelper.BindData4DataTime(newDto, datetimeValue, newFieldlist[c].ColName, dtp, false);
                            dtp.Checked = true;
                            dtp.Location = new System.Drawing.Point(_x, _y);
                            UcPanel.Controls.Add(dtp);
                            UcPanel.Controls.Add(lbl);
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
                        if (coldata.IsFKRelationAttribute || QueryParameters.Find(q => q.QueryField == newFieldlist[c].ColName).RelatedTableExpType != null)
                        {
                            //手动设置关联
                            if (QueryParameters.Find(q => q.QueryField == newFieldlist[c].ColName).RelatedTableExpType != null)
                            {
                                coldata.FKTableName = QueryParameters.Find(q => q.QueryField == newFieldlist[c].ColName).RelatedTableExpType.Name;
                            }

                            KryptonComboBox cmb = new KryptonComboBox();
                            cmb.Name = newFieldlist[c].ColName;
                            cmb.Text = "";
                            cmb.Width = 150;

                            QueryParameter<T> queryParameter = QueryParameters.Find(q => q.QueryField == cmb.Name);

                            //只处理需要缓存的表

                            KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                            if (CacheHelper.Manager.NewTableList.TryGetValue(coldata.FKTableName, out pair))
                            {
                                string IDColName = pair.Key;
                                string ColName = pair.Value;
                                //DataBindingHelper.BindData4Cmb<T>(QueryDto, key, value, coldata.FKTableName, cmb);
                                //这里加载时 是指定了相关的外键表的对应实体的类型
                                if (ReladtedEntityType == null)
                                {
                                    ReladtedEntityType = typeof(T);
                                }

                                //关联要绑定的类型
                                Type mytype = null;

                                //通过反射来执行类的静态方法
                                DataBindingHelper dbh = new DataBindingHelper();
                                if (queryParameter.IsView)
                                {
                                    mytype = queryParameter.RelatedTableExpType;
                                }
                                else
                                {
                                    PropertyInfo PI = ReflectionHelper.GetPropertyInfo(ReladtedEntityType, newDto, coldata.FKTableName.ToLower());
                                    mytype = PI.PropertyType;
                                }



                                if (queryParameter == null || (queryParameter.QueryFieldDataPara == null && queryParameter.FieldLimitCondition == null))
                                {
                                    #region 没有限制条件时

                                    if (mytype != null)
                                    {

                                        MethodInfo mf = dbh.GetType().GetMethod("BindData4CmbRef").MakeGenericMethod(new Type[] { mytype });
                                        object[] args = new object[5] { newDto, IDColName, ColName, coldata.FKTableName, cmb };
                                        mf.Invoke(dbh, args);
                                    }
                                    else
                                    {
                                        MainForm.Instance.logger.LogError("动态加载外键数据时出错，" + typeof(T).Name + "在代理类中的属性名不对，自动生成规则可能有变化" + coldata.FKTableName.ToLower());
                                        MainForm.Instance.uclog.AddLog("动态加载外键数据时出错加载数据出错。请联系管理员。");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    //非常值和学习借鉴有代码 TODO 重点学习代码
                                    //UI传入过滤条件 下拉可以显示不同的数据

                                    ExpConverter expConverter = new ExpConverter();
                                    var whereExp = expConverter.ExportByClassNameToT(queryParameter.limitedExpType, queryParameter.FieldLimitCondition);
                                    #region 
                                    //绑定下拉
                                    MethodInfo mf1 = dbh.GetType().GetMethod("BindData4CmbRefWithLimited").MakeGenericMethod(new Type[] { mytype });
                                    object[] args1 = new object[6] { newDto, IDColName, ColName, coldata.FKTableName, cmb, whereExp };
                                    mf1.Invoke(dbh, args1);


                                    //扩展查询
                                    //BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(coldata.FKTableName + "Processor");
                                    List<string> listconditions = queryParameter.SubQueryParameter;

                                    MethodInfo mf2 = dbh.GetType().GetMethod("InitFilterForControl").MakeGenericMethod(new Type[] { mytype });
                                    object[] args2 = new object[6] { newDto, cmb, ColName, whereExp, null, listconditions.ToArray() };
                                    mf2.Invoke(dbh, args2);

 



                                    #endregion
                                }
                            }
                            cmb.Location = new System.Drawing.Point(_x, _y);
                            UcPanel.Controls.Add(cmb);
                            UcPanel.Controls.Add(lbl);
                            //cmb.SelectedIndex = -1;
                        }
                        else
                        {
                            KryptonComboBox cmb = new KryptonComboBox();
                            cmb.Name = newFieldlist[c].ColName;
                            cmb.Text = "";
                            cmb.Width = 150;


                            #region 枚举类型处理
                            //DataBindingHelper.BindData4Cmb<T>(QueryDto, key, value, coldata.FKTableName, cmb);
                            //这里加载时 是指定了相关的外键表的对应实体的类型
                            if (ReladtedEntityType == null)
                            {
                                ReladtedEntityType = typeof(T);
                            }

                            //通过反射来执行类的静态方法
                            DataBindingHelper dbh = new DataBindingHelper();

                            QueryParameter<T> queryParameter = QueryParameters.Find(q => q.QueryField == cmb.Name);
                            if (queryParameter != null)
                            {
                                if (queryParameter.QueryFieldType == QueryFieldType.CmbEnum)
                                {
                                    //非常值和学习借鉴有代码 TODO 重点学习代码
                                    //UI传入过滤条件 下拉可以显示不同的数据
                                    if (queryParameter.QueryFieldType == QueryFieldType.CmbEnum)
                                    {
                                        #region ss



                                        #endregion

                                        EnumBindingHelper bindingHelper = new EnumBindingHelper();
                                        QueryFieldEnumData<T> queryFieldData = new QueryFieldEnumData<T>();
                                        queryFieldData = queryParameter.QueryFieldDataPara as QueryFieldEnumData<T>;
                                        #region 
                                        //绑定下拉值
                                        // bindingHelper.InitDataToCmbByEnumOnWhere(queryFieldData.BindDataSource, queryFieldData.EnumValueColName, cmb);
                                        //绑定实体值 这里没有指定 过滤
                                        //枚举的值 默认为-1，则指定为请选择
                                        newDto.SetPropertyValue(cmb.Name, -1);
                                        DataBindingHelper.BindData4CmbByEnum<T>(newDto, queryFieldData.expEnumValueColName, queryFieldData.EnumType, cmb, queryFieldData.AddSelectItem);
                                        //if (queryFieldData.AddSelectItem)
                                        //{
                                        //    cmb.SelectedIndex = cmb.FindString("请选择");
                                        //}
                                        #endregion
                                    }
                                    cmb.Location = new System.Drawing.Point(_x, _y);
                                    UcPanel.Controls.Add(cmb);
                                    UcPanel.Controls.Add(lbl);
                                }

                                //cmb.SelectedIndex = -1;

                                #endregion
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
                        //if (item.UseLike)
                        //{
                        KryptonTextBox tb_box = new KryptonTextBox();
                        tb_box.Name = newFieldlist[c].ColName;
                        tb_box.Width = 150;

                        DataBindingHelper.BindData4TextBox(newDto, newFieldlist[c].ColName, tb_box, BindDataType4TextBox.Text, false);
                        tb_box.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(tb_box);
                        UcPanel.Controls.Add(lbl);

                     
                        break;
                    default:
                        break;
                }


                row++;

            }
            sw.Stop();
            TimeSpan dt = sw.Elapsed;
            MainForm.Instance.uclog.AddLog("性能", "加载控件耗时：" + dt.TotalMilliseconds.ToString());
            
            return Query;
        }
        */

        /// <summary>
        /// 动态构建一些特性，针对不同的数据类型，比方日期等变动一个新的实体类型
        /// 注意这里构建代理类时是在原以字段后面加上Proxy,字段是_加下划线,这个在解析查询条件时会用到
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Type AttributesBuilder_New2024(bool useLike, Type type, QueryFilter queryFilter)
        {
            //TypeBuilder
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
                    coldata.ColDataType = coldata.ColDataType.GetBaseType();
                    if (coldata.ColDataType.Name == "Byte[]")
                    {
                        continue;
                    }

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



                            ////下拉
                            //if (coldata.IsFKRelationAttribute)
                            //{
                            //    string newSelectProName1 = coldata.ColName + "_请选择";
                            //    var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.ColName, "请选择", newSelectProName1, AdvQueryProcessType.defaultSelect });
                            //    //动态属性要提前创建生成，后面要实体化传入控件
                            //    PropertyBuilder newlikeProp1 = AddProperty(tb, newSelectProName1);
                            //    newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                            //}
                            //下拉
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
            return newtype;
        }


        /// <summary>
        /// 动态构建一些特性，针对不同的数据类型，比方日期等变动一个新的实体类型
        /// 注意这里构建代理类时是在原以字段后面加上Proxy,字段是_加下划线,这个在解析查询条件时会用到
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Type AttributesBuilder(bool useLike, Type type, QueryFilter queryFilter)
        {
            //TypeBuilder
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
                            ////下拉
                            //if (coldata.IsFKRelationAttribute)
                            //{
                            //    string newSelectProName1 = coldata.ColName + "_请选择";
                            //    var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.ColName, "请选择", newSelectProName1, AdvQueryProcessType.defaultSelect });
                            //    //动态属性要提前创建生成，后面要实体化传入控件
                            //    PropertyBuilder newlikeProp1 = AddProperty(tb, newSelectProName1);
                            //    newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                            //}
                            //下拉
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
            return newtype;
        }



        /*

        Label[] labels = new Label[9];  //存放动态生成的控件
        int labelNum = 0;


        private void AddLabel()
        {
         
        Label labelTemp = new Label();
        labelTemp.AutoSize = true;
                labelTemp.Font = new Font("微软雅黑", 10F, FontStyle.Regular, GraphicsUnit.Point, 134);
        // 设置字体

        labelTemp.Visible = true;
                labelTemp.Name = "label" + labelNum.ToString();
                switch (labelNum)
                {
                    case 1:
                        labelTemp.Text = "西湖";
                        labelTemp.Location = new Point(150, 60);
                        break;
                    case 2:
                        labelTemp.Text = "长城";
                        labelTemp.Location = new Point(150, 90);
                        break;
                    case 3:
                        labelTemp.Text = "故宫";
                        labelTemp.Location = new Point(150, 120);
                        break;
                    case 4:
                        labelTemp.Text = "外滩";
                        labelTemp.Location = new Point(150, 150);
                        break;
                    case 5:
                        labelTemp.Text = "泰山";
                        labelTemp.Location = new Point(150, 180);
                        break;
                    case 6:
                        labelTemp.Text = "黄山";
                        labelTemp.Location = new Point(150, 30);
                        break;
                    case 7:
                        labelTemp.Text = "庐山";
                        labelTemp.Location = new Point(150, 60);
                        break;
                    case 8:
                        labelTemp.Text = "天坛";
                        labelTemp.Location = new Point(150, 90);
                        break;
                    case 9:
                        labelTemp.Text = "漓江";
                        labelTemp.Location = new Point(150, 180);
                        break;
                    case 10:
                        labelTemp.Text = "大理";
                        labelTemp.Location = new Point(150, 30);
                        break;
                    default:
                        break;
                }
    labels[labelNum - 1] = labelTemp;
                //  Controls.Add(labelTemp);   //直接添加到窗体的定位处
                //  flowLayoutPanel.Controls.Add(labelTemp);  //添加到布局里面
            }
        */



        /// <summary>
        /// 取指定列的最大字段名称的长度
        /// </summary>
        /// <param name="Fieldlist"></param>
        /// <param name="colNum">每行放的控件列数(组）</param>
        /// <returns></returns>
        private int GetMaxTextLen(List<BaseDtoField> Fieldlist, int colNum, int targetCol)
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
            List<BaseDtoField> targetList = new List<BaseDtoField>();
            for (int i = targetCol; i < Fieldlist.Count; i = i + colNum)
            {
                targetList.Add(Fieldlist[i]);
            }
            MaxTextLen = targetList.Max(t => t.Caption.Length);
            return MaxTextLen;
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

        private PropertyBuilder AddProperty(TypeBuilder tb, string MemberName, Type memberType)
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
