/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Diagnostics;

namespace SolidOpt.Core.Loader.Demo.TransformLoader
{
	class Program
	{
		public static int Main(string[] args)
		{
			Console.WriteLine("Hello Loader!");
			
			Trace.Listeners.Add(new ConsoleTraceListener());
			
			int result = new TransformLoader().Run(args);
			
			Console.ReadKey(true);
			return result;
			
		}
	}
}