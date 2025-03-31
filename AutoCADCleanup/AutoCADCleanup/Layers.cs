using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System.Globalization;
using CsvHelper;
using Exception = System.Exception;
using Color = Autodesk.AutoCAD.Colors.Color;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;


namespace AutoCADCleanup
{
    internal class Layers
    {
        //Method to create the layers from CSV and to move the entities to specific layer
        public List<string> ManageLayers(List<string> standarLayersList,List<string> unwantedLayerList, List<string> emptyLayerList)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    // Access the LayerTable
                    LayerTable layerTable = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;

                    // Access the BlockTableRecord (ModelSpace)
                    BlockTable blockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord modelSpace = trans.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    //Method to delete the layer on basis of unwanted layer list
                    //Not deleting the layer in this method as its giving error for snapshots, so preserving the layers name to delete this layers at the end

                    emptyLayerList = MoveUnwantedEntities(trans, layerTable, unwantedLayerList, modelSpace, db, ed, standarLayersList, emptyLayerList);
                    int layerCount = 0;

                    foreach (var layerId in layerTable)
                    {
                        LayerTableRecord? currentLayer = trans.GetObject(layerId, OpenMode.ForRead) as LayerTableRecord;
                        if(currentLayer != null)
                        {
                            // Skip if the layer is a standard layer
                            if (standarLayersList.Exists(sl => sl == currentLayer.Name) || (emptyLayerList.Exists(sl => sl == currentLayer.Name)))
                            {
                                layerCount++;
                                continue;
                            }   
                        }

                        // Check if the current layer matches any standard layer by name
                        foreach (var standardLayer in standarLayersList)
                        {
                            string standardSubstring = standardLayer.Substring(0, Math.Min(4, standardLayer.Length)).ToUpper();
                            //if (currentLayer.Name.IndexOf(standardLayer, StringComparison.OrdinalIgnoreCase) >= 0 || currentLayer.Name.ToUpper().Contains(standardSubstring))
                            if (currentLayer.Name.IndexOf(standardLayer, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                currentLayer.Name.IndexOf(standardSubstring, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                ed.WriteMessage($"\nMoving entities from layer '{currentLayer.Name}' to '{standardLayer}'.");

                                // Move entities from current layer to standard layer
                                MoveEntitiesToLayer(trans, modelSpace, blockTable, currentLayer.Name, standardLayer);

                                ed.WriteMessage($"\nLayer '{currentLayer.Name}' has been deleted.");
                            }
                        }
                    }
                    layerCount = layerCount + 1;
                }
                catch (Exception ex)
                { }
                trans.Commit();
                ed.WriteMessage("\nLayers cleaned and merged successfully.");
            }
            
            return emptyLayerList;
        }

        // Method to move entities from one layer to another
        private void MoveEntitiesToLayer(Transaction trans, BlockTableRecord modelSpace, BlockTable blockTable, string sourceLayerName, string targetLayerName)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor editor = doc.Editor;

            foreach (ObjectId objId in modelSpace)
            {
                Entity entity = trans.GetObject(objId, OpenMode.ForWrite) as Entity;
                if (entity != null && entity.Layer == sourceLayerName)
                {
                    entity.UpgradeOpen();
                    entity.Layer = targetLayerName;

                    entity.Color = Color.FromColorIndex(ColorMethod.ByLayer, 0);

                }
                else if (entity != null && entity is Autodesk.AutoCAD.DatabaseServices.BlockReference bf)
                {
                    if (blockTable.Has(bf.Name))
                    {
                        // Get the block definition
                        BlockTableRecord bdef = (BlockTableRecord)trans.GetObject(blockTable[bf.Name], OpenMode.ForWrite);

                        // Iterate through entities in the block definition
                        foreach (ObjectId eid in bdef)
                        {
                            Entity ent = trans.GetObject(eid, OpenMode.ForWrite) as Entity;


                            if (ent != null && ent.Layer == sourceLayerName)
                            {
                                entity.UpgradeOpen();
                                entity.Layer = targetLayerName; // Moves the entity to target layer 

                                entity.Color = Color.FromColorIndex(ColorMethod.ByLayer, 0);

                            }
                            else if (ent != null && entity is Autodesk.AutoCAD.DatabaseServices.BlockReference bff)
                            {
                                MoveBlockEntitiesToLayer(trans, blockTable, bff.Name, sourceLayerName, targetLayerName);
                            }
                        }
                    }
                }

            }

        }

        public void MoveBlockEntitiesToLayer(Transaction tr, BlockTable blockTable, string blockName, String sourceLayerName, String TargetLayerName)
        {
            if (blockTable.Has(blockName))
            {
                // Get the block definition
                BlockTableRecord bdef = (BlockTableRecord)tr.GetObject(blockTable[blockName], OpenMode.ForWrite);

                // Iterate through entities in the block definition
                foreach (ObjectId eid in bdef)
                {
                    Entity entity = tr.GetObject(eid, OpenMode.ForWrite) as Entity;

                    // Check the entity layer matches the standard layer name
                    if (entity != null && entity.Layer == sourceLayerName) // Example: Delete a Line entity
                    {
                        entity.UpgradeOpen();
                        entity.Layer = TargetLayerName; // Moves the data of source Layer name to target layer name
                    }
                    else if (entity != null && entity is Autodesk.AutoCAD.DatabaseServices.BlockReference bff)
                    {
                        MoveBlockEntitiesToLayer(tr, blockTable, bff.Name, sourceLayerName, TargetLayerName);
                    }
                }
            }
        }


        //Method to create the layers from the CSV file
        [CommandMethod("CreateLayersFromCSV")]
        public List<string> CreateLayersFromCSV()
        {
            // Prompt user to provide the path to the CSV file
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            var standarLayersList = new List<string>();
            try
            {

                string standardLayersFilepath = null;
                
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.Title = "Select Standard layer list";

                // Show the dialog and check if a file is selected
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the selected file path
                    standardLayersFilepath = openFileDialog.FileName;

                    // Check if the file exists
                    if (!File.Exists(standardLayersFilepath))
                    {
                        Console.WriteLine("\nFile does not exist.");
                        return null; // or handle as needed
                    }

                }
                else
                {
                    Console.WriteLine("\nNo file was selected.");
                }

                if (standardLayersFilepath != null)
                {
                    // Read CSV file and process it
                    using (var reader = new StreamReader(standardLayersFilepath))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        var records = csv.GetRecords<LayerData>().ToList();
                        //var records2 = csv.GetRecords<LayerData>();
                        CreateLayers(records);
                        foreach (var record in records)
                        {
                            standarLayersList.Add(record.Name);
                        }

                    }
                }
                ed.WriteMessage("\nLayers created successfully.");
            }
            catch (Exception ex)
            {
                ed.WriteMessage($"\nAn error occurred: {ex.Message}");
            }
            return standarLayersList;
        }

        // Method to create layers in AutoCAD
        private void CreateLayers(IEnumerable<LayerData> StandarLayerList)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                // Open the LayerTable for writing
                LayerTable layerTable = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForWrite);

                foreach (var layerData in StandarLayerList)
                {
                    if (!layerTable.Has(layerData.Name))
                    {
                        LayerTableRecord newLayer = new LayerTableRecord
                        {
                            Name = layerData.Name,
                            Color = Color.FromColorIndex(ColorMethod.ByAci, layerData.ColorIndex)
                        };

                        // Add the new layer to the table
                        layerTable.Add(newLayer);
                        trans.AddNewlyCreatedDBObject(newLayer, true);
                    }
                }
                // Commit the transaction
                trans.Commit();
            }
        }

        // Class to map CSV records
        public class LayerData
        {
            public string Name { get; set; }
            public short ColorIndex { get; set; }
        }

        // Method to read unwanted layers from the CSV file
        public List<string> ReadUnwantedLayers()
        {
            var unwantedLayers = new List<string>();
            string unwantedLayersFilepath = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog.Title = "Select Unwanted layer list";

            // Show the dialog and check if a file is selected
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file path
                unwantedLayersFilepath = openFileDialog.FileName;

                // Check if the file exists
                if (!File.Exists(unwantedLayersFilepath))
                {
                    Console.WriteLine("\nFile does not exist.");
                    return null; // or handle as needed
                }

            }
            else
            {
                Console.WriteLine("\nNo file was selected.");
            }

            using (var reader = new StreamReader(unwantedLayersFilepath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Map the data to a simple model for the layer name
                var records = csv.GetRecords<UnwantedLayer>();
                foreach (var record in records)
                {
                    unwantedLayers.Add(record.Name);
                }
            }

            return unwantedLayers;
        }

        // Class to map unwanted layer data
        public class UnwantedLayer
        {
            public string Name { get; set; }
        }
        
        private List<string> MoveUnwantedEntities(Transaction trans, LayerTable layerTable, List<string> unwantedList, BlockTableRecord modelSpace, Database db, Editor ed, List<string> standardLayerList, List<string> emptyLayerList)
        {
            try
            {

                //First we will create a layer and then move all the unwanted 
                // Open the Layer table for read
                LayerTable acLayerTable;
                acLayerTable = trans.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;

                // Check if the layer already exists
                if (!acLayerTable.Has("ToBeDeleted"))
                {
                    // Open the Layer table for write
                    acLayerTable.UpgradeOpen();

                    // Create a new Layer table record
                    LayerTableRecord layer = new LayerTableRecord();
                    layer.Name = "ToBeDeleted";

                    // Set the color and linetype to "ByLayer"
                    layer.Color = Color.FromColorIndex(ColorMethod.ByLayer, 0); // No specific color, ByLayer

                    // Add the new layer to the layer table
                    acLayerTable.Add(layer);
                    trans.AddNewlyCreatedDBObject(layer, true);
                    ed.WriteMessage("\nNew layer created successfully with no default color or linetype.");
                }
                else
                {
                    ed.WriteMessage("\nLayer 'MyNewLayer' already exists.");
                }
                
                foreach (ObjectId layerId in layerTable)
                {
                    LayerTableRecord layer = (LayerTableRecord)trans.GetObject(layerId, OpenMode.ForRead);

                    // Check if layer name matches any unwanted item
                    foreach (var unwanted in unwantedList)
                    {
                        string unwantedSubstring = unwanted.Substring(0, Math.Min(4, unwanted.Length)).ToUpper();
                        if (layer.Name.IndexOf(unwantedSubstring, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            layer.Name.IndexOf(unwantedSubstring, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            // Delete all entities on the layer
                            foreach (ObjectId objId in modelSpace)
                            {

                                Entity entity = trans.GetObject(objId, OpenMode.ForWrite) as Entity;
                                if (entity != null && entity.Layer == layer.Name)
                                {
                                    entity.Layer = "ToBeDeleted";
                                }
                                if (entity.Linetype.IndexOf("hidden", StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    entity.Layer = "ToBeDeleted";
                                }
                            }

                            if (!standardLayerList.Exists(sl => sl == layer.Name))
                            {
                                //Storing the name of the layer to be deleted afterwards
                                emptyLayerList.Add(layer.Name);
                                // Delete the layer (if empty and not the current layer)
                                //if (layer.Id != db.Clayer)
                                //{
                                //    layer.UpgradeOpen();
                                //    layer.Erase();
                                //}
                            }
                        }

                    }
                }
            }
            catch(Exception ex)
            {

            }
            return emptyLayerList;
        }

        public void PurgeUnused()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            // Get the Purge options
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                // Run Purge for unused items
                doc.SendStringToExecute("_.Purge All ", true, false, false);
                trans.Commit();
            }

            doc.Editor.WriteMessage("\nPurge operation completed.");
        }
    
      
    }
}
