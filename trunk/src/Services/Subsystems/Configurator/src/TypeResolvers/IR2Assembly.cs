/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using System.Reflection;
	
using System.Diagnostics;

using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;
using Mono.Cecil.Metadata;

namespace SolidOpt.Services.Subsystems.Configurator
{
	/// <summary>
	/// Description of IR2Assembly.
	/// </summary>
	public class IR2Assembly<TParamName> : ITypeResolver
	{
		private ConfigurationManager<TParamName> configurator = ConfigurationManager<TParamName>.Instance;
		public IR2Assembly()
		{
			
		}
		
		public bool CanBuild(string fileFormat)
		{
			return fileFormat == "dll";
		}
		
		public void Build(Dictionary<TParamName, object> configRepresenation)
		{
			Build(configRepresenation, @"D:\FMI\Diplomna\SolidOpt\trunk\Documentation\Demos\Core\Configuration\ConfigDemo\bin\Debug\Config3.dll",
			         @"D:\FMI\Diplomna\SolidOpt\trunk\Documentation\Demos\Core\Configuration\ConfigDemo\bin\Debug\Config3.modified.dll");
			         
		}
		
		public void Build(Dictionary<TParamName, object> configRepresenation, string input, string output)
		{
			Optimize(input, output);
		}
		
		public bool CanParse(Uri resource)
		{
			return false;
		}
		
		public Dictionary<TParamName, object> LoadConfiguration(Uri resourse)
		{
			throw new NotImplementedException();
		}
		
		public object TryResolve(object paramValue)
		{
			throw new NotImplementedException();
		}
		
		public void Optimize(string inputFileName, string outputFileName)
		{
			Trace.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());
			
			AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(inputFileName);
			
			Trace.Indent();
			
			Trace.WriteLine("Modules:");
			Trace.Indent();
			foreach (ModuleDefinition m in assembly.Modules) {
				Trace.WriteLine(m.Name+((m.IsMain)?" (main)":""));
//				Trace.Indent();

//				Trace.WriteLine("AssemblyRefs:");
//				Trace.Indent();
//				foreach (AssemblyNameReference ar in m.AssemblyReferences) {
//					Trace.WriteLine(ar);
//					Trace.Indent();
//					
//					//Trace.WriteLine("AssemblyRefs:");
//					Trace.Indent();
//					//foreach (AssemblyNameReference ar in ar..AssemblyReferences) {
//					Trace.Unindent();
//					
//					Trace.Unindent();
//				}
//				Trace.Unindent();
				
				Trace.WriteLine("ModuleRefs:");
				Trace.Indent();
				foreach (ModuleReference mr in m.ModuleReferences) {
					Trace.WriteLine(mr.Name);
				}
				Trace.Unindent();
				
//				Trace.WriteLine("ExternTypeRefs:");
//				Trace.Indent();
//				foreach (TypeReference et in m.ExternTypes) {
//					Trace.WriteLine(et);
//				}
//				Trace.Unindent();
				
//				Trace.WriteLine("MemberRefs:");
//				Trace.Indent();
//				foreach (MemberReference mer in m.MemberReferences) {
//					Trace.WriteLine(mer);
//				}
//				Trace.Unindent();
				
//				Trace.WriteLine("TypeRefs:");
//				Trace.Indent();
//				foreach (TypeReference tyr in m.TypeReferences) {
//					Trace.WriteLine(tyr);
//				}
//				Trace.Unindent();
				
				Trace.Unindent();
			}
			Trace.Unindent();
			
			
//			TypeReference tr = assembly.MainModule.Import(typeof(IgnoreOptimizationAttribute));
//
//			IgnoreOptimizationCA = new CustomAttribute(typeof(IgnoreOptimizationAttribute));
//			Console.WriteLine(IgnoreOptimizationCA);

			//IgnoreOptimizationCA = assembly.CustomAttributes.Contains(
			
		//	assembly.Accept(new StructureVisitor()); //OptimizeAssembly(assembly);
			Trace.Unindent();
			
			
			assembly.Name.Name = "Config3.modified1";
			
			
			assembly.Write(outputFileName);
		}
	}
	//TODO:
	#region Visitors
//	internal class StructureVisitor: BaseStructureVisitor
//		{
//		
//			public override void VisitAssemblyDefinition(AssemblyDefinition asm)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+asm);
//			}
//			
//			public override void VisitAssemblyNameDefinition(AssemblyNameDefinition name)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+name);
//			}
//			
//			public override void TerminateAssemblyDefinition(AssemblyDefinition asm)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+asm);
//			}
//			
//			public override void VisitAssemblyNameReferenceCollection(AssemblyNameReferenceCollection names)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+names);
//				Trace.Indent();
//				base.VisitCollection(names);
//				Trace.Unindent();
//			}
//			
//			public override void VisitAssemblyNameReference(AssemblyNameReference name)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+name);
//			}
//			
//			public override void VisitResourceCollection(ResourceCollection resources)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+resources);
//				Trace.Indent();
//				base.VisitCollection(resources);
//				Trace.Unindent();
//			}
//			
//			public override void VisitEmbeddedResource(EmbeddedResource res)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+res);
//			}
//			
//			public override void VisitLinkedResource(LinkedResource res)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+res);
//			}
//			
//			public override void VisitAssemblyLinkedResource(AssemblyLinkedResource res)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+res);
//			}
//			
//			public override void VisitModuleDefinitionCollection(ModuleDefinitionCollection modules)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+modules);
//				Trace.Indent();
//				base.VisitCollection(modules);
//				Trace.Unindent();
//			}
//			
//			public override void VisitModuleDefinition(ModuleDefinition module)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+module);
//				Trace.Indent();
//				module.Accept(new ReflectionVisitor());
//				Trace.Unindent();
//			}
//			
//			public override void VisitModuleReferenceCollection(ModuleReferenceCollection modules)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+modules);
//				Trace.Indent();
//				base.VisitCollection(modules);
//				Trace.Unindent();
//			}
//			
//			public override void VisitModuleReference(ModuleReference module)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+module);
//			}
//		}
//		
//		internal class ReflectionVisitor: BaseReflectionVisitor
//		{
//			
//			public override void VisitModuleDefinition(ModuleDefinition module)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+module);
//			}
//			
//			public override void TerminateModuleDefinition(ModuleDefinition module)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+module);
//			}
//			
//			public override void VisitTypeDefinitionCollection(TypeDefinitionCollection types)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+types);
//				Trace.Indent();
//				base.VisitCollection(types);
//				Trace.Unindent();
//			}
//			
//			public override void VisitTypeDefinition(TypeDefinition type)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+type);
//			}
//			
//			public override void VisitTypeReferenceCollection(TypeReferenceCollection refs)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+refs);
//				Trace.Indent();
//				base.VisitCollection(refs);
//				Trace.Unindent();
//			}
//			
//			public override void VisitTypeReference(TypeReference type)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+type);
//			}
//			
//			public override void VisitMemberReferenceCollection(MemberReferenceCollection members)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+members);
//				Trace.Indent();
//				base.VisitCollection(members);
//				Trace.Unindent();
//			}
//			
//			public override void VisitMemberReference(MemberReference member)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+member);
//			}
//			
//			public override void VisitInterfaceCollection(InterfaceCollection interfaces)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+interfaces);
//				Trace.Indent();
//				base.VisitCollection(interfaces);
//				Trace.Unindent();
//			}
//			
//			public override void VisitInterface(TypeReference interf)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+interf);
//			}
//			
//			public override void VisitExternTypeCollection(ExternTypeCollection externs)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+externs);
//				Trace.Indent();
//				base.VisitCollection(externs);
//				Trace.Unindent();
//			}
//			
//			public override void VisitExternType(TypeReference externType)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+externType);
//			}
//			
//			public override void VisitOverrideCollection(OverrideCollection meth)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+meth);
//				Trace.Indent();
//				base.VisitCollection(meth);
//				Trace.Unindent();
//			}
//			
//			public override void VisitOverride(MethodReference ov)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+ov);
//			}
//			
//			public override void VisitNestedTypeCollection(NestedTypeCollection nestedTypes)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+nestedTypes);
//				Trace.Indent();
//				base.VisitCollection(nestedTypes);
//				Trace.Unindent();
//			}
//			
//			public override void VisitNestedType(TypeDefinition nestedType)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+nestedType);
//			}
//			
//			public override void VisitParameterDefinitionCollection(ParameterDefinitionCollection parameters)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+parameters);
//				Trace.Indent();
//				base.VisitCollection(parameters);
//				Trace.Unindent();
//			}
//			
//			public override void VisitParameterDefinition(ParameterDefinition parameter)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+parameter);
//			}
//			
//			public override void VisitMethodDefinitionCollection(MethodDefinitionCollection methods)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+methods);
//				Trace.Indent();
//				base.VisitCollection(methods);
//				Trace.Unindent();
//			}
//			
//			public override void VisitMethodDefinition(MethodDefinition method)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+method);
//			}
//			
//			public override void VisitConstructorCollection(ConstructorCollection ctors)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+ctors);
//				Trace.Indent();
//				base.VisitCollection(ctors);
//				
//				
//				foreach (MethodDefinition ctor in ctors){
//					if (ctor.IsStatic){
//						CilWorker cil = ctor.Body.CilWorker;
//						Trace.WriteLine(MethodBase.GetCurrentMethod().Name+"!!! "+ctor);
//						Instruction ldstr = cil.Create(OpCodes.Ldstr, "Hello Cecil World!");
//						Instruction remInstr = null;
//						foreach (Instruction instr in ctor.Body.Instructions){
//							if (instr.OpCode == OpCodes.Ldstr){
//								remInstr = instr;
//							}
//						}
//						if (remInstr != null){
//							cil.InsertBefore(remInstr, ldstr);
//							cil.Remove(remInstr);
//						}
//				}
//				}
//				
//				Trace.Unindent();
//			}
//			
//			public override void VisitConstructor(MethodDefinition ctor)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+ctor);
//			}
//			
//			public override void VisitPInvokeInfo(PInvokeInfo pinvk)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+pinvk);
//			}
//			
//			public override void VisitEventDefinitionCollection(EventDefinitionCollection events)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+events);
//				Trace.Indent();
//				base.VisitCollection(events);
//				Trace.Unindent();
//			}
//			
//			public override void VisitEventDefinition(EventDefinition evt)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+evt);
//			}
//			
//			public override void VisitFieldDefinitionCollection(FieldDefinitionCollection fields)
//			{
////				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+fields);
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+fields);
//				
//				
//				
//				
//				
//				Trace.Indent();
//				base.VisitCollection(fields);
//				
//				foreach (FieldDefinition fdef in fields){
//					Trace.WriteLine(fdef.Name);
//				}
//				Trace.Unindent();
//			}
//			
//			public override void VisitFieldDefinition(FieldDefinition field)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+field);
//			}
//			
//			public override void VisitPropertyDefinitionCollection(PropertyDefinitionCollection properties)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+properties);
//				Trace.Indent();
//				base.VisitCollection(properties);
//				Trace.Unindent();
//			}
//			
//			public override void VisitPropertyDefinition(PropertyDefinition property)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+property);
//			}
//			
//			public override void VisitSecurityDeclarationCollection(SecurityDeclarationCollection secDecls)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+secDecls);
////				Trace.Indent();
////				base.VisitCollection(secDecls);
////				Trace.Unindent();
//			}
//			
//			public override void VisitSecurityDeclaration(SecurityDeclaration secDecl)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+secDecl);
//			}
//			
//			public override void VisitCustomAttributeCollection(CustomAttributeCollection customAttrs)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+customAttrs);
//				Trace.Indent();
//				base.VisitCollection(customAttrs);
//				Trace.Unindent();
//			}
//			
//			public override void VisitCustomAttribute(CustomAttribute customAttr)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+customAttr);
//			}
//			
//			public override void VisitGenericParameterCollection(GenericParameterCollection genparams)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+genparams);
//				Trace.Indent();
//				base.VisitCollection(genparams);
//				Trace.Unindent();
//			}
//			
//			public override void VisitGenericParameter(GenericParameter genparam)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+genparam);
//			}
//			
//			public override void VisitMarshalSpec(MarshalSpec marshalSpec)
//			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+marshalSpec);
//			}
//		}
	#endregion
	
}
