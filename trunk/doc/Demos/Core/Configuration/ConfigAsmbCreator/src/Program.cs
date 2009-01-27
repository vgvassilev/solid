/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 24.12.2008 г.
 * Time: 15:58
 * 
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

using Mono.Cecil;
using Mono.Cecil.Cil;

using SolidOpt.Core.Configurator;
using SolidOpt.Core.Configurator.Parsers;

namespace ConfigAsmbCreator
{
	class Program
	{
		public static ConfigurationManager<string> configurator = new ConfigurationManager<string>();
		public static Dictionary<string, object> testDict;
		public static void Main(string[] args)
		{
			Trace.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());
			configurator.Loaders.Add(new NMSPParser<string>());
			testDict = configurator.LoadConfiguration(new Uri(
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"test.nmsp")));
			
			GenerateAssembly(testDict);
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		public static void GenerateAssembly(Dictionary<string, object> testDict)
		{
			
			AssemblyDefinition assembly =
					AssemblyFactory.DefineAssembly("testAssembly", AssemblyKind.Dll);

			assembly.Name.Name ="Test Assembly";
			assembly.Name.Version = new Version(1,1,0);
			assembly.MainModule.Accept(new StructureVisitor());
			
//			assembly.Modules.Add(new ModuleDefinition("mainM", assembly));
			
//            ModuleDefinition newModule = new ModuleDefinition("testModule", assembly);
//            newModule.Main = true;
//            newModule.Mvid = Guid.NewGuid();
//            assembly.Modules.Add(newModule);

            // Save the assembly and verify the result
            AssemblyFactory.SaveAssembly(assembly, "test.dll");
			
			
			
			
			
			
//			AssemblyDefinition asmb;
//			asmb.Name.Name = "test assembly";
//			
//			TypeDefinition type = new TypeDefinition("string", "testNamespace",TypeAttributes.Public);
////			ImportContext importer = new ImportContext(
//			
//			FieldDefinition field = new FieldDefinition("TestField", new TypeReference("testField","testNamespace"),
//			                                            FieldAttributes.Assembly);
//			asmb.MainModule.Inject(field, type, null);
		}
	}
	
	#region Visitors
	internal class StructureVisitor: BaseStructureVisitor
		{
			public override void VisitAssemblyDefinition(AssemblyDefinition asm)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+asm);
			}
			
			public override void VisitAssemblyNameDefinition(AssemblyNameDefinition name)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+name);
			}
			
			public override void TerminateAssemblyDefinition(AssemblyDefinition asm)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+asm);
			}
			
			public override void VisitAssemblyNameReferenceCollection(AssemblyNameReferenceCollection names)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+names);
				Trace.Indent();
				base.VisitCollection(names);
				Trace.Unindent();
			}
			
			public override void VisitAssemblyNameReference(AssemblyNameReference name)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+name);
			}
			
			public override void VisitResourceCollection(ResourceCollection resources)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+resources);
				Trace.Indent();
				base.VisitCollection(resources);
				Trace.Unindent();
			}
			
			public override void VisitEmbeddedResource(EmbeddedResource res)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+res);
			}
			
			public override void VisitLinkedResource(LinkedResource res)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+res);
			}
			
			public override void VisitAssemblyLinkedResource(AssemblyLinkedResource res)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+res);
			}
			
			public override void VisitModuleDefinitionCollection(ModuleDefinitionCollection modules)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+modules);
				Trace.Indent();
				base.VisitCollection(modules);
				Trace.Unindent();
			}
			
			public override void VisitModuleDefinition(ModuleDefinition module)
			{
				
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+module);
				if (Program.testDict.Count > 0){
					TypeDefinition type = new TypeDefinition("AppConfiguration","Config",
				                                         Mono.Cecil.TypeAttributes.Public, null);
					module.Types.Add(type);
					ViewIR(Program.testDict, module, type);
					
				}
				
				Trace.Indent();
				module.Accept(new ReflectionVisitor());
				Trace.Unindent();
			}
			
			internal void ViewIR(Dictionary<string, object> dict, ModuleDefinition module, TypeDefinition baseType)
			{
				string key;
				TypeDefinition type;
				FieldDefinition field;
				MethodDefinition cctor = null;
				
				foreach(KeyValuePair<string, object> item in dict){
					key = (string)Convert.ChangeType(item.Key, typeof (string));
					if (item.Value is Dictionary<string, object>){
						
						type = new TypeDefinition(key, "Config", Mono.Cecil.TypeAttributes.NestedPublic, null);
						module.Types.Add(type);
						baseType.NestedTypes.Add(type);
						
						ViewIR(item.Value as Dictionary<string, object>, module, type);
					}
					else {
						field = new FieldDefinition(key, module.Import(item.Value.GetType()),
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
			
			internal void AppendToCCtor(MethodDefinition cctor, FieldDefinition field, object currentValue)
			{
				CilWorker cil = cctor.Body.CilWorker;
				
				Instruction instr = cil.Create(OpCodes.Ldstr, (string)currentValue);
				cil.Append(instr);
				instr = cil.Create(OpCodes.Stsfld, field);
				cil.Append(instr);
			}
			
			public override void VisitModuleReferenceCollection(ModuleReferenceCollection modules)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+modules);
				Trace.Indent();
				base.VisitCollection(modules);
				Trace.Unindent();
			}
			
			public override void VisitModuleReference(ModuleReference module)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+": "+module);
			}
		}
		
		internal class ReflectionVisitor: BaseReflectionVisitor
		{
			
			public override void VisitModuleDefinition(ModuleDefinition module)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+module);
			}
			
			public override void TerminateModuleDefinition(ModuleDefinition module)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+module);
			}
			
			public override void VisitTypeDefinitionCollection(TypeDefinitionCollection types)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+types);
				Trace.Indent();
				base.VisitCollection(types);
				Trace.Unindent();
			}
			
			public override void VisitTypeDefinition(TypeDefinition type)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+type);
			}
			
			public override void VisitTypeReferenceCollection(TypeReferenceCollection refs)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+refs);
				Trace.Indent();
				base.VisitCollection(refs);
				Trace.Unindent();
			}
			
			public override void VisitTypeReference(TypeReference type)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+type);
			}
			
			public override void VisitMemberReferenceCollection(MemberReferenceCollection members)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+members);
				Trace.Indent();
				base.VisitCollection(members);
				Trace.Unindent();
			}
			
			public override void VisitMemberReference(MemberReference member)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+member);
			}
			
			public override void VisitInterfaceCollection(InterfaceCollection interfaces)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+interfaces);
				Trace.Indent();
				base.VisitCollection(interfaces);
				Trace.Unindent();
			}
			
			public override void VisitInterface(TypeReference interf)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+interf);
			}
			
			public override void VisitExternTypeCollection(ExternTypeCollection externs)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+externs);
				Trace.Indent();
				base.VisitCollection(externs);
				Trace.Unindent();
			}
			
			public override void VisitExternType(TypeReference externType)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+externType);
			}
			
			public override void VisitOverrideCollection(OverrideCollection meth)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+meth);
				Trace.Indent();
				base.VisitCollection(meth);
				Trace.Unindent();
			}
			
			public override void VisitOverride(MethodReference ov)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+ov);
			}
			
			public override void VisitNestedTypeCollection(NestedTypeCollection nestedTypes)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+nestedTypes);
				Trace.Indent();
				base.VisitCollection(nestedTypes);
				Trace.Unindent();
			}
			
			public override void VisitNestedType(TypeDefinition nestedType)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+nestedType);
			}
			
			public override void VisitParameterDefinitionCollection(ParameterDefinitionCollection parameters)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+parameters);
				Trace.Indent();
				base.VisitCollection(parameters);
				Trace.Unindent();
			}
			
			public override void VisitParameterDefinition(ParameterDefinition parameter)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+parameter);
			}
			
			public override void VisitMethodDefinitionCollection(MethodDefinitionCollection methods)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+methods);
				Trace.Indent();
				base.VisitCollection(methods);
				Trace.Unindent();
			}
			
			public override void VisitMethodDefinition(MethodDefinition method)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+method);
			}
			
			public override void VisitConstructorCollection(ConstructorCollection ctors)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+ctors);
				Trace.Indent();
				base.VisitCollection(ctors);
				
//				TypeReference returnType = new TypeReference("System.Void","", null, false);
//				
//				MethodDefinition cctor = new MethodDefinition(".cctor", 
//				                                              Mono.Cecil.MethodAttributes.Private |
//				                                              Mono.Cecil.MethodAttributes.HideBySig |
//				                                              Mono.Cecil.MethodAttributes.SpecialName |
//				                                              Mono.Cecil.MethodAttributes.RTSpecialName |
//				                                              Mono.Cecil.MethodAttributes.Static,
//					                                          returnType);
//				ctors.Add(cctor);
//
//				CilWorker cil = cctor.Body.CilWorker;
//				
//				Instruction instr = cil.Create(OpCodes.Ldstr, (string)currentValue);
//				cil.Append(instr);
//				instr = cil.Create(OpCodes.Stsfld, fld);
//				cil.Append(instr);
				
				
				
//				Trace.WriteLine(cctor.DeclaringType.FullName +":> ..."+cctor);
//					ctor.DeclaringType.FullName
//				Instruction ldstr = cil.Create(OpCodes.Ldstr, "Hello Cecil World!");
//				cil.Append(ldstr);
				
				
				
				
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
				
				Trace.Unindent();
			}
			
			public override void VisitConstructor(MethodDefinition ctor)
			{
			}
			
			public override void VisitPInvokeInfo(PInvokeInfo pinvk)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+pinvk);
			}
			
			public override void VisitEventDefinitionCollection(EventDefinitionCollection events)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+events);
				Trace.Indent();
				base.VisitCollection(events);
				Trace.Unindent();
			}
			
			public override void VisitEventDefinition(EventDefinition evt)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+evt);
			}
			
			public override void VisitFieldDefinitionCollection(FieldDefinitionCollection fields)
			{
//				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+fields);
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+fields);
				
				
				
				
				
				Trace.Indent();
				base.VisitCollection(fields);
				
				foreach (FieldDefinition fdef in fields){
					Trace.WriteLine(fdef.Name);
				}
				Trace.Unindent();
			}
			
			public override void VisitFieldDefinition(FieldDefinition field)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+field);
			}
			
			public override void VisitPropertyDefinitionCollection(PropertyDefinitionCollection properties)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+properties);
				Trace.Indent();
				base.VisitCollection(properties);
				Trace.Unindent();
			}
			
			public override void VisitPropertyDefinition(PropertyDefinition property)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+property);
			}
			
			public override void VisitSecurityDeclarationCollection(SecurityDeclarationCollection secDecls)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+secDecls);
//				Trace.Indent();
//				base.VisitCollection(secDecls);
//				Trace.Unindent();
			}
			
			public override void VisitSecurityDeclaration(SecurityDeclaration secDecl)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+secDecl);
			}
			
			public override void VisitCustomAttributeCollection(CustomAttributeCollection customAttrs)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+customAttrs);
				Trace.Indent();
				base.VisitCollection(customAttrs);
				Trace.Unindent();
			}
			
			public override void VisitCustomAttribute(CustomAttribute customAttr)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+customAttr);
			}			
			public override void VisitGenericParameterCollection(GenericParameterCollection genparams)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+genparams);
				Trace.Indent();
				base.VisitCollection(genparams);
				Trace.Unindent();
			}
			
			public override void VisitGenericParameter(GenericParameter genparam)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+genparam);
			}
			
			public override void VisitMarshalSpec(MarshalSpec marshalSpec)
			{
				Trace.WriteLine(MethodBase.GetCurrentMethod().Name+":> "+marshalSpec);
			}
		}
	#endregion
	
}
