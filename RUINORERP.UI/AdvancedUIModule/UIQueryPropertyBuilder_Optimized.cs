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
    /// <summary>
    /// 动态查询属性构建器 - 优化版
    /// 核心功能: 根据查询实体类型动态生成代理类型，包含查询条件所需的扩展属性
    /// </summary>
    public static class UIQueryPropertyBuilder
    {
        /// <summary>
        /// 动态构建查询代理类型
        /// 代理类型继承自原始实体类型，添加查询条件相关的扩展属性
        /// </summary>
        /// <param name="baseEntityType">基础实体类型 (如 Logs)</param>
        /// <param name="queryFilter">查询过滤器，包含字段配置和查询类型</param>
        /// <returns>动态生成的代理类型 (如 LogsProxy)</returns>
        /// <exception cref="ArgumentNullException">当baseEntityType或queryFilter为null时抛出</exception>
        /// <exception cref="InvalidOperationException">当动态程序集创建失败时抛出</exception>
        public static Type CreateQueryProxyType(Type baseEntityType, QueryFilter queryFilter)
        {
            if (baseEntityType == null)
                throw new ArgumentNullException(nameof(baseEntityType));
            
            if (queryFilter == null)
                throw new ArgumentNullException(nameof(queryFilter));

            try
            {
                // 第一步: 创建动态程序集
                // 使用RunAndSave模式确保VS调试器能够访问类型信息
                // 使用类型名称的哈希值作为程序集名称的一部分，避免重复创建导致调试器混乱
                string uniqueAssemblyName = $"RUINORERP.DynamicUI.{baseEntityType.Name.GetHashCode():X8}";
                var dynamicAssemblyName = new System.Reflection.AssemblyName(uniqueAssemblyName);
                var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                    dynamicAssemblyName, 
                    AssemblyBuilderAccess.RunAndSave);
                
                var moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicUIModule");

                // 第二步: 创建继承自基础类型的代理类型
                // 命名规则: {原类型名}Proxy，如 "LogsProxy"
                string proxyTypeName = $"{baseEntityType.Name}Proxy";
                var typeBuilder = moduleBuilder.DefineType(
                    proxyTypeName, 
                    TypeAttributes.Public | TypeAttributes.Class, 
                    baseEntityType);

                // 第三步: 构建属性特性构造函数信息
                var attributeCtorParams = new Type[] { typeof(string), typeof(string), typeof(string), typeof(AdvQueryProcessType) };
                var attributeCtorInfo = typeof(AdvExtQueryAttribute).GetConstructor(attributeCtorParams);

                // 第四步: 获取基础实体的字段信息
                var dtoFields = UIHelper.GetDtoFieldNameList(baseEntityType);

                // 第五步: 根据查询字段配置，动态添加扩展属性
                foreach (var dtoField in dtoFields)
                {
                    var fieldData = dtoField as BaseDtoField;
                    if (fieldData == null)
                        continue;

                    // 跳过字节数组字段
                    if (fieldData.ColDataType.Name == "Byte[]")
                        continue;

                    // 在查询过滤器中查找对应配置
                    var queryField = queryFilter.QueryFields.Find(x => x.FieldName == fieldData.FieldName);
                    if (queryField == null)
                        continue;

                    // 根据数据类型生成对应的扩展属性
                    AddQueryExtensionProperties(typeBuilder, attributeCtorInfo, fieldData, queryField);
                }

                // 第六步: 创建并返回代理类型
                var proxyType = typeBuilder.CreateType();
                return proxyType;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"创建查询代理类型失败: {baseEntityType.FullName}", ex);
            }
        }

        /// <summary>
        /// 根据字段数据类型和查询配置，添加相应的扩展属性
        /// </summary>
        /// <param name="typeBuilder">类型构建器</param>
        /// <param name="attributeCtorInfo">特性构造函数信息</param>
        /// <param name="fieldData">字段数据</param>
        /// <param name="queryField">查询字段配置</param>
        private static void AddQueryExtensionProperties(
            TypeBuilder typeBuilder, 
            ConstructorInfo attributeCtorInfo, 
            BaseDtoField fieldData, 
            QueryField queryField)
        {
            var colDataType = Enum.Parse<EnumDataType>(fieldData.ColDataType.Name);

            switch (colDataType)
            {
                case EnumDataType.Boolean:
                    AddBooleanQueryProperties(typeBuilder, attributeCtorInfo, fieldData, queryField);
                    break;

                case EnumDataType.Int16:
                case EnumDataType.UInt16:
                case EnumDataType.Int32:
                case EnumDataType.UInt32:
                case EnumDataType.Int64:
                case EnumDataType.UInt64:
                    AddIntegerQueryProperties(typeBuilder, attributeCtorInfo, fieldData, queryField);
                    break;

                case EnumDataType.DateTime:
                    AddDateTimeQueryProperties(typeBuilder, attributeCtorInfo, fieldData);
                    break;

                case EnumDataType.String:
                    AddStringQueryProperties(typeBuilder, attributeCtorInfo, fieldData, queryField);
                    break;

                default:
                    // 其他类型暂不处理
                    break;
            }
        }

        /// <summary>
        /// 添加布尔类型查询属性
        /// 生成: {FieldName}_Enable (控制是否启用查询条件)
        /// </summary>
        private static void AddBooleanQueryProperties(
            TypeBuilder typeBuilder, 
            ConstructorInfo attributeCtorInfo, 
            BaseDtoField fieldData, 
            QueryField queryField)
        {
            string enablePropertyName = $"{fieldData.FieldName}_Enable";
            
            var propertyBuilder = AddProperty(typeBuilder, enablePropertyName, typeof(bool));
            
            var attributeBuilder = new CustomAttributeBuilder(attributeCtorInfo, 
                new object[] { fieldData.FieldName, "是", enablePropertyName, AdvQueryProcessType.useYesOrNoToAll });
            propertyBuilder.SetCustomAttribute(attributeBuilder);
        }

        /// <summary>
        /// 添加整数类型查询属性
        /// 根据查询类型生成: 单选/多选/多选可忽略
        /// </summary>
        private static void AddIntegerQueryProperties(
            TypeBuilder typeBuilder, 
            ConstructorInfo attributeCtorInfo, 
            BaseDtoField fieldData, 
            QueryField queryField)
        {
            switch (queryField.AdvQueryFieldType)
            {
                case AdvQueryProcessType.CmbMultiChoiceCanIgnore:
                    AddMultiChoiceIgnoreProperties(typeBuilder, attributeCtorInfo, fieldData);
                    break;

                case AdvQueryProcessType.CmbMultiChoice:
                    AddMultiChoiceProperties(typeBuilder, attributeCtorInfo, fieldData);
                    break;

                case AdvQueryProcessType.defaultSelect:
                    AddSingleSelectProperties(typeBuilder, attributeCtorInfo, fieldData);
                    break;

                case AdvQueryProcessType.None when fieldData.IsFKRelationAttribute:
                    // 如果是外键字段且未指定查询类型，默认使用单选
                    AddSingleSelectProperties(typeBuilder, attributeCtorInfo, fieldData);
                    break;
            }
        }

        /// <summary>
        /// 添加多选可忽略查询属性
        /// 生成: {FieldName}_CmbMultiChoiceCanIgnore + {FieldName}_MultiChoiceResults
        /// </summary>
        private static void AddMultiChoiceIgnoreProperties(
            TypeBuilder typeBuilder, 
            ConstructorInfo attributeCtorInfo, 
            BaseDtoField fieldData)
        {
            // 可忽略标记属性
            string ignorePropertyName = $"{fieldData.FieldName}_CmbMultiChoiceCanIgnore";
            var ignorePropertyBuilder = AddProperty(typeBuilder, ignorePropertyName, typeof(bool));
            var ignoreAttributeBuilder = new CustomAttributeBuilder(attributeCtorInfo, 
                new object[] { fieldData.FieldName, "多选可忽略", ignorePropertyName, AdvQueryProcessType.CmbMultiChoiceCanIgnore });
            ignorePropertyBuilder.SetCustomAttribute(ignoreAttributeBuilder);

            // 多选结果属性
            string resultsPropertyName = $"{fieldData.FieldName}_MultiChoiceResults";
            var resultsPropertyBuilder = AddProperty(typeBuilder, resultsPropertyName, typeof(List<object>));
            var resultsAttributeBuilder = new CustomAttributeBuilder(attributeCtorInfo, 
                new object[] { fieldData.FieldName, "多选结果", resultsPropertyName, AdvQueryProcessType.CmbMultiChoice });
            resultsPropertyBuilder.SetCustomAttribute(resultsAttributeBuilder);
        }

        /// <summary>
        /// 添加多选查询属性
        /// 生成: {FieldName}_MultiChoiceResults
        /// </summary>
        private static void AddMultiChoiceProperties(
            TypeBuilder typeBuilder, 
            ConstructorInfo attributeCtorInfo, 
            BaseDtoField fieldData)
        {
            string resultsPropertyName = $"{fieldData.FieldName}_MultiChoiceResults";
            var propertyBuilder = AddProperty(typeBuilder, resultsPropertyName, typeof(List<object>));
            var attributeBuilder = new CustomAttributeBuilder(attributeCtorInfo, 
                new object[] { fieldData.FieldName, "多选结果", resultsPropertyName, AdvQueryProcessType.CmbMultiChoice });
            propertyBuilder.SetCustomAttribute(attributeBuilder);
        }

        /// <summary>
        /// 添加单选查询属性
        /// 生成: {FieldName}_请选择
        /// </summary>
        private static void AddSingleSelectProperties(
            TypeBuilder typeBuilder, 
            ConstructorInfo attributeCtorInfo, 
            BaseDtoField fieldData)
        {
            string selectPropertyName = $"{fieldData.FieldName}_请选择";
            var propertyBuilder = AddProperty(typeBuilder, selectPropertyName, typeof(long));
            var attributeBuilder = new CustomAttributeBuilder(attributeCtorInfo, 
                new object[] { fieldData.FieldName, "请选择", selectPropertyName, AdvQueryProcessType.defaultSelect });
            propertyBuilder.SetCustomAttribute(attributeBuilder);
        }

        /// <summary>
        /// 添加日期时间查询属性
        /// 生成: {FieldName}_Start + {FieldName}_End
        /// </summary>
        private static void AddDateTimeQueryProperties(
            TypeBuilder typeBuilder, 
            ConstructorInfo attributeCtorInfo, 
            BaseDtoField fieldData)
        {
            // 起始时间属性
            string startPropertyName = $"{fieldData.FieldName}_Start";
            var startPropertyBuilder = AddProperty(typeBuilder, startPropertyName, typeof(DateTime?));
            var startAttributeBuilder = new CustomAttributeBuilder(attributeCtorInfo, 
                new object[] { fieldData.FieldName, "时间起", startPropertyName, AdvQueryProcessType.datetimeRange });
            startPropertyBuilder.SetCustomAttribute(startAttributeBuilder);

            // 结束时间属性
            string endPropertyName = $"{fieldData.FieldName}_End";
            var endPropertyBuilder = AddProperty(typeBuilder, endPropertyName, typeof(DateTime?));
            var endAttributeBuilder = new CustomAttributeBuilder(attributeCtorInfo, 
                new object[] { fieldData.FieldName, "时间止", endPropertyName, AdvQueryProcessType.datetimeRange });
            endPropertyBuilder.SetCustomAttribute(endAttributeBuilder);
        }

        /// <summary>
        /// 添加字符串查询属性
        /// 生成: {FieldName}_Like (如果UseLike为true)
        /// </summary>
        private static void AddStringQueryProperties(
            TypeBuilder typeBuilder, 
            ConstructorInfo attributeCtorInfo, 
            BaseDtoField fieldData, 
            QueryField queryField)
        {
            // 如果未设置或启用Like，则生成Like属性
            if (!queryField.UseLike.HasValue || queryField.UseLike.Value)
            {
                string likePropertyName = $"{fieldData.FieldName}_Like";
                var propertyBuilder = AddProperty(typeBuilder, likePropertyName, typeof(string));
                var attributeBuilder = new CustomAttributeBuilder(attributeCtorInfo, 
                    new object[] { fieldData.FieldName, "like", likePropertyName, AdvQueryProcessType.stringLike });
                propertyBuilder.SetCustomAttribute(attributeBuilder);
            }
        }

        /// <summary>
        /// 动态创建属性 (字符串类型，默认string)
        /// </summary>
        /// <param name="typeBuilder">类型构建器</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性构建器</returns>
        private static PropertyBuilder AddProperty(TypeBuilder typeBuilder, string propertyName)
        {
            return AddProperty(typeBuilder, propertyName, typeof(string));
        }

        /// <summary>
        /// 动态创建属性 (指定类型)
        /// </summary>
        /// <param name="typeBuilder">类型构建器</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyType">属性类型</param>
        /// <returns>属性构建器</returns>
        /// <exception cref="ArgumentException">当propertyName为空或空白时抛出</exception>
        private static PropertyBuilder AddProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("属性名称不能为空", nameof(propertyName));

            // 定义私有字段
            string fieldName = $"m_{propertyName}";
            var fieldBuilder = typeBuilder.DefineField(fieldName, propertyType, FieldAttributes.Private);

            // 定义公共属性
            var propertyBuilder = typeBuilder.DefineProperty(
                propertyName,
                PropertyAttributes.HasDefault,
                propertyType,
                null);

            // 定义get方法
            MethodAttributes getSetAttributes = MethodAttributes.Public | 
                MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            var getMethodBuilder = typeBuilder.DefineMethod(
                $"get_{propertyName}",
                getSetAttributes,
                propertyType,
                Type.EmptyTypes);

            var getIL = getMethodBuilder.GetILGenerator();
            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getIL.Emit(OpCodes.Ret);

            // 定义set方法
            var setMethodBuilder = typeBuilder.DefineMethod(
                $"set_{propertyName}",
                getSetAttributes,
                null,
                new Type[] { propertyType });

            var setIL = setMethodBuilder.GetILGenerator();
            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, fieldBuilder);
            setIL.Emit(OpCodes.Ret);

            // 关联get/set方法
            propertyBuilder.SetGetMethod(getMethodBuilder);
            propertyBuilder.SetSetMethod(setMethodBuilder);

            return propertyBuilder;
        }

        /// <summary>
        /// 从来源数组中按列序号获取数据
        /// 用于UI布局计算
        /// </summary>
        /// <param name="sourceList">来源列表</param>
        /// <param name="columnCount">每行列数</param>
        /// <param name="targetColumnIndex">目标列索引 (从1开始)</param>
        /// <returns>指定列的数据列表</returns>
        /// <exception cref="ArgumentOutOfRangeException">当参数无效时抛出</exception>
        public static List<QueryField> GetColumnData(
            List<QueryField> sourceList, 
            int columnCount, 
            int targetColumnIndex)
        {
            var result = new List<QueryField>();

            // 参数验证
            if (sourceList == null || sourceList.Count == 0)
                return result;

            if (columnCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(columnCount), "列数必须大于0");

            if (targetColumnIndex <= 0 || targetColumnIndex > columnCount)
                throw new ArgumentOutOfRangeException(nameof(targetColumnIndex), $"列索引必须在1-{columnCount}之间");

            // 提取指定列的数据
            for (int i = 0; i < sourceList.Count; i++)
            {
                int currentColumnIndex = (i % columnCount) + 1;
                if (currentColumnIndex == targetColumnIndex)
                {
                    result.Add(sourceList[i]);
                }
            }

            return result;
        }
    }
}
