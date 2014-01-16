namespace DashboardViewer {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.formAssistant1 = new DevExpress.XtraBars.FormAssistant();
			this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
			this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
			this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
			this.loadFile = new DevExpress.XtraBars.BarButtonItem();
			this.dashboardViewer1 = new DevExpress.DashboardWin.DashboardViewer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dashboardViewer1)).BeginInit();
			this.SuspendLayout();
			// 
			// defaultLookAndFeel1
			// 
			this.defaultLookAndFeel1.LookAndFeel.SkinName = "VS2010";
			// 
			// ribbonControl1
			// 
			this.ribbonControl1.ExpandCollapseItem.Id = 0;
			this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.loadFile});
			this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
			this.ribbonControl1.MaxItemId = 4;
			this.ribbonControl1.Name = "ribbonControl1";
			this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
			this.ribbonControl1.ShowFullScreenButton = DevExpress.Utils.DefaultBoolean.True;
			this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
			this.ribbonControl1.ShowToolbarCustomizeItem = false;
			this.ribbonControl1.Size = new System.Drawing.Size(1226, 53);
			this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
			this.ribbonControl1.Toolbar.ItemLinks.Add(this.loadFile);
			this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
			// 
			// ribbonStatusBar1
			// 
			this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 620);
			this.ribbonStatusBar1.Name = "ribbonStatusBar1";
			this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
			this.ribbonStatusBar1.Size = new System.Drawing.Size(1226, 33);
			// 
			// loadFile
			// 
			this.loadFile.Caption = "barButtonItem1";
			this.loadFile.CategoryGuid = new System.Guid("6ffddb2b-9015-4d97-a4c1-91613e0ef537");
			this.loadFile.Glyph = ((System.Drawing.Image)(resources.GetObject("loadFile.Glyph")));
			this.loadFile.Id = 1;
			this.loadFile.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("loadFile.LargeGlyph")));
			this.loadFile.Name = "loadFile";
			this.loadFile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.loadFile_ItemClick);
			// 
			// dashboardViewer1
			// 
			this.dashboardViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dashboardViewer1.Location = new System.Drawing.Point(0, 53);
			this.dashboardViewer1.Name = "dashboardViewer1";
			this.dashboardViewer1.Size = new System.Drawing.Size(1226, 567);
			this.dashboardViewer1.TabIndex = 2;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1226, 653);
			this.Controls.Add(this.dashboardViewer1);
			this.Controls.Add(this.ribbonStatusBar1);
			this.Controls.Add(this.ribbonControl1);
			this.Name = "Form1";
			this.Ribbon = this.ribbonControl1;
			this.StatusBar = this.ribbonStatusBar1;
			this.Text = "DWO Dashboard Viewer";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dashboardViewer1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraBars.FormAssistant formAssistant1;
		private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
		private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
		private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
		private DevExpress.XtraBars.BarButtonItem loadFile;
		private DevExpress.DashboardWin.DashboardViewer dashboardViewer1;

	}
}

