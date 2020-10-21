using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SvgNet.SvgGdi;

namespace VecViz
{
    public partial class MainForm : Form
    {
        static readonly string titleText = "Scribblenauts Vector Format Visualizer";
        ScribVector vec = new ScribVector();

        public MainForm()
        {
            InitializeComponent();
            //ResizeRedraw = true;
            KeyPreview = true;
            scaleToolStripTextBox.Text = renderer.FigureScale.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] cmdArgs = Environment.GetCommandLineArgs();
            if (cmdArgs.Length > 1)
            {
                loadFile(cmdArgs[1]);
            }
        }

        void loadFile(string path)
        {
            try
            {
                ScribVector vecNew = new ScribVector();
                using (BinaryReader br = new BinaryReader(File.OpenRead(path)))
                {
                    vecNew.Load(br);
                }
                vec = vecNew;
                renderer.Vec = vec;
                Text = titleText + " - " + path;
                renderer.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Failed to open .vec file: " + ex.Message, titleText, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.O)
            {
                openToolStripButton_Click(this, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            base.OnKeyDown(e);
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                loadFile(openFileDialog1.FileName);
            }
        }

        private void centerToolStripButton_Click(object sender, EventArgs e)
        {
            renderer.FigureX = 0;
            renderer.FigureY = 0;
            renderer.FigureScale = 1;
            renderer.Invalidate();
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1 && Path.GetExtension(files[0]).ToLowerInvariant() == ".vec") e.Effect = DragDropEffects.Copy;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            loadFile(files[0]);
        }

        private void renderToolStripButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Choose render output path...";
            saveFileDialog1.Filter = "PNG Image (*.png)|*.png";
            if (saveFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using (Bitmap bmp = new Bitmap(ClientSize.Width, ClientSize.Height - toolStrip1.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                    {
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            IGraphics gg = new GdiGraphics(g);
                            renderer.RenderingParameters.ClearBackground = false;
                            if (renderer.Antialias) g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            vec.Draw(gg, renderer.RenderingParameters, renderer.FigureScale, renderer.FigureX, renderer.FigureY);
                        }
                        bmp.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    MessageBox.Show(this, "Render saved.", titleText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Failed to render to file: " + ex.Message, titleText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    renderer.RenderingParameters.ClearBackground = true;
                }
            }
        }

        private void scaleToolStripTextBox_Leave(object sender, EventArgs e)
        {
            float scale;
            if (float.TryParse(scaleToolStripTextBox.Text, out scale))
            {
                renderer.FigureScale = scale;
            }
        }

        private void exportToolStripButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Choose export path...";
            saveFileDialog1.Filter = "Scalable Vector Graphics (*.svg)|*.svg";
            if (saveFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    SvgGraphics g = new SvgGraphics();
                    vec.Draw(g, new RenderingParameters() { ClearBackground = false, IgnoreEdgeDrawing = true }, 1, 0, 0);
                    File.WriteAllText(saveFileDialog1.FileName, g.WriteSVGString());
                    MessageBox.Show(this, "Exported to SVG.", titleText, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Failed to export to file: " + ex.Message, titleText, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void renderer_ScaleChanged(object sender, ScaleChangedEventArgs e)
        {
            scaleToolStripTextBox.Text = e.Scale.ToString();
        }
    }
}
