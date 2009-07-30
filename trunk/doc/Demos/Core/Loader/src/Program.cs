/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 05.8.2008 ã.
 * Time: 11:07
 * 
 */
using System;

using SolidOpt.Core.Loader;
using SolidOpt.Core.Services;

using TestPluginService;

namespace Load
{
	class Program
	{
		public static int Main(string[] args)
		{
			
			Console.WriteLine("Start...");
			
			Loader l = new Loader();
			int result = l.Run(args);
			foreach (IService srv in l.ServicesContainer.services) {
				Console.WriteLine(srv.GetType());
			}
			
			IAddService addition = (IAddService) l.ServicesContainer.GetService(typeof(IAddService));
			Console.WriteLine(addition.Add(1, 2));
			
			IService[] additionArr = (IService[]) l.ServicesContainer.GetServices(typeof(IAddService));
			foreach (IAddService srv in additionArr)
				Console.WriteLine(srv.Add(1, 2));
			
			Console.ReadKey();
			return result;
		}
	}
}
