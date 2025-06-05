using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Xml.Schema;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.UI;
using System.Runtime.Remoting;
using System.Windows.Forms;
using System.Drawing;
using System.Text.Json;
using Netron.GraphLib;
using System.Text;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using WorkflowCore.Interface;
using Netron.GraphLib.Entitology;
using MathNet.Numerics.Distributions;
using Formatting = Newtonsoft.Json.Formatting;
using RUINORERP.UI.ProductEAV;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
using RUINORERP.UI.WorkFlowDesigner.Nodes;
using RUINORERP.Common.Extensions;
using WorkflowCore.Services;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using RUINORERP.WF.WorkFlow;
using RUINORERP.WF;
using System.Workflow.ComponentModel;
using Newtonsoft.Json.Linq;

namespace RUINORERP.UI.WorkFlowDesigner.Service
{
    /// <summary>
    /// JSONSerializer serializes a graph to JSON
    /// Thanks to Martin Cully for his work on this.
    /// </summary>
    public class JSONSerializer
    {
        private static JSONSerializer m_instance;

        public static JSONSerializer Instance
        {
            get
            {
                if (m_instance == null)
                {
                    Initialize();
                }
                return m_instance;
            }
            set
            {
                m_instance = value;
            }
        }


        /// <summary>
        /// 对象实例化
        /// </summary>
        public static void Initialize()
        {
            m_instance = new JSONSerializer();
        }

        #region Fields
        private GraphControl site = null;
        private string dtdPath = "http://nml.graphdrawing.org/dtds/1.0rc/nml.dtd";


        //private Hashtable keyList = new Hashtable();

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        private JSONSerializer()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="site"></param>
        public JSONSerializer(GraphControl site)
        {
            this.site = site;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dtdPath"></param>
        public JSONSerializer(string dtdPath)
        {
            this.dtdPath = dtdPath;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Opens a NML serialized file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="site"></param>
        public static void Open(string filename, GraphControl site)
        {

            try
            {
                //XmlTextReader reader = new XmlTextReader(filename);
                //IO.JSON.JSONSerializer ser = new IO.JSON.JSONSerializer(site);

                //site.extract = ser.Deserialize(reader) as GraphAbstract;
                //reader.Close();
            }
            catch (System.IO.DirectoryNotFoundException exc)
            {
                System.Windows.Forms.MessageBox.Show(exc.Message);
            }
            catch (System.IO.FileLoadException exc)
            {
                System.Windows.Forms.MessageBox.Show(exc.Message);
            }
            catch (System.IO.FileNotFoundException exc)
            {
                System.Windows.Forms.MessageBox.Show(exc.Message);
            }
            catch
            {
                site.OutputInfo("Non-CLS exception caught.", "BinarySerializer.SaveAs", OutputInfoLevels.Exception);
            }
        }

        /// <summary>
        /// Saves the diagram to NML format
        /// </summary>
        /// <param name="fileName">the file-path</param>
        /// <param name="site">the graph-control instance to be serialized</param>
        /// <returns></returns>
        public static bool SaveAs(string fileName, GraphControl site)
        {
            XmlTextWriter tw = null;
            try
            {

                // tw = new XmlTextWriter(fileName, System.Text.Encoding.Unicode);
                // tw.Formatting = System.Xml.Formatting.Indented;
                // IO.JSON.JSONSerializer g = new IO.JSON.JSONSerializer();
                // g.Serialize(tw, site.Abstract);

                return true;
            }
            catch (Exception exc)
            {
                //TODO: more specific exception handling here
                Trace.WriteLine(exc.Message, "JSONSerializer.SaveAs");
            }
            finally
            {
                if (tw != null) tw.Close();
            }
            return false;

        }


        #region Serialization

        public string JsonText { get; set; } = string.Empty;

        public List<BaseNode> WFNodes { get; set; } = new List<BaseNode>();


        public bool CheckWorkflowConfig()
        {
            bool rs = true;
            GraphAbstract g = this.site.Abstract;

            //预处理
            foreach (Shape item in g.Shapes)
            {
                WFNodes.Add(item as BaseNode);
            }

            if (WFNodes.Count > 0 && WFNodes.Where(x => x.NodeType == WFNodeType.Start).Count() == 0)
            {
                MessageBox.Show("流程中必须要有且只能一个开始节点。");
                rs = false;
                return rs;
            }
            //foreach (BaseNode item in WFNodes)
            //{
            //    if (item.NodeType == WFNodeType.Start)
            //    {
            //        continue;
            //    }
            //    //如果这个节点有相关联的节点。则设置关联性
            //    if (item.AdjacentNodes.Count > 0)
            //    {
            //        foreach (Connector connPoint in item.Connectors)
            //        {
            //            //一个节点有四个点，分别是上，下，左，右，看每个点的线的情况来定下一个nextnodeid属性
            //            if (connPoint.Connections.Count > 0)
            //            {
            //                Console.WriteLine(connPoint.Name);
            //                var targetPoint = connPoint.Connections[0].To;
            //                if (targetPoint.BelongsTo.UID == item.UID)
            //                {
            //                    continue;//指向自己跳过
            //                }
            //                else
            //                {
            //                    // item.SetPropertyValue("NextStepId", (targetPoint.BelongsTo as BaseNode).Id.ToString());
            //                }
            //            }
            //        }
            //    }
            //}

            Start start = WFNodes.Where(x => x.NodeType == WFNodeType.Start).FirstOrDefault() as Start;
            if (start == null)
            {
                MessageBox.Show("流程中必须要有且只能一个开始节点。");
                rs = false;
                return rs;
            }
            rs = true;
            return rs;
        }

        /// <summary>
        /// 只要根据UI上的节点变成json格式的字符串工作流配置即可
        /// </summary>	
        /// <returns></returns>
        public string Serialize()
        {
            if (!CheckWorkflowConfig())
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            /*


                        List<Product> ProductList = new List<Product>
                                                            {
                                                                new Product
                                                                {
                                                                    ShopID = 1,
                                                                    Price=10,
                                                                    Count=4,
                                                                    Name = "产品一"
                                                                },
                                                                new Product
                                                                {
                                                                    ShopID = 2,
                                                                     Price=11,
                                                                    Count=3,
                                                                    Name = "产品二"
                                                                },
                                                                new Product
                                                                {
                                                                    ShopID = 1,
                                                                     Price=12,
                                                                    Count=1,
                                                                    Name = "产品三"
                                                                },
                                                                new Product
                                                                {
                                                                    ShopID = 2,
                                                                     Price=17,
                                                                    Count=10,
                                                                    Name = "产品四"
                                                                },
                                                                new Product
                                                                {
                                                                    ShopID = 3,
                                                                    Price=13,
                                                                    Count=2,
                                                                    Name = "产品五"
                                                                }
                                                            };

                        //场景一
                        string jsonString = JsonConvert.SerializeObject(ProductList, Formatting.Indented, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            ContractResolver = new JsonPropertyContractResolver(new List<string> { "ShopID", "Name", "Count" })
                        });
                        Console.WriteLine("场景一：" + jsonString);
                        sb.Append(jsonString);
                        sb.Append("===============");
                        //场景二

                        var prod = new Product
                        {
                            ShopID = 3,
                            Price = 13,
                            Count = 2,
                            Name = "你好"
                        };

                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        settings.Formatting = Formatting.Indented;
                        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        settings.ContractResolver = new JsonPropertyContractResolver(new List<string> { "Name", "Price" });
                        string s = JsonConvert.SerializeObject(prod, settings);
                        Console.WriteLine("场景二：" +s );
                        sb.Append(s);
                        sb.Append("===============");
             */

            try
            {
                var obj = this.site.WorkflowData;
                //类头要标记 [JsonObject(MemberSerialization.OptIn)]
                //https://www.cnblogs.com/1175429393wljblog/p/5888098.html
                //Newtonsoft.Json高级用法 1.忽略某些属性 2.默认值的处理 3.空值的处理 4.支持非公共成员 5.日期处理 6.自定义序列化的字段名称 
                JsonSerializerSettings mysettings = new JsonSerializerSettings();
                mysettings.Formatting = Formatting.Indented; //缩进显示
                                                             //mysettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                                                             //mysettings.ContractResolver = new JsonPropertyContractResolver(new List<string> { "Id", "Version", "Steps", "NextStepId", "StepType" });
                string ss = JsonConvert.SerializeObject(obj, mysettings);
                sb.Append(ss);



                // 将工作流对象序列化为 JSON 字符串g
                return sb.ToString();
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.Message, "生成JSON字符串异常");
                return string.Empty;
            }
        }

        #endregion

        /// <summary>
        /// 测试工作流
        /// </summary>
        /// <returns></returns>
        public async Task<string> TestWorkflow()
        {
            string rs = string.Empty;
            if (!CheckWorkflowConfig())
            {
                return string.Empty;
            }
            string json = JsonText;
            // 解析JSON数据
            JObject jsonObject = JObject.Parse(json);

            // 获取Id值
            string idValue = jsonObject["Id"].ToString();
            string version = jsonObject["Version"].ToString();
            //WorkFlowConfigData wFStartRool = (WFNodes.FirstOrDefault(c => c.NodeType == WFNodeType.Start).NodeStepPropertyValue as WorkFlowConfigData);
            string workflowid = idValue;
            if (string.IsNullOrEmpty(workflowid))
            {
                return await Task.FromResult(""); ;
            }
            WorkflowRegisterService wfrs = MainForm.Instance.AppContext.GetRequiredService<WorkflowRegisterService>();
            wfrs.RegisterWorkflowDefinition(json, workflowid);

            Dictionary<string, int> keys = new Dictionary<string, int>();
            keys.Add("Days", 5);

            WorkFlowContextData wfData = new();
            wfData.Name = "jsonNameInit";
            string _workflowid = await MainForm.Instance.AppContext.workflowHost.StartWorkflow<WorkFlowContextData>(workflowid, int.Parse(version), wfData, null);
            return rs;
        }



        #endregion

        #region Helper Functions
        /// <summary>
        /// Validation of the XML
        /// </summary>
        /// <param name="reader"></param>
        public static void Validate(XmlReader reader)
        {
            //TODO: looks a little odd this one
            XmlValidatingReader vr = new XmlValidatingReader(reader);

            vr.ValidationType = ValidationType.Auto;
            vr.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

            while (vr.Read()) { };
        }
        /// <summary>
        /// Outputs the validation of the XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void ValidationHandler(object sender, ValidationEventArgs args)
        {
            Trace.WriteLine(args.ToString(), "NMLSeriliazer.ValidationHandler");
        }

        /// <summary>
        /// Returns a shape on the basis of the unique instantiation key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Shape GetShape(string key)
        {

            ObjectHandle handle;
            Shape shape;
            for (int k = 0; k < site.Libraries.Count; k++)
            {
                for (int m = 0; m < site.Libraries[k].ShapeSummaries.Count; m++)
                {
                    if (site.Libraries[k].ShapeSummaries[m].Key == key)
                    {
                        //Type shapeType = Type.GetType(site.Libraries[k].ShapeSummaries[m].ReflectionName);

                        //object[] args = {this.site};
                        Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));
                        handle = Activator.CreateInstanceFrom(site.Libraries[k].Path, site.Libraries[k].ShapeSummaries[m].ReflectionName);
                        shape = handle.Unwrap() as Shape;
                        return shape;

                    }
                }
            }

            return null;


        }

        /// <summary>
        /// Returns the UID of the entity in string format
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static string FormatID(Entity e)
        {
            return String.Format("{0}", e.UID.ToString());
        }





        /// <summary>
        /// Returns qualified type name of o
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string GetTypeQualifiedName(object o)
        {
            if (o == null)
                throw new ArgumentNullException("GetTypeQualifiedName(object) was called with null parameter");
            return GetTypeQualifiedName(o.GetType());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static string GetTypeQualifiedName(Type t)
        {
            return Assembly.CreateQualifiedName(
                t.Assembly.FullName,
                t.FullName
                );
        }

        private static Type ToType(string text)
        {
            return Type.GetType(text, true);
        }

        private static bool ToBool(string text)
        {
            return bool.Parse(text);
        }
        #endregion
    }

    public class Product
    {
        public int ShopID { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
    }
}

