// 
//  Pipeline.cs
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
