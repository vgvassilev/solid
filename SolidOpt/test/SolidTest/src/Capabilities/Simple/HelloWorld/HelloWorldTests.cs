/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;

using NUnit.Framework;

using SolidTest.Capabilities.Simple.HelloWorld;

namespace SolidTest.Capabilities.Simple.HelloWorld.Tests
{
  [TestFixture]
  public class HelloWorldTests
  {
    [Test]
    public void Test_MainOutput()
    {
      TextWriter cout = new StringWriter();
      Console.SetOut(cout);
      
      HelloWorld.Main(new string[0]);
      
      Assert.AreEqual("Hello World!" + cout.NewLine, cout.ToString());
    }
    
    [Test]
    public void Test_MainOutputNullArgument()
    {
      TextWriter cout = new StringWriter();
      Console.SetOut(cout);
      
      HelloWorld.Main(null);
      
      Assert.AreEqual("Hello World!" + cout.NewLine, cout.ToString());
    }
  }
}
