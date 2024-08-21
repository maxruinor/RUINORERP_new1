using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using HLH.WinControl.ComBoBoxEx;

namespace CommonProcess.StringProcess
{
    public partial class UCCheckData : UCMyBase
    {
        public UCCheckData()
        {
            InitializeComponent();
        }

        public string KeyID { get; set; }





        private void UCFindSpecialChar_Load(object sender, EventArgs e)
        {

        }



        public string ProcessString(string SourceString)
        {
            string rs = string.Empty;
            HtmlAgilityPack.HtmlDocument ReferenceHtmlDoc = new HtmlAgilityPack.HtmlDocument();
            ReferenceHtmlDoc.LoadHtml(SourceString);
            if (ReferenceHtmlDoc.DocumentNode.InnerText.Length < int.Parse(txtLength.Text.Trim()))
            {
                printInfoMessage(KeyID + ",", Color.Red);
            }

            return SourceString;
        }




    }
}
