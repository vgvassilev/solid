/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 23.12.2008 г.
 * Time: 22:08
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;

using SolidOpt.Core.Configurator;
using SolidOpt.Core.Configurator.Parsers;
using SolidOpt.Core.Configurator.Builders;

namespace NMSPFileFormat
{
	/// <summary>
	/// The example demonstrates the new config file format, which is hierarchical. Here is an example:
	/// a1{
	///		x1=5
	///		b{
	///			x2=13
	///			x5=asd
	///			x4=11
	///			c{
	///				x=2
	///				d{
	///					asd=10
	///				}
	///			}
	/// 	}
	/// }
	/// x=14
	/// y=opa
	/// 
	/// Analogically can be added and metadata into the format if needed. Here is an example:
	/// types{
	///		a{
	///			x1=int
	///			b{
	///				x2=int
	///				x5=string
	///				x4=class(object)
	///				c{
	///					x=structure(PointF)
	///					d{
	///						asd=10
	///					}
	///				}
	///			}
	///		}
	///	}
	/// </summary>
	class Program
	{
		public static void Main(string[] args)
		{

			ConfigurationManager<string> configurator = new ConfigurationManager<string>();
			
//			//Adding Loaders (Parsers) to the config manager
//			NMSPParser<string> nmspParser = new NMSPParser<string>();
//			configurator.Loaders.Add(nmspParser);
//			
//			//Adding Savers (Builders) to the config manager
//			NMSPBuilder<string> nmspBuilder = new NMSPBuilder<string>();
//			configurator.Savers.Add(nmspBuilder);
//			
//			
//			Dictionary<string, object> testDict  = configurator.LoadConfiguration(new Uri(
//				Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"test.nmsp")));
//			
//			ViewIR(testDict);
//			
//			configurator.SaveConfiguration(testDict, new Uri(Path.Combine(
//				AppDomain.CurrentDomain.BaseDirectory,"test.modified.nmsp")), "nmsp");
			
			
//			//Adding Loaders (Parsers) to the config manager
//			NMSPParser<string> nmspParser = new NMSPParser<string>();
//			configurator.Loaders.Add(nmspParser);
//			
//			//Adding Savers (Builders) to the config manager
//			INIBuilder<string> iniBuilder = new INIBuilder<string>();
//			configurator.Savers.Add(iniBuilder);
//			
//			
//			Dictionary<string, object> testDict  = configurator.LoadConfiguration(new Uri(
//				Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"test.nmsp")));
//			
//			ViewIR(testDict);
//			
//			configurator.SaveConfiguration(testDict, new Uri(Path.Combine(
//				AppDomain.CurrentDomain.BaseDirectory,"test.modified.ini")), "ini");
//			
//			configurator.Loaders.Remove(nmspParser);
//			configurator.Loaders.Add(new INIParser<string>());
//			
//			testDict  = configurator.LoadConfiguration(new Uri(
//				Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"test.modified.ini")));
//			
//			ViewIR(testDict);
			
			//Adding Loaders (Parsers) to the config manager
			NMSPParser<string> nmspParser = new NMSPParser<string>();
			configurator.Loaders.Add(nmspParser);
			
			//Adding Savers (Builders) to the config manager
			ILBuilder<string> ilBuilder = new ILBuilder<string>();
			configurator.Savers.Add(ilBuilder);
			
			
			Dictionary<string, object> testDict  = configurator.LoadConfiguration(new Uri(
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"test.nmsp")));
			
			ViewIR(testDict);
			
			configurator.SaveConfiguration(testDict, new Uri(Path.Combine(
				AppDomain.CurrentDomain.BaseDirectory,"test.modified.dll")), "dll");

			configurator.Loaders.Remove(nmspParser);
			configurator.Loaders.Add(new ILParser<string>());
			
			ViewIR(configurator.LoadConfiguration(new Uri(
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"test.modified.dll"))));
			
			
			
			
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		internal static void ViewIR(Dictionary<string, object> dict)
		{
			ViewIR(dict, "");
		}
		
		internal static void ViewIR(Dictionary<string, object> dict, string tab)
		{
			string key;
			
			foreach(KeyValuePair<string, object> item in dict){
				key = (string)Convert.ChangeType(item.Key, typeof (string));
				if (item.Value is Dictionary<string, object>){
					Console.WriteLine("{0}{1} {2}", tab, key,"{");
					ViewIR(item.Value as Dictionary<string, object>, tab + "  ");
					Console.WriteLine("{0}{1}", tab, "}");
				}
				else {
					Console.WriteLine("{0}{1} = {2}",tab, key, item.Value);
				}
			}
		}
	}
}
