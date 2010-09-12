/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 24.6.2009 г.
 * Time: 11:37
 * 
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
