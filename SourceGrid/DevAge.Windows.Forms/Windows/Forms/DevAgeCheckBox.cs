using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevAge.Windows.Forms
{
    /// <summary>
    /// A TextBox that allows to set the type of value to edit, then you can use the Value property to read and write the specific type.
    /// </summary>
    public class DevAgeCheckBox : System.Windows.Forms.CheckBox
    {
        #region Generic validation methods
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
        }



        private DevAge.ComponentModel.Validator.IValidator mValidator = null;
        /// <summary>
        /// Gets or sets the Validator class useded to validate the value and convert the text when using the Value property.
        /// You can use the ApplyValidatorRules method to apply the settings of the Validator directly to the ComboBox, for example the list of values.
        /// </summary>
        [DefaultValue(null)]
        public DevAge.ComponentModel.Validator.IValidator Validator
        {
            get { return mValidator; }
            set
            {
                if (mValidator != value)
                {
                    if (mValidator != null)
                        mValidator.Changed -= mValidator_Changed;

                    mValidator = value;
                    mValidator.Changed += mValidator_Changed;
                    ApplyValidatorRules();
                }
            }
        }

        void mValidator_Changed(object sender, EventArgs e)
        {
            ApplyValidatorRules();
        }

        /// <summary>
        /// Apply the current Validator rules. This method is automatically fired when the Validator change.
        /// </summary>
        protected virtual void ApplyValidatorRules()
        {

        }

        /// <summary>
        /// Check if the selected value is valid based on the current validator and returns the value.
        /// </summary>
        /// <param name="convertedValue"></param>
        /// <returns></returns>
        public bool IsValidValue(out object convertedValue)
        {
            if (Validator != null)
            {
                if (Validator.IsValidObject(this.Checked, out convertedValue))
                    return true;
                else
                    return false;
            }
            else
            {
                convertedValue = this.Checked;
                return true;
            }
        }

        /// <summary>
        /// Gets or sets the typed value for the control, using the Validator class.
        /// If the Validator is ull the Text property is used.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Value
        {
            get
            {
                object val;
                if (IsValidValue(out val))
                    return val;
                else
                    throw new ArgumentOutOfRangeException("Text");
            }
            set
            {
                if (Validator != null)
                {
                    if (Validator.IsStringConversionSupported())
                    {
                        if (value == null)
                        {
                            Checked = false;
                        }
                        else
                        {
                            Checked = bool.Parse(value.ToString());
                        }
                     
                    }

                    else
                    {
                        Checked = bool.Parse(value.ToString());
                    }

                }
                else
                {
                    if (value == null)
                        Checked = false;
                    else
                        Checked = bool.Parse(value.ToString());
                }
            }
        }

        #endregion
    }
}

