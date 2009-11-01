/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 12.1.2009 г.
 * Time: 18:56
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;

using SolidOpt.Services.Subsystems.Configurator;
using SolidOpt.Services.Subsystems.Configurator.Sources;
using SolidOpt.Services.Subsystems.Configurator.Targets;

using SolidOpt.Services.Subsystems.Configurator.TypeResolvers;

using SolidOpt.Services.Subsystems.HetAccess;
using SolidOpt.Services.Subsystems.HetAccess.Importers;
using SolidOpt.Services.Subsystems.HetAccess.Exporters;

using SolidOpt.Services.Subsystems.Configurator.Mappers;

namespace AdvancedConfigDemo
{
	class Program
	{
		private static string resourcePath;
		public static void Main(string[] args)
		{
			resourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
			                            Path.Combine("..", Path.Combine("..", "TestResources")));
			
			
//			FromNMSP2IL();
//			FromIL2NMSP();
//			FromIL2INI();
//			FromIL2MappedINI();
//			FromMappedINI2IL();
			FromNMSP2ResolvedIL();
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		
		
		
		
		public static void FromNMSP2ResolvedIL()
		{
			ConfigurationManager<string> configurator = new ConfigurationManager<string>();
			configurator.Sources.Add(new NMSPSource<string>());
			configurator.Targets.Add(new ILTarget<string>());
			
			URIManager resManager = new URIManager();
			resManager.Importers.Add(new FileImporter());
			resManager.Exporters.Add(new FileExporter());
			
			configurator.StreamProvider = resManager;
			
			configurator.LoadConfiguration((new Uri(
				Path.Combine(resourcePath,"test.nmsp"))));
			
			TypeManager<string> typeManager = new TypeManager<string>();
			configurator.TypeManager = typeManager;
			typeManager.Resolver = new ChainResolver()
				.Add(new IntResolver())
				.Add(new FloatResolver())
				.Add(new StringResolver());
			
			configurator.SaveConfiguration(new Uri(
				Path.Combine(resourcePath,"test.modified.dll")), "dll");
			
			ViewCurrentCIR(configurator.IR,"");
			Console.WriteLine("Successfully converted from nmsp to dll!");
		}
		
		
		public static void FromMappedINI2IL()
		{
			ConfigurationManager<string> configurator = new ConfigurationManager<string>();
			configurator.Sources.Add(new INISource<string>());
			configurator.Targets.Add(new ILTarget<string>());
			
			URIManager resManager = new URIManager();
			resManager.Importers.Add(new FileImporter());
			resManager.Exporters.Add(new FileExporter());
			
			configurator.StreamProvider = resManager;
			
			MapManager<string> mapManager = new MapManager<string>();
			mapManager.Add(new INIMaper<string>());
			
			configurator.MapManager = mapManager;
			
			configurator.LoadConfiguration((new Uri(
				Path.Combine(resourcePath,"test.ini"))));
			
//			TypeManager<string> typeManager = new TypeManager<string>();
//			configurator.TypeManager = typeManager;
//			
//			configurator.SaveConfiguration(new Uri(
//				Path.Combine(resourcePath,"test.map_modified.dll")), "dll");
			
			ViewCurrentCIR(configurator.IR, "");
			Console.WriteLine("Successfully unmapped and converted from ini to IL!");
		}
		
		/// <summary>
		/// Създава от асембли мапнат ини файл, без вложени типове, т.е
		/// public class level1
		//{
		//    // Fields
		//    public static string x1;
		//
		//    // Methods
		//    static level1();
		//
		//    // Nested Types
		//    public class level2
		//    {
		//        // Fields
		//        public static string x2;
		//        public static string x4;
		//        public static string x5;
		//
		/// в
		/// level1.x1 = 
		/// level1.level2.x2
		/// ...
		/// </summary>
		public static void FromIL2MappedINI()
		{
			ConfigurationManager<string> configurator = new ConfigurationManager<string>();
			configurator.Sources.Add(new ILSource<string>());
			configurator.Targets.Add(new INITarget<string>());
			
			URIManager resManager = new URIManager();
			resManager.Importers.Add(new FileImporter());
			resManager.Exporters.Add(new FileExporter());
			
			configurator.StreamProvider = resManager;
			
			MapManager<string> mapManager = new MapManager<string>();
			mapManager.Add(new INIMaper<string>());
			
			configurator.MapManager = mapManager;
			
			configurator.LoadConfiguration((new Uri(
				Path.Combine(resourcePath,"test.map_modified.dll"))));
			configurator.SaveConfiguration(new Uri(
				Path.Combine(resourcePath,"test.ini")), "ini");
			
			ViewCurrentCIR(configurator.IR,"");
			
			ViewCurrentCIR(configurator.MapManager.MmCIR,"");
			Console.WriteLine("Successfully converted and mapped from IL to ini!");
		}
		
		/// <summary>
		/// This method is used to print the current Configuration Intermediate Representation
		/// </summary>
		public static void ViewCurrentCIR(Dictionary<string, object> dict, string tab)
		{
			string key;
			
			foreach(KeyValuePair<string, object> item in dict){
				key = (string)Convert.ChangeType(item.Key, typeof (string));
				if (item.Value is Dictionary<string, object>){
					Console.WriteLine("{0}{1} {2}", tab, key,"{");
					ViewCurrentCIR(item.Value as Dictionary<string, object>, tab + "  ");
					Console.WriteLine("{0}{1}", tab, "}");
				}
				else {
					Console.WriteLine("{0}{1} = {2}",tab, key, item.Value);
				}
			}
		}
		
		/// <summary>
		/// Loads configuration from nmsp file format and saves it to assembly (dll).
		/// </summary>
		public static void FromIL2NMSP()
		{
			ConfigurationManager<string> configurator = new ConfigurationManager<string>();
			configurator.Sources.Add(new ILSource<string>());
			configurator.Targets.Add(new NMSPTarget<string>());
			
			URIManager resManager = new URIManager();
			resManager.Importers.Add(new FileImporter());
			resManager.Exporters.Add(new FileExporter());
			
			configurator.StreamProvider = resManager;
			
			configurator.LoadConfiguration((new Uri(
				Path.Combine(resourcePath,"test.modified.dll"))));
			configurator.SaveConfiguration(new Uri(
				Path.Combine(resourcePath,"test.modified.nmsp")), "nmsp");
			
			ViewCurrentCIR(configurator.IR,"");
			Console.WriteLine("Successfully converted from dll to nmsp!");
		}
		
		/// <summary>
		/// Loads configuration from nmsp file format and saves it to assembly (dll).
		/// </summary>
		public static void FromNMSP2IL()
		{
			ConfigurationManager<string> configurator = new ConfigurationManager<string>();
			configurator.Sources.Add(new NMSPSource<string>());
			configurator.Targets.Add(new ILTarget<string>());
			
			URIManager resManager = new URIManager();
			resManager.Importers.Add(new FileImporter());
			resManager.Exporters.Add(new FileExporter());
			
			configurator.StreamProvider = resManager;
			
			configurator.LoadConfiguration((new Uri(
				Path.Combine(resourcePath,"test.nmsp"))));
			configurator.SaveConfiguration(new Uri(
				Path.Combine(resourcePath,"test.modified.dll")), "dll");
			
			ViewCurrentCIR(configurator.IR,"");
			Console.WriteLine("Successfully converted from nmsp to dll!");
		}
		
		public static void FromIL2INI()
		{
			ConfigurationManager<string> configurator = new ConfigurationManager<string>();
			configurator.Sources.Add(new ILSource<string>());
			configurator.Targets.Add(new INITarget<string>());
			
			URIManager resManager = new URIManager();
			resManager.Importers.Add(new FileImporter());
			resManager.Exporters.Add(new FileExporter());
			
			configurator.StreamProvider = resManager;
			
			configurator.LoadConfiguration((new Uri(
				Path.Combine(resourcePath,"test.modified.dll"))));
			configurator.SaveConfiguration(new Uri(
				Path.Combine(resourcePath,"test.ini")), "ini");
			
			ViewCurrentCIR(configurator.IR,"");
			Console.WriteLine("Successfully converted from IL to ini!");
		}
	}
}