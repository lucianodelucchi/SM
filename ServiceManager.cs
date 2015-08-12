using System;
using System.Reactive.Linq;
using System.ServiceProcess;
using SM.Model;
using System.Reactive.Concurrency;

namespace SM
{
	public static class ServiceManager
	{
		public static IObservable<IService> GetServices()
		{
			return ServiceController.GetServices().ToObservable(TaskPoolScheduler.Default).SelectMany(async s => await Service.FromServiceControllerAsync(s));
		}
	}
}