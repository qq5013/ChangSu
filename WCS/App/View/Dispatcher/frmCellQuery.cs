using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDAL;

namespace App.View.Dispatcher
{
    public partial class frmCellQuery : BaseForm
    {
        BLL.BLLBase bll = new BLL.BLLBase();
        
        private Dictionary<int, DataRow[]> shelf = new Dictionary<int, DataRow[]>();
        private Dictionary<int, string> ShelfCode = new Dictionary<int, string>();

        private DataTable cellTable = null;        
        private bool needDraw = false;
        private bool filtered = false;

        private int columns = 40;
        private int rows = 12;
        private int cellWidth = 0;
        private int cellHeight = 0;
        private int currentPage = 1;
        private int[] top = new int[2];
        private int left = 5;
        string CellCode = "";
      
        private bool IsWheel = true;

        public frmCellQuery()
        {
            InitializeComponent();
            //设置双缓冲
            SetStyle(ControlStyles.DoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint, true);

            Filter.EnableFilter(dgvMain);
            pnlData.Visible = true;
            pnlData.Dock = DockStyle.Fill;

            pnlChart.Visible = false;
            pnlChart.Dock = DockStyle.Fill;

            pnlChart.MouseWheel += new MouseEventHandler(pnlChart_MouseWheel);

            this.PColor.Visible = false;
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (bsMain.Filter.Trim().Length != 0)
                {
                    DialogResult result = MessageBox.Show("重新读入数据请选择'是(Y)',清除过滤条件请选择'否(N)'", "询问", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (result)
                    {
                        case DialogResult.No:
                            DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMain);
                            return;
                        case DialogResult.Cancel:
                            return;
                    }
                }
                sbShelf.Visible = false;
                ShelfCode.Clear();

                DataTable dtShelf = bll.FillDataTable("WCS.SelectWCSShelf");
                for (int i = 0; i < dtShelf.Rows.Count; i++)
                {
                    ShelfCode.Add(i + 1, dtShelf.Rows[i]["ShelfCode"].ToString());
                }

                btnRefresh.Enabled = false;
                btnChart.Enabled = false;

                pnlProgress.Top = (pnlMain.Height - pnlProgress.Height) / 3;
                pnlProgress.Left = (pnlMain.Width - pnlProgress.Width) / 2;
                pnlProgress.Visible = true;
                Application.DoEvents();

                cellTable = bll.FillDataTable("WCS.SelectWcsCell");
                bsMain.DataSource = cellTable;

                pnlProgress.Visible = false;
                btnRefresh.Enabled = true;
                btnChart.Enabled = true;
            }
            catch (Exception exp)
            {
                MessageBox.Show("读入数据失败，原因：" + exp.Message);
            }
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            if (cellTable != null && cellTable.Rows.Count != 0)
            {
                if (pnlData.Visible)
                {
                    this.PColor.Visible = true;
                    filtered = bsMain.Filter != null;
                    needDraw = true;
                    btnRefresh.Enabled = false;
                    pnlData.Visible = false;
                    pnlChart.Visible = true;
                    sbShelf.Visible = true;
                    btnChart.Text = "列表";
                }
                else
                {
                    this.PColor.Visible = false;
                    needDraw = false;
                    btnRefresh.Enabled = true;
                    pnlData.Visible = true;
                    pnlChart.Visible = false;
                    sbShelf.Visible = false;
                    btnChart.Text = "图形";
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlChart_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (needDraw)
                {
                    Font font = new Font("微软雅黑", 9);
                    SizeF size = e.Graphics.MeasureString("01-12", font);
                    float adjustHeight = Math.Abs(size.Height - cellHeight) / 2;
                    size = e.Graphics.MeasureString("13", font);
                    float adjustWidth = (cellWidth - size.Width) / 2;

                    for (int i = 0; i <= 1; i++)
                    {
                        int key = currentPage * 2 + i - 1;
                        if (!shelf.ContainsKey(key))
                        {
                            DataRow[] rows = cellTable.Select(string.Format("SHELFCODE='{0}'", ShelfCode[key]), "CELLCODE desc");
                            shelf.Add(key, rows);
                        }

                        DrawShelf(shelf[key], e.Graphics, top[i], font, adjustWidth);
                        int tmpLeft = left + columns * cellWidth + 4;
                        for (int j = 0; j < rows; j++)
                        {
                            string s = string.Format("{0}-{1}", Convert.ToString(key).PadLeft(2, '0'), Convert.ToString(rows - j).PadLeft(2, '0'));
                            e.Graphics.DrawString(s, font, Brushes.Black, tmpLeft, top[i] + (j + 1) * cellHeight + adjustHeight - 3);
                        }
                    }

                    if (filtered)
                    {
                        int i = currentPage * 2;
                        foreach (DataGridViewRow gridRow in dgvMain.Rows)
                        {
                            DataRowView cellRow = (DataRowView)gridRow.DataBoundItem;
                            int shelf = 0;
                            for (int j = 1; j <= ShelfCode.Count; j++)
                            {
                                if (ShelfCode[j].CompareTo(cellRow["SHELFCODE"].ToString()) >= 0)
                                {
                                    shelf = j;
                                    break;
                                }
                            }
                            if (shelf == i || shelf == i - 1)
                            {
                                int top = 0;
                                if (shelf % 2 == 0)
                                    top = pnlContent.Height / 2;

                                int column = Convert.ToInt32(cellRow["CELLCOLUMN"]) - 1;
                                int row = rows - Convert.ToInt32(cellRow["CELLROW"]) + 1;
                                int quantity = ReturnColorFlag(cellRow["PALLETCODE"].ToString(),cellRow["ERRORFLAG"].ToString());
                                FillCell(e.Graphics, top, row, column, quantity);
                            }
                        }
                    }
                    IsWheel = false;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        private void DrawShelf(DataRow[] cellRows, Graphics g, int top, Font font, float adjustWidth)
        {
            foreach (DataRow cellRow in cellRows)
            {
                int column = Convert.ToInt32(cellRow["CELLCOLUMN"]) - 1;
                int row = rows - Convert.ToInt32(cellRow["CELLROW"]) + 1;
                int quantity = ReturnColorFlag(cellRow["PalletCode"].ToString(), cellRow["ERRORFLAG"].ToString());


                int x = left + (columns - 1 - column) * cellWidth;
                int y = top + row * cellHeight;


                Pen pen = new Pen(Color.RoyalBlue, 1);
                g.DrawRectangle(pen, new Rectangle(x, y, cellWidth, cellHeight));
                //g.FillRectangle(Brushes.Yellow, new Rectangle(x+1, y+1, cellWidth-1, cellHeight-1))
               
                if (!filtered)
                {
                    FillCell(g, top, row, column, quantity);
                }
            }
            for (int j = columns; j > 0; j--)
            {
                g.DrawString(Convert.ToString(j), font, Brushes.Black, left + (columns - j) * cellWidth + adjustWidth, top + cellHeight * 13 + 3);
            }
            //for (int j = columns; j > 0; j--)
            //{
            //    if (j % 2 == 0)
            //        g.DrawString(Convert.ToString(j), font, Brushes.DarkCyan, left + (columns - j) * cellWidth + adjustWidth, top + cellHeight * 13 + 3);
            //}
        }

        private void FillCell(Graphics g, int top, int row, int column, int quantity)
        {
            int x = left + (columns - 1 - column) * cellWidth;
            int y = top + row * cellHeight;
            if (quantity == 1)  //有货
                g.FillRectangle(Brushes.Blue, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));
            else if (quantity == 5) //有问题
                g.FillRectangle(Brushes.Red, new Rectangle(x + 2, y + 2, cellWidth - 3, cellHeight - 3));

        }
        
        private void pnlChart_Resize(object sender, EventArgs e)
        {
            cellWidth = (pnlContent.Width - sbShelf.Width - 60) / columns;
            cellHeight = (pnlContent.Height / 2) / (rows + 2);

            top[0] = 0;
            top[1] = pnlContent.Height / 2 - 3;
        }

        private void pnlChart_MouseClick(object sender, MouseEventArgs e)
        {
            int i = e.Y < top[1] ? 0 : 1;
            int shelf = currentPage * 2 + i - 1;

            int column = columns - (e.X - left) / cellWidth;
            
            int row = rows - (e.Y - top[i]) / cellHeight + 1;

            if (column <= columns && row <= rows)
            {
              
                DataRow[] cellRows = cellTable.Select(string.Format("ShelfCode='{0}' AND CellColumn='{1}' AND CellRow='{2}'", ShelfCode[shelf], column, row));
                if (cellRows.Length != 0)
                    CellCode = cellRows[0]["CellCode"].ToString();
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (cellRows.Length != 0)
                    {
                        Dictionary<string, Dictionary<string, object>> properties = new Dictionary<string, Dictionary<string, object>>();
                        Dictionary<string, object> property = new Dictionary<string, object>();

                        property.Add("货架编号", ShelfCode[shelf]);
                        property.Add("列", column);
                        property.Add("层", row);
                        property.Add("托盘编号", cellRows[0]["PalletCode"]);
                        property.Add("任务号", cellRows[0]["TaskID"]);
                        property.Add("入库时间", cellRows[0]["InDate"]);
                        
                        string strState = "正常";
                        if (cellRows[0]["ErrorFlag"].ToString() == "1")
                            strState = "异常(" + cellRows[0]["MEMO"].ToString() + ")"; ;
                        property.Add("状态", strState);
                        properties.Add("货位信息", property);
                        if (cellRows[0]["PalletCode"].ToString().Length > 0)
                        {
                            frmCellDialog cellDialog = new frmCellDialog(properties);
                            cellDialog.ShowDialog();
                        }
                    }
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }

        }
     
        private void pnlChart_MouseEnter(object sender, EventArgs e)
        {
            pnlChart.Focus();
        }

        private void pnlChart_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0 && currentPage + 1 <= 5)
                sbShelf.Value = (currentPage) * 30;
            else if (e.Delta > 0 && currentPage - 1 >= 1)
                sbShelf.Value = (currentPage - 2) * 30;       
        }

        private void sbShelf_ValueChanged(object sender, EventArgs e)
        {
            int pos = sbShelf.Value / 30 + 1;
            if (pos != currentPage)
            {
                currentPage = pos;
                pnlChart.Invalidate();
            }
        }

        private void dgvMain_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgvMain.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dgvMain.RowHeadersDefaultCellStyle.Font, rectangle, dgvMain.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private int ReturnColorFlag(string PalletCode, string ErrFlag)
        {
            int Flag = 0;
            if (PalletCode != "")
            {
                Flag = 1;
            }

            if (ErrFlag == "1")
                Flag = 5;
            return Flag;
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataRow[] drs = cellTable.Select(string.Format("CellCode='{0}'", CellCode));
            if (drs.Length > 0)
            {
                DataRow dr = drs[0];
                frmCellOpDialog cellDialog = new frmCellOpDialog(dr, this.Context);
                cellDialog.ShowDialog();
            }
        }

        private int X, Y;
        private void pnlChart_MouseMove(object sender, MouseEventArgs e)
        {

            if (IsWheel) return;
            if (X != e.X || Y != e.Y)
            {
                int i = e.Y < top[1] ? 0 : 1;
                int shelf = currentPage * 2 + i - 1;

                int column = columns - (e.X - left) / cellWidth;
                int row = rows - (e.Y - top[i]) / cellHeight + 1;
                if (column <= columns && row <= rows && row > 0 && column > 0)
                {
                    string tip = "货架:" + shelf.ToString() + ";列:" + column.ToString() + ";层:" + row.ToString();
                    DataRow[] drs = cellTable.Select(string.Format("ShelfCode='{0}' AND CellColumn='{1}' AND CellRow='{2}'", ShelfCode[shelf], column, row));
                    if (drs.Length > 0)
                    {
                        if (drs[0]["PalletCode"].ToString() != "")
                        {
                            tip += Environment.NewLine + "托盘编号：" + drs[0]["PalletCode"].ToString() + Environment.NewLine + "入库时间：" + drs[0]["InDate"].ToString();
                        }
                    }
                    toolTip1.SetToolTip(pnlChart, tip);
                }
                else
                    toolTip1.SetToolTip(pnlChart, null);

                X = e.X;
                Y = e.Y;
            }
        }

        private void frmCellQuery_Load(object sender, EventArgs e)
        {
            btnRefresh_Click(null, null);
        }       
    }
}
