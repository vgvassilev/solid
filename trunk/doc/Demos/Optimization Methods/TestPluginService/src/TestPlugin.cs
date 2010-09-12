/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 24.6.2009 г.
 * Time: 11:34
 * 
 */
using System;
using System.Collections.Generic;

using SolidOpt.Core.Services;

namespace TestPluginService
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	public class TestPlugin : IAddService
	{		
		public int Add(int a, int b)
		{
			return a + b;
		}
	}
}