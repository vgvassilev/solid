/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 19.8.2008 г.
 * Time: 12:09
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using SolidOpt.Core.Configurator;

namespace ConfigDemo
{
	/// <summary>
	/// Demonstrate the advantages of the use of the Assembly as main configuration model.
	/// It is type safe;
	/// It has code complete;
	/// It can be optimized by the JIT compiler or by using frameworks as Mono.Cecil.
	/// The example shows two different situations:
	/// It uses embeded configuration models/assemblies (Config1, Config2). They are part of
	/// the project (respectively the application);
	/// It uses imported model/assembly (Config3). This shows that can be used as backward 
	/// compatiability (eventually implemented as an optimizing method)
	/// </summary>
	class Program
	{
		[DllImport("kernel32.dll", SetLastError=true)] static extern void DebugBreak ();
		
		public static void Main(string[] args)
		{
			
//			Console.ReadKey(true);
//			DebugBreak();
			Console.WriteLine("{0} = {1}", Config1.str, Config1.a * Config1.b);
			
			Config1.a = 3;
			Config1.b = 4;
			Config1.str = "New surface of rect";
			
			Console.WriteLine("{0} = {1}", Config1.str, Config1.a * Config1.b);
			
			
			Console.WriteLine("{0} = {1}", Config2.str, Config2.pi * Config2.r * Config2.r);
			
//			Config2.pi = 3.20;
			Config2.r = 4;
			Config2.str = "New surface of circle";
			
			Console.WriteLine("{0} = {1}", Config2.str, Config2.pi * Config2.r * Config2.r);
			
			
//			Configuration in separate assembly
			Console.WriteLine("{0} = {1}", SeparateConfig.Config3.str, 
			                  (SeparateConfig.Config3.a + SeparateConfig.Config3.b)/2 * SeparateConfig.Config3.h);
			
			SeparateConfig.Config3.a = 8;
			SeparateConfig.Config3.b = 6;
			SeparateConfig.Config3.h = 5;
			SeparateConfig.Config3.str = "New surface of trapezium";
			
			Console.WriteLine("{0} = {1}", SeparateConfig.Config3.str, 
			                  (SeparateConfig.Config3.a + SeparateConfig.Config3.b)/2 * SeparateConfig.Config3.h);
			
			
//			Dynamic configuration
			
			ConfigurationManager<string> configurator = new ConfigurationManager<string>();
			
			configurator.SaveConfiguration(null);
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}
