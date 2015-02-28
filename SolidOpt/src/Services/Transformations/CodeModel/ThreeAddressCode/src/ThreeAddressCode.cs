/*
 * $Id: $
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Text;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SolidOpt.Services.Transformations.CodeModel.ThreeAddressCode {

    public class ThreeAddressCode
    {
        private Triplet root;
        public Triplet Root {
            get { return root; }
        }
        
        private MethodDefinition method;
        public MethodDefinition Method {
            get { return method; }
        }
        
        private List<Triplet> rawTriplets;
        public List<Triplet> RawTriplets {
            get { return rawTriplets; }
        }

        private List<VariableDefinition> temporaryVariables;
        public List<VariableDefinition> TemporaryVariables {
            get { return temporaryVariables; }
        }

        public ThreeAddressCode(MethodDefinition method, Triplet root, List<Triplet> rawTriplets, List<VariableDefinition> temporaryVariables)
        {
            this.method = method;
            this.root = root;
            this.rawTriplets = rawTriplets;
            this.temporaryVariables = temporaryVariables;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} ", method.ReturnType.ToString());
            sb.AppendFormat("{0}::{1}(", method.DeclaringType.ToString(), method.Name);
            ParameterDefinition paramDef;
            for(int i = 0; i < method.Parameters.Count; i++) {
              paramDef = method.Parameters[i];
              sb.AppendFormat("{0} {1}{2}", paramDef.ParameterType.ToString(), paramDef.Name,
                              (i < method.Parameters.Count-1) ? ", " : "");
            }
            sb.AppendLine(") {");

            int index = 0;
            foreach (Triplet triplet in RawTriplets) {
                sb.AppendLine(String.Format("  L{0}: {1}", index++, triplet.ToString()));
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        public static ThreeAddressCode FromString(string code, ref StringBuilder errors)
        {
            // parse the string and return...
            TACParser parser = new TACParser(code, ref errors);
            return parser.Parse();
        }

    private struct Token {
      public enum TokenKind {
        Unknown,
        Ident,
        Digit,
        Semicolon,
        LBrace,
        RBrace,
        Equals,
        Dot,
        Colon,
        EOF
      }
      public TokenKind kind;
      public string data;
      public Token() {
        kind = TokenKind.Unknown;
        data = "";
      }
    }

    private class Lexer {
      private string code;
      private int curPos = 0;

      public Lexer(string code) {
        this.code = code;
      }

      public Token Lex() {
        Token Tok = new Token();
        if (curPos == code.Length - 1) {
          Tok.kind =  Token.TokenKind.EOF;
          return Tok;
        }

        if (code[curPos] == ' ' || code[curPos] == '\t' || code[curPos] == '\n')
          while (code[curPos] == ' ' || code[curPos] == '\t' || code[curPos] == '\n')
            ++curPos;


        switch (code[curPos]) {
          case '0': case '1': case '2': case '3': case '4': case '5':
          case '6': case '7': case '8': case '9':
            LexNumericConstant(ref Tok);
            return Tok;

          case 'A': case 'B': case 'C': case 'D': case 'E': case 'F': case 'G': case 'H': case 'I':
          case 'J': case 'K': case 'L': case 'M': case 'N': case 'O': case 'P': case 'Q': case 'R':
          case 'S': case 'T': case 'U': case 'V': case 'W': case 'X': case 'Y': case 'Z':
          case 'a': case 'b': case 'c': case 'd': case 'e': case 'f': case 'g': case 'h': case 'i':
          case 'j': case 'k': case 'l': case 'm': case 'n': case 'o': case 'p': case 'q': case 'r':
          case 's': case 't': case 'u': case 'v': case 'w': case 'x': case 'y': case 'z':
          case '_':
            LexIdentifier(ref Tok);
            return Tok;
          case '=':
            Tok.kind = Token.TokenKind.Equals;
            ++curPos;
            return Tok;
          case '{':
            Tok.kind = Token.TokenKind.LBrace;
            ++curPos;
            break;
          case '}':
            Tok.kind = Token.TokenKind.RBrace;
            ++curPos;
            break;
          case '.':
            Tok.kind = Token.TokenKind.Dot;
            ++curPos;
            break;
          case ':':
            Tok.kind = Token.TokenKind.Colon;
            ++curPos;
            break;
          default:
            Tok.kind = Token.TokenKind.Unknown;
            ++curPos;
            break;

        }
        return Tok;
      }

      private void LexNumericConstant(ref Token Tok) {
        int prevPos = curPos++;
        char ch = code[curPos];
        while(ch >= '0' && ch <= '9')
          ch = code[++curPos];

        Tok.kind = Token.TokenKind.Digit;
        Tok.data = code.Substring(prevPos, curPos - prevPos);
      }

      private void LexIdentifier(ref Token Tok) {
        int prevPos = curPos;
        char ch = code[curPos];
        while(ch >= 'A' && ch <= 'z' || ch >= '0' && ch <='9')
          ch = code[++curPos];

        Tok.kind = Token.TokenKind.Ident;
        Tok.data = code.Substring(prevPos, curPos - prevPos);
      }

    }

    private class TACParser {

      List<Token> tokenStream = new List<Token>();

      Token Tok() {
        return tokenStream[0];
      }

      Lexer lexer;
      StringBuilder errors;

      public TACParser(string code, ref StringBuilder errors) {
        lexer = new Lexer(code);
        this.errors = errors;
      }

      public ThreeAddressCode Parse() {
        List<Triplet> triplets = new List<Triplet>();
        // Warm up the parser.
        consumeToken();

        IsMethod(ref triplets);
        ThreeAddressCode tac = new ThreeAddressCode(/*methoddef*/null, triplets[0], triplets, null);
        return tac;
      }

      bool IsMethod(ref List<Triplet> triplets) {
        // TODO: Check the Method body signature
        SkipUntil(Token.TokenKind.LBrace);

        return IsMethodBody(ref triplets);
      }

      // '{' {triplet} '}'
      bool IsMethodBody(ref List<Triplet> triplets) {
        if (Tok().kind != Token.TokenKind.LBrace) {
          errors.AppendLine("MethodBody must start with {");
          return false;
        }
        consumeToken();

        Triplet triplet;
        while (Tok().kind != Token.TokenKind.RBrace && IsTriplet(out triplet)) {
          triplets.Add(triplet);
          // consume the ;
          consumeToken();
        }
          

        if (Tok().kind != Token.TokenKind.RBrace) {
          errors.AppendLine("MethodBody must end with }");
          return false;
        }

        return true;
      }

      /// <summary>
      /// The triplets have the form
      /// triplet := [label] ident operator ident [operator] [ident]
      /// </summary>
      /// <returns><c>true</c> if this instance is triplet; otherwise, <c>false</c>.</returns>
      bool IsTriplet(out Triplet triplet) {
        triplet = null;
        if (Tok().kind != Token.TokenKind.Ident) {
          errors.AppendLine("Triplet must begin with an ident.");
          return false;
        }


          
        Token nextTok = lookAhead();

        //FIXME: Skip the label for now: L1:
        if (nextTok.kind == Token.TokenKind.Colon) {
          consumeToken();
          consumeToken();
          nextTok = lookAhead();
        }

        if (Tok().data == "return")
          IsReturnTriplet(out triplet);
        else if (nextTok.kind == Token.TokenKind.Equals)
          IsAssignmentTriplet(out triplet);
          
        consumeToken();
        if (Tok().kind == Token.TokenKind.Semicolon) {
          errors.AppendLine("; expected");
          return false;
        }
        return true;

      }

      /// <summary>
      /// return ident | digit
      /// </summary>
      /// <returns><c>true</c> if this instance is return triplet the specified triplet; otherwise, <c>false</c>.</returns>
      /// <param name="triplet">Triplet.</param>
      bool IsReturnTriplet(out Triplet triplet) {
        triplet = new Triplet(TripletOpCode.Return);

        consumeToken();
        // Support only return ident
        if (Tok().kind == Token.TokenKind.Ident) {
          triplet.Operand1 = new VariableDefinition(Tok().data, /*TypeReference*/null);
          return true;
        }
        return false;

      }

      /// <summary>
      /// result = operand1 [operator] [operand2]
      /// </summary>
      /// <returns><c>true</c> if this instance is assignment triplet the specified triplet; otherwise, <c>false</c>.</returns>
      /// <param name="triplet">Triplet.</param>
      bool IsAssignmentTriplet(out Triplet triplet) {
        triplet = new Triplet (TripletOpCode.Assignment);
        triplet.Result = new VariableDefinition(Tok().data, /*TypeReference*/null);
        consumeToken();
        // consume the =
        consumeToken();
        if (Tok().kind != Token.TokenKind.Ident && Tok().kind != Token.TokenKind.Digit) {
          errors.AppendLine("identifier or digit expected");
          return false;
        }
        if (Tok().kind == Token.TokenKind.Ident)
          triplet.Operand1 = new VariableDefinition(Tok().data, /*TypeReference*/null);
        else if (Tok().kind == Token.TokenKind.Digit)
          triplet.Operand1 = Int64.Parse(Tok().data);
        return true;
      }

      bool IsOperator(Token Tok) {
        if (Tok.kind == Token.TokenKind.Equals)
          return true;
        return false;
      }


      void consumeToken() {
        if (tokenStream.Count > 0)
          tokenStream.RemoveAt(0);

        lookAhead();
      }

      Token lookAhead() {
        if (tokenStream.Count > 1)
          return tokenStream[1];

        Token tok = lexer.Lex();
        tokenStream.Add(tok);
        return tok;
      }

      void SkipUntil(Token.TokenKind kind) {
        while (Tok().kind != Token.TokenKind.EOF && Tok().kind != kind)
          consumeToken();
      }
    }
  }
}

