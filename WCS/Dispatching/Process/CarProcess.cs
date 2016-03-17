using System;
using System.Collections.Generic;
using System.Text;
using MCP;
using System.Data;

using Util;

namespace Dispatching.Process
{
    public class CarProcess : AbstractProcess
    {

        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {
                object obj = ObjectUtil.GetObject(stateItem.State);
                if (obj == null)
                    return;
                if (obj.ToString() == "0")
                    return;
                string ItemNo = stateItem.ItemName.Substring(0, 3);
                string PalletCode = ConvertStringChar.BytesToString((byte[]) WriteToService(stateItem.Name, ItemNo + "_PalletCode"));
                string RoadNo = GetRoadNo(ItemNo);
                if (PalletCode.Length > 0)
                {
                    BLL.BLLBase bll = new BLL.BLLBase();
                    DataTable dtTask = bll.FillDataTable("WCS.SelectWMSTask", new DataParameter[] { new DataParameter("{0}", string.Format("TASKSTATUS in ('0','3') AND TASKTYPE='IB' AND PALLETID='{1}' ", RoadNo, PalletCode)) });
                    if (dtTask.Rows.Count >0)
                    {
                        string ASRSID = dtTask.Rows[0]["ASRSID"].ToString();
                        if (ASRSID == RoadNo)
                        {
                            string TaskNo = dtTask.Rows[0]["TASKID"].ToString();
                            string TaskStatus = dtTask.Rows[0]["TASKSTATUS"].ToString();
                            sbyte[] staskNo = new sbyte[26];
                            string[] LedMsgs = dtTask.Rows[0]["LEDNO"].ToString().Split(',');
                            ConvertStringChar.stringToBytes(TaskNo, 10).CopyTo(staskNo, 0);
                            ConvertStringChar.stringToBytes(LedMsgs[0], 8).CopyTo(staskNo, 10);
                            ConvertStringChar.stringToBytes(LedMsgs[1], 8).CopyTo(staskNo, 18);
                            WriteToService(stateItem.Name, ItemNo + "_WriteTaskNo", staskNo);
                            if (WriteToService(stateItem.Name, ItemNo + "_WriteFinished", 1))
                            {
                                //插入WCS_Task
                                if (TaskStatus == "0")
                                {
                                    bll.ExecNonQueryTran("WCS.SPReciveWmsTask", new DataParameter[] { new DataParameter("VTASKNO", TaskNo) });
                                }
                                Logger.Info("第" + RoadNo.ToString() + "排托盘编号：" + PalletCode + "开始入库");
                            }
                        }
                        else
                        {
                            if (WriteToService(stateItem.Name, ItemNo + "_WriteFinished", 2))
                            {
                                // Logger.Error("第" + RoadNo.ToString() + "排托盘编号：" + PalletCode + "巷道不符！");
                            }
                        }
                    }
                    else
                    {
                        if (WriteToService(stateItem.Name, ItemNo + "_WriteFinished", 6))
                        {

                        } 
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("CarProcess：" + e.Message);
            }
        }

        private string GetRoadNo(string ItemNo)
        {
            string RoadNo = "1";
            switch (ItemNo)
            {
                case  "101":
                    RoadNo = "1";
                    break;
                case "105":
                    RoadNo = "2";
                    break;
                case "109":
                    RoadNo = "3";
                    break;
                case "113":
                    RoadNo = "4";
                    break;
                case "117":
                    RoadNo = "5";
                    break;
                default:
                    Logger.Error("CarProcess获取巷道号出错！");
                    break;
            }




            return RoadNo;
        }
    }
}