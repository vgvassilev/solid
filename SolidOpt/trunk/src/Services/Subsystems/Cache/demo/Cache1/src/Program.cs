/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

namespace SolidOpt.Services.Subsystems.Cache.Demo.Cache1
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.Serialization.Formatters.Binary;
	using SolidOpt.Services.Subsystems.Cache;
	
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Cache1");
			new TestDefault().Test();
			new TestMem1().Test();
			new TestMem2().Test();
			new TestMemExpire().Test();
			new TestMemIsolatedStorage().Test();
			new TestMemIsolatedStorage1Collision().Test();
			
			
			Console.Write("Press any key to continue... ");
			Console.ReadKey(true);
		}
	}
}
