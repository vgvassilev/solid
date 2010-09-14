/*
 * Created by SharpDevelop.
 * User: Sasho
 * Date: 11.8.2008
 * Time: 11:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;

namespace SolidOpt.Services.Subsystems.Cache.Demo.Cache1
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.Serialization.Formatters.Binary;
	using SolidOpt.Services.Subsystems.Cache;
	
	/// <summary>
	/// Description of TestDefault.
	/// </summary>
	public class TestDefault: TestBase
	{
		public TestDefault(): base()
		{
			this.cache = new CacheManager<int,ResultClass>();
		}
	}
}
