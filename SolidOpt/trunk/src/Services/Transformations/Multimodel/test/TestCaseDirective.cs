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
  internal class TestCaseDirective
  {
    internal enum Kinds {
      Run,
      XFail
    }

    private Kinds kind;
    public Kinds Kind {
      get { return this.kind; }
    }

    private string command;
    public string Command {
      get {
        if (kind != Kinds.Run)
          throw new InvalidOperationException("Getter should be called only if the kind is run");
        return command;
      }
      set {
        if (kind != Kinds.Run)
          throw new InvalidOperationException("Setter should be called only if the kind is run");
        command = value;
      }
    }

    private string arguments;

    public string Arguments {
      get {
        if (kind != Kinds.Run)
          throw new InvalidOperationException("Getter should be called only if the kind is run");
        return arguments;
      }
      set {
        if (kind != Kinds.Run)
          throw new InvalidOperationException("Setter should be called only if the kind is run");
        arguments = value;
      }
    }

    public TestCaseDirective(Kinds kind)
    {
      this.kind = kind;
    }
  }
  internal class TestCaseDirectiveParser {
    private StreamReader stream;
    public TestCaseDirectiveParser(StreamReader stream)
    {
      this.stream = stream;
    }

    /// <summary>
    /// Parses the directives, specified in the test case result file. For now we have only one
    /// directive: XFAIL - denoting that the test case is expected to fail.
    /// </summary>
    /// <returns>
    /// The file represented line by line with all directives stripped out.
    /// </returns>
    /// <param name='contents'>
    /// Input file contents.
    /// </param>
    /// <param name='directives'>
    /// The list of recognized directives.
    /// </param>
    internal List<TestCaseDirective> ParseDirectives()
    {
      List<TestCaseDirective> directives = new List<TestCaseDirective>(1);
      DirectiveLexer dirLexer = new DirectiveLexer(stream);
      Token tok;
      while (true) {
        tok = dirLexer.Lex();
        if (tok.Kind == Token.Kinds.EOF)
          break;
        if (tok.Kind == Token.Kinds.Comment) {
          tok = dirLexer.Lex();
          if (tok.Kind == Token.Kinds.Ident) {
            TestCaseDirective dir = null;
            if (tok.IdentName == "XFAIL")
              dir = ParseXfailDirective();
            else if (tok.IdentName == "RUN")
              dir = ParseRunDirective(ref dirLexer);

            if (dir != null)
              directives.Add(dir);
          }
        }
      }
      return directives;
    }

    internal TestCaseDirective ParseXfailDirective()
    {
      return new TestCaseDirective(TestCaseDirective.Kinds.XFail);
    }

    internal TestCaseDirective ParseRunDirective(ref DirectiveLexer lexer)
    {
      Token tok = lexer.Lex();
      if (tok.Kind != Token.Kinds.Colon)
        return null;
      tok = lexer.Lex();
      if (tok.Kind != Token.Kinds.Quote)
        return null;

      TestCaseDirective runDir = new TestCaseDirective(TestCaseDirective.Kinds.Run);

      string command = lexer.LexUntil('"');
      runDir.Command = command;

      string arguments = lexer.LexUntil('\n'); // Practically EOF in our case.
      runDir.Arguments = arguments;

      return runDir;
    }

  }

  internal struct Token {
    internal enum Kinds {
      Colon,
      Comment,
      Ident,
      Quote,
      Unknown,
      EOF
    };

    private Kinds kind;
    public Kinds Kind {
      get { return kind; }
    }

    private string identName;
    public string IdentName {
      get {
        if (kind != Kinds.Ident)
          throw new InvalidOperationException("Getter should be called only if the kind is ident");
        return identName;
      }
      set {
        if (kind != Kinds.Ident)
          throw new InvalidOperationException("Setter should be called only if the kind is ident");
        identName = value;
      }
    }

    public Token(Token.Kinds kind)
    {
      this.identName = null;
      this.kind = kind;
    }
  }

  internal class DirectiveLexer {
    private char curPos;
    private StreamReader stream;

    public DirectiveLexer(StreamReader stream) {
      this.stream = stream;
      this.curPos = (char)stream.Read();
    }

    public Token Lex()
    {
      if (stream.EndOfStream)
        return new Token(Token.Kinds.EOF);

      while (Char.IsWhiteSpace(curPos))
        curPos = (char) stream.Read();

      if (curPos == ':') {
        curPos = (char)stream.Read();
        return new Token(Token.Kinds.Colon);
      }

      if (curPos == '"') {
        curPos = (char)stream.Read();
        return new Token(Token.Kinds.Quote);
      }

      if (curPos == '/' && ((char)stream.Peek() == '/')) {
        stream.Read(); // consume the second /
        curPos = (char) stream.Read(); // read next

        Token tok = new Token(Token.Kinds.Comment);
        return tok;
      }

      StringBuilder identName = new StringBuilder();
      while (Char.IsLetter(curPos)) {
        identName.Append(curPos);
        curPos = (char) stream.Read();
      }
      if (identName.Length > 0) {
        Token tok = new Token(Token.Kinds.Ident);
        tok.IdentName = identName.ToString();
        return tok;
      }

      curPos = (char)stream.Read();
      return new Token(Token.Kinds.Unknown);
    }

    public string LexUntil(char stopAt)
    {
      StringBuilder sb = new StringBuilder();
      while (curPos != stopAt || !stream.EndOfStream)
        sb.Append(curPos++);
      return sb.ToString();
    }

    public static string StripComments(StreamReader reader) {
      StringBuilder sb = new StringBuilder();
      char curPos;
      while (true) {
        curPos = (char)reader.Read();
        if (curPos == '/' && ((char)reader.Peek()) == '/') {
          reader.Read(); // consume the /
          curPos = (char)reader.Read();
          while(curPos != '\n' && !reader.EndOfStream)
            curPos = (char) reader.Read();
          curPos = (char)reader.Read();
        }
        sb.Append(curPos);
        if (reader.EndOfStream) { 
          break;
        }
      }
      return sb.ToString();
    }
  }
}
