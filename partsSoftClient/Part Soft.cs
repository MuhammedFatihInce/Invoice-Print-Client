using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace partsSoftClient
{
	public partial class Part_Soft : Form
	{
		
		public Part_Soft()
		{
			InitializeComponent();
		}

		private void Part_Soft_Load(object sender, EventArgs e)
		{
			
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.FullName;
			string absoluteFilePath = Path.Combine(projectDirectory, "Icons", "Print.ico");
			notifyIcon1.Icon = new Icon(absoluteFilePath);
		}

		private void BtnInvoiceList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			Forms.FrmInvoices frm = new Forms.FrmInvoices();
			frm.MdiParent = this;
			frm.Show();
		}

		private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			Forms.FrmPrinter frm = new Forms.FrmPrinter();
			frm.MdiParent = this;
			frm.Show();
		}

		private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			Forms.FrmHistory frm = new Forms.FrmHistory();
			frm.MdiParent = this;
			frm.Show();
		}

		private void Part_Soft_Resize(object sender, EventArgs e)
		{
			if (FormWindowState.Minimized == WindowState)
			{
				Hide();
				notifyIcon1.Visible = true;
				notifyIcon1.Text = "NotifyIcon Denemesi";
				notifyIcon1.BalloonTipTitle = "Program Çalışıyor";
				notifyIcon1.BalloonTipText = "Program sağ alt köşede konumlandı.";
				notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
				notifyIcon1.ShowBalloonTip(30000);
			}
		}

		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			Show();
			notifyIcon1.Visible = false;
		}

	}
}
