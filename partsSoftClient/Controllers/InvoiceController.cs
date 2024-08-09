using Newtonsoft.Json;
using partsSoftClient.Entity;
using partsSoftClient.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace partsSoftClient.Controllers
{
	public class InvoiceController
	{
		public static List<Invoice> get(string url, string endPoint)
		{
			List<Invoice> invoices;

			using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
			{
				client.BaseAddress = new Uri(url);
				HttpResponseMessage response = client.GetAsync(endPoint).Result;
				response.EnsureSuccessStatusCode();
				string result = response.Content.ReadAsStringAsync().Result;
				invoices = JsonConvert.DeserializeObject<List<Invoice>>(result);
				
			}

			return invoices;
		}

		public static async Task<ResponseModel> Post(string invoiceId, bool download)
		{
			var post = new PostInvoice
			{
				invoiceId = invoiceId,
				download = download
			};

			var json = JsonConvert.SerializeObject(post);
			var data = new StringContent(json, Encoding.UTF8, "application/json");
			var url = "http://127.0.0.1:3000/api/invoices/post/download";

			using (var client = new HttpClient())
			{
				var response = await client.PostAsync(url, data);
				var result = await response.Content.ReadAsStringAsync();

				var responseModel = JsonConvert.DeserializeObject<ResponseModel>(result);

				return responseModel;

				//MessageBox.Show($"Message: {responseModel.message}\nSuccess: {responseModel.isSuccess}", "Response", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		public static void  DownloadFile(string url, string endPoint, string filePath)
		{
			using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
			{
				client.BaseAddress = new Uri(url);
				HttpResponseMessage response = client.GetAsync(endPoint).Result;
				response.EnsureSuccessStatusCode();
				byte[] fileBytes = response.Content.ReadAsByteArrayAsync().Result;
				File.WriteAllBytes(filePath, fileBytes);
			}

		}
	}
}
