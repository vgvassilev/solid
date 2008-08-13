/*
 * Created by SharpDevelop.
 * User: Sasho
 * Date: 08.8.2008
 * Time: 13:42
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Cache1
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.Serialization.Formatters.Binary;
	using OpenF.Lib.Cache;
	
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Cache1");
//			new TestDefault().Test();
//			new TestMem1().Test();
//			new TestMem2().Test();
//			new TestMemExpire().Test();
			new TestMemIsolatedStorage().Test();
			
			Console.Write("Press any key to continue... ");
			Console.ReadKey(true);
		}
	}
}
