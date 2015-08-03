using System;
using System.Reactive.Linq;
using System.ServiceProcess;
using SM.Model;

namespace SM
{
	public static class ServiceManager
	{
		public static IObservable<Service> GetServices()
		{
			return ServiceController.GetServices().ToObservable().SelectMany(async s => await Service.FromServiceControllerAsync(s));
		}
	}
}