using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDAL;
using MCP;
using OPC;
using MCP.Service.Siemens.Config;

namespace App.View
{
    public partial class frmMonitor2 : BaseForm
    {
       
        BLL.BLLBase bll = new BLL.BLLBase();
     
        Dictionary<int, string> dicCraneAction = new Dictionary<int, string>();
        Dictionary<string, string> dicCarError = new Dictionary<string, string>();
        DataTable dtCraneError;
        private System.Timers.Timer tmCrane = new System.Timers.Timer();
        public frmMonitor2()
        {
            InitializeComponent();
        }

         
        private void frmMonitor_Load(object sender, EventArgs e)
        {
            AddDicKeyValue();
            try
            {
                dtCraneError = bll.FillDataTable("WCS.SelectCraneWarning");
                for (int i = 1; i <= 5; i++)
                {
                    Control[] ctl = this.Controls.Find("picCrane" + i.ToString(), true);
                    if (ctl.Length > 0)
                    {
                        ((PictureBox)ctl[0]).Parent = pictureBox1;
                        ((PictureBox)ctl[0]).BackColor = Color.Transparent;
                    }
                }
                for (int i = 101; i <= 120; i++)
                {
                    Control[] ctl = this.Controls.Find("picCar" + i.ToString(), true);
                    if (ctl.Length > 0)
                    {
                        ((PictureBox)ctl[0]).Visible = false;
                        ((PictureBox)ctl[0]).Parent = pictureBox1;
                        ((PictureBox)ctl[0]).BackColor = Color.Transparent;
                    }
                }
                PLCS.OnPLC += new PLCEventHandler(Monitor_OnPLC);
                Cranes.OnCrane += new CraneEventHandler(Monitor_OnCrane);
                ServerInfo[] Servers = new MonitorConfig("Monitor.xml").Servers;
                for (int i = 0; i < Servers.Length; i++)
                {
                    OPCServer opcServer = new OPCServer(Servers[i].Name);
                    opcServer.Connect(Servers[i].ProgID, Servers[i].ServerName);// opcServer.Connect(config.ConnectionString);

                    OPCGroup group = opcServer.AddGroup(Servers[i].GroupName, Servers[i].UpdateRate);
                    foreach (ItemInfo item in Servers[i].Items)
                    {
                        group.AddItem(item.ItemName, item.OpcItemName, item.ClientHandler, item.IsActive);
                    }
                    if (Servers[i].Name == "CarServer")
                    {
                        opcServer.Groups.DefaultGroup.OnDataChanged += new OPCGroup.DataChangedEventHandler(CarServer_OnDataChanged);
                    }
                    else
                    {
                        opcServer.Groups.DefaultGroup.OnDataChanged += new OPCGroup.DataChangedEventHandler(CraneServer_OnDataChanged);
                    }
                }

                tmCrane.Interval = 2000;
                tmCrane.Elapsed += new System.Timers.ElapsedEventHandler(tmCraneWorker);
                tmCrane.Start();

            }
            catch (Exception ex)
            {
                Logger.Error("监控界面中frmMonitor_Load出现异常" + ex.Message);
            }
        }
        
        private void AddDicKeyValue()
        {
            

            dicCraneAction.Add(0, "空闲");
            dicCraneAction.Add(1, "等待");
            dicCraneAction.Add(2, "定位");
            dicCraneAction.Add(3, "取货");
            dicCraneAction.Add(4, "放货");
            dicCraneAction.Add(98, "维修");
            dicCraneAction.Add(99, "失效");
            dicCraneAction.Add(100, "自动");

            dicCarError.Add("0", "");
            dicCarError.Add("1", "超高故障");
            dicCarError.Add("2", "前超长故障");
            dicCarError.Add("3", "后超长故障");
            dicCarError.Add("4", "左超宽故障");
            dicCarError.Add("5", "右超宽故障");
            dicCarError.Add("6", "条码检测故障");
            dicCarError.Add("7", "巷道不符");
            dicCarError.Add("8", "超时故障");
            dicCarError.Add("9", "无入库任务！");

        }

        #region 堆垛机，输送线监控
        void CarServer_OnDataChanged(object sender, DataChangedEventArgs e)
        {
            try
            {
                if (e.State == null)
                    return;

                PLC plc = new PLC();
                string txt = e.ItemName.Split('_')[0];
                plc.ID = txt;
                plc.PicName = "picCar";

                if (e.ItemName.IndexOf("TaskNo") >= 0)
                {
                    plc.Text = Util.ConvertStringChar.BytesToString(e.States);
                    plc.TextName = "txtCarTaskNo";
                }
                else if (e.ItemName.IndexOf("PalletCode") >= 0)
                {
                    plc.Text = Util.ConvertStringChar.BytesToString(e.States);
                    plc.TextName = "txtPalletCode";

                }
                else if (e.ItemName.IndexOf("HasProduct") >= 0) //图片
                {
                    if ((bool)e.State)
                        plc.ShowPic = "1";
                }
                else
                {
                    string strErrorNo = Util.ConvertStringChar.BytesToString(e.States);
                    if (strErrorNo != "0" && strErrorNo!="32" && strErrorNo.Length!=0)
                    {
                        plc.Text = dicCarError[strErrorNo];
                        plc.TextName = "txtCarErrorDesc";
                        plc.IsErr = "1";
                        plc.ErrCode = strErrorNo;
                        plc.ShowPic = "1";
                    }
                    else
                    {
                        plc.Text = "";
                        plc.IsErr = "0";
                        plc.TextName = "txtCarErrorDesc";
                    }
                }
                PLCS.PLCInfo(plc);

            }
            catch (Exception ex)
            {
                MCP.Logger.Error("监控界面中CarServer_OnDataChanged出现异常" + ex.Message);
            }
        }

        void CraneServer_OnDataChanged(object sender, DataChangedEventArgs e)
        {
            try
            {
                if (e.State == null)
                    return;
                PLC plc = new PLC();
                string[] txt = e.ItemName.Split('_');
                plc.ID = txt[0];
                plc.PicName = "picCrane";
                plc.ShowPic = "1";
                plc.TextName = "txt" + txt[1];

                if (e.ItemName.IndexOf("TaskNo") >= 0)
                {
                    string TaskNo = Util.ConvertStringChar.BytesToString(e.States);
                    plc.Text = TaskNo;
                    plc.TaskType = "";
                    plc.TextTypeName = "txtTaskType";
                    if (TaskNo.Length > 0)
                    {
                        DataTable dtTask = bll.FillDataTable("WCS.SelectWCSTask", new DataParameter[] { new DataParameter("{0}", string.Format("TASKID='{0}'", TaskNo)) });
                        if (dtTask.Rows.Count > 0)
                        {
                            if (dtTask.Rows[0]["TASKTYPE"].ToString() == "IB")
                                plc.TaskType = "入库";
                            else
                                plc.TaskType = "出库";
                        }
                    }


                }
                else if (e.ItemName.IndexOf("PalletCode") >= 0)
                {
                    plc.Text = Util.ConvertStringChar.BytesToString(e.States);
                }
                else if (e.ItemName.IndexOf("CraneAction") >= 0)
                {
                    int Action = int.Parse(ObjectUtil.GetObject(e.States).ToString());
                    plc.Text = dicCraneAction[Action];
                }
                else if (e.ItemName.IndexOf("Column") >= 0 )
                {
                    plc.Text = ObjectUtil.GetObject(e.States).ToString();
                }
                else if(e.ItemName.IndexOf("Row") >= 0)
                {
                    int Row = int.Parse(ObjectUtil.GetObject(e.States).ToString());
                    if (Row >= 2)
                        Row = Row - 1;
                    plc.Text = Row.ToString();
                }
                else
                {
                    string strErrorNo = ObjectUtil.GetObject(e.States).ToString();

                    if (strErrorNo != "0")
                    {
                        DataRow[] drs = dtCraneError.Select(string.Format("warncode='{0}'", strErrorNo));
                        if (drs.Length > 0)
                            plc.Text = drs[0]["WARNDESC"].ToString();
                        else
                            plc.Text = "未知错误！";
                        plc.TextName = "txtErrorDesc";
                        plc.IsErr = "1";
                        plc.ErrCode = strErrorNo;
                        plc.TextErrName = "txtErrorNo";
                    }
                    else
                    {
                        plc.Text = "";
                        plc.TextName = "txtErrorDesc";
                        plc.IsErr = "0";
                        plc.ErrCode = strErrorNo;
                        plc.TextErrName = "txtErrorNo";
                    }
                }
                PLCS.PLCInfo(plc);
            }
            catch (Exception ex)
            {
                MCP.Logger.Error("监控界面CraneServer_OnDataChanged中出现异常" + ex.Message);
            }

        }

        void Monitor_OnPLC(PLCEventArgs args)
        {


            if (InvokeRequired)
            {
                BeginInvoke(new PLCEventHandler(Monitor_OnPLC), args);
            }
            else
            {
                try
                {
                    PLC plc = args.plc;
                    TextBox txt = GetTextBox(plc.TextName, plc.ID); //任务编号
                    PictureBox pic = GetPictureBox(plc.PicName, plc.ID);
                    TextBox txtErrCode = GetTextBox(plc.TextErrName, plc.ID);
                    TextBox txtTaskType = GetTextBox(plc.TextTypeName, plc.ID);
                    if (txtErrCode != null)
                    {
                        txtErrCode.Text = plc.ErrCode;
                    }
                    if (txtTaskType != null)
                    {
                        txtTaskType.Text = plc.TaskType;
                    }
                    if (txt != null)
                    {
                        txt.Text = plc.Text;
                        if (plc.IsErr == "1")
                            txt.BackColor = Color.Red;
                        else
                            txt.BackColor = Control.DefaultBackColor;

                    }
                    if (pic != null)
                    {
                        if (plc.ShowPic == "1")
                            pic.Visible = true;
                        else
                            pic.Visible = false;

                        if (pic.Visible)
                        {
                            if (plc.IsErr == "1")
                                pic.BackColor = Color.Red;
                            else
                                pic.BackColor = Color.Transparent;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MCP.Logger.Error("监控界面中Monitor_OnPLC出现异常" + ex.Message);
                }
            }
        }

        void Monitor_OnCrane(CraneEventArgs args)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new CraneEventHandler(Monitor_OnCrane), args);
            }
            else
            {
                
                Crane crane=args.crane;
                TextBox txt = GetTextBox("txtColumn", crane.CraneNo);
                PictureBox pic = GetPictureBox("picCrane", crane.CraneNo.ToString());
                if (txt != null && pic != null)
                {
                    int X = 1009;
                    if (crane.Column == 0)
                        X = 1007;
                    int Y = 336;
                    Point P = new Point();
                    P.X = X - (int)(crane.Column * 24.38F);
                    P.Y = Y - (crane.CraneNo - 1) * 73;
                    pic.Location = P;
                }
            }
        }
        #endregion

        #region 公共方法
        TextBox GetTextBox(string name, int CraneNo)
        {
            Control[] ctl = this.Controls.Find(name + CraneNo.ToString(), true);
            if (ctl.Length > 0)
                return (TextBox)ctl[0];
            else
                return null;
        }
        TextBox GetTextBox(string name,string id)
        {
            Control[] ctl = this.Controls.Find(name + id, true);
            if (ctl.Length > 0)
                return (TextBox)ctl[0];
            else
                return null;
        }

        PictureBox GetPictureBox(string name,string ID)
        {
            if (name == null)
                return null;
            Control[] ctl = this.Controls.Find(name + ID, true);
            if (ctl.Length > 0)
                return (PictureBox)ctl[0];
            else
                return null;
        }
        #endregion

        #region 堆垛机故障跟踪
        private void tmCraneWorker(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                tmCrane.Stop();
                
                for (int i = 1; i <= 5; i++)
                {

                    TextBox txt = GetTextBox("txtColumn", i);
                    if (txt != null)
                    {
                        Crane crane = new Crane();
                        crane.CraneNo = i;
                        int column = 0;
                        if (txt.Text != "")
                            column = int.Parse(txt.Text);
                        crane.Column = column;
                        Cranes.CraneInfo(crane);
                    }



                    txt = GetTextBox("txtErrorNo", i);
                    if (txt != null)
                    {
                        if (txt.Text.Length == 0)
                            continue;

                        int ErrCode = int.Parse(txt.Text);
                        if (ErrCode == 0)
                            continue;
                        //string[] d = new string[2];
                        //d[0] = i.ToString();
                        //d[1] = GetTextBox("txtErrorDesc", i).Text;
                        //Context.ProcessDispatcher.WriteToProcess("LEDProcess", "Error", d);//LED显示
                        if (ErrCode == 506 || ErrCode == 505)
                        {
                            string serviceName = "CranePLC" + i;
                            string plcTaskNo = GetTextBox("txtTaskNo", i).Text;
                            string plcPalletCode = GetTextBox("txtPalletCode", i).Text;
                            DataTable dtErr = bll.FillDataTable("WCS.SelectWmsSend", new DataParameter[] { new DataParameter("{0}", string.Format("TASKID='{0}'", plcTaskNo)) });
                            if (dtErr.Rows.Count > 0)
                            {
                                string Taskstatus = dtErr.Rows[0]["TASKSTATUS"].ToString();
                                if (Taskstatus == "4")
                                {
                                    bll.ExecNonQueryTran("WCS.SPCancelTask", new DataParameter[] { new DataParameter("VTASKNO", plcTaskNo) });
                                    Logger.Info("任务:" + plcTaskNo + " 托盘编号：" + dtErr.Rows[0]["PALLETID"].ToString() + "任务取消！");
                                    if (ErrCode == 506)//空取异常
                                    {
                                        //取消堆垛机当前任务
                                        Context.ProcessDispatcher.WriteToService(serviceName, "CancelTask", true);
                                        sbyte[] taskNo = new sbyte[20];
                                        Util.ConvertStringChar.stringToBytes("", 20).CopyTo(taskNo, 0);
                                        Context.ProcessDispatcher.WriteToService(serviceName, "ClearTaskNo", taskNo);
                                    }
                                    else //crane.ErrCode == 505,重入异常
                                    {
                                        if (GetTextBox("txtTaskType", i).Text == "出库")
                                            continue;
                                        DataTable dtTask = bll.FillDataTable("WCS.SelectWmsTask", new DataParameter[] { new DataParameter("{0}", string.Format("PALLETID='{0}' AND TASKSTATUS='0' AND ASRSID='{1}'", plcPalletCode, i)) });
                                        if (dtTask.Rows.Count > 0)
                                        {
                                            string TaskNo = dtTask.Rows[0]["TASKID"].ToString();
                                            sbyte[] staskNo = new sbyte[20];
                                            Util.ConvertStringChar.stringToBytes(TaskNo, 12).CopyTo(staskNo, 0);
                                            Util.ConvertStringChar.stringToBytes(plcPalletCode, 8).CopyTo(staskNo, 12);
                                            string strDLocation = dtTask.Rows[0]["DLOCATION"].ToString();
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

                                            Context.ProcessDispatcher.WriteToService(serviceName, "Address", Location);
                                            Context.ProcessDispatcher.WriteToService(serviceName, "WriteTask", staskNo);
                                            if (Context.ProcessDispatcher.WriteToService(serviceName, "WriteFinish", true))
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
            }
            catch (Exception ex)
            {
                Logger.Error("监控界面中tmCraneWorker出现异常：" + ex.Message);
            }
            finally
            {
                tmCrane.Start();
            }
        }
        
        #endregion

        #region 条码检测故障，人工输入条码
        private void txtPalletCode101_DoubleClick(object sender, EventArgs e)
        {
            PalletCodeDoubleClick(1, 101);
        }
        private void txtPalletCode105_DoubleClick(object sender, EventArgs e)
        {
            PalletCodeDoubleClick(2, 105);
        }

        private void txtPalletCode109_DoubleClick(object sender, EventArgs e)
        {
            PalletCodeDoubleClick(3, 109);
        }

        private void txtPalletCode113_DoubleClick(object sender, EventArgs e)
        {
            PalletCodeDoubleClick(4, 113);
        }
        private void txtPalletCode117_DoubleClick(object sender, EventArgs e)
        {

            PalletCodeDoubleClick(5, 117);
        }
        private void btnWritePalletCode1_Click(object sender, EventArgs e)
        {
            PalletCodeDoubleClick(1, 101);
        }
        private void btnWritePalletCode2_Click(object sender, EventArgs e)
        {
            PalletCodeDoubleClick(2, 105);
        }
        private void btnWritePalletCode3_Click(object sender, EventArgs e)
        {
            PalletCodeDoubleClick(3, 109);
        }
        private void btnWritePalletCode4_Click(object sender, EventArgs e)
        {
            PalletCodeDoubleClick(4, 113);
        }
        private void btnWritePalletCode5_Click(object sender, EventArgs e)
        {
            PalletCodeDoubleClick(5, 117);
        }

        private void PalletCodeDoubleClick(int CraneNo, int ItemNo)
        {
            if (GetTextBox("txtCarErrorDesc", ItemNo).Text == "条码检测故障")
            {
                object o = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService("CarPLC", ItemNo + "_HasProduct"));
                if (o.ToString() == "1" || o.ToString() == "True")
                {
                    Task.frmHandlePalletCode frm = new Task.frmHandlePalletCode(ItemNo, CraneNo);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {

                        string TaskStatus = frm.TaskStatus;
                        
                        Context.ProcessDispatcher.WriteToService("CarPLC", ItemNo + "_WriteFinished", 3);
                        string TaskNo = frm.TaskNo;
                        
                        sbyte[] staskNo = new sbyte[26];
                        string[] LedMsgs = frm.LedNo.Split(',');
                       Util.ConvertStringChar.stringToBytes(TaskNo, 10).CopyTo(staskNo, 0);
                       Util.ConvertStringChar.stringToBytes(LedMsgs[0], 8).CopyTo(staskNo, 10);
                       Util.ConvertStringChar.stringToBytes(LedMsgs[1], 8).CopyTo(staskNo, 18);
                       Context.ProcessDispatcher.WriteToService("CarPLC", ItemNo + "_WriteTaskNo", staskNo);
                        if (Context.ProcessDispatcher.WriteToService("CarPLC", ItemNo + "_WriteFinished", 4))
                        {
                            //插入WCS_Task
                            if (TaskStatus == "0")
                            {
                                bll.ExecNonQueryTran("WCS.SPReciveWmsTask", new DataParameter[] { new DataParameter("VTASKNO", frm.TaskNo) });
                            }
                            Logger.Info(ItemNo + "输送线托盘编号：" + frm.PalletCode + "开始入库");
                        }
                    }
                }
            }
        }
        #endregion

        #region 清输送线任务号
        private void btnClearTask1_Click(object sender, EventArgs e)
        {
            ClearCarTask(1);
        }

        private void btnClearTask2_Click(object sender, EventArgs e)
        {
            ClearCarTask(2);
        }

        private void btnClearTask3_Click(object sender, EventArgs e)
        {
            ClearCarTask(3);
        }

        private void btnClearTask4_Click(object sender, EventArgs e)
        {
            ClearCarTask(4);
        }

        private void btnClearTask5_Click(object sender, EventArgs e)
        {
            ClearCarTask(5);
        }
        private void ClearCarTask(int CraneNo)
        {
            int[] Cars = new int[4];
            switch (CraneNo)
            {
                case 1:
                    Cars[0] = 101;
                    Cars[1] = 102;
                    Cars[2] = 103;
                    Cars[3] = 104;
                    break;
                case 2:
                    Cars[0] = 105;
                    Cars[1] = 106;
                    Cars[2] = 107;
                    Cars[3] = 108;
                    break;
                case 3:
                    Cars[0] = 109;
                    Cars[1] = 110;
                    Cars[2] = 111;
                    Cars[3] = 112;
                    break;
                case 4:
                    Cars[0] = 113;
                    Cars[1] = 114;
                    Cars[2] = 115;
                    Cars[3] = 116;
                    break;
                case 5:
                    Cars[0] = 117;
                    Cars[1] = 118;
                    Cars[2] = 119;
                    Cars[3] = 120;
                    break;

            }

            Task.frmSelectCar frm = new Task.frmSelectCar(Cars);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (Context.ProcessDispatcher.WriteToService("CarPLC", frm.SelectText + "_WriteFinished", 5))
                {
                    Logger.Info("清除" + frm.SelectText + "输送线任务号!");
                }
            }
        }
        #endregion 

        #region 堆垛机复位
        private void btnReset1_Click(object sender, EventArgs e)
        {
            CraneReset(1);
        }

        private void btnReset2_Click(object sender, EventArgs e)
        {
            CraneReset(2);
        }

        private void btnReset3_Click(object sender, EventArgs e)
        {
            CraneReset(3);
        }

        private void btnReset4_Click(object sender, EventArgs e)
        {
            CraneReset(4);
        }

        private void btnReset5_Click(object sender, EventArgs e)
        {
            CraneReset(5);
        }

        private void CraneReset(int CraneNo)
        {
            if (Context.ProcessDispatcher.WriteToService("CranePLC" + CraneNo, "b_O_Reset", true))
            {
                Logger.Info("复位" + CraneNo + "号堆垛机!");
            }
        }
        #endregion

        #region 堆垛机召回
        private void btnBack1_Click(object sender, EventArgs e)
        {
            CraneBack(1);
        }

        private void btnBack2_Click(object sender, EventArgs e)
        {
            CraneBack(2);

        }

        private void btnBack3_Click(object sender, EventArgs e)
        {
            CraneBack(3);
        }

        private void btnBack4_Click(object sender, EventArgs e)
        {
            CraneBack(4);
        }

        private void btnBack5_Click(object sender, EventArgs e)
        {
            CraneBack(5);
        }

        private void CraneBack(int CraneNo)
        {
            if (MessageBox.Show("是否要召回" + CraneNo + "号堆垛机到初始位置?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                Context.ProcessDispatcher.WriteToService("CranePLC" + CraneNo, "CallBack", true);
            }
        }
        #endregion

        #region 清堆垛机任务号
        private void btnClearCrane1_Click(object sender, EventArgs e)
        {
            ClearCrane(1);
        }

        private void btnClearCrane2_Click(object sender, EventArgs e)
        {
            ClearCrane(2);
        }

        private void btnClearCrane3_Click(object sender, EventArgs e)
        {
            ClearCrane(3);
        }

        private void btnClearCrane4_Click(object sender, EventArgs e)
        {
            ClearCrane(4);
        }

        private void btnClearCrane5_Click(object sender, EventArgs e)
        {
            ClearCrane(5);
        }

        private void ClearCrane(int CraneNo)
        {
            if (MessageBox.Show("是否要清除" + CraneNo + "号堆垛机任务?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {


                Context.ProcessDispatcher.WriteToService("CranePLC" + CraneNo, "CancelTask", true);
                sbyte[] taskNo = new sbyte[12];
                Util.ConvertStringChar.stringToBytes("", 12).CopyTo(taskNo, 0);
                if (Context.ProcessDispatcher.WriteToService("CranePLC" + CraneNo, "ReadTaskNo", taskNo))
                {
                    Logger.Info("清除" + CraneNo + "号堆垛机任务号!");
                }
            }
        }
        #endregion

        private int L = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            int X = 1009;
            int Column = L;
            if (Column == 0)
                X = 1007;
            int Y = 336;

            Point P = new Point();
            P.X = X - (int)(Column * 24.38F);
            P.Y = Y - (5 - 1) * 73;
            picCrane5.Location = P;
            L++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            L = 0;
        }
    }
}
