// 
//  Main.cs
//  
//  Author:
//       vvassilev <vasil.georgiev.vasilev@cern.ch>
//  
//  Copyright (c) 2010 vvassilev
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.


using System;
using System.Collections.Generic;

using Mono.Cecil;
using Mono.Cecil.Cil;

using SolidOpt.Services.Transformations;
using SolidOpt.Services.Transformations.Multimodel;

using SolidOpt.Services.Transformations.Multimodel.CilToControlFlowGraph;
using SolidOpt.Services.Transformations.CodeModel.ControlFlowGraph;

namespace SolidOpt.Documentation.Samples.Decompiler
{
	class Program
	{
		
		#region Sample Methods
		
		static void Foo (int a)
		{
			Console.WriteLine (a);
			if ((a > 0 && a < 120) && a > 160) {
				a = 42;
			} else {
				a = 24;
			}
			Console.WriteLine (a);
		}
		
		#endregion
		
		public static void Main (string[] args)
		{
			MethodDefinition method = GetProgramMethod ("Foo");
			
			List<DecompilationStep> steps = new List<DecompilationStep>();
			steps.Add(new CilToControlFlowGraph());
			
			DecompilationPipeline<MethodBody, ControlFlowGraph> dpipeline
				= new DecompilationPipeline<MethodBody, ControlFlowGraph>(steps);
			
			ControlFlowGraph cfg = dpipeline.Run(method.Body);
			
			Console.WriteLine(cfg);
			Console.ReadKey(true);
			
		}
		
		static MethodDefinition GetProgramMethod (string name)
		{
			return GetProgramAssembly ().MainModule.GetType ("SolidOpt.Documentation.Samples.Decompiler.Program").Methods[1];
		}
	
		static AssemblyDefinition GetProgramAssembly ()
		{
			var assembly = AssemblyDefinition.ReadAssembly (typeof (Program).Module.FullyQualifiedName);
			return assembly;
		}
		
	}
}
