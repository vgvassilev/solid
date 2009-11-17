/*
 *
 * User: Vassil Vassilev
 * Date: 02.11.2009 г.
 * Time: 22:34
 * 
 */
using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Transformations.Optimizations
{
	/// <summary>
	/// Description of IOptimize.
	/// </summary>
	public interface IOptimize<Target> : ITransform<Target, Target>
	{
		Target Optimize(Target source);
	}
}
