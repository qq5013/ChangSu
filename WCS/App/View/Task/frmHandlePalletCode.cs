using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;

namespace App.View.Task
{
    public partial class frmHandlePalletCode : Form
    {
        private int ItemNo;
       
        private int CraneNo;
        public string TaskNo;
        public string TaskStatus;
        public string PalletCode;
        public string LedNo;

        public frmHandlePalletCode()
        {
            InitializeComponent();
        }
        public frmHandlePalletCode(int itemno, int craneno)
        {
            InitializeComponent();
            ItemNo = itemno;
            CraneNo = craneno;
        }

        private void frmHandlePalletCode_Load(object sender, EventArgs e)
        {
            this.txtItemNo.Text = ItemNo.ToString();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.txtPalletCode.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入条码！");
                return;
            }
            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable dtTask = bll.FillDataTable("WCS.SelectWMSTask", new DataParameter[] { new DataParameter("{0}", string.Format("TASKSTATUS in ('0','3') AND TASKTYPE='IB' AND PALLETID='{0}' ", this.txtPalletCode.Text.Trim())) });
            if (dtTask.Rows.Count > 0)
            {
                if (dtTask.Rows[0]["ASRSID"].ToString() == CraneNo.ToString())
                {
                    TaskNo = dtTask.Rows[0]["TASKID"].ToString();
                    TaskStatus = dtTask.Rows[0]["TASKSTATUS"].ToString();
                    PalletCode = this.txtPalletCode.Text.Trim();
                    LedNo = dtTask.Rows[0]["LEDNO"].ToString();
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("巷道不符！");
                    return;
                }
            }
            else
            {
                MessageBox.Show("无入库任务！");
                return;

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void txtPalletCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(null, null);
            }
        }
    }
}
