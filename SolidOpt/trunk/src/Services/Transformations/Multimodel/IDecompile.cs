/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
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
