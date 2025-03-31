namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            SnapShot = new DataGridViewImageColumn();
            Name = new DataGridViewTextBoxColumn();
            DeleteFromDrawing = new DataGridViewTextBoxColumn();
            DeletFromDrawing = new DataGridViewCheckBoxColumn();
            StandardLayers = new DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { SnapShot, Name, DeleteFromDrawing, DeletFromDrawing, StandardLayers });
            dataGridView1.Location = new Point(12, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(785, 69);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // SnapShot
            // 
            SnapShot.HeaderText = "Snap Shot";
            SnapShot.Name = "SnapShot";
            // 
            // Name
            // 
            Name.HeaderText = "Name";
            Name.Name = "Name";
            // 
            // DeleteFromDrawing
            // 
            DeleteFromDrawing.HeaderText = "Column1";
            DeleteFromDrawing.Name = "DeleteFromDrawing";
            // 
            // DeletFromDrawing
            // 
            DeletFromDrawing.HeaderText = "Delete From Drawing or Not";
            DeletFromDrawing.Name = "DeletFromDrawing";
            // 
            // StandardLayers
            // 
            StandardLayers.HeaderText = "StandarLayers";
            StandardLayers.Name = "StandardLayers";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private DataGridViewImageColumn SnapShot;
        private DataGridViewTextBoxColumn Name;
        private DataGridViewTextBoxColumn DeleteFromDrawing;
        private DataGridViewCheckBoxColumn DeletFromDrawing;
        private DataGridViewComboBoxColumn StandardLayers;
    }
}
