/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 28.1.2009 г.
 * Time: 15:58
 * 
 */
using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace test
{
	/// <summary>
	/// Description of MyClass.
	/// </summary>
	[TestFixture]
	public class MyClass
	{
		[Test]
		public void TestMethod1()
		{
			int a = 5;
			if (a == 6)
				a = 7;
			Assert.AreEqual(a, 5);
		}
	}
}