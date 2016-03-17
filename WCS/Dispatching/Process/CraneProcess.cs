using System;
using System.Collections.Generic;
using System.Text;
using MCP;
using System.Data;

using System.Timers;
using Util;

namespace Dispatching.Process
{
    public class CraneProcess : AbstractProcess
    {
        private class rCrnStatus
        {
            
            public int io_flag { get; set; }

            public rCrnStatus()
            {
                io_flag = 0;
            }
        }

        // 记录堆垛机当前状态及任务相关信息
        BLL.BLLBase bll = new BLL.BLLBase();
        private Dictionary<int, int> dicCranStatus = new Dictionary<int, int>();
        private Timer tmWorkTimer = new Timer();
        private bool blRun = false;
        private string ServerName = "CranePLC";
        private DataTable dtCraneErr;
        private DataTable dtCrane;
        private int CranePLCStart = 1;


        public override void Initialize(Context context)
        {
            try
            {

                dtCrane = bll.FillDataTable("WCS.SelectWCSCRANE");

                //获取堆垛机信息
                for (int i = 1; i <= dtCrane.Rows.Count; i++)
                {
                    if (!dicCranStatus.ContainsKey(i))
                    {
                        dicCranStatus.Add(i, 0);
                    }
                }
                dtCraneErr = bll.FillDataTable("WCS.SelectCraneWarning");
                tmWorkTimer.Interval = 2000;
                tmWorkTimer.Elapsed += new ElapsedEventHandler(tmWorker);

                
                base.Initialize(context);
            }
            catch (Exception ex)
            {
                Logger.Error("CraneProcess中Initialize初始化出错，原因：" + ex.Message);
            }
        }
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            
            switch (stateItem.ItemName)
            {
                case "TaskFinish":
                    TaskFinishProcess(stateItem);
                    break;
                case "Run":
                    blRun = (int)stateItem.State == 1;
                    if (blRun)
                    {
                        tmWorkTimer.Start();
                        for (int i = 1; i <= dtCrane.Rows.Count; i++)
                        {
                            if (dtCrane.Rows[i - 1]["ISENABLED"].ToString() == "0")
                                continue;
                            WriteToService(ServerName + i.ToString(), "b_O_Auto", true);
                        }
                        Logger.Info("堆垛机联机");
                    }
                    else
                    {
                        tmWorkTimer.Stop();
                        Logger.Info("堆垛机脱机");
                    }
                    break;
                case "nAlarmCode":
                    ShowAlarm(stateItem);
                    break;
                default:
                    break;
            }
           

            return;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmWorker(object sender, ElapsedEventArgs e)
        {
            try
            {
                tmWorkTimer.Stop();

               for (int i = 1; i <= dtCrane.Rows.Count; i++)
               {
                   if (dtCrane.Rows[i - 1]["ISENABLED"].ToString() == "0")
                       continue;

                    //WriteToService(ServerName + i.ToString(), "b_O_HandShake", WriteToService(ServerName + i.ToString(), "b_I_HandShake"));
                    if (dicCranStatus[i] == 1)
                    {
                        CraneOut(i);
                    }
                    else
                    {
                        CraneIn(i);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Info("CraneProcess中tmWorker出现异常：" + ex.Message);
            }
            finally
            {
                tmWorkTimer.Start();
            }
        }
        /// <summary>
        /// 检查堆垛机入库状态
        /// </summary>
        /// <param name="piCrnNo"></param>
        /// <returns></returns>
        private bool CheckCraneStatus(int craneNo, bool blnOut)
        {
            try
            {
                string serviceName = "CranePLC" + craneNo;
                string nState = ObjectUtil.GetObject(WriteToService(serviceName, "nState")).ToString();
                if (nState.Equals("0"))
                {
                    //堆垛机就地模式指示（值：0-工作模式或1-就地模式，不接受任务）
                    string b_I_Local = ObjectUtil.GetObject(WriteToService(serviceName, "b_I_Local")).ToString();
                    //堆垛机当前运行模式指示1：自动模式0：半自动模式
                    string b_I_Auto = ObjectUtil.GetObject(WriteToService(serviceName, "b_I_Auto")).ToString();
                    //任务号
                    string ReadTaskNo = ConvertStringChar.BytesToString((byte[])WriteToService(serviceName, "ReadTaskNo"));
                    string b_I_Fork_Zero = ObjectUtil.GetObject(WriteToService(serviceName, "b_I_Fork_Zero")).ToString();
                    string ErrCode = ObjectUtil.GetObject(WriteToService(serviceName, "nAlarmCode")).ToString();
                    string TaskFinish = ObjectUtil.GetObject(WriteToService(serviceName, "TaskFinish")).ToString();
                    string CarTaskNo = "";
                    if (blnOut)
                    {
                       
                    }
                    if (nState.Equals("0") && b_I_Local.Equals("0") && b_I_Auto.Equals("1") && ReadTaskNo == "" && CarTaskNo == "" && b_I_Fork_Zero == "1" && ErrCode == "0" && (TaskFinish.ToLower().Equals("false") || TaskFinish.Equals("0"))) //  && TaskFinish.Equals("1") 
                        return true;
                    else
                        return false;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="craneNo"></param>
        private void CraneOut(int craneNo)
        {
            try
            {
                if (!CheckCraneStatus(craneNo, true))
                {
                    return;
                }
                dicCranStatus[craneNo] = 2;

                int CarNo = 104 + (craneNo - 1) * 4;
                string ReadTaskNo = ObjectUtil.GetObject(WriteToService("CarPLC", CarNo.ToString() + "_HasProduct")).ToString();
                if (ReadTaskNo == "1" || ReadTaskNo == "True")
                    return;


            }
            catch (Exception e)
            {
                dicCranStatus[craneNo] = 2;
                Logger.Error("CraneProcess中Craneout 状态检查错误:" + e.Message.ToString());
                return;
            }
            try
            {
                string Server = ServerName + craneNo.ToString();
                DataTable dtTask = bll.FillDataTable("WCS.SelectWmsTask", new DataParameter[] { new DataParameter("{0}", string.Format("TASKSTATUS='0' AND TASKTYPE in ('OB','MB') AND ASRSID='{0}'", craneNo)) });
                if (dtTask.Rows.Count > 0)
                {
                    string TaskNo = dtTask.Rows[0]["TASKID"].ToString();
                    string PalletCode = dtTask.Rows[0]["PALLETID"].ToString();

                    sbyte[] staskNo = new sbyte[20];
                    Util.ConvertStringChar.stringToBytes(TaskNo, 12).CopyTo(staskNo, 0);
                    Util.ConvertStringChar.stringToBytes(PalletCode, 8).CopyTo(staskNo, 12);
                    string strSLocation = dtTask.Rows[0]["SLOCATION"].ToString();
                    string strDLocation = dtTask.Rows[0]["DLOCATION"].ToString();
                    int[] Location = new int[6];
                    Location[0] = int.Parse(strSLocation.Split('-')[0]);
                    Location[1] = int.Parse(strSLocation.Split('-')[1]);
                    int fromRow= int.Parse(strSLocation.Split('-')[2]);
                    if (fromRow > 1)
                        fromRow += 1;
                    Location[2] = fromRow;
                    int D1 = 0;
                    try
                    {
                       int.TryParse(strDLocation.Split('-')[0], out D1);
                    }
                    catch(Exception etd)
                    {

                    }
                    if (D1 == 0)
                    {
                        Location[3] = 2 * craneNo;
                        Location[4] = 0;
                        Location[5] = 2;
                    }
                    else
                    {
                        Location[3] = int.Parse(strDLocation.Split('-')[0]);
                        Location[4] = int.Parse(strDLocation.Split('-')[1]);
                        int DRow = int.Parse(strDLocation.Split('-')[2]);
                        if (DRow > 1)
                            DRow += 1;
                        Location[5] = DRow;
                    }
                    WriteToService(Server, "Address", Location);
                    WriteToService(Server, "WriteTask", staskNo);
                    if (WriteToService(Server, "WriteFinish", true))
                    {
                        //接收WMS_Task
                        bll.ExecNonQueryTran("WCS.SPReciveWmsTask", new DataParameter[] { new DataParameter("VTASKNO", TaskNo) });
                        Logger.Info("出库任务:" + TaskNo + " 托盘编号：" + PalletCode + " 位:" + strSLocation + " 已下发给第" + craneNo + "堆垛机");
                    }
                    else
                    {
                        Logger.Error("CraneProcess出库任务:" + TaskNo + " 托盘编号：" + PalletCode + "无法写入堆垛机" + craneNo);
                    }
                }


            }
            catch (Exception ex)
            {
                Logger.Error("CraneProcess中CraneOut异常" + ex.Message.ToString());
                return;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="craneNo"></param>
        private void CraneIn(int craneNo)
        {
            try
            {
                if (!CheckCraneStatus(craneNo,false))
                    return;
                dicCranStatus[craneNo] = 1; 
            }
            catch (Exception e)
            {
                dicCranStatus[craneNo] = 1; 
                Logger.Error("CraneProcess中CraneIn状态检查错误:" + e.Message.ToString());
                return;
            }
            try
            {
                string Server = ServerName + craneNo.ToString();
                int CarItem = 102 + (craneNo - 1) * 4;
                string TaskNos = ConvertStringChar.BytesToString(ObjectUtil.GetObjects(WriteToService("CarPLC", CarItem.ToString() + "_TaskNo"))).PadLeft(27, ' ');
                if (TaskNos != "")
                {
                    string TaskNo = TaskNos.Substring(0, 10).Trim();
                    DataTable dtTask = bll.FillDataTable("WCS.SelectWCSTask", new DataParameter[] { new DataParameter("{0}", string.Format("TASKID='{0}' AND TASKSTATUS='2' AND ASRSID='{1}'", TaskNo, craneNo)) });

                    if (dtTask.Rows.Count > 0)
                    {
                        string PalletCode = dtTask.Rows[0]["PALLETID"].ToString();
                        sbyte[] staskNo = new sbyte[20];
                        Util.ConvertStringChar.stringToBytes(TaskNo, 12).CopyTo(staskNo, 0);
                        Util.ConvertStringChar.stringToBytes(PalletCode, 8).CopyTo(staskNo, 12);
                        string strDLocation = dtTask.Rows[0]["DLOCATION"].ToString();
                        int[] Location = new int[6];
                        Location[0] = 2 * craneNo - 1;
                        Location[1] = 0;
                        Location[2] = 2;
                        Location[3] = int.Parse(strDLocation.Split('-')[0]);
                        Location[4] = int.Parse(strDLocation.Split('-')[1]);
                        int ToRow = int.Parse(strDLocation.Split('-')[2]);
                        if (ToRow > 1)
                            ToRow += 1;
                        Location[5] = ToRow;
                       WriteToService(Server, "Address", Location);
                        WriteToService(Server, "WriteTask", staskNo);
                        if (WriteToService(Server, "WriteFinish", true))
                        {
                            //更新WCSTask状态为3
                            bll.ExecNonQuery("WCS.UpdateWCSTaskState", new DataParameter[] { new DataParameter("{0}", 3), new DataParameter("{1}", TaskNo) });
                            Logger.Info("入库任务:" + TaskNo + " 托盘编号：" + PalletCode + " 位:" + strDLocation + " 已下发给第" + craneNo + "堆垛机");
                        }
                        else
                        {
                            Logger.Error("CraneProcess入库任务:" + TaskNo + " 托盘编号：" + PalletCode + " 无法写入堆垛机" + craneNo);
                        }
                    }
                   
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CraneProcess中CraneIn异常:" + ex.Message);
                return;
            }

        }

        /// <summary>
        /// 堆垛机任务完成处理
        /// </summary>
        private void TaskFinishProcess(StateItem Item)
        {
            object obj = ObjectUtil.GetObject(Item.State);
            if (obj == null)
                return;
            string TaskFinish = obj.ToString();
            if (TaskFinish.Equals("False") || TaskFinish.Equals("0"))
                return;

            int CraneNo = int.Parse(Item.Name.Replace("CranePLC", ""));
            try
            {
                

                string TaskNo = ConvertStringChar.BytesToString(ObjectUtil.GetObjects(WriteToService(Item.Name, "ReadTaskNo")));

                if ((TaskFinish.Equals("True") || TaskFinish.Equals("1")) && TaskNo != "")
                {
                    DataTable dtTask = bll.FillDataTable("WCS.SelectWCSTask", new DataParameter[] { new DataParameter("{0}", string.Format("TASKID='{0}' and ASRSID='{1}'", TaskNo, CraneNo)) });
                    if (dtTask.Rows.Count > 0)
                    {
                        DataRow dr = dtTask.Rows[0];
                        string TaskType = dtTask.Rows[0]["TASKTYPE"].ToString();
                        string PalletCode = dtTask.Rows[0]["PALLETID"].ToString();
                        if (TaskType == "IB")//入库
                        {
                            //更新入库完成
                            bll.ExecNonQueryTran("WCS.SpTaskFinished", new DataParameter[] { new DataParameter("VTaskNo", TaskNo) });
                            sbyte[] taskNo = new sbyte[20];
                            ConvertStringChar.stringToBytes("", 20).CopyTo(taskNo, 0);
                            WriteToService(Item.Name, "GetRequest", false);
                            WriteToService(Item.Name, "PutRequest", false);
                            WriteToService(Item.Name, "ClearTaskNo", taskNo);
                            WriteToService(Item.Name, "TaskFinish", false);
                            Logger.Info("第" + CraneNo + "堆垛机入库任务：" + TaskNo + "托盘编号：" + PalletCode + "任务完成！");
                        }
                        else if (TaskType == "OB")//出库
                        {
                            //更新任务状态
                            bll.ExecNonQuery("WCS.UpdateWCSTaskState", new DataParameter[] { new DataParameter("{0}", 2), new DataParameter("{1}", TaskNo) });

                            //更新堆垛机PLC
                            sbyte[] taskNo = new sbyte[20];
                            ConvertStringChar.stringToBytes("", 20).CopyTo(taskNo, 0);

                            WriteToService(Item.Name, "GetRequest", false);
                            WriteToService(Item.Name, "PutRequest", false);
                            WriteToService(Item.Name, "ClearTaskNo", taskNo);
                            WriteToService(Item.Name, "TaskFinish", false);
                            Logger.Info("第" + CraneNo + "堆垛机出库任务：" + TaskNo + "托盘编号：" + PalletCode + "堆垛机任务完成！");
                            //下载任务到输送线。
                            int CarItem = 104 + (CraneNo - 1) * 4;

                            sbyte[] staskNo = new sbyte[26];
                            string[] LedMsgs = dtTask.Rows[0]["LEDNO"].ToString().Split(',');
                            ConvertStringChar.stringToBytes(TaskNo, 10).CopyTo(staskNo, 0);
                            ConvertStringChar.stringToBytes(LedMsgs[0], 8).CopyTo(staskNo, 10);
                            ConvertStringChar.stringToBytes(LedMsgs[1], 8).CopyTo(staskNo, 18);
                            WriteToService("CarPLC", CarItem + "_WriteTaskNo", staskNo);

                            if (WriteToService("CarPLC", CarItem + "_WriteFinished", 1))
                            {
                                bll.ExecNonQuery("WCS.UpdateWCSTaskState", new DataParameter[] { new DataParameter("{0}", 3), new DataParameter("{1}", TaskNo) });
                                Logger.Info("出库任务：" + TaskNo + "托盘编号：" + PalletCode + "任务下达" + CarItem + "输送线！");
                            }
                        }
                        else
                        {
                            //更新入库完成
                            bll.ExecNonQueryTran("WCS.SpTaskFinished", new DataParameter[] { new DataParameter("VTaskNo", TaskNo) });
                            sbyte[] taskNo = new sbyte[20];
                            WriteToService(Item.Name, "GetRequest", false);
                            WriteToService(Item.Name, "PutRequest", false);
                            ConvertStringChar.stringToBytes("", 20).CopyTo(taskNo, 0);
                            WriteToService(Item.Name, "ClearTaskNo", taskNo);
                            WriteToService(Item.Name, "TaskFinish", false);
                            Logger.Info("第" + CraneNo + "堆垛机任务：" + TaskNo + "托盘编号：" + PalletCode + "任务完成！");
                        }
                    }
                    else
                    {
                        WriteToService(Item.Name, "TaskFinish", false);
                        Logger.Error("CraneProcess第" + CraneNo + "堆垛机任务号：" + TaskNo + "在任务表中没有找到，请查询任务!");
                    }
                }
                else
                {
                    Logger.Error("CraneProcess第" + CraneNo + "堆垛机任务号为空！");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("CraneProcess 第" + CraneNo + "堆垛机Finish：" + ex.Message);
            }
        }


        private void ShowAlarm(StateItem Item)
        {
            try
            {
                object o = ObjectUtil.GetObject(Item.State);
                if (o == null)
                    return;
                string CraneNo = Item.Name.Replace("CranePLC", "");
                string strWarningCode = ObjectUtil.GetObject(Item.State).ToString();
                if (strWarningCode != "0")
                {
                    string strTaskNo = ConvertStringChar.BytesToString((byte[])WriteToService(Item.Name, "ReadTaskNo"));

                    DataRow[] drs = dtCraneErr.Select(string.Format("WARNCODE='{0}'", strWarningCode));

                    string strError = "";
                    if (drs.Length > 0)
                    {
                        strError = drs[0]["WARNDESC"].ToString();
                    }
                    else
                    {
                        strError = "未知错误！错误号：" + strWarningCode;
                    }
                    Logger.Error("第" + CraneNo + "堆垛机 " + strError);
                    if (strWarningCode == "505" || strWarningCode == "506")//重入异常
                    {
                        bll.ExecNonQueryTran("WCS.SPHandleTaskError", new DataParameter[] { new DataParameter("VTASKNO", strTaskNo), new DataParameter("VERRORCODE", strWarningCode) });
                    }
                }
                

            }
            catch (Exception ex)
            {
                Logger.Error("CraneProcess处理堆垛机ShowAlarm异常：" + ex.Message);
            }
        }
    }
}