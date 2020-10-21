namespace VecViz
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            VecViz.RenderingParameters renderingParameters1 = new VecViz.RenderingParameters();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.scaleToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.centerToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.renderToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.exportToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.renderer = new VecViz.VecRenderer();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripButton,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.scaleToolStripTextBox,
            this.centerToolStripButton,
            this.toolStripSeparator2,
            this.renderToolStripButton,
            this.exportToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(589, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "&Open";
            this.openToolStripButton.Click += new System.EventHandler(this.openToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(34, 22);
            this.toolStripLabel1.Text = "Scale";
            // 
            // scaleToolStripTextBox
            // 
            this.scaleToolStripTextBox.Name = "scaleToolStripTextBox";
            this.scaleToolStripTextBox.Size = new System.Drawing.Size(100, 25);
            this.scaleToolStripTextBox.Leave += new System.EventHandler(this.scaleToolStripTextBox_Leave);
            // 
            // centerToolStripButton
            // 
            this.centerToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.centerToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("centerToolStripButton.Image")));
            this.centerToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.centerToolStripButton.Name = "centerToolStripButton";
            this.centerToolStripButton.Size = new System.Drawing.Size(89, 22);
            this.centerToolStripButton.Text = "Reset Viewport";
            this.centerToolStripButton.Click += new System.EventHandler(this.centerToolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // renderToolStripButton
            // 
            this.renderToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.renderToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("renderToolStripButton.Image")));
            this.renderToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.renderToolStripButton.Name = "renderToolStripButton";
            this.renderToolStripButton.Size = new System.Drawing.Size(48, 22);
            this.renderToolStripButton.Text = "Render";
            this.renderToolStripButton.Click += new System.EventHandler(this.renderToolStripButton_Click);
            // 
            // exportToolStripButton
            // 
            this.exportToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.exportToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("exportToolStripButton.Image")));
            this.exportToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportToolStripButton.Name = "exportToolStripButton";
            this.exportToolStripButton.Size = new System.Drawing.Size(44, 22);
            this.exportToolStripButton.Text = "Export";
            this.exportToolStripButton.Click += new System.EventHandler(this.exportToolStripButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Scribblenauts Vector Data file (*.vec)|*.vec";
            this.openFileDialog1.Title = "Choose .vec file...";
            // 
            // renderer
            // 
            this.renderer.Antialias = true;
            this.renderer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderer.FigureScale = 1F;
            this.renderer.FigureX = 0F;
            this.renderer.FigureY = 0F;
            this.renderer.Location = new System.Drawing.Point(0, 25);
            this.renderer.Name = "renderer";
            renderingParameters1.ClearBackground = true;
            renderingParameters1.IgnoreEdgeDrawing = false;
            renderingParameters1.ShowOutline = false;
            renderingParameters1.WireframeMode = false;
            this.renderer.RenderingParameters = renderingParameters1;
            this.renderer.Size = new System.Drawing.Size(589, 292);
            this.renderer.TabIndex = 1;
            this.renderer.Vec = null;
            this.renderer.ScaleChanged += new System.EventHandler<VecViz.ScaleChangedEventArgs>(this.renderer_ScaleChanged);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 317);
            this.Controls.Add(this.renderer);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Scribblenauts Vector Format Visualizer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripButton centerToolStripButton;
        private System.Windows.Forms.ToolStripButton renderToolStripButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox scaleToolStripTextBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton exportToolStripButton;
        private VecRenderer renderer;

    }
}

