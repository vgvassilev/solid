/*
 * Created by SharpDevelop.
 * User: Sasho
 * Date: 11.8.2008
 * Time: 11:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

//using SolidOpt.Cache;

namespace SolidOpt.Services.Subsystems.Cache.Demo.Cache1
{

	
	/// <summary>
	/// Description of TestBase.
	/// </summary>
	public class TestBase
	{
		protected CacheManager<int,ResultClass> cache;
		
		public TestBase()
		{
		}
		
		public virtual void Test()
		{
			Console.WriteLine("Test {0}:", this.GetType().Name);
			
			Console.WriteLine("cache->{0}", cache);
			cache[1] = ClassBuilder.BuildResult(1);
			Thread.Sleep(2000);
			cache[2] = ClassBuilder.BuildResult(2);
			cache[3] = ClassBuilder.BuildResult(3);
			cache[4] = ClassBuilder.BuildResult(4);
			Console.WriteLine("cache->{0}", cache);
			Console.WriteLine("cache[1]->{0}", cache[1]);
			Console.WriteLine("cache->{0}", cache);
			cache.Remove(1);
			Console.WriteLine("cache->{0}", cache);
			Console.WriteLine("cache[1]->{0}", cache[1]);
			Console.WriteLine("cache->{0}", cache);
			Console.WriteLine("cache[2]->{0}", cache[2]);
			Console.WriteLine("cache->{0}", cache);
			
			Console.Write("Serializing state...");
			System.IO.FileStream fs = new FileStream("cache_"+this.GetType().Name+".txt", FileMode.Create);
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(fs, cache.State);
			fs.Close();
			Console.WriteLine("Done.");
			
			Console.Write("Deserializing state...");
			fs = new FileStream("cache_"+this.GetType().Name+".txt", FileMode.Open);
			bf = new BinaryFormatter();
			cache.State = bf.Deserialize(fs);
			fs.Close();
			Console.WriteLine("Done.");
			Console.WriteLine("cache->{0}", cache);
			
			Console.WriteLine("---");
			Console.WriteLine();
		}
	}
}
