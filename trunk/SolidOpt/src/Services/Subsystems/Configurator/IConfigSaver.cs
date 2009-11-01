/*
 * Created by SharpDevelop.
 * User: Vassil Vassilev
 * Date: 17.8.2008 ?.
 * Time: 13:17
 * 
 */

using System;

namespace SolidOpt.Services.Subsystems.Configurator
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
