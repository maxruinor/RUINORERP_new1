﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WF.BizOperation.BizEntities
{
    /// <summary>
    /// 流程条件
    /// </summary>
    public class BizCondition
    {

        public BizCondition()
        {

        }

        public bool result { get; set; }


        private int _id;
        private string _name;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
