/*
 * $Id$
 * It is part of the SolidOpt Copyright Policy (see Copyright.txt)
 * For further details see the nearest License.txt
 */

using System;

using SolidOpt.Core.Services;

namespace TestPluginService
{
	/// <summary>
	/// Description of IAddService.
	/// </summary>
	public interface IAddService : IService
	{
		
		int Add(int a, int b);
		
	}
}
