using partsSoftClient.Components;
using partsSoftClient.Controllers;
using partsSoftClient.Entity;
using partsSoftClient.Helpers;
using partsSoftClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace partsSoftClient.Forms
{
	public partial class FrmPrinter : Form
	{
		List<Printer> getPrinters;

		string postUrl = "http://127.0.0.1:3000/api/printer/post";

		string getUrl = "http://127.0.0.1:3000";
		string getEndPoint = "/api/printer/get";

		string updateUrl = "http://127.0.0.1:3000/api/printer/updateStatus";

		PrinterController printerController = new PrinterController();



		public FrmPrinter()
		{
			InitializeComponent();
		}

		private void FrmPrinter_Load(object sender, EventArgs e)
		{
			printerPost();
			printerLoad();
		}

		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{

			if (e.ColumnIndex == 4)
			{
				string Name = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
				string Host = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();

				var activePrinter = getPrinters.FirstOrDefault(p => p.Status);

				if (activePrinter != null)
				{
					printerController.Update(activePrinter.Name, activePrinter.HostAddress, false, updateUrl);
				}

				printerController.Update(Name, Host, true, updateUrl);
				printerLoad();
			}
		}

		private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			e.Cancel = true;
		}

		public void printerPost()
		{
			List<Printer> printers = PrinterHelper.Scanner();
			foreach (var printer in printers)
			{
				ResponseModel response = printerController.Post(printer.Name, printer.PortNumber, printer.HostAddress, false, postUrl);
				//MessageBoxComponent.Message("İnfo", response.message, response.isSuccess);
			}
		}
		public void printerLoad()
		{
			dataGridView1.Columns.Clear();
			dataGridView1.Rows.Clear();

			getPrinters = printerController.Get(getUrl, getEndPoint);

			DataGridViewComponent dataGridViewComponent = new DataGridViewComponent();
			DataGridViewButtonColumn sendButton = dataGridViewComponent.CustomButton("Select", "Seç");
			dataGridViewComponent.CustomPrinterCollumn(getPrinters, sendButton, dataGridView1);
		}
	}

}
	

