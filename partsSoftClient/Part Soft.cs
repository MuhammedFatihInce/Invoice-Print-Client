using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

		private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			Forms.FrmPdfViewer frm = new Forms.FrmPdfViewer();
			frm.MdiParent = this;
			frm.Show();
		}
	}
}
