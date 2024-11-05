namespace Scale_v3
{
	partial class Clear
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Clear));
			groupBox1 = new GroupBox();
			checkBox3 = new CheckBox();
			checkBox2 = new CheckBox();
			checkBox1 = new CheckBox();
			clearBtn = new Button();
			cancelBtn = new Button();
			groupBox1.SuspendLayout();
			SuspendLayout();
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(checkBox3);
			groupBox1.Controls.Add(checkBox2);
			groupBox1.Controls.Add(checkBox1);
			groupBox1.Location = new Point(12, 12);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(236, 100);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = "Devices";
			// 
			// checkBox3
			// 
			checkBox3.AutoSize = true;
			checkBox3.Location = new Point(67, 72);
			checkBox3.Name = "checkBox3";
			checkBox3.Size = new Size(107, 19);
			checkBox3.TabIndex = 2;
			checkBox3.Text = "S1: 192.168.1.23";
			checkBox3.UseVisualStyleBackColor = true;
			checkBox3.CheckedChanged += checkBox3_CheckedChanged;
			// 
			// checkBox2
			// 
			checkBox2.AutoSize = true;
			checkBox2.Location = new Point(67, 47);
			checkBox2.Name = "checkBox2";
			checkBox2.Size = new Size(107, 19);
			checkBox2.TabIndex = 1;
			checkBox2.Text = "S1: 192.168.1.22";
			checkBox2.UseVisualStyleBackColor = true;
			// 
			// checkBox1
			// 
			checkBox1.AutoSize = true;
			checkBox1.Location = new Point(67, 22);
			checkBox1.Name = "checkBox1";
			checkBox1.Size = new Size(107, 19);
			checkBox1.TabIndex = 0;
			checkBox1.Text = "S1: 192.168.1.21";
			checkBox1.UseVisualStyleBackColor = true;
			// 
			// clearBtn
			// 
			clearBtn.ForeColor = Color.Red;
			clearBtn.Location = new Point(93, 118);
			clearBtn.Name = "clearBtn";
			clearBtn.Size = new Size(75, 29);
			clearBtn.TabIndex = 1;
			clearBtn.Text = "Clear";
			clearBtn.UseVisualStyleBackColor = true;
			clearBtn.Click += clearBtn_Click;
			// 
			// cancelBtn
			// 
			cancelBtn.ForeColor = Color.Black;
			cancelBtn.Location = new Point(174, 118);
			cancelBtn.Name = "cancelBtn";
			cancelBtn.Size = new Size(75, 29);
			cancelBtn.TabIndex = 2;
			cancelBtn.Text = "Cancel";
			cancelBtn.UseVisualStyleBackColor = true;
			cancelBtn.Click += cancelBtn_Click;
			// 
			// Clear
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(260, 156);
			Controls.Add(cancelBtn);
			Controls.Add(clearBtn);
			Controls.Add(groupBox1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Name = "Clear";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Clear";
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private GroupBox groupBox1;
		private CheckBox checkBox3;
		private CheckBox checkBox2;
		private CheckBox checkBox1;
		private Button clearBtn;
		private Button cancelBtn;
	}
}