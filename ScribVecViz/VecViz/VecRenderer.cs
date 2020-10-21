using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SvgNet.SvgGdi;

namespace VecViz
{
    public partial class VecRenderer : UserControl
    {
        ScribVector vec;
        float scale;
        float figureX;
        float figureY;
        bool antialias;

        Point prevMouseLocation;
        Font debugFont = new Font("Arial", 8);
        StringFormat debugFormat = new StringFormat() { Alignment = StringAlignment.Far };

        public VecRenderer()
        {
            InitializeComponent();
            FigureScale = 1;
            ResizeRedraw = true;
            RenderingParameters = new RenderingParameters();
            Antialias = true;
        }

        public ScribVector Vec
        {
            get
            {
                return vec;
            }
            set
            {
                vec = value;
                Invalidate();
            }
        }
        public float FigureX
        {
            get
            {
                return figureX;
            }
            set
            {
                figureX = value;
                Invalidate();
            }
        }

        public float FigureY
        {
            get
            {
                return figureY;
            }
            set
            {
                figureY = value;
                Invalidate();
            }
        }

        public float FigureScale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                ScaleChanged(this, new ScaleChangedEventArgs(scale));
                Invalidate();
            }
        }
        public RenderingParameters RenderingParameters { get; set; }
        public bool Antialias
        {
            get
            {
                return antialias;
            }
            set
            {
                antialias = value;
                Invalidate();
            }
        }

        public event EventHandler<ScaleChangedEventArgs> ScaleChanged = delegate { };

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Vec != null)
            {
                System.Drawing.Drawing2D.SmoothingMode origSmoothing = e.Graphics.SmoothingMode;
                if (Antialias) e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                IGraphics gg = new GdiGraphics(e.Graphics);
                Vec.Draw(gg, RenderingParameters, FigureScale, FigureX, FigureY);

                string debugString = Vec.GetDebugString();
                SizeF debugStringSize = e.Graphics.MeasureString(debugString, debugFont);
                e.Graphics.DrawString(debugString, debugFont, Brushes.Black, ClientSize.Width, ClientSize.Height - debugStringSize.Height, debugFormat);

                e.Graphics.SmoothingMode = origSmoothing;
            }
            else
            {
                e.Graphics.Clear(Color.LavenderBlush);
            }
            base.OnPaint(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!e.Handled)
            {
                switch (e.KeyCode)
                {
                    case Keys.W:
                        RenderingParameters.WireframeMode = !RenderingParameters.WireframeMode;
                        Invalidate();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.A:
                        Antialias = !Antialias;
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.O:
                        RenderingParameters.ShowOutline = !RenderingParameters.ShowOutline;
                        Invalidate();
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                        break;
                }
            }

            base.OnKeyDown(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            // Break vector from mouse to origin into components, and change to unit scale
            float mouseOriginVectorX = (FigureX - e.Location.X) / FigureScale;
            float mouseOriginVectorY = (FigureY - e.Location.Y) / FigureScale;
            // Change the scale
            FigureScale += (e.Delta / 120f) * FigureScale / 5f;
            if (FigureScale < 0.01f) FigureScale = 0.01f;
            // Multiply components by new scale, and do vector addition by components to mouse location
            FigureX = e.Location.X + mouseOriginVectorX * FigureScale;
            FigureY = e.Location.Y + mouseOriginVectorY * FigureScale;
            base.OnMouseWheel(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                prevMouseLocation = e.Location;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                FigureX += e.Location.X - prevMouseLocation.X;
                FigureY += e.Location.Y - prevMouseLocation.Y;
                prevMouseLocation = e.Location;
            }
            base.OnMouseMove(e);
        }
    }

    public class ScaleChangedEventArgs : EventArgs
    {
        public float Scale { get; private set; }

        public ScaleChangedEventArgs(float scale)
        {
            Scale = scale;
        }
    }
}
