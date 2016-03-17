using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util;
using DataGridViewAutoFilter;
using MCP;

namespace App.View.Param
{
    public partial class frmCraneHandle : BaseForm
    {
        BLL.BLLBase bll = new BLL.BLLBase();

        public frmCraneHandle()
        {
            InitializeComponent();
        }

        private void toolStripButton_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton_Refresh_Click(object sender, EventArgs e)
        {
            BindData();
            
        }

        
        private void BindData()
        {
            DataTable dt = bll.FillDataTable("WCS.SelectWCSCRANE");
            bsMain.DataSource = dt;
        }

        private void frmInStock_Load(object sender, EventArgs e)
        {
            //this.BindData();
            for (int i = 0; i < this.dgvMain.Columns.Count - 1; i++)
                ((DataGridViewAutoFilterTextBoxColumn)this.dgvMain.Columns[i]).FilteringEnabled = true;
        }

        private void dgvMain_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dgvMain.Rows[e.RowIndex].Selected == false)
                    {
                        dgvMain.ClearSelection();
                        dgvMain.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (dgvMain.SelectedRows.Count == 1)
                    {
                        dgvMain.CurrentCell = dgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex];
                        if (dgvMain.Rows[e.RowIndex].Cells["ColEnabled"].Value.ToString() == "禁用")
                        {
                            this.toolStripButton_Enabled.Enabled = true;
                            this.toolStripButton_NoEnabled.Enabled = false;
                        }
                        else
                        {
                            this.toolStripButton_Enabled.Enabled = false;
                            this.toolStripButton_NoEnabled.Enabled = true;
                        }
                    }                    
                   
                    
                }
            }
        }

        private void frmInStock_Activated(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void toolStripButton_Enabled_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null)
            {
                try
                {

                    string CraneNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString();

                    bll.ExecNonQuery("WCS.UpdateWCSCrane", new DataParameter[] { new DataParameter("{0}", 1), new DataParameter("{1}", CraneNo) });
                    Logger.Info("启用" + CraneNo + " 号堆垛机！");

                    BindData();
                }
                catch (Exception ex)
                {
                    Logger.Error("frmCraneHandle中启用堆垛机出现异常" + ex.Message);

                }
            }
        }

        private void toolStripButton_NoEnabled_Click(object sender, EventArgs e)
        {
            if (this.dgvMain.CurrentCell != null)
            {
                try
                {
                    string CraneNo = this.dgvMain.Rows[this.dgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString();
                    bll.ExecNonQuery("WCS.UpdateWCSCrane", new DataParameter[] { new DataParameter("{0}", 0), new DataParameter("{1}", CraneNo) });
                    Logger.Info("禁用" + CraneNo + " 号堆垛机！");

                    BindData();
                }
                catch (Exception ex)
                {
                    Logger.Error("frmCraneHandle中禁用堆垛机出现异常" + ex.Message);

                }
            }
        }
    }
}
