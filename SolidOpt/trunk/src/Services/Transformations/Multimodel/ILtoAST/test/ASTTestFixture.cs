/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */
using System;
using System.Diagnostics;
using System.IO;

using NUnit.Framework;

using Mono.Cecil;

using SolidOpt.Services.Transformations.CodeModel.AbstractSyntacticTree;
using SolidOpt.Services.Transformations.Multimodel.Test;

namespace SolidOpt.Services.Transformations.Multimodel.ILtoAST.Test
{
  [TestFixture()]
  public class ASTTestFixture : BaseTestFixture<MethodDefinition, AstMethodDefinition, ILtoASTTransformer>
  {
    protected override string GetTestCaseFileExtension()
    {
      return "cs";
    }
    
    protected override string GetTestCaseResultFileExtension()
    {
      return "cs.ast";
    }
    
    [Test, TestCaseSource("GetTestCases")] /*Comes from the base class*/
    public void Cases(string filename)
    {
      RunTestCase(filename, LoadTestCaseMethod(filename));
    }

    /// <summary>
    /// Compiles the test case into an assembly. The default implementation uses ilasm. The
    /// subclasses may override it if neccessary.
    /// </summary>
    /// <returns>
    /// The compiled assembly.
    /// </returns>
    /// <param name='testCase'>
    /// The input source file.
    /// </param>
    protected override AssemblyDefinition CompileTestCase(string testCase)
    {
      string sourceFile = GetTestCaseFullPath(testCase);
      Assert.IsTrue(File.Exists(sourceFile), sourceFile + " not found!");
      string testCaseAssemblyName
        = Path.Combine(testCasesTmpDir, Path.GetFileNameWithoutExtension(sourceFile)+".dll");
      
      string args = string.Format ("/t:library /out:{0} {1}", testCaseAssemblyName, sourceFile);
      Process p = new Process();
      p.StartInfo.Arguments = args;
      p.StartInfo.CreateNoWindow = true;
      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.RedirectStandardInput = true;
      p.StartInfo.RedirectStandardError = true;

      if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
        p.StartInfo.FileName = "csc";
      else
        p.StartInfo.FileName = "mcs";

      p.Start();
      string output = p.StandardOutput.ReadToEnd();
      string error = p.StandardError.ReadToEnd();
      p.WaitForExit();
      Assert.AreEqual(0, p.ExitCode, output + error);
      
      return AssemblyDefinition.ReadAssembly(testCaseAssemblyName);
    }
  }
}

