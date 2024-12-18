using System;

namespace RUINOR.WinFormsUI.TreeViewThreeState
{
    /// <summary>
    /// The available states for a ThreeStateCheckBox.
    /// </summary>
    [FlagsAttribute] 
    public enum CheckBoxState
    {
        Unchecked = 1,
        Checked = 2,
        Indeterminate = CheckBoxState.Unchecked | CheckBoxState.Checked
    }
}
