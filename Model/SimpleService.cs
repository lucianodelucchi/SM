using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using SM.Utils;

namespace SM.Model
{
	public class SimpleService
	{
		public string ServiceName { get; private set; }
		public string Description { get; private set; }
		public string StartName { get; private set; }
		public string DisplayName { get; private set; }
		
		private ServiceController SC { get; set; }
		
		SimpleService()
		{
		}
		
		public static async Task<SimpleService> FromServiceControllerAsync(ServiceController sc)
		{
			var wmi = new WMI(sc.ServiceName);
			
			var s = new SimpleService();
			s.SC = sc;
			s.ServiceName = sc.ServiceName;
			s.Description = await wmi.GetWMIProperty("Description");
			s.StartName = await wmi.GetWMIProperty("StartName");;
			s.DisplayName = await wmi.GetWMIProperty("DisplayName");
			
			return s;
		}
		
		public ServiceControllerStatus GetStatus()
		{
			return this.SC.Status;
		}
		
		public void Continue()
		{
			this.SC.Continue();
		}
		
		public void Pause()
		{
			this.SC.Pause();
		}
		
		public void Refresh()
		{
			this.SC.Refresh();
		}
		
		public void Start()
		{
			this.SC.Start();
		}
		
		public void Stop()
		{
			this.SC.Stop();
		}
		
		public void WaitForStatus(ServiceControllerStatus status)
		{
			this.SC.WaitForStatus(status);
		}
	}
}
