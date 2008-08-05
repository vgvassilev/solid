/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 05.8.2008 ?.
 * Time: 11:07
 * 
 */
using System;

using SolidOpt.Core.Loading;

namespace Load
{
	class Program
	{
		public static int Main(string[] args)
		{
			
			Console.WriteLine("Start...");
			return (new Loader()).Run(args);

		}
	}
}
