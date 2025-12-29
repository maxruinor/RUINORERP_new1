        /// <summary>
        /// 处理属性选择状态变化事件
        /// 区分两种编辑场景：
        /// 1. 在已有属性中添加新属性值：生成新SKU时排除系统中已存在的相同属性组合
        /// 2. 添加全新属性：与原有属性进行完整的排列组合生成新SKU集合
        /// </summary>
        /// <param name="sender">触发事件的控件</param>
        /// <param name="e">事件参数</param>
        private void CheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb == null || !(cb.Tag is tb_ProdPropertyValue ppv))
            {
                return;
            }

            // 获取当前选中的所有属性组和属性值
            var selectedAttributeGroups = GetSelectedAttributeGroups();
            if (selectedAttributeGroups.Count == 0)
            {
                return;
            }

            // 获取当前已有的属性组合
            var existingCombinations = GetExistingAttributeCombinations();
            
            // 获取选中的属性组ID列表
            var selectedGroupIds = selectedAttributeGroups.Select(g => g.Property.Property_ID).ToList();
            
            // 获取现有属性组合中使用的属性组ID列表
            var existingGroupIds = new HashSet<long>();
            foreach (var combination in existingCombinations)
            {
                foreach (var prop in combination.Properties)
                {
                    existingGroupIds.Add(prop.Property.Property_ID);
                }
            }

            // 判断是否是添加全新属性
            bool isAddingNewProperty = selectedGroupIds.Except(existingGroupIds).Any();
            
            // 生成属性组合
            List<AttributeCombination> newCombinations = GenerateAttributeCombinations(selectedAttributeGroups);
            
            // 使用AttributeCombinationComparer来比较组合
            var comparer = new AttributeCombinationComparer();
            
            // 根据不同场景处理组合
            List<AttributeCombination> combinationsToAdd;
            List<AttributeCombination> combinationsToRemove;
            
            if (isAddingNewProperty)
            {
                // 场景1：添加全新属性 - 保留原有组合，只添加新组合
                // 当添加新属性时，我们需要删除所有现有组合（因为它们缺少新属性），
                // 并添加包含新属性的完整组合集合
                combinationsToRemove = existingCombinations.ToList();
                combinationsToAdd = newCombinations;
            }
            else
            {
                // 场景2：在已有属性中添加新属性值 - 排除已存在的组合
                combinationsToAdd = newCombinations.Except(existingCombinations, comparer).ToList();
                combinationsToRemove = existingCombinations.Except(newCombinations, comparer).ToList();
            }

            // 处理需要删除的组合
            HandleCombinationsToRemove(combinationsToRemove);

            // 处理需要添加的组合
            HandleCombinationsToAdd(combinationsToAdd);

            treeGridView1.Refresh();
        }
