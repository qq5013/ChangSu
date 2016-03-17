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

namespace App.View
{
    public partial class frmMonitor1 : BaseForm
    {
        #region 堆垛机，输送机时间触发器
        private System.Timers.Timer tmCrane1 = new System.Timers.Timer();
        private System.Timers.Timer tmCrane2 = new System.Timers.Timer();
        private System.Timers.Timer tmCrane3 = new System.Timers.Timer();
        private System.Timers.Timer tmCrane4 = new System.Timers.Timer();
        private System.Timers.Timer tmCrane5 = new System.Timers.Timer();

       
       
        private System.Timers.Timer tmCar119 = new System.Timers.Timer();
        
        #endregion

        BLL.BLLBase bll = new BLL.BLLBase();
        Dictionary<int, string> dicCraneTaskType = new Dictionary<int, string>();
        Dictionary<int, string> dicCraneAction = new Dictionary<int, string>();
        Dictionary<int, string> dicCarError = new Dictionary<int, string>();
        DataTable dtCraneError;




        public frmMonitor1()
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


                Cranes.OnCrane += new CraneEventHandler(Monitor_OnCrane);
                Cars.OnCar += new CarEventHandler(Monitor_OnCar);

                #region 堆垛机，输送线时间启动

                //tmCrane1.Interval = 3000;
                //tmCrane1.Elapsed += new System.Timers.ElapsedEventHandler(tmCraneWorker1);
                //tmCrane1.Start();

                //tmCrane2.Interval = 3000;
                //tmCrane2.Elapsed += new System.Timers.ElapsedEventHandler(tmCraneWorker2);
                //tmCrane2.Start();

                //tmCrane3.Interval = 3000;
                //tmCrane3.Elapsed += new System.Timers.ElapsedEventHandler(tmCraneWorker3);
                //tmCrane3.Start();


                //tmCrane4.Interval = 3000;
                //tmCrane4.Elapsed += new System.Timers.ElapsedEventHandler(tmCraneWorker4);
                //tmCrane4.Start();

                tmCrane5.Interval = 3000;
                tmCrane5.Elapsed += new System.Timers.ElapsedEventHandler(tmCraneWorker5);
                tmCrane5.Start();

                tmCar119.Interval = 3000;
                tmCar119.Elapsed += new System.Timers.ElapsedEventHandler(tmCarWorker119);
                tmCar119.Start();
               
                #endregion
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
        }
        private void AddDicKeyValue()
        {
            dicCraneTaskType.Add(0, "");
            dicCraneTaskType.Add(1, "入库");
            dicCraneTaskType.Add(2, "出库");

            dicCraneAction.Add(0, "空闲");
            dicCraneAction.Add(1, "等待)");
            dicCraneAction.Add(2, "定位");
            dicCraneAction.Add(3, "取货");
            dicCraneAction.Add(4, "放货");
            dicCraneAction.Add(98, "维修");
            dicCraneAction.Add(99, "失效");
            dicCraneAction.Add(100, "自动");

            dicCarError.Add(0, "");
            dicCarError.Add(1, "超高故障");
            dicCarError.Add(2, "前超长故障");
            dicCarError.Add(3, "后超长故障");
            dicCarError.Add(4, "左超宽故障");
            dicCarError.Add(5, "右超宽故障");
            dicCarError.Add(6, "条码检测故障");
            dicCarError.Add(7, "巷道不符");
            dicCarError.Add(8, "超时故障");

        }

        void Monitor_OnCrane(CraneEventArgs args)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new CraneEventHandler(Monitor_OnCrane), args);
            }
            else
            {
                Crane crane = args.crane;
                TextBox txt = GetTextBox("txtTaskNo", crane.CraneNo); //任务编号
                if (txt != null)
                    txt.Text = crane.TaskNo;

                txt = GetTextBox("txtPalletCode", crane.CraneNo);//托盘编号
                if (txt != null)
                    txt.Text = crane.PalletCode;

                txt = GetTextBox("txtTaskType", crane.CraneNo);
                if (txt != null && dicCraneTaskType.ContainsKey(crane.TaskType))
                    txt.Text = dicCraneTaskType[crane.TaskType];

                txt = GetTextBox("txtCraneAction", crane.CraneNo);
                if (txt != null && dicCraneAction.ContainsKey(crane.Action))
                    txt.Text = dicCraneAction[crane.Action];

                txt = GetTextBox("txtColumn", crane.CraneNo);
                if (txt != null)
                    txt.Text = crane.Column.ToString();

                txt = GetTextBox("txtColumn", crane.CraneNo);
                if (txt != null)
                    txt.Text = crane.Height.ToString();

                txt = GetTextBox("txtErrorNo", crane.CraneNo);
                if (txt != null)
                    txt.Text = crane.ErrCode.ToString();

                Control[] ctl = this.Controls.Find("picCrane" + crane.CraneNo.ToString(), true);
                if (ctl.Length > 0)
                {
                    if (crane.ErrCode > 0)
                    {
                        ctl[0].BackColor = Color.Red;
                    }
                    else
                    {
                        ctl[0].BackColor = Color.Transparent;
                        
                    }

                    //堆垛机位置
                    int X = 623;
                    if (crane.Column == 0)
                        X = 1007;
                    int Y = 336;

                    Point P = new Point();
                    P.X = X - (int)(crane.Column * 15.28F);
                    P.Y = Y - (crane.CraneNo - 1) * 73;
                    ctl[0].Location = P;


                }
                txt = GetTextBox("txtErrorDesc", crane.CraneNo);
                string errDesc = "";
                if (txt != null)
                {
                    if (crane.ErrCode > 0)
                    {
                        DataRow[] drs = dtCraneError.Select(string.Format("warncode='{0}'", crane.ErrCode));
                        if (drs.Length > 0)
                        {
                            txt.Text = drs[0]["description"].ToString();
                        }
                        else
                        {
                            txt.Text = "未知错误！";
                        }
                        errDesc = txt.Text;
                        txt.BackColor = Color.Red;

                        string[] d = new string[2];
                        d[0] = crane.CraneNo.ToString();
                        d[1] = txt.Text;
                        Context.ProcessDispatcher.WriteToProcess("LEDProcess", "Error", d);
                    }
                    else
                    {
                        txt.Text = "";
                        txt.BackColor = Control.DefaultBackColor;
                       // Context.ProcessDispatcher.WriteToProcess("LEDProcess" + crane.CraneNo, "Refresh", crane.CraneNo * 2);
                    }
                }
               

                //更新错误代码、错误描述
                //bll.ExecNonQuery("WCS.UpdateTaskError", new DataParameter[] { new DataParameter("@CraneErrCode", crane.ErrCode.ToString()), new DataParameter("@CraneErrDesc", errDesc), new DataParameter("@TaskNo", crane.TaskNo) });

            }
        }
        TextBox GetTextBox(string name, int CraneNo)
        {
            Control[] ctl = this.Controls.Find(name + CraneNo.ToString(), true);
            if (ctl.Length > 0)
                return (TextBox)ctl[0];
            else
                return null;
        }
        void Monitor_OnCar(CarEventArgs args)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new CarEventHandler(Monitor_OnCar), args);
            }
            else
            {
                Car car = args.car;
                TextBox txt = GetTextBox("txtCarTaskNo", car.CarNo);
                if (txt != null)
                    txt.Text = car.TaskNo;


                txt = GetTextBox("txtPalletCode", car.CarNo);
                if (txt != null)
                    txt.Text = car.PalletCode;

               
                txt = GetTextBox("txtCarErrorDesc", car.CarNo);
                if (txt != null && dicCarError.ContainsKey(car.ErrCode))
                {
                    txt.Text = dicCarError[car.ErrCode];
                    txt.ForeColor = Color.Black;
                    if (txt.Text.Length > 0)
                        txt.BackColor = Color.Red;
                    else
                        txt.BackColor = Control.DefaultBackColor;
                    
                }
                Control[] ctl = this.Controls.Find("picCar" + car.CarNo.ToString(), true);
                if (ctl.Length > 0)
                {
                    if (car.TaskNo.Length > 0)
                    {
                        ctl[0].Visible = true;
                    }
                    else
                    {
                        ctl[0].Visible = false;
                    }
                    if (car.ErrCode > 0)
                    {
                        ctl[0].BackColor = Color.Red;
                    }
                    else
                    {
                        ctl[0].BackColor = Color.Transparent;
                    }
                }
            }
        }

        #region 堆垛机信息获取
        private void tmCraneWorker1(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                tmCrane1.Stop();
                GetCraneInfo(1); 
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            finally
            {
                tmCrane1.Start();
            }
        }
        
        private void tmCraneWorker2(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                tmCrane2.Stop();
                GetCraneInfo(2);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            finally
            {
                tmCrane2.Start();
            }
        }

        private void tmCraneWorker3(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                tmCrane3.Stop();
                GetCraneInfo(3); 
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            finally
            {
                tmCrane3.Start();
            }
        }

        private void tmCraneWorker4(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                tmCrane4.Stop();
                GetCraneInfo(4); 
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            finally
            {
                tmCrane4.Start();
            }
        }

        private void tmCraneWorker5(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                tmCrane5.Stop();
                GetCraneInfo(5); 
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            finally
            {
                tmCrane5.Start();
            }
        }
        private void GetCraneInfo(int CraneNo)
        {
            try
            {

                string serviceName = "CranePLC" + CraneNo;

                Crane crane = new Crane();
                crane.CraneNo = CraneNo;

                int[] location = new int[2];
                string plcPalletCode = "";
                string plcTaskNo = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(serviceName, "s_I_task_no")));
                if (plcTaskNo != "")
                {
                    plcPalletCode = Util.ConvertStringChar.BytesToString(ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(serviceName, "s_I_pallet_code")));

                }
                crane.TaskNo = plcTaskNo;
                crane.PalletCode = plcPalletCode;
                if (plcTaskNo.Length > 0)
                {
                    int n_column1 = (int)ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "n_column1"));
                    int n_column2 = (int)ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "n_column2"));
                    if (n_column1 == 0)
                    {
                        crane.TaskType = 1;//入库
                    }
                    else
                    {
                        crane.TaskType = 2;//出库
                    }
                }
                else
                {
                    crane.TaskType = 0;
                }
                crane.Action = (int)ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "nState"));//Action



                object[] obj = ObjectUtil.GetObjects(Context.ProcessDispatcher.WriteToService(serviceName, "nTravelPos")); //列层
                for (int j = 0; j < obj.Length; j++)
                    location[j] = Convert.ToInt16(obj[j].ToString());
                crane.Column = location[0];
                crane.Height = location[1];

                string b_I_Alarm = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "b_I_Alarm")).ToString();
                string b_I_Warning = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "b_I_Warning")).ToString();
                crane.ErrCode = (int)ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService(serviceName, "nAlarmCode"));

                if (crane.ErrCode == 504 || crane.ErrCode == 505)
                {
                    DataTable dtErr = bll.FillDataTable("WCS.SelectWmsSend", new DataParameter[] { new DataParameter("{0}", string.Format("TASKID='{0}'", plcTaskNo)) });
                    if (dtErr.Rows.Count > 0)
                    {

                        string Taskstatus = dtErr.Rows[0]["TASKSTATUS"].ToString();
                        if (Taskstatus == "4")
                        {
                            bll.ExecNonQueryTran("WCS.SPCancelTask", new DataParameter[] { new DataParameter("VTASKNO", plcTaskNo) });
                            Logger.Info(dicCraneTaskType[crane.TaskType] + "任务:" + plcTaskNo + " 托盘编号：" + dtErr.Rows[0]["PALLETID"].ToString() + "任务废弃！");

                            if (crane.ErrCode == 504)//空取异常
                            {
                                //取消堆垛机当前任务
                                Context.ProcessDispatcher.WriteToService(serviceName, "b_O_Cancel_Task", true);
                                sbyte[] taskNo = new sbyte[12];
                                Util.ConvertStringChar.stringToBytes("", 12).CopyTo(taskNo, 0);
                                Context.ProcessDispatcher.WriteToService(serviceName, "s_I_task_no", taskNo);
                            }
                            else //crane.ErrCode == 505,重入异常
                            {
                                DataTable dtTask = bll.FillDataTable("WCS.SelectWmsTask", new DataParameter[] { new DataParameter("{0}", string.Format("PALLETID='{0}' AND TASKSTATUS='0' AND ASRSID='{1}'", plcPalletCode, CraneNo)) });

                                if (dtTask.Rows.Count > 0)
                                {
                                    string PalletCode = dtTask.Rows[0]["PALLETID"].ToString();
                                    string TaskNo = dtTask.Rows[0]["TASKID"].ToString();
                                    sbyte[] staskNo = new sbyte[12];
                                    Util.ConvertStringChar.stringToBytes(TaskNo, 12).CopyTo(staskNo, 0);
                                    sbyte[] spalletCode = new sbyte[8];
                                    Util.ConvertStringChar.stringToBytes(PalletCode, 8).CopyTo(spalletCode, 0);
                                    string DLocation = dtTask.Rows[0]["DLOCATION"].ToString();

                                    Int16 ToShelf = Int16.Parse(DLocation.Split('-')[0]);
                                    Int16 ToColumn = Int16.Parse(DLocation.Split('-')[1]);
                                    Int16 ToRow = Int16.Parse(DLocation.Split('-')[2]);
                                    Context.ProcessDispatcher.WriteToService(serviceName, "n_shelf1", Util.ConvertStringChar.GetBytes(0));
                                    Context.ProcessDispatcher.WriteToService(serviceName, "n_column1", Util.ConvertStringChar.GetBytes(0));
                                    Context.ProcessDispatcher.WriteToService(serviceName, "n_Row1", Util.ConvertStringChar.GetBytes(0));

                                    Context.ProcessDispatcher.WriteToService(serviceName, "n_shelf2", Util.ConvertStringChar.GetBytes(ToShelf));
                                    Context.ProcessDispatcher.WriteToService(serviceName, "n_column2", Util.ConvertStringChar.GetBytes(ToColumn));
                                    Context.ProcessDispatcher.WriteToService(serviceName, "n_Row2", Util.ConvertStringChar.GetBytes(ToRow));

                                    Context.ProcessDispatcher.WriteToService(serviceName, "s_O_task_no", staskNo);
                                    Context.ProcessDispatcher.WriteToService(serviceName, "s_O_pallet_code", spalletCode);

                                    if (Context.ProcessDispatcher.WriteToService(serviceName, "b_O_New_Task", true))
                                    {
                                        //更新WCSTask状态为3
                                        bll.ExecNonQuery("WCS.UpdateWCSTaskState", new DataParameter[] { new DataParameter("{0}", 3), new DataParameter("{1}", TaskNo) });
                                        Logger.Info("入库任务:" + TaskNo + " 托盘编号：" + PalletCode + " 位:" + DLocation + " 已下发给" + CraneNo + "堆垛机");
                                    }
                                    else
                                    {
                                        Logger.Error("入库任务:" + TaskNo + " 托盘编号：" + PalletCode + " 无法写入堆垛机" + CraneNo);
                                    }

                                }
                                else
                                {
                                    Logger.Error(dicCraneTaskType[crane.TaskType] + "任务：" + plcTaskNo + " WMS未处理该异常：");
                                }
                            }
                        }
                    }
                    else
                    {
                        Logger.Info(dicCraneTaskType[crane.TaskType] + "任务：" + plcTaskNo + " WCS未处理该异常：");
                    }
                }

                Cranes.CraneInfo(crane);
            }
            catch (Exception ex)
            {
                Logger.Error("监控界面，GetCraneInfo出现异常：" + ex.Message);
            }
        }


        #endregion

        #region 输送线信息获取

        private void tmCarWorker119(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                tmCar119.Stop();
                for (int i = 17; i <= 20; i++)
                {
                    int j = i + 100;
                    GetCarInfo(j, j + "_");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("监控界面，tmCarWorker120出现异常：" + ex.Message);
            }
            finally
            {
                tmCar119.Start();
            }
        }
      
        private void GetCarInfo(int CarNo,string Prefix)
        {
            string PalletCode = "";
            string TaskNo = Util.ConvertStringChar.BytesToString((byte[])Context.ProcessDispatcher.WriteToService("CarPLC", Prefix + "TaskNo"));
            if (TaskNo != "")
                PalletCode = Util.ConvertStringChar.BytesToString((byte[])Context.ProcessDispatcher.WriteToService("CarPLC", Prefix + "PalletCode"));
            Car car1 = new Car();
            car1.CarNo = CarNo;
            car1.TaskNo = TaskNo;
            car1.PalletCode = PalletCode;
            int ErrCode = (int)ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService("CarPLC", Prefix + "ErrCode"));
            if (ErrCode != 0)
            {
                if (CarNo == 101 || CarNo == 105 || CarNo == 109 || CarNo == 113 || CarNo == 117)
                {
                    for (int j = 1; j <= 8; j++)
                    {
                        string sValue = ObjectUtil.GetObject(Context.ProcessDispatcher.WriteToService("CarPLC", Prefix + "ErrCode_" + j.ToString())).ToString();
                        if (sValue.Equals("1"))
                        {
                            car1.ErrCode = j;
                            break;
                        }
                    }
                }
                else
                {
                    car1.ErrCode = 8;
                }
            }
            else
                car1.ErrCode = 0;

            Cars.CarInfo(car1);
        }
        #endregion

        #region  Button
        private void btnBack_Click(object sender, EventArgs e)
        {
            //if (this.btnBack1.Text == "启动")
            //{
            //    Context.ProcessDispatcher.WriteToProcess("CraneProcess", "Run", 1);
            //    this.btnBack1.Text = "停止";
            //}
            //else
            //{
            //    Context.ProcessDispatcher.WriteToProcess("CraneProcess", "Run", 0);
            //    this.btnBack1.Text = "启动";
            //}
        }

        private void btnBack1_Click(object sender, EventArgs e)
        {
            //int X = 1010;
            //if (i == 0)
            //    X = 1007;
            //int Y = 336;

            //Point P = new Point();
            //P.X = X - (int)(i * 24.51F);
            //P.Y = Y - 0 * 73;
            //this.picCrane1.Location = P;
            //if (i % 2 == 1)
            //{
            //    this.picCrane1.BackColor = Color.Red;
            //}
            //else
            //{
            //    picCrane1.BackColor = Color.Transparent;
            //}
            //i++;

           





            //if (MessageBox.Show("是否要召回1号堆垛机到初始位置?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            //{
            //    PutCommand("1", 7);
            //    Logger.Info("1号堆垛机下发召回命令");
            //}
        }
        private void PutCommand(string craneNo, byte TaskType)
        {
            //byte[] cellAddr = new byte[8];
            //cellAddr[0] = TaskType;
            //cellAddr[1] = 0;

            //for (int i = 0; i < cellAddr.Length; i++)
            //    cellAddr[i] += 48;

            //string serviceName = "CranePLC" + craneNo;
            //Context.ProcessDispatcher.WriteToService(serviceName, "TaskAddress", cellAddr);

            //Context.ProcessDispatcher.WriteToService(serviceName, "WriteFinished", 49);
        }

        private void btnStop1_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("是否要急停1号堆垛机?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            //{
            //    PutCommand("1", 8);
            //    Logger.Info("1号堆垛机下发急停命令");
            //}
        }

        private void btnBack2_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("是否要召回2号堆垛机到初始位置?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            //{
            //    PutCommand("2", 7);
            //    Logger.Info("2号堆垛机下发召回命令");
            //}
        }

        private void btnStop2_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("是否要急停2号堆垛机?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            //{
            //    PutCommand("2", 8);
            //    Logger.Info("2号堆垛机下发急停命令");
            //}
        }

        #endregion

        #region 条码检测故障，人工输入条码
        private void txtPalletCode117_DoubleClick(object sender, EventArgs e)
        {

            PalletCodeDoubleClick(5, 117);
        }

       

        private void txtPalletCode113_DoubleClick(object sender, EventArgs e)
        {
            PalletCodeDoubleClick(4, 113);
        }

        private void txtPalletCode109_DoubleClick(object sender, EventArgs e)
        {
            PalletCodeDoubleClick(3, 109);
        }

        private void txtPalletCode105_DoubleClick(object sender, EventArgs e)
        {
            PalletCodeDoubleClick(2, 105);
        }

        private void txtPalletCode101_DoubleClick(object sender, EventArgs e)
        {
            PalletCodeDoubleClick(1, 101);
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
                        sbyte[] staskNo = new sbyte[10];
                        sbyte[] sPallet = new sbyte[8];
                        Util.ConvertStringChar.stringToBytes(frm.TaskNo, 10).CopyTo(staskNo, 0);
                        Util.ConvertStringChar.stringToBytes(frm.PalletCode, 8).CopyTo(sPallet, 0);
                        Context.ProcessDispatcher.WriteToService("CarPLC", ItemNo + "_WriteFinished", 3);
                        Context.ProcessDispatcher.WriteToService("CarPLC", ItemNo + "_TaskNo", staskNo);
                        Context.ProcessDispatcher.WriteToService("CarPLC", ItemNo + "_PalletCode", sPallet);
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


    }
}
