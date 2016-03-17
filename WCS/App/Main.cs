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
    public partial class Main : Form
    {
        private bool IsActiveForm = false;
        public bool IsActiveTab = false;
        private Context context = null;
        private System.Timers.Timer tmWorkTimer = new System.Timers.Timer();

        private BLL.BLLBase bll;
        private string CurrentUser = "";
        public Main()
        {
            InitializeComponent();
        }

        

     
        private void tmWorker(object sender, System.Timers.ElapsedEventArgs e)
        {

            tmWorkTimer.Stop();

            try
            {
                tmWorkTimer.Stop();
                DataTable dt = GetMonitorData();
                MainData.TaskInfo(dt);
            }
            catch (Exception ex)
            {
                Logger.Error("Main中tmWorker获取数据库数据出现异常" + ex.Message);
            }
            finally
            {
                tmWorkTimer.Start();
            }

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

               

                System.Threading.Thread.Sleep(3000);
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

               

                tmWorkTimer.Interval = 5000;
                tmWorkTimer.Elapsed += new System.Timers.ElapsedEventHandler(tmWorker);
                tmWorkTimer.Start();

            }
            catch (Exception ee)
            {
                Logger.Error("初始化处理失败请检查配置，原因：" + ee.Message);
            }



            MainData.OnTask += new TaskEventHandler(Data_OnTask);
            for (int i = 0; i < 3; i++)
                ((DataGridViewAutoFilterTextBoxColumn)this.dgvMain.Columns[i]).FilteringEnabled = true;

        }
        void Data_OnTask(TaskEventArgs args)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new TaskEventHandler(Data_OnTask), args);
            }
            else
            {
                lock (this.dgvMain)
                {
                    DataTable dt = args.datatTable;
                    this.bsMain.DataSource = dt;
                    for (int i = 0; i < this.dgvMain.Rows.Count; i++)
                    {
                        if (dgvMain.Rows[i].Cells["colErrCode"].Value.ToString()!="")
                            this.dgvMain.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        else
                        {
                            if (i % 2 == 0)
                                this.dgvMain.Rows[i].DefaultCellStyle.BackColor = Color.White;
                            else
                                this.dgvMain.Rows[i].DefaultCellStyle.BackColor = Color.LightCyan;

                        }
                    }
                }
            }
        }

        private void BindData()
        {
            bsMain.DataSource = GetMonitorData();
        }
        private DataTable GetMonitorData()
        {
            DataTable dt = bll.FillDataTable("WCS.SelectWCSTask");
            return dt;
        }
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("您确定要退出调度系统吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
               
                Logger.Info("退出系统");
                System.Threading.Thread.Sleep(2000);
               
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

                Logger.Info("退出系统");
                System.Threading.Thread.Sleep(2000);
                System.Environment.Exit(0);
            }
        }
        #endregion

        #region 任务处理


        private void dgvMain_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (CurrentUser.Trim() == "")
                    return;

                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dgvMain.Rows[e.RowIndex].Selected == false)
                    {
                        dgvMain.ClearSelection();
                        dgvMain.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (dgvMain.SelectedRows.Count == 1)
                    {
                        dgvMain.CurrentCell = dgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex];

                        string TaskType = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells["colTaskType"].Value.ToString();
                        if (TaskType == "IB")
                        {
                            //弹出操作菜单
                            contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                        }
                        else  
                        {
                            //弹出操作菜单
                            contextMenuStrip2.Show(MousePosition.X, MousePosition.Y);
                        }
                       
                    }
                }
            }
        }      
        //请求入库
        private void TMIStockRequest_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null) 
            {
                try
                {

                    string TaskNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    string PalletCode = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[4].Value.ToString();
                    bll.ExecNonQuery("WCS.UpdateWCSTaskState", new DataParameter[] { new DataParameter("{0}", 1), new DataParameter("{1}", TaskNo) });
                    Logger.Info("入库任务:" + TaskNo + " 托盘编号：" + PalletCode + " 手动更新任务状态为入库请求！");

                    BindData();
                }
                catch (Exception ex)
                {
                    Logger.Error("Main中手动更新任务状态为入库请求出现异常" + ex.Message);

                }
            }
        }
        //到达入库端
        private void TMIStockInFinished_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null) 
            {
                try
                {
                    string TaskNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    string PalletCode = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[4].Value.ToString();
                    bll.ExecNonQuery("WCS.UpdateWCSTaskState", new DataParameter[] { new DataParameter("{0}", 2), new DataParameter("{1}", TaskNo) });
                    Logger.Info("入库任务:" + TaskNo + " 托盘编号：" + PalletCode + " 手动更新任务状态为到达入库端！");
                    //当前输送线的任务号是否与当前任务号一致

                    BindData();
                }
                catch (Exception ex)
                {
                    Logger.Error("Main中手动更新任务状态为到达入库端出现异常" + ex.Message);
                }
            }

        }
        //入库完成
        private void TMITaskInFinished_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null) //判断该任务是否堆垛机已经执行？
            {
                try
                {
                    string TaskNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    string PalletCode = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[4].Value.ToString();
                    bll.ExecNonQueryTran("WCS.SpTaskFinished", new DataParameter[] { new DataParameter("VTaskNo", TaskNo) });
                    Logger.Info("入库任务：" + TaskNo + "托盘编号：" + PalletCode + "手动更新完成！");

                    this.BindData();
                }
                catch (Exception ex)
                {
                    Logger.Error("Main中入库任务手动更新完成出现异常" + ex.Message);

                }
            }

        }
        //入库取消
        private void TMITaskInCancle_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null) //判断该任务是否堆垛机已经执行？
            {
                try
                {
                    string TaskNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    string PalletCode = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[4].Value.ToString();
                    bll.ExecNonQueryTran("WCS.SPCancelTask", new DataParameter[] { new DataParameter("VTASKNO", TaskNo) });
                    Logger.Info("入库任务：" + TaskNo + "托盘编号：" + PalletCode + "手动取消！");

                    this.BindData();
                }
                catch (Exception ex)
                {
                    Logger.Error("Main中入库任务手动更新完成出现异常" + ex.Message);

                }
            }
        }
        //到达出库端
        private void TMICraneOutFinish_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null) //判断该任务是否堆垛机已经执行？
            {
                try
                {
                    string TaskNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    string PalletCode = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[4].Value.ToString();
                    bll.ExecNonQuery("WCS.UpdateWCSTaskState", new DataParameter[] { new DataParameter("{0}", 2), new DataParameter("{1}", TaskNo) });
                    Logger.Info("出库任务:" + TaskNo + " 托盘编号：" + PalletCode + " 手动更新任务状态为到达出库端！");
                    //当前输送线的任务号是否与当前任务号一致

                    BindData();
                }
                catch (Exception ex)
                {
                    Logger.Error("Main中手动更新任务状态为到达出库端出现异常" + ex.Message);
                }
            }

        }
        //出库完成
        private void TMITaskOutFinished_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null) //判断该任务是否堆垛机已经执行？
            {
                try
                {
                    string TaskNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    string PalletCode = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[4].Value.ToString();
                    bll.ExecNonQueryTran("WCS.SpTaskFinished", new DataParameter[] { new DataParameter("VTaskNo", TaskNo) });
                    Logger.Info("出库任务：" + TaskNo + "托盘编号：" + PalletCode + "手动更新完成！");

                    this.BindData();
                }
                catch (Exception ex)
                {
                    Logger.Error("Main中出库任务手动更新完成出现异常" + ex.Message);
                }
            }

        }
        //出库取消
        private void TMICraneOutCancle_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null) //判断该任务是否堆垛机已经执行？
            {
                try
                {
                    string TaskNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    string PalletCode = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[4].Value.ToString();
                    bll.ExecNonQueryTran("WCS.SPCancelTask", new DataParameter[] { new DataParameter("VTASKNO", TaskNo) });
                    Logger.Info("出库任务：" + TaskNo + "托盘编号：" + PalletCode + "手动取消！");
                    this.BindData();
                }
                catch (Exception ex)
                {
                    Logger.Error("Main中入库任务手动更新完成出现异常" + ex.Message);

                }
            }
        }

      
        //下输送线任务
        private void TMISendOutStock_Click(object sender, EventArgs e)
        {

            if (this.dgvMain.CurrentCell != null) //判断该任务是否堆垛机已经执行？
            {

                try
                {
                    string TaskNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    string PalletCode = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[4].Value.ToString();
                    int CraneNo = int.Parse(this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString());

                    int CarItem = 104 + (CraneNo - 1) * 4;
                    //判断输送线是否有货
                    string ReadTaskNo = ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService("CarPLC", CarItem + "_HasProduct")).ToString();
                    if (ReadTaskNo == "0" || ReadTaskNo == "False")
                        ReadTaskNo = "";
                    if (ReadTaskNo == "")
                    {
                        MessageBox.Show(CarItem + "输送线没有货位，无法下达输送线任务。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    sbyte[] staskNo = new sbyte[26];
                    string[] LedMsgs = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[10].Value.ToString().Split(',');
                    Util.ConvertStringChar.stringToBytes(TaskNo, 10).CopyTo(staskNo, 0);
                    Util.ConvertStringChar.stringToBytes(LedMsgs[0], 8).CopyTo(staskNo, 10);
                    Util.ConvertStringChar.stringToBytes(LedMsgs[1], 8).CopyTo(staskNo, 18);
                    context.ProcessDispatcher.WriteToService("CarPLC", CarItem + "_WriteTaskNo", staskNo);

                    if (context.ProcessDispatcher.WriteToService("CarPLC", CarItem + "_WriteFinished", 1))
                    {
                        bll.ExecNonQuery("WCS.UpdateWCSTaskState", new DataParameter[] { new DataParameter("{0}", 3), new DataParameter("{1}", TaskNo) });
                        Logger.Info("出库任务：" + TaskNo + "托盘编号：" + PalletCode + "任务手动下达" + CarItem + "输送线！");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Main中下输送线任务出现异常" + ex.Message);
                }
            }
        }
        //重下堆垛机出库任务
        private void TMICraneOutTask_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null) //判断该任务是否堆垛机已经执行？
            {
                try
                {
                    int CraneNo = int.Parse(this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString());
                    string serviceName = "CranePLC" + CraneNo;
                    string TaskNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    string PalletCode = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[4].Value.ToString();
                    string b_I_Loaded = ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService(serviceName, "b_I_Loaded")).ToString();
                    string nState = ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService(serviceName, "nState")).ToString();
                    string b_I_Local = ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService(serviceName, "b_I_Local")).ToString();
                    string b_I_Auto = ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService(serviceName, "b_I_Auto")).ToString();
                    if (nState.Equals("0") && b_I_Local.Equals("0") && b_I_Auto.Equals("1"))
                    {
                        sbyte[] staskNo = new sbyte[20];
                        Util.ConvertStringChar.stringToBytes(TaskNo, 12).CopyTo(staskNo, 0);
                        Util.ConvertStringChar.stringToBytes(PalletCode, 8).CopyTo(staskNo, 12);
                        int[] Location = new int[6];
                        if (b_I_Loaded == "1")
                        {
                            Location[0] = 0;
                            Location[1] = 0;
                            Location[2] = 0;
                        }
                        else
                        {

                            string b_I_Fork_Zero = ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService(serviceName, "b_I_Fork_Zero")).ToString();
                            if (b_I_Fork_Zero != "1")
                            {
                                MessageBox.Show("货叉位置不在原位，不能重下堆垛机任务！");
                                return;
                            }
                            string strSLocation = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[5].Value.ToString();
                            Location[0] = int.Parse(strSLocation.Split('-')[0]);
                            Location[1] = int.Parse(strSLocation.Split('-')[1]);
                            int fromRow = int.Parse(strSLocation.Split('-')[2]);
                            if (fromRow > 1)
                                fromRow += 1;
                            Location[2] = fromRow;
                        }
                        Location[3] = 2 * CraneNo;
                        Location[4] = 0;
                        Location[5] = 2;

                        context.ProcessDispatcher.WriteToService(serviceName, "Address", Location);
                        context.ProcessDispatcher.WriteToService(serviceName, "WriteTask", staskNo);
                        if (context.ProcessDispatcher.WriteToService(serviceName, "WriteFinish", true))
                        {
                            Logger.Info("出库任务：" + TaskNo + "托盘编号：" + PalletCode + "重下堆垛机任务！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("堆垛机状态不符，不能重下堆垛机任务！");
                        return;
                    }
                }

                catch (Exception ex)
                {
                    Logger.Error("Main中重下堆垛机出库任务出现异常" + ex.Message);
                }
            }
        }
        //重下堆垛机入库任务
        private void TMICraneInTask_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null) //判断该任务是否堆垛机已经执行？
            {
                try
                {
                    int CraneNo = int.Parse(this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString());
                    string serviceName = "CranePLC" + CraneNo;
                    string TaskNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    string PalletCode = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[4].Value.ToString();
                    string b_I_Loaded = ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService(serviceName, "b_I_Loaded")).ToString();
                    string nState = ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService(serviceName, "nState")).ToString();
                    string b_I_Local = ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService(serviceName, "b_I_Local")).ToString();
                    string b_I_Auto = ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService(serviceName, "b_I_Auto")).ToString();
                    if (nState.Equals("0") && b_I_Local.Equals("0") && b_I_Auto.Equals("1"))
                    {
                        sbyte[] staskNo = new sbyte[20];
                        Util.ConvertStringChar.stringToBytes(TaskNo, 12).CopyTo(staskNo, 0);
                        Util.ConvertStringChar.stringToBytes(PalletCode, 8).CopyTo(staskNo, 12);
                        int[] Location = new int[6];
                        if (b_I_Loaded == "1")
                        {
                            Location[0] = 0;
                            Location[1] = 0;
                            Location[2] = 0;
                        }
                        else
                        {
                            string b_I_Fork_Zero = ObjectUtil.GetObject(context.ProcessDispatcher.WriteToService(serviceName, "b_I_Fork_Zero")).ToString();
                            if (b_I_Fork_Zero != "1")
                            {
                                MessageBox.Show("货叉位置不在原位，不能重下堆垛机任务！");
                                return;
                            }
                            Location[0] = 2 * CraneNo - 1;
                            Location[1] = 0;
                            Location[2] = 2;
                        }
                        string strDLocation = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[6].Value.ToString();
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
                            Logger.Info("入库任务：" + TaskNo + "托盘编号：" + PalletCode + "重下堆垛机任务！");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Main中重下堆垛机入库任务出现异常" + ex.Message);
                }
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

       





    }
}
