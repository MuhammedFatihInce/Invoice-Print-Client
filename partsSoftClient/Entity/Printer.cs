using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partsSoftClient.Entity
{
	public class Printer
	{
		public string Name { get; set; }
		public string PortNumber { get; set; }
		public string HostAddress { get; set; }
		public bool Status { get; set; }
	}
}
