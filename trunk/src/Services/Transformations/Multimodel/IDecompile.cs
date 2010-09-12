/*
 *
 * User: Vassil Vassilev
 * Date: 02.11.2009 г.
 * Time: 22:38
 * 
 */
using System;
using System.Collections.Generic;

namespace SolidOpt.Services.Transformations.Multimodel
{
	/// <summary>
	/// Description of IDecompile.
	/// </summary>
	public interface IDecompile<Source, Target> : ITransform<Source, Target>
	{
		Target Decompile(Source source);
	}
}
