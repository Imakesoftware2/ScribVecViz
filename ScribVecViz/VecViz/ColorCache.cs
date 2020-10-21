using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecViz
{
    class ColorCache
    {
        public ScribColor Color;
        public Dictionary<IndexGroup, List<ushort>> GroupedIndicies = new Dictionary<IndexGroup, List<ushort>>();
    }
}
