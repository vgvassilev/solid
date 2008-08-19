/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 19.8.2008 г.
 * Time: 12:09
 * 
 */
using System;
using System.Collections.Generic;

using SolidOpt.Core.Configurator;

namespace ConfigDemo
{
	class Program
	{
		public static void Main(string[] args)
		{
			ConfigurationManager<string> configurator = new ConfigurationManager<string>();
			
			Dictionary<string, object> testDict  = configurator.LoadConfiguration();
			
			foreach(KeyValuePair<string, object> item in testDict){
				Console.WriteLine("{0} is {1}", item.Key, (string)item.Value);
				
			}
			
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}
