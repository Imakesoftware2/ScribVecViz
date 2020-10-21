using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using SvgNet.SvgGdi;

namespace VecViz
{
    public class ScribVector
    {
        bool indexCacheReady = false;
        bool pointsCacheReady = false;
        List<ColorCache> indexCache = new List<ColorCache>();
        List<PointF> transformedPoints = new List<PointF>();
        Dictionary<IndexGroup, List<PointF>> transformedOutlinePoints = new Dictionary<IndexGroup, List<PointF>>();

        public ushort XScale { get; set; }
        public ushort YScale { get; set; }
        public List<ScribPoint> Points { get; set; }
        public List<ushort> Indicies { get; set; }
        public List<IndexGroup> MeshGroups { get; set; }
        public List<ScribColor> Colors { get; set; }

        public ScribVector()
        {
            Points = new List<ScribPoint>();
            Indicies = new List<ushort>();
            MeshGroups = new List<IndexGroup>();
            Colors = new List<ScribColor>();
        }

        public void Load(BinaryReader br)
        {
            byte flags = br.ReadByte();
            if ((flags & 0x3f) != 2) throw new NotSupportedException("Unknown vector data version.");

            XScale = (ushort)((br.ReadUInt16() >> 3) << 2);
            YScale = (ushort)((br.ReadUInt16() >> 3) << 2);
            ushort pointGroupsCount = br.ReadUInt16();
            byte layerCount = br.ReadByte();
            ushort pointsCount = br.ReadUInt16();

            if (pointGroupsCount == 0 || pointsCount == 0) return;

            // Flag set means using fixed point format, unset means using float
            if ((flags & 0x40) == 0x40)
            {
                while (pointsCount > 0)
                {
                    ushort groupPointsCount = br.ReadUInt16();
                    ushort colorIndex = br.ReadUInt16();

                    for (int i = 0; i < groupPointsCount; ++i)
                    {
                        Points.Add(new ScribPoint { Point = new PointF(br.ReadInt16() / 8196f, br.ReadInt16() / 8196f), ColorIndex = colorIndex });
                    }

                    pointsCount -= groupPointsCount;
                }
            }
            else
            {
                for (int i = 0; i < pointsCount; ++i)
                {
                    float x = br.ReadSingle();
                    float y = br.ReadSingle();
                    ushort c = br.ReadUInt16();

                    Points.Add(new ScribPoint { Point = new PointF(x, y), ColorIndex = c });
                }
            }

            ushort indCount = br.ReadUInt16();
            for (int i = 0; i < indCount; ++i)
            {
                Indicies.Add(br.ReadUInt16());
            }

            // Don't actually know what this is, haven't seen it accessed within the game
            // Seems like bounds relative to pivot point: x edge left, y edge top, y edge bottom, x edge right
            if ((flags & 0x80) == 0x80)
            {
                ushort count = br.ReadUInt16();
                for (int i = 0; i < count; ++i)
                {
                    ushort index = br.ReadUInt16();
                    addMeshGroupsAsNeeded(index);
                    MeshGroups[index].RelativeBounds = new Tuple<float, float, float, float>(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                }
            }

            ushort meshGroupCount = br.ReadUInt16();
            for (int i = 0; i < meshGroupCount; ++i)
            {
                ushort groupIndex = br.ReadUInt16();
                addMeshGroupsAsNeeded(groupIndex);
                IndexGroup g = MeshGroups[groupIndex];
                g.StartIndex = br.ReadUInt16();
                g.EndIndex = br.ReadUInt16();
            }

            for (int i = 0; i < meshGroupCount; ++i)
            {
                ushort groupIndex = br.ReadUInt16();
                addMeshGroupsAsNeeded(groupIndex);
                IndexGroup g = MeshGroups[groupIndex];
                ushort count = br.ReadUInt16();
                for (int j = 0; j < count; ++j)
                {
                    g.OutlineVertexIndexes.Add(br.ReadInt16());
                }
            }

            for (int i = 0; i < meshGroupCount; ++i)
            {
                ushort groupIndex = br.ReadUInt16();
                addMeshGroupsAsNeeded(groupIndex);
                IndexGroup g = MeshGroups[groupIndex];
                ushort count = br.ReadUInt16();
                for (int j = 0; j < count; ++j)
                {
                    g.OutlineEdges.Add(br.ReadInt16());
                }
            }

            ushort colorsCount = br.ReadUInt16();
            for (int i = 0; i < colorsCount; ++i)
            {
                ScribColor col = new ScribColor();
                byte a = br.ReadByte();
                byte b = br.ReadByte();
                byte c = br.ReadByte();
                byte d = br.ReadByte();
                if (i % 2 == 0)
                {
                    col.Color = Color.FromArgb(d, c, b, a);
                }
                else
                {
                    col.Color = Color.FromArgb(255, c, a, b);
                }
                Colors.Add(col);
            }

            for (int i = 0; i < colorsCount; ++i)
            {
                Colors[i].ZIndex = br.ReadUInt16();
            }

            UpdateIndexCache();
            UpdatePointsCache(false);
        }

        void addMeshGroupsAsNeeded(int i)
        {
            if (i > MeshGroups.Count - 1)
            {
                for (int j = MeshGroups.Count; j <= i; ++j)
                {
                    MeshGroups.Add(new IndexGroup());
                }
            }
        }

        public void UpdateIndexCache()
        {
            indexCache.Clear();
            foreach (ScribColor c in Colors)
            {
                indexCache.Add(new ColorCache { Color = c });
            }

            foreach (IndexGroup g in MeshGroups)
            {
                Dictionary<ushort, List<ushort>> colorDict = new Dictionary<ushort, List<ushort>>();
                for (int i = g.StartIndex; i < g.EndIndex; i += 3)
                {
                    ushort ind1 = Indicies[i];
                    ushort ind2 = Indicies[i + 1];
                    ushort ind3 = Indicies[i + 2];
                    ScribPoint p1 = Points[ind1];
                    ScribPoint p2 = Points[ind2];
                    ScribPoint p3 = Points[ind3];
                    //if (p1.ColorIndex != p2.ColorIndex || p1.ColorIndex != p3.ColorIndex) throw new Exception("Points in triangle do not have the same colors.");
                    List<ushort> indList;
                    if (!colorDict.TryGetValue(p1.ColorIndex, out indList))
                    {
                        indList = new List<ushort>();
                        colorDict.Add(p1.ColorIndex, indList);
                    }
                    indList.Add(ind1);
                    indList.Add(ind2);
                    indList.Add(ind3);
                }

                foreach (var pair in colorDict)
                {
                    indexCache[pair.Key].GroupedIndicies.Add(g, pair.Value);
                }
            }

            indexCache.RemoveAll(x => x.GroupedIndicies.Count == 0);
            indexCache.Sort((x, y) =>
            {
                if (x.Color.ZIndex == y.Color.ZIndex) return Colors.IndexOf(x.Color).CompareTo(Colors.IndexOf(y.Color));
                else return -x.Color.ZIndex.CompareTo(y.Color.ZIndex);
            });
            indexCacheReady = true;
        }

        public void UpdatePointsCache(bool reposition)
        {
            transformedPoints.Clear();
            foreach (ScribPoint sp in Points)
            {
                transformedPoints.Add(new PointF(sp.Point.X * XScale, sp.Point.Y * YScale));
            }

            float minX = 0;
            float minY = 0;
            foreach (PointF p in transformedPoints)
            {
                if (p.X < minX) minX = p.X;
                if (p.Y < minY) minY = p.Y;
            }

            for (int i = 0; i < transformedPoints.Count; ++i)
            {
                PointF p = transformedPoints[i];
                transformedPoints[i] = new PointF(p.X - minX, p.Y - minY);
            }

            transformedOutlinePoints.Clear();
            foreach (IndexGroup grp in MeshGroups)
            {
                List<PointF> olvs;
                if (!transformedOutlinePoints.TryGetValue(grp, out olvs))
                {
                    olvs = new List<PointF>();
                    transformedOutlinePoints.Add(grp, olvs);
                }

                for (int i = 0; i < grp.OutlineVertexIndexes.Count - 1; i += 2)
                {
                    for (int j = grp.OutlineVertexIndexes[i]; j < grp.OutlineVertexIndexes[i + 1]; ++j)
                    {
                        var point1 = transformedPoints[j];
                        olvs.Add(point1);
                    }
                }
            }

            pointsCacheReady = true;
        }

        public void Draw(IGraphics g, RenderingParameters parameters, float scale, float destX, float destY)
        {
            if (parameters.ClearBackground) g.Clear(Color.LavenderBlush);
            if (!indexCacheReady || !pointsCacheReady) return;

            foreach (ColorCache cache in indexCache)
            {
                foreach (var pair in cache.GroupedIndicies)
                {
                    List<ushort> inds = pair.Value;
                    for (int i = 0; i < inds.Count; i += 3)
                    {
                        PointF p1 = transformedPoints[inds[i]];
                        PointF p2 = transformedPoints[inds[i + 1]];
                        PointF p3 = transformedPoints[inds[i + 2]];
                        p1 = new PointF(p1.X * scale + destX, p1.Y * scale + destY);
                        p2 = new PointF(p2.X * scale + destX, p2.Y * scale + destY);
                        p3 = new PointF(p3.X * scale + destX, p3.Y * scale + destY);
                        if (parameters.WireframeMode)
                        {
                            Pen pen = cache.Color.Pen;
                            g.FillRectangle(Brushes.Black, p1.X - 3, p1.Y - 3, 6, 6);
                            g.FillRectangle(Brushes.Black, p2.X - 3, p2.Y - 3, 6, 6);
                            g.FillRectangle(Brushes.Black, p3.X - 3, p3.Y - 3, 6, 6);
                            g.DrawLine(pen, p1, p2);
                            g.DrawLine(pen, p2, p3);
                            g.DrawLine(pen, p3, p1);
                        }
                        else
                        {
                            PointF[] triPoints = new PointF[] { p1, p2, p3 };
                            if (!parameters.IgnoreEdgeDrawing) g.DrawPolygon(cache.Color.Pen, triPoints);
                            g.FillPolygon(cache.Color.Brush, triPoints);
                        }
                    }
                }
            }

            // Non-sorted drawing, don't use
            /*for (int i = 0; i < Indicies.Count; i += 3)
            {
                PointF p1 = transformedPoints[Indicies[i]];
                PointF p2 = transformedPoints[Indicies[i + 1]];
                PointF p3 = transformedPoints[Indicies[i + 2]];
                p1 = new PointF(p1.X * scale + destX, p1.Y * scale + destY);
                p2 = new PointF(p2.X * scale + destX, p2.Y * scale + destY);
                p3 = new PointF(p3.X * scale + destX, p3.Y * scale + destY);
                if (wireframe)
                {
                    Pen pen = Colors[Points[Indicies[i]].ColorIndex].Pen;
                    g.DrawLine(pen, p1, p2);
                    g.DrawLine(pen, p2, p3);
                    g.DrawLine(pen, p3, p1);
                }
                else
                {
                    Brush brush = Colors[Points[Indicies[i]].ColorIndex].Brush;
                    g.FillPolygon(brush, new PointF[] { p1, p2, p3 });
                    if (g.SmoothingMode == System.Drawing.Drawing2D.SmoothingMode.AntiAlias)
                    {
                        // Fill 2 more times for better antialiasing results
                        g.FillPolygon(brush, new PointF[] { p1, p2, p3 });
                        g.FillPolygon(brush, new PointF[] { p1, p2, p3 });
                    }
                }
            }*/

            if (parameters.ShowOutline)
            {
                foreach (IndexGroup grp in MeshGroups)
                {
                    List<PointF> olvs = transformedOutlinePoints[grp];
                    foreach (PointF p in olvs)
                    {
                        g.FillRectangle(Brushes.White, p.X * scale + destX - 3, p.Y * scale + destY - 3, 6, 6);
                    }

                    int start = 0;
                    for (int i = 0; i < grp.OutlineEdges.Count; ++i)
                    {
                        {
                            PointF op1 = olvs[start + grp.OutlineEdges[i] - 1];
                            PointF op2 = olvs[start];
                            op1 = new PointF(op1.X * scale + destX, op1.Y * scale + destY);
                            op2 = new PointF(op2.X * scale + destX, op2.Y * scale + destY);
                            g.DrawLine(Pens.Blue, op1, op2);
                        }
                        for (int j = start; j < start + grp.OutlineEdges[i] - 1; ++j)
                        {
                            PointF p1 = olvs[j];
                            PointF p2 = olvs[j + 1];
                            p1 = new PointF(p1.X * scale + destX, p1.Y * scale + destY);
                            p2 = new PointF(p2.X * scale + destX, p2.Y * scale + destY);
                            g.DrawLine(Pens.Blue, p1, p2);
                        }
                        start += grp.OutlineEdges[i];
                    }
                }
            }

            g.Flush();
        }

        public string GetDebugString()
        {
            return string.Format("Verts: {0}; Polys: {1}; Colors: {2}; Groups: {3}", Points.Count, Indicies.Count / 3, indexCache.Count, MeshGroups.Count);
        }
    }
}
