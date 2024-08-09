using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using partsSoftClient.Entity;
using partsSoftClient.Models;
using partsSoftClient.Services;
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
		PrinterService printerService = new PrinterService();

		public List<Printer> Get(string url, string endPoint)
		{
			List<Printer> printer = printerService.Get(url, endPoint);
			return printer;
		}

		public ResponseModel Post(string name, string portNumber, string hostAddress, bool status, string postUrl)
		{
			var printer = new Printer
			{
				Name = name,
				PortNumber = portNumber,
				HostAddress = hostAddress,
				Status = status
			};
			ResponseModel responseModel = printerService.Post(printer, postUrl);
			return responseModel;
		}

		public ResponseModel Update(string name, string hostAddress, bool status, string postUrl)
		{
			var printer = new UpdatePrinter
			{
				Name = name,
				HostAddress =hostAddress,
				Status = status
			};
			ResponseModel responseModel = printerService.Update(printer, postUrl);
			return responseModel;
		}
	}
}
