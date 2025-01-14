using FastReport.DevComponents.DotNetBar.Controls;
using FastReport.Table;
using Netron.GraphLib;
using Netron.Neon.HtmlHelp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.TransModel;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.SuperSocketClient;
using RUINORERP.UI.UControls;
using RUINORERP.UI.UserPersonalized;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Windows.Forms;
using System.Windows.Markup;
using TransInstruction;

namespace RUINORERP.UI.Common
{
    public static class UIBizSrvice
    {
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

            /*
            List<ColumnDisplayController> ColumnDisplays = new List<ColumnDisplayController>();

            if (QueryConditionFilter != null)
            {
                foreach (var item in QueryConditionFilter.QueryFields)
                {
                    ColumnDisplayController col = new ColumnDisplayController();
                    col.ColName = item.FieldName;
                    col.ColDisplayText = item.Caption;
                    col.ColDisplayIndex = item.DisplayIndex;
                    col.Visible = true;
                    ColumnDisplays.Add(col);
                }
            }

            var cols = from ColumnDisplayController col in ColumnDisplays
                       orderby col.ColDisplayIndex
                       select col;

            set.Conditions = cols.ToList();
            */

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
        /// <returns></returns>
        public static async Task SetGridViewAsync(Type GridSourceType, NewSumDataGridView dataGridView, tb_MenuInfo CurMenuInfo, bool ShowSettingForm = false, bool SaveGridSetting = false)
        {

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
            tb_UIGridSetting GridSetting = menuPersonalization.tb_UIGridSettings.FirstOrDefault(c => c.GridKeyName == GridSourceType.Name && c.UIMenuPID == menuPersonalization.UIMenuPID);
            if (GridSetting == null)
            {
                GridSetting = new tb_UIGridSetting();
                GridSetting.GridKeyName = GridSourceType.Name;
                GridSetting.GridType = dataGridView.GetType().Name;
                GridSetting.UIMenuPID = menuPersonalization.UIMenuPID;
                menuPersonalization.tb_UIGridSettings.Add(GridSetting);
            }
            List<ColumnDisplayController> originalColumnDisplays = new List<ColumnDisplayController>();
            //如果数据有则加载，无则加载默认的
            if (!string.IsNullOrEmpty(GridSetting.ColsSetting))
            {
                object objList = JsonConvert.DeserializeObject(GridSetting.ColsSetting);
                if (objList != null && objList.GetType().Name == "JArray")//(Newtonsoft.Json.Linq.JArray))
                {
                    var jsonlist = objList as Newtonsoft.Json.Linq.JArray;
                    originalColumnDisplays = jsonlist.ToObject<List<ColumnDisplayController>>();
                }
            }
            else
            {
                //找到最原始的数据来自于硬编码
                originalColumnDisplays = UIHelper.GetColumnDisplayList(GridSourceType);

                //newSumDataGridViewMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
                //newSumDataGridViewMaster.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
                //不能设置上面这两个属性。因为设置了将不能自动调整宽度。这里计算一下按标题给个差不多的

                // 获取Graphics对象
                Graphics graphics = dataGridView.CreateGraphics();
                originalColumnDisplays.ForEach(c =>
                {
                    c.GridKeyName = GridSourceType.Name;
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
                dataGridView.ColumnDisplays = new List<ColumnDisplayController>();
                foreach (DataGridViewColumn dc in dataGridView.Columns)
                {
                    ColumnDisplayController cdc = new ColumnDisplayController();
                    cdc.GridKeyName = GridSourceType.Name;
                    cdc.ColDisplayText = dc.HeaderText;
                    cdc.ColDisplayIndex = dc.DisplayIndex;
                    cdc.ColWidth = dc.Width;
                    cdc.ColEncryptedName = dc.Name;
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
                if (!dataGridView.ColumnDisplays.Any(ec => ec.ColName == oldCol.ColName && ec.GridKeyName == GridSourceType.Name))
                {
                    // 如果不存在 
                    dataGridView.ColumnDisplays.Add(oldCol);
                }
                else
                {
                    //更新一下标题
                    var colset = dataGridView.ColumnDisplays.FirstOrDefault(ec => ec.ColName == oldCol.ColName && ec.GridKeyName == GridSourceType.Name);
                    colset = oldCol;
                }
            }

            if (SaveGridSetting)
            {
                SaveGridSettingData(CurMenuInfo, dataGridView, GridSourceType);
                return;
            }

            if (ShowSettingForm)
            {
                frmGridViewColSetting set = new frmGridViewColSetting();
                set.dataGridView = dataGridView;
                set.gridviewType = GridSourceType;
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



        public static async void SaveGridSettingData(tb_MenuInfo CurMenuInfo, NewSumDataGridView dataGridView, Type datasourceType = null)
        {
            tb_UserPersonalized userPersonalized = MainForm.Instance.AppContext.CurrentUser_Role_Personalized;
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

            tb_UIGridSetting GridSetting = menuPersonalization.tb_UIGridSettings.FirstOrDefault(c => c.GridKeyName == datasourceType.Name && c.UIMenuPID == menuPersonalization.UIMenuPID);
            if (GridSetting == null)
            {
                GridSetting = new tb_UIGridSetting();
                GridSetting.GridKeyName = datasourceType.Name;
                GridSetting.GridType = dataGridView.GetType().Name;
                GridSetting.UIMenuPID = menuPersonalization.UIMenuPID;
                menuPersonalization.tb_UIGridSettings.Add(GridSetting);
            }
            List<ColumnDisplayController> originalColumnDisplays = new List<ColumnDisplayController>();
            //如果数据有则加载，无则加载默认的
            if (!string.IsNullOrEmpty(GridSetting.ColsSetting))
            {
                object objList = JsonConvert.DeserializeObject(GridSetting.ColsSetting);
                if (objList != null && objList.GetType().Name == "JArray")//(Newtonsoft.Json.Linq.JArray))
                {
                    var jsonlist = objList as Newtonsoft.Json.Linq.JArray;
                    originalColumnDisplays = jsonlist.ToObject<List<ColumnDisplayController>>();
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
                    c.GridKeyName = datasourceType.Name;
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
                dataGridView.ColumnDisplays = new List<ColumnDisplayController>();
                foreach (DataGridViewColumn dc in dataGridView.Columns)
                {
                    ColumnDisplayController cdc = new ColumnDisplayController();
                    cdc.GridKeyName = datasourceType.Name;
                    cdc.ColDisplayText = dc.HeaderText;
                    cdc.ColDisplayIndex = dc.DisplayIndex;
                    cdc.ColWidth = dc.Width;
                    cdc.ColEncryptedName = dc.Name;
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
                if (!dataGridView.ColumnDisplays.Any(ec => ec.ColName == oldCol.ColName && ec.GridKeyName == datasourceType.Name))
                {
                    // 如果不存在 
                    dataGridView.ColumnDisplays.Add(oldCol);
                }
                else
                {
                    //更新
                    var colset = dataGridView.ColumnDisplays.FirstOrDefault(ec => ec.ColName == oldCol.ColName && ec.GridKeyName == datasourceType.Name);
                    colset = oldCol;
                }
            }
            //发送缓存数据
            string json = JsonConvert.SerializeObject(dataGridView.ColumnDisplays,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore // 或 ReferenceLoopHandling.Serialize
               });
            GridSetting.ColsSetting = json;

            if (GridSetting.UIGID == 0)
            {
                await MainForm.Instance.AppContext.Db.Insertable(GridSetting).ExecuteReturnSnowflakeIdAsync();
            }
            else
            {
                await MainForm.Instance.AppContext.Db.Updateable(GridSetting).ExecuteCommandAsync();
            }
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
                            tb_ProductType view_Prod = new tb_ProductType();
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
                    }
                }

                //如果有默认值，则更新其他的为否
                if (true == payeeInfo.IsDefault)
                {
                    //TODO等待完善
                    List<tb_FM_PayeeInfo> payeeInfos = new List<tb_FM_PayeeInfo>();
                    if (payeeInfo.Employee_ID.HasValue)
                    {
                        payeeInfos = await ctrPayeeInfo.BaseQueryByWhereAsync(c => c.Employee_ID == payeeInfo.Employee_ID.Value);
                    }
                    if (payeeInfo.CustomerVendor_ID.HasValue)
                    {
                        payeeInfos = await ctrPayeeInfo.BaseQueryByWhereAsync(c => c.CustomerVendor_ID == payeeInfo.CustomerVendor_ID.Value);
                    }
                    //排除自己后其他全默认为否
                    payeeInfos = payeeInfos.Where(c => c.PayeeInfoID != payeeInfo.PayeeInfoID).ToList();
                    payeeInfos.ForEach(c => c.IsDefault = false);
                    await MainForm.Instance.AppContext.Db.Updateable<tb_FM_PayeeInfo>(payeeInfos).UpdateColumns(it => new { it.IsDefault }).ExecuteCommandAsync();
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


    }

}

