# ScheduledTask

## Create Scheduled Task ##

By add *TaskScheduler* COM Type Library through **Add References** dialog box, you can use any functions provided by operating system.

## ElevatedPrivileges ##

This part of functions highly inspired by [How to bypass UAC in newer Windows versions](https://0x00-0x00.github.io/research/2018/10/31/How-to-bypass-UAC-in-newer-Windows-versions.html).

### Some handy PowerShell Script ###
```
Add-Type -TypeDefinition ([IO.File]::ReadAllText("$pwd\ElevatedPrivileges.cs")) -ReferencedAssemblies "System.Windows.Forms" -OutputAssembly "ElevatedPrivileges.dll"
```
```
[Reflection.Assembly]::Load([IO.File]::ReadAllBytes("$pwd\ElevatedPrivileges.dll"))
```
```
If (([Management.Automation.PSTypeName]'ElevatedPrivileges').Type) { [ElevatedPrivileges]::Invoke("C:\ScheduledTask\ScheduledTask.exe") }
```
```
[Convert]::ToBase64String((Get-Content -Path .\ElevatedPrivileges.dll -Encoding Byte))
```
```
[Reflection.Assembly]::Load([Convert]::FromBase64String("")) | Out-Null
```

## Bibliography ##

[Markdown Cheatsheet · adam-p/markdown-here Wiki](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet)

[Technique: CMSTP - MITRE ATT&CK™](https://attack.mitre.org/techniques/T1191/)

[Technique: Bypass User Account Control - MITRE ATT&CK™](https://attack.mitre.org/techniques/T1088/)

### Visual C# ###

[WindowsPrincipal Class (System.Security.Principal) | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/api/system.security.principal.windowsprincipal?view=netframework-4.7.2)

[WindowsIdentity Class (System.Security.Principal) | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/api/system.security.principal.windowsidentity?view=netframework-4.7.2)

[TaskFolder.RegisterTaskDefinition method - Windows applications | Microsoft Docs](https://docs.microsoft.com/en-us/windows/desktop/taskschd/taskfolder-registertaskdefinition)

[Action object - Windows applications | Microsoft Docs](https://docs.microsoft.com/en-us/windows/desktop/taskschd/action)

[Stopwatch.StartNew Method (System.Diagnostics) | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.stopwatch.startnew?view=netframework-4.7.2)

[Process.MainWindowHandle Property (System.Diagnostics) | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process.mainwindowhandle?view=netframework-4.7.2)

### Windows API ###

[Windows Data Types - Windows applications | Microsoft Docs](https://docs.microsoft.com/en-us/windows/desktop/winprog/windows-data-types)

[Marshaling Data with Platform Invoke | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/framework/interop/marshaling-data-with-platform-invoke)

[Marshaling Data with COM Interop | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/framework/interop/marshaling-data-with-com-interop)

[How to: Simulate Mouse and Keyboard Events in Code | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/framework/winforms/how-to-simulate-mouse-and-keyboard-events-in-code)

[FindWindowW function | Microsoft Docs](https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-findwindoww)

[SetForegroundWindow function | Microsoft Docs](https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-setforegroundwindow)

[ShowWindow function | Microsoft Docs](https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-showwindow)

[SendMessage function | Microsoft Docs](https://docs.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-sendmessage)

[WM_KEYDOWN message - Windows applications | Microsoft Docs](https://docs.microsoft.com/en-us/windows/desktop/inputdev/wm-keydown)

[WM_KEYUP message - Windows applications | Microsoft Docs](https://docs.microsoft.com/en-us/windows/desktop/inputdev/wm-keyup)

[Virtual-Key Codes - Windows applications | Microsoft Docs](https://docs.microsoft.com/en-us/windows/desktop/inputdev/virtual-key-codes)

[Convert.ToBase64String Method (System) | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/api/system.convert.tobase64string?view=netframework-4.7.2)

[Convert.FromBase64String(String) Method (System) | Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/api/system.convert.frombase64string?view=netframework-4.7.2)

### PowerShell ###

[Get-Alias | Microsoft.PowerShell.Utility | Microsoft Docs](https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.utility/get-alias?view=powershell-6)

[Get-Content | Microsoft.PowerShell.Management | Microsoft Docs](https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.management/get-content?view=powershell-6)

[Out-Null | Microsoft.PowerShell.Core | Microsoft Docs](https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.core/out-null?view=powershell-6)

[Add-Type | Microsoft.PowerShell.Utility | Microsoft Docs](https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.utility/add-type?view=powershell-6)
