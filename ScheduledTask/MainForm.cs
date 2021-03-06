﻿using System;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TaskScheduler;

namespace ScheduledTask
{
    public partial class MainForm : Form
    {
        private WindowsIdentity ident;

        public MainForm()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            if ((Thread.CurrentPrincipal as WindowsPrincipal).IsInRole(WindowsBuiltInRole.Administrator))
            {
                Text += " **系統管理員**";
            }

            ident = WindowsIdentity.GetCurrent();
            tbCurrentUser.Text = ident.Name;
            tbSid.Text = ident.User.Value;
            ActiveControl = btnCreate;
        }

        private void btnFindWindow_Click(object sender, EventArgs e)
        {
            ElevatedPrivileges.FindWindow();
        }

        private void btnElevated_Click(object sender, EventArgs e)
        {
            // https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/cmstp
            // cmstp | Microsoft Docs
            ElevatedPrivileges.Invoke(Assembly.GetExecutingAssembly().Location);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            var service = new TaskScheduler.TaskScheduler();
            service.Connect();
            var rootFolder = service.GetFolder(@"\");
            var sb = new StringBuilder();
            sb.AppendLine($"[{Program.Name}]");

            try
            {
                IRegisteredTask task = rootFolder.GetTask(Program.Name);
                sb.AppendLine($"{task.Name}: {task.Definition.Principal.UserId}");
                sb.AppendLine();
                sb.AppendLine("是否移除此排程？");

                if (MessageBox.Show(sb.ToString(), Program.Product, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    rootFolder.DeleteTask(Program.Name, 0);
                }

                return;
            }
            catch (FileNotFoundException)
            {
                // https://docs.microsoft.com/en-us/windows/desktop/taskschd/logon-trigger-example--scripting-
                // Logon Trigger Example (Scripting) - Windows applications | Microsoft Docs

                try
                {
                    var taskDefinition = service.NewTask(0);

                    var regInfo = taskDefinition.RegistrationInfo;
                    regInfo.Description = "啟動時執行。";

                    var principal = taskDefinition.Principal;
                    principal.RunLevel = _TASK_RUNLEVEL.TASK_RUNLEVEL_HIGHEST;

                    var settings = taskDefinition.Settings;
                    settings.DisallowStartIfOnBatteries = false;
                    settings.StartWhenAvailable = true;

                    var idleSettings = settings.IdleSettings;
                    idleSettings.IdleDuration = null;
                    idleSettings.WaitTimeout = null;

                    taskDefinition.Triggers.Create(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_LOGON);

                    var action = taskDefinition.Actions.Create(_TASK_ACTION_TYPE.TASK_ACTION_EXEC) as IExecAction;
                    action.Path = Assembly.GetExecutingAssembly().Location;
                    action.Arguments = "/job";

                    // https://docs.microsoft.com/en-us/windows/desktop/services/localsystem-account
                    // LocalSystem Account - Windows applications | Microsoft Docs
                    rootFolder.RegisterTaskDefinition(Program.Name, taskDefinition, (int)_TASK_CREATION.TASK_CREATE_OR_UPDATE, @"NT AUTHORITY\SYSTEM", null, _TASK_LOGON_TYPE.TASK_LOGON_SERVICE_ACCOUNT);
                }
                catch (Exception ex)
                {
                    sb.AppendLine();
                    sb.AppendLine($"{ex.GetType().Name}: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine();
                sb.AppendLine($"{ex.GetType().Name}: {ex.Message}");
            }

            MessageBox.Show(sb.ToString(), Program.Product);
        }
    }
}