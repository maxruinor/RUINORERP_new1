using RUINORERP.Model.Base;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    /// <summary>
    /// 销售订单扩展类 - 提供便捷的图片访问方法
    /// </summary>
    public partial class tb_SaleOrder
    {
        #region 高频图片字段（直接存储在业务表）

        /// <summary>
        /// 凭证图片路径（直接存储）
        /// 此字段为高频必填单图，直接存储在业务表中
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Description("凭证图片路径")]
        public string VoucherImagePath => VoucherImage;

        #endregion

        #region 低频图片字段（通过关联表）

        /// <summary>
        /// 证据图片列表（关联表，延迟加载）
        /// 用于存储订单相关的多张证据图片
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Description("证据图片列表")]
        public List<tb_FS_FileStorageInfo> EvidenceImages { get; set; }

        /// <summary>
        /// 结案图片列表（关联表，延迟加载）
        /// 用于存储订单结案时的图片
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Description("结案图片列表")]
        public List<tb_FS_FileStorageInfo> CloseImages { get; set; }

        /// <summary>
        /// 备注图片列表（关联表，延迟加载）
        /// 用于存储订单相关的备注图片
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Description("备注图片列表")]
        public List<tb_FS_FileStorageInfo> RemarkImages { get; set; }

        #endregion

        #region 便捷访问方法

        /// <summary>
        /// 获取所有图片的统一字典
        /// Key: 图片类型, Value: 图片列表
        /// </summary>
        /// <returns>图片字典</returns>
        [SugarColumn(IsIgnore = true)]
        public Dictionary<string, List<tb_FS_FileStorageInfo>> AllImages
        {
            get
            {
                var dict = new Dictionary<string, List<tb_FS_FileStorageInfo>>();

                // 添加关联表图片
                if (EvidenceImages != null && EvidenceImages.Any())
                    dict["Evidence"] = EvidenceImages;

                if (CloseImages != null && CloseImages.Any())
                    dict["Close"] = CloseImages;

                if (RemarkImages != null && RemarkImages.Any())
                    dict["Remark"] = RemarkImages;

                return dict;
            }
        }

        /// <summary>
        /// 获取总图片数量
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int TotalImageCount
        {
            get
            {
                int count = 0;

                if (!string.IsNullOrEmpty(VoucherImage))
                    count++;

                if (EvidenceImages != null)
                    count += EvidenceImages.Count;

                if (CloseImages != null)
                    count += CloseImages.Count;

                if (RemarkImages != null)
                    count += RemarkImages.Count;

                return count;
            }
        }

        #endregion
    }

    /// <summary>
    /// 产品扩展类 - 提供便捷的图片访问方法
    /// </summary>
    public partial class tb_Prod
    {
        #region 高频图片字段（直接存储）

        /// <summary>
        /// 产品主图路径（直接存储）
        /// 此字段为高频必填单图，直接存储在业务表中
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Description("产品主图路径")]
        public string MainImagePath => ImagesPath;

        #endregion

        #region 低频图片字段（关联表）

        /// <summary>
        /// 包装图片列表（关联表，延迟加载）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Description("包装图片列表")]
        public List<tb_FS_FileStorageInfo> PackageImages { get; set; }

        /// <summary>
        /// 质检图片列表（关联表，延迟加载）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Description("质检图片列表")]
        public List<tb_FS_FileStorageInfo> QualityImages { get; set; }

        /// <summary>
        /// 标签图片列表（关联表，延迟加载）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Description("标签图片列表")]
        public List<tb_FS_FileStorageInfo> LabelImages { get; set; }

        #endregion

        #region 便捷访问方法

        /// <summary>
        /// 获取所有图片的统一字典
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public Dictionary<string, List<tb_FS_FileStorageInfo>> AllImages
        {
            get
            {
                var dict = new Dictionary<string, List<tb_FS_FileStorageInfo>>();

                if (PackageImages != null && PackageImages.Any())
                    dict["Package"] = PackageImages;

                if (QualityImages != null && QualityImages.Any())
                    dict["Quality"] = QualityImages;

                if (LabelImages != null && LabelImages.Any())
                    dict["Label"] = LabelImages;

                return dict;
            }
        }

        /// <summary>
        /// 获取总图片数量
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int TotalImageCount
        {
            get
            {
                int count = 0;

                if (!string.IsNullOrEmpty(ImagesPath))
                    count++;

                if (PackageImages != null)
                    count += PackageImages.Count;

                if (QualityImages != null)
                    count += QualityImages.Count;

                if (LabelImages != null)
                    count += LabelImages.Count;

                return count;
            }
        }

        #endregion
    }

    /// <summary>
    /// 产品明细扩展类
    /// </summary>
    public partial class tb_ProdDetail
    {
        #region 图片字段

        /// <summary>
        /// 细节图片列表（关联表，延迟加载）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Description("细节图片列表")]
        public List<tb_FS_FileStorageInfo> DetailImages { get; set; }

        #endregion
    }

    /// <summary>
    /// 费用报销扩展类
    /// </summary>
    public partial class tb_FM_ExpenseClaim
    {
        #region 图片字段

        /// <summary>
        /// 凭证图片列表（关联表，延迟加载）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Description("凭证图片列表")]
        public List<tb_FS_FileStorageInfo> EvidenceImages { get; set; }

        /// <summary>
        /// 发票图片列表（关联表，延迟加载）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Description("发票图片列表")]
        public List<tb_FS_FileStorageInfo> InvoiceImages { get; set; }

        #endregion

        #region 便捷访问方法

        /// <summary>
        /// 获取所有图片
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<tb_FS_FileStorageInfo> AllImages
        {
            get
            {
                var allImages = new List<tb_FS_FileStorageInfo>();

                if (EvidenceImages != null)
                    allImages.AddRange(EvidenceImages);

                if (InvoiceImages != null)
                    allImages.AddRange(InvoiceImages);

                return allImages;
            }
        }

        #endregion
    }

    /// <summary>
    /// 付款记录扩展类
    /// </summary>
    public partial class tb_FM_PaymentRecord
    {
        #region 图片字段

        /// <summary>
        /// 付款凭证图片列表（关联表，延迟加载）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Description("付款凭证图片列表")]
        public List<tb_FS_FileStorageInfo> PaymentVoucherImages { get; set; }

        /// <summary>
        /// 收款凭证图片列表（关联表，延迟加载）
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Browsable(false)]
        [Description("收款凭证图片列表")]
        public List<tb_FS_FileStorageInfo> ReceiptImages { get; set; }

        #endregion
    }
}
