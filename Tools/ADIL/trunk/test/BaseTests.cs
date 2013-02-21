// /*
//  * $Id:
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */
//
using System;
using NUnit.Framework;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Reflection;
using System.Linq;

namespace SolidOpt.Tools.ADIL.Test
{
    [TestFixture]
    public class BaseTests
    {
        public AssemblyDefinition FuncsAssembly;
        public TypeDefinition FuncsType;
        public AppDomain SandboxAppDomain; 

        [TestFixtureSetUp]
        public void Init()
        {
            //
        }

        [TestFixtureTearDown]
        public void Done()
        {
            //
        }

        public MethodInfo GetDiff(string methodName)
        {
            AssemblyDefinition FuncsAssembly = AssemblyDefinition.ReadAssembly("ADIL.Test.Func.dll");
            TypeDefinition FuncsType = FuncsAssembly.MainModule.GetType("ADIL.Test.Func.TestFuncs");

            MethodDefinition df = ADIL.d(FuncsType.Methods.First(m => m.Name == methodName));

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            FuncsAssembly.MainModule.Mvid = Guid.NewGuid();
            FuncsAssembly.MainModule.Name =  "diff_" + FuncsAssembly.MainModule.Name;
            FuncsAssembly.Name.Version = new Version(FuncsAssembly.Name.Version.Major+1, FuncsAssembly.Name.Version.Minor);
            FuncsAssembly.Name.Name = "diff_" + FuncsAssembly.Name.Name;
            FuncsAssembly.Write(FuncsAssembly.Name.Name + ".dll");
            FuncsAssembly.Write(ms);

            AppDomain SandboxAppDomain = AppDomain.CreateDomain("sandbox");
            Assembly sandboxedAssembly = SandboxAppDomain.Load(ms.GetBuffer());
            Type t = sandboxedAssembly.GetType("ADIL.Test.Func.TestFuncs");
            return t.GetMethod("d_" + methodName);
            //AppDomain.Unload(SandboxAppDomain);
        }

        [Test]
        public void dxf1()
        {
            MethodInfo mi = GetDiff("f1");
            Assert.AreEqual(0, mi.Invoke(null, new Object[]{0}));
            Assert.AreEqual(2, mi.Invoke(null, new Object[]{1}));
            Assert.AreEqual(4, mi.Invoke(null, new Object[]{2}));
            Assert.AreEqual(6, mi.Invoke(null, new Object[]{3}));
        }

        [Test]
        public void dxf1_again()
        {
            MethodInfo mi = GetDiff("f1");
            Assert.AreEqual(0, mi.Invoke(null, new Object[]{0}));
            Assert.AreEqual(2, mi.Invoke(null, new Object[]{1}));
            Assert.AreEqual(4, mi.Invoke(null, new Object[]{2}));
            Assert.AreEqual(6, mi.Invoke(null, new Object[]{3}));
        }

    }
}

