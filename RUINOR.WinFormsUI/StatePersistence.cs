using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;

namespace RUINOR.WinFormsUI
{
    //使用方法
    /*
     		StatePersistence persist = null;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox textBox4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			persist = new StatePersistence(this);
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

     
     */



    /// <summary>
    /// 可以保存介面控件中的值，下次启动时还是这个值 目前主要是TextBox
    /// 状态持久性-跨应用程序启动保持状态 Summary description for StatePersistence.
    /// </summary>
    public class StatePersistence
    {
        private System.Collections.Specialized.NameValueCollection formValues = null;
        string formStateFileName = string.Empty;
        private Form form;

        public StatePersistence(Form form)
        {
            this.form = form;
            this.form.Closing += new System.ComponentModel.CancelEventHandler(form_Closing);
            formStateFileName = Assembly.GetExecutingAssembly().Location.Substring(0,
                Assembly.GetExecutingAssembly().Location.LastIndexOf("\\")) + "\\" + form.Name + ".xml";
            LoadControlValues();
        }

        public void LoadControlValues()
        {
            RetrieveFormState();
            foreach (Control ctrl in form.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox tb = (ctrl as TextBox);
                    tb.Text = formValues.Get(tb.Name);
                }
            }
        }
        public void SaveControlValues()
        {
            foreach (Control ctrl in form.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox tb = (ctrl as TextBox);
                    formValues.Set(tb.Name, tb.Text);
                }
            }
            SaveFormState();
        }

        private void RetrieveFormState()
        {
            if (File.Exists(formStateFileName))
            {
                object o = null;
                using (Stream stream = new FileStream(formStateFileName, FileMode.Open, FileAccess.Read))
                {
                    SoapFormatter formatter = new SoapFormatter();
                    o = formatter.Deserialize(stream);
                    formValues = (NameValueCollection)o;
                }
            }
            else
            {
                formValues = new NameValueCollection();
            }
        }

        private void SaveFormState()
        {
            using (Stream s = new FileStream(formStateFileName, FileMode.Create, FileAccess.Write))
            {
                SoapFormatter formatter = new SoapFormatter();
                formatter.Serialize(s, formValues);

                using (StreamWriter w = new StreamWriter(s))
                {
                    w.Flush();
                }
            }
        }

        private void form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveControlValues();
        }
    }
}

