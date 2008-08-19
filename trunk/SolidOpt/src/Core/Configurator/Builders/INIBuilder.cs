/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 17.8.2008 ?.
 * Time: 13:40
 * 
 */

using System;

namespace SolidOpt.Core.Configurator.Savers
{
	/// <summary>
	/// Description of BinarySaver.
	/// </summary>
	public class INIBuilder : IConfigSaver
	{
		public INIBuilder()
		{
		}
		
		public bool CanSave()
		{
			return true; //TODO: add logics
		}
		
		public void Save()
		{
			throw new NotImplementedException();
		}
		
		public void SaveAs()
		{
			throw new NotImplementedException();
		}
	}
}
