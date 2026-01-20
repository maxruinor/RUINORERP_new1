using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.Common.CollectionExtension;
using System.Reflection;
using RUINORERP.Model;
using SqlSugar;
using RUINORERP.Business;
using RUINORERP.Common.Helper;

namespace RUINORERP.Assistant.UC
{
    public partial class UCPermissionInfoInit : UserControl
    {
        public UCPermissionInfoInit()
        {
            InitializeComponent();
        }

        tb_MenuInfoController mc = Program.GetFromFac<tb_MenuInfoController>();
        private async void btnInitFieldInfo_Click(object sender, EventArgs e)
        {
            await mc.AddFieldInfo(menuList);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtAssembly.Text = openFileDialog1.FileName;
                //意思是 加载过，数据库中设置好了，就不显示了。
                this.dataGridView1.DataSource = LoadTypes(txtAssembly.Text).ToBindingSortCollection<tb_FieldInfo>();
                // this.dataGridView1.Columns[0].HeaderText = "菜单数" + menuList.Count + "个";
            }
        }
        List<tb_FieldInfo> menuList = new List<tb_FieldInfo>();
        private List<tb_FieldInfo> LoadTypes(string assemblyPath)
        {
            menuList.Clear();
            var assembly = AssemblyLoader.LoadFromPath(assemblyPath);
            if (assembly == null)
            {
                return;
            }
            Type[]? types = assembly.GetExportedTypes();
            if (types != null)
            {
                var descType = typeof(SugarColumn);
                foreach (Type type in types)
                {


                    SugarColumn entityAttr;
                    foreach (PropertyInfo field in type.GetProperties())
                    {
                        foreach (Attribute attr in field.GetCustomAttributes(true))
                        {
                            entityAttr = attr as SugarColumn;
                            if (null != entityAttr)
                            {
                                if (entityAttr.ColumnDescription == null)
                                {
                                    continue;
                                }

                                if (entityAttr.ColumnDescription.Trim().Length > 0)
                                {

                                    tb_FieldInfo info = new tb_FieldInfo();
                                    info.FieldName = entityAttr.ColumnName;
                                    info.FieldText = entityAttr.ColumnDescription;
                                    info.EntityName = type.Name;
                                    info.ClassPath = type.FullName;
                                    info.IsEnabled = !entityAttr.IsIgnore;
                                    menuList.Add(info);
                                }
                            }
                        }
                    }


                }
            }


            return menuList;
        }

        private void btnClearAllField_Click(object sender, EventArgs e)
        {
            
        }
    }
}
