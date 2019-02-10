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
        public static string Product { get; set; }
        public static string Name { get; set; }
        public static string AppDataPath { get; set; }

        private static readonly object _lock = new object();

        public static void Logger(string info)
        {
            lock (_lock)
            {
                using (Stream stream = new FileStream(Path.Combine(AppDataPath, $"{Product}.log"), FileMode.Append, FileAccess.Write, FileShare.None, 1024, FileOptions.WriteThrough))
                using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]: {info}");
                }
            }
        }

        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Product = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyProductAttribute>().Product;
            Name = Product + "_" + Assembly.GetExecutingAssembly().GetCustomAttribute<GuidAttribute>().Value.ToUpperInvariant();
            AppDataPath = Path.Combine(GetFolderPath(SpecialFolder.CommonApplicationData),
                Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCompanyAttribute>().Comp‌​any, Product);
            Directory.CreateDirectory(AppDataPath);
            Directory.SetCurrentDirectory(AppDataPath);

            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else if (args[0] == "/job")
            {
                var principal = "一般使用者";
                AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

                if ((Thread.CurrentPrincipal as WindowsPrincipal).IsInRole(WindowsBuiltInRole.Administrator))
                {
                    principal = "系統管理員";
                }

                var ident = WindowsIdentity.GetCurrent();
                Logger($"[{ident.Name}] [{ident.User.Value}] 啟動已排程工作〔{Name}〕{principal}");
            }
        }
    }
}