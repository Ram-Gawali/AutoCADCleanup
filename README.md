# Word-Search

To successfully build the project, you need to have the "AutoCadCleanup.bundle" in the Application Plugin folder on the C drive.

`Steps to copy the bundle:`
Copy the folder "AutoCadCleanup.bundle" from the source folder.
Paste it into the following directory:
C:\Users\<Username>\AppData\Roaming\Autodesk\ApplicationPlugins.
This ensures that the "SimplifyDesign.cuix" file is automatically loaded into AutoCAD upon launch.

The "AutoCadCleanup.bundle" folder contains a "PackageContents.xml" file, which specifies loading the main DLL file from the 2025 folder.

# Functions Achieved:
1. `Create Layers` : Generates layers based on a CSV file, with properties specified in the CSV.
2. `Layer Standardization` : Moves entities from a layer to the corresponding standard layer based on their names and deletes the original layers.
3. `Entity Cleanup` : Deletes all entities and layers based on an unwanted items list.
4. `UI Table` : Creates a table in the UI that displays block snapshots, block names, checkboxes to delete blocks or move them to a standard layer, and a dropdown to select the standard layer. The structure is complete.
5. `Block Snapshots` : Implements functionality to capture snapshots of blocks and save them to a user-specified folder.

# Functions To Be Achieved:
1. `Snapshot Display and Block Management` : Display block snapshots and names in the table.
 - Handle blocks based on checkboxes:
    - If the checkbox is checked, delete the block entities.
    - If unchecked, move the block entities to the specified standard layer from the dropdown.

2. `Unwanted Entities Cleanup` : Write an algorithm to delete unwanted entities from standard layers.
                            - Either ask the user by displaying blocks in a table (as in the previous point) or use an alternative method.

3. `Batch Processing` : Implement batch processing functionality.#   A u t o C A D C l e a n u p  
 