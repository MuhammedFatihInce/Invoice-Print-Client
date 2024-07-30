using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using partsSoftClient.Components;
using partsSoftClient.Controllers;
using partsSoftClient.Entity;
using partsSoftClient.Helpers;
using partsSoftClient.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace partsSoftClient.Forms
{
	public partial class FrmPrinter : Form
	{
		public FrmPrinter()
		{
			InitializeComponent();

		}

		private void FrmPrinter_Load(object sender, EventArgs e)
		{
			printerPost();
			printerLoad();
		}

		private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{

			if (e.ColumnIndex == 4)
			{
				string Name = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
				string Host = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();

				List<Printer> printers = PrinterController.get();
				foreach (var printer in printers)
				{
					await PrinterController.Update(printer.Name, printer.HostAddress, false);
				}
				await PrinterController.Update(Name, Host, true);
				printerLoad();
			}
		}

		private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			e.Cancel = true;
		}

		public async void printerPost()
		{
			List<Printer> printers = PrinterHelper.Scanner();
			foreach (var printer in printers)
			{
				ResponseModel response = await PrinterController.Post(printer.Name, printer.PortNumber, printer.PortNumber, false);
				//MessageBoxComponent.Message("İnfo", response.message, response.isSuccess);
			}
		}
		public void printerLoad()
		{
			dataGridView1.Columns.Clear();
			dataGridView1.Rows.Clear();

			List<Printer> printers = PrinterController.get();

			DataGridViewComponent dataGridViewComponent = new DataGridViewComponent();
			DataGridViewButtonColumn sendButton = dataGridViewComponent.CustomButton("Select", "Seç");
			dataGridViewComponent.CustomPrinterCollumn(printers, sendButton, dataGridView1);
		}
	}

}
	

