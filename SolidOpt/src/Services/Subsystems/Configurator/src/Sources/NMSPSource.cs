/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using SolidOpt.Services.Subsystems.HetAccess;

namespace SolidOpt.Services.Subsystems.Configurator.Sources
{
  /// <summary>
  /// Generates intermediate represenation (Dictionary) from the NMSP file format.
  /// The example demonstrates the new config file format, which is hierarchical. Here is an example:
  /// <example>
  /// a1{
  ///    x1=5
  ///    b{
  ///      x2=13
  ///      x5=asd
  ///      x4=11
  ///      c{
  ///        x=2
  ///        d{
  ///          asd=10
  ///        }
  ///      }
  ///   }
  /// }
  /// x=14
  /// y=opa
  /// </example>
  /// NMSP file format has very simple and useful grammar:
  /// NMSP = [namespace | {param}]
  /// namespace = ident '{' [{namespace | param}] '}'
  /// param = ident '=' value '\n'
  /// ident = letter {letter | digit}
  /// value = {letter | digit}
  /// </summary>
  //FIXME: Fix the grammar and the parser...
  public class NMSPSource<TParamName> : IConfigSource<TParamName>
  {
    public NMSPSource()
    {
    }
    
    public bool CanParse(Uri resUri, Stream resStream)
    {
      return resUri.IsFile && Path.GetExtension(resUri.LocalPath).ToLower() == ".nmsp";
    }
    
    /// <summary>
    /// Iterates over the stream delivered by the Stream Provider Manager and creates the IR.
    /// </summary>
    /// <returns>IR</returns>
    public Dictionary<TParamName, object> LoadConfiguration(Stream resStream)
    {
      ConfigNMSPParser<TParamName> parser = new ConfigNMSPParser<TParamName>(resStream);
      parser.AnalizeSyntax();
      return parser.ConfigIR;
    }
  }

  #region Lexem Hierarchy
  
    /// <summary>
    /// Lexem hierarchy describing the type of the lexems.
    /// </summary>
    internal class Lexem{
      protected object value;    
      public object Value {
        get { return value; }
        set { this.value = value; }
      }
      public Lexem(){
      }
      public Lexem(object value)
      {
        this.value = value;
      }
    }
    internal class LexIdentifier : Lexem{
      public LexIdentifier(object value) : base(value){}
        
    }
    internal class LexSpecialSymbol : Lexem{
      public LexSpecialSymbol(object value) : base(value){
        
      }
      public new char Value {
        get { return (char)value; }
        set { this.value = value; }
      }
    }
    internal class LexValue : Lexem{
      public LexValue(){}
      public LexValue(object value) : base(value){}
    }
    internal class LexIntValue : LexValue{
      public LexIntValue(object value) : base(value)
      {
        
      }
      public new int Value {
        get { return (int)value; }
        set { this.value = value; }
      }
    }
    internal class LexEndOfFile : Lexem{
      public LexEndOfFile() : base(){
      }
    }
    internal class LexUnknownSymbol : LexSpecialSymbol{
      public LexUnknownSymbol(object value) : base(value){
        
      }
    }
  #endregion  
  
  
  internal class ConfigNMSPParser<TParamName>
  {
    private bool isValue = false;
    private char ch = ' ';
    private Lexem lexem;
    private Stream stream;
    
    private Dictionary<TParamName, object> configIR = new Dictionary<TParamName, object>();    
    public Dictionary<TParamName, object> ConfigIR {
      get { return configIR; }
      set { configIR = value; }
    }
    public ConfigNMSPParser()
    {
      
    }
    public ConfigNMSPParser(Stream stream)
    {
      this.stream = stream;  
    }
    
    public Lexem getLexem()
    {
      while(true){
        if (isValue) {
          StringBuilder sb = new StringBuilder();
          while(ch != '\n' && ch != '\r'){
            sb.Append(ch);
            ch = (char)stream.ReadByte();
          }
          isValue = false;
          return new LexValue(sb.ToString());
        }
        else if (char.IsLetterOrDigit(ch)){
          StringBuilder sb = new StringBuilder();
          while(char.IsLetterOrDigit(ch)){
            sb.Append(ch);
            ch = (char)stream.ReadByte();
          }
          return new LexIdentifier(sb.ToString());
        }
//        else if (char.IsDigit(ch)){
//          StringBuilder sb = new StringBuilder();
//          while(char.IsDigit(ch)){
//            sb.Append(ch);
//            ch = (char)stream.ReadByte();
//            if (ch == '.') {
//              sb.Append('.');
//              ch = (char)stream.ReadByte();
//              while(char.IsDigit(ch)) {
//                sb.Append(ch);
//                ch = (char)stream.ReadByte();
//              }
//            }
//          }
////          Console.WriteLine("Number" + sb.ToString());
//          return new LexIntValue(sb.ToString());
//        }
        else if (ch == '=' || ch == '{' || ch == '}'){
          if (ch == '=') {
            isValue = true;
          }
          char ch1 = ch;
          try{
            ch = (char)stream.ReadByte();
          }
          catch{
            ch = Char.MaxValue;
          }
          return new LexSpecialSymbol(ch1);
        }
        else if (char.IsWhiteSpace(ch)){
          try{
            ch = (char)stream.ReadByte();
          }
          catch{
            ch = Char.MaxValue;
          }
        }
        else if (ch == char.MaxValue){
          return new LexEndOfFile();
        }
        else{
          char ch1 = ch;
          ch = (char)stream.ReadByte();
          return new LexUnknownSymbol(ch1);
        }
      }
    }
    
    public bool AnalizeSyntax()
    {
      lexem = getLexem();
      
      return isConfig(out configIR);
    }
    
    public bool isConfig(out Dictionary<TParamName, object> dict)
    {
      Lexem nameLexem;
      dict = new Dictionary<TParamName, object>();
      
      while(true){
        if (!(lexem is LexIdentifier)) return false;
        
        nameLexem = lexem;
        lexem = getLexem();
        
        if (lexem is LexSpecialSymbol && (lexem as LexSpecialSymbol).Value == '=') {
          lexem = getLexem();
        
          dict[(TParamName)Convert.ChangeType(nameLexem.Value, typeof(TParamName))] = lexem.Value;
          
          lexem = getLexem();
          
        }
        else if (lexem is LexSpecialSymbol && (lexem as LexSpecialSymbol).Value == '{') {
          lexem = getLexem();
          
          Dictionary<TParamName, object> subDict;
          isConfig(out subDict);
          
          dict[(TParamName)Convert.ChangeType(nameLexem.Value, typeof(TParamName))] = subDict;
          
          if (!(lexem is LexSpecialSymbol && (lexem as LexSpecialSymbol).Value == '}'))
              return false;
          lexem = getLexem();
        }
        else return false;
      }
    }
  }
}
