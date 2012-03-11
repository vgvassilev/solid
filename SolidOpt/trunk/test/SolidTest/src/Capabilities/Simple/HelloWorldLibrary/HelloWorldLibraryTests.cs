/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using NUnit.Framework;

using SolidTest.Capabilities.Simple.HelloWorldLibrary;

namespace SolidTest.Capabilities.Simple.HelloWorldLibrary.Tests
{
	[TestFixture]
	public class HelloWorldLibraryTests
	{
		[Test]
		public void Test_SayHelloStatic()
		{
			Assert.AreEqual("Hello", Hello.SayHelloStatic());
		}
		
		[Test]
		public void Test_SayWorldStatic()
		{
			Assert.AreEqual("World", World.SayWorldStatic());
		}
		
		[Test]
		public void Test_SayHelloWorldStatic()
		{
			Assert.AreEqual("Hello World!", HelloWorld.SayHelloWorldStatic());
		}
		
		[Test]
		public void Test_SayHello()
		{
			Hello hello = new Hello();
			
			Assert.IsNotNull(hello);
			Assert.AreEqual("Hello", hello.SayHello());
		}
		
		[Test]
		public void Test_SayWorld()
		{
			World world = new World();
			
			Assert.IsNotNull(world);
			Assert.AreEqual("World", world.SayWorld());
		}
		
		[Test]
		public void Test_SayHelloWorld()
		{
			HelloWorld helloWorld = new HelloWorld();
			
			Assert.IsNotNull(helloWorld);
			Assert.AreEqual("Hello World!", helloWorld.SayHelloWorld());
		}
	}
}
