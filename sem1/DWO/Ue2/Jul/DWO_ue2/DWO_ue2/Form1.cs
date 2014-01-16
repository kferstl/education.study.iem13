using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DWO_ue2 {
	public partial class Form1 : DevExpress.XtraEditors.XtraForm {
		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			// TODO: This line of code loads data into the 'fH_DWODataSet.Ue2SalesOrdersByWeek' table. You can move, or remove it, as needed.
			this.ue2SalesOrdersByWeekTableAdapter.Fill(this.fH_DWODataSet.Ue2SalesOrdersByWeek);
			// TODO: This line of code loads data into the 'fH_DWODataSet.Ue2SalesOrdersByMonth' table. You can move, or remove it, as needed.
			this.ue2SalesOrdersByMonthTableAdapter.Fill(this.fH_DWODataSet.Ue2SalesOrdersByMonth);
		}

		private void chartControl2_CustomizeXAxisLabels(object sender, DevExpress.XtraCharts.CustomizeXAxisLabelsEventArgs e) {
		
		}

		private void chartControl2_CustomDrawSeriesPoint(object sender, DevExpress.XtraCharts.CustomDrawSeriesPointEventArgs e) {

		}

		private void chartControl2_CustomDrawAxisLabel(object sender, DevExpress.XtraCharts.CustomDrawAxisLabelEventArgs e) {

			if(e.Item.AxisValue is DateTime) {
				var dateTime = (DateTime)e.Item.AxisValue;
				e.Item.Text = "KW: " + GetWeekNumber(dateTime) + " / " + dateTime.Year.ToString();
			}


		}

		private void chartControl2_CustomDrawCrosshair(object sender, DevExpress.XtraCharts.CustomDrawCrosshairEventArgs e) {
			
		}


		public static int GetWeekNumber(DateTime dtPassed) {
			CultureInfo ciCurr = CultureInfo.CurrentCulture;
			int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
			return weekNum;
		}
	}
}
