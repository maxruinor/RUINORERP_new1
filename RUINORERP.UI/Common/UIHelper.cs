using Krypton.Toolkit;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Common;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;
using RUINORERP.Model.Base;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.UI.AdvancedQuery;
using RUINORERP.Common.Extensions;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using RUINORERP.UI.UControls;
using Krypton.Navigator;
using FastReport.Table;
using System.Drawing.Drawing2D;
using System.Drawing;
using StackExchange.Redis;

namespace RUINORERP.UI.Common
{
    public static class UIHelper
    {
        #region 查找控件
        public static KryptonCheckBox FindKryptonCheckBox(Control parentControl, string dataMember)
        {
            // 检查当前控件是否为 KryptonTextBox 并且具有匹配的 dataMember
            if (parentControl is KryptonCheckBox textBox)
            {
                if (textBox.DataBindings.Count > 0)
                {
                    foreach (Binding binding in textBox.DataBindings)
                    {
                        if (binding.BindingMemberInfo.BindingMember == dataMember)
                        {
                            return textBox;
                        }
                    }
                }
                else
                {
                    //按名称来查找
                    if (textBox.Name == "txt" + dataMember)
                    {
                        return textBox;
                    }
                }
            }

            // 递归检查所有子控件
            KryptonCheckBox foundTextBox = FindKryptonCheckBoxControls(parentControl.Controls, dataMember);
            if (foundTextBox != null)
            {
                return foundTextBox;
            }

            return null;
        }

        public static KryptonCheckBox FindKryptonCheckBoxControls(Control.ControlCollection controls, string dataMember)
        {
            foreach (Control control in controls)
            {
                // 检查当前控件是否为 KryptonTextBox 并且具有匹配的 dataMember
                if (control is KryptonCheckBox textBox)
                {
                    if (textBox.DataBindings.Count > 0)
                    {
                        foreach (Binding binding in textBox.DataBindings)
                        {
                            if (binding.BindingMemberInfo.BindingMember == dataMember)
                            {
                                return textBox;
                            }
                        }
                    }
                    else
                    {
                        //按名称来查找
                        if (textBox.Name == "txt" + dataMember)
                        {
                            return textBox;
                        }
                    }
                }

                // 如果当前控件有子控件，继续递归检查
                if (control.HasChildren)
                {
                    KryptonCheckBox foundTextBox = FindKryptonCheckBoxControls(control.Controls, dataMember);
                    if (foundTextBox != null)
                    {
                        return foundTextBox;
                    }
                }
            }
            return null;
        }



        public static KryptonTextBox FindTextBox(Control parentControl, string dataMember)
        {
            // 检查当前控件是否为 KryptonTextBox 并且具有匹配的 dataMember
            if (parentControl is KryptonTextBox textBox)
            {
                if (textBox.DataBindings.Count > 0)
                {
                    foreach (Binding binding in textBox.DataBindings)
                    {
                        if (binding.BindingMemberInfo.BindingMember == dataMember)
                        {
                            return textBox;
                        }
                    }
                }
                else
                {
                    //按名称来查找
                    if (textBox.Name == "txt" + dataMember)
                    {
                        return textBox;
                    }
                }
            }

            // 递归检查所有子控件
            KryptonTextBox foundTextBox = FindTextBoxInControls(parentControl.Controls, dataMember);
            if (foundTextBox != null)
            {
                return foundTextBox;
            }

            return null;
        }

        public static KryptonTextBox FindTextBoxInControls(Control.ControlCollection controls, string dataMember)
        {
            foreach (Control control in controls)
            {
                // 检查当前控件是否为 KryptonTextBox 并且具有匹配的 dataMember
                if (control is KryptonTextBox textBox)
                {
                    if (textBox.DataBindings.Count > 0)
                    {
                        foreach (Binding binding in textBox.DataBindings)
                        {
                            if (binding.BindingMemberInfo.BindingMember == dataMember)
                            {
                                return textBox;
                            }
                        }
                    }
                    else
                    {
                        //按名称来查找
                        if (textBox.Name == "txt" + dataMember)
                        {
                            return textBox;
                        }
                    }
                }

                // 如果当前控件有子控件，继续递归检查
                if (control.HasChildren)
                {
                    KryptonTextBox foundTextBox = FindTextBoxInControls(control.Controls, dataMember);
                    if (foundTextBox != null)
                    {
                        return foundTextBox;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 显示的文字找不到就按名称的规则查找
        /// </summary>
        /// <param name="parentControl"></param>
        /// <param name="LabelText"></param>
        /// <param name="LabelName"></param>
        /// <returns></returns>
        public static KryptonLabel FindLabel(Control parentControl, string LabelText, string LabelName)
        {
            // 检查当前控件是否为 KryptonTextBox 并且具有匹配的 dataMember
            if (parentControl is KryptonLabel lbl && lbl.Text.Length > 0)
            {
                if (lbl.Text == LabelText)
                {
                    return lbl;
                }
                if (lbl.Name == "lbl" + LabelName)
                {
                    return lbl;
                }
            }
            // 递归检查所有子控件
            KryptonLabel foundLabel = FindLabelInControls(parentControl.Controls, LabelText, LabelName);
            if (foundLabel != null)
            {
                return foundLabel;
            }

            return null;
        }

        public static KryptonLabel FindLabelInControls(Control.ControlCollection controls, string LabelText, string LabelName)
        {
            foreach (Control control in controls)
            {
                // 检查当前控件是否为 KryptonTextBox 并且具有匹配的 dataMember
                if (control is KryptonLabel lbl && lbl.Text.Length > 0)
                {
                    if (lbl.Text == LabelText)
                    {
                        return lbl;
                    }
                    if (lbl.Name == "lbl" + LabelName)
                    {
                        return lbl;
                    }
                }

                // 如果当前控件有子控件，继续递归检查
                if (control.HasChildren)
                {
                    KryptonLabel foundTextBox = FindLabelInControls(control.Controls, LabelText, LabelName);
                    if (foundTextBox != null)
                    {
                        return foundTextBox;
                    }
                }
            }
            return null;
        }


        #endregion
        public static void ControlColumnsInvisible(tb_MenuInfo CurMenuInfo,
            HashSet<string> InvisibleCols, HashSet<string> DefaultHideCols = null, bool isChild = false)
        {
            if (InvisibleCols == null)
            {
                InvisibleCols = new HashSet<string>();
            }

            if (DefaultHideCols == null)
            {
                DefaultHideCols = new HashSet<string>();
            }

            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Fields != null)
                {
                    List<tb_P4Field> P4Fields =
                    CurMenuInfo.tb_P4Fields
                    .Where(p => p.RoleID == MainForm.Instance.AppContext.CurrentUser_Role.RoleID
                    ).ToList();

                    foreach (var item in P4Fields)
                    {
                        if (item != null)
                        {
                            if (item.tb_fieldinfo != null && item.tb_fieldinfo.IsChild == isChild)
                            {
                                //权限设置了不可见
                                if (!item.IsVisble && !InvisibleCols.Contains(item.tb_fieldinfo.FieldName))
                                {
                                    InvisibleCols.Add(item.tb_fieldinfo.FieldName);
                                }
                                //系统级设置了字段不可用时
                                if (!item.tb_fieldinfo.IsEnabled && !InvisibleCols.Contains(item.tb_fieldinfo.FieldName))
                                {
                                    InvisibleCols.Add(item.tb_fieldinfo.FieldName);
                                }
                                if (item.tb_fieldinfo.DefaultHide && !DefaultHideCols.Contains(item.tb_fieldinfo.FieldName))
                                {
                                    DefaultHideCols.Add(item.tb_fieldinfo.FieldName);
                                }

                            }
                        }
                    }
                }
            }
        }
        public static object GetDisplayText(ConcurrentDictionary<string, List<KeyValuePair<object, string>>> ColNameDataDictionary,
            string colDbName, object Value, List<Type> ColDisplayTypes = null, Type entityType = null)
        {
            string DisplayText = string.Empty;
            //固定字典值显示
            if (ColNameDataDictionary.ContainsKey(colDbName))
            {
                List<KeyValuePair<object, string>> kvlist = new List<KeyValuePair<object, string>>();
                //意思是通过列名找，再通过值找到对应的文本
                ColNameDataDictionary.TryGetValue(colDbName, out kvlist);
                if (kvlist != null)
                {
                    KeyValuePair<object, string> kv = kvlist.FirstOrDefault(t => t.Key.ToString().ToLower() == Value.ToString().ToLower());
                    if (kv.Value != null)
                    {
                        DisplayText = kv.Value;
                        if (!string.IsNullOrEmpty(DisplayText))
                        {
                            return DisplayText;
                        }
                    }
                }
            }

            Type dataType = Value.GetType();
            // We need to check whether the property is NULLABLE
            if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                dataType = dataType.GetGenericArguments()[0];
            }
            //下次优化时。要注释一下 什么类型的字段 数据 要特殊处理。实际可能又把另一个情况弄错。
            switch (dataType.FullName)
            {
                case "System.Boolean":
                    break;
                case "System.DateTime":
                    ///DisplayText = Value.ToString();
                    return Value;
                    //DisplayText = DateTime.Parse(Value.ToString()).ToString("yyyy-MM-dd");
                    break;
                case "System.Int32":
                case "System.String":
                    DisplayText = Value.ToString();
                    break;
                default:
                    break;
            }


            if (!string.IsNullOrEmpty(DisplayText))
            {
                return DisplayText;
            }

            //动态字典值显示
            string colName = string.Empty;
            if (ColDisplayTypes != null && ColDisplayTypes.Count > 0)
            {
                colName = UIHelper.ShowGridColumnsNameValue(ColDisplayTypes.ToArray(), colDbName, Value);
            }
            else
            {
                colName = UIHelper.ShowGridColumnsNameValue(entityType, colDbName, Value);
            }
            if (!string.IsNullOrEmpty(colName))
            {
                DisplayText = colName;
            }
            return DisplayText;
        }
        /// <summary>
        /// 数组中值相乘带小数对于像这样的数组:[1,2,3,4]，我想得到 1*2*3*4 的乘积.
        /// </summary>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal MultiplyDecimalsWithRound(params decimal[] decimals)
        {
            return Math.Round(decimals.Aggregate(1m, (p, d) => p * d), 2);
        }
        /// <summary>
        /// 数组中值相乘对于像这样的数组:[1,2,3,4]，我想得到 1*2*3*4 的乘积.
        /// </summary>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static decimal MultiplyDecimals(params decimal[] decimals)
        {
            return decimals.Aggregate(1m, (p, d) => p * d);
        }

        public static Type GetTypeByName(string fullName)
        {
            //获取应用程序用到的所有程序集
            var asy = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < asy.Length; i++)
            {
                //从对应的程序集中根据名字字符串找的对应的实体类型
                //需要填写包括命名空间的类型全名，例"System.Windows.Forms.Button"
                if (asy[i].GetType(fullName, false) != null)
                {
                    return asy[i].GetType(fullName, false);
                }
            }
            return null;
        }
        #region 反射相关

        /// <summary>
        /// 动态创建字段
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="MemberName"></param>
        /// <returns></returns>
        public static PropertyBuilder AddProperty(TypeBuilder tb, string MemberName)
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
        #endregion


        #region 字段描述对应列表







        /// <summary>
        /// 根据特性生成指定的查询条件列的实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<BaseDtoField> GetDtoFieldNameList<T>()
        {

            List<BaseDtoField> fieldNameList = new List<BaseDtoField>();
            Type type = typeof(T);
            fieldNameList = GetDtoFieldNameList(type);
            return fieldNameList;
        }



        /// <summary>
        /// 目前所有表暂时只是一个主键
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetPrimaryKeyColName(Type type)
        {
            string PrimaryKeyColName = string.Empty;
            foreach (PropertyInfo field in type.GetProperties())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {

                    if (attr is SugarColumn sugarColumnAttr)
                    {

                        if (sugarColumnAttr.IsPrimaryKey)
                        {
                            PrimaryKeyColName = sugarColumnAttr.ColumnName;
                        }

                    }
                }
            }
            return PrimaryKeyColName;
        }




        /// <summary>
        /// 因为生成的高级查询DTO时，字段带了这个特性
        /// 去掉了主键这些,并且如果描述为空也没有提取。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<BaseDtoField> GetDtoFieldNameList(Type type)
        {
            List<AdvExtQueryAttribute> tempAdvExtList = new List<AdvExtQueryAttribute>();
            foreach (PropertyInfo field in type.GetProperties())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    if (attr is AdvExtQueryAttribute)
                    {
                        var advLikeAttr = attr as AdvExtQueryAttribute;
                        tempAdvExtList.Add(advLikeAttr);
                    }
                }
            }

            List<BaseDtoField> fieldNameList = new List<BaseDtoField>();

            AdvQueryAttribute advQueryAttr;
            //Type type = typeof(T);

            foreach (PropertyInfo field in type.GetProperties())
            {
                BaseDtoField bdf = new BaseDtoField();
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    //动态生成的
                    if (attr is AdvExtQueryAttribute)
                    {
                        continue;
                    }

                    if (attr is FKRelationAttribute)
                    {
                        FKRelationAttribute fkrattr = attr as FKRelationAttribute;
                        bdf.IsFKRelationAttribute = true;
                        bdf.fKRelationAttribute = fkrattr;
                        //bdf.FKPrimarykey = fkrattr.FK_IDColName;
                        bdf.FKTableName = fkrattr.FKTableName;
                    }

                    if (attr is SugarColumn sugarColumnAttr)
                    {
                        bdf.SugarCol = sugarColumnAttr;
                        sugarColumnAttr = attr as SugarColumn;
                        if (null != sugarColumnAttr)
                        {
                            if (sugarColumnAttr.ColumnDescription == null)
                            {
                                continue;
                            }
                            if (sugarColumnAttr.IsIdentity)
                            {
                                continue;
                            }
                            if (sugarColumnAttr.IsPrimaryKey)
                            {
                                continue;
                            }
                            //这里可以判断长度，来处理navarchar like情况
                            if (sugarColumnAttr.Length > 47483647)
                            {
                                bdf.UseLike = true;
                            }
                        }
                    }

                    if (attr is AdvQueryAttribute)
                    {
                        advQueryAttr = attr as AdvQueryAttribute;
                        if (advQueryAttr.ColDesc.Trim().Length > 0)
                        {
                            bdf.FieldName = advQueryAttr.ColName;
                            bdf.Caption = advQueryAttr.ColDesc;
                            bdf.ColDataType = field.PropertyType;
                            bdf.ExtendedAttribute = tempAdvExtList.Where(w => w.RelatedFields == advQueryAttr.ColName).ToList();
                            if (bdf.ExtendedAttribute.Count > 0)
                            {
                                bdf.UseLike = true;
                            }
                            fieldNameList.Add(bdf);
                        }
                    }
                }
            }

            return fieldNameList;
        }


        /// <summary>
        /// 获取实体的所有属性
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<BaseDtoField> GetALLFieldInfoList(Type type)
        {
            List<AdvExtQueryAttribute> tempAdvExtList = new List<AdvExtQueryAttribute>();
            foreach (PropertyInfo field in type.GetProperties())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    if (attr is AdvExtQueryAttribute)
                    {
                        var advLikeAttr = attr as AdvExtQueryAttribute;
                        tempAdvExtList.Add(advLikeAttr);
                    }
                }
            }

            List<BaseDtoField> fieldNameList = new List<BaseDtoField>();

            AdvQueryAttribute advQueryAttr;
            //Type type = typeof(T);

            foreach (PropertyInfo field in type.GetProperties())
            {
                BaseDtoField bdf = new BaseDtoField();
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    //动态生成的
                    if (attr is AdvExtQueryAttribute)
                    {
                        continue;
                    }

                    if (attr is FKRelationAttribute)
                    {
                        FKRelationAttribute fkrattr = attr as FKRelationAttribute;
                        bdf.IsFKRelationAttribute = true;
                        bdf.fKRelationAttribute = fkrattr;
                        // bdf.FKPrimarykey = fkrattr.FK_IDColName;
                        bdf.FKTableName = fkrattr.FKTableName;
                    }

                    if (attr is SugarColumn sugarColumnAttr)
                    {
                        bdf.SugarCol = sugarColumnAttr;
                        bdf.ColDataType = field.PropertyType;
                        sugarColumnAttr = attr as SugarColumn;
                        if (null != sugarColumnAttr)
                        {
                            //if (sugarColumnAttr.ColumnDescription == null)
                            //{
                            //    continue;
                            //}
                            //if (sugarColumnAttr.IsIdentity)
                            //{
                            //    continue;
                            //}
                            //if (sugarColumnAttr.IsPrimaryKey)
                            //{
                            //    continue;
                            //}
                            //这里可以判断长度，来处理navarchar like情况
                            if (sugarColumnAttr.Length > 47483647)
                            {
                                bdf.UseLike = true;
                            }
                        }
                    }

                    if (attr is AdvQueryAttribute)
                    {
                        advQueryAttr = attr as AdvQueryAttribute;
                        if (advQueryAttr.ColDesc.Trim().Length > 0)
                        {
                            bdf.FieldName = advQueryAttr.ColName;
                            bdf.Caption = advQueryAttr.ColDesc;
                            bdf.ColDataType = field.PropertyType;
                            bdf.ExtendedAttribute = tempAdvExtList.Where(w => w.RelatedFields == advQueryAttr.ColName).ToList();
                            if (bdf.ExtendedAttribute.Count > 0)
                            {
                                bdf.UseLike = true;
                            }

                        }
                    }
                }
                if (bdf.SugarCol != null && bdf.SugarCol.ColumnName != null)
                {
                    fieldNameList.Add(bdf);
                }
                else
                {

                }

            }




            return fieldNameList;

        }


        /// <summary>
        /// 生成带查询的特性列表
        /// </summary>
        /// <typeparam name="T">比方查盘点单，T就是盘点主表实体</typeparam>
        /// <param name="Tcopy">这个是复制T,但是动态生成了一个特性</param>
        /// <param name="QueryConditions">指定的条件</param>
        /// <returns></returns>
        public static List<BaseDtoField> GetDtoFieldNameList<T>(Type Tcopy, List<Expression<Func<T, object>>> QueryConditions)
        {
            Type type = Tcopy;
            List<AdvExtQueryAttribute> tempAdvExtList = new List<AdvExtQueryAttribute>();
            foreach (PropertyInfo field in type.GetProperties())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    if (attr is AdvExtQueryAttribute)
                    {
                        var advLikeAttr = attr as AdvExtQueryAttribute;
                        tempAdvExtList.Add(advLikeAttr);
                    }
                }
            }

            List<BaseDtoField> fieldNameList = new List<BaseDtoField>();
            SugarColumn entityAttr;
            AdvQueryAttribute advQueryAttr;
            //Type type = typeof(T);

            foreach (PropertyInfo field in type.GetProperties())
            {
                BaseDtoField bdf = new BaseDtoField();
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    //动态生成的
                    if (attr is AdvExtQueryAttribute)
                    {
                        continue;
                    }

                    if (attr is FKRelationAttribute)
                    {
                        FKRelationAttribute fkrattr = attr as FKRelationAttribute;
                        bdf.IsFKRelationAttribute = true;
                        bdf.fKRelationAttribute = fkrattr;
                        // bdf.FKPrimarykey = fkrattr.FK_IDColName;
                        bdf.FKTableName = fkrattr.FKTableName;
                    }
                    entityAttr = attr as SugarColumn;
                    if (null != entityAttr)
                    {
                        if (entityAttr.ColumnDescription == null)
                        {
                            continue;
                        }
                        if (entityAttr.IsIdentity)
                        {
                            continue;
                        }
                        if (entityAttr.IsPrimaryKey)
                        {
                            continue;
                        }
                        //这里可以判断长度，来处理navarchar like情况
                        if (entityAttr.Length > 47483647)
                        {
                            bdf.UseLike = true;
                        }
                    }
                    if (attr is AdvQueryAttribute)
                    {
                        advQueryAttr = attr as AdvQueryAttribute;
                        if (advQueryAttr.ColDesc.Trim().Length > 0)
                        {
                            bdf.FieldName = advQueryAttr.ColName;
                            bdf.Caption = advQueryAttr.ColDesc;
                            bdf.ColDataType = field.PropertyType;
                            bdf.ExtendedAttribute = tempAdvExtList.Where(w => w.RelatedFields == advQueryAttr.ColName).ToList();
                            if (bdf.ExtendedAttribute.Count > 0)
                            {
                                bdf.UseLike = true;
                            }

                            //如果存在指定集合中才显示到查询UI当条件用
                            foreach (var item in QueryConditions)
                            {
                                var m = item.GetMemberInfo();
                                if (bdf.FieldName == m.Name)
                                {
                                    fieldNameList.Add(bdf);
                                }
                            }


                        }
                    }
                }
            }




            return fieldNameList;

        }
        #endregion



        #region 字段描述对应列表

        //这个是开始的思路，实际也可以tb_product 实例的 属性来实现
        public static ConcurrentDictionary<string, string> GetFieldNameList<T>(bool PrimaryKey = false)
        {
            Type type = typeof(T);
            return GetFieldNameList(PrimaryKey, type);
        }

        /// <summary>
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static ConcurrentDictionary<string, string> GetFieldNameList(bool PrimaryKey = false, params Type[] types)
        {
            ConcurrentDictionary<string, string> fieldNameList = new ConcurrentDictionary<string, string>();
            SugarColumn entityAttr;
            foreach (var type in types)
            {
                foreach (PropertyInfo field in type.GetProperties())
                {
                    bool brow = true;
                    var attributes = field.GetCustomAttributes(true);
                    if (attributes.Contains(new BrowsableAttribute(false)))
                    {
                        brow = false;
                        // Checks to see if the value of the BrowsableAttribute is Yes.
                        //if (attributes[typeof(BrowsableAttribute)].Equals(BrowsableAttribute.No))
                        //{
                        //    brow = false;
                        //}
                    }

                    foreach (Attribute attr in field.GetCustomAttributes(true))
                    {
                        //if (attr is BrowsableAttribute)
                        //{
                        //    BrowsableAttribute browAttr = attr as BrowsableAttribute;
                        //    if (!browAttr.Browsable)
                        //    {
                        //        brow = false;
                        //    }
                        //}

                        entityAttr = attr as SugarColumn;
                        if (null != entityAttr)
                        {
                            if (entityAttr.ColumnDescription == null)
                            {
                                continue;
                            }
                            if (entityAttr.IsIdentity)
                            {
                                continue;
                            }
                            if (!PrimaryKey && entityAttr.IsPrimaryKey)
                            {
                                continue;
                            }
                            if (entityAttr.ColumnDescription.Trim().Length > 0 && brow)
                            {
                                fieldNameList.TryAdd(field.Name, entityAttr.ColumnDescription);
                            }
                        }

                    }



                }

            }




            return fieldNameList;
        }

        public static ConcurrentDictionary<string, KeyValuePair<string, bool>> GetFieldNameColList(params Type[] types)
        {
            return GetFieldNameColList(false, types);
        }

        /// <summary>
        /// SugarColumn  依赖了这个特性来获取列名
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static ConcurrentDictionary<string, KeyValuePair<string, bool>> GetFieldNameColList(bool IncludePK = false, params Type[] types)
        {
            ConcurrentDictionary<string, KeyValuePair<string, bool>> fieldNameList = new ConcurrentDictionary<string, KeyValuePair<string, bool>>();
            SugarColumn entityAttr;
            foreach (var type in types)
            {
                foreach (PropertyInfo field in type.GetProperties())
                {
                    bool brow = true;
                    var attributes = field.GetCustomAttributes(true);
                    if (attributes.Contains(new BrowsableAttribute(false)))
                    {
                        brow = false;
                        // Checks to see if the value of the BrowsableAttribute is Yes.
                        //if (attributes[typeof(BrowsableAttribute)].Equals(BrowsableAttribute.No))
                        //{
                        //    brow = false;
                        //}
                    }
                    foreach (Attribute attr in field.GetCustomAttributes(true))
                    {
                        //if (attr is BrowsableAttribute)
                        //{
                        //    BrowsableAttribute browAttr = attr as BrowsableAttribute;
                        //    if (!browAttr.Browsable)
                        //    {
                        //        brow = false;
                        //    }
                        //}

                        entityAttr = attr as SugarColumn;
                        if (null != entityAttr)
                        {
                            if (entityAttr.ColumnDescription == null)
                            {
                                continue;
                            }
                            if (entityAttr.IsIdentity)
                            {
                                continue;
                            }
                            if (entityAttr.IsPrimaryKey)
                            {   //逻辑处理时可能要主键
                                if (IncludePK)
                                {
                                    fieldNameList.TryAdd(field.Name, new KeyValuePair<string, bool>(entityAttr.ColumnDescription, true));
                                }
                                else
                                {
                                    fieldNameList.TryAdd(field.Name, new KeyValuePair<string, bool>(entityAttr.ColumnDescription, false));
                                }

                            }
                            if (entityAttr.ColumnDescription.Trim().Length > 0 && brow)
                            {
                                fieldNameList.TryAdd(field.Name, new KeyValuePair<string, bool>(entityAttr.ColumnDescription, true));
                            }
                        }

                    }



                }

            }

            return fieldNameList;
        }

        /// <summary>
        /// 获取outlook列的显示控制列表
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static List<ColDisplayController> GetColumnDisplayList(params Type[] types)
        {
            List<ColDisplayController> columnDisplayControllers = new List<ColDisplayController>();

            SugarColumn entityAttr;
            foreach (var type in types)
            {
                foreach (PropertyInfo field in type.GetProperties())
                {
                    bool Browsable = true;
                    var attributes = field.GetCustomAttributes(true);
                    if (attributes.Contains(new BrowsableAttribute(false)))
                    {
                        Browsable = false;
                        // Checks to see if the value of the BrowsableAttribute is Yes.
                        //if (attributes[typeof(BrowsableAttribute)].Equals(BrowsableAttribute.No))
                        //{
                        //    brow = false;
                        //}
                    }
                    foreach (Attribute attr in field.GetCustomAttributes(true))
                    {
                        //if (attr is BrowsableAttribute)
                        //{
                        //    BrowsableAttribute browAttr = attr as BrowsableAttribute;
                        //    if (!browAttr.Browsable)
                        //    {
                        //        brow = false;
                        //    }
                        //}

                        entityAttr = attr as SugarColumn;
                        if (null != entityAttr)
                        {
                            //类型
                            if (entityAttr.ColumnDescription == null)
                            {
                                ColDisplayController col = new ColDisplayController();
                                col.ColDisplayIndex = 0;
                                col.Visible = false;//默认不显示主键
                                col.ColName = field.Name;
                                col.Disable = true;
                                columnDisplayControllers.Add(col);
                                continue;
                            }
                            if (entityAttr.IsIgnore)
                            {
                                ColDisplayController col = new ColDisplayController();
                                col.ColDisplayText = entityAttr.ColumnDescription;
                                col.ColDisplayIndex = columnDisplayControllers.Count;
                                col.Visible = false;//默认不显示主键
                                col.ColName = field.Name;
                                col.Disable = true;
                                columnDisplayControllers.Add(col);
                                continue;
                            }

                            if (entityAttr.IsIdentity)
                            {
                                ColDisplayController col = new ColDisplayController();
                                col.ColDisplayText = entityAttr.ColumnDescription;
                                col.ColDisplayIndex = columnDisplayControllers.Count;
                                col.Visible = (entityAttr.ColumnDescription.Trim().Length > 0) ? true : false;
                                col.ColName = field.Name;
                                col.Disable = (entityAttr.ColumnDescription.Trim().Length > 0) ? false : true;
                                columnDisplayControllers.Add(col);
                                continue;
                            }
                            if (entityAttr.IsPrimaryKey)
                            {   //逻辑处理时可能要主键
                                ColDisplayController col = new ColDisplayController();
                                col.ColDisplayText = entityAttr.ColumnDescription;
                                col.ColDisplayIndex = columnDisplayControllers.Count;
                                col.Visible = false;//默认不显示主键
                                col.ColName = field.Name;
                                col.Disable = (entityAttr.ColumnDescription.Trim().Length > 0) ? false : true;
                                columnDisplayControllers.Add(col);
                                continue;
                            }

                            if (entityAttr.ColumnDescription.Trim().Length > 0 && Browsable)
                            {
                                ColDisplayController col = new ColDisplayController();
                                col.ColDisplayText = entityAttr.ColumnDescription;
                                col.ColDisplayIndex = columnDisplayControllers.Count;
                                col.Visible = true;
                                col.ColName = field.Name;
                                columnDisplayControllers.Add(col);
                            }
                            else
                            {
                                //逻辑处理时可能要主键
                                ColDisplayController col = new ColDisplayController();
                                col.ColDisplayText = entityAttr.ColumnDescription;
                                col.ColDisplayIndex = columnDisplayControllers.Count;
                                col.Visible = (entityAttr.ColumnDescription.Trim().Length > 0) ? true : false;
                                col.ColName = field.Name;
                                col.Disable = (entityAttr.ColumnDescription.Trim().Length > 0) ? false : true;
                                columnDisplayControllers.Add(col);
                            }
                        }

                    }



                }

            }
            return columnDisplayControllers;
        }



        /// <summary>
        /// outlook分析时列需要取类型
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static ConcurrentDictionary<string, KeyValuePair<string, SugarColumn>> GetFieldNamePropertyInfoList(params Type[] types)
        {
            ConcurrentDictionary<string, KeyValuePair<string, SugarColumn>> fieldNameList = new ConcurrentDictionary<string, KeyValuePair<string, SugarColumn>>();
            SugarColumn entityAttr;
            foreach (var type in types)
            {
                foreach (PropertyInfo field in type.GetProperties())
                {
                    bool brow = true;
                    var attributes = field.GetCustomAttributes(true);
                    if (attributes.Contains(new BrowsableAttribute(false)))
                    {
                        brow = false;
                        // Checks to see if the value of the BrowsableAttribute is Yes.
                        //if (attributes[typeof(BrowsableAttribute)].Equals(BrowsableAttribute.No))
                        //{
                        //    brow = false;
                        //}
                    }
                    foreach (Attribute attr in field.GetCustomAttributes(true))
                    {
                        //if (attr is BrowsableAttribute)
                        //{
                        //    BrowsableAttribute browAttr = attr as BrowsableAttribute;
                        //    if (!browAttr.Browsable)
                        //    {
                        //        brow = false;
                        //    }
                        //}

                        entityAttr = attr as SugarColumn;
                        if (null != entityAttr)
                        {
                            if (entityAttr.ColumnDescription == null)
                            {
                                continue;
                            }
                            if (entityAttr.IsIdentity)
                            {
                                continue;
                            }
                            if (entityAttr.IsPrimaryKey)
                            {   //逻辑处理时可能要主键
                                fieldNameList.TryAdd(field.Name, new KeyValuePair<string, SugarColumn>(entityAttr.ColumnDescription, entityAttr));
                            }
                            if (entityAttr.ColumnDescription.Trim().Length > 0 && brow)
                            {
                                fieldNameList.TryAdd(field.Name, new KeyValuePair<string, SugarColumn>(entityAttr.ColumnDescription, entityAttr));
                            }
                        }

                    }



                }

            }




            return fieldNameList;
        }



        #endregion



        #region 实现通过缓存，传入对应的ID的列名和值，找到对应的显示名称

        /// <summary>
        /// 用来保存外键表名与外键主键列名  通过这个打到对应的名称。
        /// </summary>
        public static ConcurrentDictionary<string, string> FKValueColNameTBList = new ConcurrentDictionary<string, string>();

        public static void SetFKTBList<C>() where C : class
        {
            string tableName = typeof(C).Name;

            foreach (var field in typeof(C).GetProperties())
            {
                //获取指定类型的自定义特性
                object[] attrs = field.GetCustomAttributes(false);
                foreach (var attr in attrs)
                {
                    if (attr is FKRelationAttribute)
                    {
                        FKRelationAttribute fkrattr = attr as FKRelationAttribute;
                        FKValueColNameTBList.TryAdd(fkrattr.FK_IDColName, fkrattr.FKTableName);
                    }
                }
            }
        }
        #endregion


        public static void ControlButton(tb_MenuInfo CurMenuInfo, ToolStripSplitButton btnItem)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Buttons == null)
                {
                    btnItem.Visible = false;
                }
                else
                {
                    //如果因为热键 Text改变了。到时再处理
                    tb_P4Button p4b = CurMenuInfo.tb_P4Buttons.Where(b => b.tb_buttoninfo.BtnText == btnItem.Text).FirstOrDefault();
                    if (p4b != null)
                    {
                        btnItem.Visible = p4b.IsVisble;
                        btnItem.Enabled = p4b.IsEnabled;
                    }
                    else
                    {
                        btnItem.Visible = false;
                    }
                }
            }
        }

        public static void ControlButton(tb_MenuInfo CurMenuInfo, ToolStripMenuItem btnItem)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Buttons == null)
                {
                    btnItem.Visible = false;
                }
                else
                {
                    //如果因为热键 Text改变了。到时再处理
                    tb_P4Button p4b = CurMenuInfo.tb_P4Buttons.Where(b => b.tb_buttoninfo.BtnText == btnItem.Text).FirstOrDefault();
                    if (p4b != null)
                    {
                        btnItem.Visible = p4b.IsVisble;
                        btnItem.Enabled = p4b.IsEnabled;
                    }
                    else
                    {
                        btnItem.Visible = false;
                    }
                }
            }
        }

        public static void ControlButton(tb_MenuInfo CurMenuInfo, ToolStripDropDownButton btnItem)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Buttons == null)
                {
                    btnItem.Visible = false;
                }
                else
                {
                    //如果因为热键 Text改变了。到时再处理
                    tb_P4Button p4b = CurMenuInfo.tb_P4Buttons.Where(b => b.tb_buttoninfo.BtnText == btnItem.Text).FirstOrDefault();
                    if (p4b != null)
                    {
                        btnItem.Visible = p4b.IsVisble;
                        btnItem.Enabled = p4b.IsEnabled;
                    }
                    else
                    {
                        btnItem.Visible = false;
                    }
                }
            }
        }

        public static void ControlButton(tb_MenuInfo CurMenuInfo, ToolStripButton btnItem)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Buttons == null)
                {
                    btnItem.Visible = false;
                }
                else
                {
                    //如果因为热键 Text改变了。到时再处理
                    tb_P4Button p4b = CurMenuInfo.tb_P4Buttons.Where(b => b.tb_buttoninfo.BtnText == btnItem.Text).FirstOrDefault();
                    if (p4b != null)
                    {
                        btnItem.Visible = p4b.IsVisble;
                        btnItem.Enabled = p4b.IsEnabled;
                    }
                    else
                    {
                        btnItem.Visible = false;
                    }
                }
            }
        }
        public static void ControlButton(tb_MenuInfo CurMenuInfo, ToolStripItem btnItem)
        {
            if (!MainForm.Instance.AppContext.IsSuperUser)
            {
                if (CurMenuInfo.tb_P4Buttons == null)
                {
                    btnItem.Visible = false;
                }
                else
                {
                    //如果因为热键 Text改变了。到时再处理
                    tb_P4Button p4b = CurMenuInfo.tb_P4Buttons.Where(b => b.tb_buttoninfo.BtnText == btnItem.Text).FirstOrDefault();
                    if (p4b != null)
                    {
                        btnItem.Visible = p4b.IsVisble;
                        btnItem.Enabled = p4b.IsEnabled;
                    }
                    else
                    {
                        btnItem.Visible = false;
                    }
                }
            }
        }


        /// <summary>
        /// 通过基础资料表中的id列名和值找到对应的描述
        /// 通过T表的关联外键的所有表及主键和找到对应的名称列并且显示出来
        /// </summary>
        /// <param name="type">一定是表的类型，在系统初始时传的是常用表数据缓存</param>
        /// <param name="idColName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ShowGridColumnsNameValue(Type[] types, string idColName, object value)
        {
            string NameValue = string.Empty;
            foreach (var t in types)
            {
                NameValue = ShowGridColumnsNameValue(t, idColName, value);
                if (!string.IsNullOrEmpty(NameValue))
                {
                    break;
                }
            }

            return NameValue;
        }


        /// <summary>
        /// 通过基础资料表中的id列名和值找到对应的描述
        /// 通过T表的关联外键的所有表及主键和找到对应的名称列并且显示出来
        /// </summary>
        /// <param name="type">一定是表的类型，在系统初始时传的是常用表数据缓存</param>
        /// <param name="idColName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ShowGridColumnsNameValue(Type type, string idColName, object value)
        {
            return ShowGridColumnsNameValue(type.Name, idColName, value, type);
        }

        /// <summary>
        /// 通过基础资料表中的id列名和值找到对应的描述
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="idColName"></param>
        /// <param name="value"></param>
        /// <param name="type">如果是视图则不能为空，否则可以为空。直接用表名即可</param>
        /// <returns></returns>
        public static string ShowGridColumnsNameValue(string TableName, string idColName, object value
            , Type type = null)
        {
            if (value == null)
            {
                return string.Empty;
            }
            //如果是decimal类型，直接返回
            if (value.GetType().Name == "Decimal")
            {
                return value.ToString();
            }
            //如果是decimal类型，直接返回
            if (value.GetType().Name != "Int64")
            {
                return value.ToString();
            }

            string NameValue = string.Empty;

            //如果是修改人创建人统一处理,并且要放在前面
            //定义两个字段为了怕后面修改，不使用字符串
            //2024-2-17 完善 这里应该是有点小问题
            Expression<Func<tb_Employee, long>> Created_by;
            Expression<Func<tb_Employee, long>> Modified_by;
            Expression<Func<tb_Employee, long>> Approver_by;

            Approver_by = c => c.Employee_ID;
            Created_by = c => c.Created_by.Value;
            Modified_by = c => c.Modified_by.Value;

            if (idColName == Created_by.GetMemberInfo().Name || idColName == Modified_by.GetMemberInfo().Name || idColName == "Approver_by")
            {
                if (value.ToString() == "0")
                {
                    return "";
                }
                string baseTableName = typeof(tb_Employee).Name;
                object obj = BizCacheHelper.Instance.GetValue(baseTableName, value);
                if (obj != null && obj.ToString() != "System.Object")
                {
                    NameValue = obj.ToString();
                    return NameValue;
                }
            }

            //自己引用自己的特殊处理 如类 目 BOM?
            if (typeof(tb_ProdCategories).Name == TableName)
            {
                #region
                string tableName = TableName;
                if (!BizCacheHelper.Manager.FkPairTableList.ContainsKey(tableName))
                {
                    List<KeyValuePair<string, string>> kvSelflist = new List<KeyValuePair<string, string>>();
                    Expression<Func<tb_ProdCategories, long?>> expKey = c => c.Parent_id;
                    MemberInfo minfo = expKey.GetMemberInfo();
                    string key = minfo.Name;
                    KeyValuePair<string, string> kv = new KeyValuePair<string, string>(key, typeof(tb_ProdCategories).Name);
                    kvSelflist.Add(kv);
                    if (kvSelflist.Count > 0)
                    {
                        BizCacheHelper.Manager.FkPairTableList.TryAdd(tableName, kvSelflist);
                    }
                }

                #endregion
            }

            //视图暂时没有实体生成时没有设置关联外键的特性，所以在具体业务实现时 手工指定成一个集合了。
            if (!TableName.Contains("View") && type != null)
            {
                BizCacheHelper.Manager.SetFkColList(type);
            }

            //优先处理本身，比方 BOM_ID显示BOM_NO，只要传tb_BOM_S
            if (BizCacheHelper.Manager.NewTableList.ContainsKey(TableName))
            {
                var nkv = BizCacheHelper.Manager.NewTableList[TableName];
                if (nkv.Key == idColName)
                {
                    string baseTableName = TableName;
                    object obj = BizCacheHelper.Instance.GetValue(baseTableName, value);
                    if (obj != null && obj.GetType().Name != "Object")
                    {
                        NameValue = obj.ToString();
                        return NameValue;
                    }
                }

            }

            List<KeyValuePair<string, string>> kvlist = new List<KeyValuePair<string, string>>();
            if (BizCacheHelper.Manager.FkPairTableList.TryGetValue(TableName, out kvlist))
            {
                var kv = kvlist.Find(k => k.Key == idColName);
                //如果找不到呢？
                if (kv.Key == null)
                {
                    return string.Empty;
                }
                string baseTableName = kv.Value;
                object obj = BizCacheHelper.Instance.GetValue(baseTableName, value);
                if (obj != null)
                {
                    NameValue = obj.ToString();
                }
            }
            if (string.IsNullOrEmpty(NameValue) && TableName.Contains("View"))
            {
                object obj = BizCacheHelper.Instance.GetValue(TableName, value);
                if (obj != null && obj.GetType().Name != "Object")
                {
                    NameValue = obj.ToString();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(NameValue))
                {
                    object obj = BizCacheHelper.Instance.GetValue(TableName, value);
                    if (obj != null && obj.GetType().Name != "Object")
                    {
                        NameValue = obj.ToString();
                    }
                }
            }
            return NameValue;
        }

        public static string ShowGridColumnsNameValue<T>(string idColName, object value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            //如果是KEY类型，直接返回
            if (value.GetType().Name != "Int64")
            {
                return value.ToString();
            }
            string NameValue = string.Empty;
            //如果是修改人创建人统一处理,并且要放在前面
            //定义两个字段为了怕后面修改，不使用字符串
            Expression<Func<tb_Employee, long>> Created_by;
            Expression<Func<tb_Employee, long>> Modified_by;
            Expression<Func<tb_Employee, long>> Approver_by;
            Created_by = c => c.Created_by.Value;
            Modified_by = c => c.Modified_by.Value;
            Approver_by = c => c.Employee_ID;

            var Approver_byName = "Approver_by";//上面指定的是一个表达式，这里指的是一个字段名。即哪个字段显示的是ID实际要显示名称。
            if (idColName == Created_by.GetMemberInfo().Name || idColName == Modified_by.GetMemberInfo().Name || idColName == Approver_byName)
            {
                if (value.ToString() == "0")
                {
                    return "";
                }
                //通过id找名称，知道哪一个表的情况下。在缓存里面找
                string baseTableName = typeof(tb_Employee).Name;
                object obj = BizCacheHelper.Instance.GetValue(baseTableName, value);
                if (obj != null && obj.ToString() != "System.Object")
                {
                    NameValue = obj.ToString();
                    return NameValue;
                }
            }

            //自己引用自己的特殊处理 如类目 BOM?
            if (typeof(tb_ProdCategories).Name == typeof(T).Name)
            {
                #region
                string tableName = typeof(T).Name;
                if (!BizCacheHelper.Manager.FkPairTableList.ContainsKey(tableName))
                {
                    List<KeyValuePair<string, string>> kvSelflist = new List<KeyValuePair<string, string>>();
                    Expression<Func<tb_ProdCategories, long?>> expKey = c => c.Parent_id;
                    MemberInfo minfo = expKey.GetMemberInfo();
                    string key = minfo.Name;
                    KeyValuePair<string, string> kv = new KeyValuePair<string, string>(key, typeof(tb_ProdCategories).Name);
                    kvSelflist.Add(kv);
                    if (kvSelflist.Count > 0)
                    {
                        BizCacheHelper.Manager.FkPairTableList.TryAdd(tableName, kvSelflist);
                    }
                }

                #endregion
            }
            if (!typeof(T).Name.Contains("View"))
            {
                BizCacheHelper.Manager.SetFkColList<T>();
            }

            List<KeyValuePair<string, string>> kvlist = new List<KeyValuePair<string, string>>();
            if (BizCacheHelper.Manager.FkPairTableList.TryGetValue(typeof(T).Name, out kvlist))
            {
                var kv = kvlist.Find(k => k.Key == idColName);
                //如果找不到呢？
                if (kv.Key == null)
                {
                    return string.Empty;
                }
                string baseTableName = kv.Value;
                object obj = BizCacheHelper.Instance.GetValue(baseTableName, value);
                if (obj != null)
                {
                    NameValue = obj.ToString();
                }
            }

            return NameValue;
        }


        #region 注入窗体-开始


        /// <summary>
        /// 获取对应的窗体用于首次自动生成菜单
        /// </summary>
        /// <returns></returns>
        public static List<MenuAttrAssemblyInfo> RegisterForm(string assemblyPath)
        {
            List<MenuAttrAssemblyInfo> infolist = new List<MenuAttrAssemblyInfo>();
            Type[] types = null;
            if (string.IsNullOrEmpty(assemblyPath))
            {
                types = Assembly.GetExecutingAssembly()?.GetExportedTypes();
            }
            else
            {
                types = Assembly.LoadFrom(assemblyPath).GetExportedTypes();
            }
            //  Type[]? types = Assembly.GetExecutingAssembly()?.GetExportedTypes();

            if (types != null)
            {
                var descType = typeof(MenuAttrAssemblyInfo);
                var form = typeof(Form);
                foreach (Type type in types)
                {
                    // 类型是否为窗体，否则跳过，进入下一个循环
                    //if (type.GetTypeInfo != form)
                    //    continue;

                    // 是否为自定义特性，否则跳过，进入下一个循环
                    if (!type.IsDefined(descType, false))
                        continue;
                    // 强制为自定义特性
                    MenuAttrAssemblyInfo? attribute = type.GetCustomAttribute(descType, false) as MenuAttrAssemblyInfo;
                    // 如果强制失败或者不需要注入的窗体跳过，进入下一个循环
                    if (attribute == null || !attribute.Enabled)
                        continue;
                    // 域注入
                    //Services.AddScoped(type);
                    MenuAttrAssemblyInfo info = new MenuAttrAssemblyInfo();
                    info = attribute;
                    info.ClassName = type.Name;
                    info.ClassType = type;
                    info.ClassPath = type.FullName;
                    info.Caption = attribute.Describe;
                    info.MenuPath = attribute.MenuPath;
                    info.UiType = attribute.UiType;
                    if (attribute.MenuBizType.HasValue)
                    {
                        info.MenuBizType = attribute.MenuBizType.Value;
                    }
                    info.BIBaseForm = type.BaseType.Name;
                    if (type.BaseType.IsGenericType)
                    {
                        Type[] paraTypes = type.BaseType.GetGenericArguments();
                        if (paraTypes.Length > 0)
                        {
                            info.EntityName = paraTypes[0].Name;
                        }
                        //如果类型是例如此代码可为空，返回int部分(底层类型)。如果只需要将对象转换为特定类型，则可以使用System.Convert.ChangeType方法。
                    }
                    else
                    {
                        //UserControl 都是这个类型
                        if (type.BaseType.FullName == "RUINORERP.UI.BaseForm.BaseList")
                        {
                            //这个实例化过程 到UCUnitList时居然会执行里面的Query方法 导致 出错。只是会在init菜单时
                            var RelatedForms = Startup.GetFromFacByName<UserControl>(type.Name);
                            //var menu=Startup.GetFromFac<UI.BI.UCLocationTypeList>();
                            //获取关联实体名
                        }
                        //if (type.BaseType.Name == "BaseListWithTree")
                        //{
                        //    info.EntityName = typeof(tb_ProdCategories).Name;
                        //}
                        // Console.WriteLine($"注入：{attribute.FormType.Namespace}.{attribute.FormType.Name},{attribute.Describe}");
                    }

                    //特性标记
                    if (!string.IsNullOrEmpty(info.MenuPath))
                    {
                        infolist.Add(info);
                    }

                }
            }


            return infolist;
        }

        public static List<MenuAttrAssemblyInfo> RegisterForm()
        {
            return RegisterForm("");
        }
        #endregion 注入窗体-结束


        #region 注入窗体-开始  新2023


        /// <summary>
        /// 获取对应的窗体用于首次自动生成菜单
        /// </summary>
        /// <returns></returns>
        public static List<MenuAttrAssemblyInfo> RegisterFormNew(string assemblyPath)
        {
            List<MenuAttrAssemblyInfo> infolist = new List<MenuAttrAssemblyInfo>();
            Type[] types = null;
            if (string.IsNullOrEmpty(assemblyPath))
            {
                types = Assembly.GetExecutingAssembly()?.GetExportedTypes();
            }
            else
            {
                types = Assembly.LoadFrom(assemblyPath).GetExportedTypes();
            }
            //  Type[]? types = Assembly.GetExecutingAssembly()?.GetExportedTypes();

            if (types != null)
            {
                var descType = typeof(MenuAttrAssemblyInfo);
                var form = typeof(Form);
                foreach (Type type in types)
                {
                    // 类型是否为窗体，否则跳过，进入下一个循环
                    //if (type.GetTypeInfo != form)
                    //    continue;

                    // 是否为自定义特性，否则跳过，进入下一个循环
                    if (!type.IsDefined(descType, false))
                        continue;
                    // 强制为自定义特性
                    MenuAttrAssemblyInfo? attribute = type.GetCustomAttribute(descType, false) as MenuAttrAssemblyInfo;
                    // 如果强制失败或者不需要注入的窗体跳过，进入下一个循环
                    if (attribute == null || !attribute.Enabled)
                        continue;
                    // 域注入
                    //Services.AddScoped(type);
                    MenuAttrAssemblyInfo info = new MenuAttrAssemblyInfo();
                    info.ClassName = type.Name;
                    info.ClassPath = type.FullName;
                    info.Caption = attribute.Describe;
                    info.MenuPath = attribute.MenuPath;
                    info.UiType = attribute.UiType;
                    if (attribute.MenuBizType.HasValue)
                    {
                        info.MenuBizType = attribute.MenuBizType.Value;
                    }
                    info.BIBaseForm = type.BaseType.Name;
                    if (type.BaseType.IsGenericType)
                    {





                        Type[] paraTypes = type.BaseType.GetGenericArguments();
                        if (paraTypes.Length > 0)
                        {
                            info.EntityName = paraTypes[0].Name;
                        }
                        //如果类型是例如此代码可为空，返回int部分(底层类型)。如果只需要将对象转换为特定类型，则可以使用System.Convert.ChangeType方法。
                    }
                    else
                    {
                        //UserControl 都是这个类型
                        if (type.BaseType.FullName == "RUINORERP.UI.BaseForm.BaseList")
                        {
                            //这个实例化过程 到UCUnitList时居然会执行里面的Query方法 导致 出错。只是会在init菜单时
                            var RelatedForms = Startup.GetFromFacByName<UserControl>(type.Name);
                            //var menu=Startup.GetFromFac<UI.BI.UCLocationTypeList>();
                            //获取关联实体名
                        }
                        //if (type.BaseType.Name == "BaseListWithTree")
                        //{
                        //    info.EntityName = typeof(tb_ProdCategories).Name;
                        //}
                        // Console.WriteLine($"注入：{attribute.FormType.Namespace}.{attribute.FormType.Name},{attribute.Describe}");
                    }

                    //特性标记
                    if (!string.IsNullOrEmpty(info.MenuPath))
                    {
                        infolist.Add(info);
                    }

                }
            }


            return infolist;
        }

        public static List<MenuAttrAssemblyInfo> RegisterFormNew()
        {
            return RegisterForm("");
        }
        #endregion 注入窗体-结束



        #region 配置持久化


        /// <summary>
        /// 持久化只能这个格式，所以思路 只保存name->enname bool
        /// </summary>
        /// <param name="qs"></param>
        public static void SaveColumnsList(SerializableDictionary<string, bool> qs, string XmlFileName)
        {
            if (string.IsNullOrEmpty(XmlFileName))
            {

            }

            string rs = string.Empty;
            rs = Serializer(typeof(SerializableDictionary<string, bool>), qs);

            string PathwithFileName = System.IO.Path.Combine(Application.StartupPath + "\\ColumnsConfig", XmlFileName.ToString());

            System.IO.FileInfo fi = new FileInfo(PathwithFileName);
            //判断目录是否存在
            if (!System.IO.Directory.Exists(fi.Directory.FullName))
            {
                System.IO.Directory.CreateDirectory(fi.Directory.FullName);
            }
            if (!System.IO.File.Exists(PathwithFileName))
            {
                System.IO.FileStream f = System.IO.File.Open(PathwithFileName, FileMode.CreateNew, FileAccess.ReadWrite);
                f.Close();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(PathwithFileName, false);
            f2.Write(rs);
            f2.Close();
            f2.Dispose();
        }




        public static SerializableDictionary<string, bool> LoadColumnsList(string XmlFileName)
        {
            string PickConfigPath = string.Empty;
            string filepath = System.IO.Path.Combine(Application.StartupPath + "\\ColumnsConfig", XmlFileName.ToString());
            //判断目录是否存在
            if (!System.IO.Directory.Exists(Application.StartupPath + "\\ColumnsConfig"))
            {
                System.IO.Directory.CreateDirectory(Application.StartupPath + "\\ColumnsConfig");
            }
            string s = "";
            if (System.IO.File.Exists(filepath))
            {

                if (!System.IO.File.Exists(filepath))
                    s = "不存在相应的目录";
                else
                {
                    StreamReader f2 = new StreamReader(filepath);
                    s = f2.ReadToEnd();
                    f2.Close();
                    f2.Dispose();
                }
            }
            SerializableDictionary<string, bool> qs = new SerializableDictionary<string, bool>();
            qs = Deserialize(typeof(SerializableDictionary<string, bool>), s) as SerializableDictionary<string, bool>;
            if (qs == null)
            {
                qs = new SerializableDictionary<string, bool>();
            }
            return qs;
        }
        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }

        #endregion

        #endregion

        public static string[] Split(this string source, int count)
        {
            List<string> list = new List<string>();
            string temp = string.Empty;
            if (source.Length % count != 0)
                source = source.PadRight(source.Length + (count - source.Length % count));
            for (int i = 0; i < source.Length; i = i + count)
            {
                for (int j = 0; j < count; j++)
                {
                    temp += source[i + j];
                }
                list.Add(temp);
                temp = string.Empty;
            }
            return list.ToArray();
        }


    }
}
