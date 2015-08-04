﻿using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using SM.Utils;

namespace SM.Model
{
	public sealed class Service : IService
	{
		public string ServiceName { get; private set; }
		public string Description { get; private set; }
		public string StartName { get; private set; }
		public string DisplayName { get; private set; }
		
		AsyncLazy<ServiceController> SC { get; set; }
		
		WMI WMI;
		
		Service()
		{
			this.SC = new AsyncLazy<ServiceController>(() => new ServiceController(this.ServiceName));
			this.WMI = new WMI();
		}
		
		public static async Task<IService> FromServiceControllerAsync(ServiceController sc)
		{			
			var s = new Service();
			s.WMI.ServiceName = sc.ServiceName; 
			s.ServiceName = sc.ServiceName;
			s.Description = await s.WMI.GetWMIProperty("Description");
			s.StartName = await s.WMI.GetWMIProperty("StartName");
			s.DisplayName = await s.WMI.GetWMIProperty("DisplayName");
			
			return s;
		}
		
		public async Task<ServiceControllerStatus> GetStatus()
		{
			return (await this.SC).Status;
		}
		
		public async Task Continue()
		{
			if (await this.CanChangeTo(ServiceControllerStatus.Running))
				(await this.SC).Continue();
		}
		
		public async Task Pause()
		{
			if (await this.CanChangeTo(ServiceControllerStatus.Paused))
				(await this.SC).Pause();
		}
		
		public async Task Restart()
		{
			await this.Stop();
			await this.WaitForStatus(ServiceControllerStatus.Stopped);
			await this.Start();
		}
		
		public async Task Refresh()
		{
			(await this.SC).Refresh();
		}
		
		public async Task Start()
		{
			if (await this.CanChangeTo(ServiceControllerStatus.Running))
				(await this.SC).Start();
		}
		
		public async Task Stop()
		{
			if (await this.CanChangeTo(ServiceControllerStatus.Stopped))
				(await this.SC).Stop();
		}
		
		public async Task WaitForStatus(ServiceControllerStatus status)
		{
			(await this.SC).WaitForStatus(status);
		}
		
		async Task<bool> CanChangeTo(ServiceControllerStatus changeToStatus)
		{
			var actualStatus = (await this.GetStatus());
			
			switch (changeToStatus) {
				case ServiceControllerStatus.Running:
					return actualStatus.Equals(ServiceControllerStatus.Stopped) || actualStatus.Equals(ServiceControllerStatus.StopPending) || actualStatus.Equals(ServiceControllerStatus.Paused);
				case ServiceControllerStatus.Stopped:
					return (await this.SC).CanStop && !actualStatus.Equals(ServiceControllerStatus.Stopped);
				case ServiceControllerStatus.Paused:
					return (await this.SC).CanPauseAndContinue && (actualStatus.Equals(ServiceControllerStatus.Running) || actualStatus.Equals(ServiceControllerStatus.StartPending));
				default:
					return false;
			}
		}
	}
}