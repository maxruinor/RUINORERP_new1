using AutoMapper;
using RUINOR.Core;
using RUINORERP.Business;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.CommService;
using RUINORERP.Business.LogicaService;
using RUINORERP.Business.Processor;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;





namespace RUINORERP.UI.ProductEAV
{

    [MenuAttrAssemblyInfo("产品管理", ModuleMenuDefine.模块定义.基础资料, ModuleMenuDefine.基础资料.产品资料)]
    public partial class UCProductList : BaseForm.BaseListGeneric<tb_Prod>
    {

        private List<tb_ProdCategories> _categorylist = new List<tb_ProdCategories>();

        public UCProductList()
        {
            InitializeComponent();
            bool isDesignMode = this.DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime;

            if (!isDesignMode)
            {
                base.EditForm = typeof(frmProductEdit);
                //显示时目前只缓存了基础数据。单据也可以考虑id显示编号。后面来实现。如果缓存优化好了
                DisplayTextResolver.Initialize(dataGridView1);
                #region 准备枚举值在列表中显示
                System.Linq.Expressions.Expression<Func<tb_Prod, int?>> expr;
                expr = (p) => p.SourceType;
                base.ColNameDataDictionary.TryAdd(expr.GetMemberInfo().Name, CommonHelper.Instance.GetKeyValuePairs(typeof(GoodsSource)));
                DisplayTextResolver.AddFixedDictionaryMappingByEnum<tb_Prod>(t => t.SourceType, typeof(GoodsSource));
                #endregion

                #region 准备枚举值在列表中显示
                System.Linq.Expressions.Expression<Func<tb_Prod, int?>> exprP;
                exprP = (p) => p.PropertyType;
                base.ColNameDataDictionary.TryAdd(exprP.GetMemberInfo().Name, CommonHelper.Instance.GetKeyValuePairs(typeof(ProductAttributeType)));

                DisplayTextResolver.AddFixedDictionaryMappingByEnum<tb_Prod>(t => t.PropertyType, typeof(ProductAttributeType));
                #endregion

                // 配置图片列显示（显示缩略图，双击可查看原图）
                DisplayTextResolver.AddColumnDisplayType<tb_Prod>(p => p.Images, ColumnDisplayTypeEnum.Image);

                dataGridView1.CustomRowNo = true;
                dataGridView1.CellPainting += dataGridView1_CellPainting;

            }



        }


        // 忽略属性配置
        // 重写忽略属性配置
        protected override IgnorePropertyConfiguration ConfigureIgnoreProperties()
        {
            return base.ConfigureIgnoreProperties()
                // 主表忽略的属性
                .Ignore<tb_Prod>(
                    e => e.ProdBaseID,
                    e => e.ProductNo,
                    e => e.ShortCode,
                    e => e.DataStatus,
                    e => e.PrimaryKeyID,
                    e => e.PropertyType,
                    e => e.tb_ProdDetails,
                    e => e.StateManager)
                // 明细表忽略的属性
                .Ignore<tb_ProdDetail>(
                    e => e.ProdDetailID,
                    e => e.ProdBaseID,
                    e => e.PrimaryKeyID,
                    e => e.SKU,
                    e => e.BarCode
                    );
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // 检查是否是行头
            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {

                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                Rectangle indexRect = e.CellBounds;
                indexRect.Inflate(-2, -2);

                TextRenderer.DrawText(e.Graphics,
                    (e.RowIndex + 1).ToString(),
                    e.CellStyle.Font,
                    indexRect,
                    e.CellStyle.ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                // 检查是否需要标记的行
                DataGridViewRow dr = dataGridView1.Rows[e.RowIndex];
                tb_Packing tb_Packing = null;
                BoxRuleBasis basis = GDIHelper.Instance.CheckForBoxSpecBasis(dr, out tb_Packing);
                // 绘制图案
                switch (basis)
                {
                    case BoxRuleBasis.Product:
                        GDIHelper.Instance.DrawPattern(e, Color.DarkGreen);
                        break;
                    case BoxRuleBasis.Attributes:
                        //DrawPattern(e, Color.DarkMagenta);
                        GDIHelper.Instance.DrawPattern(e);
                        break;
                    case BoxRuleBasis.Product | BoxRuleBasis.Attributes:
                        GDIHelper.Instance.DrawPattern(e, Color.OrangeRed);
                        break;
                    default:
                        break;
                }
                e.Handled = true;
                return;
            }
            
            // ✅ 新增: 处理Images列的智能图片预览(单属性显示主图,多属性优先显示SKU图)
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                var columnName = dataGridView1.Columns[e.ColumnIndex].Name;
                if (columnName == "Images" || columnName == "ImagesPath")
                {
                    DrawSmartProductImageThumbnail(e);
                    return;
                }
            }
        }


        /// <summary>
        /// 扩展带条件查询
        /// 因为产品相关性多，重写这个方法用高级导航查询
        /// </summary>     
        protected async override void ExtendedQuery(bool UseAutoNavQuery = false)
        {
            if (ValidationHelper.hasValidationErrors(this.Controls))
                return;
            if (QueryDtoProxy == null)
            {
                return;
            }
            dataGridView1.ReadOnly = true;

            //既然前台指定的查询哪些字段，到时可以配置。这里应该是 除软件删除外的。其他字段不需要

            int pageNum = 1;
            int pageSize = int.Parse(txtMaxRows.Text);

            List<tb_Prod> list = await ctr.BaseQueryByAdvancedNavWithConditionsAsync(true, QueryConditionFilter, QueryDtoProxy, pageNum, pageSize) as List<tb_Prod>;

            List<string> masterlist = RuinorExpressionHelper.ExpressionListToStringList(SummaryCols);
            if (masterlist.Count > 0)
            {
                dataGridView1.IsShowSumRow = true;
                dataGridView1.SumColumns = masterlist.ToArray();
            }

            list.ForEach(
                c =>
                {
                    c.tb_ProdDetails.ForEach(d => d.BeginOperation());
                    c.BeginOperation();
                }
            );

            ListDataSoure.DataSource = list.ToBindingSortCollection();//这句是否能集成到上一层生成
            dataGridView1.DataSource = ListDataSoure;

            ToolBarEnabledControl(MenuItemEnums.查询);
            
            // ✅ 查询完成后预加载可见行图片(优化体验)
            await PreloadVisibleImagesAsync();
        }


        /// <summary>
        /// 产品编辑特别，修改要保存后进行。不可以新建后就修改
        /// </summary>
        protected override async Task<object> Add()
        {
            base.toolStripButtonModify.Enabled = false;
            object obj = await base.Add();
            return obj;
        }


        IList<tb_Prod> oldList;
        tb_ProdController<tb_Prod> pctr = Startup.GetFromFac<tb_ProdController<tb_Prod>>();
        tb_ProdCategoriesController<tb_ProdCategories> mca = Startup.GetFromFac<tb_ProdCategoriesController<tb_ProdCategories>>();


        protected async override Task<bool> Delete()
        {
            bool rs = false;
            if (MessageBox.Show("系统不建议删除基本资料\r\n确定删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                tb_Prod loc = (tb_Prod)this.bindingSourceList.Current;
                if (loc.DataStatus == (int)Global.DataStatus.确认)
                {
                    MessageBox.Show("确认后的数据不能删除。");
                }
                else
                {
                    this.bindingSourceList.Remove(loc);
                    rs = await pctr.DeleteByNavAsync(loc);
                    if (rs)
                    {
                        //缓存只是显示用，所以删除后，并不影响。等待服务器的更新机制更新即可。
                    }
                }

            }
            return rs;
        }

        /// <summary>
        /// 产品比较特殊 
        /// </summary>
        public async override Task<List<tb_Prod>> Save()
        {
            List<tb_Prod> list = new List<tb_Prod>();
            tb_ProdController<tb_Prod> pctr = Startup.GetFromFac<tb_ProdController<tb_Prod>>();
            
            // ✅ 第一步: 先保存所有产品数据到数据库(获取有效的ProdDetailID)
            foreach (var item in bindingSourceList.List)
            {
                var entity = item as tb_Prod;

                switch (entity.ActionStatus)
                {
                    case ActionStatus.无操作:
                        break;
                    case ActionStatus.新增:
                    case ActionStatus.修改:
                        base.toolStripButtonSave.Enabled = false;
                        ReturnResults<tb_Prod> rr = new ReturnResults<tb_Prod>();
                        rr = await pctr.SaveOrUpdateAsync(entity);
                        if (rr.Succeeded)
                        {
                            base.toolStripButtonSave.Enabled = true;
                            ToolBarEnabledControl(MenuItemEnums.保存);
                            list.Add(rr.ReturnObject);
                            MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_Prod>("产品保存", rr.ReturnObject);
                            
                            //根据要缓存的列表集合来判断是否需要上传到服务器。让服务器分发到其他客户端
                            base._eventDrivenCacheManager.UpdateEntity<tb_Prod>(rr.ReturnObject);
                        }
                        else
                        {
                            base.toolStripButtonSave.Enabled = false;
                            MainForm.Instance.uclog.AddLog(rr.ErrorMsg, Global.UILogType.错误);
                        }
                        break;
                    case ActionStatus.删除:
                        break;
                    default:
                        break;
                }
                entity.HasChanged = false;
            }

            // ✅ 第二步: 统一处理所有产品的主图和SKU图片上传(此时所有产品和SKU都有有效的ID)
            int totalSuccessCount = 0;
            int totalFailCount = 0;
            
            foreach (var product in list)
            {
                // ✅ 处理产品主图
                if (product.HasUnsavedImageChanges)
                {
                    try
                    {
                        MainForm.Instance.uclog.AddLog($"开始处理产品 '{product.CNName}' 的主图上传...");
                        
                        // 获取文件服务
                        var fileService = Startup.GetFromFac<RUINORERP.UI.Network.Services.FileBusinessService>();
                        if (fileService == null)
                        {
                            MainForm.Instance.uclog.AddLog("FileBusinessService不可用", Global.UILogType.错误);
                            totalFailCount++;
                            continue;
                        }

                        bool hasError = false;

                        // 1. 处理删除操作
                        var deleteImages = product.GetPendingDeleteImages();
                        foreach (var delImg in deleteImages)
                        {
                            if (delImg.ExistingFileId.HasValue && delImg.ExistingFileId.Value > 0)
                            {
                                MainForm.Instance.uclog.AddLog($"  - 删除主图 FileId={delImg.ExistingFileId.Value}");
                                
                                // ✅ 调用删除接口
                                var deleteResponse = await fileService.DeleteImagesByIdsAsync(
                                    product.ProdBaseID,
                                    "tb_Prod",
                                    new List<long> { delImg.ExistingFileId.Value },
                                    physicalDelete: false
                                );
                                
                                if (deleteResponse == null || !deleteResponse.IsSuccess)
                                {
                                    hasError = true;
                                    string errorMsg = deleteResponse?.ErrorMessage ?? "删除结果为空";
                                    MainForm.Instance.uclog.AddLog($"    删除失败: {errorMsg}", Global.UILogType.错误);
                                }
                            }
                        }

                        // 2. 处理替换操作(先删后增)
                        var replaceImages = product.GetPendingReplaceImages();
                        foreach (var repImg in replaceImages)
                        {
                            if (repImg.ExistingFileId.HasValue && repImg.ImageData != null)
                            {
                                MainForm.Instance.uclog.AddLog($"  - 替换主图 FileId={repImg.ExistingFileId.Value}");
                                
                                // ✅ 先删除旧的
                                var deleteResponse = await fileService.DeleteImagesByIdsAsync(
                                    product.ProdBaseID,
                                    "tb_Prod",
                                    new List<long> { repImg.ExistingFileId.Value },
                                    physicalDelete: false
                                );
                                
                                if (deleteResponse == null || !deleteResponse.IsSuccess)
                                {
                                    hasError = true;
                                    string errorMsg = deleteResponse?.ErrorMessage ?? "删除结果为空";
                                    MainForm.Instance.uclog.AddLog($"    删除旧图失败: {errorMsg}", Global.UILogType.错误);
                                    continue;
                                }
                                
                                // ✅ 再上传新的
                                var uploadResult = await fileService.UploadImageAsync(
                                    product,
                                    repImg.FileName ?? $"image_{DateTime.Now.Ticks}.jpg",
                                    repImg.ImageData,
                                    "ImagesPath"
                                );
                                
                                if (uploadResult != null && uploadResult.IsSuccess)
                                {
                                    totalSuccessCount++;
                                    MainForm.Instance.uclog.AddLog($"    上传成功: FileId={uploadResult.FileStorageInfos?.FirstOrDefault()?.FileId}");
                                }
                                else
                                {
                                    hasError = true;
                                    string errorMsg = uploadResult?.ErrorMessage ?? "上传结果为空";
                                    MainForm.Instance.uclog.AddLog($"    上传失败: {errorMsg}", Global.UILogType.错误);
                                }
                            }
                        }

                        // 3. 处理新增操作
                        var addImages = product.GetPendingAddImages();
                        foreach (var addImg in addImages)
                        {
                            if (addImg.ImageData != null && !string.IsNullOrEmpty(addImg.FileName))
                            {
                                MainForm.Instance.uclog.AddLog($"  - 新增主图 {addImg.FileName} ({addImg.ImageData.Length} bytes)");
                                
                                // ✅ 上传图片
                                var uploadResult = await fileService.UploadImageAsync(
                                    product,
                                    addImg.FileName,
                                    addImg.ImageData,
                                    "ImagesPath"
                                );
                                
                                if (uploadResult != null && uploadResult.IsSuccess)
                                {
                                    totalSuccessCount++;
                                    MainForm.Instance.uclog.AddLog($"    上传成功: FileId={uploadResult.FileStorageInfos?.FirstOrDefault()?.FileId}");
                                }
                                else
                                {
                                    hasError = true;
                                    string errorMsg = uploadResult?.ErrorMessage ?? "上传结果为空";
                                    MainForm.Instance.uclog.AddLog($"    上传失败: {errorMsg}", Global.UILogType.错误);
                                }
                            }
                        }

                        if (!hasError)
                        {
                            // 清除待处理列表
                            product.ClearPendingImages();
                            MainForm.Instance.uclog.AddLog($"产品 '{product.CNName}' 主图处理成功");
                        }
                        else
                        {
                            totalFailCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        totalFailCount++;
                        MainForm.Instance.uclog.AddLog(
                            $"处理产品 '{product.CNName}' 主图时异常: {ex.Message}",
                            Global.UILogType.错误);
                    }
                }
            }
            
            foreach (var product in list)
            {
                if (product.tb_ProdDetails != null && product.tb_ProdDetails.Count > 0)
                {
                    foreach (var detail in product.tb_ProdDetails)
                    {
                        // ✅ 修复: 移除ProdDetailID > 0检查,因为新增SKU的PendingImages可能不为空
                        // 新增SKU在第一步保存后会获得有效ProdDetailID,此时才能上传图片
                        if (detail.HasUnsavedImageChanges)
                        {
                            try
                            {
                                MainForm.Instance.uclog.AddLog($"开始处理产品 '{product.CNName}' 的SKU '{detail.SKU}' 的图片上传...");
                                
                                // 获取文件服务
                                var fileService = Startup.GetFromFac<RUINORERP.UI.Network.Services.FileBusinessService>();
                                if (fileService == null)
                                {
                                    MainForm.Instance.uclog.AddLog("FileBusinessService不可用", Global.UILogType.错误);
                                    totalFailCount++;
                                    continue;
                                }

                                bool hasError = false;

                                // 1. 处理删除操作
                                var deleteImages = detail.GetPendingDeleteImages();
                                foreach (var delImg in deleteImages)
                                {
                                    if (delImg.ExistingFileId.HasValue && delImg.ExistingFileId.Value > 0)
                                    {
                                        MainForm.Instance.uclog.AddLog($"  - 删除图片 FileId={delImg.ExistingFileId.Value}");
                                        
                                        // ✅ 调用删除接口
                                        var deleteResponse = await fileService.DeleteImagesByIdsAsync(
                                            detail.ProdDetailID,
                                            "tb_ProdDetail",
                                            new List<long> { delImg.ExistingFileId.Value },
                                            physicalDelete: false
                                        );
                                        
                                        if (deleteResponse == null || !deleteResponse.IsSuccess)
                                        {
                                            hasError = true;
                                            string errorMsg = deleteResponse?.ErrorMessage ?? "删除结果为空";
                                            MainForm.Instance.uclog.AddLog($"    删除失败: {errorMsg}", Global.UILogType.错误);
                                        }
                                    }
                                }

                                // 2. 处理替换操作(先删后增)
                                var replaceImages = detail.GetPendingReplaceImages();
                                foreach (var repImg in replaceImages)
                                {
                                    if (repImg.ExistingFileId.HasValue && repImg.ImageData != null)
                                    {
                                        MainForm.Instance.uclog.AddLog($"  - 替换图片 FileId={repImg.ExistingFileId.Value}");
                                        
                                        // ✅ 先删除旧的
                                        var deleteResponse = await fileService.DeleteImagesByIdsAsync(
                                            detail.ProdDetailID,
                                            "tb_ProdDetail",
                                            new List<long> { repImg.ExistingFileId.Value },
                                            physicalDelete: false
                                        );
                                        
                                        if (deleteResponse == null || !deleteResponse.IsSuccess)
                                        {
                                            hasError = true;
                                            string errorMsg = deleteResponse?.ErrorMessage ?? "删除结果为空";
                                            MainForm.Instance.uclog.AddLog($"    删除旧图失败: {errorMsg}", Global.UILogType.错误);
                                            continue;
                                        }
                                        
                                        // ✅ 再上传新的
                                        var uploadResult = await fileService.UploadImageAsync(
                                            detail,
                                            repImg.FileName ?? $"image_{DateTime.Now.Ticks}.jpg",
                                            repImg.ImageData,
                                            "ImagesPath"
                                        );
                                        
                                        if (uploadResult != null && uploadResult.IsSuccess)
                                        {
                                            totalSuccessCount++;
                                            MainForm.Instance.uclog.AddLog($"    上传成功: FileId={uploadResult.FileStorageInfos?.FirstOrDefault()?.FileId}");
                                        }
                                        else
                                        {
                                            hasError = true;
                                            string errorMsg = uploadResult?.ErrorMessage ?? "上传结果为空";
                                            MainForm.Instance.uclog.AddLog($"    上传失败: {errorMsg}", Global.UILogType.错误);
                                        }
                                    }
                                }

                                // 3. 处理新增操作
                                var addImages = detail.GetPendingAddImages();
                                foreach (var addImg in addImages)
                                {
                                    if (addImg.ImageData != null && !string.IsNullOrEmpty(addImg.FileName))
                                    {
                                        MainForm.Instance.uclog.AddLog($"  - 新增图片 {addImg.FileName} ({addImg.ImageData.Length} bytes)");
                                        
                                        // ✅ 上传图片
                                        var uploadResult = await fileService.UploadImageAsync(
                                            detail,
                                            addImg.FileName,
                                            addImg.ImageData,
                                            "ImagesPath"
                                        );
                                        
                                        if (uploadResult != null && uploadResult.IsSuccess)
                                        {
                                            totalSuccessCount++;
                                            MainForm.Instance.uclog.AddLog($"    上传成功: FileId={uploadResult.FileStorageInfos?.FirstOrDefault()?.FileId}");
                                        }
                                        else
                                        {
                                            hasError = true;
                                            string errorMsg = uploadResult?.ErrorMessage ?? "上传结果为空";
                                            MainForm.Instance.uclog.AddLog($"    上传失败: {errorMsg}", Global.UILogType.错误);
                                        }
                                    }
                                }

                                if (!hasError)
                                {
                                    // 清除待处理列表
                                    detail.ClearPendingImages();
                                    MainForm.Instance.uclog.AddLog($"SKU '{detail.SKU}' 图片处理成功");
                                }
                                else
                                {
                                    totalFailCount++;
                                }
                            }
                            catch (Exception ex)
                            {
                                totalFailCount++;
                                MainForm.Instance.uclog.AddLog(
                                    $"处理SKU '{detail.SKU}' 图片时异常: {ex.Message}",
                                    Global.UILogType.错误);
                            }
                        }
                    }
                }
            }
            
            if (totalFailCount > 0)
            {
                MessageBox.Show(
                    $"有{totalFailCount}个SKU的图片处理失败!\r\n\r\n" +
                    $"请查看日志了解详细信息。",
                    "SKU图片处理完成",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                
                MainForm.Instance.uclog.AddLog($"SKU图片处理完成: 成功{totalSuccessCount}个, 失败{totalFailCount}个", Global.UILogType.警告);
            }
            else if (totalSuccessCount > 0)
            {
                MainForm.Instance.uclog.AddLog($"成功处理{totalSuccessCount}个SKU的图片");
            }

            base.toolStripButtonModify.Enabled = true;
            return list;
        }

        /// <summary>
        /// ✅ 新增: 智能绘制产品图片缩略图(懒加载)
        /// 规则: 统一优先显示SKU图片,如果没有SKU图片则回退到主图
        /// </summary>
        private async void DrawSmartProductImageThumbnail(DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                var row = dataGridView1.Rows[e.RowIndex];
                var product = row.DataBoundItem as tb_Prod;
                
                if (product == null)
                {
                    DrawPlaceholderImage(e);
                    e.Handled = true;
                    return;
                }
                
                // ✅ 智能获取应该显示的图片FileId(优先SKU,其次主图)
                long? targetFileId = await GetSmartImageFileIdAsync(product);
                
                if (!targetFileId.HasValue || targetFileId.Value <= 0)
                {
                    // 没有图片,不绘制任何内容(保持空白)
                    e.PaintBackground(e.ClipBounds, true);
                    e.Handled = true;
                    return;
                }
                
                // ✅ 使用ImageCacheService获取图片(异步懒加载)
                var imageCacheService = Startup.GetFromFac<RUINORERP.UI.Network.Services.ImageCacheService>();
                if (imageCacheService != null)
                {
                    var imageInfo = await imageCacheService.GetImageInfoByFileIdAsync(targetFileId.Value);
                    
                    if (imageInfo != null && imageInfo.ImageData != null && imageInfo.ImageData.Length > 0)
                    {
                        // 绘制缩略图
                        using (var ms = new System.IO.MemoryStream(imageInfo.ImageData))
                        using (var image = Image.FromStream(ms))
                        {
                            DrawThumbnailInCell(e, image);
                        }
                        e.Handled = true;
                        return;
                    }
                }
                
                // 缓存未命中或加载失败,不绘制任何内容
                e.PaintBackground(e.ClipBounds, true);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                // ✅ 静默失败,不弹窗、不记录日志、不提示
                // 图片绘制失败不影响数据展示,只保持单元格空白即可
                e.PaintBackground(e.ClipBounds, true);
                e.Handled = true;
            }
        }

        /// <summary>
        /// ✅ 新增: 智能获取图片FileId
        /// 规则: 统一优先查找SKU图片,如果没有则回退到主图
        /// </summary>
        private async Task<long?> GetSmartImageFileIdAsync(tb_Prod product)
        {
            try
            {
                // ✅ 第一步: 优先查找SKU图片(遍历所有SKU,找到第一个有图片的)
                if (product.tb_ProdDetails != null && product.tb_ProdDetails.Count > 0)
                {
                    foreach (var detail in product.tb_ProdDetails)
                    {
                        if (!string.IsNullOrEmpty(detail.ImagesPath))
                        {
                            var skuFileIds = ParseFileIds(detail.ImagesPath);
                            if (skuFileIds.Count > 0)
                            {
                                // 找到第一个有图片的SKU,返回其第一个FileId
                                return skuFileIds[0];
                            }
                        }
                    }
                }
                
                // ✅ 第二步: SKU都没有图片,回退到主图
                if (!string.IsNullOrEmpty(product.ImagesPath))
                {
                    var mainFileIds = ParseFileIds(product.ImagesPath);
                    if (mainFileIds.Count > 0)
                    {
                        return mainFileIds[0];
                    }
                }
                
                // 没有任何图片
                return null;
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"获取智能图片FileId失败: {ex.Message}", Global.UILogType.警告);
                return null;
            }
        }

        /// <summary>
        /// ✅ 新增: 解析ImagesPath中的FileId列表
        /// </summary>
        private List<long> ParseFileIds(string imagesPath)
        {
            if (string.IsNullOrEmpty(imagesPath))
                return new List<long>();
            
            return imagesPath.Split(',')
                .Where(s => long.TryParse(s.Trim(), out _))
                .Select(long.Parse)
                .Where(id => id > 0)
                .ToList();
        }

        /// <summary>
        /// ✅ 新增: 绘制缩略图到单元格(自动缩放)
        /// </summary>
        private void DrawThumbnailInCell(DataGridViewCellPaintingEventArgs e, Image image)
        {
            // 绘制背景
            e.PaintBackground(e.ClipBounds, true);
            
            // 计算缩略图尺寸(保持宽高比)
            int maxWidth = e.CellBounds.Width - 4;
            int maxHeight = e.CellBounds.Height - 4;
            
            if (maxWidth <= 0 || maxHeight <= 0 || image.Width == 0 || image.Height == 0)
            {
                DrawPlaceholderImage(e);
                return;
            }
            
            float ratio = Math.Min(
                (float)maxWidth / image.Width,
                (float)maxHeight / image.Height
            );
            
            int thumbWidth = (int)(image.Width * ratio);
            int thumbHeight = (int)(image.Height * ratio);
            
            // 居中绘制
            int x = e.CellBounds.X + (e.CellBounds.Width - thumbWidth) / 2;
            int y = e.CellBounds.Y + (e.CellBounds.Height - thumbHeight) / 2;
            
            Rectangle thumbRect = new Rectangle(x, y, thumbWidth, thumbHeight);
            
            // 高质量绘制
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImage(image, thumbRect);
            
            // 绘制边框
            using (var pen = new Pen(Color.LightGray, 1))
            {
                e.Graphics.DrawRectangle(pen, thumbRect);
            }
        }

        /// <summary>
        /// ✅ 新增: 绘制占位符(已废弃,无图片时保持空白)
        /// </summary>
        private void DrawPlaceholderImage(DataGridViewCellPaintingEventArgs e)
        {
            // 不再绘制"无图片"文字,保持单元格空白
            e.PaintBackground(e.ClipBounds, true);
        }

        /// <summary>
        /// ✅ 新增: 查询完成后预加载可见行图片(优化体验)
        /// 在数据绑定后调用,批量加载当前可见行的图片到缓存
        /// </summary>
        public async Task PreloadVisibleImagesAsync()
        {
            try
            {
                if (dataGridView1.DataSource == null || dataGridView1.Rows.Count == 0)
                    return;

                // 获取可见行的FileId列表
                var visibleFileIds = new List<long>();
                
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.Visible || row.IsNewRow) continue;
                    
                    var product = row.DataBoundItem as tb_Prod;
                    if (product != null)
                    {
                        // ✅ 使用智能逻辑获取FileId
                        var fileId = await GetSmartImageFileIdAsync(product);
                        if (fileId.HasValue && fileId.Value > 0)
                        {
                            visibleFileIds.Add(fileId.Value);
                        }
                    }
                }

                if (visibleFileIds.Count > 0)
                {
                    MainForm.Instance.uclog.AddLog($"开始预加载{visibleFileIds.Count}个产品图片...");
                    
                    // ✅ 批量加载到缓存
                    var imageCacheService = Startup.GetFromFac<RUINORERP.UI.Network.Services.ImageCacheService>();
                    if (imageCacheService != null)
                    {
                        await imageCacheService.GetImageInfosBatchAsync(visibleFileIds);
                        MainForm.Instance.uclog.AddLog("图片预加载完成");
                        
                        // 触发重绘,显示图片
                        dataGridView1.Invalidate();
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.uclog.AddLog($"预加载图片失败: {ex.Message}", Global.UILogType.警告);
            }
        }

        private void UCProductList_Load(object sender, EventArgs e)
        {

        }
    }
}
