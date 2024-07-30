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
	public partial class frmRegister : Form
	{
		public frmRegister()
		{
			InitializeComponent();
		}

		private void panel3_Paint(object sender, PaintEventArgs e)
		{
			
		}

		private void panel4_Paint(object sender, PaintEventArgs e)
		{
			
		}

		private void panel5_Paint(object sender, PaintEventArgs e)
		{
			
		}

		private void textEdit1_EditValueChanged(object sender, EventArgs e)
		{
		
		}

		private void textEdit2_EditValueChanged(object sender, EventArgs e)
		{
			
		}

		private void textEdit3_EditValueChanged(object sender, EventArgs e)
		{
			
		}

		private void textEdit1_Click(object sender, EventArgs e)
		{
			panel3.BackColor = Color.White;
			panel4.BackColor = SystemColors.Control;
			panel5.BackColor = SystemColors.Control;
		}

		private void textEdit2_Click(object sender, EventArgs e)
		{
			panel3.BackColor = SystemColors.Control;
			panel4.BackColor = Color.White;
			panel5.BackColor = SystemColors.Control;
		}

		private void textEdit3_Click(object sender, EventArgs e)
		{
			panel3.BackColor = SystemColors.Control;
			panel4.BackColor = SystemColors.Control;
			panel5.BackColor = Color.White;
		}

		private void hyperlinkLabelControl3_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
