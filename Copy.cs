using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Scale_v3
{
	public partial class Copy : Form
	{
		IniParser iniFile;

		public string CB1 { get; private set; }
		public string CB2 { get; private set; }
		public string CB3 { get; private set; }
		public string RadioBtn { get; private set; }

		public Copy()
		{
			this.iniFile = new IniParser(@"C:\RCS\Scale\config.ini");
			InitializeComponent();
		}

		private void copyBtn_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("Are you sure you want to proceed?", "Copy", MessageBoxButtons.YesNo);
			if (result == DialogResult.Yes)
			{
				if (radioButton1.Checked)
					RadioBtn = this.iniFile.GetSetting("Address", "SD1");
				else if (radioButton2.Checked)
					RadioBtn = this.iniFile.GetSetting("Address", "SD2");
				else if (radioButton3.Checked)
					RadioBtn = this.iniFile.GetSetting("Address", "SD3");

				if (checkBox1.Checked)
					CB1 = this.iniFile.GetSetting("Address", "SD1");

				if (checkBox2.Checked)
					CB2 = this.iniFile.GetSetting("Address", "SD2");

				if (checkBox3.Checked)
					CB3 = this.iniFile.GetSetting("Address", "SD3");

				this.DialogResult = DialogResult.OK;
			}
		}

		private void Cancel_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("Are you sure you want to proceed?", "Cancel", MessageBoxButtons.YesNo);

			if (result == DialogResult.Yes)
			{
				this.RadioBtn = "";
				this.Close();
			}
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton1.Checked)
			{
				checkBox1.Enabled = false;
				checkBox1.Checked = false;
				checkBox2.Enabled = true;
				checkBox3.Enabled = true;
			}
		}

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton2.Checked)
			{
				checkBox1.Enabled = true;
				checkBox2.Enabled = false;
				checkBox2.Checked = false;
				checkBox3.Enabled = true;
			}
		}

		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButton3.Checked)
			{
				checkBox1.Enabled = true;
				checkBox2.Enabled = true;
				checkBox3.Enabled = false;
				checkBox3.Checked = false;
			}
		}
	}
}
