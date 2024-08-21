using RUINORERP.Model;
using RUINORERP.UI.UCSourceGrid;
using SourceLibrary.Windows.Forms;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;


namespace SourceGrid2.DataModels
{
    public class EditorTextBoxButtonQuery : EditorControlBase
    {
        #region Constructor
        /// <summary>
        /// Construct a Model. Based on the Type specified the constructor populate AllowNull, DefaultValue, TypeConverter, StandardValues, StandardValueExclusive
        /// </summary>
        /// <param name="p_Type">The type of this model</param>
        public EditorTextBoxButtonQuery(Type p_Type) : base(p_Type)
        {
        }
        #endregion

        #region Edit Control
        public override Control CreateEditorControl()
        {
            //TextBoxTypedButtonQuery l_ComboBox = new TextBoxTypedButtonQuery();
            TextBoxTypedButtonQuery l_ComboBox = new TextBoxTypedButtonQuery();
            l_ComboBox.DialogOpen += L_ComboBox_DialogOpen;
            l_ComboBox.TextBox.BorderStyle = BorderStyle.None;
            return l_ComboBox;
        }

        private void L_ComboBox_DialogOpen(object sender, EventArgs e)
        {
            using (QueryFormGeneric dg = new QueryFormGeneric())
            {
                dg.StartPosition = FormStartPosition.CenterScreen;
                if (dg.ShowDialog() == DialogResult.OK)
                {
                    TextBoxTypedButtonQuery l_ComboBox = sender as TextBoxTypedButtonQuery;
                    //l_ComboBox.Tag = dg.QueryObject;//TagValue
                    l_ComboBox.TextBox.Value = dg.prodQuery.QueryValue;
                    l_ComboBox.TextBox.Text = dg.prodQuery.QueryValue;
                    l_ComboBox.TextBox.TagValue = dg.prodQuery.QueryObjects;


                }
            }
        }


        [Obsolete("Use GetEditorTextBoxTypedButton(GridSubPanel)")]
        public virtual SourceLibrary.Windows.Forms.TextBoxTypedButtonQuery GetEditorTextBoxTypedButton(GridVirtual p_Grid)
        {
            return (SourceLibrary.Windows.Forms.TextBoxTypedButtonQuery)GetEditorControl(p_Grid);
        }

        public virtual SourceLibrary.Windows.Forms.TextBoxTypedButtonQuery GetEditorTextBoxTypedButton(GridSubPanel p_GridPanel)
        {
            return (SourceLibrary.Windows.Forms.TextBoxTypedButtonQuery)GetEditorControl(p_GridPanel);
        }


        #endregion

        /// <summary>
        /// Start editing the cell passed. Do not call this method for start editing a cell, you must use Cell.StartEdit.
        /// </summary>
        /// <param name="p_Cell">Cell to start edit</param>
        /// <param name="p_Position">Editing position(Row/Col)</param>
        /// <param name="p_StartEditValue">Can be null(in this case use the p_cell.Value</param>
        public override void InternalStartEdit(Cells.ICellVirtual p_Cell, Position p_Position, object p_StartEditValue)
        {
            base.InternalStartEdit(p_Cell, p_Position, p_StartEditValue);

            if (EnableEdit == false)
                return;
            TextBoxTypedButtonQuery l_Combo = GetEditorTextBoxTypedButton(p_Cell.Grid.PanelAtPosition(p_Position));

            l_Combo.Validator = this;

            l_Combo.EnableEscapeKeyUndo = false;
            l_Combo.EnableEnterKeyValidate = false;
            l_Combo.EnableLastValidValue = false;
            l_Combo.EnableAutoValidation = false;

            if (p_StartEditValue is string && IsStringConversionSupported())
            {
                l_Combo.TextBox.Text = SourceLibrary.Windows.Forms.TextBoxTyped.ValidateCharactersString((string)p_StartEditValue, l_Combo.TextBox.ValidCharacters, l_Combo.TextBox.InvalidCharacters);
                if (l_Combo.TextBox.Text != null)
                    l_Combo.TextBox.SelectionStart = l_Combo.TextBox.Text.Length;
                else
                    l_Combo.TextBox.SelectionStart = 0;
            }
            else
            {
                l_Combo.Value = p_Cell.GetValue(p_Position);
                l_Combo.SelectAllTextBox();
            }
        }



        /// <summary>
        /// Returns the value inserted with the current editor control
        /// </summary>
        /// <returns></returns>
        public override object GetEditedValue()
        {
            return GetEditorTextBoxTypedButton(EditCell.Grid.PanelAtPosition(EditPosition)).Value;
        }

        public override object GetEditedTagValue()
        {
            return GetEditorTextBoxTypedButton(EditCell.Grid.PanelAtPosition(EditPosition)).TagValue;
        }
    }
}

