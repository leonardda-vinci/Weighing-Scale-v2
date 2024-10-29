
using Microsoft.VisualBasic.Logging;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
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

		private FileSystemWatcher fileWatcher;
		IniParser iniFile;

		public Form1()
		{
			this.devices = new string[3];
			this.deviceStatus = new string[3];

			this.iniFile = new IniParser(@"C:\RCS\Scale\config.ini");
			this.sourceDir = this.iniFile.GetSetting("Settings", "Source");
			this.downloadDir = this.iniFile.GetSetting("Target", "DLPath");
			this.aclasApp = this.iniFile.GetSetting("APP", "EXE");
			this.extractDir = this.iniFile.GetSetting("Target", "ExtractPath");
			this.inDir = this.iniFile.GetSetting("Settings", "In");

			Console.WriteLine($"Source = " + this.iniFile.GetSetting("Settings", "Source"));

			InitializeComponent();


			if (!Directory.Exists(this.downloadDir))
			{
				Directory.CreateDirectory(this.downloadDir);
			}

			if (!Directory.Exists(this.extractDir))
			{
				Directory.CreateDirectory(this.extractDir);
			}

			if (InitClass.CheckApp() == true)
			{
				MessageBox.Show("Application is already running.", "Weighing Scale Integration", MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.Close();
				Application.Exit();
			}

			try
			{
				fileWatcher = new FileSystemWatcher(this.sourceDir);
				fileWatcher.Filter = "*bingo";
				fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
				fileWatcher.Changed += OnChanged;
				fileWatcher.EnableRaisingEvents = true;
			}
			catch (Exception err)
			{
				MessageBox.Show(err.Message);
			}

			logTextBox.AppendText("Scale is running...\r\n");
			bingoFound();
		}
		private void OnChanged(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType != WatcherChangeTypes.Changed)
			{
				return;
			}

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
					{
						backgroundWorker1.RunWorkerAsync();
					}
				}
			}
		}

		private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

			for (int i = 1; i <= 3; i++)
			{
				string ip = this.iniFile.GetSetting("Address", "SD" + i.ToString());
				this.deviceStatus[i - 1] = timestamp + "|" + ip;
				if (i == 1)
				{
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

				if (i == 2)
				{
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

				if (i == 3)
				{
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
			}
			UpDoDel();
		}

		private async void UpDoDel()
		{
			//logs("UpDoDel");
			Upload(this.devices);

			for (int i = 0; i < this.devices.Length; i++)
			{
				Download(this.devices[i]);
			}

			StatusLogs();

			using (StreamWriter writer = new StreamWriter(this.inDir + "bingo"))
			{
				writer.WriteLine();
			}

			await this.HTTP(this.iniFile.GetSetting("Address", "Get"));

			for (int i = 0; i < this.devices.Length; i++)
			{
				Delete(this.devices[i]);
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
						if (ip[i] == null)
						{
							continue;
						}
						logs($"Uploading {name} to {ip[i]}...");
						aclasCmd("Upload", name, ip[i]);
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

			if (ip == null)
			{
				return;
			}
			logs($"Downloading {ip} data...");
			aclasCmd("Download", filename, ip);
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
			if (ip == null)
			{
				return;
			}
			try
			{
				string[] delFiles = Directory.GetFiles(this.sourceDir, "*.d");

				foreach (string del in delFiles)
				{
					aclasCmd("Delete", del, ip);
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

		private void aclasCmd(string action, string filename, string ip)
		{
			try
			{
				ProcessStartInfo startInfo = new ProcessStartInfo();
				startInfo.CreateNoWindow = false;
				startInfo.UseShellExecute = false;
				startInfo.FileName = this.aclasApp;
				startInfo.WindowStyle = ProcessWindowStyle.Hidden;
				if (action == "Upload")
				{
					startInfo.Arguments = @"-h " + ip + @":5002 -t DOWN -b PLU -n " + this.sourceDir + filename + " -f Ascii";

				}
				else if (action == "Download")
				{
					startInfo.Arguments = @"-h " + ip + @":5002 -t UP -b PLU -n " + this.inDir + filename + " -f Ascii";

				}
				else if (action == "Delete")
				{
					startInfo.Arguments = @"-h " + ip + @":5002 -t DEL -b PLU -n " + this.sourceDir + filename + " -f Ascii";
				}
				else
				{
					logs("Action is not valid!");
				}

				using (Process process = Process.Start(startInfo))
				{
					if (process != null)
					{
						process.WaitForExit();
						for (int i = 0; i < this.deviceStatus.Length; i++)
						{
							if (this.deviceStatus[i].Contains(ip))
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
			{
				backgroundWorker1.RunWorkerAsync();
			}
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
			{
				Directory.CreateDirectory(logPath);
			}

			using (StreamWriter sw = new StreamWriter(file, true))
			{
				sw.WriteLine($"{now:yyyy-MM-dd HH:mm:ss}" + "             " + message + "\r\n");
			}
		}

		//private void processLogs(string message)
		//{
		//	DateTime now = DateTime.Now;
		//	DateTime date = DateTime.Today;
		//	string current = String.Format("{0:yyyyMMdd", date);
		//	string logPath = this.iniFile.GetSetting("Target", "StatusPath");
		//	string dat = this.iniFile.GetSetting("Settings", "Branch") + "." + current + ".SCALE.LOGS.dat";
		//	string file = Path.Combine(logPath, dat);

		//	if (!Directory.Exists(logPath))
		//	{
		//		Directory.CreateDirectory(logPath);
		//	}

		//	using (StreamWriter writer = new StreamWriter(file, true))
		//	{
		//		writer.WriteLine($"{now:yyyy-MM-dd HH:mm:ss}" + "             " + message);
		//	}
		//}

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
			{
				logs("Cancelled!");
			}
			else if (e.Error != null)
			{
				logs($"Error: {e.Error.Message}");
			}
			else
			{
				backgroundWorker1.CancelAsync();
				logs("Weighing Scale Process Done!");
			}
		}
	}
}
