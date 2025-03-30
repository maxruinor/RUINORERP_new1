using RUINORERP.UI.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Shared.Extensions.DataProcessing
{
    // Extensions/DataProcessing/Pipeline.cs
 
        public class DataProcessingPipeline
        {
            private readonly List<IDataProcessor> _processors = new();

            public DataProcessingPipeline AddProcessor(IDataProcessor processor)
            {
                _processors.Add(processor);
                return this;
            }

            public ChartData Process(ChartData data)
            {
                return _processors.Aggregate(data, (current, processor) =>
                    processor.Process(current));
            }
        }

        public interface IDataProcessor
        {
            ChartData Process(ChartData data);
        }

        // 示例处理器
        public class OutlierFilterProcessor : IDataProcessor
        {
            public ChartData Process(ChartData data)
            {
                // 实现异常值过滤逻辑
                return data;
            }
        }
    }
 
