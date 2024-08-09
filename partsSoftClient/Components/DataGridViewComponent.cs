﻿using partsSoftClient.Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace partsSoftClient.Components
{
	public class DataGridViewComponent
	{
		public DataGridViewButtonColumn CustomButton(string headerText, string name)
		{
			DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn
			{
				FlatStyle = FlatStyle.Flat,
				HeaderText = headerText,
				Text = name,
				Name = name,
				UseColumnTextForButtonValue = true,
				Width = 50
			};

			// Butonun varsayılan stilini özelleştirme
			btn1.DefaultCellStyle.BackColor = SystemColors.Highlight;
			btn1.DefaultCellStyle.SelectionBackColor = Color.Aqua;
			btn1.DefaultCellStyle.ForeColor = Color.White;
			btn1.DefaultCellStyle.SelectionForeColor = Color.Black;

			// Buton hücresinin düz kenarları ve renk geçişi gibi stil ayarları
			btn1.CellTemplate.Style.BackColor = SystemColors.Highlight;
			btn1.CellTemplate.Style.ForeColor = Color.White;
			btn1.CellTemplate.Style.SelectionBackColor = Color.Aqua;
			btn1.CellTemplate.Style.SelectionForeColor = Color.Black;

			return btn1;

		}

		public void CustomPrinterCollumn(List<Printer> printers, DataGridViewButtonColumn sendButton, DataGridView dataGridView1)
		{

			dataGridView1.Dock = DockStyle.Fill;

			DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
			dataGridView1.Columns.Add(chk);
			chk.DataPropertyName = "Status";
			chk.Name = "Seçili Yazıcı";
			// Sütunları tanımlayın
			dataGridView1.Columns.Add("Name", "Name");
			dataGridView1.Columns.Add("PortNumber", "Port Number");
			dataGridView1.Columns.Add("HostAddress", "Host Address");


			dataGridView1.Columns[0].Width = 100;
			dataGridView1.Columns[1].Width = 255;
			dataGridView1.Columns[2].Width = 255;
			dataGridView1.Columns[3].Width = 255;


			dataGridView1.AllowUserToAddRows = false;

			foreach (var printer in printers)
			{
				dataGridView1.Rows.Add(printer.Status, printer.Name, printer.PortNumber, printer.HostAddress);
			}

			//Butonu kolon olarak ekliyoruz
			dataGridView1.Columns.Add(sendButton);
		}

		public void CustomInvoiceCollumn(List<Invoice> invoices, DataGridView dataGridView1, bool button)
		{

			dataGridView1.Dock = DockStyle.Fill;

			DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
			dataGridView1.Columns.Add(chk);
			chk.DataPropertyName = "download";
			chk.Name = "Download";

			// Sütunları tanımlayın
			dataGridView1.Columns.Add("invoiceId", "Invoice Id");
			dataGridView1.Columns.Add("UserId", "User Id");
			dataGridView1.Columns.Add("userName", "User Name");
		
			
			//dataGridView1.Columns.Add("invoicePath", "Invoice Path");

			DataGridViewLinkColumn col = new DataGridViewLinkColumn();
			col.DataPropertyName = "invoicePath";
			col.Name = "Invoice Path";
			dataGridView1.Columns.Add(col);

			dataGridView1.Columns.Add("updatedAt", "Date");
			dataGridView1.Columns.Add("updatedAt", "Hour");


			//dataGridView1.Columns.Add("download", "Download");


			dataGridView1.Columns[0].Width = 50;
			//dataGridView1.Columns[1].Width = 314;
			dataGridView1.Columns[3].Width = 200;
			dataGridView1.Columns[4].Width = 220;

			dataGridView1.AllowUserToAddRows = false;

			

			foreach (var invoice in invoices)
			{
				
				TimeZoneInfo localZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"); // Türkiye saat dilimi
				DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(invoice.updatedAt, localZone);
				string date = localDateTime.ToString("dd/MM/yyyy");
				string hour = localDateTime.ToString("HH:mm:ss");

				dataGridView1.Rows.Add(invoice.download, invoice.invoiceId, invoice.UserId, invoice.userName, invoice.invoicePath, date, hour);
			}

			//Butonu kolon olarak ekliyoruz
			if (button)
			{
				DataGridViewButtonColumn sendButton = CustomButton("Print", "Yazdır");
				dataGridView1.Columns.Add(sendButton);
			}
			
		}
	}
}
