using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scale_v3
{
	internal class InitClass
	{
		public static bool CheckApp()
		{
			if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool Active()
		{
			if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
