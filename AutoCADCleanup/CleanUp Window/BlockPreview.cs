using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp_Window
{
    public class BlockPreview
    {
        public string Name { get; set; }
        public Bitmap Image { get; set; }

        public string BlockLayer { get; set; }
        public BlockPreview(string name, Bitmap image, string blockLayer)
        {
            Name = name;
            Image = image;
            BlockLayer = blockLayer;
        }
    }
}
