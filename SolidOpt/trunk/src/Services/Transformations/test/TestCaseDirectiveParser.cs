/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SolidOpt.Services.Transformations.Multimodel.Test
{

  /// <summary>
  /// Parser for our test case directives.
  /// </summary>
  internal class TestCaseDirectiveParser {
    /// <summary>
    /// The lexer used by the parser to analyse.
    /// </summary>
    private DirectiveLexer lexer;

    /// <summary>
    /// The filename we are currently parsing.
    /// </summary>
    private string filename;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="SolidOpt.Services.Transformations.Multimodel.Test.TestCaseDirectiveParser"/> class.
    /// </summary>
    /// <param name='filename'>
    /// The file containing the test case being analyzed.
    /// </param>
    public TestCaseDirectiveParser(string filename)
    {
      this.filename = filename;
      StreamReader stream = new StreamReader(filename);
      this.lexer = new DirectiveLexer(stream);
    }

    /// <summary>
    /// Parses the directives, specified in the test case result file.
    /// </summary>
    /// <returns>
    /// List of directives found.
    /// </returns>
    internal List<TestCaseDirective> ParseDirectives()
    {
      List<TestCaseDirective> directives = new List<TestCaseDirective>(1);
      Token tok;
      while (true) {
        tok = lexer.Lex();
        if (tok.Kind == Token.Kinds.EOF)
          break;
        if (tok.Kind == Token.Kinds.Comment) {
          tok = lexer.Lex();
          if (tok.Kind == Token.Kinds.Ident) {
            TestCaseDirective dir = null;
            if (tok.IdentName == "XFAIL")
              dir = ParseXfailDirective();
            else if (tok.IdentName == "RUN")
              dir = ParseRunDirective();

            if (dir != null)
              directives.Add(dir);
          }
        }
      }
      return directives;
    }

    /// <summary>
    /// Parses the xfail directive. It is used to denote that the test case is expected to fail.
    /// </summary>
    /// XFAIL has the syntax:
    /// <example>//XFAIL:</example>
    /// <returns>
    /// The concrete xfail directive.
    /// </returns>
    internal TestCaseDirective ParseXfailDirective()
    {
      Token tok = lexer.Lex();
      if (tok.Kind != Token.Kinds.Colon)
        return null;

      return new TestCaseDirective(TestCaseDirective.Kinds.XFail);
    }

    /// <summary>
    /// Parses the run directive. It is used to a command to be run in the test case.
    /// </summary>
    /// RUN has the syntax:
    /// <example>//RUN: "command" arguments</example>
    /// <returns>
    /// The concrete run directive.
    /// </returns>
    internal TestCaseDirective ParseRunDirective()
    {
      Token tok = lexer.Lex();
      if (tok.Kind != Token.Kinds.Colon)
        return null;
      tok = lexer.Lex();
      if (tok.Kind != Token.Kinds.Quote)
        return null;

      TestCaseDirective runDir = new TestCaseDirective(TestCaseDirective.Kinds.Run);

      string command = lexer.LexUntil('"');
      lexer.Lex(); // Lex the closing quote.

      // Expand the command if known:
      if (command[0] == '@') {
        if (command == "@ILASM@")
          command = BuildInformation.BuildInfo.ILASMCompiler;
        else if (command == "@CSC@")
          command = BuildInformation.BuildInfo.CSCCompiler;
      }

      runDir.Command = command;

      string arguments = lexer.LexUntil('\n'); // Practically EOF in our case.
      // Expand arguments if known:
      arguments = arguments.Replace("@TEST_CASE@", filename);
      arguments = arguments.Replace("@CMAKE_LIBRARY_OUTPUT_DIR@", BuildInformation.BuildInfo.LibraryOutputDir);
      runDir.Arguments = arguments.TrimEnd('\r');

      return runDir;
    }
  }
}
