using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
//using System.Runtime.Remoting;

using Mono.Cecil.Cil;
using Mono.Cecil;

using SolidOpt.Core.Services;
using SolidOpt.Optimizer.Transformers;

namespace SolidOpt.Core.Loader
{
	public class Loader
	{
		private PluginServiceContainer servicesContainer = new PluginServiceContainer();
		public PluginServiceContainer ServicesContainer {
			get { return servicesContainer; }
			set { servicesContainer = value; }
		}
		
		public Loader()
		{
			Trace.WriteLine("Initialize...");
		}

		public int Run(string[] args)
		{
			if (args.Length < 1) return -1;
			
			foreach(string s in args) Console.WriteLine(s);
			Trace.WriteLine("Search plugins...");
			
			ServicesContainer.AddPlugins(AppDomain.CurrentDomain.BaseDirectory + "core");
			ServicesContainer.AddPlugins(AppDomain.CurrentDomain.BaseDirectory + "plugins");
						
			foreach(PluginInfo p in ServicesContainer.plugins)
				 Trace.WriteLine(p.fullName);
			
			Trace.WriteLine("Load plugins...");
			ServicesContainer.LoadPlugins();
			
			Trace.WriteLine("Configure plugins...");
			Optimize(args);
			
			return 0;
		}
		
		internal void Optimize(string[] args)
		{
			//IService[] transformers = (IService[]) ServicesContainer.GetServices(typeof(ITransform<MethodDefinition>));
			List<ITransform<MethodDefinition>> transformers = ServicesContainer.GetServices<ITransform<MethodDefinition>>();
			Trace.WriteLine("Transformers.Count: " + transformers.Count);
			
			AssemblyDefinition assembly = AssemblyFactory.GetAssembly(args[0]);
			assembly.MainModule.Accept(new StructureVisitor(transformers));
			AssemblyFactory.SaveAssembly(assembly, Path.ChangeExtension(args[0], ".modified" + Path.GetExtension(args[0])));
		}
		
		internal class StructureVisitor: BaseStructureVisitor
		{
			private List<ITransform<MethodDefinition>> transformers;
			
			public StructureVisitor(List<ITransform<MethodDefinition>> transformers) : base()
			{
				this.transformers = transformers;
			}
			
			public override void VisitModuleDefinition(ModuleDefinition module)
			{
				module.Accept(new ReflectionVisitor(transformers));
			}
		}
	
		internal class ReflectionVisitor: BaseReflectionVisitor
		{
			private List<ITransform<MethodDefinition>> transformers;
			
			public ReflectionVisitor(List<ITransform<MethodDefinition>> transformers) : base()
			{
				this.transformers = transformers;
			}
			
			public override void VisitTypeDefinitionCollection(TypeDefinitionCollection types)
			{
				base.VisitTypeDefinitionCollection(types);
				foreach (TypeDefinition type in types)
					type.Accept(this);
			}
			
			public override void VisitTypeDefinition(TypeDefinition type)
			{
				base.VisitTypeDefinition(type);
			}
			
			public override void VisitMethodDefinitionCollection(MethodDefinitionCollection methods)
			{
				base.VisitMethodDefinitionCollection(methods);
				foreach (MethodDefinition method in methods)
					method.Accept(this);
			}
			
			
			public override void VisitMethodDefinition(MethodDefinition method)
			{
				base.VisitMethodDefinition(method);
				Trace.WriteLine(method.Name);
				
				foreach (ITransform<MethodDefinition> transformer in transformers) {
//					if (RemotingServices.IsTransparentProxy(transformer))
//						Trace.WriteLine("Proxy");
//					else
//						Trace.WriteLine("NoProxy");
					method = transformer.Transform(method);
				}
					
			}		
		}
		
	}
}
