using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace App.View.Task
{
    public partial class frmSelectCar : Form
    {
        int[] Cars;
        public string SelectText;
        public frmSelectCar()
        {
            InitializeComponent();
        }

        public frmSelectCar(int [] cars)
        {
            InitializeComponent();
            Cars=cars;
        }
        private void frmSelectCar_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= Cars.Length; i++)
            {
                RadioButton r = GetRadioButton(i);
                r.Text = Cars[i - 1].ToString();
            }

        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            RadioButton r = null;
            for (int i = 1; i <= Cars.Length; i++)
            {
                r = GetRadioButton(i);
                if (r.Checked)
                {
                    break;
                }
            }

            if (r.Checked)
            {
                if (MessageBox.Show("是否要清除" + r.Text + "输送线任务号?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    SelectText = r.Text;
                    this.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                MessageBox.Show("请选择要清除的输送线！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        RadioButton GetRadioButton(int CraneNo)
        {
            Control[] ctl = this.Controls.Find("rbt" + CraneNo.ToString(), true);
            if (ctl.Length > 0)
                return (RadioButton)ctl[0];
            else
                return null;
        }

       
    }
}
