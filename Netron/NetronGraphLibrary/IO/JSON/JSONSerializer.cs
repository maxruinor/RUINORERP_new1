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

namespace Netron.GraphLib.IO.JSON
{
    /// <summary>
    /// JSONSerializer serializes a graph to JSON
    /// Thanks to Martin Cully for his work on this.
    /// </summary>
    public class JSONSerializer
    {
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
                XmlTextReader reader = new XmlTextReader(filename);
                IO.JSON.JSONSerializer ser = new IO.JSON.JSONSerializer(site);

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

                tw = new XmlTextWriter(fileName, System.Text.Encoding.Unicode);
                tw.Formatting = System.Xml.Formatting.Indented;
                IO.JSON.JSONSerializer g = new IO.JSON.JSONSerializer();
                g.Serialize(tw, site.Abstract);

                return true;
            }
            catch (Exception exc)
            {
                //TODO: more specific exception handling here
                Trace.WriteLine(exc.Message, "JSONSerializer.SaveAs");
            }
            catch
            {
                Trace.WriteLine("Non-CLS exception caught.", "BinarySerializer.SaveAs");
            }
            finally
            {
                if (tw != null) tw.Close();
            }
            return false;

        }


        #region Serialization


        /// <summary>
        /// 只要根据UI上的节点变成json格式的字符串工作流配置即可
        /// </summary>	
        /// <returns></returns>
        public string Serialize()
        {
            try
            {
                GraphAbstract g = this.site.Abstract;

                foreach (var item in g.Shapes)
                {
                    if (item is  )
                    {

                    }

                }
                // 将工作流对象序列化为 JSON 字符串
                var json = JsonSerializer.Serialize(workflow);

                return json;
            }
            catch (Exception exc)
            {
                Trace.WriteLine(exc.Message, "NMLSerializer.Serialize");
                return string.Empty;
            }
        }

        #endregion




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
}

