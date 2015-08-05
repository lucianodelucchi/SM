using System;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace SM.Model
{
	public interface IService
	{
		string ServiceName { get; }
		string Description { get; }
		string StartName { get; }
		string DisplayName { get; }
		ServiceControllerStatus Status{ get; }
		
		
		
		Task<ServiceControllerStatus> GetStatus();
		
		Task Continue();
		
		Task Pause();
		
		Task Restart();
		
		Task Refresh();
		
		Task Start();
		
		Task Stop();
		
		Task WaitForStatus(ServiceControllerStatus status);
	}
}
