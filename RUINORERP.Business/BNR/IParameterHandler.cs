﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUINORERP.Business.BNR
{
    public interface IParameterHandler
    {
        void Execute(StringBuilder sb, string value);

        object Factory
        {
            get;
            set;
        }
    }



}
