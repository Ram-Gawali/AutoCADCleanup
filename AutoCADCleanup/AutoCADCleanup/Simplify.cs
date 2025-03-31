using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using CleanUp_Window;
using System.Drawing;
using System.Drawing.Imaging;
//using System.Drawing;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Exception = System.Exception;
using Path = System.IO.Path;

[assembly: CommandClass(typeof(AutoCADCleanup.Cleanup))]

namespace AutoCADCleanup
{
    public class Cleanup
    {
        // This method will get called on click of Simply button.
        [CommandMethod("SimplifyCleanUP", CommandFlags.Modal | CommandFlags.UsePickSet)]
        public  void Simplify()
        {
            try
            {
                int layerCount = 0;
                layerCount = GetLayerCount();

                //To set all entities in the layers  by layer from by block.
                ChangeAllByBlockToByLayer();

                Layers layers = new Layers();
                List<string> emptyLayersList = new List<string>();
                List<string> standarLayersList = layers.CreateLayersFromCSV();
                List<string> unwantedLayerList = layers.ReadUnwantedLayers();
                List<CleanUp_Window.BlockPreview> blockPreviews = new List<CleanUp_Window.BlockPreview>(); // Store name & image

                //Manage layers function will create the standard layers and move the entities to the respected layer also remove unwanted layers from unwanted layers list.
                emptyLayersList = layers.ManageLayers(standarLayersList, unwantedLayerList, emptyLayersList);

                //
                layerCount = GetLayerCount();

                //For taking the snapshots of the block references
                blockPreviews = GenerateBlockPreviews2(blockPreviews, standarLayersList);

                //Call for UI
                Table(blockPreviews, standarLayersList);

                //Method for checking if the layers are empty or not
                EraseEmptyLayers(emptyLayersList);

                layerCount = GetLayerCount();
            }
            catch (Exception ex)
            { }
        }

        static public List<CleanUp_Window.BlockPreview> GenerateBlockPreviews2(List<CleanUp_Window.BlockPreview> blockPreviews, List<string> standarLayersList)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            try
            {
                string iconPath = " ";

                // Use FolderBrowserDialog for selecting the output directory
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Select the folder to save snapshots";
                    folderDialog.ShowNewFolderButton = true;

                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        iconPath = folderDialog.SelectedPath;
                    }
                    else
                    {
                        ed.WriteMessage("\nNo folder selected. Aborting.");
                        return null;
                    }
                }

                // Ensure the output directory exists
                if (!Directory.Exists(iconPath))
                {
                    Directory.CreateDirectory(iconPath);
                }

                int numIcons = 0;

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    BlockTable table = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    LayerTable layerTable = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);

                    string blockLayer = string.Empty;

                    ObjectId toBeDeletedLayerId = ObjectId.Null;
                    if (layerTable.Has("ToBeDeleted"))
                    {
                        toBeDeletedLayerId = layerTable["ToBeDeleted"];
                    }

                    // Convert Standard Layer Names to ObjectIds for easy comparison
                    HashSet<ObjectId> standardLayerIds = new HashSet<ObjectId>();
                    foreach (string layer in standarLayersList)
                    {
                        if (layerTable.Has(layer))
                        {
                            standardLayerIds.Add(layerTable[layer]);
                        }
                    }

                    foreach (ObjectId blkId in table)
                    {
                        DBObject obj = tr.GetObject(blkId, OpenMode.ForRead);
                        BlockTableRecord blk = (BlockTableRecord)tr.GetObject(blkId, OpenMode.ForRead);

                        // Ignore layouts and anonymous blocks
                        if (blk.IsLayout || blk.IsAnonymous)
                            continue;

                        bool skipSnapshot = false;
                        foreach (ObjectId entId in blk)
                        {
                            Entity entity = tr.GetObject(entId, OpenMode.ForRead) as Entity;
                            if (entity == null) continue;

                            if (entity.LayerId == toBeDeletedLayerId || standardLayerIds.Contains(entity.LayerId))
                            {
                                skipSnapshot = true;
                                break;
                            }
                        }

                        if (skipSnapshot)
                        {
                            ed.WriteMessage($"\nSkipping block '{blk.Name}' as it contains entities on a restricted layer.");
                            continue;
                        }


                        string outputFile = Path.Combine(iconPath, blk.Name + ".jpeg");

                        // Open block in a temporary drawing for high-resolution rendering
                        using (Database tempDb = new Database(true, true))
                        {
                            if (blk.IsErased)
                            {
                                ed.WriteMessage($"\nBlock '{blk.Name}' is erased, skipping.");
                                continue;
                            }
                            db.WblockCloneObjects(new ObjectIdCollection(new ObjectId[] { blkId }), tempDb.BlockTableId, new IdMapping(), DuplicateRecordCloning.Replace, false);
                            using (Transaction tempTr = tempDb.TransactionManager.StartTransaction())
                            {
                                BlockTable tempTable = (BlockTable)tempTr.GetObject(tempDb.BlockTableId, OpenMode.ForRead);
                                BlockTableRecord tempBlk = (BlockTableRecord)tempTr.GetObject(tempTable[blk.Name], OpenMode.ForRead);

                                // Ensure the block is not erased before continuing
                                if (tempBlk.IsErased)
                                {
                                    ed.WriteMessage($"\nBlock '{blk.Name}' is erased, skipping.");
                                    continue;
                                }

                                // Generate a larger preview image
                                if (tempBlk.PreviewIcon != null)
                                {
                                    using (Bitmap bitmap = new Bitmap(128, 128)) // Increase resolution
                                    {
                                        using (Graphics g = Graphics.FromImage(bitmap))
                                        {
                                            g.Clear(System.Drawing.Color.White);
                                            ViewTableRecord view = new ViewTableRecord();
                                            view.CenterPoint = new Point2d(64, 64);
                                            view.Height = 125;
                                            view.Width = 125;

                                            using (Viewport vp = new Viewport())
                                            {
                                                //vp.SetView(view);
                                                g.DrawImage(tempBlk.PreviewIcon, 0, 0, 128, 128);
                                            }
                                        }

                                        bitmap.Save(outputFile, ImageFormat.Jpeg);


                                        foreach (ObjectId blkRefId in tempBlk)
                                        {
                                            Entity entity = (Entity)tempTr.GetObject(blkRefId, OpenMode.ForRead);
                                            //if (entity is BlockReference blockRef)
                                            //{
                                            //    // Get the layer of the BlockReference
                                            //    blockLayer = entity.Layer;
                                            //    break; // Assuming we only care about the first BlockReference's layer
                                            //}
                                            blockLayer = entity.Layer;
                                            break;
                                        }

                                        blockPreviews.Add(new CleanUp_Window.BlockPreview(blk.Name, new Bitmap(bitmap),blockLayer));
                                        numIcons++;
                                        ed.WriteMessage($"\nLarger icon for block '{blk.Name}' saved to {outputFile}");
                                    }
                                }
                                else
                                {
                                    ed.WriteMessage($"\nNo preview icon available for block '{blk.Name}', skipping.");
                                }
                                tempTr.Commit();
                            }
                        }
                    }
                    tr.Commit();
                }

                //ed.WriteMessage("\n{0} block icons saved to \"{1}\".", numIcons, iconPath);
            }
            catch (Exception ex)
            {
                ed.WriteMessage("\nError: " + ex.Message);
            }
            return blockPreviews;
        }

        static public void Table(List<CleanUp_Window.BlockPreview> blockPreviews, List<string> standarLayersList)
        {
            try
            {
                if (blockPreviews.Count != 0)
                {
                    CleanUp_Window.UI_Table table = new CleanUp_Window.UI_Table(blockPreviews, standarLayersList);
                    table.ShowDialog();
                }
            }
            catch (Exception ex) 
            { return; }
            
        }

        public static void EraseEmptyLayers(List<string> layerNames)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                LayerTable layerTable = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);
                BlockTable blockTable = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead);

                foreach (string layerName in layerNames)
                {
                    if (!layerTable.Has(layerName))
                    {
                        ed.WriteMessage($"Layer '{layerName}' not found.\n");
                        // Consider not found layers as not empty
                        continue;
                    }

                    ObjectId layerId = layerTable[layerName];
                    LayerTableRecord layerRecord = tr.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;
                    bool hasEntities = false;

                    // Check for entities on the layer
                    foreach (ObjectId objId in btr)
                    {
                        Entity ent = tr.GetObject(objId, OpenMode.ForRead) as Entity;
                        if (ent != null && ent.Layer == layerName)
                        {
                            hasEntities = true;
                            break;
                        }
                    }

                    if (hasEntities)
                    {
                        ed.WriteMessage($"\nLayer '{layerName}' is not empty and cannot be deleted.");
                        continue;
                    }

                    //Delete layer
                    if (layerRecord != null)
                    {
                        layerRecord.Erase();
                    }
                    
                    ed.WriteMessage($"Layer '{layerName}' is {(hasEntities ? "empty" : "not empty")}.\n");
                }
                tr.Commit();
            }
        }

        public int GetLayerCount()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            var layerCount = 0;

            using (Transaction tr = db.TransactionManager.StartTransaction()) 
            {
                LayerTable? layerTable = tr.GetObject(db.LayerTableId, OpenMode.ForRead)as LayerTable;
                foreach(ObjectId layer in layerTable)
                {
                    layerCount++;
                }
                tr.Commit();
            }
            return layerCount;
        }

        public void ChangeAllByBlockToByLayer()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                // Access the BlockTable
                BlockTable? blockTable = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                
                if(blockTable != null)
                {
                    foreach (ObjectId btrId in blockTable)
                    {
                        BlockTableRecord? btr = tr.GetObject(btrId, OpenMode.ForRead) as BlockTableRecord;

                        // Skip layout blocks (e.g., Model Space and Paper Space)
                        if (btr.IsLayout) continue;

                        btr.UpgradeOpen(); // Open for write to allow modifications

                        // Iterate through all entities
                        foreach (ObjectId entId in btr)
                        {
                            Entity ent = tr.GetObject(entId, OpenMode.ForWrite) as Entity;

                            if(ent is BlockReference blockref)
                            {
                                ProcessBlockReference(blockref, tr);
                            }
                            else if(ent != null && ent.Color.IsByBlock)
                            {
                                // Change color to ByLayer
                                ent.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, 256);
                            }
                        }
                    }
                }
                tr.Commit();
                ed.WriteMessage("\nAll entities with color 'ByBlock' have been changed to 'ByLayer'.");
            }
        }
        private void ProcessBlockReference(BlockReference blockRef, Transaction tr)
        {
            if (blockRef.BlockTableRecord.IsValid)
            {
                BlockTableRecord blockRecord = tr.GetObject(blockRef.BlockTableRecord, OpenMode.ForRead) as BlockTableRecord;
                if (blockRecord != null)
                {
                    foreach (ObjectId entId in blockRecord)
                    {
                        Entity nestedEnt = tr.GetObject(entId, OpenMode.ForWrite) as Entity;

                        if (nestedEnt is BlockReference nestedBlockRef)
                        {
                            // Recursively process nested block references
                            ProcessBlockReference(nestedBlockRef, tr);
                        }
                        else if (nestedEnt != null && nestedEnt.Color.IsByBlock)
                        {
                            nestedEnt.Color = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByLayer, 256);
                        }
                    }
                }
            }
        }

    }
}