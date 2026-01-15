using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Krypton.Toolkit;
using ComponentFactory.Krypton.Toolkit;

namespace RUINORERP.UI.HelpSystem.Extensions
{
    /// <summary>
    /// Extension methods for enhancing help system support for Krypton Toolkit controls
    /// </summary>
    public static class KryptonHelpExtensions
    {
        /// <summary>
        /// Gets the actual focused control within Krypton controls by traversing their nested structure
        /// </summary>
        /// <param name="control">The control to check</param>
        /// <returns>The actual focused control, or null if not found</returns>
        public static Control GetActualFocusedControl(this Control control)
        {
            if (control == null)
                return null;

            // If the control is focused and not a Krypton container, return it directly
            if (control.Focused && !control.IsKryptonContainerControl())
                return control;

            // Check if it's a Krypton control with internal focused control
            if (control.IsKryptonControl())
            {
                var actualFocused = GetKryptonInternalFocusedControl(control);
                if (actualFocused != null)
                    return actualFocused;
            }

            // Recursively check child controls
            foreach (Control child in control.Controls)
            {
                var focused = GetActualFocusedControl(child);
                if (focused != null)
                    return focused;
            }

            return null;
        }

        /// <summary>
        /// Gets all controls from a control hierarchy, including Krypton control internal components
        /// </summary>
        /// <param name="control">The root control to start from</param>
        /// <returns>List of all controls in the hierarchy</returns>
        public static IEnumerable<Control> GetAllKryptonControls(this Control control)
        {
            if (control == null)
                yield break;

            // Yield the control itself
            yield return control;

            // Get Krypton child controls including internal ones
            var childControls = control.GetKryptonChildControls();

            // Recursively yield all child controls
            foreach (var child in childControls)
            {
                foreach (var grandChild in child.GetAllKryptonControls())
                {
                    yield return grandChild;
                }
            }
        }

        /// <summary>
        /// Determines if a control is a Krypton Toolkit control
        /// </summary>
        /// <param name="control">The control to check</param>
        /// <returns>True if the control is a Krypton control, otherwise false</returns>
        public static bool IsKryptonControl(this Control control)
        {
            if (control == null)
                return false;

            // Check if the control type starts with Krypton or is in Krypton namespace
            return control.GetType().FullName?.StartsWith("ComponentFactory.Krypton") == true ||
                   control.GetType().Name.StartsWith("Krypton");
        }

        /// <summary>
        /// Determines if a Krypton control is a container type that might have internal focused controls
        /// </summary>
        /// <param name="control">The control to check</param>
        /// <returns>True if the control is a Krypton container, otherwise false</returns>
        public static bool IsKryptonContainerControl(this Control control)
        {
            if (!control.IsKryptonControl())
                return false;

            var controlType = control.GetType();
            return controlType.Name.Contains("Panel") ||
                   controlType.Name.Contains("Group") ||
                   controlType.Name.Contains("Container") ||
                   controlType.Name.Contains("Navigator") ||
                   controlType.Name.Contains("Workspace") ||
                   controlType.Name == "KryptonForm" ||
                   controlType.Name == "KryptonUserControl";
        }

        /// <summary>
        /// Gets all child controls of a Krypton control, including internal components
        /// </summary>
        /// <param name="control">The Krypton control</param>
        /// <returns>List of child controls including internal ones</returns>
        public static IEnumerable<Control> GetKryptonChildControls(this Control control)
        {
            if (control == null)
                yield break;

            // Return regular child controls first
            foreach (Control child in control.Controls)
            {
                yield return child;
            }

            // Get internal controls for specific Krypton controls
            if (control.IsKryptonControl())
            {
                var internalControls = GetInternalKryptonControls(control);
                foreach (var internalControl in internalControls)
                {
                    yield return internalControl;
                }
            }
        }

        /// <summary>
        /// Gets the internal focused control from specific Krypton controls using reflection
        /// </summary>
        /// <param name="kryptonControl">The Krypton control</param>
        /// <returns>The internal focused control if found, otherwise null</returns>
        private static Control GetKryptonInternalFocusedControl(Control kryptonControl)
        {
            if (kryptonControl == null || !kryptonControl.IsKryptonControl())
                return null;

            try
            {
                var controlType = kryptonControl.GetType();
                object internalControl = null;

                // Handle specific Krypton control types
                switch (controlType.Name)
                {
                    case "KryptonTextBox":
                        internalControl = GetPrivateFieldValue(kryptonControl, "_textBox");
                        break;
                    case "KryptonComboBox":
                        internalControl = GetPrivateFieldValue(kryptonControl, "_comboBox");
                        break;
                    case "KryptonRichTextBox":
                        internalControl = GetPrivateFieldValue(kryptonControl, "_richTextBox");
                        break;
                    case "KryptonMaskedTextBox":
                        internalControl = GetPrivateFieldValue(kryptonControl, "_maskedTextBox");
                        break;
                    case "KryptonCheckedListBox":
                        internalControl = GetPrivateFieldValue(kryptonControl, "_checkedListBox");
                        break;
                    case "KryptonListBox":
                        internalControl = GetPrivateFieldValue(kryptonControl, "_listBox");
                        break;
                    case "KryptonNumericUpDown":
                        internalControl = GetPrivateFieldValue(kryptonControl, "_numericUpDown");
                        break;
                    case "KryptonDomainUpDown":
                        internalControl = GetPrivateFieldValue(kryptonControl, "_domainUpDown");
                        break;
                    case "KryptonDateTimePicker":
                        internalControl = GetPrivateFieldValue(kryptonControl, "_dateTimePicker");
                        break;
                    case "KryptonDataGridView":
                        internalControl = GetPrivateFieldValue(kryptonControl, "_dataGridView");
                        break;
                    // Add more Krypton control types as needed
                }

                return internalControl as Control;
            }
            catch (Exception)
            {
                // If reflection fails, return null
                return null;
            }
        }

        /// <summary>
        /// Gets internal controls from Krypton controls using reflection
        /// </summary>
        /// <param name="kryptonControl">The Krypton control</param>
        /// <returns>List of internal controls</returns>
        private static IEnumerable<Control> GetInternalKryptonControls(Control kryptonControl)
        {
            if (kryptonControl == null)
                yield break;

            try
            {
                var controlType = kryptonControl.GetType();
                var fields = controlType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (var field in fields)
                {
                    // Look for fields that are controls or collections of controls
                    if (typeof(Control).IsAssignableFrom(field.FieldType))
                    {
                        var control = field.GetValue(kryptonControl) as Control;
                        if (control != null)
                        {
                            yield return control;
                        }
                    }
                    else if (typeof(IEnumerable<Control>).IsAssignableFrom(field.FieldType))
                    {
                        var controls = field.GetValue(kryptonControl) as IEnumerable<Control>;
                        if (controls != null)
                        {
                            foreach (var control in controls)
                            {
                                yield return control;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // If reflection fails, return empty
                yield break;
            }
        }

        /// <summary>
        /// Helper method to get private field values using reflection
        /// </summary>
        /// <param name="obj">The object to get field from</param>
        /// <param name="fieldName">The name of the private field</param>
        /// <returns>The field value</returns>
        private static object GetPrivateFieldValue(object obj, string fieldName)
        {
            if (obj == null)
                return null;

            try
            {
                var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                return field?.GetValue(obj);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the parent control that should be used for help resolution
        /// </summary>
        /// <param name="control">The control to check</param>
        /// <returns>The appropriate parent control for help resolution</returns>
        public static Control GetHelpParentControl(this Control control)
        {
            if (control == null)
                return null;

            // If it's an internal Krypton control, return its parent Krypton control
            if (control.Parent != null && control.Parent.IsKryptonControl() && control != control.Parent)
            {
                // Check if this is an internal control of a Krypton control
                var parentType = control.Parent.GetType();
                var controlType = control.GetType();

                // If the parent is a Krypton control and this is not a direct child control added by the user
                if (parentType.Name.StartsWith("Krypton") && !controlType.Name.StartsWith("Krypton"))
                {
                    return control.Parent;
                }
            }

            return control;
        }

        /// <summary>
        /// Gets the control's name in a format suitable for help key generation, handling Krypton control naming conventions
        /// </summary>
        /// <param name="control">The control</param>
        /// <returns>The formatted control name</returns>
        public static string GetHelpControlName(this Control control)
        {
            if (control == null)
                return string.Empty;

            // If it's an internal Krypton control, use the parent's name
            var helpControl = control.GetHelpParentControl();
            return helpControl.Name;
        }
    }
}