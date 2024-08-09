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
		private string endPoint;
		private string endPointWithUserId;

		public FrmHistory()
		{
			InitializeComponent();
		}

		private void FrmHistory_Load(object sender, EventArgs e)
		{
			string fileName = ".config";
			var config = ReadFileHelper.ReadConfigFile(fileName);

			url = config["url"];
			endPoint = config["endPoint"];
			endPointWithUserId = config["endPoint"] + config["userId"];


			LoadInvoices();
		}

		private void LoadInvoices()
		{
			try
			{
				// DataGridView'ı temizle
				dataGridView1.Columns.Clear();
				dataGridView1.Rows.Clear();

				// 1. Fatura listesini çekin
				List<Invoice> invoices = InvoiceController.get(url, endPointWithUserId);

				// 2. DateTimePicker'dan seçilen tarihi alın
				//DateTime selectedDate = dateTimePicker1.Value.Date;
				DateTime today = DateTime.Now.Date;
				string date = today.ToString("dd.MM.yyyy");
				

				// 3. Seçilen tarihle eşleşen ve indirilebilir olan faturaları filtreleyin
				List<Invoice> downloadInvoices = invoices
					.Where(invoice => invoice.download && invoice.updatedAt.Date.ToString("dd.MM.yyyy") == date)
					.ToList();

				// DataGridView'e özelleştirilmiş buton ve sütunları ekle
				DataGridViewComponent dataGridViewComponent = new DataGridViewComponent();
				dataGridViewComponent.CustomInvoiceCollumn(downloadInvoices, dataGridView1, true);



				//// DataGridView'ı temizle
				//dataGridView1.Columns.Clear();
				//dataGridView1.Rows.Clear();

				//// Faturaları indir ve filtrele
				//List<Invoice> invoices = InvoiceController.get(url, endPointWithUserId);
				//List<Invoice> downloadInvoices = invoices.Where(invoice => invoice.download).ToList();

				//// DataGridView'e özelleştirilmiş buton ve sütunları ekle
				//DataGridViewComponent dataGridViewComponent = new DataGridViewComponent();
				//dataGridViewComponent.CustomInvoiceCollumn(downloadInvoices, dataGridView1, true);

			}
			catch (Exception ex)
			{
				MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			e.Cancel = true;
		}


		private void simpleButton1_Click(object sender, EventArgs e)
		{
			// DataGridView'ı temizle
			dataGridView1.Columns.Clear();
			dataGridView1.Rows.Clear();

			// 1. Fatura listesini çekin
			List<Invoice> invoices = InvoiceController.get(url, endPointWithUserId);

			// 2. DateTimePicker'dan seçilen tarihi alın
			DateTime selectedDate = dateTimePicker1.Value.Date;

			// 3. Seçilen tarihle eşleşen ve indirilebilir olan faturaları filtreleyin
			List<Invoice> downloadInvoices = invoices
				.Where(invoice => invoice.download && invoice.updatedAt.Date == selectedDate)
				.ToList();

			// DataGridView'e özelleştirilmiş buton ve sütunları ekle
			DataGridViewComponent dataGridViewComponent = new DataGridViewComponent();
			dataGridViewComponent.CustomInvoiceCollumn(downloadInvoices, dataGridView1, true);


		}

		private void simpleButton2_Click(object sender, EventArgs e)
		{
			LoadInvoices();
		}

		private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 7)
			{
				string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				string uploadPath = Path.Combine(desktopPath, "uploads");
				string fileName = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
				string filePath = Path.Combine(uploadPath, fileName);
				PrinterHelper.PrintPdf(filePath);
				MessageBox.Show("Yazdırma Başarılı.");

			}
		}

		private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 4)
			{
				string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				string uploadPath = Path.Combine(desktopPath, "uploads");
				string fileName = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
				string filePath = Path.Combine(uploadPath, fileName);
				ProcessStartInfo sInfo = new ProcessStartInfo(filePath);
				Process.Start(sInfo);
			}
		}
	}
}
