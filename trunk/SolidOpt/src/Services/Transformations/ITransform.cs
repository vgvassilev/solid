/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 24.6.2009 г.
 * Time: 15:18
 * 
 */
using System;
using System.Collections.Generic;

using SolidOpt.Core.Services;

namespace SolidOpt.Optimizer.Transformers
{
	/// <summary>
	/// Description of ITransform.
	/// </summary>
	public interface ITransform<Source, Target> : IService
	{
		Target Transform(Source source);
	}
	
	public interface ITransform<Target> : ITransform<Target, Target>
	{
	}
}
