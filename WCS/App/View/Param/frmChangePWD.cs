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
    public partial class frmChangePWD : Form
    {
        public frmChangePWD()
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
            if (this.txtNewPWD.Text.Trim().Length == 0)
            {
                MessageBox.Show("请输入新密码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.txtNewPWD.Text.Trim()!=this.txtNewPWD2.Text.Trim())
            {
                MessageBox.Show("密码不一致！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BLL.BLLBase bll = new BLL.BLLBase();
            DataTable dt = bll.FillDataTable("WCS.SelectUser", new DataParameter[] { new DataParameter("{0}", this.txtUser.Text.Trim()), new DataParameter("{1}", this.txtPWD.Text.Trim()) });
            if (dt.Rows.Count > 0)
            {
                bll.ExecNonQuery("WCS.UpdateUser", new DataParameter[] { new DataParameter("{0}", this.txtUser.Text.Trim()), new DataParameter("{1}", this.txtNewPWD.Text.Trim()) });



                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("用户名或密码不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
