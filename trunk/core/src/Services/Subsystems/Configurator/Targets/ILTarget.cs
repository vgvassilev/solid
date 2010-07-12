/*
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
using Mono.Collections.Generic;

using SolidOpt.Services.Subsystems.HetAccess;

using System.Reflection;
using System.Diagnostics;

namespace SolidOpt.Services.Subsystems.Configurator.Targets
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
			
			//Mono 0.9 migration: AssemblyDefinition assembly = AssemblyFactory.DefineAssembly("Config", AssemblyKind.Dll);
			//Mono 0.9 migration: assembly.Name.Name = "Config";
			//Mono 0.9 migration: assembly.Name.Version = new Version(1,0,0,0);
			AssemblyDefinition assembly = AssemblyDefinition.CreateAssembly(
				new AssemblyNameDefinition("Config", new Version(1,0,0,0)),
				"Config",
				ModuleKind.Dll);
			
			//Mono 0.9 migration: assembly.MainModule.Accept(new StructureVisitor<TParamName>());
			VisitModuleDefinition(assembly.MainModule);
			
            // Save the assembly and verify the result
            //Mono 0.9 migration: byte[] asm;
            //Mono 0.9 migration: AssemblyFactory.SaveAssembly(assembly, out asm);
            assembly.Write(streamWriter.BaseStream);
            
            return streamWriter.BaseStream;
//			uriManager.SetResource(streamWriter.BaseStream, resourse);
//			
//			streamWriter.Close();
		}
		
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
		internal void VisitModuleDefinition(ModuleDefinition module)
		{
			if (ILTarget<TParamName>.dict.Count > 0){
				TypeDefinition type = new TypeDefinition("Global", "Config",
				                                         Mono.Cecil.TypeAttributes.Public,
				                                         module.Import(typeof(object)));
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
						
					field = new FieldDefinition(key.ToString(), Mono.Cecil.FieldAttributes.Static | Mono.Cecil.FieldAttributes.Public, module.Import(item.Value.GetType()));
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
						baseType.Methods.Add(cctor);
						baseType.Methods.Add(InsertObjectCtor(module));
					}
				
					AppendToCCtor(cctor, field, item.Value);
				}
			}
			
			//Mono 0.9 migration: if (cctor != null)
			//Mono 0.9 migration: cctor.Body.CilWorker.Append(cctor.Body.CilWorker.Create(OpCodes.Ret));
			if (cctor != null) {
				ILProcessor il = cctor.Body.GetILProcessor();
				il.Append(il.Create(OpCodes.Ret));
			}
		}
		
		internal void FillConfigTypes(Dictionary<TParamName, object> dict, ModuleDefinition module, TypeDefinition baseType)
		{
			TParamName key;
			TypeDefinition type;
			
			if (baseType != null) {
				FillConfigValues(dict, module, baseType);
			}
			
			foreach(KeyValuePair<TParamName, object> item in dict){
				if (item.Value is Dictionary<TParamName, object>){
					
					key = (TParamName)Convert.ChangeType(item.Key, typeof (TParamName));
					
					if (baseType != null) {
						type = new TypeDefinition(key.ToString(), "", 
					                          Mono.Cecil.TypeAttributes.NestedPublic |
					                          Mono.Cecil.TypeAttributes.BeforeFieldInit,
					                         module.Import(typeof(object)));
		
// Adding link to System.Object constructor							
//							baseType.Constructors.Add(InsertObjectCtor(module));
						
						baseType.NestedTypes.Add(type);
					}
					else {
						// Да се внимава с неймспейса Config защото може би не трябва да го има.
						type = new TypeDefinition(key.ToString(), "Config", 
					                          Mono.Cecil.TypeAttributes.Public |
					                          Mono.Cecil.TypeAttributes.BeforeFieldInit,
					                         module.Import(typeof(object)));
					}
					
					module.Types.Add(type);
					
					FillConfigTypes(item.Value as Dictionary<TParamName, object>, module, type);
				}
			}
		}
		
		private MethodDefinition InsertObjectCtor(ModuleDefinition module)
		{
			TypeReference returnType = new TypeReference("System.Void","", null, false);
						
			MethodDefinition ctor = new MethodDefinition(".ctor", 
	                                              Mono.Cecil.MethodAttributes.Public |
	                                              Mono.Cecil.MethodAttributes.HideBySig |
	                                              Mono.Cecil.MethodAttributes.SpecialName |
	                                              Mono.Cecil.MethodAttributes.RTSpecialName,
		                                          returnType);
			MethodReference objectCtor = module.Import(typeof(object).GetConstructor(new Type[]{}));
			//Mono 0.9 migration: CilWorker cil = ctor.Body.CilWorker;
			ILProcessor cil = ctor.Body.GetILProcessor();
			cil.Append(cil.Create(OpCodes.Ldarg_0));
			cil.Append(cil.Create(OpCodes.Call, objectCtor));
			cil.Append(cil.Create(OpCodes.Ret));
			
			return ctor;
		}
		
		private void AppendToCCtor(MethodDefinition cctor, FieldDefinition field, object currentValue)
		{
			//Mono 0.9 migration: CilWorker cil = cctor.Body.CilWorker;
			ILProcessor cil = cctor.Body.GetILProcessor();
			
			Instruction instr = null; 
			
			//Mono 0.9 migration: switch (field.FieldType.GetOriginalType().FullName) {
			switch (field.FieldType.GetElementType().FullName) {
					case "System.String" : 
						instr = cil.Create(OpCodes.Ldstr, (String)currentValue);
						break;
					
					case "System.Int64" : 
						instr = cil.Create(OpCodes.Ldc_I8, (Int64)currentValue);
						break;
						
					case "System.Int32" : 
						switch ((Int32)currentValue) {
							case -1 : instr = cil.Create(OpCodes.Ldc_I4_M1, -1); break;
							case 0 : instr = cil.Create(OpCodes.Ldc_I4_0); break;
							case 1 : instr = cil.Create(OpCodes.Ldc_I4_1); break;
							case 2 : instr = cil.Create(OpCodes.Ldc_I4_2); break;
							case 3 : instr = cil.Create(OpCodes.Ldc_I4_3); break;
							case 4 : instr = cil.Create(OpCodes.Ldc_I4_4); break;
							case 5 : instr = cil.Create(OpCodes.Ldc_I4_5); break;
							case 6 : instr = cil.Create(OpCodes.Ldc_I4_6); break;
							case 7 : instr = cil.Create(OpCodes.Ldc_I4_7); break;
							case 8 : instr = cil.Create(OpCodes.Ldc_I4_8); break;
							default : instr = cil.Create(OpCodes.Ldc_I4, (Int32)currentValue); break;
						}
					break;
					
					case "System.Single" : 
						instr = cil.Create(OpCodes.Ldc_R4, (Single)currentValue);
						break;
						
					case "System.Double" : 
						instr = cil.Create(OpCodes.Ldc_R8, (Double)currentValue);
						break;
			}
			if (instr != null)
				cil.Append(instr);
			instr = cil.Create(OpCodes.Stsfld, field);
			cil.Append(instr);
		}
	}

	
	//Mono 0.9 migration: 
	/*
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
					                                         Mono.Cecil.TypeAttributes.Public,
					                                         module.Import(typeof(object)));
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
							baseType.Constructors.Add(InsertObjectCtor(module));
						}
					
						AppendToCCtor(cctor, field, item.Value);
					}
				}
				
				//Mono 0.9 migration: if (cctor != null)
				//Mono 0.9 migration: cctor.Body.CilWorker.Append(cctor.Body.CilWorker.Create(OpCodes.Ret));
				if (cctor != null) {
					ILProcessor il = cctor.Body.GetILProcessor();
					il.Append(il.Create(OpCodes.Ret));
				}
			}
			
			internal void FillConfigTypes(Dictionary<TParamName, object> dict, ModuleDefinition module, TypeDefinition baseType)
			{
				TParamName key;
				TypeDefinition type;
				
				if (baseType != null) {
					FillConfigValues(dict, module, baseType);
				}
				
				foreach(KeyValuePair<TParamName, object> item in dict){
					if (item.Value is Dictionary<TParamName, object>){
						
						key = (TParamName)Convert.ChangeType(item.Key, typeof (TParamName));
						
						if (baseType != null) {
							type = new TypeDefinition(key.ToString(), "", 
						                          Mono.Cecil.TypeAttributes.NestedPublic |
						                          Mono.Cecil.TypeAttributes.BeforeFieldInit,
						                         module.Import(typeof(object)));
			
// Adding link to System.Object constructor							
//							baseType.Constructors.Add(InsertObjectCtor(module));
							
							baseType.NestedTypes.Add(type);
						}
						else {
							// Да се внимава с неймспейса Config защото може би не трябва да го има.
							type = new TypeDefinition(key.ToString(), "Config", 
						                          Mono.Cecil.TypeAttributes.Public |
						                          Mono.Cecil.TypeAttributes.BeforeFieldInit,
						                         module.Import(typeof(object)));
						}
						
						module.Types.Add(type);
						
						FillConfigTypes(item.Value as Dictionary<TParamName, object>, module, type);
					}
				}
			}
			
			private MethodDefinition InsertObjectCtor(ModuleDefinition module)
			{
				TypeReference returnType = new TypeReference("System.Void","", null, false);
							
				MethodDefinition ctor = new MethodDefinition(".ctor", 
		                                              Mono.Cecil.MethodAttributes.Public |
		                                              Mono.Cecil.MethodAttributes.HideBySig |
		                                              Mono.Cecil.MethodAttributes.SpecialName |
		                                              Mono.Cecil.MethodAttributes.RTSpecialName,
			                                          returnType);
				MethodReference objectCtor = module.Import(typeof(object).GetConstructor(new Type[]{}));
				//Mono 0.9 migration: CilWorker cil = ctor.Body.CilWorker;
				ILProcessor cil = ctor.Body.GetILProcessor();
				cil.Append(cil.Create(OpCodes.Ldarg_0));
				cil.Append(cil.Create(OpCodes.Call, objectCtor));
				cil.Append(cil.Create(OpCodes.Ret));
				
				return ctor;
			}
			
			private void AppendToCCtor(MethodDefinition cctor, FieldDefinition field, object currentValue)
			{
				//Mono 0.9 migration: CilWorker cil = cctor.Body.CilWorker;
				ILProcessor cil = cctor.Body.GetILProcessor();
				
				Instruction instr = null; 
				
				//Mono 0.9 migration: switch (field.FieldType.GetOriginalType().FullName) {
				switch (field.FieldType.GetElementType().FullName) {
						case "System.String" : 
							instr = cil.Create(OpCodes.Ldstr, (String)currentValue);
							break;
						
						case "System.Int64" : 
							instr = cil.Create(OpCodes.Ldc_I8, (Int64)currentValue);
							break;
							
						case "System.Int32" : 
							switch ((Int32)currentValue) {
								case -1 : instr = cil.Create(OpCodes.Ldc_I4_M1, -1); break;
								case 0 : instr = cil.Create(OpCodes.Ldc_I4_0); break;
								case 1 : instr = cil.Create(OpCodes.Ldc_I4_1); break;
								case 2 : instr = cil.Create(OpCodes.Ldc_I4_2); break;
								case 3 : instr = cil.Create(OpCodes.Ldc_I4_3); break;
								case 4 : instr = cil.Create(OpCodes.Ldc_I4_4); break;
								case 5 : instr = cil.Create(OpCodes.Ldc_I4_5); break;
								case 6 : instr = cil.Create(OpCodes.Ldc_I4_6); break;
								case 7 : instr = cil.Create(OpCodes.Ldc_I4_7); break;
								case 8 : instr = cil.Create(OpCodes.Ldc_I4_8); break;
								default : instr = cil.Create(OpCodes.Ldc_I4, (Int32)currentValue); break;
							}
						break;
						
						case "System.Single" : 
							instr = cil.Create(OpCodes.Ldc_R4, (Single)currentValue);
							break;
							
						case "System.Double" : 
							instr = cil.Create(OpCodes.Ldc_R8, (Double)currentValue);
							break;
				}
				if (instr != null)
					cil.Append(instr);
				instr = cil.Create(OpCodes.Stsfld, field);
				cil.Append(instr);
			}
			
		}
	#endregion
	*/
}
