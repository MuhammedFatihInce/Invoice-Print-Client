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
using partsSoftClient.Services;

namespace partsSoftClient.Forms
{
	public partial class FrmInvoices : Form
	{
		private System.Timers.Timer timer;
		private string url;
		private string endPointWithUserId;

		PrinterController printerController = new PrinterController();
		InvoiceController invoiceController = new InvoiceController();

		List<Invoice> invoices;
		string getUrlPrinter = "http://127.0.0.1:3000";
		string getEndPointPrinter = "/api/printer/get";

		string updateUrlInvoice = "http://127.0.0.1:3000/api/invoices/post/download";

		public FrmInvoices()
		{
			InitializeComponent();
		}
		private void FrmInvoices_Load(object sender, EventArgs e)
		{
			string fileName = ".config";
			var (_url, _endPointWithUserId) = GetConfigValues(fileName);

			url = _url;
			endPointWithUserId = _endPointWithUserId;

			LoadInvoices();

			CreateAndStartTimer(2000, OnTimedEvent);

		}

		private void OnTimedEvent(Object source, ElapsedEventArgs e)
		{
			this.Invoke(new Action(() => LoadInvoices()));
		}

		private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			e.Cancel = true;
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 4)
			{
				string fileName = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
				string filePath = FileHelper.getFolderPath(fileName);
				ProcessStartInfo sInfo = new ProcessStartInfo(filePath);
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
		private void LoadInvoices()
		{
			try
			{

				dataGridView1.Columns.Clear();
				dataGridView1.Rows.Clear();

				// Faturaları indir ve filtrele
				invoices = invoiceController.Get(url, endPointWithUserId);
				List<Invoice> downloadInvoices = invoices.Where(invoice => !invoice.download).ToList();

				// DataGridView'e özelleştirilmiş buton ve sütunları ekle
				DataGridViewComponent dataGridViewComponent = new DataGridViewComponent();
				dataGridViewComponent.CustomInvoiceCollumn(downloadInvoices, dataGridView1, false);

				if (downloadInvoices != null)
				{
					// Aktif yazıcıyı bul
					string printerName = FindActivePrinterName(getUrlPrinter, getEndPointPrinter);

					// Her bir indirilecek fatura için yazdırma işlemini gerçekleştir ve durumu güncelle
					PrintAndUpdateInvoices(downloadInvoices, printerName, updateUrlInvoice);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private string pdf_Dowload(string fileName)
		{
			string url = "http://localhost:3000"; // İndirilecek dosyanın URL'si
			string endPoint = "/uploads" + "/" + fileName; // İndirilecek dosyanın URL'si

			string filePath = FileHelper.getFolderPath(fileName);

			try
			{
				invoiceController.DownloadFile(url, endPoint, filePath);
				//MessageBox.Show("Dosya başarıyla indirildi ve kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Dosya indirilemedi: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return filePath;
		}
		public string FindActivePrinterName(string baseUrl, string endpoint)
		{
			List<Printer> printers = printerController.Get(baseUrl, endpoint);

			string printerName = printers.FirstOrDefault(printer => printer.Status)?.Name;

			if (string.IsNullOrEmpty(printerName))
			{
				MessageBox.Show("Aktif yazıcı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return printerName;
		}
		public void PrintAndUpdateInvoices(List<Invoice> downloadInvoices, string printerName, string updateUrl)
		{
			foreach (var invoice in downloadInvoices)
			{
				string id = invoice.invoiceId.ToString();
				string filePath = pdf_Dowload(invoice.invoicePath); // PDF dosyasını indir
				bool isSuccess = PrinterHelper.PrintInvoice(filePath, printerName); // Yazdırma işlemini gerçekleştir

				if (isSuccess)
				{
					// Yazdırma başarılıysa faturanın durumunu güncelle
					invoiceController.Update(id, updateUrl);
				}
				else
				{
					MessageBox.Show($"Fatura {id} yazdırılamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			//// Tüm faturalar yazdırıldı mesajı
			//MessageBox.Show("Tüm faturalar yazdırıldı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

		}
		public (string url, string endPointWithUserId) GetConfigValues(string fileName)
		{
			var config = FileHelper.ReadConfigFile(fileName);

			string url = config["url"];
			string endPointWithUserId = config["endPoint"] + config["userId"];

			return (url, endPointWithUserId);
		}
		public System.Timers.Timer CreateAndStartTimer(double intervalInMilliseconds, ElapsedEventHandler onElapsedEvent)
		{
			var timer = new System.Timers.Timer(intervalInMilliseconds);
			timer.Elapsed += onElapsedEvent;
			timer.AutoReset = true;
			timer.Enabled = true;
			return timer;
		}
	}
}


