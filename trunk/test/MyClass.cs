/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
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