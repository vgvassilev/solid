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

    /// <summary>
    /// The test case current dir.
    /// </summary>
    protected static string testCasesDir
      = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
             Path.Combine ("..",
             Path.Combine("..",
                   Path.Combine ("test", "TestCases" + Path.DirectorySeparatorChar))));
    /// <summary>
    /// The test cases tmp dir, where the temporary files will be stored.
    /// </summary>
    protected static string testCasesTmpDir = Path.Combine(testCasesDir, "Tmp");

    private List<TestCaseDirective> directives = null;

    public BaseTestFixture()
    {
      if (!Directory.Exists(testCasesTmpDir)) {
        Directory.CreateDirectory(testCasesTmpDir);
      }
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
      string[] testCases = Directory.GetFiles(testCasesDir, "*." + GetTestCaseFileExtension());
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
    public virtual void RunTestCase(string testCaseName, Source source) {
      bool testXFail = false;
      string testCaseFile = GetTestCaseFullPath(testCaseName);
      // Check whether the file exists first.
      Assert.IsTrue(File.Exists(testCaseFile),
                    String.Format("{0} does not exist.", testCaseName));

      string testCaseResultFile = GetTestCaseResultFullPath(testCaseName);
      // Check whether the result file exists first.
      Assert.IsTrue(File.Exists(testCaseResultFile),
                    String.Format("{0} does not exist.", testCaseResultFile));

      string[] seen = new string[0]; // in case of exception preventing seen to get value.
      string[] expected;
      string errMsg = String.Empty; 
      StreamReader stream = new StreamReader(GetTestCaseResultFullPath(testCaseName));
      expected = Normalize(DirectiveLexer.StripComments(stream)).Split('\n');
      testXFail = directives.Find(d => d.Kind == TestCaseDirective.Kinds.XFail) != null;
      try {
        Transformer transformer = new Transformer();
        Target target = transformer.Transform(source);
        seen = Normalize(target.ToString().Split('\n'));
      }
      catch (Exception e) {
        if (!testXFail)
          throw e;
      }
      finally {
        bool match = Validate(seen, expected, ref errMsg);
        if (testXFail && match) {
          errMsg += "\nUnexpected pass.";
          Assert.Fail(errMsg);
        }
        else if (testXFail) {
          errMsg += "\nExpected to fail.";
          Assert.Ignore(errMsg);
        }
        else {
          if (!match) {
            // if the test wasn't expected to fail write out the what was seen so that one can diff
            File.WriteAllLines(GetTestCaseOutFullPath(testCaseName), seen);
          }
          Assert.IsTrue(match, errMsg);
        }
      }
    }

    /// <summary>
    /// Compare whether the produced result is the expected result. Usually there is a file we want
    /// to check against. If the check fails the error is written to a string so that proper
    /// diagnostics can be produced..
    /// </summary>
    /// <param name='seen'>
    /// What the algorithm actually produced.
    /// </param>
    /// <param name='expected'>
    /// What we expect to see.
    /// </param>
    /// <param name='errMsg'>
    /// Errors if the match fails.
    /// </param>
    /// <returns>True on success.</returns>
    public bool Validate(string[] seenLines, string[] expectedLines, ref string errMsg)
    {
      string seenLine, expectedLine;
      bool match = true;
      // take the min, because we don't want out of range exception
      // the test may be marked as XFAIL
      int minLineCount = Math.Min(seenLines.Length, expectedLines.Length);
      // compare line by line
      for (int i = 0; i < minLineCount; i++) {
        seenLine = Normalize(seenLines[i]);
        expectedLine = Normalize(expectedLines[i]);
        if (seenLine != expectedLine) {
          errMsg = String.Format("Difference at line {0}: ", (i + 1).ToString());
          errMsg += String.Format("{0} != {1}", seenLine, expectedLine);
          match = false;
        }
      }
      if (seenLines.Length != expectedLines.Length) {
        match = false;
        errMsg += "More lines follow in either seen or expected.";
      }

      return match;
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
    protected MethodDefinition LoadTestCaseMethod(string testCaseName)
    {
      StreamReader stream = new StreamReader(GetTestCaseFullPath(testCaseName));
      TestCaseDirectiveParser dirParser = new TestCaseDirectiveParser(stream);
      directives = dirParser.ParseDirectives();

      AssemblyDefinition assembly = CompileTestCase(testCaseName);

      TypeDefinition type = assembly.MainModule.GetType("TestCase");
      Assert.IsNotNull (type, "Type TestCase not found!");
      MethodDefinition found = GetMethod(type.Methods, "Main");
      Assert.IsNotNull (found, "Method TestCase.Main not found!");
      return found;
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
          = Path.Combine(testCasesTmpDir, Path.GetFileNameWithoutExtension(sourceFile)+".dll");

      TestCaseDirective runDir = directives.Find(d => d.Kind == TestCaseDirective.Kinds.Run);

      string args = string.Format ("/DLL \"/OUTPUT:{0}\" {1}", testCaseAssemblyName, sourceFile);
      if (runDir != null)
        args = runDir.Arguments;

      Process p = new Process();
      p.StartInfo.Arguments = args;
      p.StartInfo.CreateNoWindow = true;
      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.RedirectStandardInput = true;
      p.StartInfo.RedirectStandardError = true;
      p.StartInfo.FileName = "ilasm";
      if (runDir != null)
        p.StartInfo.FileName = runDir.Command;
      p.Start();
      string output = p.StandardOutput.ReadToEnd();
      string error = p.StandardError.ReadToEnd();
      p.WaitForExit();
      Trace.WriteIf(p.ExitCode != 0, output + error);
      //Assert.AreEqual(0, p.ExitCode, output + error);

      return AssemblyDefinition.ReadAssembly(testCaseAssemblyName);
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
      HashSet<string> res = new HashSet<string>();
      Type ty = GetType();
      foreach(System.Reflection.MethodInfo mInfo in ty.GetMethods()) {
        if (mInfo.GetCustomAttributes(typeof(TestAttribute), true).Length == 1)
          res.Add(mInfo.Name);
      }
      return res;
    }

    protected string GetTestCaseFullPath(string testCaseName)
    {
      string result = Path.Combine(testCasesDir, testCaseName);
      result = Path.ChangeExtension(result, GetTestCaseFileExtension());
      return result;
    }

    protected string GetTestCaseResultFullPath(string testCaseName)
    {
      string result = Path.Combine(testCasesDir, testCaseName);
      result = Path.ChangeExtension(result, GetTestCaseResultFileExtension());
      return result;
    }

    protected string GetTestCaseOutFullPath(string testCaseName)
    {
      return GetTestCaseResultFullPath(testCaseName) + '.' + GetTestCaseOutFileExtension();
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
