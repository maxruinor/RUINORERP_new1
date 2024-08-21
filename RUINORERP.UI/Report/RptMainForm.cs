using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.Report
{
    public partial class RptMainForm : KryptonForm
    {
        private List<Category> FBusinessObject;
        private FastReport.Report FReport;//</category>
        public RptMainForm()
        {
            InitializeComponent();
        }

        private void btnDesign_Click(object sender, EventArgs e)
        {
            //RptDesignForm dForm = new RptDesignForm();
            //dForm.Show();
            DesignReport();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            FReport = new FastReport.Report();   //实例化一个Report报表
            String reportFile = string.Format("ReportTemplate/{0}", "TEST1.frx");
            FReport.Load(reportFile);  //载入报表文件
            FReport.Preview = previewControl1; //设置报表的Preview控件（这里的previewControl1就是我们之前拖进去的那个）
            FReport.Prepare();   //准备
            FReport.ShowPrepared();  //显示
        }

        private void RptMainForm_Load(object sender, EventArgs e)
        {

        }

        public void CreateDataSource()
        {
            FBusinessObject = new List<Category>(); //Create list of categories

            Category category = new Category("Beverages", "Soft drinks, coffees, teas, beers"); //Create new instance of category
            category.Products.Add(new Product("Chai", 18m)); //Add new product to category
            category.Products.Add(new Product("Chang", 19m));
            category.Products.Add(new Product("Ipoh coffee", 46m));

            FBusinessObject.Add(category); //Add the category to the List

            category = new Category("Confections", "Desserts, candies, and sweet breads");
            category.Products.Add(new Product("Chocolade", 12.75m));
            category.Products.Add(new Product("Scottish Longbreads", 12.5m));
            category.Products.Add(new Product("Tarte au sucre", 49.3m));

            FBusinessObject.Add(category);

            category = new Category("Seafood", "Seaweed and fish");
            category.Products.Add(new Product("Boston Crab Meat", 18.4m));
            category.Products.Add(new Product("Red caviar", 15m));

            FBusinessObject.Add(category);
        }

        public void RegisterData()
        {
            FReport.RegisterData(FBusinessObject, "Categories");
        }

        public void DesignReport()
        {
            FReport = new FastReport.Report();
            CreateDataSource();
            RegisterData();
            FReport.Design();
        }

    }
    public class Product
    {
        private string FName;
        private decimal FUnitPrice;

        public string Name
        {
            get { return FName; }
        }

        public decimal UnitPrice
        {
            get { return FUnitPrice; }
        }

        public Product(string name, decimal unitPrice)
        {
            FName = name;
            FUnitPrice = unitPrice;
        }
    }

    public class Category
    {
        private string FName;
        private string FDescription;
        private List<Product> FProducts;

        public string Name
        {
            get { return FName; }
        }

        public string Description
        {
            get { return FDescription; }
        }

        public List<Product> Products
        {
            get { return FProducts; }
        }

        public Category(string name, string description)
        {
            FName = name;
            FDescription = description;
            FProducts = new List<Product>();
        }
    }
}
