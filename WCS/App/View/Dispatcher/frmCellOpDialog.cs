using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Util;
using MCP;
 

namespace App.View.Dispatcher
{
  
    public partial class frmCellOpDialog : Form
    {
        Context context;
        DataRow dr;
        string CellCode;
        BLL.BLLBase bll = new BLL.BLLBase();
        
        public frmCellOpDialog()
        {
            InitializeComponent();
        }
        public frmCellOpDialog(DataRow dr,Context context1)
        {
            InitializeComponent();
            this.dr = dr;
            context = context1;
        }

        private void CellOpDialog_Load(object sender, EventArgs e)
        {
            CellCode = dr["CellCode"].ToString();
            this.txtCellCode.Text = CellCode;
            this.txtPalletCode.Text = dr["PalletCode"].ToString();
            this.txtTaskID.Text = dr["TaskID"].ToString();
            if (dr["InDate"].ToString() == "")
            {
                this.dtpInDate.Checked = false;
            }
            else
            {
                this.dtpInDate.Checked = true;
                this.dtpInDate.Value = DateTime.Parse(dr["InDate"].ToString());
            }
            if (dr["ErrorFlag"].ToString() == "0")
            {
                this.chkError.Checked = false;
            }
            else
            {
                this.chkError.Checked = true;
                this.txtMemo.Text = dr["MEMO"].ToString();
            }

            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要对货位" + this.txtCellCode.Text + "修改吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                string ErrorFlag = this.chkError.Checked ? "1" : "0";
                string sql = string.Format(" ErrorFlag='{0}'", ErrorFlag);
                sql += string.Format(",PALLETCODE='{0}'", this.txtPalletCode.Text.Trim());
                sql += string.Format(",TASKID='{0}'", this.txtTaskID.Text.Trim());
                if (this.dtpInDate.Checked)
                    sql += string.Format(",InDate='{0}'", this.dtpInDate.Value);

                DataParameter [] param = new DataParameter[] { new DataParameter("{0}", sql), new DataParameter("{1}", string.Format("CellCode='{0}'", this.txtCellCode.Text)) };
                bll.ExecNonQuery("WCS.UpdateCellByFilter", param);




                Logger.Info("货位：" + CellCode + "编辑成功！");
            }
            DialogResult = DialogResult.OK;
        }

  

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
         
    }
}
