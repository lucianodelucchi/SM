﻿using System;
using System.Management;
using System.Threading.Tasks;

namespace SM.Utils
{
	internal sealed class WMI
	{
		public string ServiceName { get; set; }
		static AsyncLazy<ManagementObject> ManagementObject;
		
		public WMI()
		{
			ManagementObject = new AsyncLazy<ManagementObject>(() => new ManagementObject());
		}
		
		public async Task<string> GetWMIProperty(string field)
		{
			if (string.IsNullOrEmpty(field)) 
			{
				throw new ArgumentNullException("field");
			}
			
			if (string.IsNullOrEmpty(this.ServiceName)) 
			{
				throw new InvalidOperationException("You have to provide a valid ServiceName before calling this method.");
			}
			
			var wmiService = await ManagementObject;
			wmiService.Path = new ManagementPath("Win32_Service.Name='" + ServiceName + "'");
			wmiService.Get();

			if (wmiService[field] == null) 
			{
				return string.Empty;
			}
			
			return wmiService[field].ToString();
		}
	}
}
