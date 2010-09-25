/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Transformations
{
	public class Pipeline<Source, Target> : IPipeline<Source, Target>
		where Source : class
		where Target : class
	{

		List<IStep> steps = new List<IStep>();	
		
		public Pipeline (params IStep[] steps) 
			: this (steps as IEnumerable<IStep>)
		{
		}	
		
		public Pipeline (IEnumerable<IStep> steps)
		{
			this.steps.AddRange(steps);		
		}
		
		#region IPipeline<Source, Target> implementation
		
		public Target Run (Source source)
		{
			//TODO: Change object from the return type of the steps. 
			// Extract common interface between the CodeModels.
				
			object currentCodeModel = source;
				
			foreach (Step step in steps) {
				currentCodeModel = step.Process(currentCodeModel);
			}
			
			return currentCodeModel as Target;
		}
		
		#endregion
	}
}
