/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

#region Assembly info

using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("SolidTest.Capabilities.Simple.HelloWorld")]
[assembly: AssemblyDescription("SolidOpt optimization framework. SolidTest test suite. Capabilities - HelloWorld program.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("SolidOpt.org")]
[assembly: AssemblyProduct("SolidOpt/SolidTest")]
[assembly: AssemblyCopyright("Copyright 2009-2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: AssemblyVersion("1.0.*")]

#endregion

namespace SolidTest.Capabilities.Simple.HelloWorld
{
  class HelloWorld
  {
    public static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
    }
  }
}