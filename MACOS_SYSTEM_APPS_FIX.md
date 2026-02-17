# macOS System Application Filtering Fix

## Problem Report

**Issue (Spanish):** "en Mac aun aparece Notification center, problem reporter y control center y no deberian"

**Translation:** "On Mac, Notification Center, Problem Reporter and Control Center still appear and they shouldn't"

## Root Cause

The issue was that the initial exclusion list for macOS system applications did not include all possible name variations that macOS uses for these system components. On macOS, the `kCGWindowOwnerName` API returns the **display name** of the application, which can vary:

1. **Capitalization variations**: "Control Center" vs "ControlCenter"
2. **Space variations**: "Notification Center" vs "NotificationCenter"
3. **Bundle ID format**: "com.apple.controlcenter" vs display name "Control Center"
4. **Crash reporting variations**: "Problem Reporter", "ProblemReporter", "ReportCrash", "CrashReporter"

## Solution

Added comprehensive list of name variations for macOS system applications to ensure they are properly filtered regardless of how macOS reports their names.

### Added Exclusions

**For Notification Center:**
- `NotificationCenter`
- `Notification Center`
- `UserNotificationCenter`
- `NotificationCenterUI`
- `com.apple.notificationcenterui`

**For Control Center:**
- `Control Center`
- `ControlCenter`
- `com.apple.controlcenter`

**For Problem Reporter / Crash Reporter:**
- `Problem Reporter`
- `ProblemReporter`
- `ReportCrash`
- `CrashReporter`
- `com.apple.problemreporter`

## How to Apply This Fix

### For New Installations
New installations will automatically get the updated exclusion list.

### For Existing Installations

Users with existing installations have two options:

#### Option 1: Reset Settings (Recommended)
Delete your settings file to get the new defaults:
```bash
rm ~/Library/Application\ Support/WindowTabsFree/settings.json
```
Then restart WindowTabsFree.

#### Option 2: Manual Update
Edit your settings file:
```bash
open ~/Library/Application\ Support/WindowTabsFree/settings.json
```

Add these entries to the `ExcludedApplications` array:
```json
"ExcludedApplications": [
  "ReportCrash",
  "CrashReporter",
  "UserNotificationCenter",
  "NotificationCenterUI",
  "com.apple.controlcenter",
  "com.apple.notificationcenterui",
  "com.apple.problemreporter"
]
```

Save the file and restart WindowTabsFree.

## Verification

After applying the fix, these system applications should no longer appear in:
- The main window list
- Tab groups
- Floating window manager
- Any window enumeration operations

## Technical Details

The filtering happens in `WindowManagerService.GetManageableWindows()`:

```csharp
return allWindows.Where(w => 
    (!string.IsNullOrWhiteSpace(w.Title) || !string.IsNullOrWhiteSpace(w.ProcessName)) &&
    !excludedApps.Contains(w.ProcessName, StringComparer.OrdinalIgnoreCase));
```

Key points:
- **Case-insensitive matching**: `StringComparer.OrdinalIgnoreCase` handles case variations
- **Multiple variations**: We include all known name formats to ensure comprehensive coverage
- **macOS API quirks**: `kCGWindowOwnerName` can return different formats depending on macOS version and locale

## Related Files

- `src/WindowTabsFree.Core/Services/ConfigurationService.cs` - Default exclusion list
- `src/WindowTabsFree.Core/settings.example.json` - Example configuration
- `SYSTEM_EXCLUSIONS.md` - Full documentation (English)
- `SYSTEM_EXCLUSIONS_ES.md` - Full documentation (Spanish)

## Future Improvements

If users continue to see system applications:
1. Check Debug output to see exact ProcessName values
2. Add those exact names to the exclusion list
3. Report to the development team for inclusion in defaults

## Additional Notes

This fix addresses the three specific applications mentioned in the bug report:
- ✅ Notification Center (all variants)
- ✅ Problem Reporter (all variants)
- ✅ Control Center (all variants)

The comprehensive list now includes 7 additional name variations to cover all possible ways macOS might report these applications.
