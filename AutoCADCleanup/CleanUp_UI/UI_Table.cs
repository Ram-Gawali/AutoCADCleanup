using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;


using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace CleanUp_Window
{
    public partial class UI_Table : Form
    {
        // Constructor for initializing the form
        public UI_Table()
        {
            InitializeComponent();
            Button okButton = new Button
            {
                Text = "OK",
                Location = new System.Drawing.Point(this.Width - 100, this.Height - 50)
            };
            okButton.Click += OkButton_Click;
            this.Controls.Add(okButton);
        }

        // Constructor for initializing with data
        public UI_Table(List<BlockPreview> nameList, List<string> standarLayersList)
        {
            InitializeComponent();
            try
            {
                AddOrModifyData(nameList, standarLayersList);
            }
            catch { }
        }

        // Method to add or modify data in the DataGridView
        public void AddOrModifyData(List<BlockPreview> nameList, List<string> standarLayersList)
        {
            // Clear existing rows before adding new data
            dataGridView1.Rows.Clear();

            // Set the DataSource of the ComboBox column once (not inside the loop)
            StandardLayerColumn.DataSource = standarLayersList;

            // Add rows for each element in the nameList
            foreach (BlockPreview element in nameList)
            {
                // Add a new row with the element's data and the DataSource of the ComboBox
                dataGridView1.Rows.Add(
                    element.Image,     // Snapshot column
                    element.Name,      // Name column
                    null,              // Delete column (checkbox - could be null or false by default)
                    standarLayersList.FirstOrDefault() // Default value for MoveTo column (you can modify this if needed)
                );
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);

         
                ObjectId toBeDeletedLayerId = lt["ToBeDeleted"];

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[1].Value == null || row.Cells[2].Value == null || row.Cells[3].Value == null)
                        continue;

                    string blockLayerName = row.Cells[1].Value.ToString();
                    bool isChecked = (bool)row.Cells[2].Value;
                    string selectedLayer = row.Cells[3].Value.ToString();

                    if (!lt.Has(blockLayerName))
                    {
                        ed.WriteMessage($"Layer {blockLayerName} not found.\n");
                        continue;
                    }

                    ObjectId targetLayerId;
                    if (isChecked && lt.Has(selectedLayer))
                    {
                        targetLayerId = lt[selectedLayer];
                    }
                    else
                    {
                        targetLayerId = toBeDeletedLayerId;
                    }

                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);

                    foreach (ObjectId objId in btr)
                    {
                        Entity ent = tr.GetObject(objId, OpenMode.ForWrite) as Entity;
                        if (ent != null && ent.Layer == blockLayerName)
                        {
                            ent.LayerId = targetLayerId;
                        }
                    }
                }

                tr.Commit();
                ed.WriteMessage("Entities moved based on selection.");
            }
        }
    }
}
