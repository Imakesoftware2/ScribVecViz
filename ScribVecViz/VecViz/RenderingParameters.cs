using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecViz
{
    public class RenderingParameters
    {
        public bool WireframeMode { get; set; }
        public bool ClearBackground { get; set; }
        public bool ShowOutline { get; set; }
        public bool IgnoreEdgeDrawing { get; set; }

        public RenderingParameters()
        {
            ClearBackground = true;
        }
    }
}
