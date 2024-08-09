using partsSoftClient.Components;
using partsSoftClient.Controllers;
using partsSoftClient.Entity;
using partsSoftClient.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace partsSoftClient.Forms
{
	public partial class FrmHistory : Form
	{
		private string url;
		private string endPointWithUserId;

		List<Invoice> invoices;

		InvoiceController invoiceController = new InvoiceController();

		public FrmHistory()
		{
			InitializeComponent();
		}

		private void FrmHistory_Load(object sender, EventArgs e)
		{
			string fileName = ".config";
			var config = FileHelper.ReadConfigFile(fileName);

			url = config["url"];
			endPointWithUserId = config["endPoint"] + config["userId"];

			DateTime selectedDate = DateTime.Now.Date;
			LoadInvoices(selectedDate);
		}

		private void LoadInvoices(DateTime selectedDate)
		{
			try
			{
				dataGridView1.Columns.Clear();
				dataGridView1.Rows.Clear();

				invoices = invoiceController.Get(url, endPointWithUserId);

				string date = selectedDate.ToString("dd.MM.yyyy");
				
				List<Invoice> downloadInvoices = invoices
					.Where(invoice => invoice.download && invoice.updatedAt.Date.ToString("dd.MM.yyyy") == date)
					.ToList();

				
				DataGridViewComponent dataGridViewComponent = new DataGridViewComponent();
				dataGridViewComponent.CustomInvoiceCollumn(downloadInvoices, dataGridView1, true);

			}
			catch (Exception ex)
			{
				MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{
			DateTime selectedDate = dateTimePicker1.Value.Date;
			LoadInvoices(selectedDate);
		}

		private void simpleButton2_Click(object sender, EventArgs e)
		{
			DateTime selectedDate = DateTime.Now.Date;
			LoadInvoices(selectedDate);
		}

		private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 7)
			{
				string fileName = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
				string filePath = FileHelper.getFolderPath(fileName);

				PrinterHelper.PrintPdf(filePath);
				MessageBox.Show("Yazdırma Başarılı.");

			}
		}

		private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 4)
			{
				string fileName = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
				string filePath = FileHelper.getFolderPath(fileName);

				ProcessStartInfo sInfo = new ProcessStartInfo(filePath);
				Process.Start(sInfo);
			}
		}

		private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			e.Cancel = true;
		}
	}
}
