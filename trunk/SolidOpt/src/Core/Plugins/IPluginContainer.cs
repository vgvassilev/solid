/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 31.7.2008 ?.
 * Time: 15:45
 * 
 */

using System;
using System.Runtime.InteropServices;

namespace SolidOpt.Core.Plugins
{
	/// <summary>
	/// Base interface of the plugin container.
	/// </summary>
	[GuidAttribute("0E3A458B-97E0-4038-B95F-649B8B95A268")]
	public interface IPluginContainer
	{
		void AddPlugin(IPlugin plugin);
		void RemovePlugin(IPlugin plugin);
	}
}
