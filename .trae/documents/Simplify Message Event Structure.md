# Simplify Message Event Structure

## Problem
After reducing the message types from 17 to 3 core categories (Popup, Business, System), the MessageService class still maintains 5 events:
- PopupMessageReceived
- BusinessMessageReceived
- DepartmentMessageReceived
- BroadcastMessageReceived
- SystemNotificationReceived

The issue is that DepartmentMessageReceived and BroadcastMessageReceived are based on recipient categories rather than message types, making the event structure inconsistent with our simplified message type system.

## Solution
Align the event structure with the new message type system by:

1. **Keep only events that correspond to the new message types**:
   - PopupMessageReceived
   - BusinessMessageReceived
   - SystemNotificationReceived

2. **Remove events based on recipient categories**:
   - DepartmentMessageReceived
   - BroadcastMessageReceived

3. **Update message handling logic**:
   - Modify the EnhancedMessageManager to only subscribe to the relevant events
   - Update the message processing to use message type for routing instead of separate events

4. **Update event invocation**:
   - Ensure all messages are routed through the appropriate event based on their message type

## Implementation Steps

1. **Update EnhancedMessageManager.cs**:
   - Remove subscriptions to DepartmentMessageReceived and BroadcastMessageReceived
   - Update the message processing logic to handle all message types through the remaining events

2. **Update MessageService.cs**:
   - Remove DepartmentMessageReceived and BroadcastMessageReceived events
   - Remove the corresponding OnXXXMessageReceived methods
   - Ensure all messages are routed through the appropriate event based on their message type

3. **Update MessageCommandHandler.cs**:
   - Ensure messages are sent with the correct message type
   - Remove any references to the deprecated events

4. **Update any other files**:
   - Check for any other references to the deprecated events and update them

## Benefits
- Consistent event structure aligned with the new message type system
- Reduced complexity in message handling
- Easier maintenance and understanding
- Clear mapping between message types and events

This will simplify the message system further, making it more consistent and easier to maintain while keeping the core functionality intact.