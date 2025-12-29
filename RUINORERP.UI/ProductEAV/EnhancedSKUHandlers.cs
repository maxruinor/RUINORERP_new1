        /// <summary>
        /// 处理需要添加的属性组合
        /// 增强版实现，支持两种编辑场景：
        /// 1. 添加全新属性时：创建包含所有属性的完整组合集合
        /// 2. 添加新属性值时：创建新的属性组合，但保留已有属性组合的特性
        /// </summary>
        /// <param name="combinationsToAdd">需要添加的组合列表</param>
        private void HandleCombinationsToAdd(List<AttributeCombination> combinationsToAdd)
        {
            // 检查是否是从单属性转换为多属性的情况，获取原始SKU信息
            tb_ProdDetail originalSkuDetail = null;
            if (EditEntity.tb_Prod_Attr_Relations.Count == 1)
            {
                ProductAttributeType pt = (ProductAttributeType)(int.Parse(cmbPropertyType.SelectedValue.ToString()));
                if (pt == ProductAttributeType.可配置多属性 && EditEntity.tb_Prod_Attr_Relations[0].Property_ID == null)
                {
                    // 从单属性转换为多属性，获取原始产品详情
                    originalSkuDetail = EditEntity.tb_ProdDetails.FirstOrDefault();
                }
            }

            bool isFirstCombination = true;
            foreach (var combination in combinationsToAdd)
            {
                // 创建新的产品详情
                long skuRowId = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                var newDetail = new tb_ProdDetail
                {
                    ProdBaseID = EditEntity.ProdBaseID,
                    ProdDetailID = skuRowId, // 临时ID，保存到DB前会重新设置
                    ActionStatus = ActionStatus.新增,
                    Is_enabled = true,
                    Is_available = true,
                    Created_at = DateTime.Now,
                    Created_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID,
                    tb_Prod_Attr_Relations = new List<tb_Prod_Attr_Relation>()
                };

                // 如果是第一个组合且存在原始SKU信息，继承原始SKU属性
                // 当添加新属性时，第一个组合设为默认值
                if (isFirstCombination && originalSkuDetail != null)
                {
                    newDetail.SKU = originalSkuDetail.SKU;
                    // 复制其他相关字段，确保产品信息一致性
                    newDetail.ProdBaseID = originalSkuDetail.ProdBaseID;
                    newDetail.PrimaryKeyID = originalSkuDetail.PrimaryKeyID;
                    newDetail.BarCode = originalSkuDetail.BarCode;
                    newDetail.Weight = originalSkuDetail.Weight;
                    newDetail.BOM_ID = originalSkuDetail.BOM_ID;
                    newDetail.DataStatus = originalSkuDetail.DataStatus;
                    newDetail.Created_at = originalSkuDetail.Created_at;
                    newDetail.Created_by = originalSkuDetail.Created_by;
                    BusinessHelper.Instance.EditEntity(newDetail); // 初始化编辑信息
                    newDetail.Discount_Price = originalSkuDetail.Discount_Price;
                    newDetail.Image = originalSkuDetail.Image;
                    newDetail.Images = originalSkuDetail.Images;
                    newDetail.ImagesPath = originalSkuDetail.ImagesPath;
                    newDetail.Is_available = originalSkuDetail.Is_available;
                    newDetail.Is_enabled = originalSkuDetail.Is_enabled;
                    newDetail.Market_Price = originalSkuDetail.Market_Price;
                    newDetail.Modified_at = originalSkuDetail.Modified_at;
                    newDetail.Modified_by = originalSkuDetail.Modified_by;
                    newDetail.Notes = originalSkuDetail.Notes;
                    newDetail.ProdDetailID = originalSkuDetail.ProdDetailID;
                    newDetail.SalePublish = originalSkuDetail.SalePublish;
                    newDetail.Standard_Price = originalSkuDetail.Standard_Price;
                }

                // 创建TreeGrid节点
                Font boldFont = new Font(treeGridView1.DefaultCellStyle.Font, FontStyle.Bold);
                TreeGridNode node = treeGridView1.Nodes.Add(skuRowId, 0, "", GetPropertiesText(combination),
                    newDetail.SKU != null ? newDetail.SKU : "等待生成", EditEntity.CNName, "新增", newDetail.Is_enabled);
                node.NodeName = skuRowId.ToString();
                node.ImageIndex = 1; // 新增图标
                node.Tag = newDetail;
                node.DefaultCellStyle.Font = boldFont;

                // 为每个属性值创建属性关系
                foreach (var attrValuePair in combination.Properties)
                {
                    var relation = new tb_Prod_Attr_Relation
                    {
                        Property_ID = attrValuePair.Property.Property_ID,
                        PropertyValueID = attrValuePair.PropertyValue.PropertyValueID,
                        ProdBaseID = EditEntity.ProdBaseID,
                        ProdDetailID = skuRowId,
                        ActionStatus = ActionStatus.新增,
                        tb_prodproperty = attrValuePair.Property,
                        tb_prodpropertyvalue = attrValuePair.PropertyValue
                    };

                    // 添加到产品详情和编辑实体
                    newDetail.tb_Prod_Attr_Relations.Add(relation);
                    EditEntity.tb_Prod_Attr_Relations.Add(relation);

                    // 创建子节点
                    long rowId = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                    TreeGridNode subNode = node.Nodes.Add(rowId, relation.RAR_ID, attrValuePair.Property.PropertyName, attrValuePair.PropertyValue.PropertyValueName, "", "", "新增");
                    subNode.NodeName = rowId.ToString();
                    subNode.Tag = relation;
                    subNode.ImageIndex = 1; // 新增图标
                }

                // 添加到产品详情列表
                EditEntity.tb_ProdDetails.Add(newDetail);

                isFirstCombination = false;
            }
        }

        /// <summary>
        /// 处理需要删除的属性组合
        /// 增强版实现，根据不同编辑场景采取不同的删除策略：
        /// 1. 添加全新属性时：删除所有缺少新属性的旧组合
        /// 2. 添加新属性值时：只删除不再需要的特定组合
        /// </summary>
        /// <param name="combinationsToRemove">需要删除的组合列表</param>
        private void HandleCombinationsToRemove(List<AttributeCombination> combinationsToRemove)
        {
            foreach (var combination in combinationsToRemove)
            {
                if (combination.ProductDetail != null)
                {
                    // 从TreeGrid中移除对应的节点
                    TreeGridNode nodeToRemove = treeGridView1.Nodes.FirstOrDefault(n => n.Tag == combination.ProductDetail);
                    if (nodeToRemove != null)
                    {
                        // 如果是新增的产品详情，直接删除
                        if (combination.ProductDetail.ActionStatus == ActionStatus.新增)
                        {
                            // 移除所有子节点对应的属性关系
                            foreach (TreeGridNode subNode in nodeToRemove.Nodes)
                            {
                                if (subNode.Tag is tb_Prod_Attr_Relation relation)
                                {
                                    // 安全地从编辑实体中移除关系
                                    EditEntity.tb_Prod_Attr_Relations.Remove(relation);
                                }
                            }

                            // 移除产品详情
                            EditEntity.tb_ProdDetails.Remove(combination.ProductDetail);
                            treeGridView1.Nodes.Remove(nodeToRemove);
                        }
                        // 如果是已存在的产品详情，标记为删除
                        else
                        {
                            // 标记产品详情为删除状态
                            combination.ProductDetail.ActionStatus = ActionStatus.删除;
                            nodeToRemove.Cells[6].Value = "删除";
                            nodeToRemove.ImageIndex = 3; // 删除图标
                        }
                    }
                }
            }
        }
