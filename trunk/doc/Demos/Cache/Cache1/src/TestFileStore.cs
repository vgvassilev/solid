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
	/// Description of TestMem1.
	/// </summary>
	public class TestMem1: TestBase
	{
		public TestMem1(): base()
		{
			this.cache = new CacheManager<int,ResultClass>(
				new CacheManager<int,ResultClass>.ValidateDelegate(Validate),
				new CacheManager<int,ResultClass>.CalculateDelegate(Calc),
				new CacheManager<int,ResultClass>.UpdateDelegate(Update)
			);
		}
		
		public bool Validate(int key, ResultClass value)
		{
			return key != 1;
		}
		
		public ResultClass Calc(int key)
		{
			return ClassBuilder.BuildResult(0); //key
		}
		
		public ResultClass Update(int key, ResultClass old_value)
		{
			old_value.Text += "!";
			return old_value;
		}
	}
}
