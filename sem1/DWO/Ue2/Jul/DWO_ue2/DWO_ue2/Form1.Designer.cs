namespace DWO_ue2 {
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
			DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
			DevExpress.XtraCharts.SecondaryAxisY secondaryAxisY1 = new DevExpress.XtraCharts.SecondaryAxisY();
			DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
			DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel1 = new DevExpress.XtraCharts.PointSeriesLabel();
			DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
			DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
			DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel2 = new DevExpress.XtraCharts.PointSeriesLabel();
			DevExpress.XtraCharts.LineSeriesView lineSeriesView2 = new DevExpress.XtraCharts.LineSeriesView();
			DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel3 = new DevExpress.XtraCharts.PointSeriesLabel();
			DevExpress.XtraCharts.LineSeriesView lineSeriesView3 = new DevExpress.XtraCharts.LineSeriesView();
			DevExpress.XtraCharts.XYDiagram xyDiagram2 = new DevExpress.XtraCharts.XYDiagram();
			DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
			DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel4 = new DevExpress.XtraCharts.PointSeriesLabel();
			DevExpress.XtraCharts.LineSeriesView lineSeriesView4 = new DevExpress.XtraCharts.LineSeriesView();
			DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel5 = new DevExpress.XtraCharts.PointSeriesLabel();
			DevExpress.XtraCharts.LineSeriesView lineSeriesView5 = new DevExpress.XtraCharts.LineSeriesView();
			this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
			this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
			this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
			this.ue2SalesOrdersByMonthTableAdapter = new DWO_ue2.FH_DWODataSetTableAdapters.Ue2SalesOrdersByMonthTableAdapter();
			this.ue2SalesOrdersByMonthBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.fH_DWODataSet = new DWO_ue2.FH_DWODataSet();
			this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
			this.chartControl2 = new DevExpress.XtraCharts.ChartControl();
			this.ue2SalesOrdersByWeekBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.ue2SalesOrdersByWeekTableAdapter = new DWO_ue2.FH_DWODataSetTableAdapters.Ue2SalesOrdersByWeekTableAdapter();
			((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
			this.xtraTabControl1.SuspendLayout();
			this.xtraTabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ue2SalesOrdersByMonthBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fH_DWODataSet)).BeginInit();
			this.xtraTabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chartControl2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(xyDiagram2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ue2SalesOrdersByWeekBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// xtraTabControl1
			// 
			this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
			this.xtraTabControl1.Name = "xtraTabControl1";
			this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
			this.xtraTabControl1.Size = new System.Drawing.Size(913, 691);
			this.xtraTabControl1.TabIndex = 0;
			this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
			// 
			// xtraTabPage1
			// 
			this.xtraTabPage1.Controls.Add(this.chartControl1);
			this.xtraTabPage1.Name = "xtraTabPage1";
			this.xtraTabPage1.Size = new System.Drawing.Size(907, 663);
			this.xtraTabPage1.Text = "Sales By Month";
			// 
			// chartControl1
			// 
			this.chartControl1.DataAdapter = this.ue2SalesOrdersByMonthTableAdapter;
			this.chartControl1.DataSource = this.ue2SalesOrdersByMonthBindingSource;
			xyDiagram1.AxisX.DateTimeScaleOptions.GridAlignment = DevExpress.XtraCharts.DateTimeGridAlignment.Month;
			xyDiagram1.AxisX.DateTimeScaleOptions.MeasureUnit = DevExpress.XtraCharts.DateTimeMeasureUnit.Month;
			xyDiagram1.AxisX.GridLines.Visible = true;
			xyDiagram1.AxisX.Label.Angle = 45;
			xyDiagram1.AxisX.Label.DateTimeOptions.Format = DevExpress.XtraCharts.DateTimeFormat.MonthAndYear;
			xyDiagram1.AxisX.Range.AlwaysShowZeroLevel = true;
			xyDiagram1.AxisX.Range.ScrollingRange.SideMarginsEnabled = true;
			xyDiagram1.AxisX.Range.SideMarginsEnabled = true;
			xyDiagram1.AxisX.Title.Text = "Month";
			xyDiagram1.AxisX.Title.Visible = true;
			xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
			xyDiagram1.AxisY.Label.EndText = " €";
			xyDiagram1.AxisY.Range.AlwaysShowZeroLevel = true;
			xyDiagram1.AxisY.Range.ScrollingRange.SideMarginsEnabled = true;
			xyDiagram1.AxisY.Range.SideMarginsEnabled = true;
			xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
			secondaryAxisY1.AxisID = 0;
			secondaryAxisY1.Name = "Secondary AxisY 1";
			secondaryAxisY1.Range.AlwaysShowZeroLevel = true;
			secondaryAxisY1.Range.ScrollingRange.SideMarginsEnabled = true;
			secondaryAxisY1.Range.SideMarginsEnabled = true;
			secondaryAxisY1.VisibleInPanesSerializable = "-1";
			xyDiagram1.SecondaryAxesY.AddRange(new DevExpress.XtraCharts.SecondaryAxisY[] {
            secondaryAxisY1});
			this.chartControl1.Diagram = xyDiagram1;
			this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chartControl1.Location = new System.Drawing.Point(0, 0);
			this.chartControl1.Name = "chartControl1";
			series1.ArgumentDataMember = "date";
			series1.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.DateTime;
			pointSeriesLabel1.LineVisible = true;
			series1.Label = pointSeriesLabel1;
			series1.Name = "Count";
			series1.ValueDataMembersSerializable = "Count";
			lineSeriesView1.AxisYName = "Secondary AxisY 1";
			series1.View = lineSeriesView1;
			series2.ArgumentDataMember = "date";
			series2.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.DateTime;
			pointSeriesLabel2.LineVisible = true;
			series2.Label = pointSeriesLabel2;
			series2.Name = "Sum";
			series2.ValueDataMembersSerializable = "Sum";
			series2.View = lineSeriesView2;
			this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2};
			pointSeriesLabel3.LineVisible = true;
			this.chartControl1.SeriesTemplate.Label = pointSeriesLabel3;
			this.chartControl1.SeriesTemplate.View = lineSeriesView3;
			this.chartControl1.Size = new System.Drawing.Size(907, 663);
			this.chartControl1.TabIndex = 0;
			// 
			// ue2SalesOrdersByMonthTableAdapter
			// 
			this.ue2SalesOrdersByMonthTableAdapter.ClearBeforeFill = true;
			// 
			// ue2SalesOrdersByMonthBindingSource
			// 
			this.ue2SalesOrdersByMonthBindingSource.DataMember = "Ue2SalesOrdersByMonth";
			this.ue2SalesOrdersByMonthBindingSource.DataSource = this.fH_DWODataSet;
			this.ue2SalesOrdersByMonthBindingSource.Position = 0;
			// 
			// fH_DWODataSet
			// 
			this.fH_DWODataSet.DataSetName = "FH_DWODataSet";
			this.fH_DWODataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// xtraTabPage2
			// 
			this.xtraTabPage2.Controls.Add(this.chartControl2);
			this.xtraTabPage2.Name = "xtraTabPage2";
			this.xtraTabPage2.Size = new System.Drawing.Size(907, 663);
			this.xtraTabPage2.Text = "xtraTabPage2";
			// 
			// chartControl2
			// 
			this.chartControl2.CrosshairOptions.ShowCrosshairLabels = false;
			this.chartControl2.CrosshairOptions.ShowValueLabels = true;
			this.chartControl2.CrosshairOptions.ShowValueLine = true;
			this.chartControl2.DataAdapter = this.ue2SalesOrdersByMonthTableAdapter;
			this.chartControl2.DataSource = this.ue2SalesOrdersByWeekBindingSource;
			xyDiagram2.AxisX.AutoScaleBreaks.MaxCount = 7;
			xyDiagram2.AxisX.DateTimeScaleOptions.GridAlignment = DevExpress.XtraCharts.DateTimeGridAlignment.Week;
			xyDiagram2.AxisX.DateTimeScaleOptions.MeasureUnit = DevExpress.XtraCharts.DateTimeMeasureUnit.Week;
			xyDiagram2.AxisX.GridLines.Visible = true;
			xyDiagram2.AxisX.GridSpacing = 0.1D;
			xyDiagram2.AxisX.GridSpacingAuto = false;
			xyDiagram2.AxisX.Label.Angle = 90;
			xyDiagram2.AxisX.Label.DateTimeOptions.Format = DevExpress.XtraCharts.DateTimeFormat.MonthAndDay;
			xyDiagram2.AxisX.Range.AlwaysShowZeroLevel = true;
			xyDiagram2.AxisX.Range.ScrollingRange.SideMarginsEnabled = true;
			xyDiagram2.AxisX.Range.SideMarginsEnabled = true;
			xyDiagram2.AxisX.Title.Text = "Week";
			xyDiagram2.AxisX.Title.Visible = true;
			xyDiagram2.AxisX.VisibleInPanesSerializable = "-1";
			xyDiagram2.AxisY.Range.AlwaysShowZeroLevel = true;
			xyDiagram2.AxisY.Range.ScrollingRange.SideMarginsEnabled = true;
			xyDiagram2.AxisY.Range.SideMarginsEnabled = true;
			xyDiagram2.AxisY.VisibleInPanesSerializable = "-1";
			this.chartControl2.Diagram = xyDiagram2;
			this.chartControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chartControl2.Location = new System.Drawing.Point(0, 0);
			this.chartControl2.Name = "chartControl2";
			series3.ArgumentDataMember = "OrderFirstDayOfWeek";
			series3.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.DateTime;
			pointSeriesLabel4.LineVisible = true;
			series3.Label = pointSeriesLabel4;
			series3.Name = "Count";
			series3.ValueDataMembersSerializable = "Count";
			series3.View = lineSeriesView4;
			this.chartControl2.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series3};
			pointSeriesLabel5.LineVisible = true;
			this.chartControl2.SeriesTemplate.Label = pointSeriesLabel5;
			this.chartControl2.SeriesTemplate.View = lineSeriesView5;
			this.chartControl2.Size = new System.Drawing.Size(907, 663);
			this.chartControl2.TabIndex = 1;
			this.chartControl2.CustomDrawCrosshair += new DevExpress.XtraCharts.CustomDrawCrosshairEventHandler(this.chartControl2_CustomDrawCrosshair);
			this.chartControl2.CustomDrawSeriesPoint += new DevExpress.XtraCharts.CustomDrawSeriesPointEventHandler(this.chartControl2_CustomDrawSeriesPoint);
			this.chartControl2.CustomDrawAxisLabel += new DevExpress.XtraCharts.CustomDrawAxisLabelEventHandler(this.chartControl2_CustomDrawAxisLabel);
			this.chartControl2.CustomizeXAxisLabels += new DevExpress.XtraCharts.CustomizeXAxisLabelsEventHandler(this.chartControl2_CustomizeXAxisLabels);
			// 
			// ue2SalesOrdersByWeekBindingSource
			// 
			this.ue2SalesOrdersByWeekBindingSource.DataMember = "Ue2SalesOrdersByWeek";
			this.ue2SalesOrdersByWeekBindingSource.DataSource = this.fH_DWODataSet;
			// 
			// ue2SalesOrdersByWeekTableAdapter
			// 
			this.ue2SalesOrdersByWeekTableAdapter.ClearBeforeFill = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(913, 691);
			this.Controls.Add(this.xtraTabControl1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
			this.xtraTabControl1.ResumeLayout(false);
			this.xtraTabPage1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(secondaryAxisY1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ue2SalesOrdersByMonthBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fH_DWODataSet)).EndInit();
			this.xtraTabPage2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(xyDiagram2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(pointSeriesLabel5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(lineSeriesView5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartControl2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ue2SalesOrdersByWeekBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
		private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
		private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
		private DevExpress.XtraCharts.ChartControl chartControl1;
		private FH_DWODataSetTableAdapters.Ue2SalesOrdersByMonthTableAdapter ue2SalesOrdersByMonthTableAdapter;
		private FH_DWODataSet fH_DWODataSet;
		private System.Windows.Forms.BindingSource ue2SalesOrdersByMonthBindingSource;
		private DevExpress.XtraCharts.ChartControl chartControl2;
		private System.Windows.Forms.BindingSource ue2SalesOrdersByWeekBindingSource;
		private FH_DWODataSetTableAdapters.Ue2SalesOrdersByWeekTableAdapter ue2SalesOrdersByWeekTableAdapter;

	}
}

