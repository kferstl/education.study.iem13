using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DashboardViewer {
	public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm {
		public Form1() {
			InitializeComponent();
		}


		private void LoadFile() {
			var openFile = new OpenFileDialog();
			openFile.Filter = "XML Files | *.xml";
			openFile.Multiselect = false;

			if(openFile.ShowDialog() == DialogResult.OK) {
				try {
					dashboardViewer1.LoadDashboard(openFile.FileName);
				}catch {
					XtraMessageBox.Show("Error loading File!");
				}
			}			
		}

		private void loadFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			LoadFile();
		}

		private void Form1_Load(object sender, EventArgs e) {
			LoadFile();
		}
	}
}
