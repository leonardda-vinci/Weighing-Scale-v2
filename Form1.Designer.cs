namespace Scale_v3
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			logTextBox = new TextBox();
			pictureBox1 = new PictureBox();
			label1 = new Label();
			backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			notifyIcon1 = new NotifyIcon(components);
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			// 
			// logTextBox
			// 
			logTextBox.BackColor = SystemColors.ButtonHighlight;
			logTextBox.BorderStyle = BorderStyle.FixedSingle;
			logTextBox.Location = new Point(12, 12);
			logTextBox.Multiline = true;
			logTextBox.Name = "logTextBox";
			logTextBox.ReadOnly = true;
			logTextBox.ScrollBars = ScrollBars.Vertical;
			logTextBox.Size = new Size(524, 137);
			logTextBox.TabIndex = 0;
			// 
			// pictureBox1
			// 
			pictureBox1.ErrorImage = Properties.Resources.scale_icon;
			pictureBox1.Image = Properties.Resources.scale_icon;
			pictureBox1.InitialImage = Properties.Resources.scale_icon;
			pictureBox1.Location = new Point(551, 12);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(121, 119);
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox1.TabIndex = 1;
			pictureBox1.TabStop = false;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(583, 134);
			label1.Name = "label1";
			label1.Size = new Size(63, 15);
			label1.TabIndex = 2;
			label1.Text = "Version 3.0";
			// 
			// backgroundWorker1
			// 
			backgroundWorker1.WorkerSupportsCancellation = true;
			backgroundWorker1.DoWork += backgroundWorker1_DoWork;
			backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
			// 
			// notifyIcon1
			// 
			notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
			notifyIcon1.Text = "notifyIcon1";
			notifyIcon1.Visible = true;
			notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
			// 
			// Form1
			// 
			AutoScaleMode = AutoScaleMode.Inherit;
			ClientSize = new Size(684, 161);
			Controls.Add(label1);
			Controls.Add(pictureBox1);
			Controls.Add(logTextBox);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MaximizeBox = false;
			Name = "Form1";
			ShowInTaskbar = false;
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Weighing Scale";
			WindowState = FormWindowState.Minimized;
			Shown += Form1_Shown;
			Resize += Form1_Resize;
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private PictureBox pictureBox1;
		private Label label1;
		private NotifyIcon notifyIcon1;
		private TextBox logTextBox;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
	}
}
