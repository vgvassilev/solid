/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Mono.Cecil;
using Mono.Collections.Generic;

using NUnit.Framework;

namespace SolidOpt.Services.Transformations.Multimodel.Test
{

  /// <summary>
  /// Abstract out the common interfaces for running a single testcase.
  /// </summary>
  public abstract class BaseTestFixture<Source, Target, Transformer>
    where Transformer: SolidOpt.Services.Transformations.ITransform<Source, Target>, new()
  {
    private List<TestCaseDirective> directives = null;

    public BaseTestFixture()
    {
      string path = GetTestCasesDir();
      if (!Directory.Exists(path))
        throw new DirectoryNotFoundException("Directory not found. Please check testCasesDirCache variable");
      // Cleanup the last invocation.
      string[] testCases = Directory.GetFiles(path, "*." + GetTestCaseFileExtension());
      foreach (string testCaseName in testCases)
        Cleanup(Path.GetFileNameWithoutExtension(testCaseName)); // FIXME: Be smarter here. Don't cut.
    }

    /// <summary>
    /// Gets the test cases in the testCasesDir. Allows overriding of test cases. For example,
    /// <code> 
    /// [Test]
    /// public void MyTest() {}
    /// </code>
    /// defined in the derived class will override the current default logic.
    /// </summary>
    /// <returns>
    /// The TestCaseData representation of the test case.
    /// </returns>
    protected IEnumerable GetTestCases()
    {
      string[] testCases = Directory.GetFiles(GetTestCasesDir(), "*." + GetTestCaseFileExtension());
      HashSet<string> excludedTestCases = GetOverridenTestCases();

      foreach (string testCase in testCases) {
        if (!excludedTestCases.Contains(Path.GetFileNameWithoutExtension(testCase))) {
          TestCaseData data = new TestCaseData(testCase);
          data.SetName(Path.GetFileNameWithoutExtension(testCase));
          yield return data;
        }
      }
    }

    /// <summary>
    /// Runs a single test case.
    /// </summary>
    /// <param name='testCaseName'>
    /// Test case name.
    /// </param>
    /// The list of the Source entities to be transformed and verified.
    /// <param name='sources'>
    /// </param>
    public virtual void RunTestCase(string testCaseName, Source[] sources) {
      // Set invariant culture
      System.Threading.Thread.CurrentThread.CurrentCulture =
        System.Globalization.CultureInfo.InvariantCulture; 

      //FIXME: Here we do that and then reconstrunct the same path to source.
      testCaseName = Path.GetFileNameWithoutExtension(testCaseName);
      string testCaseFile = GetTestCaseFullPath(testCaseName);
      // Check whether the file exists first.
      Assert.IsTrue(File.Exists(testCaseFile),
                    String.Format("{0} does not exist.", testCaseName));

      string testCaseResultFile = GetTestCaseResultFullPath(testCaseName);
      // Check whether the result file exists first.
      Assert.IsTrue(File.Exists(testCaseResultFile),
                    String.Format("{0} does not exist.", testCaseResultFile));

      List<string> seen = new List<string>(); // in case of exception preventing seen to get value.

      bool testXFail = false;
      if (directives != null) // in cases where there were no directives in the test at all.
        testXFail = directives.Find(d => d.Kind == TestCaseDirective.Kinds.XFail) != null;
      try {
        Transformer transformer = new Transformer();
        foreach(Source source in sources) {
          Target target = transformer.Transform(source);
          seen.AddRange(TargetToString(target).Split('\n'));
        }
      }
      catch (Exception e) {
        // Fill in the debug files with the exception message.
        // This is useful when there is partially built 'stuff'.
        seen.AddRange(e.Message.Split('\n'));
        if (!testXFail) {
          throw e;
        }
      }
      finally {
        Validate(testCaseName, seen.ToArray());
      }
    }

    public virtual string TargetToString(Target target) {
      return target.ToString();
    }

    /// <summary>
    /// Compare whether the produced result is the expected result. Usually there is a file we want
    /// to check against. If the check fails the error is written to a string so that proper
    /// diagnostics can be produced..
    /// </summary>
    /// <param name='testCaseName'>
    /// The testcase name from which we will pickup the expected results.
    /// </param>
    /// <param name='seenLines'>
    /// What the algorithm actually produced.
    /// </param>
    /// <returns>True on success.</returns>
    public bool Validate(string testCaseName, string[] seenLines)
    {
      string resultFile = GetTestCaseResultFullPath(testCaseName);
      string debugFile = GetTestCaseOutFullPath(testCaseName);
      File.WriteAllLines(debugFile, seenLines);

      Process p = new Process();
      p.StartInfo.CreateNoWindow = true;
      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.RedirectStandardInput = true;
      p.StartInfo.RedirectStandardError = true;
      p.StartInfo.FileName = "diff";
      // --strip-trailing-cr avoid comparing the new lines, which are different for the different platforms.
      // --ignore-blank-lines because all test expected results end with blank line.
      p.StartInfo.Arguments = string.Format ("-u --strip-trailing-cr --ignore-blank-lines \"{0}\" \"{1}\"", 
                                             resultFile, debugFile);
      if (Environment.OSVersion.Platform == PlatformID.Win32Windows) {
        p.StartInfo.Arguments = string.Format ("\"{0}\" \"{1}\"", resultFile, debugFile);
        p.StartInfo.FileName = "FB";
      }
      // Log the invocation.
      LogProcessInvocation(p, testCaseName);
      p.Start();
      string output = p.StandardOutput.ReadToEnd();
      //string error = p.StandardError.ReadToEnd();
      p.WaitForExit();

      //if (error != String.Empty)
      //  errMsg += string.Format("Errors: {0}", error);

      File.WriteAllText(debugFile, output);
      // if the exit code is 0 this means there is no difference.
      if (p.ExitCode > 0) {
        //errMsg += PrintTestCaseRunInfo(testCaseName);
        PrintTestCaseRunInfo(testCaseName);
      } 
      else if (p.ExitCode == 0)
        Cleanup(testCaseName);

      bool match = p.ExitCode == 0;

      bool testXFail = false;
      if (directives != null) // in cases where there were no directives in the test at all.
        testXFail = directives.Find(d => d.Kind == TestCaseDirective.Kinds.XFail) != null;
      if (testXFail && match) {
        //errMsg += "\nUnexpected pass.";
        //Assert.Fail(errMsg);
        Assert.Fail("\nUnexpected pass, diff file {0}", debugFile);
        return false;
      }
      else if (testXFail) {
        //errMsg += "\nExpected to fail.";
        //Assert.Ignore(errMsg);
        Assert.Ignore("\nExpected to fail, diff file {0}", debugFile);
        return true;
      }
      else {
        if (!match) {
          // if the test wasn't expected to fail write out the what was seen so that one can diff
          File.WriteAllLines(debugFile, seenLines);
        }
        //Assert.IsTrue(match, errMsg);
        Assert.IsTrue(match, string.Format("\nTest failed, see {0}", debugFile));
        return match;
      }

    }

    private void LogProcessInvocation(Process p, string testCaseName) {
      string invokeFile = GetTestCaseOutFullPath(testCaseName) + ".invoke";
      StringBuilder sb = new StringBuilder();
      sb.AppendFormat("{0} {1}",p.StartInfo.FileName, p.StartInfo.Arguments);
      sb.AppendLine();
      File.AppendAllText(invokeFile, sb.ToString()); 
    }

    private void PrintTestCaseRunInfo(string testCaseName)
    {
      string debugFile = GetTestCaseOutFullPath(testCaseName);
      string invokeFile = debugFile + ".invoke";
      if (File.Exists(invokeFile)) {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine((char)27 + "[31Invocation:");
        Console.WriteLine(File.ReadAllText(invokeFile));
        Console.ResetColor();
      }
      if (File.Exists(debugFile)) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(File.ReadAllText(debugFile));
        Console.ResetColor();
      }
    }

    /// <summary>
    /// In the process of testing we write a debug output to different files. However if the 
    /// test was successful there is no need of keeping those files around.
    /// </summary>
    /// <param name="testCaseName">Test case name.</param>
    private void Cleanup(string testCaseName)
    {
      string debugFile = GetTestCaseOutFullPath(testCaseName);
      string invokeFile = debugFile + ".invoke";
      string dllFile 
        = Path.Combine(GetTestCasesBuildDir(), Path.GetFileNameWithoutExtension(testCaseName) + ".dll");
      if (File.Exists(debugFile))
        File.Delete(debugFile);
      if (File.Exists(invokeFile))
        File.Delete(invokeFile);
      if (File.Exists(dllFile))
        File.Delete(dllFile);
    }

    /// <summary>
    /// Compiles the given test case and returns the main method of the compiled assembly.
    /// </summary>
    /// <returns>
    /// The main method of the assembly.
    /// </returns>
    /// <param name='testCaseName'>
    /// The test case to be loaded.
    /// </param>
    /// <param name='methods'>
    /// List of optional 'other' methods to check besides Main.
    /// </param>
    protected MethodDefinition[] LoadTestCaseMethod(string testCaseName, params string[] methods)
    {
      //FIXME: Here we do that and then reconstrunct the same path to source.
      testCaseName = Path.GetFileNameWithoutExtension(testCaseName);
      TestCaseDirectiveParser dirParser = new TestCaseDirectiveParser(GetTestCaseFullPath(testCaseName));
      directives = dirParser.ParseDirectives();

      AssemblyDefinition assembly = CompileTestCase(testCaseName);

      TypeDefinition type = assembly.MainModule.GetType("TestCase");
      Assert.IsNotNull (type, "Type TestCase not found!");
      List<MethodDefinition> result = new List<MethodDefinition>();
      // Add by default the Main method.
      MethodDefinition found = GetMethod(type.Methods, "Main");
      Assert.IsNotNull(found, "Method TestCase.Main not found!");
      result.Add(found);
      foreach (string s in methods) {
        found = GetMethod(type.Methods, s);
        Assert.IsNotNull(found, String.Format("Method {0} not found!", s));
        result.Add(found);
      }
      return result.ToArray();
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
    protected virtual AssemblyDefinition CompileTestCase(string testCase)
    {
      string sourceFile = GetTestCaseFullPath(testCase);
      Assert.IsTrue(File.Exists(sourceFile), sourceFile + " not found!");
      string testCaseAssemblyName
          = Path.Combine(GetTestCasesBuildDir(), Path.GetFileNameWithoutExtension(sourceFile)+".dll");

      TestCaseDirective runDir = directives.Find(d => d.Kind == TestCaseDirective.Kinds.Run);
      Assert.NotNull(runDir, "Are you missing RUN: directive?");

      Process p = new Process();
      p.StartInfo.Arguments = runDir.Arguments;
      p.StartInfo.CreateNoWindow = true;
      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.RedirectStandardInput = true;
      p.StartInfo.RedirectStandardError = true;
      p.StartInfo.FileName = runDir.Command;
      p.StartInfo.WorkingDirectory = GetTestCasesBuildDir();
      // Log the invocation
      LogProcessInvocation(p, testCase);
      p.Start();
      string output = p.StandardOutput.ReadToEnd();
      string error = p.StandardError.ReadToEnd();
      p.WaitForExit();
      Assert.AreEqual(0, p.ExitCode, output + error);

      var resolver = new DefaultAssemblyResolver();
      resolver.AddSearchDirectory(SolidOpt.BuildInformation.BuildInfo.LibraryOutputDir);
      ReaderParameters @params = new ReaderParameters();
      @params.AssemblyResolver = resolver;
      return AssemblyDefinition.ReadAssembly(testCaseAssemblyName, @params);
    }

    #region Util functions

    /// <summary>
    /// Scans the TextFixture for TestAttributes and adds them in a hash set.
    /// </summary>
    /// <returns>
    /// The overriden test cases.
    /// </returns>
    protected HashSet<string> GetOverridenTestCases()
    {
      //TODO: HashSet<> is .net 4.0 class. May be we need use some 2.0 class (Dictionary<,>)?
      HashSet<string> res = new HashSet<string>();
      Type ty = GetType();
      foreach(System.Reflection.MethodInfo mInfo in ty.GetMethods()) {
        if (mInfo.GetCustomAttributes(typeof(TestAttribute), true).Length == 1)
          res.Add(mInfo.Name);
      }
      return res;
    }

    protected string GetTestCasesDir()
    {
      return Path.Combine(BuildInformation.BuildInfo.SourceDir, GetTestCaseDirOffset());
    }

    protected string GetTestCasesBuildDir()
    {
      return Path.Combine(BuildInformation.BuildInfo.BinaryDir, GetTestCaseDirOffset());
    }

    /// <summary>
    /// Usually the test cases are contained in a sub-folder.
    /// </summary>
    /// <returns>The the offset from the SourceDir.</returns>
    ///
    protected virtual string GetTestCaseDirOffset()
    {
      return "";
    }

    protected string GetTestCaseFullPath(string testCaseName)
    {
      string result = Path.Combine(GetTestCasesDir(), testCaseName);
      result = Path.ChangeExtension(result, GetTestCaseFileExtension());
      return Path.GetFullPath(result);
    }

    protected string GetTestCaseResultFullPath(string testCaseName)
    {
      string result = Path.Combine(GetTestCasesDir(), testCaseName);
      result = Path.ChangeExtension(result, GetTestCaseResultFileExtension());
      return Path.GetFullPath(result);
    }

    protected string GetTestCaseOutFullPath(string testCaseName)
    {
      string result = Path.Combine(GetTestCasesBuildDir(), testCaseName);
      result = Path.ChangeExtension(result, GetTestCaseResultFileExtension());
      result = Path.GetFullPath(result);
      return result + '.' + GetTestCaseOutFileExtension();
    }

    /// <summary>
    /// Subclasses should implement the method to provide test case specific extensions.
    /// </summary>
    /// <returns>
    /// The test case file extension.
    /// </returns>
    protected virtual string GetTestCaseFileExtension()
    {
      return "tst";
    }

    /// <summary>
    /// Subclasses should implement the method to provide test case result specific extensions.
    /// </summary>
    /// <returns>
    /// The test case result file extension.
    /// </returns>
    protected virtual string GetTestCaseResultFileExtension()
    {
      return "result";
    }

    /// <summary>
    /// Subclasses should implement the method to provide test case output specific extensions.
    /// </summary>
    /// <returns>
    /// The test case out file extension.
    /// </returns>
    protected virtual string GetTestCaseOutFileExtension()
    {
      return "debug";
    }


    /// <summary>
    /// Removes leading and trailing new lines and tabs.
    /// </summary>
    /// <param name='s'>
    /// The string to normalize.
    /// </param>
    protected static string Normalize(string s)
    {
      char[] CharsToTrim = new char[]{' ','\t'};
      // for Mac, Win, Lin
      return s.Normalize().Replace("\n\r", "\n").Replace("\r\n", "\n").Replace("\r", "\n").Trim(CharsToTrim);
    }

    /// <summary>
    /// Removes leading and trailing new lines and tabs.
    /// </summary>
    /// <param name='s'>
    /// The string array to normalize.
    /// </param>
    protected static string[] Normalize(string[] s)
    {
      char[] CharsToTrim = new char[]{' ','\t'};
      // for Mac, Win, Lin

      for (int i = 0; i < s.Length; i++)
        s[i] = s[i].Normalize().Replace("\n\r", "\n").Replace("\r\n", "\n").Replace("\r", "\n").Trim(CharsToTrim);
      return s;
   }


    /// <summary>
    /// Iterates over a collection of methods and finds the one with the given name.
    /// </summary>
    /// <returns>
    /// The method in the collection with the given namer.
    /// </returns>
    /// <param name='methods'>
    /// Collection of methods.
    /// </param>
    /// <param name='name'>
    /// The name to look for.
    /// </param>
    private MethodDefinition GetMethod(Collection<MethodDefinition> methods, string name)
    {
      foreach (MethodDefinition mDef in methods) {
        if (mDef.Name == name)
          return mDef;
      }
      return null;
    }

    #endregion
  }
}
