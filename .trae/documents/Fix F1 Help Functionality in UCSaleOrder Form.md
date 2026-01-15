## Problem Analysis

The F1 help functionality doesn't work when focusing on the order number input field (`txtOrderNo`) in the UCSaleOrder form. After analyzing the code, I've identified the root cause:

1. The `ProcessCmdKey` method in `BaseBillEdit.cs` handles F1 key presses but has two issues:
   - It only works if `toolTipBase.Active` is `true`
   - The `ProcessHelpInfo` method only handles KryptonTextBox controls, not regular TextBox controls

2. The `txtOrderNo` control in UCSaleOrder is a regular TextBox, not a KryptonTextBox, so the current F1 handling doesn't recognize it

## Solution

Modify the `ProcessCmdKey` method in `BaseBillEdit.cs` to use the new help system instead of the legacy `ProcessHelpInfo` method. This will:

1. Remove the dependency on `toolTipBase.Active`
2. Support both Krypton and native WinForms controls
3. Use the already implemented `ShowContextHelp` method which leverages the new `HelpManager`

## Implementation Steps

1. **Update `ProcessCmdKey` method in `BaseBillEdit.cs`**: 
   - Remove the `toolTipBase.Active` check
   - Call `ShowContextHelp()` when F1 is pressed
   - Return `true` to indicate the key was handled
   - Call `base.ProcessCmdKey` for other keys

2. **Ensure `ShowContextHelp` is properly implemented**: 
   - Verify it uses `HelpManager.Instance.GetFocusedControl()` to correctly identify the focused control
   - Ensure it calls `HelpManager.Instance.ShowControlHelp()` or `ShowFormHelp()` accordingly

## Expected Outcome

After the fix, pressing F1 when focusing on any control (including regular TextBox controls like `txtOrderNo`) will:
1. Trigger the `ProcessCmdKey` method
2. Call `ShowContextHelp()`
3. Get the actual focused control using the new help system
4. Display the appropriate help information

This will ensure consistent F1 help functionality across all controls in the application.