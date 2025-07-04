﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model
{
    public partial class tb_P4Button : BaseEntity, ICloneable
    {

        //一定要加,不然会与数据库关联上会出错
        [SugarColumn(IsIgnore = true, ColumnDescription = "说明")]
        public string Notes { get; set; }

        //一定要加,不然会与数据库关联上会出错
        [SugarColumn(IsIgnore = true, ColumnDescription = "按钮类型")]
        public string ButtonType { get; set; }
    }
}
