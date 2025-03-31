using System.Windows.Forms;
using System.Drawing;
using Autodesk.AutoCAD.Runtime;


[assembly: CommandClass(typeof(AutoCADCleanup.TableLauncher))]
namespace AutoCADCleanup
{
    public class TableLauncher
    {
        [CommandMethod("ShowTable")]
        public void ShowTable(List<string> list)
        {
            // Ensure the UI runs on the main thread
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModelessDialog(new UITable(list));
        }
    }

    public class UITable : Form
    {
        private Button clickMeButton;
        private Label displayLabel;
        private DataGridView dataGridView;

        public UITable(List<string> list)
        {

            try
            {

            this.Text = "AutoCAD Form";
            this.Size = new System.Drawing.Size(400, 300);

            displayLabel = new Label
            {
                Text = "Welcome to AutoCAD WinForms!",
                AutoSize = true,
                Location = new System.Drawing.Point(150, 50)
            };
            this.Controls.Add(displayLabel);

            //--------------------------------------------------------------------------
            this.Text = "Table with Snapshots";
            this.Size = new Size(600, 400);

            // Initialize DataGridView
            dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false, // Disable adding rows manually
                RowTemplate = { Height = 60 } // Adjust row height for images
            };

            // Add columns
            InitializeColumns();

            // Add DataGridView to the form
            this.Controls.Add(dataGridView);

            }
            catch (System.Exception ee)
            {

            }

        }

        private void InitializeColumns()
        {
            // Snapshot Column (Image)
            var imageColumn = new DataGridViewImageColumn
            {
                HeaderText = "Snapshot",
                Name = "SnapshotColumn",
                ImageLayout = DataGridViewImageCellLayout.Zoom // Adjust image size to fit
            };
            dataGridView.Columns.Add(imageColumn);

            // Name Column (String)
            var nameColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Name = "NameColumn"
            };
            dataGridView.Columns.Add(nameColumn);

            // Delete Column (Checkbox)
            var deleteColumn = new DataGridViewCheckBoxColumn
            {
                HeaderText = "Delete",
                Name = "DeleteColumn"
            };
            dataGridView.Columns.Add(deleteColumn);

            // Move to Column (Dropdown)
            var dropdownColumn = new DataGridViewComboBoxColumn
            {
                HeaderText = "Move To",
                Name = "MoveToColumn",
                DataSource = new[] { "Folder A", "Folder B", "Folder C" } // Dropdown options
            };
            dataGridView.Columns.Add(dropdownColumn);

        }
    }
}
