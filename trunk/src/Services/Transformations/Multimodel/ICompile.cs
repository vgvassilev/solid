/*
 *
 * User: Vassil Vassilev
 * Date: 02.11.2009 г.
 * Time: 22:37
 * 
 */
using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Transformations.Multimodel
{
	/// <summary>
	/// Description of ICompile.
	/// </summary>
	public interface ICompile<Source, Target> : ITransform<Source, Target>
	{
		Target Compile(Source source);
	}
}
