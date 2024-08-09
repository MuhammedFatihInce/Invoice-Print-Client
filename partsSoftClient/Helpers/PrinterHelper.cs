using partsSoftClient.Components;
using partsSoftClient.Entity;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace partsSoftClient.Helpers
{
	public class PrinterHelper
	{
		public static List<Printer> Scanner()
		{
			List<Printer> printers = new List<Printer>();

			var scope = new ManagementScope(@"\root\cimv2");
			scope.Connect();

			var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
			var results = searcher.Get();

			foreach (var printer in results)
			{
				var portName = printer.Properties["PortName"].Value;

				var searcher2 = new ManagementObjectSearcher("SELECT * FROM Win32_TCPIPPrinterPort where Name LIKE '" + portName + "'");
				var results2 = searcher2.Get();
				foreach (var printer2 in results2)
				{
					string Name =  printer.Properties["Name"].Value.ToString();
					string PortNumber = printer2.Properties["PortNumber"].Value.ToString();
					string HostAddress = printer2.Properties["HostAddress"].Value.ToString();


					Printer p = new Printer
					{
						Name = Name,
						PortNumber = PortNumber,
						HostAddress = HostAddress,
					};


					printers.Add(p);
				}

			}
			return printers;
		}

		public static void PrintPdf(string filePath)
		{
			if (!File.Exists(filePath))
			{
				MessageBoxComponent.Message("Uyarı", "Fatura bulunamadı.", false);
				return;
			}

			using (var document = PdfDocument.Load(filePath))
			{
				using (var printDocument = document.CreatePrintDocument())
				{
					

					PrintDialog printDialog1 = new PrintDialog();
					printDialog1.Document = printDocument;

					DialogResult result = printDialog1.ShowDialog();

					if (result == DialogResult.OK)
					{
						try
						{
							printDocument.Print();
							MessageBoxComponent.Message("Başarılı", "Yazıcıya gönderildi.", true);
						}
						catch (Exception ex)
						{
							MessageBoxComponent.Message("Uyarı", $"Error printing {filePath}: {ex.Message}", false);
						}
					}
				}
			}
		}

		public static bool PrintInvoice(string filePath, string printerName)
		{
			bool isSuccess= false;
			if (!File.Exists(filePath))
			{
				MessageBox.Show($"File {filePath} does not exist.");
				isSuccess = false;
				return isSuccess;
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
						isSuccess = true;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error printing {filePath}: {ex.Message}");
				isSuccess = false;
			}

			return isSuccess;
		}
	}
}
