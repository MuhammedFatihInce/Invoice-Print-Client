using Newtonsoft.Json;
using partsSoftClient.Entity;
using partsSoftClient.Models;
using partsSoftClient.Services;
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
		InvoiceService invoiceService = new InvoiceService();

		public List<Invoice> Get(string url, string endPoint)
		{
			List<Invoice> invoice = invoiceService.Get(url, endPoint);
			return invoice;
		}

		public ResponseModel Update(string id, string postUrl)
		{
			var update = new UpdateInvoice
			{
				invoiceId = id,
				download = true
			};
			ResponseModel responseModel = invoiceService.Update(update, postUrl);
			return responseModel;
		}

		public void DownloadFile(string url, string endPoint, string filePath)
		{
			invoiceService.DownloadFile(url, endPoint, filePath);
		}


	}
}
