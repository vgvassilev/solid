// /*
//  * $Id: $
//  * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
//  * For further details see the nearest License.txt
//  */

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
      while (curPos != stopAt && !stream.EndOfStream) {
        sb.Append(curPos);
        curPos = (char) stream.Read();
      }
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

}
