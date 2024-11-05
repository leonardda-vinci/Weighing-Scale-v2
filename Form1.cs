
using Microsoft.VisualBasic.Logging;
using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Scale_v3
{
	public partial class Form1 : Form
	{
		string[] devices;
		string[] deviceStatus;
		string sourceDir;
		string downloadDir;
		string extractDir;
		string aclasApp;
		string inDir;
		List<string> scale;

		private FileSystemWatcher fileWatcher;
		IniParser iniFile;

		public Form1()
		{
			this.devices = new string[3];
			this.deviceStatus = new string[3];
			this.scale = new List<string>();

			this.iniFile = new IniParser(@"C:\RCS\Scale\config.ini");
			this.sourceDir = this.iniFile.GetSetting("Settings", "Source");
			this.downloadDir = this.iniFile.GetSetting("Target", "DLPath");
			this.aclasApp = this.iniFile.GetSetting("APP", "EXE");
			this.extractDir = this.iniFile.GetSetting("Target", "ExtractPath");
			this.inDir = this.iniFile.GetSetting("Settings", "In");

			Console.WriteLine("Source = " + this.iniFile.GetSetting("Settings", "Source"));

			InitializeComponent();


			if (!Directory.Exists(this.downloadDir)) Directory.CreateDirectory(this.downloadDir);

			if (!Directory.Exists(this.extractDir)) Directory.CreateDirectory(this.extractDir);

			if (InitClass.CheckApp() == true)
			{
				MessageBox.Show("Application is already running.", "Weighing Scale Integration", MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Close();
				Application.Exit();
			}

			logTextBox.AppendText($"Weighing Scale is running...\r\n");
			InitializeWatcher();
			bingoFound();
		}

		private void InitializeWatcher()
		{
			try
			{
				fileWatcher = new FileSystemWatcher(this.sourceDir, "*bingo");
				fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
				fileWatcher.Changed += OnChanged;
				fileWatcher.Error += OnError;
				fileWatcher.EnableRaisingEvents = true;
				fileWatcher.InternalBufferSize = 64 * 1024;
			}
			catch (Exception err)
			{
				logTextBox.AppendText(err.Message);
			}
		}

		private void OnError(object sender, ErrorEventArgs e)
		{
			PrintException(e.GetException());
			if (e.GetException() is InternalBufferOverflowException) logs($"{e.GetException().Message}");
			InitializeWatcher();
			logs($"File System Watcher has been re-initialized successfully!");

			while (true)
			{
				if (Directory.Exists(this.sourceDir))
				{
					if (File.Exists("bingo")) bingoFound();
					InitializeWatcher();
					logs($"Successfully connected to {this.sourceDir}.");
					break;
				}
				else
				{
					logs($"Unable to access {this.sourceDir}. Retrying in 5 minutes...");
					Thread.Sleep(300000);
				}
			}
		}

		private void PrintException(Exception? err)
		{
			if (err == null) return;
			string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

			if (err.Message != null) logs($"[{timestamp}] Message: {err.Message}");

			if (err.InnerException != null) logs($"[{timestamp}] InnerException: {err.InnerException}");

			if (err.StackTrace != null) logs($"[{timestamp}] StackTrace: {err.StackTrace}");

			

			PrintException(err.InnerException);
			
		}

		private void OnChanged(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType != WatcherChangeTypes.Changed) return;

			Debug.WriteLine($"Changed: {e.FullPath} - {e.ChangeType}");
			if (e.FullPath.Contains("bingo"))
			{
				if (File.Exists(e.FullPath))
				{
					logTextBox.Invoke((Action)delegate
					{
						logTextBox.Text = "bingo file found!" + "\r\n" + logTextBox.Text;
					});

					if (!backgroundWorker1.IsBusy) 
						backgroundWorker1.RunWorkerAsync();
				}
			}
		}

		private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

			for (int i = 1; i <= this.devices.Length; i++)
			{
				string ip = this.iniFile.GetSetting("Address", "SD" + i.ToString());
				this.deviceStatus[i - 1] = timestamp + "|" + ip;
				if (CheckPing(ip))
				{
					logs($"Device {ip} is available!");
					this.devices[i - 1] = ip;
					this.deviceStatus[i - 1] += "|OK";
				}
				else
				{
					logs($"Device {ip} is not available!");
					this.deviceStatus[i - 1] += "|NOK";
					continue;
				}
			}
			UpDoDel();
		}

		private async void UpDoDel()
		{
			//logs("UpDoDel");
			Upload(this.devices);

			for (int i = 0; i < this.devices.Length; i++)
			{
				if (this.devices[i] != null)
				{
					Download(this.devices[i]);
				}
				else
				{
					this.deviceStatus[i] += "|NOK";
				}
			}

			StatusLogs();

			using (StreamWriter writer = new StreamWriter(this.inDir + "bingo"))
			{
				writer.WriteLine();
			}

			await this.HTTP(this.iniFile.GetSetting("Address", "Get"));

			for (int i = 0; i < this.devices.Length; i++)
			{
				if (this.devices[i] != null)
				{
					Delete(this.devices[i]);
				}
				else
				{
					this.deviceStatus[i] += "|NOK";
				}
			}
		}

		private bool CheckPing(string ip)
		{
			Ping sender = new Ping();
			PingOptions options = new PingOptions();
			PingReply reply = sender.Send(ip);
			logs($"Pinging " + ip);
			if (reply.Status == IPStatus.Success)
			{
				logs($"The {ip} IP address is available.");
				return true;
			}
			else
			{
				logs($"The {ip} IP address is not available");
				return false;
			}
		}

		private void Upload(string[] ip)
		{
			string[] files = Directory.GetFiles(this.sourceDir, "*.iu");

			if (files.Length > 0)
			{
				foreach (string file in files)
				{
					string name = Path.GetFileName(file);
					for (int i = 0; i < this.devices.Length; i++)
					{
						if (this.devices[i] == null)
						{
							this.deviceStatus[i] += "|NOK";
							continue; // Skip to the next iteration
						}

						logs($"Uploading {name} to {ip[i]}...");
						aclasCmd("Upload", name, ip[i], this.sourceDir);
						logs($"{name} was successfully uploaded to {ip[i]}...");
					}

					try
					{
						string destination = Path.Combine(this.downloadDir, name);
						if (File.Exists(destination))
						{
							File.Delete(destination);
						}
						File.Move(file, destination);
						logs($"{name} has been successfully moved!");
						if (File.Exists(this.sourceDir + "\\bingo"))
						{
							File.Delete(this.sourceDir + "\\bingo");
						}

					}
					catch (Exception ex)
					{
						logs($"Error moving file: {ex.Message}");
					}
				}
			}
		}

		private void Download(string ip)
		{
			DateTime date = DateTime.Today;
			string current = String.Format("{0:yyyyMMdd}", date);
			string filename = ip + "_PLU-UP_" + current + ".txt";
			string from = Path.Combine(this.inDir, filename);
			string to = Path.Combine(this.extractDir, filename);
			logs($"Downloading {ip} data...");
			aclasCmd("Download", filename, ip, this.inDir);
			try
			{
				logs($"{filename} was sucessfully created!");
				if (File.Exists(to))
				{
					File.Delete(to);
				}
				File.Copy(from, to);
			}
			catch (Exception err)
			{
				logs($"Failed to download {ip} data due to {err.Message}");
			}
		}

		private void Delete(string ip)
		{
			try
			{
				string[] delFiles = Directory.GetFiles(this.sourceDir, "*.d");

				foreach (string del in delFiles)
				{
					aclasCmd("Delete", del, ip, this.sourceDir);
					string destination = Path.Combine(this.downloadDir, del);
					if (File.Exists(destination))
					{
						File.Delete(destination);
					}
					File.Move(del, destination);
				}
			}
			catch (Exception err)
			{
				logs($"Failed to delete the item/s on {ip} due to {err.Message}");
			}
		}

		private async Task HTTP(string url)
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					var response = await client.GetAsync(url);
					if (response.IsSuccessStatusCode)
					{
					}
					else
					{
					}
				}
			}
			catch (Exception err)
			{
				logs($"Failed to upload data to the SMS Central due to {err.Message}");
			}
		}

		private void aclasCmd(string action, string filename, string ip, string target)
		{
			try
			{
				ProcessStartInfo startInfo = new ProcessStartInfo();
				startInfo.CreateNoWindow = false;
				startInfo.UseShellExecute = false;
				startInfo.FileName = this.aclasApp;
				startInfo.WindowStyle = ProcessWindowStyle.Hidden;

				switch (action)
				{
					case "Upload":
						startInfo.Arguments = @"-h " + ip + @":5002 -t DOWN -b PLU -n " + target + filename + " -f Ascii";
						break;
					case "Download":
						startInfo.Arguments = @"-h " + ip + @":5002 -t UP -b PLU -n " + target + filename + " -f Ascii";
						break;
					case "Delete":
						startInfo.Arguments = @"-h " + ip + @":5002 -t DEL -b PLU -n " + target + filename + " -f Ascii";
						break;
					default:
						logs($"{action} is not valid!");
						return;
				}

				using (Process process = Process.Start(startInfo))
				{
					if (process != null)
					{
						process.WaitForExit();
						for (int i = 0; i < this.deviceStatus.Length; i++)
						{
							if (this.deviceStatus[i]?.Contains(ip) == true)
							{
								string statusMessage = process.ExitCode == 0 ? "|OK" : "|NOD";
								this.deviceStatus[i] += statusMessage;
								break;
							}
						}
					}
				}
			}
			catch (Exception err)
			{
				logs($"An error occured: {err.Message}");
				for (int i = 0; i < this.deviceStatus.Length; i++)
				{
					if (this.deviceStatus[i].Contains(ip))
					{
						this.deviceStatus[i] += "|NOK";
						break;
					}
				}
			}
		}

		private void StatusLogs()
		{
			string filename = this.iniFile.GetSetting("Settings", "Branch") + ".SCALE.STATUS.LOGS.sta";
			string filepath = Path.Combine(this.inDir, filename);

			using (StreamWriter writer = new StreamWriter(filepath, false))
			{
				for (int i = 0; i < this.deviceStatus.Length; i++)
				{
					writer.WriteLine(this.deviceStatus[i]);
				}
			}
		}

		private void bingoFound()
		{
			string[] bingoFiles = Directory.GetFiles(this.sourceDir, "*bingo");
			if (bingoFiles.Length > 0)
				backgroundWorker1.RunWorkerAsync();
		}

		private void logs(string message)
		{
			logTextBox.Invoke((Action)delegate
			{
				logTextBox.Text = message + "\r\n" + logTextBox.Text;
			});
			ProcessLogs(message);
		}

		private void ProcessLogs(string message)
		{
			DateTime now = DateTime.Now;
			DateTime date = DateTime.Today;
			string current = String.Format("{0:yyyyMMdd}", date);
			string logPath = this.iniFile.GetSetting("Target", "StatusPath");
			string branch = this.iniFile.GetSetting("Settings", "Branch");
			string dat = (branch + "." + current + ".SCALE.LOGS.dat");
			string file = Path.Combine(logPath, dat);

			if (!Directory.Exists(logPath))
				Directory.CreateDirectory(logPath);

			using (StreamWriter sw = new StreamWriter(file, true))
			{
				sw.WriteLine($"{now:yyyy-MM-dd HH:mm:ss}" + "             " + message + "\r\n");
			}
		}

		private void Form1_Resize(object sender, EventArgs e)
		{
			if (FormWindowState.Minimized == this.WindowState)
			{
				Hide();
				notifyIcon1.Visible = true;
				this.ShowInTaskbar = false;
				notifyIcon1.Text = "Weighing Scale - Running";
			}
		}

		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			Show();
			this.WindowState = FormWindowState.Normal;
			this.ShowInTaskbar = true;
			notifyIcon1.Visible = false;
		}

		private void Form1_Shown(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
			notifyIcon1.Visible = true;
			this.ShowInTaskbar = false;
			notifyIcon1.Text = "Weighing Scale - Running";
		}

		private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled == true)
				logs("Cancelled!");
			else if (e.Error != null)
				logs($"Error: {e.Error.Message}");
			else
				logs("Weighing Scale Process Done!");


			fileWatcher?.Dispose();
			backgroundWorker1.CancelAsync();
			InitializeWatcher();
		}
		
		private async void copyBtn_Click(object sender, EventArgs e)
		{
			DateTime date = DateTime.Today;
			string current = String.Format("{0:yyyyMMdd}", date);
			string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			string filename = "PLU." + current + ".iu";
			string file = Path.Combine(this.sourceDir, filename);
			Copy copy = new Copy();
			copy.ShowDialog();

			if (!string.IsNullOrEmpty(copy.CB1)) this.scale.Add(copy.CB1);
			if (!string.IsNullOrEmpty(copy.CB2)) this.scale.Add(copy.CB2);
			if (!string.IsNullOrEmpty(copy.CB3)) this.scale.Add(copy.CB3);

			string radioBtn = copy.RadioBtn;

			if (radioBtn != "")
			{
				for (int i = 1; i <= 3; i++)
				{
					if (radioBtn == this.iniFile.GetSetting("Address", "SD" + i.ToString()))
					{
						logs($"Copying data of {radioBtn}, please wait...");
						aclasCmd("Download", filename, radioBtn, this.sourceDir);
						if (File.Exists(this.sourceDir + filename)) logs($"{filename} has been successfully created!");
						break;
					}
				}

				if (file.Length > 0)
				{
					foreach (string sd in this.scale)
					{
						aclasCmd("Upload", filename, sd, this.sourceDir);
						aclasCmd("Download", (sd + "_PLU-UP_" + current + ".txt"), sd, this.inDir);
						logs($"{radioBtn} data has been successfully transfered to {sd}.");
					}

					if (File.Exists(this.downloadDir + filename)) 
						File.Delete(this.downloadDir + filename);
					File.Move(file, this.downloadDir + filename);

					using (StreamWriter writer = new StreamWriter(this.inDir + "bingo"))
					{
						writer.WriteLine();
					}

					await this.HTTP(this.iniFile.GetSetting("Address", "Get"));
					InitializeWatcher();
				}
			}
		}

		private async void clearBtn_Click(object sender, EventArgs e)
		{
			DateTime date = DateTime.Today;
			string current = String.Format("{0:yyyyMMdd}", date);
			string filename = "PLU." + current + ".d";
			string file = Path.Combine(this.sourceDir, filename);
			Clear clear = new Clear();
			clear.ShowDialog();

			if (!string.IsNullOrEmpty(clear.CB1)) this.scale.Add(clear.CB1);
			if (!string.IsNullOrEmpty(clear.CB2)) this.scale.Add(clear.CB2);
			if (!string.IsNullOrEmpty(clear.CB3)) this.scale.Add(clear.CB3);

			foreach (string sd in scale)
			{
				for (int i = 1; i <= 3; i++)
				{
					if (sd == this.iniFile.GetSetting("Address", "SD" + i.ToString()))
					{
						aclasCmd("Download", filename, sd, this.sourceDir);
						if (File.Exists(this.sourceDir + filename))
							logs($"{filename} has been successfully created");
					}
				}

				if (file.Length > 0)
				{
					aclasCmd("Delete", file, sd, this.sourceDir);
					aclasCmd("Download", (sd + "_PLU-UP_" + current + ".txt"), sd, this.inDir);
					logs($"{sd} data has been cleared successfully.");
				}

				if (File.Exists(this.downloadDir + filename)) File.Delete(this.downloadDir + filename);
				File.Move(this.sourceDir + filename, this.downloadDir + filename);

				using (StreamWriter writer = new StreamWriter(this.inDir + "bingo"))
				{
					writer.WriteLine();
				}

				await this.HTTP(this.iniFile.GetSetting("Address", "Get"));
				InitializeWatcher();
			}
		}
	}
}