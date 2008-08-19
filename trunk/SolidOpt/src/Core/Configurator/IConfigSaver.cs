/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 17.8.2008 ?.
 * Time: 13:17
 * 
 */

using System;

namespace SolidOpt.Core.Configurator
{
	/// <summary>
	/// Description of IConfigSaver.
	/// </summary>
	public interface IConfigSaver
	{
		bool CanSave();
		void Save();
		void SaveAs();
	}
}
