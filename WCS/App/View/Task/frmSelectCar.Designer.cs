namespace App.View.Task
{
    partial class frmSelectCar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbt1 = new System.Windows.Forms.RadioButton();
            this.rbt2 = new System.Windows.Forms.RadioButton();
            this.rbt3 = new System.Windows.Forms.RadioButton();
            this.rbt4 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(167, 121);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 33);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(63, 121);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 33);
            this.btnOk.TabIndex = 19;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbt4);
            this.groupBox1.Controls.Add(this.rbt3);
            this.groupBox1.Controls.Add(this.rbt2);
            this.groupBox1.Controls.Add(this.rbt1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 85);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择输送线";
            // 
            // rbt1
            // 
            this.rbt1.AutoSize = true;
            this.rbt1.Location = new System.Drawing.Point(14, 45);
            this.rbt1.Name = "rbt1";
            this.rbt1.Size = new System.Drawing.Size(41, 16);
            this.rbt1.TabIndex = 0;
            this.rbt1.TabStop = true;
            this.rbt1.Text = "101";
            this.rbt1.UseVisualStyleBackColor = true;
            // 
            // rbt2
            // 
            this.rbt2.AutoSize = true;
            this.rbt2.Location = new System.Drawing.Point(80, 45);
            this.rbt2.Name = "rbt2";
            this.rbt2.Size = new System.Drawing.Size(41, 16);
            this.rbt2.TabIndex = 1;
            this.rbt2.TabStop = true;
            this.rbt2.Text = "101";
            this.rbt2.UseVisualStyleBackColor = true;
            // 
            // rbt3
            // 
            this.rbt3.AutoSize = true;
            this.rbt3.Location = new System.Drawing.Point(146, 45);
            this.rbt3.Name = "rbt3";
            this.rbt3.Size = new System.Drawing.Size(41, 16);
            this.rbt3.TabIndex = 2;
            this.rbt3.TabStop = true;
            this.rbt3.Text = "101";
            this.rbt3.UseVisualStyleBackColor = true;
            // 
            // rbt4
            // 
            this.rbt4.AutoSize = true;
            this.rbt4.Location = new System.Drawing.Point(212, 45);
            this.rbt4.Name = "rbt4";
            this.rbt4.Size = new System.Drawing.Size(41, 16);
            this.rbt4.TabIndex = 3;
            this.rbt4.TabStop = true;
            this.rbt4.Text = "101";
            this.rbt4.UseVisualStyleBackColor = true;
            // 
            // frmSelectCar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 183);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectCar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "清输送线任务号";
            this.Load += new System.EventHandler(this.frmSelectCar_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbt4;
        private System.Windows.Forms.RadioButton rbt3;
        private System.Windows.Forms.RadioButton rbt2;
        private System.Windows.Forms.RadioButton rbt1;
    }
}