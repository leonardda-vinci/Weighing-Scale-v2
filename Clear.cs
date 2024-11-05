using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scale_v3
{
	public partial class Clear : Form
	{
		IniParser iniFile;

		public string CB1 { get; private set; }
		public string CB2 { get; private set; }
		public string CB3 { get; private set; }

		public Clear()
		{
			this.iniFile = new IniParser(@"C:\RCS\Scale\config.ini");
			InitializeComponent();
		}

		private void clearBtn_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("Are you want to proceed?", "Clear", MessageBoxButtons.YesNo);
			if (result == DialogResult.Yes)
			{
				if (checkBox1.Checked)
					CB1 = this.iniFile.GetSetting("Address", "SD1");

				if (checkBox2.Checked)
					CB2 = this.iniFile.GetSetting("Address", "SD2");

				if (checkBox3.Checked)
					CB3 = this.iniFile.GetSetting("Address", "SD3");

				this.DialogResult = DialogResult.Yes;
			}
		}

		private void cancelBtn_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("Are you sure you want to proceed?", "Cancel", MessageBoxButtons.YesNo);

			if (result == DialogResult.Yes)
			{
				this.CB1 = "";
				this.CB2 = "";
				this.CB3 = "";
				this.Close();
			}
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{

		}
	}
}
