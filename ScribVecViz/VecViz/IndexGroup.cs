using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecViz
{
    public class IndexGroup
    {
        public ushort StartIndex { get; set; }
        public ushort EndIndex { get; set; }
        public List<short> OutlineVertexIndexes { get; set; }
        public List<short> OutlineEdges { get; set; }
        public Tuple<float, float, float, float> RelativeBounds { get; set; }

        public IndexGroup()
        {
            OutlineVertexIndexes = new List<short>();
            OutlineEdges = new List<short>();
        }
    }
}
