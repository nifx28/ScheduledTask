namespace ScheduledTask
{
    partial class MainForm
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lbCurrentUser = new System.Windows.Forms.Label();
            this.tbCurrentUser = new System.Windows.Forms.TextBox();
            this.tbSid = new System.Windows.Forms.TextBox();
            this.lbSid = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnElevated = new System.Windows.Forms.Button();
            this.btnFindWindow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbCurrentUser
            // 
            this.lbCurrentUser.Location = new System.Drawing.Point(12, 14);
            this.lbCurrentUser.Name = "lbCurrentUser";
            this.lbCurrentUser.Size = new System.Drawing.Size(80, 12);
            this.lbCurrentUser.TabIndex = 0;
            this.lbCurrentUser.Text = "目前使用者:";
            this.lbCurrentUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbCurrentUser
            // 
            this.tbCurrentUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCurrentUser.Location = new System.Drawing.Point(98, 12);
            this.tbCurrentUser.Multiline = true;
            this.tbCurrentUser.Name = "tbCurrentUser";
            this.tbCurrentUser.ReadOnly = true;
            this.tbCurrentUser.Size = new System.Drawing.Size(267, 16);
            this.tbCurrentUser.TabIndex = 1;
            // 
            // tbSid
            // 
            this.tbSid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbSid.Location = new System.Drawing.Point(98, 34);
            this.tbSid.Multiline = true;
            this.tbSid.Name = "tbSid";
            this.tbSid.ReadOnly = true;
            this.tbSid.Size = new System.Drawing.Size(267, 16);
            this.tbSid.TabIndex = 3;
            // 
            // lbSid
            // 
            this.lbSid.Location = new System.Drawing.Point(12, 36);
            this.lbSid.Name = "lbSid";
            this.lbSid.Size = new System.Drawing.Size(80, 12);
            this.lbSid.TabIndex = 2;
            this.lbSid.Text = "安全性識別碼:";
            this.lbSid.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCreate
            // 
            this.btnCreate.AutoSize = true;
            this.btnCreate.Location = new System.Drawing.Point(287, 56);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(79, 23);
            this.btnCreate.TabIndex = 4;
            this.btnCreate.Text = "建立排程(&C)";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnElevated
            // 
            this.btnElevated.AutoSize = true;
            this.btnElevated.Location = new System.Drawing.Point(202, 56);
            this.btnElevated.Name = "btnElevated";
            this.btnElevated.Size = new System.Drawing.Size(79, 23);
            this.btnElevated.TabIndex = 5;
            this.btnElevated.Text = "提升權限(&E)";
            this.btnElevated.UseVisualStyleBackColor = true;
            this.btnElevated.Click += new System.EventHandler(this.btnElevated_Click);
            // 
            // btnFindWindow
            // 
            this.btnFindWindow.AutoSize = true;
            this.btnFindWindow.Location = new System.Drawing.Point(117, 56);
            this.btnFindWindow.Name = "btnFindWindow";
            this.btnFindWindow.Size = new System.Drawing.Size(79, 23);
            this.btnFindWindow.TabIndex = 6;
            this.btnFindWindow.Text = "尋找視窗(&F)";
            this.btnFindWindow.UseVisualStyleBackColor = true;
            this.btnFindWindow.Click += new System.EventHandler(this.btnFindWindow_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 90);
            this.Controls.Add(this.btnFindWindow);
            this.Controls.Add(this.btnElevated);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.tbSid);
            this.Controls.Add(this.lbSid);
            this.Controls.Add(this.tbCurrentUser);
            this.Controls.Add(this.lbCurrentUser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工作排程器";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbCurrentUser;
        private System.Windows.Forms.TextBox tbCurrentUser;
        private System.Windows.Forms.TextBox tbSid;
        private System.Windows.Forms.Label lbSid;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnElevated;
        private System.Windows.Forms.Button btnFindWindow;
    }
}

