using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using Fireasy.Common.Extensions;
using MathNet.Numerics.LinearAlgebra.Factorization;
using Microsoft.Extensions.Logging;
using Netron.GraphLib;
using Netron.Neon.HtmlHelp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.POIFS.Crypt.Dsig;

using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.Model.TransModel;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.SuperSocketClient;
using RUINORERP.UI.UControls;
using RUINORERP.UI.UCSourceGrid;
using RUINORERP.UI.UserPersonalized;
using SHControls.DataGrid;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Markup;
using TransInstruction;

namespace RUINORERP.UI.Common
{
    public static class UIBizSrvice
    {
        #region 录入数据预设模板

        /// <summary>
        /// 设置查询条件的个性化参数
        /// </summary>
        /// <param name="CurMenuInfo"></param>
        /// <param name="QueryConditionFilter"></param>
        /// <param name="QueryDto"></param>
        /// <returns></returns>
        public static async Task<bool> SetInputDataAsync<T>(tb_MenuInfo CurMenuInfo, T Dto)
        {
            bool SetResults = false;
            tb_UIMenuPersonalizationController<tb_UIMenuPersonalization> ctr = Startup.GetFromFac<tb_UIMenuPersonalizationController<tb_UIMenuPersonalization>>();

            //用户登陆后会有对应的角色下的个性化配置数据。如果没有则给一个默认的（登陆验证时已经实现）。
            tb_UserPersonalized userPersonalized = MainForm.Instance.AppContext.CurrentUser_Role_Personalized;
            if (userPersonalized.tb_UIMenuPersonalizations == null)
            {
                userPersonalized.tb_UIMenuPersonalizations = new();
            }

            tb_UIMenuPersonalization menuPersonalization = userPersonalized.tb_UIMenuPersonalizations.FirstOrDefault(t => t.MenuID == CurMenuInfo.MenuID && t.UserPersonalizedID == userPersonalized.UserPersonalizedID);
            if (menuPersonalization == null)
            {
                menuPersonalization = new tb_UIMenuPersonalization();
                menuPersonalization.MenuID = CurMenuInfo.MenuID;
                menuPersonalization.UserPersonalizedID = userPersonalized.UserPersonalizedID;
                menuPersonalization.QueryConditionCols = 4;//查询条件显示的列数 默认值
                userPersonalized.tb_UIMenuPersonalizations.Add(menuPersonalization);
                //QueryConditionCols UI上设置
                //MainForm.Instance.AppContext.Db.Insertable(MainForm.Instance.AppContext.CurrentUser_Role.tb_userpersonalized.tb_uimenupersonalization).ExecuteReturnSnowflakeIdAsync();
                ReturnResults<tb_UIMenuPersonalization> rs = await ctr.SaveOrUpdate(menuPersonalization);
                if (!rs.Succeeded)
                {
                    return false;
                }
            }

            frmInputDataColSetting set = new();
            set.MenuPersonSetting = menuPersonalization;
            //这里是列的控制情况 
            //但是这个是grid列的显示控制的。这里是处理查询条件的，默认值，是否显示参与查询
            //应该要实体绑定，再与查询参数生成条件时都关联起来。

            // 假设menuPersonalization.tb_UIQueryConditions已经存在
            if (menuPersonalization.tb_UIInputDataFields == null)
            {
                menuPersonalization.tb_UIInputDataFields = new List<tb_UIInputDataField>();
            }

            List<tb_UIInputDataField> InputFields = menuPersonalization.tb_UIInputDataFields;
            List<QueryField> fields = new List<QueryField>();
            List<tb_UIInputDataField> DefaultInputFields = new List<tb_UIInputDataField>();
            DefaultInputFields = GetInputDataField<T>(Dto, fields);
            DefaultInputFields.ForEach(c => c.UIMenuPID = menuPersonalization.UIMenuPID);
            if (InputFields.Count == 0)
            {
                InputFields = DefaultInputFields;
            }
            set.QueryFields = fields;
            set.InputFields = InputFields.OrderBy(c => c.Sort).ToList();
            set.TargetEntityDto = Dto;
            set.CurMenuInfo = CurMenuInfo;
            if (set.ShowDialog() == DialogResult.OK)
            {
                await MainForm.Instance.AppContext.Db.Insertable(set.InputFields.Where(c => c.PresetValueID == 0).ToList()).ExecuteReturnSnowflakeIdListAsync();
                await MainForm.Instance.AppContext.Db.Updateable(set.InputFields.Where(c => c.PresetValueID > 0).ToList()).ExecuteCommandAsync();
                if (menuPersonalization.tb_UIInputDataFields.Count == 0)
                {
                    menuPersonalization.tb_UIInputDataFields = set.InputFields;
                }
                await MainForm.Instance.AppContext.Db.Updateable(menuPersonalization).ExecuteCommandAsync();
                SetResults = true;

            }
            return SetResults;
        }

        /// <summary>
        /// 根据不同的类型返回可以设置默认值的列
        /// </summary>
        public static List<tb_UIInputDataField> GetInputDataField<T>(T Dto, List<QueryField> fields = null)
        {
            return GetInputDataField(typeof(T), fields);
        }

        public static List<tb_UIInputDataField> GetInputDataField(Type DtoType, List<QueryField> fields = null)
        {

            List<tb_UIInputDataField> cols = new List<tb_UIInputDataField>();
            foreach (PropertyInfo field in DtoType.GetProperties())
            {
                //获取指定类型的自定义特性
                object[] attrs = field.GetCustomAttributes(false);
                if (attrs.Length == 0)
                {
                    continue;
                }
                if (!field.CanWrite)
                {
                    continue;
                }
                //查询字段才可能是录入的数据
                if (attrs.FirstOrDefault(c => c.GetType().Name == typeof(AdvQueryAttribute).Name) == null)
                {
                    continue;
                }

                QueryField queryField = new QueryField();
                queryField.QueryTargetType = DtoType;
                tb_UIInputDataField col = new tb_UIInputDataField();

                //SugarColumn排最后！！这样判断的前置条件才先生效
                List<object> attrs1 = attrs.OrderBy(c => c.GetType().Name).ToList();

                foreach (var attr in attrs1)
                {
                    //用于是否为外键，是的话，编辑时生成下拉控件
                    if (attr is FKRelationAttribute)
                    {
                        FKRelationAttribute FKAttribute = attr as FKRelationAttribute;
                        //如果子表中引用的主表主键,则不显示
                        //if (typeof(T).Name.Contains(FKAttribute.FKTableName) && typeof(T).Name.Replace("Detail", "") == FKAttribute.FKTableName)
                        //{

                        //}
                        //else
                        //{

                        //}
                        queryField.fKRelationAttribute = FKAttribute;
                        queryField.FKTableName = FKAttribute.FKTableName;
                        queryField.SubQueryTargetType = Assembly.LoadFrom("RUINORERP.Model.dll").GetType("RUINORERP.Model." + FKAttribute.FKTableName);
                        queryField.SubFilter.QueryTargetType = Assembly.LoadFrom("RUINORERP.Model.dll").GetType("RUINORERP.Model." + FKAttribute.FKTableName);
                    }

                    if (attr is SubtotalResultAttribute)
                    {
                        SubtotalResultAttribute subtotalResultAttribute = attr as SubtotalResultAttribute;
                    }

                    //if (attr is SubtotalAttribute)
                    //{
                    //    SubtotalAttribute subtotalAttribute = attr as SubtotalAttribute;
                    //    col.Subtotal = true;
                    //}
                    if (attr is SummaryAttribute)
                    {
                        SummaryAttribute summaryAttribute = attr as SummaryAttribute;
                    }
                    if (attr is ToolTipAttribute)
                    {
                        ToolTipAttribute toolTipAttribute = attr as ToolTipAttribute;
                    }
                    if (attr is ReadOnlyAttribute)//图片只读
                    {
                        ReadOnlyAttribute readOnlyAttribute = attr as ReadOnlyAttribute;
                        continue;
                    }
                    if (attr is VisibleAttribute)//明细的产品ID隐藏
                    {
                        VisibleAttribute visibleAttribute = attr as VisibleAttribute;
                        continue;
                    }

                    if (attr is SugarColumn)
                    {

                        SugarColumn sugarColumn = attr as SugarColumn;
                        if (string.IsNullOrEmpty(sugarColumn.ColumnDescription))
                        {
                            continue;
                        }

                        col.Caption = sugarColumn.ColumnDescription;

                        if (sugarColumn.IsIdentity)
                        {
                            continue;
                        }
                        //明细中的主键不用显示
                        if (sugarColumn.IsPrimaryKey)
                        {
                            continue;
                        }
                        if (sugarColumn != null && sugarColumn.ColumnDataType != null)
                        {
                            switch (sugarColumn.ColumnDataType)
                            {
                                case "datetime":
                                    queryField.AdvQueryFieldType = AdvQueryProcessType.datetime;
                                    break;
                                case "money"://金额不能有预设值
                                    continue;
                                case "bit":
                                    queryField.AdvQueryFieldType = AdvQueryProcessType.YesOrNo;
                                    break;
                                case "bigint":
                                    if (!string.IsNullOrEmpty(queryField.FKTableName))
                                    {
                                        queryField.AdvQueryFieldType = AdvQueryProcessType.defaultSelect;
                                    }
                                    break;
                                case "decimal":
                                    break;
                                case "varchar":
                                    queryField.AdvQueryFieldType = AdvQueryProcessType.TextSelect;
                                    break;
                                case "image":
                                    break;
                                case "int":
                                    queryField.AdvQueryFieldType = AdvQueryProcessType.Int;
                                    break;
                                default:
                                    break;
                            }

                            if (sugarColumn.IsNullable == true)
                            {
                                col.IsVisble = false;
                            }
                        }

                        if (field.Name == "Selected")
                        {
                            continue;
                        }

                        queryField.FieldName = field.Name;
                        queryField.FieldPropertyInfo = field;

                        col.ValueType = field.PropertyType.Name;

                    }
                }
                //硬编码排除一些不能设置录入数据预设值的字段
                if (field.Name.Contains("Modified_at"))
                    continue;
                if (field.Name.Contains("Modified_by"))
                    continue;
                if (field.Name.Contains("Created_at"))
                    continue;
                if (field.Name.Contains("UpdateTime"))
                    continue;
                if (field.Name.Contains("Created_by"))
                    continue;
                if (field.Name.Contains("isdeleted"))
                    continue;
                if (field.Name.Contains("DataStatus"))
                    continue;
                if (field.Name.Contains("ApprovalOpinions"))
                    continue;
                if (field.Name.Contains("PrintStatus"))
                    continue;
                if (field.Name.Contains("ApprovalResults"))
                    continue;
                if (field.Name.Contains("ApprovalStatus"))
                    continue;
                if (field.Name.Contains("Approver_by"))
                    continue;
                if (field.Name.Contains("Approver_at"))
                    continue;
                if (field.Name.Contains("Approver_at"))
                    continue;
                if (field.Name.Contains("CloseCaseOpinions"))
                    continue;
                if (field.Name.Contains("CloseCase"))
                    continue;
                if (field.Name.ToLower().Contains("amount"))
                    continue;
                if (col.Caption == null)
                {
                    continue;
                }
                if (col.Caption.Contains("数量"))
                    continue;
                if (col.Caption.Contains("引用"))
                    continue;
                if (col.Caption.Contains("编号"))
                    continue;
                if (col.Caption.Contains("单号"))
                    continue;
                if (col.Caption.Contains("金额"))
                    continue;
                if (col.Caption.Contains("小计"))
                    continue;
                if (col.Caption.Contains("总计"))
                    continue;
                if (col.Caption.Contains("合计"))
                    continue;
                if (col.Caption.Contains("运费"))
                    continue;
                if (col.Caption.Contains("税额"))
                    continue;
                Type propertyType = null;
                if (field.PropertyType.IsGenericType && field.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = field.PropertyType.GenericTypeArguments[0];
                }
                else
                {
                    propertyType = field.PropertyType;
                }

                queryField.ColDataType = propertyType;
                col.FieldName = field.Name;
                col.BelongingObjectType = DtoType.Name;
                fields.Add(queryField);
                cols.Add(col);
            }
            return cols;

        }
        #endregion


        /// <summary>
        /// 设置查询条件的个性化参数
        /// </summary>
        /// <param name="CurMenuInfo"></param>
        /// <param name="QueryConditionFilter"></param>
        /// <param name="QueryDto"></param>
        /// <returns></returns>
        public static async Task<bool> SetQueryConditionsAsync(tb_MenuInfo CurMenuInfo, QueryFilter QueryConditionFilter, BaseEntity QueryDto)

        {
            bool SetResults = false;
            tb_UIMenuPersonalizationController<tb_UIMenuPersonalization> ctr = Startup.GetFromFac<tb_UIMenuPersonalizationController<tb_UIMenuPersonalization>>();

            //用户登陆后会有对应的角色下的个性化配置数据。如果没有则给一个默认的（登陆验证时已经实现）。
            tb_UserPersonalized userPersonalized = MainForm.Instance.AppContext.CurrentUser_Role_Personalized;
            if (userPersonalized.tb_UIMenuPersonalizations == null)
            {
                userPersonalized.tb_UIMenuPersonalizations = new();
            }

            tb_UIMenuPersonalization menuPersonalization = userPersonalized.tb_UIMenuPersonalizations.FirstOrDefault(t => t.MenuID == CurMenuInfo.MenuID && t.UserPersonalizedID == userPersonalized.UserPersonalizedID);
            if (menuPersonalization == null)
            {
                menuPersonalization = new tb_UIMenuPersonalization();
                menuPersonalization.MenuID = CurMenuInfo.MenuID;
                menuPersonalization.UserPersonalizedID = userPersonalized.UserPersonalizedID;
                menuPersonalization.QueryConditionCols = 4;//查询条件显示的列数 默认值
                userPersonalized.tb_UIMenuPersonalizations.Add(menuPersonalization);
                //QueryConditionCols UI上设置
                //MainForm.Instance.AppContext.Db.Insertable(MainForm.Instance.AppContext.CurrentUser_Role.tb_userpersonalized.tb_uimenupersonalization).ExecuteReturnSnowflakeIdAsync();
                ReturnResults<tb_UIMenuPersonalization> rs = await ctr.SaveOrUpdate(menuPersonalization);
                if (!rs.Succeeded)
                {
                    return false;
                }
            }

            //查询条件表
            //tb_UIQueryCondition
            //MenuPersonalizedSettings();
            frmQueryConditionSetting set = new frmQueryConditionSetting();
            //为了显示传入带中文的集合
            set.Personalization = menuPersonalization;
            set.QueryShowColQty.Value = menuPersonalization.QueryConditionCols;
            //这里是列的控制情况 
            //但是这个是grid列的显示控制的。这里是处理查询条件的，默认值，是否显示参与查询
            //应该要实体绑定，再与查询参数生成条件时都关联起来。

            // 假设menuPersonalization.tb_UIQueryConditions已经存在
            if (menuPersonalization.tb_UIQueryConditions == null)
            {
                menuPersonalization.tb_UIQueryConditions = new List<tb_UIQueryCondition>();
            }
            List<tb_UIQueryCondition> existingConditions = menuPersonalization.tb_UIQueryConditions;

            //这里如果是初始化时以硬编码的过滤条件为标准生成一组条件。如果已经有了。则按数据库中与这个比较。硬编码条件为标准增量？
            List<tb_UIQueryCondition> queryConditions = new List<tb_UIQueryCondition>();
            if (QueryConditionFilter != null)
            {
                foreach (var item in QueryConditionFilter.QueryFields)
                {
                    tb_UIQueryCondition condition = new tb_UIQueryCondition();
                    condition.FieldName = item.FieldName;
                    condition.Sort = item.DisplayIndex;
                    //时间区间排最后
                    if (item.AdvQueryFieldType == AdvQueryProcessType.datetimeRange && condition.Sort == 0)
                    {
                        condition.Sort = 100;
                    }
                    condition.IsVisble = true;
                    condition.Caption = item.Caption;
                    if (item.ColDataType != null)
                    {
                        condition.ValueType = item.ColDataType.Name;
                    }
                    condition.UIMenuPID = menuPersonalization.UIMenuPID;
                    queryConditions.Add(condition);
                }
            }
            // 对queryConditions进行排序
            var sortedQueryConditions = queryConditions.OrderBy(condition => condition.Sort).ToList();

            // 检查并添加条件
            foreach (var condition in sortedQueryConditions)
            {
                // 检查existingConditions中是否已经存在相同的条件
                if (!existingConditions.Any(ec => ec.FieldName == condition.FieldName && ec.UIMenuPID == condition.UIMenuPID))
                {
                    // 如果不存在，则添加到existingConditions中
                    existingConditions.Add(condition);
                }
                else
                {
                    //更新一下标题
                    existingConditions.FirstOrDefault(ec => ec.FieldName == condition.FieldName && ec.UIMenuPID == condition.UIMenuPID).Caption = condition.Caption.Trim();
                }

            }


            // 更新set.Conditions
            set.Conditions = existingConditions.OrderBy(c => c.Sort).ToList();
            set.QueryFields = QueryConditionFilter.QueryFields;
            set.QueryDto = QueryDto;
            //var conditions = from tb_UIQueryCondition condition in queryConditions
            //                 orderby condition.Sort
            //                 select condition;
            //set.Conditions = conditions.ToList();



            if (set.ShowDialog() == DialogResult.OK)
            {
                await MainForm.Instance.AppContext.Db.Insertable(set.Conditions.Where(c => c.UIQCID == 0).ToList()).ExecuteReturnSnowflakeIdListAsync();
                await MainForm.Instance.AppContext.Db.Updateable(set.Conditions.Where(c => c.UIQCID > 0).ToList()).ExecuteCommandAsync();
                if (menuPersonalization.tb_UIQueryConditions.Count == 0)
                {
                    menuPersonalization.tb_UIQueryConditions = set.Conditions;
                }
                menuPersonalization.QueryConditionCols = set.QueryShowColQty.Value.ToInt();
                await MainForm.Instance.AppContext.Db.Updateable(menuPersonalization).ExecuteCommandAsync();
                SetResults = true;

            }
            return SetResults;
        }

        /// <summary>
        /// 设置NewSumDataGridView列的显示的个性化参数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataGridView"></param>
        /// <param name="CurMenuInfo"></param>
        /// <param name="ShowSettingForm"></param>
        /// <param name="InvisibleCols">系统硬编码不可见和权限设置的不可见</param>
        /// <param name="DefaultHideCols">系统硬编码不可见和权限设置的不可见</param>
        /// <returns></returns>
        public static async Task SetGridViewAsync(Type GridSourceType, NewSumDataGridView dataGridView, tb_MenuInfo CurMenuInfo,
            bool ShowSettingForm = false, HashSet<string> InvisibleCols = null,
            HashSet<string> DefaultHideCols = null,
            bool SaveGridSetting = false)
        {
            if (dataGridView == null)
            {
                return;
            }
            if (MainForm.Instance.AppContext.CurrentUser_Role == null && MainForm.Instance.AppContext.IsSuperUser)
            {
                dataGridView.NeedSaveColumnsXml = true;
                return;
            }
            tb_UIMenuPersonalizationController<tb_UIMenuPersonalization> ctr = Startup.GetFromFac<tb_UIMenuPersonalizationController<tb_UIMenuPersonalization>>();
            dataGridView.NeedSaveColumnsXml = false;
            //用户登陆后会有对应的角色下的个性化配置数据。如果没有则给一个默认的（登陆验证时已经实现）。
            tb_UserPersonalized userPersonalized = MainForm.Instance.AppContext.CurrentUser_Role_Personalized;
            if (userPersonalized.tb_UIMenuPersonalizations == null)
            {
                userPersonalized.tb_UIMenuPersonalizations = new();
            }

            tb_UIMenuPersonalization menuPersonalization = userPersonalized.tb_UIMenuPersonalizations.FirstOrDefault(t => t.MenuID == CurMenuInfo.MenuID && t.UserPersonalizedID == userPersonalized.UserPersonalizedID);
            if (menuPersonalization == null)
            {
                menuPersonalization = new tb_UIMenuPersonalization();
                menuPersonalization.MenuID = CurMenuInfo.MenuID;
                menuPersonalization.UserPersonalizedID = userPersonalized.UserPersonalizedID;
                menuPersonalization.QueryConditionCols = 4;//查询条件显示的列数 默认值
                userPersonalized.tb_UIMenuPersonalizations.Add(menuPersonalization);
                ReturnResults<tb_UIMenuPersonalization> rs = await ctr.SaveOrUpdate(menuPersonalization);
                if (!rs.Succeeded)
                {

                }
            }


            //这里是列的控制情况 
            if (menuPersonalization.tb_UIGridSettings == null)
            {
                menuPersonalization.tb_UIGridSettings = new List<tb_UIGridSetting>();
            }
            tb_UIGridSetting GridSetting = menuPersonalization.tb_UIGridSettings.FirstOrDefault(c => c.GridKeyName == GridSourceType.Name
            && c.UIMenuPID == menuPersonalization.UIMenuPID
            && c.GridType == dataGridView.GetType().Name
            );
            if (GridSetting == null)
            {
                GridSetting = new();
                GridSetting.GridKeyName = GridSourceType.Name;
                GridSetting.GridType = dataGridView.GetType().Name;
                GridSetting.UIMenuPID = menuPersonalization.UIMenuPID;
                menuPersonalization.tb_UIGridSettings.Add(GridSetting);
            }
            List<ColDisplayController> originalColumnDisplays = new List<ColDisplayController>();
            //如果数据有则加载，无则加载默认的

            bool hasValidSettings = !string.IsNullOrEmpty(GridSetting.ColsSetting);

            if (hasValidSettings)
            {
                try
                {
                    object objList = JsonConvert.DeserializeObject(GridSetting.ColsSetting);
                    if (objList != null && objList.GetType().Name == "JArray")//(Newtonsoft.Json.Linq.JArray))
                    {
                        var jsonlist = objList as Newtonsoft.Json.Linq.JArray;
                        originalColumnDisplays = jsonlist.ToObject<List<ColDisplayController>>();
                        // 查找ColName属性值相同的元素
                        //var hasDuplicates = originalColumnDisplays
                        //    .GroupBy(item => item.CompositeKey)
                        //    .Where(group => group.Count() > 1)
                        //    //.Select(g => g.Skip(1))//排除掉第一个元素，这个是第一个重复的元素，要保留
                        //    .SelectMany(group => group);

                        if (originalColumnDisplays.Count == 0)
                        {
                            hasValidSettings = false;
                        }

                        // 检查是否有重复项
                        bool hasDuplicates = originalColumnDisplays
                            .GroupBy(item => item.CompositeKey)
                            .Any(group => group.Count() > 1);

                        if (hasDuplicates)
                        {
                            hasValidSettings = false;
                        }
                    }
                    else
                    {
                        hasValidSettings = false;
                    }

                }
                catch (Exception ex)
                {
                    // 处理JSON解析异常
                    MainForm.Instance.logger?.LogError("解析列设置时出错: " + ex.Message);
                    hasValidSettings = false;
                }
            }

            // 如果设置无效，则加载默认设置
            if (!hasValidSettings)
            {
                //找到最原始的数据来自于硬编码
                originalColumnDisplays = UIHelper.GetColumnDisplayList(GridSourceType);

                // 获取Graphics对象
                using (Graphics graphics = dataGridView.CreateGraphics())
                {
                    originalColumnDisplays.ForEach(c =>
                    {
                        // 计算文本宽度
                        float textWidth = UITools.CalculateTextWidth(c.ColDisplayText, dataGridView.Font, graphics);
                        c.ColWidth = (int)textWidth + 10; // 加上一些额外的空间
                        if (c.ColWidth < 100)
                        {
                            c.ColWidth = 100;
                        }
                    });
                }

                // 获取Graphics对象
                //Graphics graphics = dataGridView.CreateGraphics();
                //originalColumnDisplays.ForEach(c =>
                //{
                //    // 计算文本宽度
                //    float textWidth = UITools.CalculateTextWidth(c.ColDisplayText, dataGridView.Font, graphics);
                //    c.ColWidth = (int)textWidth + 10; // 加上一些额外的空间
                //    if (c.ColWidth < 100)
                //    {
                //        c.ColWidth = 100;
                //    }
                //});

                // 第一次设置为默认隐藏，然后用户可以设置是否显示
                if (DefaultHideCols != null)
                {
                    originalColumnDisplays.ForEach(c =>
                    {
                        // 权限设置隐藏的
                        if (DefaultHideCols.Any(ic => c.ColName.Equals(ic)))
                        {
                            c.Visible = false;
                            c.Disable = false;
                        }
                    });
                }



            }

            if (dataGridView.ColumnDisplays == null)
            {
                dataGridView.ColumnDisplays = new List<ColDisplayController>();
                foreach (DataGridViewColumn dc in dataGridView.Columns)
                {
                    ColDisplayController cdc = new ColDisplayController();
                    cdc.ColDisplayText = dc.HeaderText;
                    cdc.ColDisplayIndex = dc.DisplayIndex;
                    cdc.ColWidth = dc.Width;
                    cdc.ColName = dc.Name;
                    cdc.IsFixed = dc.Frozen;
                    cdc.Visible = dc.Visible;
                    cdc.DataPropertyName = dc.DataPropertyName;
                    originalColumnDisplays.Add(cdc);
                }
            }

            //系统指定不显示的
            if (InvisibleCols != null)
            {
                //不管什么情况都处理系统和权限的限制列显示
                originalColumnDisplays.ForEach(c =>
                {

                    if (InvisibleCols.Any(ic => c.ColName.Equals(ic)))
                    {
                        c.Visible = false;
                        c.Disable = true;
                    }
                });
            }

            // 检查并添加条件
            foreach (var oldCol in originalColumnDisplays)
            {
                // 检查existingConditions中是否已经存在相同的条件
                int index = dataGridView.ColumnDisplays.FindIndex(ec => ec.ColName == oldCol.ColName);
                if (index == -1)
                {
                    // 如果不存在 
                    dataGridView.ColumnDisplays.Add(oldCol);
                }
                else
                {
                    // 更新现有列设置
                    dataGridView.ColumnDisplays[index] = oldCol;
                }

                //// 检查existingConditions中是否已经存在相同的条件
                //if (!dataGridView.ColumnDisplays.Any(ec => ec.ColName == oldCol.ColName))
                //{
                //    // 如果不存在 
                //    dataGridView.ColumnDisplays.Add(oldCol);
                //}
                //else
                //{
                //    //更新一下标题
                //    var colset = dataGridView.ColumnDisplays.FirstOrDefault(ec => ec.ColName == oldCol.ColName);
                //    colset = oldCol;
                //}
            }

            if (SaveGridSetting)
            {
                SaveGridSettingData(CurMenuInfo, dataGridView, GridSourceType);
                return;
            }

            if (ShowSettingForm)
            {
                frmGridViewColSetting set = new frmGridViewColSetting();
                set.ConfiguredGrid = dataGridView;
                set.InvisibleCols = InvisibleCols;
                set.gridviewType = GridSourceType;
                set.CurMenuInfo = CurMenuInfo;
                set.GridSetting = GridSetting;
                set.ColumnDisplays = dataGridView.ColumnDisplays;
                if (set.ShowDialog() == DialogResult.OK)
                {
                    SaveGridSettingData(CurMenuInfo, dataGridView, GridSourceType);
                }

            }

            dataGridView.AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode)GridSetting.ColumnsMode;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            dataGridView.BindColumnStyle();
        }


        /// <summary>
        /// 保存录入数据字段的预设值
        /// </summary>
        /// <param name="CurMenuInfo"></param>
        /// <param name="InputDataFields"></param>
        public static async void SaveInputDataPresetValues(tb_MenuInfo CurMenuInfo,
            List<tb_UIInputDataField> InputDataFields)
        {

            tb_UserPersonalized userPersonalized = MainForm.Instance.AppContext.CurrentUser_Role_Personalized;
            if (userPersonalized == null)
            {
                return;
            }
            if (userPersonalized.tb_UIMenuPersonalizations == null)
            {
                return;
            }

            tb_UIMenuPersonalization menuPersonalization = userPersonalized.tb_UIMenuPersonalizations.FirstOrDefault(t => t.MenuID == CurMenuInfo.MenuID && t.UserPersonalizedID == userPersonalized.UserPersonalizedID);
            if (menuPersonalization == null)
            {
                return;
            }
            if (menuPersonalization.tb_UIInputDataFields == null)
            {
                menuPersonalization.tb_UIInputDataFields = new List<tb_UIInputDataField>();
            }
            for (int i = 0; i < InputDataFields.Count; i++)
            {
                if (InputDataFields[i].PresetValueID > 0)
                {
                    RUINORERP.Business.BusinessHelper.Instance.EditEntity(InputDataFields[i]);
                }
                else
                {
                    RUINORERP.Business.BusinessHelper.Instance.InitEntity(InputDataFields[i]);
                }
            }
            menuPersonalization.tb_UIInputDataFields = InputDataFields;

            await MainForm.Instance.AppContext.Db.Insertable(menuPersonalization.tb_UIInputDataFields.Where(c => c.PresetValueID == 0).ToList()).ExecuteReturnSnowflakeIdListAsync();
            await MainForm.Instance.AppContext.Db.Updateable(menuPersonalization.tb_UIInputDataFields.Where(c => c.PresetValueID > 0).ToList()).ExecuteCommandAsync();
        }


        public static async void SaveGridSettingData(tb_MenuInfo CurMenuInfo, NewSumDataGridView dataGridView, Type datasourceType = null)
        {
            if (CurMenuInfo==null)
            {
                return;
            }
            if (dataGridView == null)
            {
                return;
            }
            tb_UserPersonalized userPersonalized = MainForm.Instance.AppContext.CurrentUser_Role_Personalized;
            if (userPersonalized == null)
            {
                return;
            }
            if (userPersonalized.tb_UIMenuPersonalizations == null)
            {
                return;
            }

            tb_UIMenuPersonalization menuPersonalization = userPersonalized.tb_UIMenuPersonalizations.FirstOrDefault(t => t.MenuID == CurMenuInfo.MenuID && t.UserPersonalizedID == userPersonalized.UserPersonalizedID);
            if (menuPersonalization == null)
            {
                return;
            }
            if (menuPersonalization.tb_UIGridSettings == null)
            {
                menuPersonalization.tb_UIGridSettings = new List<tb_UIGridSetting>();
            }

            tb_UIGridSetting GridSetting = menuPersonalization.tb_UIGridSettings.FirstOrDefault(c => c.GridKeyName == datasourceType.Name
            && c.GridType == dataGridView.GetType().Name
            && c.UIMenuPID == menuPersonalization.UIMenuPID);
            if (GridSetting == null)
            {
                GridSetting = new tb_UIGridSetting();
                GridSetting.GridKeyName = datasourceType.Name;
                GridSetting.GridType = dataGridView.GetType().Name;
                GridSetting.UIMenuPID = menuPersonalization.UIMenuPID;
                menuPersonalization.tb_UIGridSettings.Add(GridSetting);
            }
            List<ColDisplayController> originalColumnDisplays = new List<ColDisplayController>();
            //如果数据有则加载，无则加载默认的
            if (!string.IsNullOrEmpty(GridSetting.ColsSetting))
            {
                object objList = JsonConvert.DeserializeObject(GridSetting.ColsSetting);
                if (objList != null && objList.GetType().Name == "JArray")//(Newtonsoft.Json.Linq.JArray))
                {
                    var jsonlist = objList as Newtonsoft.Json.Linq.JArray;
                    originalColumnDisplays = jsonlist.ToObject<List<ColDisplayController>>();
                }
            }
            else
            {
                if (datasourceType == null)
                {
                    return;
                }
                //找到最原始的数据来自于硬编码
                originalColumnDisplays = UIHelper.GetColumnDisplayList(datasourceType);

                //newSumDataGridViewMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
                //newSumDataGridViewMaster.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
                //不能设置上面这两个属性。因为设置了将不能自动调整宽度。这里计算一下按标题给个差不多的

                // 获取Graphics对象
                Graphics graphics = dataGridView.CreateGraphics();
                originalColumnDisplays.ForEach(c =>
                {
                    // 计算文本宽度
                    float textWidth = UITools.CalculateTextWidth(c.ColDisplayText, dataGridView.Font, graphics);
                    c.ColWidth = (int)textWidth + 10; // 加上一些额外的空间
                    if (c.ColWidth < 100)
                    {
                        c.ColWidth = 100;
                    }
                });
            }

            if (dataGridView.ColumnDisplays == null)
            {
                dataGridView.ColumnDisplays = new List<ColDisplayController>();
                foreach (DataGridViewColumn dc in dataGridView.Columns)
                {
                    ColDisplayController cdc = new ColDisplayController();
                    cdc.ColDisplayText = dc.HeaderText;
                    cdc.ColDisplayIndex = dc.DisplayIndex;
                    cdc.ColWidth = dc.Width;
                    cdc.ColName = dc.Name;
                    cdc.IsFixed = dc.Frozen;
                    cdc.Visible = dc.Visible;
                    cdc.DataPropertyName = dc.DataPropertyName;
                    originalColumnDisplays.Add(cdc);
                }
            }

            // 检查并添加条件
            foreach (var oldCol in originalColumnDisplays)
            {
                // 检查existingConditions中是否已经存在相同的条件
                if (!dataGridView.ColumnDisplays.Any(ec => ec.ColName == oldCol.ColName))
                {
                    // 如果不存在 
                    dataGridView.ColumnDisplays.Add(oldCol);
                }
                else
                {
                    //更新
                    var colset = dataGridView.ColumnDisplays.FirstOrDefault(ec => ec.ColName == oldCol.ColName);
                    colset = oldCol;
                }
            }
            string json = JsonConvert.SerializeObject(dataGridView.ColumnDisplays,
             new JsonSerializerSettings
             {
                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
             });


            List<ColDisplayController> oldColumns = new List<ColDisplayController>();
            if (!string.IsNullOrEmpty(GridSetting.ColsSetting))
            {

                try
                {
                    oldColumns = JsonConvert.DeserializeObject<List<ColDisplayController>>(GridSetting.ColsSetting);
                }
                catch (Exception)
                {

                }
            }

            List<ColDisplayController> newColumns = JsonConvert.DeserializeObject<List<ColDisplayController>>(json);



            var result = oldColumns.CompareColumns(newColumns);

            if (GridSetting.UIGID == 0)
            {
                GridSetting.ColsSetting = json;
                RUINORERP.Business.BusinessHelper.Instance.InitEntity(GridSetting);
                await MainForm.Instance.AppContext.Db.Insertable(GridSetting).ExecuteReturnSnowflakeIdAsync();
            }
            else
            {
                // 处理比较结果
                if (result.HasDifferences)
                {
                    if (GridSetting.ColsSetting != json)
                    {
                        GridSetting.ColsSetting = json;
                        RUINORERP.Business.BusinessHelper.Instance.EditEntity(GridSetting);
                        int updatecount = await MainForm.Instance.AppContext.Db.Updateable(GridSetting).ExecuteCommandAsync();
                        if (updatecount > 0)
                        {

                        }
                    }

                }
            }
        }

        public static List<tb_UIInputDataField> InitInputDataFields(List<tb_UIInputDataField> ColumnDisplays, object Dto
         , tb_MenuInfo CurMenuInfo, List<QueryField> fields)
        {
            List<tb_UIInputDataField> allInitCols = new List<tb_UIInputDataField>();
            if (Dto != null)
            {
                allInitCols = GetInputDataField(Dto.GetType(), fields);
            }
            else
            {
                MessageBox.Show("预设值的对象不能为空。");
            }
            return allInitCols;
        }


        public static void InitDataGridViewColumnDisplays(List<ColDisplayController> ColumnDisplays,
            NewSumDataGridView dataGridView, Type GridSourceType, tb_MenuInfo CurMenuInfo, HashSet<string> InvisibleCols)
        {
            if (dataGridView == null)
            {
                return;
            }
            ColumnDisplays.Clear();
            List<ColDisplayController> allInitCols = new List<ColDisplayController>();
            allInitCols = UIHelper.GetColumnDisplayList(GridSourceType);
            //如果功能变化 新增加了列则会显示到allInitCols,这时要把多出来的也显示到ColumnDisplays
            foreach (var col in allInitCols.Except(ColumnDisplays))
            {
                if (col.IsPrimaryKey)
                {
                    col.Visible = false;
                }
                else
                {
                    col.Visible = true;
                }
                ColumnDisplays.Add(col);
            }

            if (InvisibleCols == null)
            {
                InvisibleCols = dataGridView.BizInvisibleCols;
            }




            // 获取Graphics对象
            Graphics graphics = dataGridView.CreateGraphics();
            ColumnDisplays.ForEach(c =>
            {
                //计算文本宽度
                float textWidth = UITools.CalculateTextWidth(c.ColDisplayText, dataGridView.Font, graphics);
                c.ColWidth = (int)textWidth + 10; // 加上一些额外的空间
                if (c.ColWidth < 100)
                {
                    c.ColWidth = 100;
                }

                //如果设置前有列，则按这些列重新设置一下是否可见。如果初始列为空或0则全部显示
                if (allInitCols.Any(x => x.ColName == c.ColName))
                {
                    var colset = allInitCols.FirstOrDefault(x => x.ColName == c.ColName);
                    c.Visible = colset.Visible;
                }
                if (allInitCols.Count == 0)
                {
                    c.Visible = true;
                }

            });



            //权限限制，默认值等
            ColumnDisplays.ForEach(c =>
            {
                List<tb_P4Field> P4Fields =
                CurMenuInfo.tb_P4Fields
                .Where(p => p.RoleID == MainForm.Instance.AppContext.CurrentUser_Role.RoleID
                && p.tb_fieldinfo.IsChild).ToList();

                P4Fields.ForEach(p =>
                {
                    if (GridSourceType.ToString().Contains("Detail"))
                    {
                        if (p.tb_fieldinfo.FieldName == c.ColName && p.tb_fieldinfo.IsChild)
                        {
                            if (!p.tb_fieldinfo.IsEnabled)
                            {
                                c.Disable = true;
                                return;
                            }
                            else
                            {
                                c.Disable = false;
                                if (p.tb_fieldinfo.DefaultHide)
                                {
                                    c.Visible = false;
                                }
                                else
                                {
                                    c.Visible = true;
                                }
                                //如果字段表中已经设置默认啥的 这里初始化要以默认的为标准
                            }

                        }
                    }
                    else
                    {
                        if (p.tb_fieldinfo.FieldName == c.ColName && !p.tb_fieldinfo.IsChild)
                        {
                            if (!p.tb_fieldinfo.IsEnabled)
                            {
                                c.Disable = true;
                                return;
                            }
                            else
                            {
                                c.Disable = false;
                                if (p.tb_fieldinfo.DefaultHide)
                                {
                                    c.Visible = false;
                                }
                                else
                                {
                                    c.Visible = true;
                                }
                                //如果字段表中已经设置默认啥的 这里初始化要以默认的为标准
                            }

                        }
                    }


                });
            });
            //不管什么情况都处理系统和权限的限制列显示
            //程序中 硬编码不显示的列，在这里要排掉,----要隐藏
            //ColumnDisplays = ColumnDisplays.Where(x => !InvisibleCols.Contains(x.ColName)).ToList();
            ColumnDisplays.ForEach(c =>
            {
                //系统指定不显示的
                if (InvisibleCols != null)
                {
                    if (InvisibleCols.Any(ic => c.ColName.Equals(ic)))
                    {
                        c.Visible = false;
                        c.Disable = true;
                    }

                }

            });
            dataGridView.ColumnDisplays = ColumnDisplays;
            dataGridView.BindColumnStyle();

        }



        public static T GetProdDetail<T>(long ProdDetailID) where T : class
        {
            string PKCol = BaseUIHelper.GetEntityPrimaryKey<T>();
            T prodDetail = null;

            if (BizCacheHelper.Manager.NewTableList.ContainsKey(typeof(T).Name))
            {
                var nkv = BizCacheHelper.Manager.NewTableList[typeof(T).Name];
                if (nkv.Key != null)
                {
                    object obj = BizCacheHelper.Instance.GetEntity<T>(ProdDetailID);
                    if (obj != null && obj.GetType().Name != "Object" && obj is T)
                    {
                        prodDetail = obj as T;
                    }
                    else
                    {
                        if (typeof(T).Name == "tb_ProductType")
                        {
                            tb_ProductType view_Prod = new();
                            view_Prod.Type_ID = ProdDetailID;
                            prodDetail = view_Prod as T;
                        }
                        else
                        {
                            //一个缓存 一个查询不科学。暂时没有处理。TODO:
                            //prodDetail = await MainForm.Instance.AppContext.Db.Queryable<View_ProdDetail>().Where(p => p.GetPropertyValue(PKCol).ToString().Equals(ProdDetailID.ToString())).SingleAsync();
                            View_ProdDetail view_Prod = new View_ProdDetail();
                            view_Prod.ProdDetailID = ProdDetailID;
                            prodDetail = view_Prod as T;
                        }


                    }
                }
            }
            return prodDetail;
        }


        /// <summary>
        /// 获取固定值数据字典
        /// 目前是指一些枚举值转换为文字
        /// </summary>
        /// <returns></returns>
        public static ConcurrentDictionary<string, List<KeyValuePair<object, string>>> GetFixedDataDict()
        {
            //三级 还是两级呢。  反向来 一是 KEY VALUE  然后是列名
            ConcurrentDictionary<string, List<KeyValuePair<object, string>>> _DataDictionary = new ConcurrentDictionary<string, List<KeyValuePair<object, string>>>();
            //固定值也包括枚举值,也可以将没有缓存的提前查询出来给
            _DataDictionary.TryAdd(nameof(DataStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus)));
            return _DataDictionary;

        }



        #region 从缓存中取产品显示数据
        [Obsolete]
        public static List<KeyValuePair<object, string>> GetProductList()
        {
            List<KeyValuePair<object, string>> proDetailList = new List<KeyValuePair<object, string>>();
            List<View_ProdDetail> list = new List<View_ProdDetail>();
            var cachelist = BizCacheHelper.Manager.CacheEntityList.Get(nameof(View_ProdDetail));
            if (cachelist == null)
            {
                list = MainForm.Instance.AppContext.Db.Queryable<View_ProdDetail>().ToList();
            }
            else
            {
                #region 利用缓存
                Type listType = cachelist.GetType();
                if (TypeHelper.IsGenericList(listType))
                {
                    if (listType.FullName.Contains("System.Collections.Generic.List`1[[System.Object"))
                    {
                        List<View_ProdDetail> lastOKList = new List<View_ProdDetail>();
                        var lastlist = ((IEnumerable<dynamic>)cachelist).ToList();
                        foreach (var item in lastlist)
                        {
                            lastOKList.Add(item);
                        }
                        list = lastOKList;
                    }
                    else
                    {
                        list = cachelist as List<View_ProdDetail>;
                    }
                }
                else if (TypeHelper.IsJArrayList(listType))
                {
                    List<View_ProdDetail> lastOKList = new List<View_ProdDetail>();
                    var objlist = TypeHelper.ConvertJArrayToList(typeof(View_ProdDetail), cachelist as Newtonsoft.Json.Linq.JArray);
                    var lastlist = ((IEnumerable<dynamic>)objlist).ToList();
                    foreach (var item in lastlist)
                    {
                        lastOKList.Add(item);
                    }
                    list = lastOKList;
                }

                #endregion
            }
            foreach (var item in list)
            {
                proDetailList.Add(new KeyValuePair<object, string>(item.ProdDetailID, item.CNName + item.Specifications));
            }
            return proDetailList;
        }

        #endregion

        /// <summary>
        /// 保存协作人信息
        /// </summary>
        /// <param name="ContactInfo"></param>
        public static async void SaveCRMCollaborator(tb_CRM_Collaborator ContactInfo)
        {
            BaseController<tb_CRM_Collaborator> ctrContactInfo = Startup.GetFromFacByName<BaseController<tb_CRM_Collaborator>>(typeof(tb_CRM_Collaborator).Name + "Controller");
            ReturnResults<tb_CRM_Collaborator> result = await ctrContactInfo.BaseSaveOrUpdate(ContactInfo);
            if (result.Succeeded)
            {
                ////根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                //KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                ////只处理需要缓存的表
                //if (BizCacheHelper.Manager.NewTableList.TryGetValue(typeof(tb_CRM_Collaborator).Name, out pair))
                //{
                //    //如果有更新变动就上传到服务器再分发到所有客户端
                //    OriginalData odforCache = ActionForClient.更新缓存<tb_CRM_Collaborator>(result.ReturnObject);
                //    byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                //    MainForm.Instance.ecs.client.Send(buffer);
                //}
            }
        }


        /// <summary>
        /// 保存联系人信息
        /// </summary>
        /// <param name="ContactInfo"></param>
        public static async void SaveCRMContact(tb_CRM_Contact ContactInfo)
        {
            BaseController<tb_CRM_Contact> ctrContactInfo = Startup.GetFromFacByName<BaseController<tb_CRM_Contact>>(typeof(tb_CRM_Contact).Name + "Controller");
            ReturnResults<tb_CRM_Contact> result = await ctrContactInfo.BaseSaveOrUpdate(ContactInfo);
            if (result.Succeeded)
            {
                //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                //只处理需要缓存的表
                if (BizCacheHelper.Manager.NewTableList.TryGetValue(typeof(tb_CRM_Contact).Name, out pair))
                {
                    //如果有更新变动就上传到服务器再分发到所有客户端
                    OriginalData odforCache = ActionForClient.更新缓存<tb_CRM_Contact>(result.ReturnObject);
                    byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                    MainForm.Instance.ecs.client.Send(buffer);
                }
            }
        }


        /// <summary>
        /// 保存收款人信息
        /// </summary>
        /// <param name="payeeInfo"></param>
        public static async void SavePayeeInfo(tb_FM_PayeeInfo payeeInfo)
        {
            BaseController<tb_FM_PayeeInfo> ctrPayeeInfo = Startup.GetFromFacByName<BaseController<tb_FM_PayeeInfo>>(typeof(tb_FM_PayeeInfo).Name + "Controller");
            ReturnResults<tb_FM_PayeeInfo> result = await ctrPayeeInfo.BaseSaveOrUpdate(payeeInfo);

            //保存图片   这段代码和员工添加时一样。可以重构为一个方法。
            #region 
            if (result.Succeeded && ReflectionHelper.ExistPropertyName<tb_FM_PayeeInfo>(nameof(result.ReturnObject.RowImage)) && result.ReturnObject.RowImage != null)
            {
                if (result.ReturnObject.RowImage.image != null)
                {
                    if (!result.ReturnObject.RowImage.oldhash.Equals(result.ReturnObject.RowImage.newhash, StringComparison.OrdinalIgnoreCase)
                     && result.ReturnObject.GetPropertyValue("PaymentCodeImagePath").ToString() == result.ReturnObject.RowImage.ImageFullName)
                    {
                        HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                        //如果服务器有旧文件 。可以先删除
                        if (!string.IsNullOrEmpty(result.ReturnObject.RowImage.oldhash))
                        {
                            string oldfileName = result.ReturnObject.RowImage.Dir + result.ReturnObject.RowImage.realName + "-" + result.ReturnObject.RowImage.oldhash;
                            string deleteRsult = await httpWebService.DeleteImageAsync(oldfileName, "delete123");
                            MainForm.Instance.PrintInfoLog("DeleteImage:" + deleteRsult);
                        }
                        string newfileName = result.ReturnObject.RowImage.GetUploadfileName();
                        ////上传新文件时要加后缀名
                        string uploadRsult = await httpWebService.UploadImageAsync(newfileName + ".jpg", result.ReturnObject.RowImage.ImageBytes, "upload");
                        if (uploadRsult.Contains("UploadSuccessful"))
                        {
                            //重要
                            result.ReturnObject.RowImage.ImageFullName = result.ReturnObject.RowImage.UpdateImageName(result.ReturnObject.RowImage.newhash);
                            result.ReturnObject.SetPropertyValue("PaymentCodeImagePath", result.ReturnObject.RowImage.ImageFullName);
                            await ctrPayeeInfo.BaseSaveOrUpdate(result.ReturnObject);
                            //成功后。旧文件名部分要和上传成功后新文件名部分一致。后面修改只修改新文件名部分。再对比
                            MainForm.Instance.PrintInfoLog("UploadSuccessful for base List:" + newfileName);
                        }
                        else
                        {
                            MainForm.Instance.LoginWebServer();
                        }
                    }
                }

                //如果有默认值，则更新其他的为否
                if (true == payeeInfo.IsDefault)
                {
                    //TODO等待完善
                    List<tb_FM_PayeeInfo> payeeInfos = new List<tb_FM_PayeeInfo>();
                    if (payeeInfo.Employee_ID.HasValue) //一个员工名下
                    {
                        payeeInfos = await ctrPayeeInfo.BaseQueryByWhereAsync(c => c.Employee_ID == payeeInfo.Employee_ID.Value);
                    }
                    if (payeeInfo.CustomerVendor_ID.HasValue) //一个单位名下
                    {
                        payeeInfos = await ctrPayeeInfo.BaseQueryByWhereAsync(c => c.CustomerVendor_ID == payeeInfo.CustomerVendor_ID.Value);
                    }

                    //排除自己后其他全默认为否
                    if (payeeInfos.Count > 1)
                    {
                        payeeInfos = payeeInfos.Where(c => c.PayeeInfoID != payeeInfo.PayeeInfoID).ToList(); //排除自己
                        payeeInfos.ForEach(c => c.IsDefault = false);
                        await MainForm.Instance.AppContext.Db.Updateable<tb_FM_PayeeInfo>(payeeInfos).UpdateColumns(it => new { it.IsDefault }).ExecuteCommandAsync();
                    }
                }


            }
            #endregion
            if (result.Succeeded)
            {

                //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                //只处理需要缓存的表
                if (BizCacheHelper.Manager.NewTableList.TryGetValue(typeof(tb_FM_PayeeInfo).Name, out pair))
                {
                    //如果有更新变动就上传到服务器再分发到所有客户端
                    OriginalData odforCache = ActionForClient.更新缓存<tb_FM_PayeeInfo>(result.ReturnObject);
                    byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                    MainForm.Instance.ecs.client.Send(buffer);
                }
            }
        }


        /// <summary>
        /// 保存开票信息
        /// </summary>
        /// <param name="BillingInformation"></param>
        public static async void SaveBillingInformation(tb_BillingInformation Info)
        {
            BaseController<tb_BillingInformation> ctrInfo = Startup.GetFromFacByName<BaseController<tb_BillingInformation>>(typeof(tb_BillingInformation).Name + "Controller");
            ReturnResults<tb_BillingInformation> result = await ctrInfo.BaseSaveOrUpdate(Info);

            //保存图片   这段代码和员工添加时一样。可以重构为一个方法。
            #region 
            if (result.Succeeded && ReflectionHelper.ExistPropertyName<tb_BillingInformation>(nameof(result.ReturnObject.RowImage)) && result.ReturnObject.RowImage != null)
            {
                if (result.ReturnObject.RowImage.image != null)
                {
                    if (!result.ReturnObject.RowImage.oldhash.Equals(result.ReturnObject.RowImage.newhash, StringComparison.OrdinalIgnoreCase)
                     && result.ReturnObject.GetPropertyValue("PaymentCodeImagePath").ToString() == result.ReturnObject.RowImage.ImageFullName)
                    {
                        HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                        //如果服务器有旧文件 。可以先删除
                        if (!string.IsNullOrEmpty(result.ReturnObject.RowImage.oldhash))
                        {
                            string oldfileName = result.ReturnObject.RowImage.Dir + result.ReturnObject.RowImage.realName + "-" + result.ReturnObject.RowImage.oldhash;
                            string deleteRsult = await httpWebService.DeleteImageAsync(oldfileName, "delete123");
                            MainForm.Instance.PrintInfoLog("DeleteImage:" + deleteRsult);
                        }
                        string newfileName = result.ReturnObject.RowImage.GetUploadfileName();
                        ////上传新文件时要加后缀名
                        string uploadRsult = await httpWebService.UploadImageAsync(newfileName + ".jpg", result.ReturnObject.RowImage.ImageBytes, "upload");
                        if (uploadRsult.Contains("UploadSuccessful"))
                        {
                            //重要
                            result.ReturnObject.RowImage.ImageFullName = result.ReturnObject.RowImage.UpdateImageName(result.ReturnObject.RowImage.newhash);
                            result.ReturnObject.SetPropertyValue("PaymentCodeImagePath", result.ReturnObject.RowImage.ImageFullName);
                            await ctrInfo.BaseSaveOrUpdate(result.ReturnObject);
                            //成功后。旧文件名部分要和上传成功后新文件名部分一致。后面修改只修改新文件名部分。再对比
                            MainForm.Instance.PrintInfoLog("UploadSuccessful for base List:" + newfileName);
                        }
                        else
                        {
                            MainForm.Instance.LoginWebServer();
                        }
                    }
                }

                //如果有默认值，则更新其他的为否  一个单位时 同时只能一个开票资料有效
                if (true == Info.IsActive)
                {
                    List<tb_BillingInformation> Infos = new List<tb_BillingInformation>();
                    Infos = await ctrInfo.BaseQueryByWhereAsync(c => c.CustomerVendor_ID == Info.CustomerVendor_ID);
                    if (Infos.Count > 1)
                    {
                        //一个单位时 排除自己后其他全默认为否
                        Infos = Infos.Where(c => c.CustomerVendor_ID != Info.CustomerVendor_ID).ToList();
                        Infos.ForEach(c => c.IsActive = false);
                        await MainForm.Instance.AppContext.Db.Updateable<tb_BillingInformation>(Infos).UpdateColumns(it => new { it.IsActive }).ExecuteCommandAsync();
                    }

                }


            }
            #endregion
            if (result.Succeeded)
            {

                //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                //只处理需要缓存的表
                if (BizCacheHelper.Manager.NewTableList.TryGetValue(typeof(tb_BillingInformation).Name, out pair))
                {
                    //如果有更新变动就上传到服务器再分发到所有客户端
                    OriginalData odforCache = ActionForClient.更新缓存<tb_BillingInformation>(result.ReturnObject);
                    byte[] buffer = CryptoProtocol.EncryptClientPackToServer(odforCache);
                    MainForm.Instance.ecs.client.Send(buffer);
                }
            }
        }


        public static void RequestCache<T>()
        {
            RequestCache(typeof(T).Name, typeof(T));
        }

        public static void RequestCache(Type type)
        {
            RequestCache(type.Name, type);
        }
        public static void RequestCache(string tableName, Type type = null)
        {
            //优先处理本身，比方 BOM_ID显示BOM_NO，只要传tb_BOM_S
            if (BizCacheHelper.Manager.NewTableList.ContainsKey(tableName))
            {
                //请求本身
                var rslist = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                if (NeedRequesCache(rslist, tableName) && BizCacheHelper.Instance.typeNames.Contains(tableName))
                {
                    ClientService.请求缓存(tableName);
                }
            }

            //请求关联表
            List<KeyValuePair<string, string>> kvlist = new List<KeyValuePair<string, string>>();
            if (!BizCacheHelper.Manager.FkPairTableList.TryGetValue(tableName, out kvlist))
            {
                if (kvlist == null)
                {
                    if (type == null)
                    {
                        type = Assembly.LoadFrom(Global.GlobalConstants.ModelDLL_NAME).GetType(Global.GlobalConstants.Model_NAME + "." + tableName);
                    }

                    BizCacheHelper.Manager.SetFkColList(type);
                }
            }

            //获取相关的表
            if (BizCacheHelper.Manager.FkPairTableList.TryGetValue(tableName, out kvlist))
            {
                foreach (var item in kvlist)
                {
                    var rslist = BizCacheHelper.Manager.CacheEntityList.Get(item.Value);
                    //并且要存在于缓存列表的表集合中才取。有些是没有缓存的业务单据表。不需要取缓存
                    if (NeedRequesCache(rslist, item.Value) && BizCacheHelper.Instance.typeNames.Contains(item.Value))
                    {
                        ClientService.请求缓存(item.Value);
                    }

                }
            }

        }


        //将来是不是要判断具体的行里面的数据是不是有变化。
        public static bool NeedRequesCache(object rslist, string tableName)
        {
            if (rslist == null)
            {
                return true;
            }
            CacheInfo cacheInfo = new CacheInfo();
            //对比缓存信息概率。行数变化了也要请求最新的
            if (MainForm.Instance.CacheInfoList.TryGetValue(tableName, out cacheInfo))
            {
                if (rslist.GetType().Name == "JArray")//(Newtonsoft.Json.Linq.JArray))
                {
                    var jsonlist = rslist as Newtonsoft.Json.Linq.JArray;
                    if (cacheInfo.CacheCount != jsonlist.Count)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    //强类型

                    return true;
                }
            }

            return true;
        }




        #region  单据SourceGrid列的显示控制

        /// <summary>
        /// 设置NewSumDataGridView列的显示的个性化参数
        /// </summary>
        /// <param name="gridDefine"></param>
        /// <param name="CurMenuInfo"></param>
        /// <param name="InvisibleCols">系统硬编码不可见和权限设置的不可见</param>
        /// <param name="DefaultHideCols">系统硬编码不可见和权限设置的不可见</param>
        /// <returns></returns>
        public static List<SGColDisplayHandler> SetCustomSourceGridAsync(SourceGridDefine gridDefine,
            tb_MenuInfo CurMenuInfo,
            HashSet<string> InvisibleCols = null,
            HashSet<string> DefaultHideCols = null,
            bool SaveGridSetting = false,
            List<SGColDisplayHandler> SaveTargetColumnDisplays = null
            )
        {
            SourceGrid.Grid grid1 = gridDefine.grid;

            //if (MainForm.Instance.AppContext.CurrentUser_Role == null && MainForm.Instance.AppContext.IsSuperUser)
            //{
            //    grid1.NeedSaveColumnsXml = true;
            //    return;
            //}
            //dataGridView.NeedSaveColumnsXml = false;
            //用户登陆后会有对应的角色下的个性化配置数据。如果没有则给一个默认的（登陆验证时已经实现）。
            tb_UserPersonalized userPersonalized = MainForm.Instance.AppContext.CurrentUser_Role_Personalized;

            if (userPersonalized.tb_UIMenuPersonalizations == null)
            {
                userPersonalized.tb_UIMenuPersonalizations = new();
            }

            tb_UIMenuPersonalization menuPersonalization = userPersonalized.tb_UIMenuPersonalizations.FirstOrDefault(t => t.MenuID == CurMenuInfo.MenuID && t.UserPersonalizedID == userPersonalized.UserPersonalizedID);
            if (menuPersonalization == null)
            {
                menuPersonalization = new tb_UIMenuPersonalization();
                menuPersonalization.MenuID = CurMenuInfo.MenuID;
                menuPersonalization.UserPersonalizedID = userPersonalized.UserPersonalizedID;
                menuPersonalization.QueryConditionCols = 4;//查询条件显示的列数 默认值
                userPersonalized.tb_UIMenuPersonalizations.Add(menuPersonalization);
                MainForm.Instance.AppContext.Db.Insertable<tb_UIMenuPersonalization>(menuPersonalization).ExecuteReturnEntityAsync();
            }


            //这里是列的控制情况 
            if (menuPersonalization.tb_UIGridSettings == null)
            {
                menuPersonalization.tb_UIGridSettings = new List<tb_UIGridSetting>();
            }
            tb_UIGridSetting GridSetting = menuPersonalization
                .tb_UIGridSettings.FirstOrDefault(c => c.GridKeyName == gridDefine.MainBizDependencyTypeName
            && c.GridType == grid1.GetType().Name
            && c.UIMenuPID == menuPersonalization.UIMenuPID);
            if (GridSetting == null)
            {
                GridSetting = new();
                GridSetting.GridKeyName = gridDefine.MainBizDependencyTypeName;
                GridSetting.GridType = grid1.GetType().Name;
                GridSetting.UIMenuPID = menuPersonalization.UIMenuPID;
                menuPersonalization.tb_UIGridSettings.Add(GridSetting);
            }
            List<SGColDisplayHandler> originalColumnDisplays = new List<SGColDisplayHandler>();
            //如果数据有则加载，无则加载默认的
            // 如果数据有则加载，无则加载默认的
            bool hasValidSettings = !string.IsNullOrEmpty(GridSetting.ColsSetting);

            if (hasValidSettings)
            {
                try
                {

                    object objList = JsonConvert.DeserializeObject(GridSetting.ColsSetting);
                    if (objList != null && objList.GetType().Name == "JArray")//(Newtonsoft.Json.Linq.JArray))
                    {
                        var jsonlist = objList as Newtonsoft.Json.Linq.JArray;
                        originalColumnDisplays = jsonlist.ToObject<List<SGColDisplayHandler>>();
                        //如果缓存设置中的唯一标识为空，则重新生成唯一标识
                        originalColumnDisplays.ForEach(c =>
                        {
                            if (c.UniqueId == null)
                            {
                                c.UniqueId = Guid.NewGuid().ToString();
                            }
                        }
                        );

                        if (originalColumnDisplays.Count == 0)
                        {
                            hasValidSettings = false;
                        }

                        // 检查是否有重复项
                        bool hasDuplicates = originalColumnDisplays
                                .GroupBy(item => item.CompositeKey)
                                .Any(group => group.Count() > 1);

                        if (hasDuplicates)
                        {
                            hasValidSettings = false;
                        }
                    }
                    else
                    {
                        hasValidSettings = false;
                    }
                }
                catch (Exception ex)
                {
                    // 处理JSON解析异常
                    MainForm.Instance.logger?.LogError("解析SG列设置时出错: " + ex.Message);
                    hasValidSettings = false;
                }
            }
            // 如果设置无效，则加载默认设置
            if (!hasValidSettings)
            {
                originalColumnDisplays = LoadInitSourceGridSetting(gridDefine, CurMenuInfo);
            }

            //不管什么情况都处理系统和权限的限制列显示
            originalColumnDisplays.ForEach(c =>
            {
                //系统指定不显示的
                if (InvisibleCols != null)
                {
                    if (InvisibleCols.Any(ic => c.ColName.Equals(ic)))
                    {
                        c.Visible = false;
                        c.Disable = true;
                    }

                }

            });

            if (SaveGridSetting)
            {
                // 如果需要保存设置，启动后台任务
                Task.Run(async () =>
                {
                    await SaveGridSettingsAsync(menuPersonalization, GridSetting, SaveTargetColumnDisplays ?? originalColumnDisplays);
                });
            }
            return originalColumnDisplays;


        }



        /// <summary>
        /// SourceGrid列的设置保存
        /// </summary>
        /// <param name="menuPersonalization"></param>
        /// <param name="gridSetting"></param>
        /// <param name="columnDisplays"></param>
        /// <returns></returns>
        private static async Task SaveGridSettingsAsync(tb_UIMenuPersonalization menuPersonalization, tb_UIGridSetting gridSetting, List<SGColDisplayHandler> columnDisplays)
        {

            // 发送缓存数据
            string json = JsonConvert.SerializeObject(columnDisplays, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            List<SGColDisplayHandler> oldColumns = new List<SGColDisplayHandler>();
            if (!string.IsNullOrEmpty(gridSetting.ColsSetting))
            {

                try
                {
                    oldColumns = JsonConvert.DeserializeObject<List<SGColDisplayHandler>>(gridSetting.ColsSetting);
                }
                catch (Exception)
                {

                }
            }

            List<SGColDisplayHandler> newColumns = JsonConvert.DeserializeObject<List<SGColDisplayHandler>>(json);


            var result = oldColumns.CompareColumns(newColumns);



            if (gridSetting.UIGID == 0)
            {
                gridSetting.ColsSetting = json;
                RUINORERP.Business.BusinessHelper.Instance.InitEntity(gridSetting);
                await MainForm.Instance.AppContext.Db.Insertable(gridSetting).ExecuteReturnSnowflakeIdAsync();
            }
            else
            {
                // 处理比较结果
                if (result.HasDifferences)
                {
                    if (gridSetting.ColsSetting != json)
                    {
                        gridSetting.ColsSetting = json;
                        RUINORERP.Business.BusinessHelper.Instance.EditEntity(gridSetting);
                        int updateCount = await MainForm.Instance.AppContext.Db.Updateable(gridSetting).ExecuteCommandAsync();
                        if (updateCount > 0)
                        {
                            // 更新成功后的逻辑（如果有）
                        }
                    }
                }

            }
        }


        public static List<SGColDisplayHandler> LoadInitSourceGridSetting(SourceGridDefine gridDefine, tb_MenuInfo CurMenuInfo)
        {
            List<SGDefineColumnItem> listCols = null;
            //找到最原始的数据来自于硬编码

            if (listCols == null)
            {
                listCols = new List<SGDefineColumnItem>();
            }
            //listCols = gridDefine.DefineColumns; 这个在调整顺序时 变为了UI上显示调整过的。不是默认
            listCols = gridDefine.InitDefineColumns;
            List<SGColDisplayHandler> originalColumnDisplays = listCols.Select(c => c.DisplayController).ToList();

            //这里是程序级的禁用排除
            originalColumnDisplays = originalColumnDisplays.Where(c => c.Disable == false).ToList();

            //不能设置上面这两个属性。因为设置了将不能自动调整宽度。这里计算一下按标题给个差不多的

            // 获取Graphics对象
            Graphics graphics = gridDefine.grid.CreateGraphics();
            originalColumnDisplays.ForEach(c =>
            {
                //实际这里都不会禁用，上面查询用这个条件过滤的
                if (c.Disable)
                {
                    c.Visible = true;
                }
                else
                {
                    c.Visible = true;
                }
                if (c.ColName == "Selected")
                {
                    //选择列默认隐藏,上面设置后，再覆盖上面的值
                    c.Visible = false;
                }

                // 计算文本宽度
                float textWidth = UITools.CalculateTextWidth(c.ColCaption, gridDefine.grid.Font, graphics);
                c.ColWidth = (int)textWidth + 10; // 加上一些额外的空间
                if (c.ColWidth < 100)
                {
                    c.ColWidth = 100;
                }
            });

            //第一次设置为默认隐藏，然后他自己可以设置是不是要显示
            originalColumnDisplays.ForEach(c =>
            {
                //权限设置隐藏的和不可用的情况
                if (CurMenuInfo != null && CurMenuInfo.tb_P4Fields != null)
                {
                    List<tb_P4Field> P4Fields =
                   CurMenuInfo.tb_P4Fields
                   .Where(p => p.RoleID == MainForm.Instance.AppContext.CurrentUser_Role.RoleID).ToList();

                    P4Fields.ForEach(p =>
                    {
                        if (p.tb_fieldinfo.FieldName == c.ColName)
                        {
                            if (!p.tb_fieldinfo.IsEnabled)
                            {
                                c.Disable = true;
                                return;
                            }
                            else
                            {
                                c.Disable = false;
                                if (p.tb_fieldinfo.DefaultHide)
                                {
                                    c.Visible = false;
                                }
                                else
                                {
                                    c.Visible = true;
                                }

                                //如果字段表中已经设置默认啥的 这里初始化要以默认的为标准
                            }

                        }

                    });
                }

            });

            //经过权限级过滤后再次排除禁用的列
            originalColumnDisplays = originalColumnDisplays.Where(c => c.Disable == false).ToList();

            return originalColumnDisplays;
        }

        #endregion



    }



}

