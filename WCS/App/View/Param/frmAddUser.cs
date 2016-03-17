using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;

namespace App.View.Param
{
    public partial class frmAddUser : Form
    {
        public frmAddUser()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.txtUser.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入用户！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.txtPWD.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入密码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {

                BLL.BLLBase bll = new BLL.BLLBase();
                DataTable dt = bll.FillDataTable("WCS.SelectUserByName", new DataParameter[] { new DataParameter("{0}", this.txtUser.Text.Trim()) });
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("该用户已经存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;

                }
                else
                {
                    bll.ExecNonQuery("WCS.InserUser", new DataParameter[] { new DataParameter("{0}", this.txtUser.Text.Trim()), new DataParameter("{1}", this.txtPWD.Text.Trim()) });
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入正确数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void txtPWD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK_Click(null, null);
            }
        }
    }
}
