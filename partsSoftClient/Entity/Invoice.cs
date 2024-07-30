using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partsSoftClient.Entity
{
	public class Invoice
	{
		public int invoiceId { get; set; }
		public string UserId { get; set; }
		public string userName { get; set; }
		public string invoicePath { get; set; }
		public bool download { get; set; }
	}
}
