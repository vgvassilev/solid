﻿/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 02.1.2009 г.
 * Time: 19:23
 * 
 */
 
using System;
using System.Collections.Generic;
using System.IO;

using Mono.Cecil;
using Mono.Cecil.Cil;

using SolidOpt.Core.Providers.StreamProvider;

using System.Reflection;
using System.Diagnostics;

namespace SolidOpt.Core.Configurator.Targets
{
	/// <summary>
	/// This class is used to build assemblies from the Config IR (CIR). It uses Mono.Cecil to 
	/// manage the assembly. An assembly is defined, traversed and saved. For assembly iterations 
	/// is used helper class - Structure Visitor.
	/// </summary>
	public class ILTarget<TParamName> : IConfigTarget<TParamName>
	{
		private StreamWriter streamWriter = null;
		public static Dictionary<TParamName, object> dict;
		
		public ILTarget()
		{
		}
		
		public bool CanBuild(string fileFormat)
		{
			return fileFormat == "dll";
		}
		
		public Stream Build(Dictionary<TParamName, object> configRepresenation)
		{
			streamWriter = new StreamWriter(new MemoryStream());
			dict = configRepresenation;
			
			AssemblyDefinition assembly =
					AssemblyFactory.DefineAssembly("ConfigAssembly", AssemblyKind.Dll);

			assembly.Name.Name ="Config Assembly";
			assembly.Name.Version = new Version(1,0,0);
			assembly.MainModule.Accept(new StructureVisitor<TParamName>());
			
            // Save the assembly and verify the result
            byte[] asm;
            AssemblyFactory.SaveAssembly(assembly, out asm);
            
            streamWriter.BaseStream.Write(asm, 0, asm.Length);
            
            return streamWriter.BaseStream;
//			uriManager.SetResource(streamWriter.BaseStream, resourse);
//			
//			streamWriter.Close();
		}
	}
	
	#region Visitors
	
	/// <summary>
	/// The class creates module (and namespace) in the assembly named "Config" and class for global 
	/// variables named "Global".
	/// Using recursion the CIR is emitted into the assembly. The values are represented as fields with initialization.
	/// Example:
	/// public static readonly int count = 5;
	/// First is defined the definition of the field (count) then is defined static 
	/// consturctor to initialize the value (5)
	/// </summary>
	/// 
//	TODO: Да се тества кое е по-оптималното решение. Да се използва .ToString() метода или да се направи оптимизираща
//	структура от сорта на:
//	internal string GetParamName<string>(string Param)
//	{
//		return Param;
	//	}
//	internal string GetParamName<TParanName>(TParamName Param) 
//	{
//		return Param.ToString();
//	}
	
	
	internal class StructureVisitor<TParamName>: BaseStructureVisitor
		{
			public override void VisitModuleDefinition(ModuleDefinition module)
			{
				if (ILTarget<TParamName>.dict.Count > 0){
					TypeDefinition type = new TypeDefinition("Global", "Config",
				                                         Mono.Cecil.TypeAttributes.Public, null);
					module.Types.Add(type);
					FillConfigValues(ILTarget<TParamName>.dict, module, type);
					
					FillConfigTypes(ILTarget<TParamName>.dict, module, null);
				}
			}
			
			internal void FillConfigValues(Dictionary<TParamName, object> dict, ModuleDefinition module, TypeDefinition baseType)
			{
				TParamName key;
				FieldDefinition field;
				MethodDefinition cctor = null;
				
				foreach(KeyValuePair<TParamName, object> item in dict){
					if (!(item.Value is Dictionary<TParamName, object>)) {
						key = (TParamName)Convert.ChangeType(item.Key, typeof (TParamName));
							
						field = new FieldDefinition(key.ToString(), module.Import(item.Value.GetType()),
						        Mono.Cecil.FieldAttributes.Static | Mono.Cecil.FieldAttributes.Public);
						baseType.Fields.Add(field);
						
						if (cctor == null){
							TypeReference returnType = new TypeReference("System.Void","", null, false);
				
							cctor = new MethodDefinition(".cctor", 
				                                              Mono.Cecil.MethodAttributes.Private |
				                                              Mono.Cecil.MethodAttributes.HideBySig |
				                                              Mono.Cecil.MethodAttributes.SpecialName |
				                                              Mono.Cecil.MethodAttributes.RTSpecialName |
				                                              Mono.Cecil.MethodAttributes.Static,
					                                          returnType);
							baseType.Constructors.Add(cctor);
						}
					
						AppendToCCtor(cctor, field, item.Value);
					}
				}
				
				if (cctor != null)
					cctor.Body.CilWorker.Append(cctor.Body.CilWorker.Create(OpCodes.Ret));
			}
			
			internal void FillConfigTypes(Dictionary<TParamName, object> dict, ModuleDefinition module, TypeDefinition baseType)
			{
				TParamName key;
				TypeDefinition type;
				
				if (baseType != null)
					FillConfigValues(dict, module, baseType);
				
				foreach(KeyValuePair<TParamName, object> item in dict){
					if (item.Value is Dictionary<TParamName, object>){
						
						key = (TParamName)Convert.ChangeType(item.Key, typeof (TParamName));
						
						if (baseType != null) {
							type = new TypeDefinition(key.ToString(), "Config", 
						                          Mono.Cecil.TypeAttributes.NestedPublic, null);
							baseType.NestedTypes.Add(type);
						}
						else {
							type = new TypeDefinition(key.ToString(), "Config", 
						                          Mono.Cecil.TypeAttributes.Public, null);
						}
						
						module.Types.Add(type);
						
						FillConfigTypes(item.Value as Dictionary<TParamName, object>, module, type);
					}
				}
			}
			
			private void AppendToCCtor(MethodDefinition cctor, FieldDefinition field, object currentValue)
			{
				CilWorker cil = cctor.Body.CilWorker;
				
				Instruction instr = null; 
				
				switch (field.FieldType.GetOriginalType().FullName) {
						case "System.String" : 
							instr = cil.Create(OpCodes.Ldstr, (String)currentValue);
							break;
						
						case "System.Int64" : 
							instr = cil.Create(OpCodes.Ldc_I8, (Int64)currentValue);
							break;
						
				}
				if (instr != null)
					cil.Append(instr);
				instr = cil.Create(OpCodes.Stsfld, field);
				cil.Append(instr);
			}
			
		}
	#endregion
	
}