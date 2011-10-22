/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using NUnit.Framework;
	
using SolidOpt.Core.Loader;

namespace SolidOpt.Core.Loader.Tests
{
	/// <summary>
	/// Loader test cases.
	/// </summary>
	[TestFixture]
	public class Loader
	{
		[Test]
		public void TestBaseLoader()
		{
			int a = 5;
			if (a == 6)
				a = 7;
			Assert.AreEqual(a, 5);
		}
	}
}