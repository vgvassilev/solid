/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

#region Assembly info

using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("SolidTest.Capabilities.Simple.HelloWorldLibrary")]
[assembly: AssemblyDescription("SolidOpt optimization framework. SolidTest test suite. Capabilities - HelloWorld library.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("SolidOpt.org")]
[assembly: AssemblyProduct("SolidOpt/SolidTest")]
[assembly: AssemblyCopyright("Copyright 2009-2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: AssemblyVersion("1.0.*")]

#endregion

namespace SolidTest.Capabilities.Simple.HelloWorldLibrary
{
	class Hello
	{
		public static string SayHelloStatic()
		{
			return "Hello";
		}
		
		public string SayHello()
		{
			return "Hello";
		}
	}
	
	class World
	{
		public static string SayWorldStatic()
		{
			return "World";
		}
		
		public string SayWorld()
		{
			return "World";
		}
	}

	class HelloWorld
	{
		public static string SayHelloWorldStatic()
		{
			return Hello.SayHelloStatic() + " " + World.SayWorldStatic() + "!";
		}
		
		public  string SayHelloWorld()
		{
			Hello hello = new Hello();
			World world = new World();
			return hello.SayHello() + " " + world.SayWorld() + "!";
		}
	}
}