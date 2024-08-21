using System.Windows.Forms;

namespace WindowsApplication23
{
    public class ComboBoxFormat : ComboBox
    {
        #region 成员变量
        private const int WM_LBUTTONDOWN = 0x201, WM_LBUTTONDBLCLK = 0x203;
        private string m_sNullValue;
        private string m_sFormat;
        #endregion
        #region 构造函数
        public ComboBoxFormat()
        {

        }
        #endregion      
        #region 重写方法
        private void ShowDropDown()
        {
            FormFormat frmFormat = new FormFormat(m_sNullValue, m_sFormat);
            if (frmFormat.ShowDialog(this) == DialogResult.OK)
            {
                m_sNullValue = frmFormat.NullValue;
                m_sFormat = frmFormat.Format;
                Text = m_sFormat;
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDBLCLK || m.Msg == WM_LBUTTONDOWN)
            {
                ShowDropDown();
                return;
            }
            base.WndProc(ref m);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
