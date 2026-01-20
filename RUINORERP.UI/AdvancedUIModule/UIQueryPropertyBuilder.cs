using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Business.Processor;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global;
using RUINORERP.Model.Base;
using RUINORERP.UI.Common;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.AdvancedUIModule
{
    public static class UIQueryPropertyBuilder
    {

        /// <summary>
        /// 动态构建一些特性，针对不同的数据类型，比方日期等变动一个新的实体类型
        /// 注意这里构建代理类时是在原以字段后面加上Proxy,字段是_加下划线,这个在解析查询条件时会用到
        /// </summary>
        /// <param name="type">基础实体类型</param>
        /// <param name="queryFilter">查询过滤器</param>
        /// <returns>动态生成的代理类型</returns>
        public static Type AttributesBuilder_New2024(Type type, QueryFilter queryFilter)
        {
            // 使用RunAndSave模式创建动态程序集,确保VS调试器能够访问类型信息
            // 使用类型名称的哈希值作为程序集名称的一部分,避免重复创建导致调试器混乱
            string uniqueName = "RUINORERP.DynamicUI." + type.Name.GetHashCode().ToString("X");
            var dynamicAssemblyName = new System.Reflection.AssemblyName(uniqueName);
            var ab = AppDomain.CurrentDomain.DefineDynamicAssembly(dynamicAssemblyName, AssemblyBuilderAccess.RunAndSave);
            var mb = ab.DefineDynamicModule("DynamicUIModule");
            var tb = mb.DefineType(type.Name + "Proxy", System.Reflection.TypeAttributes.Public, type);
            #region 前期处理  根据指定的类型  生成对应的相关属性


            #region 查询
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
                if (queryField == null)
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
                            PropertyBuilder newBoolProp1 = UIQueryPropertyBuilder.AddProperty(tb, newBoolProName1, typeof(bool));
                            newBoolProp1.SetCustomAttribute(attrBoolBuilder1);
                        }
                        else
                        {
                            //逻辑删除 也生成可以查询的条件。和上面的一样by watson 2025-8-08
                            string newBoolProName1 = coldata.FieldName + "_Enable";
                            var attrBoolBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "是", newBoolProName1, AdvQueryProcessType.useYesOrNoToAll });
                            //动态属性要提前创建生成，后面要实体化传入控件
                            PropertyBuilder newBoolProp1 = UIQueryPropertyBuilder.AddProperty(tb, newBoolProName1, typeof(bool));
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

                        if (queryField.AdvQueryFieldType == AdvQueryProcessType.CmbMultiChoiceCanIgnore)
                        {
                            //先成一个标记可忽略的属性
                            string newCmbMultiChoiceCanIgnore = coldata.FieldName + "_CmbMultiChoiceCanIgnore";
                            var attrBuilderCmbMultiChoiceCanIgnore = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "多选可忽略", newCmbMultiChoiceCanIgnore, AdvQueryProcessType.CmbMultiChoiceCanIgnore });
                            //动态属性要提前创建生成，后面要实体化传入控件
                            PropertyBuilder newPropCmbMultiChoiceCanIgnore = UIQueryPropertyBuilder.AddProperty(tb, newCmbMultiChoiceCanIgnore, typeof(bool));
                            newPropCmbMultiChoiceCanIgnore.SetCustomAttribute(attrBuilderCmbMultiChoiceCanIgnore);

                            #region 动态属性要提前创建生成，后面要实体化传入控件

                            string newProNameMultiChoiceResults = coldata.FieldName + "_MultiChoiceResults";
                            var attrBuilderMultiChoiceResults = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "多选结果", newProNameMultiChoiceResults, AdvQueryProcessType.CmbMultiChoice });
                            //动态属性要提前创建生成，后面要实体化传入控件
                            PropertyBuilder newPropMultiChoiceResults = UIQueryPropertyBuilder.AddProperty(tb, newProNameMultiChoiceResults, typeof(List<object>));
                            newPropMultiChoiceResults.SetCustomAttribute(attrBuilderMultiChoiceResults);
                            #endregion

                            //string newSelectProName1 = coldata.FieldName + "_请选择";
                            //var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "请选择", newSelectProName1, AdvQueryProcessType.CmbMultiChoice });
                            ////动态属性要提前创建生成，后面要实体化传入控件
                            //PropertyBuilder newlikeProp1 = UIQueryPropertyBuilder.AddProperty(tb, newSelectProName1);
                            //newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                            break;
                        }

                        if (queryField.AdvQueryFieldType == AdvQueryProcessType.CmbMultiChoice)
                        {

                            #region 动态属性要提前创建生成，后面要实体化传入控件

                            string newProNameMultiChoiceResults = coldata.FieldName + "_MultiChoiceResults";
                            var attrBuilderMultiChoiceResults = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "多选结果", newProNameMultiChoiceResults, AdvQueryProcessType.CmbMultiChoice });
                            //动态属性要提前创建生成，后面要实体化传入控件
                            PropertyBuilder newPropMultiChoiceResults = UIQueryPropertyBuilder.AddProperty(tb, newProNameMultiChoiceResults, typeof(List<object>));
                            newPropMultiChoiceResults.SetCustomAttribute(attrBuilderMultiChoiceResults);
                            #endregion

                            //string newSelectProName1 = coldata.FieldName + "_请选择";
                            //var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "请选择", newSelectProName1, AdvQueryProcessType.CmbMultiChoice });
                            ////动态属性要提前创建生成，后面要实体化传入控件
                            //PropertyBuilder newlikeProp1 = UIQueryPropertyBuilder.AddProperty(tb, newSelectProName1);
                            //newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                            break;
                        }
                        if (queryField.AdvQueryFieldType == AdvQueryProcessType.defaultSelect)
                        {

                            string newSelectProName1 = coldata.FieldName + "_请选择";
                            var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "请选择", newSelectProName1, AdvQueryProcessType.defaultSelect });
                            //动态属性要提前创建生成，后面要实体化传入控件
                            PropertyBuilder newlikeProp1 = UIQueryPropertyBuilder.AddProperty(tb, newSelectProName1);
                            newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                            break;
                        }

                        break;
                        //下拉
                        if (coldata.IsFKRelationAttribute)
                        {
                            if (coldata.fKRelationAttribute.CmbMultiChoice)
                            {
                                #region 动态属性要提前创建生成，后面要实体化传入控件
                                string newProNameMultiChoiceResults = coldata.FieldName + "_MultiChoiceResults";
                                var attrBuilderMultiChoiceResults = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "多选结果", newProNameMultiChoiceResults, AdvQueryProcessType.CmbMultiChoice });
                                //动态属性要提前创建生成，后面要实体化传入控件
                                //PropertyBuilder newPropMultiChoiceResults = UIQueryPropertyBuilder.AddProperty(tb, newProNameMultiChoiceResults);

                                //这个属性 在控件里定义了一个对应的MultiChoiceResults 属性是类型是List<object>
                                PropertyBuilder newPropMultiChoiceResults = UIQueryPropertyBuilder.AddProperty(tb, newProNameMultiChoiceResults, typeof(List<object>));
                                newPropMultiChoiceResults.SetCustomAttribute(attrBuilderMultiChoiceResults);
                                #endregion

                                string newSelectProName1 = coldata.FieldName + "_请选择";
                                var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "请选择", newSelectProName1, AdvQueryProcessType.defaultSelect });
                                //动态属性要提前创建生成，后面要实体化传入控件
                                PropertyBuilder newlikeProp1 = UIQueryPropertyBuilder.AddProperty(tb, newSelectProName1);
                                newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                            }
                            else
                            {
                                string newSelectProName1 = coldata.FieldName + "_请选择";
                                var attrSelectBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "请选择", newSelectProName1, AdvQueryProcessType.defaultSelect });
                                //动态属性要提前创建生成，后面要实体化传入控件
                                PropertyBuilder newlikeProp1 = UIQueryPropertyBuilder.AddProperty(tb, newSelectProName1);
                                newlikeProp1.SetCustomAttribute(attrSelectBuilder1);
                            }

                        }
                        break;
                    case EnumDataType.Object:
                        break;
                    case EnumDataType.String:
                        //如果没有设置则默认为like。如果设置了则是开启了like就生成like属性
                        if ((!queryField.UseLike.HasValue) || (queryField.UseLike.HasValue && queryField.UseLike.Value == true))
                        {
                            string newlikeProNameString = coldata.FieldName + "_Like";
                            var attrlikeBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "like", newlikeProNameString, AdvQueryProcessType.stringLike });
                            //动态属性要提前创建生成，后面要实体化传入控件
                            PropertyBuilder newlikePropstring = UIQueryPropertyBuilder.AddProperty(tb, newlikeProNameString);
                            newlikePropstring.SetCustomAttribute(attrlikeBuilder1);
                        }

                        break;
                    case EnumDataType.DateTime:

                        string newProName1 = coldata.FieldName + "_Start";
                        string newProName2 = coldata.FieldName + "_End";
                        var attrBuilder1 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "时间起", newProName1, AdvQueryProcessType.datetimeRange });
                        var attrBuilder2 = new CustomAttributeBuilder(attrCtorInfo, new object[] { coldata.FieldName, "时间止", newProName2, AdvQueryProcessType.datetimeRange });
                        //动态属性要提前创建生成，后面要实体化传入控件
                        PropertyBuilder newProp1 = UIQueryPropertyBuilder.AddProperty(tb, newProName1, typeof(DateTime?));//起始时间是可以选空的，实际如果不可空的话，要调试到这里看什么情况
                                                                                                                          //动态属性要提前创建生成，后面要实体化传入控件
                        PropertyBuilder newProp2 = UIQueryPropertyBuilder.AddProperty(tb, newProName2, typeof(DateTime?));
                        newProp1.SetCustomAttribute(attrBuilder1);
                        newProp2.SetCustomAttribute(attrBuilder2);
                        break;
                    default:
                        break;
                }
            }
            #endregion


            #endregion
            Type newtype = tb.CreateType();
            return newtype;
        }

        /// <summary>
        /// 从来源数组中按每个行存放列的个数，获取指定列序号下的数据
        /// </summary>
        /// <param name="targetList">来源数组</param>
        /// <param name="RowOfColNum">每行存放列数</param>
        /// <param name="TargetColIndex">指定的列序号（从1开始）</param>
        /// <returns>指定列序号下的数据列表</returns>
        public static List<QueryField> GetTargetColumnData(List<QueryField> targetList, int RowOfColNum, int TargetColIndex)
        {
            List<QueryField> columnData = new List<QueryField>();
            if (targetList == null || targetList.Count == 0 || RowOfColNum <= 0 || TargetColIndex <= 0 || TargetColIndex > RowOfColNum)
            {
                return columnData; // 返回空列表
            }

            for (int i = 0; i < targetList.Count; i++)
            {
                // 计算当前数据项所在的列序号
                int currentColIndex = (i % RowOfColNum) + 1;
                if (currentColIndex == TargetColIndex)
                {
                    columnData.Add(targetList[i]);
                }
            }
            return columnData;
        }


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

        public static PropertyBuilder AddProperty(TypeBuilder tb, string MemberName, Type memberType)
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
