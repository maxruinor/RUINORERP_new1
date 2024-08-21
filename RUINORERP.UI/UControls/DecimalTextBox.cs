using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;

namespace RUINORERP.UI.UControls
{
    //class Class1 : ComponentFactory.Krypton.Toolkit.KryptonTextBox
    //{
    //}

    /// <devdoc>
    /// DecimalTextBox control definition class.
    /// A text box that accepts numeric decimal input only.
    /// </devdoc>
    public class DecimalTextBox : Krypton.Toolkit.KryptonTextBox
    {
        const int WM_COPY = 0x0301,
        WM_CUT = 0x0300,
        WM_PASTE = 0x0302,
        WM_CLEAR = 0x0303,
        WM_CONTEXTMENU = 0x007B;

        //static NumberFormatInfo nfi = NumberFormatInfo.CurrentInfo;
        public Decimal TextValue;//文本的 Decimal值
        private NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
        private System.Drawing.Color _redColor = System.Drawing.Color.Red; //为负数时候的颜色
        private int _intDecimal = 2;
        private bool _isNegative = false;
        private int _decimalLength = 15;
        [Category("Categoria"), DefaultValue(2), Browsable(true), Description("小数的位数")]
        public int intDecimal
        {
            get { return _intDecimal; }
            set
            {
                this._intDecimal = value;
                nfi.NumberDecimalDigits = value;

                ResetText();

            }

        }
        [Category("Categoria"), DefaultValue(false), Browsable(true), Description("是否为可以为负数")]

        public bool isNegative
        {
            get { return this._isNegative; }
            set
            {
                this._isNegative = value;
                try
                {
                    if (isNegative)
                    {

                        if (Text.ToString().IndexOf("-") < 0)
                        {
                            base.Text = "-" + base.Text;
                            this.ForeColor = _redColor;
                        }
                    }
                }
                catch { }
            }
        }
        [Category("Categoria"), DefaultValue(15), Browsable(true), Description("文本框的文本长度")]
        public int decimalLength
        {
            get { return _decimalLength; }
            set { this._decimalLength = value; }
        }

        /// <devdoc>
        /// Set the initial params.
        /// </devdoc>
        public DecimalTextBox()
        {
            // nfi.NumberDecimalDigits = intDecimal;
            base.TextAlign = HorizontalAlignment.Right;
            ResetText();
        }


        /// <devdoc>
        /// Overridden to validate and format the text.
        /// </devdoc>
        public override string Text
        {
            get
            {
                return base.Text;

            }
            set
            {
                if (value != base.Text)
                {
                    SetFormattedText(new StringBuilder(value));
                }

            }
        }

        /// <devdoc>
        /// Overridden to handle unsupported RETURN key.
        /// </devdoc>
        protected override bool IsInputKey(Keys keyData)
        {
            if ((keyData & Keys.KeyCode) == Keys.Return)
            {
                return false;
            }
            return base.IsInputKey(keyData);
        }

        /// <devdoc>
        /// Overridden to handle supported shortcuts Ctrl-C (Copy All) & Ctrl-D (Delete All).
        /// </devdoc>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //
            // The base class should be called first because it implements ShortcutsEnabled,
            // which takes precedence over Ctrl+C
            //
            bool msgProcessed = base.ProcessCmdKey(ref msg, keyData);

            if (!msgProcessed)
            {
                if ((int)keyData == (int)Shortcut.CtrlC)
                {
                    WmCopy();
                    msgProcessed = true;
                }
                else if (((int)keyData == (int)Shortcut.CtrlD))
                {
                    ResetText();
                    msgProcessed = true;
                }
                else if (((int)keyData == (int)Shortcut.CtrlX))
                {
                    WmCopy();
                    ResetText();
                    msgProcessed = true;
                }
            }

            return msgProcessed;
        }

        /// <devdoc>
        /// Set the initial text.
        /// </devdoc>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (this.Visible)
            {
                this.SelectionStart = 1;
                this.SelectionLength = 0;
            }
        }

        /// <devdoc>
        /// Handle text deletion.
        /// </devdoc>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            Keys keyCode = e.KeyCode;

            if (this.SelectionLength > 0)
            {
                // For now we don't support replace over selection.
                this.SelectionLength = 0;
            }

            if (keyCode == Keys.Back || keyCode == Keys.Delete)
            {
                e.Handled = true;

                if ((e.Modifiers & Keys.Modifiers) != 0)
                {
                    // Key modifiers not allowed for these keys.
                    return;
                }

                int caretPos = this.SelectionStart;
                StringBuilder txt = new StringBuilder(this.Text);
                int originalTextLength = txt.Length;

                if (originalTextLength - caretPos <= nfi.NumberDecimalDigits) // caret at a decimal location
                {
                    // Corner cases for caret postion.
                    // At the beginning: Del works as normal. Bck jumps over the decimal separator.
                    // At the end:: Del does nothing. Bck works as normal.

                    if (caretPos == originalTextLength && keyCode == Keys.Delete)
                    {
                        return;
                    }

                    // On Bck move caret back by one to perform Del.
                    if (keyCode == Keys.Back)
                    {
                        caretPos--;
                    }

                    bool updateCaret = true;

                    if (txt[caretPos] != nfi.NumberDecimalSeparator[0]) // Not at the decimal separator.
                    {
                        txt.Remove(caretPos, 1);
                        updateCaret = SetFormattedText(txt);
                    }

                    if (updateCaret)
                    {
                        this.SelectionStart = caretPos;
                    }
                }
                else // caret at a whole number's location or at the decimal separator's location.
                {
                    // Corner cases for caret postion.
                    // At the beginning: Del should not move the caret (exeption: When first number is zero it should jump it).
                    // Bck should do nothing.
                    // At the end: Del jumps over the decimal separator. Bck works as normal.

                    if (txt[caretPos] == nfi.NumberDecimalSeparator[0]) // at the decimal separator location (end).
                    {
                        if (keyCode == Keys.Delete) // Jump caret over the decimal separator.
                        {
                            this.SelectionStart = caretPos + 1;
                            return;
                        }
                    }

                    if (keyCode == Keys.Back)
                    {
                        if (caretPos == 0) // at the beginning of string.
                        {
                            return;
                        }
                        caretPos--; // Get ready for a Del.
                    }

                    // If caret is at a group separator, deleting the separator would not change the text since it would
                    // get formatted the same way. We need to skip over it.
                    // Del: skip right. Bck: we alread skip left above.
                    if (txt[caretPos] == nfi.NumberGroupSeparator[0])
                    {
                        if (keyCode == Keys.Delete)
                        {
                            caretPos++;
                        }

                        this.SelectionStart = caretPos;
                        return;
                    }

                    // removing a number.
                    txt.Remove(caretPos, 1);
                    if (SetFormattedText(txt))
                    {

                        // Set new caret location.
                        if (caretPos == 0)
                        {
                            // When first number is 0, Del on it should simply skip over it.
                            if (txt[0] == '0' && keyCode == Keys.Delete)
                            {
                                caretPos++;
                            }
                        }
                        else
                        {
                            // +1 so the caret moves on Delete or stays on BackSpc.
                            caretPos = caretPos - (originalTextLength - txt.Length) + 1;
                        }

                        this.SelectionStart = caretPos;
                    }
                }
            }
        }

        /// <devdoc>
        /// Handle input filtering and text insertion.
        /// </devdoc>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            e.Handled = true; // This disables undersired keys. ProcessCmdKey is used to enabled supported shortcuts.

            // Filter allowed input: integers or decimal separator only.
            if (this.Text.Length > decimalLength)
            {
                e.Handled = true;

            }
            else
            {
                //如果是负数并且输入的为负数
                if (this.isNegative && e.KeyChar == '-')
                {
                    if (!(this.isNegative && this.SelectionStart == 0 && this.Text.IndexOf("-") < 0))
                    {
                        return;
                    }
                }
                else
                {

                    if ((!char.IsDigit(e.KeyChar)) && e.KeyChar != nfi.NumberDecimalSeparator[0])
                    {
                        return;
                    }
                }

                int caretPos = this.SelectionStart;
                StringBuilder txt = new StringBuilder(this.Text);

                if (e.KeyChar == nfi.NumberDecimalSeparator[0])
                {
                    // Go to decimal input location.
                    this.SelectionStart = txt.Length - nfi.NumberDecimalDigits;
                    return;
                }

                if (caretPos == txt.Length) // caret at the end of the box
                {
                    // input ignored.
                    return;
                }

                int originalTextLength = txt.Length;

                if (originalTextLength - caretPos <= nfi.NumberDecimalDigits) // caret at decimal location
                {
                    // Replace the current location's value with the input.
                    txt[caretPos] = e.KeyChar;
                    this.Text = txt.ToString();
                    this.SelectionStart = caretPos + 1;
                }
                else // caret at a whole number's location or at the decimal separator's location.
                {
                    // If caret is at pos 0 and the number is zero, we need to move caret so it can be inserted after the 0
                    // and get properly formatted; otherwise, the input would be inserted at zero (effect = multiplied by 10).
                    if (caretPos == 0 && txt[0] == '0')
                    {
                        caretPos++;
                    }

                    // Insert value.

                    if (e.KeyChar == '-' && txt.ToString().IndexOf("-") < 0)
                    {
                        txt.Insert(0, e.KeyChar);
                    }
                    else
                    {
                        txt.Insert(caretPos, e.KeyChar);
                    }
                    if (SetFormattedText(txt))
                    {
                        this.SelectionStart = caretPos + (txt.Length - originalTextLength);
                    }
                }
            }

        }

        /// <devdoc>
        /// Overridden to reset selected text.
        /// </devdoc>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            ResetSelection();
        }

        /// <devdoc>
        /// Selecting Text is not allowed.
        /// </devdoc>
        private void ResetSelection()
        {
            if (this.SelectionLength > 0)
            {
                this.SelectionLength = 0;
            }
        }

        /// <devdoc>
        /// </devdoc>
        public override void ResetText()
        {
            SetFormattedText(new StringBuilder("0"));
        }


        /// <devdoc>
        /// Sets the decimal-formatted text into the control's text.
        /// Returns false if an exception is thrown during text formatting due to overflow or unexpected characters in
        /// the specified text.
        /// Methods calling this method should check for the return value before doing further processing.
        /// The input should contain interger numbers and decimal group separator characters only; it does not expect
        /// the string to represent a well formatted decimal number.
        /// </devdoc>
        private bool SetFormattedText(StringBuilder txt)
        {
            // Remove group separators to re-format the string.

            String _text = txt.ToString();

            txt.Replace(nfi.NumberGroupSeparator, "");

            try
            {
                string _temp = "-";
                if (_text.IndexOf("-") > -1 && this.isNegative)
                {
                    _temp = "-";
                }
                else
                {
                    _temp = "";
                }
                decimal value = decimal.Parse(txt.ToString()); // parse decimal value.
                txt.Length = 0; // clear text.

                txt.Append(value.ToString("N", nfi)); // set new text.
                if (_temp == "-" && txt.ToString().IndexOf("-") < 0)
                {
                    base.Text = _temp + txt.ToString(); // set control's text.
                }
                else
                {
                    base.Text = txt.ToString();
                }
                if (Text.ToString().IndexOf("-") < 0)
                {
                    //isNegative = false;
                    this.ForeColor = System.Drawing.SystemColors.WindowText;
                }
                else
                {
                    this.ForeColor = _redColor;
                }

                TextValue = Decimal.Parse(Decimal.Parse(this.Text).ToString(nfi));


                return true;
            }
            catch (System.OverflowException)
            {
                // Input too big.
            }
            catch (System.FormatException)
            {
                // Debug.Fail("Input was not in a correct format.");
                // Input not formatted properly.
            }

            return false;
        }

        /// <devdoc>
        /// Copies current selection text to the clipboard, formatted according to the IncludeLiterals properties but
        /// ignoring the prompt character.
        /// Returns true if the operation succeeded, false otherwise.
        /// </devdoc>
        private bool WmCopy()
        {
            try
            {
                Clipboard.SetText(this.Text);
            }
            catch (Exception ex)
            {
                // Note: Sometimes the above operation throws but it successfully sets the
                // data in the clipboard. This usually happens when the Application's Main
                // is not attributed with [STAThread].
                if (ex is OutOfMemoryException || ex is StackOverflowException)
                {
                    throw;
                }
            }
            return true;
        }

        /// <devdoc>
        /// </devdoc>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                // Disabled operations.
                case WM_CONTEXTMENU:
                case WM_PASTE:
                    // Do nothing.
                    break;

                case WM_COPY:
                    WmCopy();
                    break;

                case WM_CUT:
                    WmCopy();
                    ResetText();
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }


        /////////////////////////////// Unsupported methods /////////////////////////////////////////////

        /// <devdoc>
        /// Unsupported method/property.
        /// </devdoc>
        [
        Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public new bool AcceptsReturn
        {
            get { return false; }
            set { }
        }

        /// <devdoc>
        /// Unsupported method/property.
        /// </devdoc>
        [
        Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public new bool AcceptsTab
        {
            get { return false; }
            set { }
        }

        /// <devdoc>
        /// Unsupported method/property.
        /// </devdoc>
        [
        Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public new event EventHandler AcceptsTabChanged
        {
            add { }
            remove { }
        }

        /// <devdoc>
        /// Unsupported method/property.
        /// </devdoc>
        [
        Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public new AutoCompleteMode AutoCompleteMode
        {
            get { return AutoCompleteMode.None; }
            set { }
        }

        /// <devdoc>
        /// Unsupported method/property.
        /// </devdoc>
        [
        Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public new AutoCompleteSource AutoCompleteSource
        {
            get { return AutoCompleteSource.None; }
            set { }
        }
        /*
       /// <devdoc>
       /// Unsupported method/property.
       /// virtual method.
       /// </devdoc>
       [
       Browsable(false),
       EditorBrowsable(EditorBrowsableState.Never),
       DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
       ]
       //public override int MaxLength
       //{
       //    get { return base.MaxLength; }
       //    set { }
       //}

       /// <devdoc>
       /// Unsupported method/property.
       /// virtual method.
       /// </devdoc>
       [
       Browsable(false),
       EditorBrowsable(EditorBrowsableState.Never),
       DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
       ]
       public override bool Multiline
       {
           get { return false; }
           set { }
       }
       */

        /// <devdoc>
        /// Unsupported method/property.
        /// </devdoc>
        [
        Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public new event EventHandler MultilineChanged
        {
            add { }
            remove { }
        }

        /// <devdoc>
        /// Unsupported method/property.
        /// virtual method.
        /// </devdoc>
        [
        EditorBrowsable(EditorBrowsableState.Never)
        ]
        protected override void OnMultilineChanged(EventArgs e)
        {
        }

        /// <devdoc>
        /// Unsupported method/property.
        /// </devdoc>
        [
        Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public new char PasswordChar
        {
            get { return '\0'; }
            set { }
        }

        [
        Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public new ScrollBars ScrollBars
        {
            get { return ScrollBars.None; }
            set { }
        }


        /// <devdoc>
        /// Unsupported method/property.
        /// </devdoc>
        [
        EditorBrowsable(EditorBrowsableState.Never)
        ]
        public new void ScrollToCaret()
        {
        }

        /// <devdoc>
        /// Unsupported method/property.
        /// </devdoc>
        [
        Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public new HorizontalAlignment TextAlign
        {
            get { return base.TextAlign; }
            set { }
        }

        /// <devdoc>
        /// Unsupported method/property.
        /// </devdoc>
        [
        EditorBrowsable(EditorBrowsableState.Never)
        ]
        public new event EventHandler TextAlignChanged
        {
            add
            {
            }
            remove
            {
            }
        }

        /// <devdoc>
        /// Unsupported method/property.
        /// </devdoc>
        [
        Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public new bool UseSystemPasswordChar
        {
            get { return false; }
            set { }
        }

        /// <devdoc>
        /// Unsupported method/property.
        /// </devdoc>
        [
        Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public new bool WordWrap
        {
            get { return false; }
            set { }
        }
    }
}
