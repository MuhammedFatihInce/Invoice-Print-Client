using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace partsSoftClient.Helpers
{
	public class TimeHelper
	{
		public static (string date, string hour) ConvertToTurkeyTime(DateTime dateTime)
		{
			try
			{
				TimeZoneInfo localZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
				DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, localZone);
				string date = localDateTime.ToString("dd/MM/yyyy");
				string hour = localDateTime.ToString("HH:mm:ss");

				return (date, hour);
			}
			catch (TimeZoneNotFoundException)
			{
				Console.WriteLine("Belirtilen saat dilimi bulunamadı.");
				return (string.Empty, string.Empty);
			}
			catch (InvalidTimeZoneException)
			{
				Console.WriteLine("Saat dilimi geçersiz.");
				return (string.Empty, string.Empty);
			}
		}
	}
}
