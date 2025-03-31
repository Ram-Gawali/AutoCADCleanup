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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI_Table));
            dataGridView1 = new DataGridView();
            SnapShotColumn = new DataGridViewImageColumn();
            NameColumn = new DataGridViewTextBoxColumn();
            DeleteFromDrawingColumn = new DataGridViewCheckBoxColumn();
            StandardLayerColumn = new DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Info;
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 6.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { SnapShotColumn, NameColumn, DeleteFromDrawingColumn, StandardLayerColumn });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.Padding = new Padding(10);
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.GridColor = Color.Black;
            dataGridView1.Location = new Point(-1, -2);
            dataGridView1.Margin = new Padding(4, 3, 4, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 100;
            dataGridView1.RowTemplate.Height = 128;
            dataGridView1.Size = new Size(732, 556);
            dataGridView1.TabIndex = 0;
            // 
            // SnapShotColumn
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = resources.GetObject("dataGridViewCellStyle2.NullValue");
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            SnapShotColumn.DefaultCellStyle = dataGridViewCellStyle2;
            SnapShotColumn.HeaderText = "SnapShot";
            SnapShotColumn.MinimumWidth = 8;
            SnapShotColumn.Name = "SnapShotColumn";
            SnapShotColumn.Width = 128;
            // 
            // NameColumn
            // 
            NameColumn.HeaderText = "Name";
            NameColumn.MinimumWidth = 8;
            NameColumn.Name = "NameColumn";
            NameColumn.Width = 200;
            // 
            // DeleteFromDrawingColumn
            // 
            DeleteFromDrawingColumn.HeaderText = "Move to layer";
            DeleteFromDrawingColumn.MinimumWidth = 8;
            DeleteFromDrawingColumn.Name = "DeleteFromDrawingColumn";
            DeleteFromDrawingColumn.Resizable = DataGridViewTriState.True;
            DeleteFromDrawingColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            DeleteFromDrawingColumn.Width = 90;
            // 
            // StandardLayerColumn
            // 
            StandardLayerColumn.HeaderText = "Standard Layers";
            StandardLayerColumn.MinimumWidth = 8;
            StandardLayerColumn.Name = "StandardLayerColumn";
            StandardLayerColumn.Width = 200;
            // 
            // UI_Table
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonHighlight;
            ClientSize = new Size(611, 443);
            Controls.Add(dataGridView1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "UI_Table";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "CleanUp Window";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewImageColumn SnapShotColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DeleteFromDrawingColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn StandardLayerColumn;
    }
}
