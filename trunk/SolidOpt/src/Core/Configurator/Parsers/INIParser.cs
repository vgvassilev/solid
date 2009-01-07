/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 17.8.2008 ?.
 * Time: 13:40
 * 
 */

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using SolidOpt.Core.Providers.StreamProvider;

namespace SolidOpt.Core.Configurator.Parsers
{
	/// <summary>
	/// Creates IR from stream, i.e loads the configuration into the configuration manager.
	/// </summary>
	public class INIParser<TParamName> : IConfigParser<TParamName>
	{
		public INIParser()
		{
		}
		
		/// <summary>
		/// Checks if the URI can be handled.
		/// </summary>
		/// <returns>Can be handled</returns>
		public bool CanParse(Uri resUri, Stream resStream)
		{
			return resUri.IsFile && Path.GetExtension(resUri.LocalPath).ToLower() == ".ini";
		}
		
		/// <summary>
		/// Iterates over the stream delivered by the Stream Provider Manager and creates the IR.
		/// </summary>
		/// <returns>IR</returns>
		public Dictionary<TParamName, object> LoadConfiguration(Stream resStream)
		{
			ConfigINIParser<TParamName> parser = new ConfigINIParser<TParamName>(resStream);
			parser.AnalizeSyntax();
			return parser.ConfigIR;
		}
	}
	
	
	
	internal class ConfigINIParser<TParamName>{
		private char ch = ' ';
		private Lexem lexem;
		private Stream stream;
		
		private Dictionary<TParamName, object> configIR = new Dictionary<TParamName, object>();		
		public Dictionary<TParamName, object> ConfigIR {
			get { return configIR; }
			set { configIR = value; }
		}
		public ConfigINIParser()
		{
			
		}
		public ConfigINIParser(Stream stream)
		{
			this.stream = stream;	
		}
		
		public Lexem getLexem()
		{
			while(true){
				if (char.IsLetterOrDigit(ch)){
					StringBuilder sb = new StringBuilder();
					while(char.IsLetterOrDigit(ch)){
						sb.Append(ch);
						ch = (char)stream.ReadByte();
					}
					return new LexIdentifier(sb.ToString());
				}
				if (char.IsDigit(ch)){
					StringBuilder sb = new StringBuilder();
					while(char.IsDigit(ch)){
						sb.Append(ch);
						ch = (char)stream.ReadByte();
					}
					return new LexIntValue(sb.ToString());
				}
				else if (ch == '=' || ch == '[' || ch == ']'){
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
			bool flag = isConfig(out configIR);
			Console.WriteLine(flag.ToString());
			return flag;
		}
		
		public bool isConfig(out Dictionary<TParamName, object> dict)
		{
			Lexem nameLexem;
			dict = new Dictionary<TParamName, object>();
			Dictionary<TParamName, object> subDict = new Dictionary<TParamName, object>();
			bool isFirstLevel = true;
			
			while(true){
				
				nameLexem = lexem;
				
				if (lexem is LexIdentifier)
					lexem = getLexem();
				if (lexem is LexEndOfFile)
					return true;
				if (lexem is LexSpecialSymbol && (lexem as LexSpecialSymbol).Value == '=') {
					lexem = getLexem();
					if (isFirstLevel)
						dict[(TParamName)Convert.ChangeType(nameLexem.Value, typeof(TParamName))] = lexem.Value;
					else
						subDict[(TParamName)Convert.ChangeType(nameLexem.Value, typeof(TParamName))] = lexem.Value;
					
					lexem = getLexem();
				}
				else if (lexem is LexSpecialSymbol && (lexem as LexSpecialSymbol).Value == '[') {
					subDict = new Dictionary<TParamName, object>();
					
					isFirstLevel = false;
					
					lexem = getLexem();
					if (lexem is LexIdentifier) {
						
						nameLexem = lexem;
							
						lexem = getLexem();
						if (lexem is LexSpecialSymbol && (lexem as LexSpecialSymbol).Value == ']'){
							
							lexem = getLexem();
							
							dict[(TParamName)Convert.ChangeType(nameLexem.Value, typeof(TParamName))] = subDict;
						}
					}
				}
				else return false;
			}
		}
	}
	
	
}
