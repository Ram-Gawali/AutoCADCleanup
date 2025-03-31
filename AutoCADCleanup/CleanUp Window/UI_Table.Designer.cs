using System.Drawing;
using System.Windows.Forms;

namespace CleanUp_Window
{
    partial class UI_Table
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI_Table));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SnapShotColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeleteFromDrawingColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.StandardLayerColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Button_OK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SnapShotColumn,
            this.NameColumn,
            this.DeleteFromDrawingColumn,
            this.StandardLayerColumn});
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle15.Padding = new System.Windows.Forms.Padding(10);
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridView1.GridColor = System.Drawing.Color.Black;
            this.dataGridView1.Location = new System.Drawing.Point(-1, -2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 100;
            this.dataGridView1.RowTemplate.Height = 128;
            this.dataGridView1.Size = new System.Drawing.Size(627, 482);
            this.dataGridView1.TabIndex = 0;
            // 
            // SnapShotColumn
            // 
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle14.NullValue")));
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.SnapShotColumn.DefaultCellStyle = dataGridViewCellStyle14;
            this.SnapShotColumn.HeaderText = "SnapShot";
            this.SnapShotColumn.MinimumWidth = 8;
            this.SnapShotColumn.Name = "SnapShotColumn";
            this.SnapShotColumn.Width = 128;
            // 
            // NameColumn
            // 
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.MinimumWidth = 8;
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.Width = 200;
            // 
            // DeleteFromDrawingColumn
            // 
            this.DeleteFromDrawingColumn.HeaderText = "Move to layer";
            this.DeleteFromDrawingColumn.MinimumWidth = 8;
            this.DeleteFromDrawingColumn.Name = "DeleteFromDrawingColumn";
            this.DeleteFromDrawingColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DeleteFromDrawingColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.DeleteFromDrawingColumn.Width = 90;
            // 
            // StandardLayerColumn
            // 
            this.StandardLayerColumn.HeaderText = "Standard Layers";
            this.StandardLayerColumn.MinimumWidth = 8;
            this.StandardLayerColumn.Name = "StandardLayerColumn";
            this.StandardLayerColumn.Width = 200;
            // 
            // Button_OK
            // 
            this.Button_OK.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.Button_OK.Location = new System.Drawing.Point(551, 483);
            this.Button_OK.Name = "Button_OK";
            this.Button_OK.Size = new System.Drawing.Size(75, 33);
            this.Button_OK.TabIndex = 1;
            this.Button_OK.Text = "OK";
            this.Button_OK.UseVisualStyleBackColor = false;
            this.Button_OK.Click += new System.EventHandler(this.Button_OK_Click);
            // 
            // UI_Table
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(629, 519);
            this.Controls.Add(this.Button_OK);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UI_Table";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CleanUp Window";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewImageColumn SnapShotColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DeleteFromDrawingColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn StandardLayerColumn;
        private Button Button_OK;
    }
}
