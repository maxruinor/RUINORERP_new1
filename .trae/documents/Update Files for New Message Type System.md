## Update Plan for Message Type System

### 1. MessageListControl.cs
**Issue**: The `NavigateToBusiness` method (lines 564-583) uses deprecated MessageType values (Approve, Task, Notice, Reminder)

**Fix**: 
- Update the switch statement to use the new 3-category message types
- Map the navigation logic to the appropriate business handling based on message content and biz data
- Ensure backward compatibility by checking message content for specific business types

### 2. TaskVoiceReminder.cs
**Issue**: The `GenerateVoiceText` method (lines 125-145) uses deprecated MessageType values

**Fix**:
- Update the switch statement to use only the new 3-category message types (Popup, Business, System)
- Simplify the voice text generation logic based on the new categories
- Remove all cases for deprecated message types

### 3. MessageType.cs
**Status**: Already updated to use the new 3-category system
**No changes needed**

### Implementation Steps
1. Update `MessageListControl.cs` - Modify the `NavigateToBusiness` method to use new message types
2. Update `TaskVoiceReminder.cs` - Modify the `GenerateVoiceText` method to use new message types
3. Verify changes compile correctly
4. Ensure backward compatibility is maintained

### Expected Outcome
- All files will use the new 3-category message type system consistently
- No compilation errors related to deprecated MessageType values
- Voice reminders will work correctly with the new message types
- Message navigation will continue to function properly