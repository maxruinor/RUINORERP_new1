using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.UControls
{
        public class GridViewExtension
        {
            // 菜单常量定义
            public const string MULTI_SELECT_MODE = "多选模式";
            public const string BATCH_EDIT_COLUMN = "批量编辑列值";
            public const string FILTER_BY_COLUMN = "列头按指定列过滤值";

            // 构建列头菜单集合
            public List<MenuItem> BuildHeaderMenu(params string[] menuTypes)
            {
                var menuItems = new List<MenuItem>();

                foreach (var menuType in menuTypes)
                {
                    switch (menuType)
                    {
                        case MULTI_SELECT_MODE:
                            menuItems.Add(new MenuItem
                            {
                                Text = MULTI_SELECT_MODE,
                                CommandName = "MultiSelect",
                                Icon = "check-square-o"
                            });
                            break;

                        case BATCH_EDIT_COLUMN:
                            menuItems.Add(new MenuItem
                            {
                                Text = BATCH_EDIT_COLUMN,
                                CommandName = "BatchEdit",
                                Icon = "pencil-square-o"
                            });
                            break;

                        case FILTER_BY_COLUMN:
                            menuItems.Add(new MenuItem
                            {
                                Text = FILTER_BY_COLUMN,
                                CommandName = "ColumnFilter",
                                Icon = "filter"
                            });
                            break;

                        default:
                            throw new ArgumentException($"未知的菜单类型: {menuType}");
                    }
                }

                return menuItems;
            }
        }

        // 菜单项类定义
        public class MenuItem
        {
            public string Text { get; set; }
            public string CommandName { get; set; }
            public string Icon { get; set; }
            // 可添加其他属性如IsEnabled, IsVisible等
        }
    }
