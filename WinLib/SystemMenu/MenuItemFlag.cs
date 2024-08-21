using System;
using System.Collections.Generic;
using System.Text;

namespace WinLib
{
    public enum MenuItemFlag
    {
        MF_UNCHECKED = 0x00000000, // ... IS NOT CHECKED
        MF_STRING = 0x00000000, // ... CONTAINS A STRING AS LABEL
        MF_DISABLED = 0x00000002, // ... IS DISABLED
        MF_GRAYED = 0x00000001, // ... IS GRAYED
        MF_CHECKED = 0x00000008, // ... IS CHECKED
        MF_POPUP = 0x00000010, // ... IS A POPUP MENU. PASS THE

        // MENU HANDLE OF THE POPUP
        // MENU INTO THE ID PARAMETER.

        MF_BARBREAK = 0x00000020, // ... IS A BAR BREAK
        MF_BREAK = 0x00000040, // ... IS A BREAK
        MF_BYPOSITION = 0x00000400, // ... IS IDENTIFIED BY THE POSITION
        MF_BYCOMMAND = 0x00000000, // ... IS IDENTIFIED BY ITS ID
        MF_SEPARATOR = 0x00000800 // ... IS A SEPERATOR (STRING AND
    }
}
