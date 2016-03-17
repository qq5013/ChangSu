namespace App
{
    partial class MainServer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.taskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inStockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OutStockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Monitor = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Cell = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_TaskQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Crane = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_CraneHandle = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Param = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_AddUser = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_ChangePWD = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lbLog = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_InStockTask = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_OutStock = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_CellMonitor = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_TaskQuery = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_StartCrane = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Login = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Exist = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_Close = new System.Windows.Forms.ToolStripButton();
            this.pnlTab = new System.Windows.Forms.Panel();
            this.tabForm = new System.Windows.Forms.TabControl();
            this.menuStrip1.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnlTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.taskToolStripMenuItem,
            this.ToolStripMenuItem_Monitor,
            this.ToolStripMenuItemSetup});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1284, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // taskToolStripMenuItem
            // 
            this.taskToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inStockToolStripMenuItem,
            this.OutStockToolStripMenuItem});
            this.taskToolStripMenuItem.Name = "taskToolStripMenuItem";
            this.taskToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.taskToolStripMenuItem.Text = "任务操作";
            // 
            // inStockToolStripMenuItem
            // 
            this.inStockToolStripMenuItem.Name = "inStockToolStripMenuItem";
            this.inStockToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.inStockToolStripMenuItem.Text = "入库任务";
            this.inStockToolStripMenuItem.Click += new System.EventHandler(this.inStockToolStripMenuItem_Click);
            // 
            // OutStockToolStripMenuItem
            // 
            this.OutStockToolStripMenuItem.Name = "OutStockToolStripMenuItem";
            this.OutStockToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.OutStockToolStripMenuItem.Text = "出库任务";
            this.OutStockToolStripMenuItem.Click += new System.EventHandler(this.OutStockToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem_Monitor
            // 
            this.ToolStripMenuItem_Monitor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Cell,
            this.ToolStripMenuItem_TaskQuery,
            this.ToolStripMenuItem_Crane,
            this.ToolStripMenuItem_CraneHandle});
            this.ToolStripMenuItem_Monitor.Name = "ToolStripMenuItem_Monitor";
            this.ToolStripMenuItem_Monitor.Size = new System.Drawing.Size(68, 21);
            this.ToolStripMenuItem_Monitor.Text = "调度监控";
            // 
            // ToolStripMenuItem_Cell
            // 
            this.ToolStripMenuItem_Cell.Name = "ToolStripMenuItem_Cell";
            this.ToolStripMenuItem_Cell.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_Cell.Text = "货位查询";
            this.ToolStripMenuItem_Cell.Click += new System.EventHandler(this.ToolStripMenuItem_Cell_Click);
            // 
            // ToolStripMenuItem_TaskQuery
            // 
            this.ToolStripMenuItem_TaskQuery.Name = "ToolStripMenuItem_TaskQuery";
            this.ToolStripMenuItem_TaskQuery.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_TaskQuery.Text = "任务查询";
            this.ToolStripMenuItem_TaskQuery.Click += new System.EventHandler(this.ToolStripMenuItem_TaskQuery_Click);
            // 
            // ToolStripMenuItem_Crane
            // 
            this.ToolStripMenuItem_Crane.Name = "ToolStripMenuItem_Crane";
            this.ToolStripMenuItem_Crane.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_Crane.Text = "堆垛机监控";
            this.ToolStripMenuItem_Crane.Click += new System.EventHandler(this.ToolStripMenuItem_Crane_Click);
            // 
            // ToolStripMenuItem_CraneHandle
            // 
            this.ToolStripMenuItem_CraneHandle.Name = "ToolStripMenuItem_CraneHandle";
            this.ToolStripMenuItem_CraneHandle.Size = new System.Drawing.Size(152, 22);
            this.ToolStripMenuItem_CraneHandle.Text = "堆垛机管理";
            this.ToolStripMenuItem_CraneHandle.Visible = false;
            this.ToolStripMenuItem_CraneHandle.Click += new System.EventHandler(this.ToolStripMenuItem_CraneHandle_Click);
            // 
            // ToolStripMenuItemSetup
            // 
            this.ToolStripMenuItemSetup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Param,
            this.ToolStripMenuItem_AddUser,
            this.ToolStripMenuItem_ChangePWD});
            this.ToolStripMenuItemSetup.Name = "ToolStripMenuItemSetup";
            this.ToolStripMenuItemSetup.Size = new System.Drawing.Size(44, 21);
            this.ToolStripMenuItemSetup.Text = "设定";
            // 
            // ToolStripMenuItem_Param
            // 
            this.ToolStripMenuItem_Param.Name = "ToolStripMenuItem_Param";
            this.ToolStripMenuItem_Param.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItem_Param.Text = "参数设定";
            this.ToolStripMenuItem_Param.Visible = false;
            this.ToolStripMenuItem_Param.Click += new System.EventHandler(this.ToolStripMenuItem_Param_Click);
            // 
            // ToolStripMenuItem_AddUser
            // 
            this.ToolStripMenuItem_AddUser.Name = "ToolStripMenuItem_AddUser";
            this.ToolStripMenuItem_AddUser.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItem_AddUser.Text = "添加用户";
            this.ToolStripMenuItem_AddUser.Visible = false;
            this.ToolStripMenuItem_AddUser.Click += new System.EventHandler(this.TMIAddUser_Click);
            // 
            // ToolStripMenuItem_ChangePWD
            // 
            this.ToolStripMenuItem_ChangePWD.Name = "ToolStripMenuItem_ChangePWD";
            this.ToolStripMenuItem_ChangePWD.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuItem_ChangePWD.Text = "密码修改";
            this.ToolStripMenuItem_ChangePWD.Click += new System.EventHandler(this.TMIChangePWD_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lbLog);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 411);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1284, 149);
            this.pnlBottom.TabIndex = 9;
            // 
            // lbLog
            // 
            this.lbLog.BackColor = System.Drawing.SystemColors.Window;
            this.lbLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLog.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbLog.FullRowSelect = true;
            this.lbLog.LabelWrap = false;
            this.lbLog.Location = new System.Drawing.Point(0, 0);
            this.lbLog.Name = "lbLog";
            this.lbLog.ShowGroups = false;
            this.lbLog.Size = new System.Drawing.Size(1284, 149);
            this.lbLog.TabIndex = 10;
            this.lbLog.UseCompatibleStateImageBehavior = false;
            this.lbLog.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Header";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "DateTime";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Log";
            this.columnHeader3.Width = 800;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_InStockTask,
            this.toolStripButton_OutStock,
            this.toolStripButton_CellMonitor,
            this.toolStripButton_TaskQuery,
            this.toolStripButton_StartCrane,
            this.toolStripButton_Login,
            this.toolStripButton_Exist,
            this.toolStripButton_Close});
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1284, 52);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_InStockTask
            // 
            this.toolStripButton_InStockTask.AutoSize = false;
            this.toolStripButton_InStockTask.Image = global::App.Properties.Resources.down;
            this.toolStripButton_InStockTask.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_InStockTask.Name = "toolStripButton_InStockTask";
            this.toolStripButton_InStockTask.Size = new System.Drawing.Size(60, 50);
            this.toolStripButton_InStockTask.Text = "入库任务";
            this.toolStripButton_InStockTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_InStockTask.Click += new System.EventHandler(this.toolStripButton_InStockTask_Click);
            // 
            // toolStripButton_OutStock
            // 
            this.toolStripButton_OutStock.AutoSize = false;
            this.toolStripButton_OutStock.Image = global::App.Properties.Resources.up;
            this.toolStripButton_OutStock.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_OutStock.Name = "toolStripButton_OutStock";
            this.toolStripButton_OutStock.Size = new System.Drawing.Size(60, 50);
            this.toolStripButton_OutStock.Text = "出库任务";
            this.toolStripButton_OutStock.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_OutStock.Click += new System.EventHandler(this.toolStripButton_OutStock_Click);
            // 
            // toolStripButton_CellMonitor
            // 
            this.toolStripButton_CellMonitor.AutoSize = false;
            this.toolStripButton_CellMonitor.Image = global::App.Properties.Resources.monitor;
            this.toolStripButton_CellMonitor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_CellMonitor.Name = "toolStripButton_CellMonitor";
            this.toolStripButton_CellMonitor.Size = new System.Drawing.Size(70, 50);
            this.toolStripButton_CellMonitor.Text = "货位查询";
            this.toolStripButton_CellMonitor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_CellMonitor.Click += new System.EventHandler(this.toolStripButton_CellMonitor_Click);
            // 
            // toolStripButton_TaskQuery
            // 
            this.toolStripButton_TaskQuery.AutoSize = false;
            this.toolStripButton_TaskQuery.Image = global::App.Properties.Resources.zoom;
            this.toolStripButton_TaskQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_TaskQuery.Name = "toolStripButton_TaskQuery";
            this.toolStripButton_TaskQuery.Size = new System.Drawing.Size(70, 50);
            this.toolStripButton_TaskQuery.Text = "任务查询";
            this.toolStripButton_TaskQuery.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_TaskQuery.Click += new System.EventHandler(this.toolStripButton_TaskQuery_Click);
            // 
            // toolStripButton_StartCrane
            // 
            this.toolStripButton_StartCrane.AutoSize = false;
            this.toolStripButton_StartCrane.Image = global::App.Properties.Resources.process_remove;
            this.toolStripButton_StartCrane.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_StartCrane.Name = "toolStripButton_StartCrane";
            this.toolStripButton_StartCrane.Size = new System.Drawing.Size(70, 50);
            this.toolStripButton_StartCrane.Text = "堆垛机联机";
            this.toolStripButton_StartCrane.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_StartCrane.Click += new System.EventHandler(this.toolStripButton_StartCrane_Click);
            // 
            // toolStripButton_Login
            // 
            this.toolStripButton_Login.AutoSize = false;
            this.toolStripButton_Login.Image = global::App.Properties.Resources.user;
            this.toolStripButton_Login.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Login.Name = "toolStripButton_Login";
            this.toolStripButton_Login.Size = new System.Drawing.Size(70, 50);
            this.toolStripButton_Login.Text = "用户登录";
            this.toolStripButton_Login.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_Login.Click += new System.EventHandler(this.toolStripButton_Login_Click);
            // 
            // toolStripButton_Exist
            // 
            this.toolStripButton_Exist.AutoSize = false;
            this.toolStripButton_Exist.Enabled = false;
            this.toolStripButton_Exist.Image = global::App.Properties.Resources.user_remove;
            this.toolStripButton_Exist.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Exist.Name = "toolStripButton_Exist";
            this.toolStripButton_Exist.Size = new System.Drawing.Size(70, 50);
            this.toolStripButton_Exist.Text = "注销用户";
            this.toolStripButton_Exist.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_Exist.Click += new System.EventHandler(this.toolStripButton_Exist_Click);
            // 
            // toolStripButton_Close
            // 
            this.toolStripButton_Close.AutoSize = false;
            this.toolStripButton_Close.Image = global::App.Properties.Resources.remove;
            this.toolStripButton_Close.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Close.Name = "toolStripButton_Close";
            this.toolStripButton_Close.Size = new System.Drawing.Size(60, 50);
            this.toolStripButton_Close.Text = "退出系统";
            this.toolStripButton_Close.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButton_Close.Click += new System.EventHandler(this.toolStripButton_Close_Click);
            // 
            // pnlTab
            // 
            this.pnlTab.BackColor = System.Drawing.SystemColors.Menu;
            this.pnlTab.Controls.Add(this.tabForm);
            this.pnlTab.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTab.Location = new System.Drawing.Point(0, 77);
            this.pnlTab.Name = "pnlTab";
            this.pnlTab.Size = new System.Drawing.Size(1284, 23);
            this.pnlTab.TabIndex = 14;
            this.pnlTab.Visible = false;
            // 
            // tabForm
            // 
            this.tabForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabForm.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabForm.Location = new System.Drawing.Point(0, 0);
            this.tabForm.Name = "tabForm";
            this.tabForm.SelectedIndex = 0;
            this.tabForm.Size = new System.Drawing.Size(1284, 23);
            this.tabForm.TabIndex = 6;
            this.tabForm.SelectedIndexChanged += new System.EventHandler(this.tabForm_SelectedIndexChanged);
            // 
            // MainServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 560);
            this.Controls.Add(this.pnlTab);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainServer";
            this.Text = "仓储调度监控系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem taskToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inStockToolStripMenuItem;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.ListView lbLog;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Monitor;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Cell;
        private System.Windows.Forms.ToolStripMenuItem OutStockToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_InStockTask;
        private System.Windows.Forms.ToolStripButton toolStripButton_OutStock;
        private System.Windows.Forms.ToolStripButton toolStripButton_Close;
        private System.Windows.Forms.Panel pnlTab;
        private System.Windows.Forms.TabControl tabForm;
        private System.Windows.Forms.ToolStripButton toolStripButton_StartCrane;
        private System.Windows.Forms.ToolStripButton toolStripButton_CellMonitor;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSetup;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Param;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_TaskQuery;
        private System.Windows.Forms.ToolStripButton toolStripButton_TaskQuery;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_AddUser;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_ChangePWD;
        private System.Windows.Forms.ToolStripButton toolStripButton_Login;
        private System.Windows.Forms.ToolStripButton toolStripButton_Exist;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Crane;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_CraneHandle;
    }
}