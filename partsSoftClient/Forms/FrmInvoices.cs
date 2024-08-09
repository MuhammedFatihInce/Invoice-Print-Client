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
			try
			{
				// DataGridView'ı temizle
				dataGridView1.Columns.Clear();
				dataGridView1.Rows.Clear();

				// Faturaları indir ve filtrele
				List<Invoice> invoices = InvoiceController.get(url, endPointWithUserId);
				List<Invoice> downloadInvoices = invoices.Where(invoice => !invoice.download).ToList();

				// DataGridView'e özelleştirilmiş buton ve sütunları ekle
				DataGridViewComponent dataGridViewComponent = new DataGridViewComponent();
				dataGridViewComponent.CustomInvoiceCollumn(downloadInvoices, dataGridView1, false);

				// Aktif yazıcıyı bul
				List<Printer> printers = PrinterController.get();
				string printerName = printers.FirstOrDefault(printer => printer.Status)?.Name;

				if (string.IsNullOrEmpty(printerName))
				{
					MessageBox.Show("Aktif yazıcı bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				// Her bir indirilecek fatura için yazdırma işlemini gerçekleştir ve durumu güncelle
				foreach (var invoice in downloadInvoices)
				{
					string id = invoice.invoiceId.ToString();
					string filePath= pdf_Dowload(invoice.invoicePath);
					bool isSuccess = PrinterHelper.PrintInvoice(filePath, printerName);

					if (isSuccess)
					{
						await InvoiceController.Post(id, true);
					}
					else
					{
						MessageBox.Show($"Fatura {id} yazdırılamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}

				//MessageBox.Show("Tüm faturalar yazdırıldı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
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

		private void FrmInvoices_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (timer != null)
			{
				timer.Stop();
				timer.Dispose();
			}
		}

		private string pdf_Dowload(string fileName)
		{
			string url = "http://localhost:3000"; // İndirilecek dosyanın URL'si
			string endPoint = "/uploads"+"/"+ fileName; // İndirilecek dosyanın URL'si
														
			string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			string uploadPath = Path.Combine(desktopPath, "uploads");
			if (!Directory.Exists(uploadPath))
			{
				Directory.CreateDirectory(uploadPath);
			}
			string filePath = Path.Combine(uploadPath, fileName);

			try
			{
				InvoiceController.DownloadFile(url, endPoint, filePath);
				MessageBox.Show("Dosya başarıyla indirildi ve kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Dosya indirilemedi: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return filePath;
		}

		

	}
}


