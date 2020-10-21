using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VecViz
{
    public class ScribColor
    {
        Color color;

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                Pen = new Pen(color);
                Brush = new SolidBrush(color);
            }
        }
        public ushort ZIndex { get; set; }
        internal Pen Pen { get; set; }
        internal Brush Brush { get; set; }
    }
}
