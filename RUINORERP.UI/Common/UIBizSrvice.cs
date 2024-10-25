using Netron.GraphLib;
using RUINORERP.Business;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    public static class UIBizSrvice
    {
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
    }
}
