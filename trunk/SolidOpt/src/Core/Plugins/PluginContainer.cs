/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 31.7.2008 ?.
 * Time: 15:43
 * 
 */

using System;

namespace SolidOpt.Core.Plugins
{
	/// <summary>
	/// Description of PluginContainer.
	/// </summary>
	public class PluginContainer : IPluginContainer
	{
		public PluginContainer()
		{
		}
		
		void IPluginContainer.AddPlugin(IPlugin plugin)
		{
			throw new NotImplementedException();
		}
		
		void IPluginContainer.RemovePlugin(IPlugin plugin)
		{
			throw new NotImplementedException();
		}
	}
}
