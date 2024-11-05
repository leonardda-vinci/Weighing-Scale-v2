namespace Scale_v3
{
	partial class Copy
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Copy));
			groupBox1 = new GroupBox();
			radioButton3 = new RadioButton();
			radioButton2 = new RadioButton();
			radioButton1 = new RadioButton();
			groupBox2 = new GroupBox();
			checkBox3 = new CheckBox();
			checkBox2 = new CheckBox();
			checkBox1 = new CheckBox();
			copyBtn = new Button();
			Cancel = new Button();
			groupBox1.SuspendLayout();
			groupBox2.SuspendLayout();
			SuspendLayout();
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(radioButton3);
			groupBox1.Controls.Add(radioButton2);
			groupBox1.Controls.Add(radioButton1);
			groupBox1.Location = new Point(18, 12);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(117, 94);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = "From";
			// 
			// radioButton3
			// 
			radioButton3.AutoSize = true;
			radioButton3.Location = new Point(6, 65);
			radioButton3.Name = "radioButton3";
			radioButton3.Size = new Size(106, 19);
			radioButton3.TabIndex = 2;
			radioButton3.TabStop = true;
			radioButton3.Text = "S3: 192.168.1.23";
			radioButton3.UseVisualStyleBackColor = true;
			radioButton3.CheckedChanged += radioButton3_CheckedChanged;
			// 
			// radioButton2
			// 
			radioButton2.AutoSize = true;
			radioButton2.Location = new Point(6, 41);
			radioButton2.Name = "radioButton2";
			radioButton2.Size = new Size(106, 19);
			radioButton2.TabIndex = 1;
			radioButton2.TabStop = true;
			radioButton2.Text = "S2: 192.168.1.22";
			radioButton2.UseVisualStyleBackColor = true;
			radioButton2.CheckedChanged += radioButton2_CheckedChanged;
			// 
			// radioButton1
			// 
			radioButton1.AutoSize = true;
			radioButton1.Location = new Point(6, 16);
			radioButton1.Name = "radioButton1";
			radioButton1.Size = new Size(106, 19);
			radioButton1.TabIndex = 0;
			radioButton1.TabStop = true;
			radioButton1.Text = "S1: 192.168.1.21";
			radioButton1.UseVisualStyleBackColor = true;
			radioButton1.CheckedChanged += radioButton1_CheckedChanged;
			// 
			// groupBox2
			// 
			groupBox2.Controls.Add(checkBox3);
			groupBox2.Controls.Add(checkBox2);
			groupBox2.Controls.Add(checkBox1);
			groupBox2.Location = new Point(157, 12);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new Size(118, 94);
			groupBox2.TabIndex = 1;
			groupBox2.TabStop = false;
			groupBox2.Text = "To";
			// 
			// checkBox3
			// 
			checkBox3.AutoSize = true;
			checkBox3.Location = new Point(6, 65);
			checkBox3.Name = "checkBox3";
			checkBox3.Size = new Size(107, 19);
			checkBox3.TabIndex = 4;
			checkBox3.Text = "S3: 192.168.1.23";
			checkBox3.UseVisualStyleBackColor = true;
			// 
			// checkBox2
			// 
			checkBox2.AutoSize = true;
			checkBox2.Location = new Point(6, 42);
			checkBox2.Name = "checkBox2";
			checkBox2.Size = new Size(107, 19);
			checkBox2.TabIndex = 3;
			checkBox2.Text = "S2: 192,168.1.22";
			checkBox2.UseVisualStyleBackColor = true;
			// 
			// checkBox1
			// 
			checkBox1.AutoSize = true;
			checkBox1.Location = new Point(6, 17);
			checkBox1.Name = "checkBox1";
			checkBox1.Size = new Size(107, 19);
			checkBox1.TabIndex = 2;
			checkBox1.Text = "S1: 192.168.1.21";
			checkBox1.UseVisualStyleBackColor = true;
			// 
			// copyBtn
			// 
			copyBtn.ForeColor = Color.LimeGreen;
			copyBtn.Location = new Point(119, 112);
			copyBtn.Name = "copyBtn";
			copyBtn.Size = new Size(75, 29);
			copyBtn.TabIndex = 5;
			copyBtn.Text = "Copy";
			copyBtn.UseVisualStyleBackColor = true;
			copyBtn.Click += copyBtn_Click;
			// 
			// Cancel
			// 
			Cancel.ForeColor = Color.OrangeRed;
			Cancel.Location = new Point(202, 112);
			Cancel.Name = "Cancel";
			Cancel.Size = new Size(75, 29);
			Cancel.TabIndex = 6;
			Cancel.Text = "Cancel";
			Cancel.UseVisualStyleBackColor = true;
			Cancel.Click += Cancel_Click;
			// 
			// Copy
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(293, 148);
			Controls.Add(Cancel);
			Controls.Add(copyBtn);
			Controls.Add(groupBox2);
			Controls.Add(groupBox1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			Name = "Copy";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Copy";
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private GroupBox groupBox1;
		private RadioButton radioButton3;
		private RadioButton radioButton2;
		private RadioButton radioButton1;
		private GroupBox groupBox2;
		private CheckBox checkBox1;
		private CheckBox checkBox3;
		private CheckBox checkBox2;
		private Button copyBtn;
		private Button Cancel;
	}
}