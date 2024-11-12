using Netron.GraphLib;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Processor;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TransInstruction;

namespace RUINORERP.UI.Common
{
    public static class UIBizSrvice
    {

        #region 从缓存中取产品显示数据

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

                //如果有默认值，则更新其它的为否
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
                    //排除自己后其它全默认为否
                    payeeInfos = payeeInfos.Where(c => c.PayeeInfoID != payeeInfo.PayeeInfoID).ToList();
                    payeeInfos.ForEach(c => c.IsDefault = false);
                    await MainForm.Instance.AppContext.Db.Updateable<tb_FM_PayeeInfo>(payeeInfos).UpdateColumns(it => new { it.IsDefault }).ExecuteCommandAsync();
                }

            }
            #endregion
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
                if (rslist == null)
                {
                    OriginalData odforCache = ActionForClient.请求发送缓存(tableName);
                    byte[] buffer = TransInstruction.CryptoProtocol.EncryptClientPackToServer(odforCache);
                    MainForm.Instance.ecs.client.Send(buffer);
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
                    if (rslist == null)
                    {
                        OriginalData odforCache = ActionForClient.请求发送缓存(item.Value);
                        byte[] buffer = TransInstruction.CryptoProtocol.EncryptClientPackToServer(odforCache);
                        MainForm.Instance.ecs.client.Send(buffer);
                    }
                }
            }

        }


    }

}

