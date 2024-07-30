
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace partsSoftClient.Components
{
	public class MessageBoxComponent
	{
		public static void Message(string caption, string message, Boolean isSuccess)
		{
			string icon;
			if (isSuccess)
			{
				icon = @"C:\Users\Muhammed Fatih\Desktop\PartsSoftClient\partsSoftClient\partsSoftClient\Icons\Check Mark.ico";
			}
			else
			{
				icon = @"C:\Users\Muhammed Fatih\Desktop\PartsSoftClient\partsSoftClient\partsSoftClient\Icons\Info_2.ico";
			}
			XtraMessageBoxArgs args = new XtraMessageBoxArgs()
			{
				Caption = caption,
				Text = message,
				Buttons = new DialogResult[] { DialogResult.OK},
				Icon = new Icon(icon),

			};

			XtraMessageBox.Show(args);
		}


	}
}
