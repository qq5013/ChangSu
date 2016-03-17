using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MCP;
using BLL;
using App.View;
using Util;
using DataGridViewAutoFilter;

namespace App
{
    public partial class MainServer : Form
    {
        private bool IsActiveForm = false;
        public bool IsActiveTab = false;
        private Context context = null;
      

        private BLL.BLLBase bll;
        private string CurrentUser = "";
        private System.Timers.Timer tmCraneErr = new System.Timers.Timer();
        private DataTable dtCrane;
        public MainServer()
        {
            InitializeComponent();
        }

        

     
      
        void Logger_OnLog(MCP.LogEventArgs args)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new LogEventHandler(Logger_OnLog), args);
            }
            else
            {
                lock (lbLog)
                {
                    string msg1 = string.Format("[{0}]", args.LogLevel);
                    string msg2 = string.Format("{0}", DateTime.Now);
                    string msg3 = string.Format("{0} ", args.Message);
                    this.lbLog.BeginUpdate();
                    ListViewItem item = new ListViewItem(new string[] { msg1, msg2, msg3 });
                    
                    if (msg1.Contains("[ERROR]"))
                    {
                        //item.ForeColor = Color.Red;
                        item.BackColor = Color.Red;
                    }
                    lbLog.Items.Insert(0, item);
                    this.lbLog.EndUpdate();
                    WriteLoggerFile(msg1 + msg2 + msg3);
                    InsertDataLog(args.LogLevel.ToString(), args.Message);
                    
                }
            }
        }

        private void CreateDirectory(string directoryName)
        {
            if (!System.IO.Directory.Exists(directoryName))
                System.IO.Directory.CreateDirectory(directoryName);
        }

        private void WriteLoggerFile(string text)
        {
            try
            {
                string path = "";
                CreateDirectory("日志");
                path = "日志";
                path = path + @"/" + DateTime.Now.ToString().Substring(0, 4).Trim();
                CreateDirectory(path);
                path = path + @"/" + DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 7).Trim();
                path = path.TrimEnd(new char[] { '-' });
                CreateDirectory(path);
                path = path + @"/" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                System.IO.File.AppendAllText(path, string.Format("{0}", text + "\r\n"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void InsertDataLog(string LogLevel, string text)
        {
            try
            {
                bll.ExecNonQuery("WCS.InsertDataLog", new DataParameter[] { new DataParameter("{0}", LogLevel), new DataParameter("{1}", CurrentUser), new DataParameter("{2}", text) });
            }
            catch (Exception ex)
            {
 
            }
        }
        #region 公共方法
        /// <summary>
        /// 打开一个窗体

        /// </summary>
        /// <param name="frm"></param>
        private void ShowForm(Form frm)
        {
            if (OpenOnce(frm))
            {
                frm.MdiParent = this;
                ((View.BaseForm)frm).Context = context;
                frm.WindowState = FormWindowState.Maximized;
                frm.Show();
                
                AddTabPage(frm.Handle.ToString(), frm.Text);
            }
        }
        /// <summary>
        /// 判断窗体是否已打开
        /// </summary>
        /// <param name="frm"></param>
        /// <returns></returns>
        private bool OpenOnce(Form frm)
        {
            foreach (Form mdifrm in this.MdiChildren)
            {
                int index = mdifrm.Text.IndexOf(" ");
                if (index > 0)
                {
                    if (frm.Name == mdifrm.Name && frm.Text == mdifrm.Text.Substring(0, index))
                    {
                        mdifrm.Activate();
                        return false;
                    }
                }
                else
                {
                    if (frm.Name == mdifrm.Name && frm.Text == mdifrm.Text)
                    {
                        mdifrm.Activate();
                        return false;
                    }
                }
            }
            return true;

        }

        private Form GetFormByName(string name)
        {
            Form frm = null;
            foreach (Form mdifrm in this.MdiChildren)
            {
                if (mdifrm.Name == name)
                {
                    frm = mdifrm;
                    break;
                }
            }
            return frm;
            
        }
        
        private void AddTabPage(string strKey, string strText)
        {
            IsActiveForm = true;
            TabPage tab = new TabPage();
            tab.Name = strKey.ToString();
            tab.Text = strText;
            tabForm.TabPages.Add(tab);
            tabForm.SelectedTab = tab;
            this.pnlTab.Visible = true;
            IsActiveForm = false;
        }
        
        public void SetActiveTab(string strKey, bool blnActive)
        {
            foreach (TabPage tab in this.tabForm.TabPages)
            {
                if (tab.Name == strKey)
                {
                    IsActiveForm = true;
                    if (blnActive)
                        tabForm.SelectedTab = tab;
                    else
                    {
                        tabForm.TabPages.Remove(tab);
                        if (this.MdiChildren.Length > 1)
                            this.pnlTab.Visible = true;
                        else
                            this.pnlTab.Visible = false;
                    }
                    IsActiveForm = false; ;
                }
            }
        }
        private void tabForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsActiveForm)
                return;
            foreach (Form mdifrm in this.MdiChildren)
            {
                if (mdifrm.Handle.ToInt32() == int.Parse(((TabControl)sender).SelectedTab.Name))
                {
                    IsActiveTab = true;
                    mdifrm.WindowState = FormWindowState.Maximized;
                   // mdifrm.Activate();
                    IsActiveTab = false;
                }
            }
        }
        #endregion

        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                bll = new BLLBase();
                lbLog.Scrollable = true;
                Logger.OnLog += new LogEventHandler(Logger_OnLog);
                context = new Context();

               

                ContextInitialize initialize = new ContextInitialize();
                initialize.InitializeContext(context);

                Dispatching.LED2008.LEDUtil.Initialize(context.Attributes["LedCollection"].ToString());

                toolStripButton_StartCrane_Click(null, null);

                dtCrane = bll.FillDataTable("WCS.SelectWCSCRANE");

                System.Threading.Thread.Sleep(1000);
                tmCraneErr.Interval = 3000;
                tmCraneErr.Elapsed += new System.Timers.ElapsedEventHandler(tmCraneWorker);
                tmCraneErr.Start();

            }
            catch (Exception ee)
            {
                Logger.Error("初始化处理失败请检查配置，原因：" + ee.Message);
            }



            

        }
        

       
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("您确定要退出调度系统吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                Dispatching.LED2008.LEDUtil.Release();
                Logger.Info("退出系统");
                System.Environment.Exit(0);
            }
            else
                e.Cancel = true;
        }

        
        private void ToolStripMenuItem_Param_Click(object sender, EventArgs e)
        {
            App.View.Param.ParameterForm f = new App.View.Param.ParameterForm();
            ShowForm(f);
        }


        #region 入库任务
        private void toolStripButton_InStockTask_Click(object sender, EventArgs e)
        {
            App.View.Task.frmInStock f = new View.Task.frmInStock();
            ShowForm(f);
        }

        private void inStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.View.Task.frmInStock f = new View.Task.frmInStock();
            ShowForm(f);
        }
        #endregion

        #region 出库任务
        private void toolStripButton_OutStock_Click(object sender, EventArgs e)
        {
            App.View.Task.frmOutStock f = new View.Task.frmOutStock();
            ShowForm(f);
        }

        private void OutStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App.View.Task.frmOutStock f = new View.Task.frmOutStock();
            ShowForm(f);
        }
        #endregion

        #region 货位监控
        private void toolStripButton_CellMonitor_Click(object sender, EventArgs e)
        {
            App.View.Dispatcher.frmCellQuery f = new App.View.Dispatcher.frmCellQuery();
            ShowForm(f);
        }

        private void ToolStripMenuItem_Cell_Click(object sender, EventArgs e)
        {
            App.View.Dispatcher.frmCellQuery f = new App.View.Dispatcher.frmCellQuery();
            ShowForm(f);
        }
        #endregion

        #region 任务查询
        private void toolStripButton_TaskQuery_Click(object sender, EventArgs e)
        {
            View.Dispatcher.frmTaskQuery f = new View.Dispatcher.frmTaskQuery();
            ShowForm(f);
        }

        private void ToolStripMenuItem_TaskQuery_Click(object sender, EventArgs e)
        {
            View.Dispatcher.frmTaskQuery f = new View.Dispatcher.frmTaskQuery();
            ShowForm(f);
        }
        #endregion 

        #region 任务测试
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            View.Task.frmCraneTask f = new View.Task.frmCraneTask();
            ShowForm(f);
        }
        #endregion

        #region  堆垛机连机
        private void toolStripButton_StartCrane_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.toolStripButton_StartCrane.Text == "堆垛机联机")
                {
                    context.ProcessDispatcher.WriteToProcess("CraneProcess", "Run", 1);
                    this.toolStripButton_StartCrane.Image = App.Properties.Resources.process_accept;
                    this.toolStripButton_StartCrane.Text = "堆垛机脱机";
                }
                else
                {
                    context.ProcessDispatcher.WriteToProcess("CraneProcess", "Run", 0);
                    this.toolStripButton_StartCrane.Image = App.Properties.Resources.process_remove;
                    this.toolStripButton_StartCrane.Text = "堆垛机联机";
                }
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message);
            }
        }
        #endregion

        #region 退出系统
        private void toolStripButton_Close_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("您确定要退出调度系统吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                Dispatching.LED2008.LEDUtil.Release();
                Logger.Info("退出系统");
                System.Environment.Exit(0);
            }
        }
        #endregion

         

        #region 用户处理
        //用户登录
        private void toolStripButton_Login_Click(object sender, EventArgs e)
        {
            App.View.Param.frmLogin frm = new View.Param.frmLogin();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                toolStripButton_Exist.Enabled = true;
                toolStripButton_Login.Enabled = false;
                CurrentUser = frm.UserID;
                if (CurrentUser.ToLower() == "admin")
                {
                    ToolStripMenuItem_Param.Visible = true;
                    ToolStripMenuItem_AddUser.Visible = true;
                    ToolStripMenuItem_CraneHandle.Visible = true;
                    
                }

                Form frmMon = GetFormByName("frmMonitor");
                if (frmMon != null)
                {
                    ((frmMonitor)frmMon).SetBtnEnabled(true);
                }
                else
                {
                    Form frmMon2 = GetFormByName("frmMonitor2");
                    if (frmMon2 != null)
                    {
                        ((frmMonitor2)frmMon2).SetBtnEnabled(true);
                    }
                }

            }
        }
        //用户注销
        private void toolStripButton_Exist_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("您确定要退出用户登录？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                CurrentUser = "";
                toolStripButton_Exist.Enabled = false;
                toolStripButton_Login.Enabled = true;
                ToolStripMenuItem_Param.Visible = false;
                ToolStripMenuItem_AddUser.Visible = false;
                ToolStripMenuItem_CraneHandle.Visible = false;

                Form frmMon = GetFormByName("frmMonitor");
                if (frmMon != null)
                {
                    ((frmMonitor)frmMon).SetBtnEnabled(false);
                }
                else
                {
                    Form frmMon2 = GetFormByName("frmMonitor2");
                    if (frmMon2 != null)
                    {
                        ((frmMonitor2)frmMon2).SetBtnEnabled(false);
                    }
                }
            }
        }
        //添加用户
        private void TMIAddUser_Click(object sender, EventArgs e)
        {
            View.Param.frmAddUser frm = new View.Param.frmAddUser();
            frm.ShowDialog();

        }
        //用户密码修改
        private void TMIChangePWD_Click(object sender, EventArgs e)
        {
            View.Param.frmChangePWD frm = new View.Param.frmChangePWD();
            frm.ShowDialog();
        }
        #endregion

        #region 堆垛机监控，处理
        private void ToolStripMenuItem_Crane_Click(object sender, EventArgs e)
        {
            if (Screen.PrimaryScreen.WorkingArea.Width > 1366)
            {
                View.frmMonitor2 f = new View.frmMonitor2();
                ShowForm(f);
            }
            else
            {
                View.frmMonitor f = new View.frmMonitor();
                ShowForm(f);
            }
        }

        private void ToolStripMenuItem_CraneHandle_Click(object sender, EventArgs e)
        {
            App.View.Param.frmCraneHandle f = new App.View.Param.frmCraneHandle();
            ShowForm(f);
        }
        #endregion


        #region 堆垛机故障跟踪
        private void tmCraneWorker(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                tmCraneErr.Stop();

                for (int i = 1; i <= dtCrane.Rows.Count; i++)
                {
                    if (dtCrane.Rows[i - 1]["ISENABLED"].ToString() == "0")
                        continue;

                    string serviceName = "CranePLC" + i;
                    int ErrCode = int.Parse(ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService(serviceName, "nAlarmCode")).ToString());

                    if (ErrCode == 0)
                        continue;
                    if (ErrCode == 505)
                    {

                        string plcTaskNo = ConvertStringChar.BytesToString(ObjectUtil.GetObjects(context.ProcessDispatcher.WriteToService(serviceName, "ReadTaskNo")));
                        DataTable dtTask = bll.FillDataTable("WCS.SelectWmsTask", new DataParameter[] { new DataParameter("{0}", string.Format("TASKID='{0}' and ASRSID='{1}'", plcTaskNo, i)) });
                        if (dtTask.Rows.Count > 0)
                        {
                            string TaskType = dtTask.Rows[0]["TASKTYPE"].ToString();
                            string plcPalletCode = dtTask.Rows[0]["PALLETID"].ToString();
                            DataTable dtErr = bll.FillDataTable("WCS.SelectWmsSend", new DataParameter[] { new DataParameter("{0}", string.Format("TASKID='{0}'", plcTaskNo)) });
                            if (dtErr.Rows.Count > 0)
                            {
                                string Taskstatus = dtErr.Rows[0]["TASKSTATUS"].ToString();
                                if (Taskstatus == "4")
                                {
                                    //crane.ErrCode == 505,重入异常
                                    if (TaskType == "OB")
                                        continue;

                                    bll.ExecNonQueryTran("WCS.SPCancelTask", new DataParameter[] { new DataParameter("VTASKNO", plcTaskNo) });
                                    Logger.Info("入库任务：" + plcTaskNo + "托盘编号：" + plcPalletCode + "取消！");
                                    DataTable dtTaskNew = bll.FillDataTable("WCS.SelectWmsTask", new DataParameter[] { new DataParameter("{0}", string.Format("PALLETID='{0}' AND TASKSTATUS='0' AND ASRSID='{1}'", plcPalletCode, i)) });
                                    if (dtTaskNew.Rows.Count > 0)
                                    {
                                        string TaskNo = dtTaskNew.Rows[0]["TASKID"].ToString();
                                        sbyte[] staskNo = new sbyte[20];
                                        Util.ConvertStringChar.stringToBytes(TaskNo, 12).CopyTo(staskNo, 0);
                                        Util.ConvertStringChar.stringToBytes(plcPalletCode, 8).CopyTo(staskNo, 12);
                                        string strDLocation = dtTaskNew.Rows[0]["DLOCATION"].ToString();
                                        int[] Location = new int[6];
                                        Location[0] = 0;
                                        Location[1] = 0;
                                        Location[2] = 0;
                                        Location[3] = int.Parse(strDLocation.Split('-')[0]);
                                        Location[4] = int.Parse(strDLocation.Split('-')[1]);
                                        int ToRow = int.Parse(strDLocation.Split('-')[2]);
                                        if (ToRow > 1)
                                            ToRow += 1;
                                        Location[5] = ToRow;

                                        context.ProcessDispatcher.WriteToService(serviceName, "Address", Location);
                                        context.ProcessDispatcher.WriteToService(serviceName, "WriteTask", staskNo);
                                        if (context.ProcessDispatcher.WriteToService(serviceName, "WriteFinish", true))
                                        {
                                            bll.ExecNonQueryTran("WCS.SPReciveWmsTask", new DataParameter[] { new DataParameter("VTASKNO", TaskNo) });
                                            //更新WCSTask状态为3
                                            bll.ExecNonQuery("WCS.UpdateWCSTaskState", new DataParameter[] { new DataParameter("{0}", 3), new DataParameter("{1}", TaskNo) });
                                            Logger.Info("入库任务:" + TaskNo + " 托盘编号：" + plcPalletCode + " 位:" + strDLocation + " 已下发给" + i + "堆垛机");
                                        }
                                        else
                                        {
                                            Logger.Error("入库任务:" + TaskNo + " 托盘编号：" + plcPalletCode + " 无法写入堆垛机" + i);
                                        }

                                    }
                                }

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Error("MainServer中tmCraneWorker出现异常：" + ex.Message);
            }
            finally
            {
                tmCraneErr.Start();
            }
        }

        #endregion



    }
}
