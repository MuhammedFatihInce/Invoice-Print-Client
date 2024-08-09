using partsSoftClient.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace partsSoftClient.Services
{
	public class InvoiceService: ApiService<Invoice, UpdateInvoice>
	{

		public void DownloadFile(string url, string endPoint, string filePath)
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
