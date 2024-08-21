using System.Windows.Forms;
using System.Xml;
namespace HLH.Lib.Helper
{
    public class TreeViewHelper
    {
        private TreeView thetreeview;
        private string xmlfilepath;
        XmlTextWriter textWriter;
        XmlNode Xmlroot;
        XmlDocument textdoc;

        public TreeViewHelper()
        {
            //----���캯��
            textdoc = new XmlDocument();

        }

        ~TreeViewHelper()
        {
            //----��������

        }

        #region������treeview��ʵ����XML��ת��
        ///��<summary>������
        ///������treeview��ʵ����XML��ת��
        ///��</summary>������
        ///��<param��name="TheTreeView">���ؼ�����</param>������
        ///��<param��name="XMLFilePath">XML���·��</param>������
        ///��<returns>0��ʾ����˳��ִ��</returns>������

        public int TreeToXML(TreeView TheTreeView, string XMLFilePath)
        {
            //-------��ʼ��ת����������
            thetreeview = TheTreeView;
            xmlfilepath = XMLFilePath;
            textWriter = new XmlTextWriter(xmlfilepath, null);

            //-------����XMLд��������
            textWriter.Formatting = Formatting.Indented;

            //-------��ʼд���̣�����WriteStartDocument����
            textWriter.WriteStartDocument();

            //-------д��˵��
            textWriter.WriteComment("this��XML��is��created��from��a��tree");
            textWriter.WriteComment("By��˼������");

            //-------��ӵ�һ�����ڵ�
            textWriter.WriteStartElement("TreeExXMLCls");
            textWriter.WriteEndElement();

            //------��д�ĵ�����������WriteEndDocument����
            textWriter.WriteEndDocument();

            //-----�ر�������
            textWriter.Close();

            //-------����XMLDocument����
            textdoc.Load(xmlfilepath);

            //------ѡ�и��ڵ�
            XmlElement Xmlnode = textdoc.CreateElement(thetreeview.Nodes[0].Text);
            Xmlroot = textdoc.SelectSingleNode("TreeExXMLCls");

            //------����ԭtreeview�ؼ�����������Ӧ��XML
            TransTreeSav(thetreeview.Nodes, (XmlElement)Xmlroot);


            return 0;


        }

        private int TransTreeSav(TreeNodeCollection nodes, XmlElement ParXmlnode)
        {

            //-------�������ĸ������Ͻڵ㣬ͬʱ��ӽڵ���XML
            XmlElement xmlnode;
            Xmlroot = textdoc.SelectSingleNode("TreeExXMLCls");

            foreach (TreeNode node in nodes)
            {
                xmlnode = textdoc.CreateElement(node.Text);
                ParXmlnode.AppendChild(xmlnode);

                if (node.Nodes.Count > 0)
                {
                    TransTreeSav(node.Nodes, xmlnode);
                }
            }
            textdoc.Save(xmlfilepath);

            return 0;
        }

        #endregion

        #region������XML��ʵ����tree��ת��
        ///��<summary>������
        ///������treeview��ʵ����XML��ת��
        ///��</summary>������
        ///��<param��name="XMLFilePath">XML���·��</param>������
        ///��<param��name="TheTreeView">���ؼ�����</param>������
        ///��<returns>0��ʾ����˳��ִ��</returns>������

        public int XMLToTree(string XMLFilePath, TreeView TheTreeView)
        {
            //-------���³�ʼ��ת����������
            thetreeview = TheTreeView;
            xmlfilepath = XMLFilePath;

            //-------���¶�XMLDocument����ֵ
            textdoc.Load(xmlfilepath);

            XmlNode root = textdoc.SelectSingleNode("TreeExXMLCls");

            foreach (XmlNode subXmlnod in root.ChildNodes)
            {
                TreeNode trerotnod = new TreeNode();
                trerotnod.Text = subXmlnod.Name;
                thetreeview.Nodes.Add(trerotnod);
                TransXML(subXmlnod.ChildNodes, trerotnod);

            }

            return 0;
        }

        private int TransXML(XmlNodeList Xmlnodes, TreeNode partrenod)
        {
            //------����XML�е����нڵ㣬����treeview�ڵ��������
            foreach (XmlNode xmlnod in Xmlnodes)
            {
                TreeNode subtrnod = new TreeNode();
                subtrnod.Text = xmlnod.Name;
                partrenod.Nodes.Add(subtrnod);

                if (xmlnod.ChildNodes.Count > 0)
                {
                    TransXML(xmlnod.ChildNodes, subtrnod);
                }
            }

            return 0;

        }

        #endregion


    }




}
