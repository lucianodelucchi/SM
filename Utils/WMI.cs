using System;
using System.Management;
using System.Threading.Tasks;

namespace SM.Utils
{
	/// <summary>
	/// Description of WMI.
	/// </summary>
	internal sealed class WMI
	{
		readonly string ServiceName;
		static AsyncLazy<ManagementObject> ManagementObject;
		
		public WMI(string serviceName)
		{
			ServiceName = serviceName;
			ManagementObject = new AsyncLazy<ManagementObject>(() => new ManagementObject());
		}
		
		public async Task<string> GetWMIProperty(string field)
		{
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
