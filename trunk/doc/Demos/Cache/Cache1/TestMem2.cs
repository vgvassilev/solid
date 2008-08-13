/*
 * Created by SharpDevelop.
 * User: Sasho
 * Date: 11.8.2008
 * Time: 11:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace Cache1
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.Serialization.Formatters.Binary;
	using OpenF.Lib.Cache;
	
	/// <summary>
	/// Description of TestMem2.
	/// </summary>
	public class TestMem2: TestBase
	{
		public TestMem2(): base()
		{
			this.cache = new CacheManager<int,ResultClass>(
				new CacheManager<int,ResultClass>.InitDelegate(Init),
				new CacheManager<int,ResultClass>.TouchDelegate(Touch),
				new CacheManager<int,ResultClass>.ValidateDelegate(Validate),
				new CacheManager<int,ResultClass>.CalculateDelegate(Calculate),
				new CacheManager<int,ResultClass>.UpdateDelegate(Update),
				new CacheManager<int,ResultClass>.DeleteDelegate(Delete)
			);
		}
		
		public CacheManager<int,ResultClass>.CacheItem Init(int key, ResultClass value)
		{
			return new CacheManager<int,ResultClass>.CacheItem(value);
		}
		
		public CacheManager<int,ResultClass>.CacheItem Touch(CacheManager<int,ResultClass>.CacheItem item)
		{
			return item;
		}
		
		public bool Validate(int key, ResultClass value, CacheManager<int,ResultClass>.CacheItem item)
		{
			return key != 1;
		}
		
		public ResultClass Calculate(int key)
		{
			return ClassBuilder.BuildResult(0); //key
		}
		
		public ResultClass Update(int key, ResultClass old_value)
		{
			old_value.Text += "!";
			return old_value;
		}
		
		public void Delete(CacheManager<int,ResultClass>.CacheItem item)
		{
			//
		}
	}
}
