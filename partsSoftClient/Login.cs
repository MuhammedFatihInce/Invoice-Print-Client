using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace partsSoftClient
{
	public partial class Login : Form
	{
		public Login()
		{
			InitializeComponent();
		}

		private void textEdit1_Click(object sender, EventArgs e)
		{
			panel3.BackColor = Color.White;
			panel4.BackColor = SystemColors.Control;
		}

		private void textEdit2_Click(object sender, EventArgs e)
		{
			panel4.BackColor = Color.White;
			panel3.BackColor = SystemColors.Control;
		}

		private void hyperlinkLabelControl2_Click(object sender, EventArgs e)
		{
			frmRegister frmRegister = new frmRegister();
			frmRegister.Show();
			this.Hide();
		}
	}
}
