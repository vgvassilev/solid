/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

using SolidOpt.Services.Compatibility;
using SolidOpt.Services.Transformations;

namespace SolidOpt.Services.Transformations.Multimodel
{
	public class DecompilationPipeline<Source, Target> : Pipeline<Source, Target>, IDecompile<Source, Target>
		where Source : class
		where Target : class		
	{
		
		//TODO: make enumerator of the subclass. You have to do enumeration type casting
		public DecompilationPipeline (IEnumerable<DecompilationStep> steps) 
				: base (FixEnumerableVariance.Convert<DecompilationStep, IStep>(steps))
		{		
		}
			
		#region IDecompile<Source, Target> implementation

		public Target Decompile (Source source)
		{
			return Run (source);
		}		
		
		#endregion
					
	}
}
