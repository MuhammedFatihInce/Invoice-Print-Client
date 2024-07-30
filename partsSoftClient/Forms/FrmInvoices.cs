using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
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
using System.Windows.Forms;
using System.Timers;
using System.Drawing.Printing;
using PdfiumViewer;
using partsSoftClient.Models;

namespace partsSoftClient.Forms
{
	public partial class FrmInvoices : Form
	{
		private System.Timers.Timer timer;
		private string url;
		private string endPoint;
		private string endPointWithUserId;

		public FrmInvoices()
		{
			InitializeComponent();
		}

		private void FrmInvoices_Load(object sender, EventArgs e)
		{
			string fileName = ".config";
			var config = ReadFileHelper.ReadConfigFile(fileName);

			url = config["url"];
			endPoint = config["endPoint"];
			endPointWithUserId = config["endPoint"] + config["userId"];


			LoadInvoices();

			timer = new System.Timers.Timer(30000); // 5 dakika (300000 milisaniye)
			timer.Elapsed += OnTimedEvent;
			timer.AutoReset = true;
			timer.Enabled = true;

		}
		private void OnTimedEvent(Object source, ElapsedEventArgs e)
		{
			// Timer arka planda çalıştığı için UI iş parçacığında güncelleme yapmalıyız.
			this.Invoke(new Action(() => LoadInvoices()));
		}

		private async void LoadInvoices()
		{
			dataGridView1.Columns.Clear();
			dataGridView1.Rows.Clear();

			List<Invoice> DownloadInvoices = new List<Invoice>();
			List<Invoice> invoices = InvoiceController.get(url, endPointWithUserId);
			
			foreach (var invoice in invoices)
			{
				if (!invoice.download)
				{
					DownloadInvoices.Add(invoice);
				}
				
			}


			DataGridViewComponent dataGridViewComponent = new DataGridViewComponent();
			DataGridViewButtonColumn sendButton = dataGridViewComponent.CustomButton("Print", "Yazdır");
			dataGridViewComponent.CustomInvoiceCollumn(invoices, sendButton, dataGridView1);
			//MessageBox.Show("İstek Yapıldı");

			foreach (var invoice in DownloadInvoices)
			{
				string id = invoice.invoiceId.ToString();
				PrintInvoice(invoice.invoicePath, "Microsoft Print to PDF");
				await InvoiceController.Post(id, true);

			}
			//MessageBox.Show("Tümü Yazıcıya Gönderildi.");



		}

		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 5)
			{
				string invoicePath = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();

				//PrinterHelper.PrintPdf(invoicePath);
				PrintInvoice(invoicePath, "Microsoft Print to PDF");

			}
		}

		private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			e.Cancel = true;
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 3)
			{
				string sUrl = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
				ProcessStartInfo sInfo = new ProcessStartInfo(sUrl);
				Process.Start(sInfo);
			}
		}

		private void FrmInvoices_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (timer != null)
			{
				timer.Stop();
				timer.Dispose();
			}
		}

		private void PrintInvoice(string filePath, string printerName)
		{
			if (!File.Exists(filePath))
			{
				MessageBox.Show($"File {filePath} does not exist.");
				return;
			}

			try
			{
				using (var document = PdfDocument.Load(filePath))
				{
					using (var printDocument = document.CreatePrintDocument())
					{
						printDocument.PrinterSettings.PrinterName = printerName;
						printDocument.PrintController = new StandardPrintController(); // Yazdırma işlemi sırasında gösterilen dialogları devre dışı bırakır
						printDocument.Print();
						//MessageBox.Show($"Printing {filePath} complete.");
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error printing {filePath}: {ex.Message}");
			}
		}
		
	}
}


