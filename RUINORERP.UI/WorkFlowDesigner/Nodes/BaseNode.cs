using Netron.GraphLib;
using Netron.GraphLib.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.WorkFlowDesigner.Nodes
{

    /// <summary>
    /// UI流程节点基类
    /// </summary>
    public class BaseNode : Shape, ISerializable
    {

        public BaseNode(IGraphSite site, WFNodeType nodeType) : base(site)
        {
            NodeType = nodeType;
        }
        public WFNodeType NodeType { get; set; }


        public BaseNode()
        {
        }

        public BaseNode(IGraphSite site) : base(site)
        {

        }

        public BaseNode(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
