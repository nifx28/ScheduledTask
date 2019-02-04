using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Environment;

namespace ScheduledTask
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else if (args[0] == "/job")
            {
                var product = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyProductAttribute>().Product;
                var name = product + "_" + Assembly.GetExecutingAssembly().GetCustomAttribute<GuidAttribute>().Value.ToUpperInvariant();
                var appData = Path.Combine(GetFolderPath(SpecialFolder.CommonApplicationData),
                    Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCompanyAttribute>().Comp‌​any, product);
                Directory.CreateDirectory(appData);
                Directory.SetCurrentDirectory(appData);

                var principal = "一般使用者";
                AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

                if ((Thread.CurrentPrincipal as WindowsPrincipal).IsInRole(WindowsBuiltInRole.Administrator))
                {
                    principal = "系統管理員";
                }

                var ident = WindowsIdentity.GetCurrent();

                using (Stream stream = new FileStream(Path.Combine(appData, $"{product}.log"), FileMode.Append, FileAccess.Write, FileShare.None, 1024, FileOptions.WriteThrough))
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: [{ident.Name}] [{ident.User.Value}] 啟動已排程工作〔{name}〕{principal}");
                }
            }
        }
    }
}