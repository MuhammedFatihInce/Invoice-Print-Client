using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using partsSoftClient.Entity;
using partsSoftClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace partsSoftClient.Controllers
{
	public class PrinterController
	{
		public static async Task<ResponseModel> Post(string name, string portNumber, string hostAddress, bool status)
		{
			var printer = new Printer
			{
				Name = name,
				PortNumber = portNumber,
				HostAddress = hostAddress,
				Status = status
			};
		
			var json = JsonConvert.SerializeObject(printer);
			var data = new StringContent(json, Encoding.UTF8, "application/json");
			var url = "http://127.0.0.1:3000/api/printer/post";

			using (var client = new HttpClient())
			{
				var response = await client.PostAsync(url, data);
				var result = await response.Content.ReadAsStringAsync();

				var responseModel = JsonConvert.DeserializeObject<ResponseModel>(result);

				return responseModel;

				//MessageBox.Show($"Message: {responseModel.message}\nSuccess: {responseModel.isSuccess}", "Response", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		public static List<Printer> get()
		{
			List<Printer> printer;

			using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
			{
				client.BaseAddress = new Uri("http://127.0.0.1:3000");
				HttpResponseMessage response = client.GetAsync("/api/printer/get").Result;
				response.EnsureSuccessStatusCode();
				string result = response.Content.ReadAsStringAsync().Result;
				printer = JsonConvert.DeserializeObject<List<Printer>>(result);

			}

			return printer;
		}

		public static async Task<ResponseModel> Update(string Name, string hostAddress, bool status)
		{
			var post = new PostPrinter
			{
				Name = Name,
				HostAddress = hostAddress,
				Status = status
			};

			var json = JsonConvert.SerializeObject(post);
			var data = new StringContent(json, Encoding.UTF8, "application/json");
			var url = "http://127.0.0.1:3000/api/printer/updateStatus";

			using (var client = new HttpClient())
			{
				var response = await client.PostAsync(url, data);
				var result = await response.Content.ReadAsStringAsync();

				var responseModel = JsonConvert.DeserializeObject<ResponseModel>(result);

				return responseModel;

				//MessageBox.Show($"Message: {responseModel.message}\nSuccess: {responseModel.isSuccess}", "Response", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
	}
}
