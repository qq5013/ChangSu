

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MCP;
using Util;
using Dispatching.LED2008;


namespace Dispatching.Process
{
    public class LEDProcess2 : AbstractProcess
    {
        private string StockPLC = "CarPLC";
        private Dictionary<int, string> isActiveLeds = new Dictionary<int, string>();

        private delegate void delegateLedShow(string LedNo, string LedContext);
        private delegateLedShow LedShow = null;

        private Dictionary<string, string> dicStockWarning = new Dictionary<string, string>();

        private DataTable dtCraneError;

        public override void Release()
        {
            try
            {
                base.Release();
            }
            catch (Exception e)
            {
                Logger.Error("LEDProcess 资源释放失败，原因：" + e.Message);
            }
        }

        public override void Initialize(Context context)
        {
            try
            {
                base.Initialize(context);
                BLL.BLLBase bll = new BLL.BLLBase();
                dtCraneError = bll.FillDataTable("WCS.SelectCraneWarning");
                dicStockWarning.Clear();
                dicStockWarning.Add("1", "超高故障");
                dicStockWarning.Add("2", "前超长故障");
                dicStockWarning.Add("3", "后超长故障");
                dicStockWarning.Add("4", "左超宽故障");
                dicStockWarning.Add("5", "右超宽故障");
                dicStockWarning.Add("6", "条码检测故障");
                dicStockWarning.Add("7", "巷道不符");
                dicStockWarning.Add("8", "超时故障");
                dicStockWarning.Add("9", "无入库任务");
            }
            catch (Exception e)
            {
                Logger.Error("LEDProcess中Initialize初始化失败，原因：" + e.Message);
            }
        }

        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            object obj = ObjectUtil.GetObject(stateItem.State);
            if (obj == null)
                return;

            if (stateItem.ItemName.IndexOf("nAlarmCode") >= 0)
            {
                int CraneNo = int.Parse(stateItem.Name.Replace("CranePLC", "")) * 2;
                ShowLedMessage(CraneNo.ToString(), stateItem);
                return;

            }
            string ItemNo = stateItem.ItemName.Split('_')[0];
            ShowLedMessage(GetLedNo(int.Parse(ItemNo)), stateItem);

            try
            {
                if (stateItem.ItemName.IndexOf("TaskNo") >= 0 && (ItemNo == "102" || ItemNo == "106" || ItemNo == "110" || ItemNo == "114" || ItemNo == "118" || ItemNo == "103" || ItemNo == "107" || ItemNo == "111" || ItemNo == "115" || ItemNo == "119"))
                {
                    string Tasks = ConvertStringChar.BytesToString(ObjectUtil.GetObjects(stateItem.State));
                    Tasks = Tasks.PadRight(26, ' ');
                    string TaskNo = Tasks.Substring(0, 10).Trim();
                    if (TaskNo.Length > 0)
                    {

                        BLL.BLLBase bll = new BLL.BLLBase();
                        DataTable dtTask = bll.FillDataTable("WCS.SelectWCSTask", new DataParameter[] { new DataParameter("{0}", string.Format("TaskID='{0}'", TaskNo)) });
                        if (dtTask.Rows.Count > 0)
                        {
                            string PalletCode = dtTask.Rows[0]["PALLETID"].ToString();
                            switch (ItemNo)
                            {
                                case "102":
                                case "106":
                                case "110":
                                case "114":
                                case "118":
                                    //更新WCS_Task入库状态
                                    bll.ExecNonQuery("WCS.UpdateWCSTaskStateByStatus", new DataParameter[] { new DataParameter("{0}", "2"), new DataParameter("{1}", TaskNo), new DataParameter("{2}", 1) });
                                    Logger.Info("入库任务：" + TaskNo + "托盘编号：" + PalletCode + "到达入库端！");
                                    break;
                                case "103":
                                case "107":
                                case "111":
                                case "115":
                                case "119":
                                    //更新出库完成
                                    bll.ExecNonQueryTran("WCS.SpTaskFinished", new DataParameter[] { new DataParameter("VTaskNo", TaskNo) });
                                    Logger.Info("出库任务：" + TaskNo + "托盘编号：" + PalletCode + "完成任务！");
                                    break;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("LEDProcess中StateChanged出现异常" + ex.Message);
            }
        }

        private void ShowLedMessage(string LedNo, StateItem Item)
        {
            string txtContext = "";
            switch (LedNo)
            {
                case "1":
                    txtContext = GetLedMessage(LedNo, "101", Item) + Environment.NewLine + GetLedMessage(LedNo, "102", Item);
                    break;
                case "2":
                    txtContext = GetLedMessage(LedNo, "103", Item) + Environment.NewLine + GetLedMessage(LedNo, "104", Item);
                    break;
                case "3":
                    txtContext = GetLedMessage(LedNo, "105", Item) + Environment.NewLine + GetLedMessage(LedNo, "106", Item);
                    break;
                case "4":
                    txtContext = GetLedMessage(LedNo, "107", Item) + Environment.NewLine + GetLedMessage(LedNo, "108", Item);
                    break;
                case "5":
                    txtContext = GetLedMessage(LedNo, "109", Item) + Environment.NewLine + GetLedMessage(LedNo, "110", Item);
                    break;
                case "6":
                    txtContext = GetLedMessage(LedNo, "111", Item) + Environment.NewLine + GetLedMessage(LedNo, "112", Item);
                    break;
                case "7":
                    txtContext = GetLedMessage(LedNo, "113", Item) + Environment.NewLine + GetLedMessage(LedNo, "114", Item);
                    break;
                case "8":
                    txtContext = GetLedMessage(LedNo, "115", Item) + Environment.NewLine + GetLedMessage(LedNo, "116", Item);
                    break;
                case "9":
                    txtContext = GetLedMessage(LedNo, "117", Item) + Environment.NewLine + GetLedMessage(LedNo, "118", Item);
                    break;
                case "10":
                    txtContext = GetLedMessage(LedNo, "119", Item) + Environment.NewLine + GetLedMessage(LedNo, "120", Item);
                    break;
                default:
                    break;
            }
            RefreshLedShow(LedNo, txtContext);
        }

        private string GetLedMessage(string LedNo, string ItemNo, StateItem item)
        {
            string strMsg = "";
            try
            {
                string Stock = "外盘:";
                if (int.Parse(ItemNo) % 2 == 0)
                {
                    Stock = "内盘:";
                }
                string PalletCode = "";
                string CellCode = " 位:";
                string CraneErrorNo = "0";
                if (int.Parse(LedNo) % 2 == 0 && int.Parse(ItemNo) % 2 == 0)
                {
                    int CarNo = int.Parse(LedNo) / 2;
                    CraneErrorNo = ObjectUtil.GetObject(WriteToService("CranePLC" + CarNo, "nAlarmCode")).ToString();
                }
                if (CraneErrorNo != "0")
                {
                    string ErrMsg = "堆垛机未知错误";
                    DataRow[] drs = dtCraneError.Select(string.Format("warncode='{0}'", CraneErrorNo));
                    if (drs.Length > 0)
                        ErrMsg = drs[0]["WARNDESC"].ToString();

                    strMsg = ErrMsg;
                }
                else
                {
                    object[] obj = ObjectUtil.GetObjects(WriteToService(StockPLC, ItemNo + "_TaskNo"));
                    byte[] berr = new byte[1];
                    berr[0] = byte.Parse(obj[26].ToString());
                    string sTaskno = Util.ConvertStringChar.BytesToString(obj).PadLeft(27, ' ');
                    string errorNo = Util.ConvertStringChar.BytesToString(berr);
                    if (errorNo != "0" && errorNo.Length != 0)
                    {
                        PalletCode = sTaskno.Substring(10, 8).Trim();
                        strMsg = Stock + (PalletCode.Length > 0 ? PalletCode : "") + " " + dicStockWarning[errorNo];
                        if ( ItemNo == item.ItemName.Split('_')[0])
                            Logger.Error("输送线:" + ItemNo + (PalletCode.Length > 0 ? "托盘编号：" + PalletCode : "") + dicStockWarning[errorNo]);
                    }
                    else
                    {
                        PalletCode = sTaskno.Substring(10, 8).Trim();
                        if (PalletCode.Length > 0)
                            CellCode = " 位:" + sTaskno.Substring(18, 8).Trim();
                        strMsg = Stock + PalletCode + CellCode;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("LEDProcess中GetLedMessage异常：" + ex.Message);
            }
            return strMsg;
        }





        private void RefreshLedShow(string No, string LedContext)
        {
            if (LedShow == null)
            {
                LedShow = LED2008.LEDUtil.RefreshLED;
                LedShow.Invoke(No, LedContext);
            }
            else
                LedShow(No, LedContext);
        }
        private string GetLedNo(int ItemNo)
        {
            string LedNo = "1";
            if ((ItemNo - 100) % 2 == 0)
            {
                LedNo = ((ItemNo - 100) / 2).ToString();
            }
            else
            {
                LedNo = ((ItemNo - 100) / 2 + (ItemNo - 100) % 2).ToString();
            }
            return LedNo;
        }
    }
}