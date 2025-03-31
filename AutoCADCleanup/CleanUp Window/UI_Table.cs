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
using System.Drawing;

namespace CleanUp_Window
{
    public partial class UI_Table : Form
    {
        private List<BlockPreview> blockPreviewsList = new List<BlockPreview>();
        // Constructor for initializing the form
        public UI_Table()
        {
            InitializeComponent();
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
            blockPreviewsList = nameList;
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
                    false,              // Delete column (checkbox - could be null or false by default)
                    standarLayersList.FirstOrDefault() // Default value for MoveTo column (you can modify this if needed)
                );
            }
        }

        private void Button_OK_Click(object sender, EventArgs e)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                LayerTable layerTabel = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);


                ObjectId toBeDeletedLayerId = layerTabel["ToBeDeleted"];

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[1].Value == null || row.Cells[2].Value == null || row.Cells[3].Value == null)
                        continue;


                    Bitmap image = row.Cells[0].Value as Bitmap;
                    if (image == null)
                        continue;

                    // Find the block preview using the image
                    BlockPreview blockPreview = blockPreviewsList.FirstOrDefault(bp => bp.Image == image);
                    if (blockPreview == null)
                    {
                        ed.WriteMessage($"No matching block found for the image.\n");
                        continue;
                    }

                    string blockLayerName = blockPreview.BlockLayer;
                    //string blockLayerName = row.Cells[1].Value.ToString();
                    bool isChecked = (bool)row.Cells[2].Value;
                    string selectedLayer = row.Cells[3].Value.ToString();

                    if (!layerTabel.Has(blockLayerName))
                    {
                        ed.WriteMessage($"Layer {blockLayerName} not found.\n");
                        continue;
                    }

                    ObjectId targetLayerId;
                    if (isChecked && layerTabel.Has(selectedLayer))
                    {
                        targetLayerId = layerTabel[selectedLayer];
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