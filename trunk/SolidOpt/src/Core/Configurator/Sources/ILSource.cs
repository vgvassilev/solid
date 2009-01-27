/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 03.1.2009 г.
 * Time: 12:08
 * 
 */
using System;
using System.IO;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

using SolidOpt.Core.Providers.StreamProvider;

using System.Diagnostics;
using System.Reflection;

namespace SolidOpt.Core.Configurator.Sources
{
	/// <summary>
	/// Generates CIR (Dictionary) from assembly.
	/// </summary>
	public class ILSource<TParamName> : IConfigSource<TParamName>
	{
		internal static Dictionary<TParamName, object> dict = new Dictionary<TParamName, object>();
		
		public ILSource()
		{
			Trace.Listeners.Add(new ConsoleTraceListener());
		}
		
		public bool CanParse(Uri resUri, Stream resStream)
		{
			return (resUri.IsFile && Path.GetExtension(resUri.LocalPath).ToLower() == ".dll");
		}
		
		public Dictionary<TParamName, object> LoadConfiguration(Stream resStream)
		{
			AssemblyDefinition assembly = AssemblyFactory.GetAssembly(resStream);
			assembly.MainModule.Accept(new StructureVisitor<TParamName>());
			
			return dict;
		}
	}
	
	#region Visitors
	internal class StructureVisitor<TParamName>: BaseStructureVisitor
	{
		public override void VisitModuleDefinition(ModuleDefinition module)
		{				
			module.Accept(new ReflectionVisitor<TParamName>());
		}
	}
		
	internal class ReflectionVisitor<TParamName>: BaseReflectionVisitor
	{
		internal bool isFirstLevel = true;
		
		public override void VisitTypeDefinitionCollection(TypeDefinitionCollection types)
		{
			base.VisitCollection(types);
		}
		
		
		public override void VisitTypeDefinition(TypeDefinition type)
		{
			if (!type.IsNestedPublic) {
				if (isFirstLevel && type.Name != "<Module>" && type.Name != "Global") {
					isFirstLevel = false;
					Dictionary<TParamName, object> subDict = new Dictionary<TParamName, object>();
					VisitConfigTypes(type, subDict);
					ILSource<TParamName>.dict[(TParamName)Convert.ChangeType(type.Name, typeof(TParamName))] = subDict;
				}
				else
					VisitConfigTypes(type, ILSource<TParamName>.dict);
			}
		}
		
		internal void VisitConfigTypes(TypeDefinition type, Dictionary<TParamName, object> dict)
		{
			foreach (FieldDefinition field in type.Fields) {
				dict[(TParamName)Convert.ChangeType(
					field.Name, typeof(TParamName))] = GetFieldValue(type.Constructors, field);
			}
			
			foreach (TypeDefinition t in type.NestedTypes) {
				Dictionary<TParamName, object> subDict = new Dictionary<TParamName, object>();
				VisitConfigTypes(t, subDict);
				dict[(TParamName)Convert.ChangeType(t.Name, typeof(TParamName))] = subDict;
			}
		}
		
		internal object GetFieldValue(ConstructorCollection collection, FieldDefinition field)
		{
			foreach (MethodDefinition cctor in collection) {
				if (cctor.IsStatic && cctor.IsSpecialName && cctor.IsHideBySig) {
					foreach (Instruction instr in cctor.Body.Instructions) {
						if (instr.OpCode == OpCodes.Ldstr && instr.Next.OpCode == OpCodes.Stsfld) {
							if ((instr.Next.Operand as FieldDefinition).Name == field.Name)
								return instr.Operand;
						}
					}
				}
			}
			return null;
		}
	}
	#endregion
}